using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.WS.WSClienteProteccionMovil;
using System.Configuration;
using Claro.SISACT.Entity;

namespace Claro.SISACT.WS
{
    public class BWClienteProteccionMovil //PROY-24724-IDEA-28174 - NUEVA CLASE
    {
        ClienteProteccionMovilWSService _objTransaccion = new ClienteProteccionMovilWSService();

        public BWClienteProteccionMovil()
        {
            _objTransaccion.Url = ConfigurationManager.AppSettings["consClienteProteccionMovilWS_URL"].ToString();
            _objTransaccion.Credentials = System.Net.CredentialCache.DefaultCredentials;
            _objTransaccion.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["consClienteProteccionMovilWS_TimeOut"].ToString());
        }

        public BEItemMensaje ConsultarPrima(String strDescOferta, String strTipoDocumento, String strNroDocumento, String strCodMaterial, String strDescServProteccionMovil, BEItemGenerico objAudit, ref BEPrima objBEPrima)
        {
            BEItemMensaje objMensaje = new BEItemMensaje(false);
            consultarPrimaRequest objRequest = new consultarPrimaRequest();
            consultarPrimaResponse objResponse = new consultarPrimaResponse();
            IdentificationType objIdentificationType = new IdentificationType();

            objIdentificationType.idType = strTipoDocumento;
            objIdentificationType.idValue = strNroDocumento;

            AuditRequestType objRequestAudit = new AuditRequestType();
            objRequestAudit.aplicacion = objAudit.Descripcion;
            objRequestAudit.idTransaccion = objAudit.Codigo;
            objRequestAudit.ipAplicacion = objAudit.Descripcion2;
            objRequestAudit.usrAplicacion = objAudit.Codigo2;

            objRequest.auditRequest = objRequestAudit;
            objRequest.accountType = strDescOferta;
            objRequest.identificationType = objIdentificationType;
            objRequest.clientProductSKU = strDescServProteccionMovil;
            objRequest.clientAssetSKU = strCodMaterial;

            objResponse = _objTransaccion.consultarPrima(objRequest);

            objMensaje.codigo = objResponse.auditResponse.codigoRespuesta;
            objMensaje.descripcion = objResponse.auditResponse.mensajeRespuesta;

            if (objMensaje.codigo.Equals("0"))
            {
                objMensaje.exito = true;
                objBEPrima.MontoPrima = objResponse.products[0].premium;
                objBEPrima.NombreProd = objResponse.products[0].productName;
                objBEPrima.DescProd = objResponse.products[0].productDescription;
                objBEPrima.NroCertif = objResponse.quoteID;
                objBEPrima.IncidenciaTipoDanio = objResponse.products[0].incidentType;
                objBEPrima.IncidenciaTipoRobo = objResponse.products[1].incidentType;
                string deducibleRobo = objResponse.products[1].deductible;
                string deducibleDanioFalla = objResponse.products[0].deductible;
                deducibleDanioFalla = deducibleDanioFalla.Replace('n','ñ');
                int intSpace = deducibleDanioFalla.IndexOf(" ");
	 	if (intSpace >= 0) deducibleDanioFalla = deducibleDanioFalla.Substring(0, intSpace) + deducibleDanioFalla.Substring(intSpace + 1);
                objBEPrima.DeducibleDanio = deducibleRobo + " - " + deducibleDanioFalla;
            }
            return objMensaje;
        }
    }
}
