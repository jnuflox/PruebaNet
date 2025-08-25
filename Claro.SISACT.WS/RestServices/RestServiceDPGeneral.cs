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
using Claro.SISACT.Entity;
using System.Configuration;
using Newtonsoft.Json.Linq;

namespace Claro.SISACT.WS.RestServices
{
    public class RestServiceDPGeneral
    {        
        public static string strArchivo = "RestServiceDPGeneral.cs"; //PROY-140618 - IDEA-142181 - Mejora Proceso de Portabilidad

        public static T PostInvoque<T>(object obj, BEAuditoriaRequest oAuditoria)
        {
            T result;
            GeneradorLog objLog = new GeneradorLog(null, string.Format("{0}{1}", "[INICIO][PostInvoque<T>]", string.Empty), null, null);
            HttpWebRequest request = HttpWebRequest.Create(System.Configuration.ConfigurationManager.AppSettings[oAuditoria.urlRest]) as HttpWebRequest;
            try
            {
                request.Method = "POST";
                request.Headers = GetHeaders(oAuditoria);
                request.Accept = "application/json";

                string data = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[PostInvoque<T>][CadenaData]", Funciones.CheckStr(data)), null, null);
                byte[] byteArray = Encoding.UTF8.GetBytes(data);
                request.Timeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings[oAuditoria.urlTimeOut_Rest]);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[PostInvoque<T>][Valor de TimeOut : urlTimeOut_Rest]", request.Timeout), null, null);
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
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[PostInvoque<T>][responseString]", Funciones.CheckStr(responseString)), null, null);
                    result = JsonConvert.DeserializeObject<T>(responseString);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[FIN][PostInvoque<T>]", string.Empty), null, null);
                    return result;
                }
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[PostInvoque<T>][Error code:]", Funciones.CheckStr(httpResponse.StatusCode)), null, null);

