using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.WS;
using System.Collections;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Web;
using System.Runtime.Serialization;
using System.ServiceModel;
using Claro.SISACT.Common;
using System.Configuration;
using System.Xml.Serialization;

//PROY-SMS PORTABILIDAD
namespace Claro.SISACT.WS.RestServices
{
    public class RestServiceSMSPorta
    {
        private static WebHeaderCollection GetHeaders(Hashtable table,  string usuario, string ipServidor)
        {
            GeneradorLog _objLog = null;
            _objLog = new GeneradorLog("Metodo GetHeaders", null, null, "log_ProySMSPortabilidad");

            WebHeaderCollection Headers = new WebHeaderCollection();

            string codigoResultado = string.Empty;
            string mensajeResultado = string.Empty;
            string usuarioAplicacion = string.Empty;
            string clave = string.Empty;

            foreach (DictionaryEntry entry in table)
            {
                //INC-SMS_PORTA-INI
                Headers.Add(Funciones.CheckStr(entry.Key), Funciones.CheckStr(entry.Value) != null ? Funciones.CheckStr(entry.Value) : null);
                _objLog.CrearArchivolog(String.Format("{0} --> {1} = {2}","Headers",Funciones.CheckStr(entry.Key),Funciones.CheckStr(entry.Value)), null, null);
                //INC-SMS_PORTA-FIN
            }

            _objLog.CrearArchivolog("INICIO SERVICIO DESENCRIPTACION  --> BWConsultaClaves.ConsultaClaveWS", null, null);

            string ipTransaccion = DateTime.Now.ToString("YYYYMMDDHHMISSMS");
            string wsIp = string.IsNullOrEmpty(Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_SMSPorta_wsIp"])) ? ipServidor : Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_SMSPorta_wsIp"]);
            string usrAplicacion = usuario;
            string codigoAplicacion = ConfigurationManager.AppSettings["CodigoAplicacion"];
            string idAplicacion = ConfigurationManager.AppSettings["system_ConsultaClave"];
            string usuarioAplicacionEncriptado = ConfigurationManager.AppSettings["User_ConsultaNacionalidad"];
            string claveEncriptada = ConfigurationManager.AppSettings["Password_ConsultaNacionalidad"];
            //INC-SMS_PORTA-INI
            _objLog.CrearArchivolog(String.Format("{0} --> {1}","wsIp",wsIp), string.Empty, null);
            _objLog.CrearArchivolog(String.Format("{0} --> {1}", "ipServidor", ipServidor), string.Empty, null);
            _objLog.CrearArchivolog(String.Format("{0} --> {1}", "usrAplicacion", usrAplicacion), string.Empty, null);
            _objLog.CrearArchivolog(String.Format("{0} --> {1}", "codigoAplicacion", codigoAplicacion), string.Empty, null);
            _objLog.CrearArchivolog(String.Format("{0} --> {1}", "idAplicacion", idAplicacion), string.Empty, null);
            _objLog.CrearArchivolog(String.Format("{0} --> {1}", "ipTransaccion", ipTransaccion), string.Empty, null);
            _objLog.CrearArchivolog(String.Format("{0} --> {1}", "usuarioAplicacionEncriptado", usuarioAplicacionEncriptado), string.Empty, null);
            _objLog.CrearArchivolog(String.Format("{0} --> {1}", "claveEncriptada", claveEncriptada), string.Empty, null);
            //INC-SMS_PORTA-FIN
            codigoResultado = new BWConsultaClaves().ConsultaClavesWS(ipTransaccion,
                                                                 ipServidor,
                                                                 ipTransaccion,
                                                                 usrAplicacion,
                                                                 codigoAplicacion,
                                                                 idAplicacion,
                                                                 usuarioAplicacionEncriptado,
                                                                 claveEncriptada,
                                                                 out mensajeResultado,
                                                                 out usuarioAplicacion,
                                                                 out clave
                                                                 );
            //INC-SMS_PORTA-INI
            _objLog.CrearArchivolog(String.Format("{0} --> {1}","Codigo Resultado",codigoResultado), string.Empty, null);
            _objLog.CrearArchivolog(String.Format("{0} --> {1}", "Mensaje Resultado ConsultaClaves", mensajeResultado), string.Empty, null);
            _objLog.CrearArchivolog(String.Format("{0} --> {1}", "Usuario", usuarioAplicacion), string.Empty, null);
            _objLog.CrearArchivolog(String.Format("{0} --> {1}", "Clave", clave), string.Empty, null);
            //INC-SMS_PORTA-FIN
            string strEncryptedBase64;

            if (codigoResultado == "0")
            {
                strEncryptedBase64 = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", usuarioAplicacion, clave)));

                if (strEncryptedBase64 != "1" && strEncryptedBase64 != "-1" && strEncryptedBase64 != "-2" && strEncryptedBase64 != "-3")
                {
                   Headers.Add("Authorization", "Basic " + strEncryptedBase64);
                }
            }

            return Headers;
        }


        public static T PostInvoque<T>(string name, Hashtable objHeader, object obj,  string usuario, string ipServidor)
        {
            GeneradorLog objLog = new GeneradorLog(null, "Inicio metodo  PostInvoque", null, "log_ProySMSPortabilidad");
            objLog.CrearArchivolog("Inicio metodo PostInvoque ", null, null);
            HttpWebRequest request = HttpWebRequest.Create(ConfigurationManager.AppSettings[name]) as HttpWebRequest; //INC-SMS_PORTA
            request.Method = "POST";
            request.Headers = GetHeaders(objHeader, usuario,ipServidor);
            request.Accept = "application/json";

            string data = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            objLog.CrearArchivolog(String.Format("{0} --> {1}", "PROY-SMS PORTABILIDAD Cadena data", data.ToString()), string.Empty, null); //INC-SMS_PORTA
            byte[] byteArray = Encoding.UTF8.GetBytes(data);
            request.Timeout = Funciones.CheckInt(ConfigurationManager.AppSettings["strSMSPortaTimeOut"]); //INC-SMS_PORTA
            objLog.CrearArchivolog(String.Format("{0} : {1}", "Valor de TimeOut strSMSPortaTimeOut", request.Timeout), string.Empty, null); //INC-SMS_PORTA
            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse ws = request.GetResponse();

            using (Stream stream = ws.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                String responseString = reader.ReadToEnd();
                T result = JsonConvert.DeserializeObject<T>(responseString);
                objLog.CrearArchivolog("Fin metodo PostInvoque ", null, null); //INC-SMS_PORTA
                return result;
            }
        }
    }
}