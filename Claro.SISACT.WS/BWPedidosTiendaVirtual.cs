using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity.PedidosTiendVirtual.Response;
using Claro.SISACT.Common;
using Claro.SISACT.Entity;
using System.Configuration;
using System.Collections;
using Claro.SISACT.WS.RestServices;
using System.Web;
using Claro.SISACT.Entity.PedidosTiendVirtual.Request;

namespace Claro.SISACT.WS
{
    public class BWPedidosTiendaVirtual
    {
        public BWPedidosTiendaVirtual()
        {
        }
        #region INICIATIVA - 803 | INICIO | JLOPETAS
        public BodyResponseTiendaVirtual RegistrarPedidosTiendaVirtual(BodyRequestTiendaVirtual objRequest)
        {
            GeneradorLog objLog = new GeneradorLog(null, string.Format("{0}{1}", "[INICIO][INICIATIVA-803][RegistrarPedidosTiendaVirtual]", string.Empty), null, null);
            BodyResponseTiendaVirtual objResponse = new BodyResponseTiendaVirtual();
            BEAuditoriaRequest objAuditoria = new BEAuditoriaRequest();
            Hashtable Header = new Hashtable();
            string strIdTransaccion = string.Empty;
            string strCodigoRespuesta = string.Empty;
            string strMensajeRespuesta = string.Empty;

            try
            {
                objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIO][INICIATIVA-803][RegistrarPedidosTiendaVirtual]", string.Empty), null, null);
                #region Header - Auditoria
                objAuditoria.wsIp = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_ConsultaNacionalidad_wsIp"]);
                objAuditoria.ipTransaccion = Funciones.CheckStr(HttpContext.Current.Session["CurrentTerminal"]);
                objAuditoria.usuarioAplicacion = Funciones.CheckStr(HttpContext.Current.Session["CurrentUser"]);
                objAuditoria.applicationCode = Funciones.CheckStr(ConfigurationManager.AppSettings["constAplicacion"]);
                objAuditoria.ipApplication = Funciones.CheckStr(ConfigurationManager.AppSettings["constUsuarioAplicacion"]);
                objAuditoria.usuarioAplicacionEncriptado = Funciones.CheckStr(ConfigurationManager.AppSettings["User_ConsultaNacionalidad"]);
                objAuditoria.claveEncriptada = Funciones.CheckStr(ConfigurationManager.AppSettings["Password_ConsultaNacionalidad"]);
                objAuditoria.urlRest = "constUrlRegistrarPedidosTV";
                objAuditoria.urlTimeOut_Rest = "Time_Out_PedidosTV";
                objAuditoria.dataPower = false;
                #endregion

                #region Log's Auditoria
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-803][RegistrarPedidosTiendaVirtual][RequestHeader][wsIp]", objAuditoria.wsIp), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-803][RegistrarPedidosTiendaVirtual][RequestHeader][ipTransaccion]", objAuditoria.ipTransaccion), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-803][RegistrarPedidosTiendaVirtual][RequestHeader][usuarioAplicacion]", objAuditoria.usuarioAplicacion), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-803][RegistrarPedidosTiendaVirtual][RequestHeader][applicationCode]", objAuditoria.applicationCode), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-803][RegistrarPedidosTiendaVirtual][RequestHeader][ipApplication]", objAuditoria.ipApplication), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-803][RegistrarPedidosTiendaVirtual][RequestHeader][usuarioAplicacionEncriptado]", objAuditoria.usuarioAplicacionEncriptado), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-803][RegistrarPedidosTiendaVirtual][RequestHeader][claveEncriptada]", objAuditoria.claveEncriptada), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-803][RegistrarPedidosTiendaVirtual][RequestHeader][urlRest]", objAuditoria.urlRest), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-803][RegistrarPedidosTiendaVirtual][RequestHeader][urlTimeOut_Rest]", objAuditoria.urlTimeOut_Rest), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-803][RegistrarPedidosTiendaVirtual][RequestHeader][dataPower]",Funciones.CheckStr(objAuditoria.dataPower)), null, null);
                #endregion

                #region Header - HastTable
                Header.Add("idTransaccion", DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                Header.Add("msgId", DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                Header.Add("userId", Funciones.CheckStr(ConfigurationManager.AppSettings["constAplicacion"]));
                Header.Add("timestamp", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"));
                objAuditoria.table = Header;
                #endregion

                objResponse = RestServiceDPGeneral.PostInvoque<BodyResponseTiendaVirtual>(objRequest, objAuditoria);

                strIdTransaccion = Funciones.CheckStr(objResponse.MessageResponse.body.auditResponse.idTransaccion);
                strCodigoRespuesta = Funciones.CheckStr(objResponse.MessageResponse.body.auditResponse.codigoRespuesta);
                strMensajeRespuesta = Funciones.CheckStr(objResponse.MessageResponse.body.auditResponse.mensajeRespuesta);
                
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-803][RegistrarPedidosTiendaVirtual][Response][strIdTransaccion]", Funciones.CheckStr(strIdTransaccion)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-803][RegistrarPedidosTiendaVirtual][Response][strCodigoRespuesta]", Funciones.CheckStr(strCodigoRespuesta)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-803][RegistrarPedidosTiendaVirtual][Response][strMensajeRespuesta]", Funciones.CheckStr(strMensajeRespuesta)), null, null);

            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-803][RegistrarPedidosTiendaVirtual][Ocurrio un error con el servicio]", Funciones.CheckStr(ConfigurationManager.AppSettings[objAuditoria.urlRest])), null, null);
                objLog.CrearArchivolog(string.Format("{0} {1} | {2}", "[INICIATIVA-803][RegistrarPedidosTiendaVirtual][ERROR]", Funciones.CheckStr(ex.Message), Funciones.CheckStr(ex.StackTrace)), null, null);
            }

            objLog.CrearArchivolog(string.Format("{0}{1}", "[FIN][INICIATIVA-803][RegistrarPedidosTiendaVirtual]", string.Empty), null, null);

            return objResponse;
        }

    }
    #endregion
}
