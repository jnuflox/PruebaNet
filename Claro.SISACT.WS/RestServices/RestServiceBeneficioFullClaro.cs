using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Common;
using System.Configuration;
using System.Net;
using System.Collections;
using System.IO;
using Newtonsoft.Json;
using Claro.SISACT.Entity;
using System.Web;

namespace Claro.SISACT.WS.RestServices
{
    public class RestServiceBeneficioFullClaro
    {
        static string strArchivo = Funciones.CheckStr(ConfigurationManager.AppSettings["strNombreLogSISACT"]);

        protected RestServiceBeneficioFullClaro()
        {
        }

        private static WebHeaderCollection GetHeaders(Hashtable table, string strUserEncrypted, string strPassEncrypted)
        {
            WebHeaderCollection Headers = new WebHeaderCollection();
            string idLogDatos = "GetHeaders";
            string codigoResultado = string.Empty;
            string usuarioAplicacion = string.Empty;
            string clave = string.Empty;
            string strmensajeResultado = string.Empty;
            string strusuarioAplicacion = string.Empty;
            string strclave = string.Empty;

            string strEncryptedBase64;

            GeneradorLog.EscribirLog(strArchivo, idLogDatos, "***** INICIO MÉTODO GetHeaders *****");
            foreach (DictionaryEntry entry in table)
            {
                Headers.Add(entry.Key.ToString(), entry.Value != null ? entry.Value.ToString() : null);
                GeneradorLog.EscribirLog(strArchivo, String.Format("{0} --> {1} = {2}", "Headers", Funciones.CheckStr(entry.Key), Funciones.CheckStr(entry.Value)));
            }

            string ipTransaccion = Funciones.CheckStr(HttpContext.Current.Session["CurrentTerminal"]); // BIEN
            string ipServidor = string.Empty;
            string wsIp = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_CampanaFullClaro_wsIp"]);
            string usrAplicacion = Funciones.CheckStr(HttpContext.Current.Session["CurrentUser"]);
            string codigoAplicacion = Funciones.CheckStr(ConfigurationManager.AppSettings["constAplicacion"]);
            string idAplicacion = Funciones.CheckStr(ConfigurationManager.AppSettings["constUsuarioAplicacion"]);
            string usuarioAplicacionEncriptado = strUserEncrypted;
            string claveEncriptada = strPassEncrypted;

            GeneradorLog.EscribirLog(strArchivo, String.Format("{0} --> {1}", "[GetHeaders] ipTransaccion", Funciones.CheckStr(ipTransaccion)));
            GeneradorLog.EscribirLog(strArchivo, String.Format("{0} --> {1}", "[GetHeaders] ipServidor", Funciones.CheckStr(ipServidor)));
            GeneradorLog.EscribirLog(strArchivo, String.Format("{0} --> {1}", "[GetHeaders] wsIp", Funciones.CheckStr(wsIp)));
            GeneradorLog.EscribirLog(strArchivo, String.Format("{0} --> {1}", "[GetHeaders] usrAplicacion", Funciones.CheckStr(usrAplicacion)));
            GeneradorLog.EscribirLog(strArchivo, String.Format("{0} --> {1}", "[GetHeaders] codigoAplicacion", Funciones.CheckStr(codigoAplicacion)));
            GeneradorLog.EscribirLog(strArchivo, String.Format("{0} --> {1}", "[GetHeaders] idAplicacion", Funciones.CheckStr(idAplicacion)));
            GeneradorLog.EscribirLog(strArchivo, String.Format("{0} --> {1}", "[GetHeaders] usuarioAplicacionEncriptado", Funciones.CheckStr(usuarioAplicacionEncriptado)));
            GeneradorLog.EscribirLog(strArchivo, String.Format("{0} --> {1}", "[GetHeaders] claveEncriptada", Funciones.CheckStr(claveEncriptada)));

            codigoResultado = new BWConsultaClaves().ConsultaClavesWS(ipTransaccion,
                                                                 ipServidor,
                                                                 ipTransaccion,
                                                                 usrAplicacion,
                                                                 codigoAplicacion,
                                                                 idAplicacion,
                                                                 usuarioAplicacionEncriptado,
                                                                 claveEncriptada,
                                                                 out strmensajeResultado,
                                                                 out strusuarioAplicacion,
                                                                 out strclave
                                                                 );

            GeneradorLog.EscribirLog(strArchivo, String.Format("{0} --> {1}", "[GetHeaders] Codigo Resultado", codigoResultado));

            if (codigoResultado == "0")
            {
                usuarioAplicacion = strusuarioAplicacion;
                clave = strclave;

                GeneradorLog.EscribirLog(strArchivo, String.Format("{0}  --> {1}", "Usuario", usuarioAplicacion), false);
                GeneradorLog.EscribirLog(strArchivo, String.Format("{0}  --> {1}", "Clave", clave), false);

                strEncryptedBase64 = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", usuarioAplicacion, clave)));

                if (strEncryptedBase64 != "1" && strEncryptedBase64 != "-1" && strEncryptedBase64 != "-2" && strEncryptedBase64 != "-3")
                {
                    Headers.Add("Authorization", String.Format("{0} {1}", "Basic", strEncryptedBase64));
                }
            }

            GeneradorLog.EscribirLog(strArchivo, "FIN SERVICIO DESENCRIPTACION  --> BWConsultaClaves.ConsultaClaveWS", false);
            GeneradorLog.EscribirLog(strArchivo, idLogDatos, "***** FIN MÉTODO GetHeaders *****");
            return Headers;
        }

        public static T PostInvoque<T>(string name, Hashtable objHeader, object obj, string strUserEncrypted, string strPassEncrypted)
        {
            string idLogDatos = "[INICIATIVA-805][Campana Descuento Cargo Fijo]";
            GeneradorLog.EscribirLog(strArchivo, String.Format("{0}", "[RestServiceBeneficioFullClaro] [INICIO PostInvoque]"));
            GeneradorLog.EscribirLog(strArchivo, idLogDatos, "***********INICIO MÉTODO PostInvoque***********");
            HttpWebRequest request = HttpWebRequest.Create(System.Configuration.ConfigurationManager.AppSettings[name]) as HttpWebRequest;
            request.Method = "POST";
            request.Headers = GetHeaders(objHeader, strUserEncrypted, strPassEncrypted);
            request.Accept = "application/json";
            request.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["BeneficioFC_TimeOut"]);

            string data = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            byte[] byteArray = Encoding.UTF8.GetBytes(data);
            GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("{0}|{1}", "request : ", data));
            request.ContentType = "application/json;charset=UTF-8";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            GeneradorLog.EscribirLog(strArchivo, idLogDatos, "***********MÉTODO PostInvoque-INICIO-GetResponse***********");
            WebResponse ws = request.GetResponse();
            GeneradorLog.EscribirLog(strArchivo, idLogDatos, "***********MÉTODO PostInvoque-FIN-GetResponse***********");

            using (Stream stream = ws.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                String responseString = reader.ReadToEnd();
                GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("{0}|{1}", "responseString : ", responseString));
                T result = JsonConvert.DeserializeObject<T>(responseString);
                GeneradorLog.EscribirLog(strArchivo, idLogDatos, "***********FIN MÉTODO PostInvoque***********");
                return result;
            }
        }
    }
}