                    using (Stream data = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(data))
                        {
                            string responseString = reader.ReadToEnd();
                            result = JsonConvert.DeserializeObject<T>(responseString);
                            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[PostInvoque<T>][Error:]", Funciones.CheckStr(responseString)), null, null);
                            objLog.CrearArchivolog(string.Format("{0}{1}", "[FIN][PostInvoque<T>]", string.Empty), null, null);
                            return result;
                        }
                    }
                }
            }
        }

        private static WebHeaderCollection GetHeaders(BEAuditoriaRequest oAuditoria)
        {
            GeneradorLog objLog = new GeneradorLog(null, string.Format("{0}{1}", "[INICIO][GetHeaders]", string.Empty), null, null);
            WebHeaderCollection Headers = new WebHeaderCollection();

            string codigoResultado = string.Empty;
            string mensajeResultado = string.Empty;
            string usuarioAplicacion = string.Empty;
            string clave = string.Empty;
            string strEncryptedBase64 = string.Empty;

            foreach (DictionaryEntry entry in oAuditoria.table)
            {
                Headers.Add(entry.Key.ToString(), entry.Value != null ? entry.Value.ToString() : null);
                objLog.CrearArchivolog(string.Format("[GetHeaders] [key]: {0} --> [Value]: {1} ", entry.Key.ToString(), entry.Value.ToString()), null, null);
            }

            if (oAuditoria.dataPower)
            {
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[GetHeaders][bolDataPower]", oAuditoria.dataPower), null, null);

                codigoResultado = new BWConsultaClaves().ConsultaClavesWS(DateTime.Now.ToString("YYYYMMDDHHMISSMS"),
                                                                            oAuditoria.wsIp,
                                                                            oAuditoria.ipTransaccion,
                                                                            oAuditoria.usuarioAplicacion,
                                                                            oAuditoria.idAplicacion,
                                                                            oAuditoria.applicationCode,
                                                                            oAuditoria.usuarioAplicacionEncriptado,
                                                                            oAuditoria.claveEncriptada,
                                                                             out mensajeResultado,
                                                                             out usuarioAplicacion,
                                                                            out clave);

                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[GetHeaders][Codigo Resultado]", Funciones.CheckStr(codigoResultado)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[GetHeaders][mensajeResultado]", Funciones.CheckStr(mensajeResultado)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[GetHeaders][usuarioAplicacion]", Funciones.CheckStr(usuarioAplicacion)), null, null);

                if (codigoResultado == "0")
                {

                    strEncryptedBase64 = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", usuarioAplicacion, clave)));

                    if (strEncryptedBase64 != "1" && strEncryptedBase64 != "-1" && strEncryptedBase64 != "-2" && strEncryptedBase64 != "-3")
                    {
                        Headers.Add("Authorization", "Basic " + strEncryptedBase64);
                    }
                }
            }
            else
            {
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[GetHeaders][bolDataPower]", "false"), null, null);
            }

            objLog.CrearArchivolog(string.Format("{0}{1}", "[FIN][GetHeaders]", string.Empty), null, null);

            return Headers;            
        }

        //PROY-140618 - IDEA-142181 - Mejora Proceso de Portabilidad INI
        public static T HttpCallInvoque<T>(string httpMethod, string nombreKeyUrl, Dictionary<string, string> parametros, object objData, Hashtable objHeader, BEAuditoriaRequest objAuditoria)
        {
            try
            {
                GeneradorLog.EscribirLog(strArchivo, null, string.Format("{0} {1}<{2}> {0}", "----------------", "Inicio Método HttpCallGet", typeof(T).Name));
                string uri = getUri(nombreKeyUrl, parametros);
                HttpWebRequest request = getRequest(uri, httpMethod, objData, objHeader, objAuditoria);
                WebResponse ws = request.GetResponse();
                using (Stream stream = ws.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    String responseString = reader.ReadToEnd();
                    GeneradorLog.EscribirLog(strArchivo, null, string.Format("responseString --> {0}", responseString)); //PROY-140743
                    T result = JsonConvert.DeserializeObject<T>(responseString);
                    JObject obj = JObject.FromObject(result);
                    GeneradorLog.EscribirLog(strArchivo, null, string.Format("responseString --> {0}", obj.ToString(Formatting.None)));
                    GeneradorLog.EscribirLog(strArchivo, null, string.Format("{0} {1}<{2}> {0}", "----------------", "Fin Método HttpCallGet", typeof(T).Name));
                    return result;
                }
            }
            catch (Exception ex)
            {
                GeneradorLog.EscribirLog(strArchivo, null, string.Format("ERROR. Ha ocurrido un error al crear la uri. {0}", ex.Message), ex);
                throw;
            }
        }

        public static string getUri(string nombreKeyUrl, Dictionary<string, string> parametros)
        {
            GeneradorLog.EscribirLog(strArchivo, null, string.Format("{0} {1} {0}", "===================", "Inicio Método getUri"));
            string uri = string.Empty;
            try
            {
                uri = Funciones.CheckStr(ConfigurationManager.AppSettings[nombreKeyUrl]);
                GeneradorLog.EscribirLog(strArchivo, null, string.Format("[URI] --->[ {0} ]", uri));

                if (parametros.Count > 0)
                    uri = string.Format("{0}?{1}", uri, getQueryString(parametros));
            }
            catch (Exception ex)
            {
                GeneradorLog.EscribirLog(strArchivo, null, string.Format("ERROR. Ha ocurrido un error al crear la uri. {0}", ex.Message), ex);
                throw;
            }
            GeneradorLog.EscribirLog(strArchivo, null, string.Format("{0} {1} {0}", "----------", "Fin Método getUri"));
            return uri;
        }

        private static string getQueryString(Dictionary<string, string> parameters)
        {
            GeneradorLog.EscribirLog(strArchivo, null, string.Format("{0} {1} {0}", "----------", "Fin Método getQueryString"));
            bool FirstParam = true;
            StringBuilder strQueryString = null;

            if (parameters != null)
            {
                strQueryString = new StringBuilder();
                foreach (KeyValuePair<string, string> param in parameters)
                {
                    strQueryString.Append(FirstParam ? "" : "&");
                    strQueryString.Append(param.Key + "=" + (param.Value));
                    GeneradorLog.EscribirLog(strArchivo, null, string.Format("[{0} {1}] | [{2} {3}] ", "Key : ", param.Key, "value : ", param.Value));
                    FirstParam = false;
                }
            }
            GeneradorLog.EscribirLog(strArchivo, null, string.Format("{0} [{1}] ", "strQueryString : ", strQueryString.ToString()));
            GeneradorLog.EscribirLog(strArchivo, null, string.Format("{0} {1} {0}", "----------", "Fin Método getQueryString"));
            return strQueryString == null ? string.Empty : strQueryString.ToString();

        }
        public static HttpWebRequest getRequest(string uri, string httpMethod, object objData, Hashtable objHeader, BEAuditoriaRequest objAuditoria)
        {
            try
            {
                GeneradorLog.EscribirLog(strArchivo, null, string.Format("{0} {1} {0}", "----------", "Inicio Método getRequest"));

                GeneradorLog.EscribirLog(strArchivo, null, string.Format("{0} {1} {0}", "=============", "parametros consulta claves"));
                GeneradorLog.EscribirLog(strArchivo, null, string.Format("objAuditoria.wsIp : {0}", objAuditoria.wsIp));
                GeneradorLog.EscribirLog(strArchivo, null, string.Format("objAuditoria.ipTransaccion : {0}", objAuditoria.ipTransaccion));
                GeneradorLog.EscribirLog(strArchivo, null, string.Format("objAuditoria.usuarioAplicacion : {0}", objAuditoria.usuarioAplicacion));
                GeneradorLog.EscribirLog(strArchivo, null, string.Format("objAuditoria.idAplicacion : {0}", objAuditoria.idAplicacion));
                GeneradorLog.EscribirLog(strArchivo, null, string.Format("objAuditoria.applicationCode : {0}", objAuditoria.applicationCode));
                GeneradorLog.EscribirLog(strArchivo, null, string.Format("objAuditoria.usuarioAplicacionEncriptado : {0}", objAuditoria.usuarioAplicacionEncriptado));
                GeneradorLog.EscribirLog(strArchivo, null, string.Format("objAuditoria.claveEncriptada : {0}", objAuditoria.claveEncriptada));

                GeneradorLog.EscribirLog(strArchivo, null, string.Format("{0} {1} {0}", "=============", "parametros consulta claves"));

                HttpWebRequest request = HttpWebRequest.Create(uri) as HttpWebRequest;
                request.Method = httpMethod;
                request.Headers = GetHeaders(objAuditoria);
                request.Accept = "application/json;charset=UTF-8";
                request.Timeout = Convert.ToInt32(objAuditoria.urlTimeOut_Rest);

                GeneradorLog.EscribirLog(strArchivo, null, string.Format("Request Uri --> {0}", request.RequestUri));
                GeneradorLog.EscribirLog(strArchivo, null, string.Format("Request Method --> {0}", request.Method));
                GeneradorLog.EscribirLog(strArchivo, null, string.Format("Request Accept --> {0}", request.Accept));
                GeneradorLog.EscribirLog(strArchivo, null, string.Format("Request Timeout --> {0}", request.Timeout));
                GeneradorLog.EscribirLog(strArchivo, null, string.Format("{0} {1} {0}", "--------------", "Request Headers"));

                for (int i = 0; i < request.Headers.Count; i++)
                {
                    string header = request.Headers.GetKey(i);
                    foreach (string value in request.Headers.GetValues(i))
                    {
                        GeneradorLog.EscribirLog(strArchivo, null, string.Format("Request Header {0} --> {1}", header, value));
                    }
                }

                GeneradorLog.EscribirLog(strArchivo, null, string.Format("{0} {1} {0}", "=================", "Request getRequest"));

                if (objData != null)
                {
                    string data = Newtonsoft.Json.JsonConvert.SerializeObject(objData);

                    byte[] byteArray = Encoding.UTF8.GetBytes(data);
                    request.ContentType = "application/json;charset=UTF-8";
                    request.ContentLength = byteArray.Length;

                    Stream dataStream = request.GetRequestStream();
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();

                    GeneradorLog.EscribirLog(strArchivo, null, string.Format("Request ContentType --> {0}", request.ContentType));
                    GeneradorLog.EscribirLog(strArchivo, null, string.Format("Request ContentLength --> {0}", request.ContentLength));
                    GeneradorLog.EscribirLog(strArchivo, null, string.Format("Request Data --> {0}", data.ToString()));
                }

                return request;
            }
            catch (Exception ex)
            {
                GeneradorLog.EscribirLog(strArchivo, null, string.Format("ERROR. Ha ocurrido un error en el método getRequest. {0}", ex.Message), ex);
                throw;
            }
            finally
            {
                GeneradorLog.EscribirLog(strArchivo, null, string.Format("{0} {1} {0}", "=================", "Fin Método getRequest"));
            }
        }
        //PROY-140618 - IDEA-142181 - Mejora Proceso de Portabilidad FIN
    }
}