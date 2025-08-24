using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Claro.SISACT.Common;
using System.Net;
using System.Web.Script.Serialization;
using System.IO;
using Newtonsoft.Json;
using System.Configuration;

namespace Claro.SISACT.WS.RestReferences
{
    public class PostInvoqueDP
    {
        public static T PostInvoque<T>(string name, Hashtable objHeader, object obj, string usuario, string ipServidor, string strnombreProy, string strWSIP, string strUserEncrypted, string strPassEncrypted, string strTimeout)
        {
            GeneradorLog objLog = new GeneradorLog(null, "Inicio metodo  PostInvoque", null, strnombreProy);
            objLog.CrearArchivolog("Inicio metodo PostInvoque ", null, null);

            HttpWebRequest request = HttpWebRequest.Create(System.Configuration.ConfigurationManager.AppSettings[name]) as HttpWebRequest;
            request.Method = "POST";
            request.Headers = GetHeadersDP.GetHeaders(objHeader, usuario, ipServidor, strnombreProy, strWSIP, strUserEncrypted, strPassEncrypted);
            request.Accept = "application/json";

            objLog.CrearArchivolog(String.Format("{0} : {1}", "Valor de WS", Funciones.CheckStr(System.Configuration.ConfigurationManager.AppSettings[name])), null, null);
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            string data = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            objLog.CrearArchivolog(String.Format("{0} : {1}","Cadena data",data.ToString()), null, null);
            byte[] byteArray = Encoding.UTF8.GetBytes(data);
            request.Timeout = Convert.ToInt32(strTimeout);
            objLog.CrearArchivolog(String.Format("{0} : {1}","Valor de TimeOut",request.Timeout), null, null);
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
                objLog.CrearArchivolog(String.Format("{0} : {1}", "Response data", Funciones.CheckStr(responseString)), null, null);
                T result = JsonConvert.DeserializeObject<T>(responseString);
                return result;
            }
        }
    }
}
