using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity.claro_inte_cuparticipante.consultarParticipante;
using Claro.SISACT.Common;
using System.Collections;
using System.Configuration;
using Claro.SISACT.WS.RestServices;
using Claro.SISACT.Entity;
using System.Web;

namespace Claro.SISACT.WS.BWServicesCBIO
{
    public class BWCuParticipanteCBIO
    {
        public BWCuParticipanteCBIO()
        {
        }

        #region INICIATIVA-219 | Legados 2.0 | Andre Chumbes Lizarraga
        public BuscarCuParticipanteResponse ConsultarParticipanteWSCBIO(BuscarCuParticipanteRequest objRequest)
        {
            GeneradorLog objLog = new GeneradorLog(null, string.Format("{0}{1}", "[INICIO][INICIATIVA-219][ConsultarParticipanteWSCBIO]", string.Empty), null, null);
            BuscarCuParticipanteResponse objResponse = new BuscarCuParticipanteResponse();
            BEAuditoriaRequest objAuditoria = new BEAuditoriaRequest();
            Hashtable datosHeader = new Hashtable();

            try
            {
                #region Header
                objAuditoria.wsIp = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_ConsultaNacionalidad_wsIp"]);
                objAuditoria.ipTransaccion = Funciones.CheckStr(HttpContext.Current.Session["CurrentTerminal"]);
                objAuditoria.usuarioAplicacion = Funciones.CheckStr(HttpContext.Current.Session["CurrentUser"]);
                objAuditoria.applicationCode = Funciones.CheckStr(ConfigurationManager.AppSettings["constAplicacion"]);
                objAuditoria.ipApplication = Funciones.CheckStr(ConfigurationManager.AppSettings["constUsuarioAplicacion"]);
                objAuditoria.usuarioAplicacionEncriptado = Funciones.CheckStr(ConfigurationManager.AppSettings["User_ConsultaNacionalidad"]);
                objAuditoria.claveEncriptada = Funciones.CheckStr(ConfigurationManager.AppSettings["Password_ConsultaNacionalidad"]);
                objAuditoria.urlRest = "UrlConsultarParticipanteWS";
                objAuditoria.urlTimeOut_Rest = "TimeOutConsultarParticipanteWS";
                objAuditoria.dataPower = false;

                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarParticipanteWSCBIO][RequestHeader][wsIp]", objAuditoria.wsIp), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarParticipanteWSCBIO][RequestHeader][ipTransaccion]", objAuditoria.ipTransaccion), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarParticipanteWSCBIO][RequestHeader][usuarioAplicacion]", objAuditoria.usuarioAplicacion), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarParticipanteWSCBIO][RequestHeader][applicationCode]", objAuditoria.applicationCode), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarParticipanteWSCBIO][RequestHeader][idAplicacion]", objAuditoria.idAplicacion), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarParticipanteWSCBIO][RequestHeader][bolDataPower]", objAuditoria.dataPower), null, null);

                datosHeader.Add("idTransaccion", DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                datosHeader.Add("msgId", DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                datosHeader.Add("userId", Funciones.CheckStr(ConfigurationManager.AppSettings["constAplicacion"]));
                datosHeader.Add("timestamp", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"));

                objAuditoria.table = datosHeader;
                #endregion

                #region Body
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarParticipanteWSCBIO][RequestBody][codigoClienteUnico]", Funciones.CheckStr(objRequest.codigoClienteUnico)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarParticipanteWSCBIO][RequestBody][participanteID]", Funciones.CheckStr(objRequest.participanteID)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarParticipanteWSCBIO][RequestBody][tipoDocumento]", Funciones.CheckStr(objRequest.tipoDocumento)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarParticipanteWSCBIO][RequestBody][numeroDocumento]", Funciones.CheckStr(objRequest.numeroDocumento)), null, null);
                #endregion

                objLog.CrearArchivolog(string.Format("{0} : {1} | {2} : {3}", "[INICIATIVA-219][ConsultarParticipanteWSCBIO][NameKeyWS]", objAuditoria.urlRest, "[ValorKeyWS]", Funciones.CheckStr(ConfigurationManager.AppSettings["UrlConsultarParticipanteWS"])), null, null);
                objLog.CrearArchivolog(string.Format("{0} : {1} | {2} : {3}", "[INICIATIVA-219][ConsultarParticipanteWSCBIO][NameTimeOutWS]", objAuditoria.urlTimeOut_Rest, "[ValorTimeOutWS]", Funciones.CheckStr(ConfigurationManager.AppSettings["TimeOutConsultarParticipanteWS"])), null, null);

                string strCodigoRespuesta = string.Empty;
                string strMensajeRespuesta = string.Empty;

                objResponse = RestServiceDPGeneral.PostInvoque<BuscarCuParticipanteResponse>(objRequest, objAuditoria);

                strCodigoRespuesta = Funciones.CheckStr(objResponse.codigoRespuesta);
                strMensajeRespuesta = Funciones.CheckStr(objResponse.mensajeRespuesta);

                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarParticipanteWSCBIO][Response][strCodigoRespuesta]", strCodigoRespuesta), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarParticipanteWSCBIO][Response][strMensajeRespuesta]", strMensajeRespuesta), null, null);
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarParticipanteWSCBIO][Ocurrio un error con el servicio]", Funciones.CheckStr(ConfigurationManager.AppSettings[objAuditoria.urlRest])), null, null);
                objLog.CrearArchivolog(string.Format("{0} {1} | {2}", "[INICIATIVA-219][ConsultarParticipanteWSCBIO][ERROR]", Funciones.CheckStr(ex.Message), Funciones.CheckStr(ex.StackTrace)), null, null);
            }

            objLog.CrearArchivolog(string.Format("{0}{1}", "[FIN][INICIATIVA-219][ConsultarParticipanteWSCBIO]", string.Empty), null, null);

            return objResponse;
        }
        #endregion
    }
}
