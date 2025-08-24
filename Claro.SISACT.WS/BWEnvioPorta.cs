using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using Claro.SISACT.Entity;
using Claro.SISACT.Common;
using Claro.SISACT.WS.WSEnvioPorta;

namespace Claro.SISACT.WS
{
    public class BWEnvioPorta
    {
        EnvioPortaWSService _objTransaccion = new EnvioPortaWSService();

        public BWEnvioPorta()
        {
            _objTransaccion.Url = ConfigurationManager.AppSettings["RutaWS_EnvioPorta"].ToString();
            _objTransaccion.Credentials = System.Net.CredentialCache.DefaultCredentials;
            _objTransaccion.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["TimeOut_EnvioPorta"].ToString());
        }

        public BEItemMensaje realizarConsultaPrevia(string numeroSec, string observaciones, BEItemGenerico objAudit)
        {
            BEItemMensaje objMensaje = new BEItemMensaje(false);
            try
            {
                realizarConsultaPreviaResponse objResponse = new realizarConsultaPreviaResponse();
                realizarConsultaPreviaRequest objRequest = new realizarConsultaPreviaRequest();

                auditRequestType objRequestAudit = new auditRequestType();
                objRequestAudit.idTransaccion = objAudit.Codigo;
                objRequestAudit.ipAplicacion = objAudit.Descripcion2;
                objRequestAudit.nombreAplicacion = objAudit.Descripcion;
                objRequestAudit.usuarioAplicacion = objAudit.Codigo2;
                objRequest.auditRequest = objRequestAudit;

                objRequest.numeroSec = numeroSec;
                objRequest.flagCP = string.Empty;
                objRequest.observaciones = observaciones;
                objRequest.id = string.Empty;

                objRequest.tipoPort = "mm";
                objRequest.nombreHost = System.Net.Dns.GetHostName();
                objRequest.nombreServidor = System.Net.Dns.GetHostName();
                objRequest.ipServidor = objAudit.Descripcion2;
                objRequest.codigoAplicacion = objAudit.Codigo3;

                //INI: PROY-BLACKOUT

                if (objAudit.Estado.Equals("1"))
                {
                    objMensaje.codigo = "0";
                    objMensaje.descripcion = objAudit.Valor;        
                }
                else
                {                   
                objResponse = _objTransaccion.realizarConsultaPrevia(objRequest);
                objMensaje.codigo = objResponse.auditResponse.codigoRespuesta.ToString();
                objMensaje.descripcion = objResponse.auditResponse.mensajeRespuesta.ToString();
                }
               
                if (objMensaje.codigo == "0") objMensaje.exito = true;

                //FIN: PROY-BLACKOUT
            }
            catch (Exception ex)
            {
                objMensaje.codigo = ex.Source;
                objMensaje.mensajeSistema = ex.Message;
                
            }
            finally
            {
                _objTransaccion.Dispose();
            }
            return objMensaje;
        }

    }
}
