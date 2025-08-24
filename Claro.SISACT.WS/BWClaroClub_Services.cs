using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using Claro.SISACT.Common;
using Claro.SISACT.Entity;

namespace Claro.SISACT.WS
{
    public class BWClaroClub_Services
    {
        public BWClaroClub_Services() { }

        public ConsultarPuntosWS.consultarPuntosResponse consultarPuntosClaroClub(String tipoDoc, String numDoc, String codigoCliente, BEItemGenerico oAudit)
        {
            string idLog = "consultarPuntosClaroClub";

            ConsultarPuntosWS.ebsConsultaPuntosClaroClubService oConsultarPuntosWS = new ConsultarPuntosWS.ebsConsultaPuntosClaroClubService();
            oConsultarPuntosWS.Url = ConfigurationManager.AppSettings["WSConsultaPuntosCC_url"];
            oConsultarPuntosWS.Timeout = Funciones.CheckInt(ConfigurationManager.AppSettings["WSConsultaPuntosCC_tiemout"]);
            oConsultarPuntosWS.Credentials = System.Net.CredentialCache.DefaultCredentials;

            GeneradorLog.EscribirLog("LogClaroPunto", idLog, "Parametros ->");
            GeneradorLog.EscribirLog("LogClaroPunto", idLog, "Url: " + oConsultarPuntosWS.Url);
            GeneradorLog.EscribirLog("LogClaroPunto", idLog, "tipoDoc: " + Funciones.CheckStr(tipoDoc));
            GeneradorLog.EscribirLog("LogClaroPunto", idLog, "numDoc: " + Funciones.CheckStr(numDoc));
            GeneradorLog.EscribirLog("LogClaroPunto", idLog, "codigoCliente:" + Funciones.CheckStr(codigoCliente));
            

            ConsultarPuntosWS.consultarPuntosRequest objConsultarPuntosRequest = new ConsultarPuntosWS.consultarPuntosRequest();
            ConsultarPuntosWS.consultarPuntosResponse objConsultarPuntosResponse = new ConsultarPuntosWS.consultarPuntosResponse();            
            objConsultarPuntosRequest.idTransaccion = oAudit.Codigo;
            objConsultarPuntosRequest.ipAplicacion = oAudit.Codigo2;
            objConsultarPuntosRequest.aplicacion = oAudit.Descripcion;
            objConsultarPuntosRequest.usuarioAplicacion = oAudit.Descripcion2;

            objConsultarPuntosRequest.tipoDoc = tipoDoc;
            objConsultarPuntosRequest.numDoc = numDoc;
            objConsultarPuntosRequest.codigoCliente = codigoCliente;

                GeneradorLog.EscribirLog("LogClaroPunto", idLog, "Inicio [oConsultarPuntosWS.consultarPuntosClaroClub]");                
                objConsultarPuntosResponse = oConsultarPuntosWS.consultarPuntosClaroClub(objConsultarPuntosRequest);
                GeneradorLog.EscribirLog("LogClaroPunto", idLog, "Fin [oConsultarPuntosWS.consultarPuntosClaroClub]");


            return objConsultarPuntosResponse;
        }

        //PROY-24740
        public void bloquearPuntos(String tipoDoc, String numDoc, String tipoClie, String usuario, BEItemGenerico oAudit, 
            ref String txId, ref String errorCode, ref String errorMsg)
        {
            string idLog = string.Format("bloquearPuntos[{0}]", Funciones.CheckStr(usuario));

            try
            {
                WSPuntosClaroClub.ebsGestionarPuntosService oWSPuntosClaroClub = new WSPuntosClaroClub.ebsGestionarPuntosService();
                oWSPuntosClaroClub.Url = ConfigurationManager.AppSettings["WSPuntosClaroClub_url"];
                oWSPuntosClaroClub.Timeout = Funciones.CheckInt(ConfigurationManager.AppSettings["WSPuntosClaroClub_timeout"]);
                oWSPuntosClaroClub.Credentials = System.Net.CredentialCache.DefaultCredentials;


                GeneradorLog.EscribirLog("LogClaroPunto", idLog, "Parametros ->");
                GeneradorLog.EscribirLog("LogClaroPunto", idLog, string.Format("Url: {0}", oWSPuntosClaroClub.Url));
                GeneradorLog.EscribirLog("LogClaroPunto", idLog, string.Format("tipoDoc: {0}", Funciones.CheckStr(tipoDoc)));
                GeneradorLog.EscribirLog("LogClaroPunto", idLog, string.Format("numDoc: {0}", Funciones.CheckStr(numDoc)));
                GeneradorLog.EscribirLog("LogClaroPunto", idLog, string.Format("tipoClie:{0}", Funciones.CheckStr(tipoClie)));
                GeneradorLog.EscribirLog("LogClaroPunto", idLog, string.Format("usuario:{0}", Funciones.CheckStr(usuario)));
                GeneradorLog.EscribirLog("LogClaroPunto", idLog, string.Format("txId:{0}", Funciones.CheckStr(txId)));
                


                WSPuntosClaroClub.bloquearPuntosRequest objBloquearPuntosRequest = new WSPuntosClaroClub.bloquearPuntosRequest();
                WSPuntosClaroClub.bloquearPuntosResponse objBloquearPuntosResponse = new WSPuntosClaroClub.bloquearPuntosResponse();
                objBloquearPuntosRequest.idTransaccion = oAudit.Codigo;
                objBloquearPuntosRequest.ipAplicacion = oAudit.Codigo2;
                objBloquearPuntosRequest.aplicacion = oAudit.Descripcion;
                objBloquearPuntosRequest.usuarioAplicacion = oAudit.Descripcion2;

                objBloquearPuntosRequest.tipoDoc = tipoDoc;
                objBloquearPuntosRequest.numDoc = numDoc;
                objBloquearPuntosRequest.tipoClie = tipoClie;
                objBloquearPuntosRequest.usuario = usuario; 

                GeneradorLog.EscribirLog("LogClaroPunto", idLog, "Inicio [oWSPuntosClaroClub.bloquearPuntos] ");
                objBloquearPuntosResponse = oWSPuntosClaroClub.bloquearPuntos(objBloquearPuntosRequest);
                GeneradorLog.EscribirLog("LogClaroPunto", idLog, "Fin [oWSPuntosClaroClub.bloquearPuntos] ");
                txId = objBloquearPuntosResponse.audit.txId;
                errorCode = objBloquearPuntosResponse.audit.errorCode;
                errorMsg = objBloquearPuntosResponse.audit.errorMsg;

                GeneradorLog.EscribirLog("LogClaroPunto", idLog, string.Format("Fin [oWSPuntosClaroClub.bloquearPuntos] txId: {0}", Funciones.CheckStr(txId)));
                GeneradorLog.EscribirLog("LogClaroPunto", idLog, string.Format("Fin [oWSPuntosClaroClub.bloquearPuntos] errorCode: {0}", Funciones.CheckStr(errorCode)));
                GeneradorLog.EscribirLog("LogClaroPunto", idLog, string.Format("Fin [oWSPuntosClaroClub.bloquearPuntos] errorMsg: {0}", Funciones.CheckStr(errorMsg)));
            }
            catch (Exception ex)
            {
                GeneradorLog.EscribirLog("LogClaroPunto", idLog, string.Format("Error [oWSPuntosClaroClub.bloquearPuntos]:{0}", ex.Message));
                
                throw ex;
            }
        }

