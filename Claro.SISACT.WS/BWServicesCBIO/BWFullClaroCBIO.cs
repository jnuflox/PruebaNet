using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity.claro_vent_fullclaroCBIO;
using Claro.SISACT.Entity.claro_vent_fullclaroCBIO.consultarActivosCBIO;
using Claro.SISACT.Common;
using Claro.SISACT.Entity;
using System.Collections;
using System.Configuration;
using Claro.SISACT.WS.RestServices;
using Claro.SISACT.Entity.claro_vent_fullclaroCBIO.consultarPendientesActivasCBIO;

namespace Claro.SISACT.WS.BWServicesCBIO
{
    public class BWFullClaroCBIO
    {
        public ConsultarActivosCBIOResponse ValidarCandidatoFullClaroWSCBIO(Request objRequest)
        {
            GeneradorLog objLog = new GeneradorLog(null, string.Format("{0}{1}", "[INICIO][INICIATIVA-710][ValidarCandidatoFullClaroWSCBIO]", string.Empty), null, null);
            ConsultarActivosCBIOResponse objResponse = new ConsultarActivosCBIOResponse();
            BEAuditoriaRequest objAuditoria = new BEAuditoriaRequest();
            Hashtable datosHeader = new Hashtable();

            try
            {
                #region Header
                objAuditoria.urlRest = "UrlValidarCandidatoFullClaroWS";
                objAuditoria.urlTimeOut_Rest = "TimeOutValidarCandidatoFullClaroWS";
                objAuditoria.dataPower = false;

                datosHeader.Add("idTransaccion", DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                datosHeader.Add("msgId", DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                datosHeader.Add("userId", Funciones.CheckStr(ConfigurationManager.AppSettings["constAplicacion"]));
                datosHeader.Add("timestamp", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"));
                objAuditoria.table = datosHeader;
                #endregion

                #region Body
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-710][ValidarCandidatoFullClaroWSCBIO][RequestBody][tipoDocumento]", Funciones.CheckStr(objRequest.consultarActivosCBIORequest.tipoDocumento)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-710][ValidarCandidatoFullClaroWSCBIO][RequestBody][numeroDocumento]", Funciones.CheckStr(objRequest.consultarActivosCBIORequest.numeroDocumento)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-710][ValidarCandidatoFullClaroWSCBIO][RequestBody][tipoServicio]", Funciones.CheckStr(objRequest.consultarActivosCBIORequest.tipoServicio)), null, null);
                #endregion

                objLog.CrearArchivolog(string.Format("{0} : {1} | {2} : {3}", "[INICIATIVA-710][ValidarCandidatoFullClaroWSCBIO][NameKeyWS]", objAuditoria.urlRest, "[ValorKeyWS]", Funciones.CheckStr(ConfigurationManager.AppSettings[objAuditoria.urlRest])), null, null);
                objLog.CrearArchivolog(string.Format("{0} : {1} | {2} : {3}", "[INICIATIVA-710][ValidarCandidatoFullClaroWSCBIO][NameTimeOutWS]", objAuditoria.urlTimeOut_Rest, "[ValorTimeOutWS]", Funciones.CheckStr(ConfigurationManager.AppSettings[objAuditoria.urlTimeOut_Rest])), null, null);

                string strIdTransaccion = string.Empty;
                string strCodigoRespuesta = string.Empty;
                string strMensajeRespuesta = string.Empty;

                objResponse = RestServiceDPGeneral.PostInvoque<ConsultarActivosCBIOResponse>(objRequest, objAuditoria);

                strIdTransaccion = Funciones.CheckStr(objResponse.idTransaccion);
                strCodigoRespuesta = Funciones.CheckStr(objResponse.codigoRespuesta);
                strMensajeRespuesta = Funciones.CheckStr(objResponse.mensajeRespuesta);

                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-710][ValidarCandidatoFullClaroWSCBIO][Response][idTransaccion]", strIdTransaccion), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-710][ValidarCandidatoFullClaroWSCBIO][Response][codigoRespuesta]", strCodigoRespuesta), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-710][ValidarCandidatoFullClaroWSCBIO][Response][mensajeRespuesta]", strMensajeRespuesta), null, null);
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-710][ValidarCandidatoFullClaroWSCBIO][Ocurrio un error con el servicio]", Funciones.CheckStr(ConfigurationManager.AppSettings[objAuditoria.urlRest])), null, null);
                objLog.CrearArchivolog(string.Format("{0} {1} | {2}", "[INICIATIVA-710][ValidarCandidatoFullClaroWSCBIO][ERROR]", Funciones.CheckStr(ex.Message), Funciones.CheckStr(ex.StackTrace)), null, null);
            }

            objLog.CrearArchivolog(string.Format("{0}{1}", "[FIN][INICIATIVA-710][ValidarCandidatoFullClaroWSCBIO]", string.Empty), null, null);

            return objResponse;
        }

        public ConsultarPendientesActivasCBIOResponse ValidarProductosFullClaroWSCBIO(RequestConsultarPendientesActivasCBIO objRequest)
        {
            GeneradorLog objLog = new GeneradorLog(null, string.Format("{0}{1}", "[INICIO][INICIATIVA-710][ValidarProductosFullClaroWSCBIO]", string.Empty), null, null);
            ConsultarPendientesActivasCBIOResponse objResponse = new ConsultarPendientesActivasCBIOResponse();
            BEAuditoriaRequest objAuditoria = new BEAuditoriaRequest();
            Hashtable datosHeader = new Hashtable();

            try
            {
                #region Header
                objAuditoria.urlRest = "UrlValidarProductosFullClaroWS";
                objAuditoria.urlTimeOut_Rest = "TimeOutValidarProductosFullClaroWS";
                objAuditoria.dataPower = false;

                datosHeader.Add("idTransaccion", DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                datosHeader.Add("msgId", DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                datosHeader.Add("userId", Funciones.CheckStr(ConfigurationManager.AppSettings["constAplicacion"]));
                datosHeader.Add("timestamp", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"));
                objAuditoria.table = datosHeader;
                #endregion

                #region Body
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-710][ValidarProductosFullClaroWSCBIO][RequestBody][tipoDocumento]", Funciones.CheckStr(objRequest.consultarPendientesActivasCBIORequest.tipoDocumento)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-710][ValidarProductosFullClaroWSCBIO][RequestBody][numeroDocumento]", Funciones.CheckStr(objRequest.consultarPendientesActivasCBIORequest.nroDocumento)), null, null);
                #endregion

                objLog.CrearArchivolog(string.Format("{0} : {1} | {2} : {3}", "[INICIATIVA-710][ValidarProductosFullClaroWSCBIO][NameKeyWS]", objAuditoria.urlRest, "[ValorKeyWS]", Funciones.CheckStr(ConfigurationManager.AppSettings[objAuditoria.urlRest])), null, null);
                objLog.CrearArchivolog(string.Format("{0} : {1} | {2} : {3}", "[INICIATIVA-710][ValidarProductosFullClaroWSCBIO][NameTimeOutWS]", objAuditoria.urlTimeOut_Rest, "[ValorTimeOutWS]", Funciones.CheckStr(ConfigurationManager.AppSettings[objAuditoria.urlTimeOut_Rest])), null, null);

                string strIdTransaccion = string.Empty;
                string strCodigoRespuesta = string.Empty;
                string strMensajeRespuesta = string.Empty;

                objResponse = RestServiceDPGeneral.PostInvoque<ConsultarPendientesActivasCBIOResponse>(objRequest, objAuditoria);

                strIdTransaccion = Funciones.CheckStr(objResponse.idTransaccion);
                strCodigoRespuesta = Funciones.CheckStr(objResponse.codigoRespuesta);
                strMensajeRespuesta = Funciones.CheckStr(objResponse.mensajeRespuesta);

                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-710][ValidarProductosFullClaroWSCBIO][Response][idTransaccion]", strIdTransaccion), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-710][ValidarProductosFullClaroWSCBIO][Response][codigoRespuesta]", strCodigoRespuesta), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-710][ValidarProductosFullClaroWSCBIO][Response][mensajeRespuesta]", strMensajeRespuesta), null, null);
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-710][ValidarProductosFullClaroWSCBIO][Ocurrio un error con el servicio]", Funciones.CheckStr(ConfigurationManager.AppSettings[objAuditoria.urlRest])), null, null);
                objLog.CrearArchivolog(string.Format("{0} {1} | {2}", "[INICIATIVA-710][ValidarProductosFullClaroWSCBIO][ERROR]", Funciones.CheckStr(ex.Message), Funciones.CheckStr(ex.StackTrace)), null, null);
            }

            objLog.CrearArchivolog(string.Format("{0}{1}", "[FIN][INICIATIVA-710][ValidarProductosFullClaroWSCBIO]", string.Empty), null, null);

            return objResponse;
        }
    }
}
