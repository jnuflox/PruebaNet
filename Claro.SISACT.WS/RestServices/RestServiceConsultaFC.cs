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
using Claro.SISACT.Entity.ConsultarClienteFullClaroRest;

namespace Claro.SISACT.WS.RestServices
{
    public static class RestServiceConsultaFC
    {
        private static WebHeaderCollection GetHeaders(Hashtable table, string usuario, string ipServidor)
        {
            GeneradorLog _objLog = null;
            _objLog = new GeneradorLog("Metodo GetHeaders", null, null, "log_ProyFULLCLARO");

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

            string ipTransaccion = DateTime.Now.ToString("YYYYMMDDHHMISSMS");
            string wsIp = string.IsNullOrEmpty(Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_FullClaro_wsIp"])) ? ipServidor : Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_FullClaro_wsIp"]);
            string usrAplicacion = usuario;
            string codigoAplicacion = ConfigurationManager.AppSettings["CodigoAplicacion"];
            string idAplicacion = ConfigurationManager.AppSettings["system_ConsultaClave"];
            string usuarioAplicacionEncriptado = ConfigurationManager.AppSettings["User_FullClaroService"];
            string claveEncriptada = ConfigurationManager.AppSettings["Password_FullClaroService"];

            _objLog.CrearArchivolog("wsIp  --> " + wsIp, "", null);
            _objLog.CrearArchivolog("ipServidor  --> " + ipServidor, "", null);
            _objLog.CrearArchivolog("usrAplicacion  --> " + usrAplicacion, "", null);
            _objLog.CrearArchivolog("codigoAplicacion  --> " + codigoAplicacion, "", null);
            _objLog.CrearArchivolog("idAplicacion  --> " + idAplicacion, "", null);
            _objLog.CrearArchivolog("ipTransaccion  --> " + ipTransaccion, "", null);

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

        public static T PostInvoque<T>(string name, Hashtable objHeader, ConsultarClientesFullClaroRequest obj, string usuario, string ipServidor)
        {
            GeneradorLog objLog = new GeneradorLog(null, "Inicio metodo  PostInvoque", null, "log_ProyFULLCLARO");
            objLog.CrearArchivolog("Inicio metodo PostInvoque GET", null, null);//INICIATIVA-219
            var parametroURL = string.Format("?tipoDocumento={0}&nroDocumento={1}", obj.MessageRequest.body.tipoDocumento, obj.MessageRequest.body.nroDocumento);
            objLog.CrearArchivolog(string.Format("Valor de URL: " + parametroURL), "", null);//INICIATIVA-219
            HttpWebRequest request = HttpWebRequest.Create(String.Format("{0}{1}",System.Configuration.ConfigurationManager.AppSettings[name],parametroURL)) as HttpWebRequest;
            request.Method = "GET";
            request.Headers = GetHeaders(objHeader, usuario, ipServidor);
            request.Accept = "application/json";

            request.Timeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["strFullClaroTimeOut"]);
            objLog.CrearArchivolog(string.Format("Valor de TimeOut strFullClaroTimeOut: " + request.Timeout), "", null);
            request.ContentType = "application/json";

            WebResponse ws = request.GetResponse();

            using (Stream stream = ws.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                String responseString = reader.ReadToEnd();
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[PostInvoque<T>][responseString]", Funciones.CheckStr(responseString)), null, null);//INICIATIVA-219
                T result = JsonConvert.DeserializeObject<T>(responseString);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[FIN][PostInvoque<T>]", string.Empty), null, null);//INICIATIVA-219
                return result;
            }
        }
    }
}
