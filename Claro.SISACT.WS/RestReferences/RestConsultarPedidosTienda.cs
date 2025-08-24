using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity;
using System.Net;
using Claro.SISACT.WS.RestServices;
using Claro.SISACT.Common;
using System.Collections;
using Claro.SISACT.Entity.claro_vent_pedidostienda.Request;//INICIATIVA-803 INI
using Claro.SISACT.Entity.claro_vent_pedidostienda.Response;//INICIATIVA-803 INI

namespace Claro.SISACT.WS.RestReferences
{
    public class RestConsultarPedidosTienda : RestServiceGeneric
    {
        static string strLogNameCapaWS = "claro_vent_pedidostienda";
        public RestConsultarPedidosTienda()
        {
            
        }
        public PedidosTiendaResponse restConsultarPedidosVenta(Dictionary<string, string> parametros, BEAuditoriaRequest objBeAuditRequest)
        {
            string logFileName = string.Format("{0} - {1}", strLogNameCapaWS, "Metodo restConsultarPedidosVenta");
            this.SetLogFileName(logFileName);
            GeneradorLog.EscribirLog(logFileName, null, string.Format("{0} {1} {0}", "********************", "Inicio método restConsultarPedidosVenta"));
            try
            {
                BEAuditoriaRequest objAuditoria = GetAuditoria(objBeAuditRequest);
                RestServiceDPGeneral.strArchivo = logFileName;

                return RestServiceDPGeneral.HttpCallInvoque<PedidosTiendaResponse>(WebRequestMethods.Http.Get,objBeAuditRequest.urlRest, parametros, null, null, objAuditoria);
            }
            catch (Exception ex)
            {
                GeneradorLog.EscribirLog(logFileName, null, string.Format("ERROR. Ha ocurrido un error en el método restConsultarPedidosVenta - Message. {0}", ex.Message), null, ex);
                GeneradorLog.EscribirLog(logFileName, null, string.Format("ERROR. Ha ocurrido un error en el método restConsultarPedidosVenta - StackTrace. {0}", ex.StackTrace), null, ex);
                throw;
            }
            finally
            {
                GeneradorLog.EscribirLog(logFileName, null, string.Format("{0} {1} {0}", "********************", "Fin método restConsultarPedidosVenta"));
            }
        }

        //aprobar
        public PedidosTiendaResponse AprobarPedidosTienda(Dictionary<string, string> parametros,MessageRequestPT objAprobarPedidos, BEAuditoriaRequest objBEAuditoriaRequest)
        {
            string logFileName = string.Format("{0} - {1}", strLogNameCapaWS, "Metodo AprobarPedidosTienda");
            this.SetLogFileName(logFileName);
            GeneradorLog.EscribirLog(logFileName, null, string.Format("{0} {1} {0}", "********************", "Inicio método restConsultarPedidosVenta"));
            try
            {
                BEAuditoriaRequest objAuditoria = GetAuditoria(objBEAuditoriaRequest);
                RestServiceDPGeneral.strArchivo = logFileName;

                return RestServiceDPGeneral.HttpCallInvoque<PedidosTiendaResponse>(WebRequestMethods.Http.Post, objBEAuditoriaRequest.urlRest, parametros, objAprobarPedidos, null, objAuditoria);
            }
            catch (Exception ex)
            {
                GeneradorLog.EscribirLog(logFileName, null, string.Format("ERROR. Ha ocurrido un error en el método restConsultarPedidosVenta - Message. {0}", ex.Message), null, ex);
                GeneradorLog.EscribirLog(logFileName, null, string.Format("ERROR. Ha ocurrido un error en el método restConsultarPedidosVenta - StackTrace. {0}", ex.StackTrace), null, ex);
                throw;
            }
            finally
            {
                GeneradorLog.EscribirLog(logFileName, null, string.Format("{0} {1} {0}", "********************", "Fin método restConsultarPedidosVenta"));
            }

        }

