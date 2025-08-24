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
using Claro.SISACT.Entity;

//using Claro.SISACT.WS;

//using System.Web;
using System.Runtime.Serialization;
using System.ServiceModel;
//PROY-140657
namespace Claro.SISACT.WS.RestReferences
{
    public class GetInvoqueDP
    {
        public static T GetInvoque<T>(BEItemGetInvoque objGetInvoque,Hashtable objHeader) 
        {
            GeneradorLog objLog = new GeneradorLog(null, "Inicio metodo  GetInvoque", null, objGetInvoque.idProy);
            objLog.CrearArchivolog("Inicio metodo GetInvoque ", null, null);
            objLog.CrearArchivolog(string.Format("Parametros de URL: " + objGetInvoque.parametroUrl), "", null);
            HttpWebRequest request = HttpWebRequest.Create(String.Format("{0}{1}", ConfigurationManager.AppSettings[objGetInvoque.name_Url], objGetInvoque.parametroUrl)) as HttpWebRequest;
            //System.Configuration.ConfigurationManager.AppSettings[Funciones.CheckStr(objGetInvoque.name_Url)]
            request.Method = "GET";
            request.Headers = GetHeadersDP.GetHeaders(objHeader,
                                                      objGetInvoque.usuario,
                                                      objGetInvoque.ipServidor,
                                                      objGetInvoque.idProy,
                                                      objGetInvoque.strWSIP,
                                                      objGetInvoque.UserEncrypted,
                                                      objGetInvoque.PassEncrypted);
            request.Accept = "application/json";
            request.Timeout = Convert.ToInt32(objGetInvoque.TimeoutUrl);
            objLog.CrearArchivolog(string.Format("Valor de TimeOut: " + request.Timeout), "", null);
            //request.ContentType = "application/json";

            WebResponse ws = request.GetResponse();

            using (Stream stream = ws.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                String responseString = reader.ReadToEnd();
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[GetInvoque<T>][responseString]", Funciones.CheckStr(responseString)), null, null);
                T result = JsonConvert.DeserializeObject<T>(responseString);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[FIN][GetInvoque<T>]", string.Empty), null, null);
                return result;
            }
        }
    }
}
