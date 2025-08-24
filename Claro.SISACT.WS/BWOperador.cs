using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using Claro.SISACT.Entity;
using Claro.SISACT.Common;
using Claro.SISACT.WS.WSOperador;

namespace Claro.SISACT.WS
{
    public class BWOperador
    {
        EbsIdentificaOperadorService _objTransaccion = new EbsIdentificaOperadorService();

        public BWOperador()
        {
            _objTransaccion.Url = ConfigurationManager.AppSettings["RutaWS_Operador"].ToString();
            _objTransaccion.Credentials = System.Net.CredentialCache.DefaultCredentials;
            _objTransaccion.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["TimeOut_Operador"].ToString());
        }

        private BEItemMensaje validaTelefonoClaroLista(string[] lstTelefono)
        {
            BEItemMensaje objMensaje = new BEItemMensaje(false);
            foreach (string telefono in lstTelefono)
            {
                if (!string.IsNullOrEmpty(telefono))
                {
                    objMensaje = validaTelefonoClaro(telefono);
                    if (!objMensaje.exito) break;
                }
            }

            return objMensaje;
        }

        private BEItemMensaje validaTelefonoClaro(string telefono)
        {
            BEItemMensaje objMensaje = new BEItemMensaje(false);
            try
            {
                linea[] objResponse = null;
                IdentificadorOperadorRequest objRequest = new IdentificadorOperadorRequest();

                objRequest.Login = "";
                objRequest.Sistema = ConfigurationManager.AppSettings["constNombreAplicacion"].ToString();
                objRequest.TelefonoTriado = telefono;
                objRequest.TelefonoUsuario = telefono;

                objResponse = _objTransaccion.buscarOperadorLineas(objRequest);

                if (objResponse != null && objResponse.Length > 0)
                {
                    objMensaje.codigo = objResponse[0].CodRpta;
                    objMensaje.descripcion = objResponse[0].DesRpta;

                    if (objMensaje.codigo == "0") objMensaje.exito = true;
                }
            }
            catch (Exception ex)
            {
                objMensaje.codigo = ex.Source;
                objMensaje.mensajeSistema = ex.Message;
                objMensaje.mensajeCliente = string.Format(ConfigurationManager.AppSettings["consMsjErrorTelefonoPortaII"].ToString(), telefono);
            }
            finally
            {
                _objTransaccion.Dispose();
            }

            return objMensaje;
        }

        //INI: INICIATIVA 941 
        public bool validaLineaClaro(string telefono)
        {
            bool esLineaClaro = false;
            IdentificadorOperadorRequest validaLineaRequest = null;
            linea[] oNumeros = new linea[1];
            try
            {
                validaLineaRequest = new IdentificadorOperadorRequest();
                validaLineaRequest.TelefonoUsuario = telefono;
                validaLineaRequest.TelefonoTriado = telefono;
                validaLineaRequest.Login = "";
                validaLineaRequest.Sistema = ConfigurationManager.AppSettings["constNombreAplicacion"].ToString();
                oNumeros = _objTransaccion.buscarOperadorLineas(validaLineaRequest);
                if (oNumeros != null)
                {
                    string[] arrLineaActiva = Convert.ToString(ConfigurationManager.AppSettings["P_MSJ_ValidacionOperador"]).Split(Convert.ToChar(ConfigurationManager.AppSettings["P_SEPARADOR_COMA"]));
                    if ((oNumeros.ElementAt(0).Operador == arrLineaActiva[0]) || (oNumeros.ElementAt(0).Operador == arrLineaActiva[1]) || (oNumeros.ElementAt(0).Operador == arrLineaActiva[2]))
                    {
                        esLineaClaro = true;
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _objTransaccion.Dispose();
            }
            return esLineaClaro;

        }
        //FIN: INICIATIVA 941 
    }
}
