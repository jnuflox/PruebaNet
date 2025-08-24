using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.WS.WSGestionaProteccionMovil;
using System.Configuration;
using Claro.SISACT.Entity;

namespace Claro.SISACT.WS 
{
    public class BWGestionaProteccionMovil //PROY-24724-IDEA-28174 - NUEVA CLASE
    {
        GestionaProteccionMovilWSService _objTransaccion = new GestionaProteccionMovilWSService();

        public BWGestionaProteccionMovil()
        {
            _objTransaccion.Url = ConfigurationManager.AppSettings["consGestionaProteccionMovilWS_URL"].ToString();
            _objTransaccion.Credentials = System.Net.CredentialCache.DefaultCredentials;
            _objTransaccion.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["consGestionaProteccionMovilWS_TimeOut"].ToString());
        }

        public BEItemMensaje GuardarProteccionMovil(BEPrima objGuardaPrima, BEItemGenerico objAudit)
        {
            BEItemMensaje objMensaje = new BEItemMensaje(false);
            

            auditRequestType objRequestAudit = new auditRequestType();
            objRequestAudit.idTransaccion = objAudit.Codigo;
            objRequestAudit.nombreAplicacion = objAudit.Descripcion;
            objRequestAudit.usuarioAplicacion = objAudit.Codigo2;
            objRequestAudit.ipAplicacion = objAudit.Descripcion2;

            guardarPrimaRequest objRequest = new guardarPrimaRequest();
            guardarPrimaResponse objResponse = new guardarPrimaResponse();

            objRequest.auditRequest = objRequestAudit;
            objRequest.codMaterialReq = objGuardaPrima.CodMaterial;
            objRequest.deducibleDanio = objGuardaPrima.DeducibleDanio;
            objRequest.deducibleRobo = objGuardaPrima.DeducibleRobo;
            objRequest.descProdRpta = objGuardaPrima.DescProd;
            objRequest.descProtReq = objGuardaPrima.DescProt;
            objRequest.fechaEvaluacion = objGuardaPrima.FechaEvaluacion;
            objRequest.fechaModif = objGuardaPrima.FechaModif;
            objRequest.flagEstado = objGuardaPrima.FlagEstado;
            objRequest.incidenciaTipoDanio = objGuardaPrima.IncidenciaTipoDanio;
            objRequest.incidenciaTipoRobo = objGuardaPrima.IncidenciaTipoRobo;
            objRequest.montoPrimaRpta = objGuardaPrima.MontoPrima;
            objRequest.nombreProdRpta = objGuardaPrima.NombreProd;
            objRequest.nroCertifRpta = objGuardaPrima.NroCertif;
            objRequest.nroDocReq = objGuardaPrima.NroDoc;
            objRequest.nroSec = objGuardaPrima.NroSec;
            objRequest.resultadoRpta = objGuardaPrima.Resultado;
            objRequest.soplnCodigo = objGuardaPrima.SoplnCodigo;
            objRequest.tipoClienteReq = objGuardaPrima.TipoCliente;
            objRequest.tipoDocReq = objGuardaPrima.TipoDoc;
            objRequest.tipoOperacion = objGuardaPrima.TipoOperacion;
            objRequest.usrMod = objGuardaPrima.UsrMod;

            objResponse = _objTransaccion.guardarPrima(objRequest);

            objMensaje.codigo = objResponse.auditResponse.codigoRespuesta;
            objMensaje.descripcion = objResponse.auditResponse.mensajeRespuesta;

            if (objMensaje.codigo == "0") objMensaje.exito = true;
            else objMensaje.mensajeCliente = ConfigurationManager.AppSettings["consGestionaProteccionMovilWS_Error"].ToString();

            _objTransaccion.Dispose();

            return objMensaje;
        }

        public BEItemMensaje BuscarProteccionMovil(String strNroSEC, BEItemGenerico objAudit, ref ObjetoPrimaType[] arrObjPrima)
        {
            BEItemMensaje objMensaje = new BEItemMensaje(false);

            auditRequestType objRequestAudit = new auditRequestType();
            objRequestAudit.idTransaccion = objAudit.Codigo;
            objRequestAudit.nombreAplicacion = objAudit.Descripcion;
            objRequestAudit.usuarioAplicacion = objAudit.Codigo2;
            objRequestAudit.ipAplicacion = objAudit.Descripcion2;

            buscarPrimaRequest objRequest = new buscarPrimaRequest();
            buscarPrimaResponse objResponse = new buscarPrimaResponse();

            objRequest.auditRequest = objRequestAudit;
            objRequest.sec = strNroSEC;
            objRequest.certificado = string.Empty;
            objRequest.solicitud = string.Empty;

            objResponse = _objTransaccion.buscarPrima(objRequest);
            
            objMensaje.codigo = objResponse.auditResponse.codigoRespuesta;
            objMensaje.descripcion = objResponse.auditResponse.mensajeRespuesta;
            arrObjPrima = objResponse.listaDatosPrima;

            if (objMensaje.codigo == "0") objMensaje.exito = true;
            else objMensaje.mensajeCliente = ConfigurationManager.AppSettings["consGestionaProteccionMovilWS_Error"].ToString();
            
            _objTransaccion.Dispose();

            return objMensaje;
        }
    }
}
