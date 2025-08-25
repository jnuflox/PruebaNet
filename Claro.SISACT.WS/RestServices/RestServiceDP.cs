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

namespace Claro.SISACT.WS.RestServices
{
    //proy-140245 
    public static class RestServiceDP
    {
        
        private static WebHeaderCollection GetHeaders(Hashtable table)
        {
            GeneradorLog _objLog = null;
            _objLog = new GeneradorLog("Metodo GetHeaders",null,null,"log_ProyOfertaColabMovil");
            
            WebHeaderCollection Headers = new WebHeaderCollection();
            
            string codigoResultado = string.Empty;
            string mensajeResultado = string.Empty;
            string usuarioAplicacion = string.Empty;
            string clave = string.Empty;

            foreach (DictionaryEntry entry in table)
            {
                Headers.Add(entry.Key.ToString(), entry.Value != null ? entry.Value.ToString() : null);
            }

            _objLog.CrearArchivolog("INICIO SERVICIO DESENCRIPTACION  --> BWConsultaClaves.ConsultaClaveWS", null, null);
           
            string ipTransaccion = HttpContext.Current.Session["CurrentTerminal"].ToString(); 
            string wsIp = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_ConsultaNacionalidad_wsIp"]);
            string usrAplicacion = HttpContext.Current.Session["CurrentUser"].ToString();
            string codigoAplicacion = ConfigurationManager.AppSettings["CodigoAplicacion"]; 
            string idAplicacion = ConfigurationManager.AppSettings["system_ConsultaClave"];
            string usuarioAplicacionEncriptado = ConfigurationManager.AppSettings["User_ConsultaNacionalidad"]; //ConfigurationManager.AppSettings["User_ValidarCantCampania"];
            string claveEncriptada = ConfigurationManager.AppSettings["Password_ConsultaNacionalidad"]; ;//ConfigurationManager.AppSettings["Password_ValidarCantCampania"];

            _objLog.CrearArchivolog("wsIp  --> " + wsIp , "", null);
            _objLog.CrearArchivolog("usrAplicacion  --> " + usrAplicacion, "", null);
            _objLog.CrearArchivolog("codigoAplicacion  --> " + codigoAplicacion, "", null);
            _objLog.CrearArchivolog("idAplicacion  --> " + idAplicacion, "", null);
            _objLog.CrearArchivolog("ipTransaccion  --> " + ipTransaccion, "", null);
            _objLog.CrearArchivolog("usuarioAplicacionEncriptado  --> " + usuarioAplicacionEncriptado, "", null);
            _objLog.CrearArchivolog("claveEncriptada  --> " + claveEncriptada, "", null);

            codigoResultado = new BWConsultaClaves().ConsultaClavesWS(DateTime.Now.ToString("YYYYMMDDHHMISSMS"),
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
            GeneradorLog objLog = new GeneradorLog(null, "Inicio metodo  PostInvoque", null, "log_ProyOfertaColabMovil");
            objLog.CrearArchivolog("Inicio metodo PostInvoque ", null, null);
            HttpWebRequest request = HttpWebRequest.Create(System.Configuration.ConfigurationManager.AppSettings[name]) as HttpWebRequest;
            request.Method = "POST";
            request.Headers = GetHeaders(objHeader);
            request.Accept = "application/json";

            string data = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            objLog.CrearArchivolog("PROY-140245 Cadena data  -->" + data.ToString(), "", null);
            byte[] byteArray = Encoding.UTF8.GetBytes(data);
            request.Timeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["strRelationPlanTimeOut"]);
            objLog.CrearArchivolog(string.Format("Valor de TimeOut strRelationPlanTimeOut:" + request.Timeout), "", null);
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
                return result; 
            }
           
        }

    }
}