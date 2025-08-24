using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity.claro_ope_comportamientoCliente.consultarComportamientoPago;
using Claro.SISACT.Common;
using Claro.SISACT.Entity;
using System.Configuration;
using System.Collections;
using Claro.SISACT.WS.RestServices;
using System.Web;

namespace Claro.SISACT.WS.BWServicesCBIO
{
    public class BWComportamientoClienteCBIO
    {
        public BWComportamientoClienteCBIO()
        {
        }

        #region INICIATIVA-219 | Legados 2.0 | Andre Chumbes Lizarraga
        public BodyResponseComportamientoCliente ObtenerComportamientoPagoWSCBIO(BodyRequestComportamientoCliente objRequest)
        {
            GeneradorLog objLog = new GeneradorLog(null, string.Format("{0}{1}", "[INICIO][INICIATIVA-219][ObtenerComportamientoPagoWSCBIO]", string.Empty), null, null);
            BodyResponseComportamientoCliente objResponse = new BodyResponseComportamientoCliente();
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
                objAuditoria.urlRest = "UrlConsultarComportamientoPagoWS";
                objAuditoria.urlTimeOut_Rest = "TimeOutConsultarComportamientoPagoWS";
                objAuditoria.dataPower = false;

                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ObtenerComportamientoPagoWSCBIO][RequestHeader][wsIp]", objAuditoria.wsIp), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ObtenerComportamientoPagoWSCBIO][RequestHeader][ipTransaccion]", objAuditoria.ipTransaccion), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ObtenerComportamientoPagoWSCBIO][RequestHeader][usuarioAplicacion]", objAuditoria.usuarioAplicacion), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ObtenerComportamientoPagoWSCBIO][RequestHeader][applicationCode]", objAuditoria.applicationCode), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ObtenerComportamientoPagoWSCBIO][RequestHeader][idAplicacion]", objAuditoria.idAplicacion), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ObtenerComportamientoPagoWSCBIO][RequestHeader][bolDataPower]", objAuditoria.dataPower), null, null);

                datosHeader.Add("idTransaccion", DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                datosHeader.Add("msgId", DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                datosHeader.Add("userId", Funciones.CheckStr(ConfigurationManager.AppSettings["constAplicacion"]));
                datosHeader.Add("timestamp", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"));
                objAuditoria.table = datosHeader;
                #endregion

                #region Body
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ObtenerComportamientoPagoWSCBIO][RequestBody][tipoDocumento]", Funciones.CheckStr(objRequest.consultarComportamientPago.tipoDocumento)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ObtenerComportamientoPagoWSCBIO][RequestBody][numeroDocumento]", Funciones.CheckStr(objRequest.consultarComportamientPago.numeroDocumento)), null, null);
                #endregion

                objLog.CrearArchivolog(string.Format("{0} : {1} | {2} : {3}", "[INICIATIVA-219][ObtenerComportamientoPagoWSCBIO][NameKeyWS]", objAuditoria.urlRest, "[ValorKeyWS]", Funciones.CheckStr(ConfigurationManager.AppSettings["UrlConsultarComportamientoPagoWS"])), null, null);
                objLog.CrearArchivolog(string.Format("{0} : {1} | {2} : {3}", "[INICIATIVA-219][ObtenerComportamientoPagoWSCBIO][NameTimeOutWS]", objAuditoria.urlTimeOut_Rest, "[ValorTimeOutWS]", Funciones.CheckStr(ConfigurationManager.AppSettings["TimeOutConsultarComportamientoPagoWS"])), null, null);

                string strIdTransaccion = string.Empty;
                string strCodigoRespuesta = string.Empty;
                string strMensajeRespuesta = string.Empty;

                objResponse = RestServiceDPGeneral.PostInvoque<BodyResponseComportamientoCliente>(objRequest, objAuditoria);

                strIdTransaccion = Funciones.CheckStr(objResponse.consultarComportamientoPagoResponse.responseAudit.idTransaccion);
                strCodigoRespuesta = Funciones.CheckStr(objResponse.consultarComportamientoPagoResponse.responseAudit.codigoRespuesta);
                strMensajeRespuesta = Funciones.CheckStr(objResponse.consultarComportamientoPagoResponse.responseAudit.mensajeRespuesta);

                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ObtenerComportamientoPagoWSCBIO][Response][strIdTransaccion]", strIdTransaccion), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ObtenerComportamientoPagoWSCBIO][Response][strCodigoRespuesta]", strCodigoRespuesta), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ObtenerComportamientoPagoWSCBIO][Response][strMensajeRespuesta]", strMensajeRespuesta), null, null);
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ObtenerComportamientoPagoWSCBIO][Ocurrio un error con el servicio]", Funciones.CheckStr(ConfigurationManager.AppSettings[objAuditoria.urlRest])), null, null);
                objLog.CrearArchivolog(string.Format("{0} {1} | {2}", "[INICIATIVA-219][ObtenerComportamientoPagoWSCBIO][ERROR]", Funciones.CheckStr(ex.Message), Funciones.CheckStr(ex.StackTrace)), null, null);
            }

            return objResponse;
        }

        public BodyResponseComportamientoCliente ObtenerPromedioFacturadoWSCBIO(BodyRequestComportamientoCliente objRequest)
        {
            GeneradorLog objLog = new GeneradorLog(null, string.Format("{0}{1}", "[INICIO][INICIATIVA-219][ObtenerPromedioFacturadoWSCBIO]", string.Empty), null, null);
            BodyResponseComportamientoCliente objResponse = new BodyResponseComportamientoCliente();
            BEAuditoriaRequest objAuditoria = new BEAuditoriaRequest();
            Hashtable datosHeader = new Hashtable();

            try
            {
                #region Header
                objAuditoria.wsIp = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_ConsultaNacionalidad_wsIp"]);
                objAuditoria.applicationCode = Funciones.CheckStr(ConfigurationManager.AppSettings["constAplicacion"]);
                objAuditoria.ipApplication = Funciones.CheckStr(ConfigurationManager.AppSettings["constUsuarioAplicacion"]);
                objAuditoria.usuarioAplicacionEncriptado = Funciones.CheckStr(ConfigurationManager.AppSettings["User_ConsultaNacionalidad"]);
                objAuditoria.claveEncriptada = Funciones.CheckStr(ConfigurationManager.AppSettings["Password_ConsultaNacionalidad"]);
                objAuditoria.urlRest = "UrlConsultarPromedioFacturadoWS";
                objAuditoria.urlTimeOut_Rest = "TimeOutConsultarPromedioFacturadoWS";
                objAuditoria.dataPower = false;

                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ObtenerPromedioFacturadoWSCBIO][RequestHeader][wsIp]", Funciones.CheckStr(objAuditoria.wsIp)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ObtenerPromedioFacturadoWSCBIO][RequestHeader][applicationCode]", Funciones.CheckStr(objAuditoria.applicationCode)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ObtenerPromedioFacturadoWSCBIO][RequestHeader][ipApplication]", Funciones.CheckStr(objAuditoria.ipApplication)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ObtenerPromedioFacturadoWSCBIO][RequestHeader][bolDataPower]", objAuditoria.dataPower), null, null);

                datosHeader.Add("idTransaccion", DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                datosHeader.Add("msgId", DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                datosHeader.Add("userId", Funciones.CheckStr(ConfigurationManager.AppSettings["constAplicacion"]));
                datosHeader.Add("timestamp", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"));

                objAuditoria.table = datosHeader;
                #endregion

                #region Body
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ObtenerPromedioFacturadoWSCBIO][RequestBody][tipoDocumento]", Funciones.CheckStr(objRequest.consultarPromedioFacturado.tipoDocumento)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ObtenerPromedioFacturadoWSCBIO][RequestBody][nroDocumento]", Funciones.CheckStr(objRequest.consultarPromedioFacturado.numeroDocumento)), null, null);
                #endregion

                objLog.CrearArchivolog(string.Format("{0} : {1} | {2} : {3}", "[INICIATIVA-219][ObtenerPromedioFacturadoWSCBIO][NameKeyWS]", objAuditoria.urlRest, "[ValorKeyWS]", Funciones.CheckStr(ConfigurationManager.AppSettings["UrlConsultarPromedioFacturadoWS"])), null, null);
                objLog.CrearArchivolog(string.Format("{0} : {1} | {2} : {3}", "[INICIATIVA-219][ObtenerPromedioFacturadoWSCBIO][NameTimeOutWS]", objAuditoria.urlTimeOut_Rest, "[ValorTimeOutWS]", Funciones.CheckStr(ConfigurationManager.AppSettings["TimeOutConsultarPromedioFacturadoWS"])), null, null);

                string strIdTransaccion = string.Empty;
                string strCodigoRespuesta = string.Empty;
                string strMensajeRespuesta = string.Empty;

                objResponse = RestServiceDPGeneral.PostInvoque<BodyResponseComportamientoCliente>(objRequest, objAuditoria);

                strIdTransaccion = Funciones.CheckStr(objResponse.consultarPromedioFacturadoResponse.responseAudit.idTransaccion);
                strCodigoRespuesta = Funciones.CheckStr(objResponse.consultarPromedioFacturadoResponse.responseAudit.mensajeRespuesta);
                strMensajeRespuesta = Funciones.CheckStr(objResponse.consultarPromedioFacturadoResponse.responseAudit.codigoRespuesta);

                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ObtenerPromedioFacturadoWSCBIO][Response][strIdTransaccion]", strIdTransaccion), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ObtenerPromedioFacturadoWSCBIO][Response][strCodigoRespuesta]", strCodigoRespuesta), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ObtenerPromedioFacturadoWSCBIO][Response][strMensajeRespuesta]", strMensajeRespuesta), null, null);
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ObtenerPromedioFacturadoWSCBIO][Ocurrio un error con el servicio]", Funciones.CheckStr(ConfigurationManager.AppSettings[objAuditoria.urlRest])), null, null);
                objLog.CrearArchivolog(string.Format("{0} {1} | {2}", "[INICIATIVA-219][ObtenerPromedioFacturadoWSCBIO][ERROR]", Funciones.CheckStr(ex.Message), Funciones.CheckStr(ex.StackTrace)), null, null);
            }

            objLog.CrearArchivolog(string.Format("{0}{1}", "[FIN][INICIATIVA-219][ObtenerPromedioFacturadoWSCBIO]", string.Empty), null, null);

            return objResponse;
        }
        #endregion
    }
}