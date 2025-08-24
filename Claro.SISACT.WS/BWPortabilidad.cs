using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using Claro.SISACT.Entity;
using Claro.SISACT.Common;
using Claro.SISACT.WS.WSPortabilidad;

namespace Claro.SISACT.WS
{
    public class BWPortabilidad
    {
        EbsValidacionService _objTransaccion = new EbsValidacionService();

        public BWPortabilidad()
		{
            _objTransaccion.Url = ConfigurationManager.AppSettings["RutaWS_Portabilidad"].ToString();
            _objTransaccion.Credentials = System.Net.CredentialCache.DefaultCredentials;
            _objTransaccion.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["TimeOut_Portabilidad"].ToString());
		}

        public BEItemMensaje validaEstadoPortaLista(string[] lstTelefono)
        {
            BEItemMensaje objMensaje = new BEItemMensaje(false);
            foreach (string telefono in lstTelefono)
            {
                if (!string.IsNullOrEmpty(telefono))
                {
                    objMensaje = validaEstadoPorta(telefono);
                    if (!objMensaje.exito) break;
                }
            }

            return objMensaje;
        }

        public BEItemMensaje validaEstadoPorta(string telefono)
        {
            BEItemMensaje objMensaje = new BEItemMensaje(false);
            try
            {
                ValidacionResponse objResponse = new ValidacionResponse();
                ValidacionRequest objRequest = new ValidacionRequest();

                objRequest.telefono = telefono;
                objResponse = _objTransaccion.validarEstado(objRequest);

                objMensaje.codigo = objResponse.codigo.ToString();
                objMensaje.descripcion = objResponse.descripcion;
                objMensaje.id = telefono;

                if (objMensaje.codigo == "0") objMensaje.exito = true;
            }
            catch (Exception ex)
            {
                objMensaje.codigo = ex.Source;
                objMensaje.mensajeSistema = ex.Message;
                objMensaje.mensajeCliente = string.Format(ConfigurationManager.AppSettings["consMsjErrorTelefonoPorta"].ToString(), telefono);
            }
            finally
            {
                _objTransaccion.Dispose();
            }

            return objMensaje;
        }
    }
}
