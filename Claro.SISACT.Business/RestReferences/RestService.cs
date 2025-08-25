//PROY-140546
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Configuration;
using Claro.SISACT.Common;
using Claro.SISACT.WebReferences;
using Claro.SISACT.Entity.DataPowerRest;

namespace Claro.SISACT.Business.RestServices
{
    public static class RestService
    {
        public static T PostInvoque<T>(string nameURL, Hashtable oHeaders, object obj,CredencialesDPRest pCredenciales)
        {
            GeneradorLog objLog = new GeneradorLog("Inicio metodo  PostInvoque_Generic", "", "", "log_ProyFULLCLARO");
            HttpWebRequest request = HttpWebRequest.Create(ConfigurationManager.AppSettings[nameURL]) as HttpWebRequest;
            request.Method = "POST";
            request.Headers = GetHeaders_Generic(oHeaders, pCredenciales);
            request.Accept = "application/json";
            try
            {
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                objLog.CrearArchivolog(String.Format("{0} : {1}", "Cadena req", data.ToString()),null,null);
                byte[] byteArray = Encoding.UTF8.GetBytes(data);
                request.ContentType = "application/json";
                request.ContentLength = byteArray.Length;
                request.Timeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["strFullClaroTimeOut"]);
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse ws = request.GetResponse();
                using (Stream stream = ws.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    String responseString = reader.ReadToEnd();
                    objLog.CrearArchivolog(String.Format("{0} : {1}", "Cadena res", responseString),null,null);
                    T result = JsonConvert.DeserializeObject<T>(responseString);
                    return result;
                }
            }
            catch (WebException wex)
            {
                objLog.CrearArchivolog(String.Format("{0} : {1}", "Error WebException description", wex.Message),null,null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", "Error WebException Source", wex.Source),null,null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", "Error WebException StackTrace", wex.StackTrace),null,null);

                if (wex.Response != null)
                {
                    using (WebResponse WebRespException = wex.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)WebRespException;
                        
                        objLog.CrearArchivolog(String.Format("{0} : {1}", "Error Code", httpResponse.StatusCode.ToString()),null,null);

                        using (Stream data = WebRespException.GetResponseStream())
                        {
                            using (var reader = new StreamReader(data))
                            {
                                string text = reader.ReadToEnd();
                                objLog.CrearArchivolog(String.Format("{0} : {1}", "Error description", text), null, null);
                            }
                        }
                    }
                }

                return default(T);
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog(String.Format("{0} : {1}", "Error description", ex.Message),null,null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", "Error Source", ex.Source),null,null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", "Error StackTrace", ex.StackTrace),null,null);

                return default(T);
            }
        }
        
        private static WebHeaderCollection GetHeaders_Generic(Hashtable table, CredencialesDPRest pCredenciales)
        {
            GeneradorLog _objLog = null;
            _objLog = new GeneradorLog("Metodo GetHeaders_Generic", null, null, "log_ProyFULLCLARO");

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
            string wsIp = ReadKeySettings.ConsCurrentIP;
            string usrAplicacion = ConfigurationManager.AppSettings["system_ConsultaClave"];
            string codigoAplicacion = ConfigurationManager.AppSettings["CodigoAplicacion"];
            string idAplicacion = ConfigurationManager.AppSettings["system_ConsultaClave"];
            string usuarioAplicacionEncriptado = pCredenciales.UsuarioEncriptado;
            string claveEncriptada = pCredenciales.ClaveEncriptada;

            _objLog.CrearArchivolog("wsIp  --> " + wsIp, "", null);
            _objLog.CrearArchivolog("ipServidor  --> " + wsIp, "", null);
            _objLog.CrearArchivolog("usrAplicacion  --> " + usrAplicacion, "", null);
            _objLog.CrearArchivolog("codigoAplicacion  --> " + codigoAplicacion, "", null);
            _objLog.CrearArchivolog("idAplicacion  --> " + idAplicacion, "", null);
            _objLog.CrearArchivolog("ipTransaccion  --> " + ipTransaccion, "", null);
            _objLog.CrearArchivolog("usuarioAplicacionEncriptado  --> " + usuarioAplicacionEncriptado, "", null);
            _objLog.CrearArchivolog("claveEncriptada  --> " + claveEncriptada, "", null);

            codigoResultado = "0";
            codigoResultado = "";
            usuarioAplicacion = "";
            clave = "";
            
            codigoResultado = new BWConsultaClaves().ConsultaClavesWS(ipTransaccion,
                                                                 wsIp,
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

            string strEncryptedBase64 = "";

            if (codigoResultado == "0")
            {
                strEncryptedBase64 = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", usuarioAplicacion, clave)));

                if (strEncryptedBase64 != "1" && strEncryptedBase64 != "-1" && strEncryptedBase64 != "-2" && strEncryptedBase64 != "-3")
                {
                    Headers.Add("Authorization", "Basic " + strEncryptedBase64);
                }                
            }
            _objLog.CrearArchivolog("Headers  --> Authorization = Basic " + strEncryptedBase64, null, null);

            return Headers;
        }
    }
}