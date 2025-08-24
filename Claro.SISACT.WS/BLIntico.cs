using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity.Intico;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Configuration;

//JRM - INTICO - INI
namespace Claro.SISACT.WS
{
    public class BLIntico
    {
        public GeneraTokenResponse GeneraTokem() 
        {
            GeneraTokenResponse objReponse = new GeneraTokenResponse();
            string urlGeneraTokenIntico = ConfigurationManager.AppSettings["urlGeneraTokenIntico"].ToString();
            HttpWebRequest http = (HttpWebRequest)HttpWebRequest.Create(urlGeneraTokenIntico);
            http.ContentType = "application/json";
            http.Method = "POST";

            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

            string client_id = ConfigurationManager.AppSettings["client_id_Intico"].ToString();
            string client_secret = ConfigurationManager.AppSettings["client_secret_Intico"].ToString();

            using (var streamWriter = new StreamWriter(http.GetRequestStream()))
            {
                string json = "{\"client_id\":\"" + client_id + "\"," +
                              "\"client_secret\":\"" + client_secret + "\"}";
                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)http.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                objReponse = JsonConvert.DeserializeObject<GeneraTokenResponse>(result);
            }

            return objReponse;
        }

        public RemueveTokenResponse RemueveTokem(string token) 
        {
            RemueveTokenResponse objReponse = new RemueveTokenResponse();
            string urlRemueveTokenIntico = ConfigurationManager.AppSettings["urlRemueveTokenIntico"].ToString();
            HttpWebRequest http = (HttpWebRequest)HttpWebRequest.Create(urlRemueveTokenIntico);
            http.ContentType = "application/json";
            http.Method = "POST";

            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

            using (var streamWriter = new StreamWriter(http.GetRequestStream()))
            {
                string json = "{\"access_token\":\""+ token +"\"}";
                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)http.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                objReponse = JsonConvert.DeserializeObject<RemueveTokenResponse>(result);
            }

            return objReponse;

        }

    }
}
//JRM - INTICO - FIN
