//PROY-140546
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Claro.SISACT.Common;
using System.Net;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;

namespace Claro.SISACT.WS.RestServices
{
    public class RestServiceConsultaHist
    {
        public static T PostInvoque<T>(string name, Hashtable objHeader, object obj, string ipServidor)
        {
            string strArchivo = "Log_consultarHistorico";
            string idTransaccion = DateTime.Now.ToString("yyyyMMddHHmmssfff");

            GeneradorLog objLog = null;
            objLog = new GeneradorLog(strArchivo, "Metodo PostInvoque ", idTransaccion, null);
            objLog.CrearArchivolog("PROY-140546|RestServiceConsultaHist|PostInvoque|-- INICIO --", null, null);

            HttpWebRequest request = HttpWebRequest.Create(ConfigurationManager.AppSettings[name]) as HttpWebRequest;
            request.Method = "POST";
            request.Headers = GetHeaders(objHeader, ipServidor);
            request.Accept = "application/json";

            string data = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            objLog.CrearArchivolog(String.Format("{0} --> {1}", "PROY-140546 Cadena data", data.ToString()), null, null);
            byte[] byteArray = Encoding.UTF8.GetBytes(data);
            request.Timeout = Funciones.CheckInt(ConfigurationManager.AppSettings["strFullClaroTimeOut"]);
            objLog.CrearArchivolog(String.Format("{0} : {1}", "Valor de TimeOut strFullClaroTimeOut", request.Timeout), null, null);
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
                objLog.CrearArchivolog(String.Format("{0} : {1}", "Response data", Funciones.CheckStr(responseString)), null, null);
                objLog.CrearArchivolog("PROY-140546|RestServiceConsultaHist|PostInvoque|-- FIN --", null, null);
                return result;
            }
        }

        private static WebHeaderCollection GetHeaders(Hashtable table, string ipServidor)
        {
            string strArchivo = "Log_consultarHistorico";
            string idTransaccion = DateTime.Now.ToString("yyyyMMddHHmmssfff");

            GeneradorLog objLog = null;
            objLog = new GeneradorLog(strArchivo, "Metodo GetHeaders ", idTransaccion, null);
            objLog.CrearArchivolog("PROY-140546|RestServiceConsultaHist|GetHeaders|-- INICIO --", null, null);

            WebHeaderCollection Headers = new WebHeaderCollection();

            string codigoResultado = string.Empty;
            string mensajeResultado = string.Empty;
            string usuarioAplicacion = string.Empty;
            string clave = string.Empty;

            foreach (DictionaryEntry entry in table)
            {
                Headers.Add(Funciones.CheckStr(entry.Key), Funciones.CheckStr(entry.Value) != null ? Funciones.CheckStr(entry.Value) : null);
                objLog.CrearArchivolog(String.Format("{0} --> {1} = {2}", "Headers", Funciones.CheckStr(entry.Key), Funciones.CheckStr(entry.Value)), null, null);
            }

            objLog.CrearArchivolog("INICIO SERVICIO DESENCRIPTACION  --> BWConsultaClaves.ConsultaClaveWS", null, null);

            string strIdTransaccion = string.Empty;
            string ipTransaccion = DateTime.Now.ToString("YYYYMMDDHHMISSMS");
            string wsIp = string.IsNullOrEmpty(Funciones.CheckStr(ReadKeySettings.ConsCurrentIP)) ? ipServidor : Funciones.CheckStr(ReadKeySettings.ConsCurrentIP);
            string usrAplicacion = ConfigurationManager.AppSettings["system_ConsultaClave"];
            string codigoAplicacion = ConfigurationManager.AppSettings["CodigoAplicacion"];
            string idAplicacion = ConfigurationManager.AppSettings["system_ConsultaClave"];
            string usuarioAplicacionEncriptado = ConfigurationManager.AppSettings["cons_CobroAnticipadoUser"];
            string claveEncriptada = ConfigurationManager.AppSettings["cons_CobroAnticipadoPass"];
            string codigoRespuesta = string.Empty;
            objLog.CrearArchivolog(String.Format("{0} --> {1}", "wsIp", wsIp), null, null);
            objLog.CrearArchivolog(String.Format("{0} --> {1}", "usrAplicacion", usrAplicacion), null, null);
            objLog.CrearArchivolog(String.Format("{0} --> {1}", "codigoAplicacion", codigoAplicacion), null, null);
            objLog.CrearArchivolog(String.Format("{0} --> {1}", "idAplicacion", idAplicacion), null, null);
            objLog.CrearArchivolog(String.Format("{0} --> {1}", "ipTransaccion", ipTransaccion), null, null);
            objLog.CrearArchivolog(String.Format("{0} --> {1}", "usuarioAplicacionEncriptado", usuarioAplicacionEncriptado), null, null);
            objLog.CrearArchivolog(String.Format("{0} --> {1}", "claveEncriptada", claveEncriptada), null, null);

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

            objLog.CrearArchivolog(String.Format("{0} --> {1}", "Codigo Resultado", codigoResultado), null, null);
            objLog.CrearArchivolog(String.Format("{0} --> {1}", "Mensaje Resultado ConsultaClaves", mensajeResultado), null, null);
            objLog.CrearArchivolog(String.Format("{0} --> {1}", "Usuario", usuarioAplicacion), null, null);
            objLog.CrearArchivolog(String.Format("{0} --> {1}", "Clave", clave), null, null);

            string strEncryptedBase64;

            if (codigoResultado == "0")
            {
                objLog.CrearArchivolog("Ingresó a encriptar las credenciales a 64 bits", null, null);
                strEncryptedBase64 = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", usuarioAplicacion, clave)));

                if (strEncryptedBase64 != "1" && strEncryptedBase64 != "-1" && strEncryptedBase64 != "-2" && strEncryptedBase64 != "-3")
                {
                    objLog.CrearArchivolog(String.Format("{0} --> {1}", "strEncryptedBase64", strEncryptedBase64), null, null);
                    Headers.Add("Authorization", "Basic " + strEncryptedBase64);
                }
            }

            objLog.CrearArchivolog("PROY-140546|RestServiceConsultaHist|GetHeaders|-- FIN --", null, null);

            return Headers;
        }
    }
}