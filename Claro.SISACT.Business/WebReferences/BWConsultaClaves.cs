//PROY-140546
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Business.WSConsultaClave;
using System.Configuration;

namespace Claro.SISACT.WebReferences
{
    public class BWConsultaClaves
    {
        ebsConsultaClavesService consultaClaves = new ebsConsultaClavesService();

        public BWConsultaClaves()
        {
            consultaClaves.Url = ConfigurationManager.AppSettings["consRutaWSConsultaClaves"];
// Setting client credentials using ClientCredentials property if available, or using appropriate credential method
            if (consultaClaves.GetType().GetProperty("ClientCredentials") != null)
            {
                var clientCredentialsProperty = consultaClaves.GetType().GetProperty("ClientCredentials");
                clientCredentialsProperty?.SetValue(consultaClaves, System.Net.CredentialCache.DefaultCredentials);
            }
            else if (consultaClaves.GetType().GetProperty("UseDefaultCredentials") != null)
            {
                var useDefaultCredentialsProperty = consultaClaves.GetType().GetProperty("UseDefaultCredentials");
                useDefaultCredentialsProperty?.SetValue(consultaClaves, true);
            }

            // Handle timeout setting - try different approaches for compatibility with .NET 8
            int timeoutValue = Convert.ToInt32(ConfigurationManager.AppSettings["TimeoutWS"]);
            var timeoutProperty = consultaClaves.GetType().GetProperty("Timeout");
            if (timeoutProperty != null)
            {
                timeoutProperty.SetValue(consultaClaves, timeoutValue);
            }
            else
            {
                // Try setting timeout through endpoint behavior if available
                var endpointProperty = consultaClaves.GetType().GetProperty("Endpoint");
                if (endpointProperty != null)
                {
                    var endpoint = endpointProperty.GetValue(consultaClaves);
                    var bindingProperty = endpoint?.GetType().GetProperty("Binding");
                    if (bindingProperty != null)
                    {
                        var binding = bindingProperty.GetValue(endpoint);
                        var sendTimeoutProperty = binding?.GetType().GetProperty("SendTimeout");
                        if (sendTimeoutProperty != null)
                        {
                            sendTimeoutProperty.SetValue(binding, TimeSpan.FromMilliseconds(timeoutValue));
                        }
                    }
                }
            }
        }

        public string ConsultaClavesWS(string idTransaccion,
                                               string ipAplicacion,
                                               string ipTransicion,
                                               string usrAplicacion,
                                               string idAplicacion,
                                               string codigoAplicacion,
                                               string usuarioAplicacionEncriptado,
                                               string claveEncriptado,
                                               out string mensajeResultado,
                                               out string usuarioAplicacion,
                                               out string clave)
        {
            string strCodigoResultado = null;
            try
            {

                strCodigoResultado = consultaClaves.desencriptar(ref idTransaccion,
                                                            ipAplicacion,
                                                            ipTransicion,
                                                            usrAplicacion,
                                                            idAplicacion,
                                                            codigoAplicacion,
                                                            usuarioAplicacionEncriptado,
                                                            claveEncriptado,
                                                            out mensajeResultado,
                                                            out usuarioAplicacion,
                                                            out clave);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strCodigoResultado;
        }
    }
}