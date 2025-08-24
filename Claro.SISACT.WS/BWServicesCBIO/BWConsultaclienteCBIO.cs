using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity.claro_int_consultacliente.consultarDatos;
using Claro.SISACT.Common;
using System.Collections;
using System.Configuration;
using Claro.SISACT.WS.RestServices;
using Claro.SISACT.Entity;
using System.Web;

namespace Claro.SISACT.WS.BWServicesCBIO
{
    public class BWConsultaclienteCBIO
    {
        public BWConsultaclienteCBIO()
        {
        }

        #region INICIATIVA-219 | Legados 2.0 | Andre Chumbes Lizarraga
        public MessageResponseConsultarCliente ConsultarClienteWSCBIO(MessageRequestConsultarCliente objRequest)
        {
            GeneradorLog objLog = new GeneradorLog(null, string.Format("{0}{1}", "[INICIO][INICIATIVA-219][ConsultarClienteWSCBIO]", string.Empty), null, null);
            MessageResponseConsultarCliente objResponse = new MessageResponseConsultarCliente();
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
                objAuditoria.urlRest = "UrlConsultarDatosClienteWS";
                objAuditoria.urlTimeOut_Rest = "TimeOutConsultarDatosClienteWS";
                objAuditoria.dataPower = false;

                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarClienteWSCBIO][RequestHeader][wsIp]", objAuditoria.wsIp), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarClienteWSCBIO][RequestHeader][ipTransaccion]", objAuditoria.ipTransaccion), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarClienteWSCBIO][RequestHeader][usuarioAplicacion]", objAuditoria.usuarioAplicacion), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarClienteWSCBIO][RequestHeader][applicationCode]", objAuditoria.applicationCode), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarClienteWSCBIO][RequestHeader][idAplicacion]", objAuditoria.idAplicacion), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarClienteWSCBIO][RequestHeader][bolDataPower]", objAuditoria.dataPower), null, null);

                datosHeader.Add("idTransaccion", DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                datosHeader.Add("msgId", DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                datosHeader.Add("userId", Funciones.CheckStr(ConfigurationManager.AppSettings["constAplicacion"]));
                datosHeader.Add("timestamp", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"));

                objAuditoria.table = datosHeader;
                #endregion

                #region Body
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarClienteWSCBIO][RequestBody][tipoDocumento]", Funciones.CheckStr(objRequest.Body.tipoDocumento)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarClienteWSCBIO][RequestBody][numeroDocumento]", Funciones.CheckStr(objRequest.Body.numeroDocumento)), null, null);
                #endregion

                objLog.CrearArchivolog(string.Format("{0} : {1} | {2} : {3}", "[INICIATIVA-219][ConsultarClienteWSCBIO][NameKeyWS]", objAuditoria.urlRest, "[ValorKeyWS]", Funciones.CheckStr(ConfigurationManager.AppSettings["UrlConsultarDatosClienteWS"])), null, null);
                objLog.CrearArchivolog(string.Format("{0} : {1} | {2} : {3}", "[INICIATIVA-219][ConsultarClienteWSCBIO][NameTimeOutWS]", objAuditoria.urlTimeOut_Rest, "[ValorTimeOutWS]", Funciones.CheckStr(ConfigurationManager.AppSettings["TimeOutConsultarDatosClienteWS"])), null, null);

                string strIdTransaccion = string.Empty;
                string strCodigoRespuesta = string.Empty;
                string strMensajeRespuesta = string.Empty;

                objResponse = RestServiceDPGeneral.PostInvoque<MessageResponseConsultarCliente>(objRequest, objAuditoria);

                strIdTransaccion = Funciones.CheckStr(objResponse.consultarDatosResponse.responseAudit.idTransaccion);
                strCodigoRespuesta = Funciones.CheckStr(objResponse.consultarDatosResponse.responseAudit.codigoRespuesta);
                strMensajeRespuesta = Funciones.CheckStr(objResponse.consultarDatosResponse.responseAudit.mensajeRespuesta);

                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarClienteWSCBIO][Response][strIdTransaccion]", strIdTransaccion), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarClienteWSCBIO][Response][strCodigoRespuesta]", strCodigoRespuesta), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarClienteWSCBIO][Response][strMensajeRespuesta]", strMensajeRespuesta), null, null);
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarClienteWSCBIO][Ocurrio un error con el servicio]", Funciones.CheckStr(ConfigurationManager.AppSettings[objAuditoria.urlRest])), null, null);
                objLog.CrearArchivolog(string.Format("{0} {1} | {2}", "[INICIATIVA-219][ConsultarClienteWSCBIO][ERROR]", Funciones.CheckStr(ex.Message), Funciones.CheckStr(ex.StackTrace)), null, null);
            }

            objLog.CrearArchivolog(string.Format("{0}{1}", "[FIN][INICIATIVA-219][ConsultarClienteWSCBIO]", string.Empty), null, null);

            return objResponse;
        }
        #endregion
    }
}