        public void liberarPuntos(String tipoDoc, String numDoc, String tipoClie, BEItemGenerico oAudit, ref String txId, ref String errorCode, ref String errorMsg)
        {
            string idLog = "liberarPuntos[]";
            try
            {

                WSPuntosClaroClub.ebsGestionarPuntosService oWSPuntosClaroClub = new WSPuntosClaroClub.ebsGestionarPuntosService();
                oWSPuntosClaroClub.Url = ConfigurationManager.AppSettings["WSPuntosClaroClub_url"];
                oWSPuntosClaroClub.Timeout = Funciones.CheckInt(ConfigurationManager.AppSettings["WSPuntosClaroClub_timeout"]);
                oWSPuntosClaroClub.Credentials = System.Net.CredentialCache.DefaultCredentials;

                WSPuntosClaroClub.liberarPuntosRequest objLiberarPuntosRequest = new WSPuntosClaroClub.liberarPuntosRequest();
                WSPuntosClaroClub.liberarPuntosResponse objLiberarPuntosResponse = new WSPuntosClaroClub.liberarPuntosResponse();
                objLiberarPuntosRequest.idTransaccion = oAudit.Codigo;
                objLiberarPuntosRequest.ipAplicacion = oAudit.Codigo2;
                objLiberarPuntosRequest.aplicacion = oAudit.Descripcion;
                objLiberarPuntosRequest.usuarioAplicacion = oAudit.Descripcion2;

                objLiberarPuntosRequest.tipoDoc = tipoDoc;
                objLiberarPuntosRequest.numDoc = numDoc;
                objLiberarPuntosRequest.tipoClie = tipoClie;

                GeneradorLog.EscribirLog("LogClaroPunto", idLog, "Parametros IN ->");
                GeneradorLog.EscribirLog("LogClaroPunto", idLog, "Url: " + oWSPuntosClaroClub.Url);
                GeneradorLog.EscribirLog("LogClaroPunto", idLog, "tipoDoc: " + Funciones.CheckStr(tipoDoc));
                GeneradorLog.EscribirLog("LogClaroPunto", idLog, "numDoc: " + Funciones.CheckStr(numDoc));
                GeneradorLog.EscribirLog("LogClaroPunto", idLog, "tipoClie:" + Funciones.CheckStr(tipoClie));                
                GeneradorLog.EscribirLog("LogClaroPunto", idLog, "txId:" + Funciones.CheckStr(txId));

                GeneradorLog.EscribirLog("LogClaroPunto", idLog, "Inicio [oWSPuntosClaroClub.liberarPuntos] ");
                objLiberarPuntosResponse = oWSPuntosClaroClub.liberarPuntos(objLiberarPuntosRequest);
                GeneradorLog.EscribirLog("LogClaroPunto", idLog, "Fin [oWSPuntosClaroClub.liberarPuntos] ");

                txId = objLiberarPuntosResponse.audit.txId;
                errorCode = objLiberarPuntosResponse.audit.errorCode;
                errorMsg = objLiberarPuntosResponse.audit.errorMsg;

                GeneradorLog.EscribirLog("LogClaroPunto", idLog, "Parametros OUT ->");
                GeneradorLog.EscribirLog("LogClaroPunto", idLog, "txId:" + Funciones.CheckStr(txId));
                GeneradorLog.EscribirLog("LogClaroPunto", idLog, "errorCode:" + Funciones.CheckStr(errorCode));
                GeneradorLog.EscribirLog("LogClaroPunto", idLog, "errorMsg:" + Funciones.CheckStr(errorMsg));

            }
            catch (Exception ex)
            {
                GeneradorLog.EscribirLog("LogClaroPunto", idLog, "Error [oWSPuntosClaroClub.liberarPuntos]:" + ex.Message);
                throw ex;
            }
        }
    }
}
