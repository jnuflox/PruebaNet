using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using Claro.SISACT.Entity;
using Claro.SISACT.Common;
using Claro.SISACT.WS.WSEnvioCorreo;

namespace Claro.SISACT.WS 
{
    public class BWEnvioCorreo //PROY-24724-IDEA-28174 - NUEVA CLASE
    {
        envioCorreoWSService _objTransaccion = new envioCorreoWSService();

        public BWEnvioCorreo()
        {
            _objTransaccion.Url = ConfigurationManager.AppSettings["consEnvioCorreoWS_URL"].ToString();
            _objTransaccion.Credentials = System.Net.CredentialCache.DefaultCredentials;
            _objTransaccion.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["consEnvioCorreoWS_TimeOut"].ToString());
        }

        public BEItemMensaje EnviarCorreo(string strRemitente, string strDestinatario, string strAsunto, string strMensaje, string strHtmlFlag, BEItemGenerico objAudit)
        {
            BEItemMensaje objMensaje = new BEItemMensaje(false);

            WSEnvioCorreo.AuditTypeResponse objResponse = new AuditTypeResponse();
            WSEnvioCorreo.AuditTypeRequest objRequest = new AuditTypeRequest();
            WSEnvioCorreo.ParametroOpcionalComplexType[] parametrosOpcionalesResponse = new WSEnvioCorreo.ParametroOpcionalComplexType[0];
            WSEnvioCorreo.ParametroOpcionalComplexType[] parametrosOpcionalesRequest = new WSEnvioCorreo.ParametroOpcionalComplexType[0];

            objRequest.idTransaccion = objAudit.Codigo;
            objRequest.usrAplicacion = objAudit.Codigo2;
            objRequest.codigoAplicacion = objAudit.Descripcion;
            objRequest.ipAplicacion = objAudit.Descripcion2;

            objResponse = _objTransaccion.enviarCorreo(objRequest,
                                                        strRemitente,
                                                        strDestinatario,
                                                        strAsunto,
                                                        strMensaje,
                                                        strHtmlFlag,
                                                        parametrosOpcionalesRequest,
                                                        out parametrosOpcionalesResponse);

            objMensaje.codigo = objResponse.codigoRespuesta.ToString();
            objMensaje.descripcion = objResponse.mensajeRespuesta.ToString();

            if (objMensaje.codigo == "0") objMensaje.exito = true;

            return objMensaje;
        }
    }
}
