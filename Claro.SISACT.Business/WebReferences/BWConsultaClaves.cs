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
            consultaClaves.Credentials = System.Net.CredentialCache.DefaultCredentials;
            consultaClaves.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["TimeoutWS"]);
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
