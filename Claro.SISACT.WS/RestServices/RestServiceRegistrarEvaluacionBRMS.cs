//INI PROY-140579 NN
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Collections;
using Claro.SISACT.Common;
using System.Configuration;
using System.Web.Script.Serialization;
using System.IO;
using Newtonsoft.Json;
using Claro.SISACT.Entity.RegistrarEvaluacionBRMSDP.Request;
using Claro.SISACT.Entity.RegistrarEvaluacionBRMSDP.Response;

namespace Claro.SISACT.WS.RestServices
{
    public static class RestServiceRegistrarEvaluacionBRMS
    {
        private static WebHeaderCollection GetHeaders(Hashtable table )
        {
            GeneradorLog _objLog = null;
            _objLog = new GeneradorLog("Metodo GetHeaders", null, null, "log_RestServiceRegistrarEvaluacionBRMS");

            WebHeaderCollection Headers = new WebHeaderCollection();
            string codigoResultado = string.Empty;
            string mensajeResultado = string.Empty;
            string usuarioAplicacion = string.Empty;
            string clave = string.Empty;

            foreach (DictionaryEntry entry in table)
            {
                Headers.Add(entry.Key.ToString(), entry.Value != null ? entry.Value.ToString() : null);
                _objLog.CrearArchivolog("Headers  --> " + entry.Key.ToString() + " = " + entry.Value.ToString(), null, null);
            }

            _objLog.CrearArchivolog("INICIO SERVICIO DESENCRIPTACION  --> BWConsultaClaves.ConsultaClaveWS", null, null);

            string ipTransaccion = DateTime.Now.ToString("YYYYMMDDHHMISSMS"); // BIEN
            string ipServidor =  string.Empty;
            string wsIp = string.IsNullOrEmpty(Funciones.CheckStr(ConfigurationManager.AppSettings["BRMSwip"])) ? ipServidor : Funciones.CheckStr(ConfigurationManager.AppSettings["BRMSwip"]);
            string usrAplicacion = ConfigurationManager.AppSettings["BRMS_Aplicativo"];
            string codigoAplicacion = ConfigurationManager.AppSettings["BRMS_CodAplicativo"];
            string idAplicacion = ConfigurationManager.AppSettings["BRMS_Aplicativo"];
            string usuarioAplicacionEncriptado = ConfigurationManager.AppSettings["User_BRMSService"];
            string claveEncriptada = ConfigurationManager.AppSettings["Password_BRMSService"];

            _objLog.CrearArchivolog("wsIp  --> " + wsIp, "", null);
            _objLog.CrearArchivolog("ipServidor  --> " + ipServidor, "", null);
            _objLog.CrearArchivolog("usrAplicacion  --> " + usrAplicacion, "", null);
            _objLog.CrearArchivolog("codigoAplicacion  --> " + codigoAplicacion, "", null);
            _objLog.CrearArchivolog("idAplicacion  --> " + idAplicacion, "", null);
            _objLog.CrearArchivolog("ipTransaccion  --> " + ipTransaccion, "", null);
            _objLog.CrearArchivolog("usuarioAplicacionEncriptado  --> " + usuarioAplicacionEncriptado, "", null);
            _objLog.CrearArchivolog("claveEncriptada  --> " + claveEncriptada, "", null);

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

            _objLog.CrearArchivolog("Codigo Resultado  --> " + codigoResultado, "", null);
            _objLog.CrearArchivolog("Mensaje Resultado ConsultaClaves --> " + mensajeResultado, "", null);
            _objLog.CrearArchivolog("Usuario  --> " + usuarioAplicacion, "", null);
            _objLog.CrearArchivolog("Clave  --> " + clave, "", null);

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

        public static T PostInvoque<T>(string name, Hashtable objHeader, object obj)
        {
            GeneradorLog objLog = new GeneradorLog(null, "Inicio metodo  PostInvoque", null, "RestServiceRegistrarEvaluacionBRMS");
            objLog.CrearArchivolog("Inicio metodo PostInvoque ", null, null);
            HttpWebRequest request = HttpWebRequest.Create(System.Configuration.ConfigurationManager.AppSettings[name]) as HttpWebRequest;
            request.Method = "POST";
            request.Headers = GetHeaders(objHeader);
            request.Accept = "application/json";

            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            string data = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            objLog.CrearArchivolog("PROY-BRMS Cadena data  -->" + data.ToString(), "", null);
            byte[] byteArray = Encoding.UTF8.GetBytes(data);
            request.Timeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["strBRMSTimeOut"]);
            objLog.CrearArchivolog(string.Format("Valor de TimeOut strBRMSTimeOut: " + request.Timeout), "", null);
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
                objLog.CrearArchivolog("PROY-BRMS RestServiceRegistrarEvaluacionBRMS() ResponseString  Return JSON: -->" + responseString.ToString(), "", null);
                T result = JsonConvert.DeserializeObject<T>(responseString);
                return result;
            }
        }

    }
}
//FIN PROY-140579 NN
