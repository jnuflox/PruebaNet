using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Claro.SISACT.Common;
using Claro.SISACT.Entity.claro_vent_ventascontingencia;
using System.Net;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using System.Web;

namespace Claro.SISACT.WS.RestServices
{
    public static class RestServiceConsultarVentasContingencia
    {
        static string strArchivo = "RestServiceConsultarVentasContingencia";

        public static WebHeaderCollection GetHeaders(Hashtable table, string userEncriptado = "", string passEncriptado = "")
        {
            WebHeaderCollection Headers = new WebHeaderCollection();
            string idLogDatos = "GetHeaders";
            string codigoResultado = string.Empty;
            string mensajeResultado = string.Empty;
            string usuarioAplicacion = string.Empty;
            string clave = string.Empty;

            foreach (DictionaryEntry entry in table)
            {
                Headers.Add(entry.Key.ToString(), entry.Value != null ? entry.Value.ToString() : null);
            }

            GeneradorLog.EscribirLog(strArchivo, idLogDatos, "***** INICIO MÉTODO GetHeaders *****");
            string idTransaccion = DateTime.Now.ToString("yyyyMMddHHmmss");
            string ipAplicacion = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_ConsultaNacionalidad_wsIp"]);
            string ipTransicion = DateTime.Now.ToString("yyyyMMddHHmmss");
            string usrAplicacion = Funciones.CheckStr(HttpContext.Current.Session["CurrentUser"]);
            string idAplicacion = Funciones.CheckStr(ConfigurationManager.AppSettings["CodigoAplicacion"]);
            string codigoAplicacion = Funciones.CheckStr(ConfigurationManager.AppSettings["system_ConsultaClave"]);
            string strUsuario = userEncriptado != string.Empty ? userEncriptado : Funciones.CheckStr(ConfigurationManager.AppSettings["User_ConsultaNacionalidad"]);
            string strPassword = passEncriptado != string.Empty ? passEncriptado : Funciones.CheckStr(ConfigurationManager.AppSettings["Password_ConsultaNacionalidad"]);

            GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("{0}|{1}", "idTransaccion : ", idTransaccion));
            GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("{0}|{1}", "ipAplicacion : ", ipAplicacion));
            GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("{0}|{1}", "ipTransicion : ", ipTransicion));
            GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("{0}|{1}", "usrAplicacion : ", usrAplicacion));
            GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("{0}|{1}", "idAplicacion : ", idAplicacion));
            GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("{0}|{1}", "codigoAplicacion : ", codigoAplicacion));
            GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("{0}|{1}", "strUsuarioEncriptado : ", strUsuario));
            GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("{0}|{1}", "strPasswordEncriptado : ", strPassword));

            codigoResultado = new BWConsultaClaves().ConsultaClavesWS(
                                                                 idTransaccion,
                                                                 ipAplicacion,
                                                                 ipTransicion,
                                                                 usrAplicacion,
                                                                 idAplicacion,
                                                                 codigoAplicacion,
                                                                 strUsuario,
                                                                 strPassword,
                                                                 out mensajeResultado,
                                                                 out usuarioAplicacion,
                                                                 out clave);

            GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("{0}|{1}", "strUsuarioDesEncriptado : ", usuarioAplicacion));
            GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("{0}|{1}", "strPasswordDesEncriptado : ", clave));
            string strEncryptedBase64;
            GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("{0}|{1}", "mensajeResultado : ", mensajeResultado));
            GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("{0}|{1}", "codigoResultado : ", codigoResultado));

            strEncryptedBase64 = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", usuarioAplicacion, clave)));
            GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("{0}|{1}", "strEncryptedBase64 : ", strEncryptedBase64));
            GeneradorLog.EscribirLog(strArchivo, idLogDatos, "***** FIN MÉTODO GetHeaders *****");
            if (strEncryptedBase64 != "1" && strEncryptedBase64 != "-1" && strEncryptedBase64 != "-2" && strEncryptedBase64 != "-3")
            {
                Headers.Add("Authorization", "Basic " + strEncryptedBase64);
            }
            return Headers;
        }

        public static T GetInvoque<T>(string name, Hashtable objHeader, Dictionary<string, string> parametros, string userEncriptado = "", string passEncriptado = "", string strNombreTimeOut = "")
        {
            string idLogDatos = "GetInvoque";

            GeneradorLog.EscribirLog(strArchivo, idLogDatos, "***********INICIO MÉTODO GetInvoque***********");
            string url = Funciones.CheckStr(System.Configuration.ConfigurationManager.AppSettings[name]);
            string parametrosConcatenados = ConcatParametros(parametros);
            string urlConParametros = url + "?" + parametrosConcatenados;

            GeneradorLog.EscribirLog(strArchivo, null, "MÉTODO GetInvoque-URI--->" + urlConParametros);

            HttpWebRequest request = HttpWebRequest.Create(urlConParametros) as HttpWebRequest;

            request.Method = "GET";
            request.Headers = GetHeaders(objHeader, userEncriptado, passEncriptado);
            request.Accept = "application/json;charset=UTF-8";
            request.Timeout = strNombreTimeOut != string.Empty ? Convert.ToInt32(strNombreTimeOut) : Convert.ToInt32(ConfigurationManager.AppSettings["TimeOut_consultaStock"]);

            GeneradorLog.EscribirLog(strArchivo, idLogDatos, "***********MÉTODO GetInvoque-INICIO-GetResponse***********");
            WebResponse ws = request.GetResponse();
            GeneradorLog.EscribirLog(strArchivo, idLogDatos, "***********MÉTODO GetInvoque-FIN-GetResponse***********");

            using (Stream stream = ws.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                String responseString = reader.ReadToEnd();
                GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("{0}|{1}", "responseString : ", responseString));
                T result = JsonConvert.DeserializeObject<T>(responseString);
                GeneradorLog.EscribirLog(strArchivo, idLogDatos, "***********FIN MÉTODO GetInvoque***********");
                return result;
            }
        }

        private static string ConcatParametros(Dictionary<string, string> parameters)
        {
            bool FirstParam = true;
            StringBuilder Parametros = null;

            if (parameters != null)
            {
                Parametros = new StringBuilder();
                foreach (KeyValuePair<string, string> param in parameters)
                {
                    Parametros.Append(FirstParam ? "" : "&");
                    Parametros.Append(param.Key + "=" + (param.Value));
                    FirstParam = false;
                }
            }

            return Parametros == null ? string.Empty : Parametros.ToString();
        }

    }
}