        //rechazar
        public PedidosTiendaResponse AnularPedidosTienda(Dictionary<string, string> parametros,MessageRequestPT objAnularPedidos, BEAuditoriaRequest objBEAuditoriaRequest)
        {
            string logFileName = string.Format("{0} - {1}", strLogNameCapaWS, "Metodo AnularPedidosTienda");
            this.SetLogFileName(logFileName);
            GeneradorLog.EscribirLog(logFileName, null, string.Format("{0} {1} {0}", "********************", "Inicio método restConsultarPedidosVenta"));
            try
            {
                BEAuditoriaRequest objAuditoria = GetAuditoria(objBEAuditoriaRequest);
                RestServiceDPGeneral.strArchivo = logFileName;

                return RestServiceDPGeneral.HttpCallInvoque<PedidosTiendaResponse>(WebRequestMethods.Http.Post, objBEAuditoriaRequest.urlRest, parametros, objAnularPedidos, null, objAuditoria);
            }
            catch (Exception ex)
            {
                GeneradorLog.EscribirLog(logFileName, null, string.Format("ERROR. Ha ocurrido un error en el método restConsultarPedidosVenta - Message. {0}", ex.Message), null, ex);
                GeneradorLog.EscribirLog(logFileName, null, string.Format("ERROR. Ha ocurrido un error en el método restConsultarPedidosVenta - StackTrace. {0}", ex.StackTrace), null, ex);
                throw;
            }
            finally
            {
                GeneradorLog.EscribirLog(logFileName, null, string.Format("{0} {1} {0}", "********************", "Fin método restConsultarPedidosVenta"));
            }

        }

         //validar
        public PedidosTiendaResponse ValidarPedidosTienda(Dictionary<string, string> parametros, MessageRequestPT objrevaluar, BEAuditoriaRequest objBEAuditoriaRequest)
        {
            string logFileName = string.Format("{0} - {1}", strLogNameCapaWS, "Metodo ValidarPedidosTienda");
            this.SetLogFileName(logFileName);
            GeneradorLog.EscribirLog(logFileName, null, string.Format("{0} {1} {0}", "********************", "Inicio método restActualizarPedidoVenta"));
            try
            {
                BEAuditoriaRequest objAuditoria = GetAuditoria(objBEAuditoriaRequest);
                RestServiceDPGeneral.strArchivo = logFileName;

                return RestServiceDPGeneral.HttpCallInvoque<PedidosTiendaResponse>(WebRequestMethods.Http.Post, objBEAuditoriaRequest.urlRest, parametros, objrevaluar, null, objAuditoria);
            }
            catch (Exception ex)
            {
                GeneradorLog.EscribirLog(logFileName, null, string.Format("ERROR. Ha ocurrido un error en el método restConsultarPedidosVenta - Message. {0}", ex.Message), null, ex);
                GeneradorLog.EscribirLog(logFileName, null, string.Format("ERROR. Ha ocurrido un error en el método restConsultarPedidosVenta - StackTrace. {0}", ex.StackTrace), null, ex);
                throw;
            }
            finally
            {
                GeneradorLog.EscribirLog(logFileName, null, string.Format("{0} {1} {0}", "********************", "Fin método restConsultarPedidosVenta"));
            }

        }
        //registrar
        public SolicitudExcepcionPrecioResponse registrarPedidosTienda(Dictionary<string, string> parametros, RegistrarSolicitudRequest objRequest, BEAuditoriaRequest objBEAuditoriaRequest)
        {
            string logFileName = string.Format("{0} - {1}", strLogNameCapaWS, "Metodo registrarPedidosTienda");
            this.SetLogFileName(logFileName);
            GeneradorLog.EscribirLog(logFileName, null, string.Format("{0} {1} {0}", "********************", "Inicio método restActualizarPedidoVenta"));
            try
            {
                BEAuditoriaRequest objAuditoria = GetAuditoria(objBEAuditoriaRequest);
                RestServiceDPGeneral.strArchivo = logFileName;

                return RestServiceDPGeneral.HttpCallInvoque<SolicitudExcepcionPrecioResponse>(WebRequestMethods.Http.Post, objBEAuditoriaRequest.urlRest, parametros, objRequest, null, objAuditoria);
            }
            catch (Exception ex)
            {
                GeneradorLog.EscribirLog(logFileName, null, string.Format("ERROR. Ha ocurrido un error en el método restConsultarPedidosVenta - Message. {0}", ex.Message), null, ex);
                GeneradorLog.EscribirLog(logFileName, null, string.Format("ERROR. Ha ocurrido un error en el método restConsultarPedidosVenta - StackTrace. {0}", ex.StackTrace), null, ex);
                throw;
            }
            finally
            {
                GeneradorLog.EscribirLog(logFileName, null, string.Format("{0} {1} {0}", "********************", "Fin método restConsultarPedidosVenta"));
            }

        }

    }
}
