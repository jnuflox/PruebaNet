using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity.VentasCuotas.ObtenerVariablesBRMS.Response;
using Claro.SISACT.Entity;
using Claro.SISACT.Entity.VentasCuotas.ValidarMaterialAccCuota.Response;
using Claro.SISACT.Common;
using Claro.SISACT.WS.RestServices;
using System.Net;
using Claro.SISACT.Entity.VentasCuotas.RegistrarVariablesBRMS.Response;
using Claro.SISACT.Entity.VentasCuotas.RegistrarVariablesBRMS.Request;
using Claro.SISACT.Entity.VentasCuotas.RegistrarVtaAccCuotas.Response;
using Claro.SISACT.Entity.VentasCuotas.RegistrarVtaAccCuotas.Request;
using Claro.SISACT.Entity.VentasCuotas.ValidarCuoAccPendXLinea.Response;
using Claro.SISACT.Entity.VentasCuotas.ValidarCuoAccPendXLinea.Request;
using Claro.SISACT.Entity.VentasCuotas.ObtenerDatosPedidoAccCuotas.Response;
using Claro.SISACT.Entity.ConsultaSOT.Response;
using Claro.SISACT.Entity.ConsultaSOT.Request;

namespace Claro.SISACT.WS.RestReferences
{
    #region [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil]
    public class RestVentasCuotas : RestServiceGeneric
    {
        static string strLogNameCapaWS = "gestioncuotasacc";

        public RestVentasCuotas()
        {
        }

        #region [Metodos GET]
        public ObtenerVariablesBRMSResponse ObtenerVariablesBRMS(Dictionary<string, string> parametros, BEAuditoriaRequest objBeAuditRequest)
        {
            string logFileName = string.Format("{0} - {1}", strLogNameCapaWS, "Metodo ObtenerVariablesBRMS");
            this.SetLogFileName(logFileName);
            GeneradorLog.EscribirLog(logFileName, null, string.Format("{0} {1} {0}", "********************", "Inicio método ObtenerVariablesBRMS"));

            try
            {
                BEAuditoriaRequest objAuditoria = GetAuditoria(objBeAuditRequest);
                RestServiceDPGeneral.strArchivo = logFileName;

                return RestServiceDPGeneral.HttpCallInvoque<ObtenerVariablesBRMSResponse>(WebRequestMethods.Http.Get, objBeAuditRequest.urlRest, parametros, null, null, objAuditoria);
            }
            catch (Exception ex)
            {
                GeneradorLog.EscribirLog(logFileName, null, string.Format("ERROR. Ha ocurrido un error en el método ObtenerVariablesBRMS - Message. {0}", ex.Message), null, ex);
                GeneradorLog.EscribirLog(logFileName, null, string.Format("ERROR. Ha ocurrido un error en el método ObtenerVariablesBRMS - StackTrace. {0}", ex.StackTrace), null, ex);
                throw;
            }
            finally
            {
                GeneradorLog.EscribirLog(logFileName, null, string.Format("{0} {1} {0}", "********************", "Fin método restConsultarPedidosVenta"));
            }

        }

        public ValidarMaterialAccCuotaResponse ValidarMaterialAccCuota(Dictionary<string, string> parametros, BEAuditoriaRequest objBeAuditRequest)
        {
            string logFileName = string.Format("{0} - {1}", strLogNameCapaWS, "Metodo ValidarMaterialAccCuota");
            this.SetLogFileName(logFileName);
            GeneradorLog.EscribirLog(logFileName, null, string.Format("{0} {1} {0}", "********************", "Inicio método ValidarMaterialAccCuota"));

            try
            {
                BEAuditoriaRequest objAuditoria = GetAuditoria(objBeAuditRequest);
                RestServiceDPGeneral.strArchivo = logFileName;

                return RestServiceDPGeneral.HttpCallInvoque<ValidarMaterialAccCuotaResponse>(WebRequestMethods.Http.Get, objBeAuditRequest.urlRest, parametros, null, null, objAuditoria);
            }
            catch (Exception ex)
            {
                GeneradorLog.EscribirLog(logFileName, null, string.Format("ERROR. Ha ocurrido un error en el método ObtenerVariablesBRMS - Message. {0}", ex.Message), null, ex);
                GeneradorLog.EscribirLog(logFileName, null, string.Format("ERROR. Ha ocurrido un error en el método ObtenerVariablesBRMS - StackTrace. {0}", ex.StackTrace), null, ex);
                throw;
            }
            finally
            {
                GeneradorLog.EscribirLog(logFileName, null, string.Format("{0} {1} {0}", "********************", "Fin método restConsultarPedidosVenta"));
            }

        }
        // PROY-140743 IDEA141192 NUEVAS VARIABLES BRMS VTACUO VICTOR CANCHICA INI 

        public ObtenerDatosPedidoAccCuotasResponse ObtenerDatosPedidoAccCuotas(Dictionary<string, string> parametros, BEAuditoriaRequest objBeAuditRequest)
        {
            string logFileName = string.Format("{0} - {1}", strLogNameCapaWS, "Metodo ObtenerVariablesBRMS");
            this.SetLogFileName(logFileName);
            GeneradorLog.EscribirLog(logFileName, null, string.Format("{0} {1} {0}", "********************", "Inicio método ObtenerVariablesBRMS"));
            
            try
            {
                BEAuditoriaRequest objAuditoria = GetAuditoria(objBeAuditRequest);
                RestServiceDPGeneral.strArchivo = logFileName;
            
                return RestServiceDPGeneral.HttpCallInvoque<ObtenerDatosPedidoAccCuotasResponse>(WebRequestMethods.Http.Get, objBeAuditRequest.urlRest, parametros, null, null, objAuditoria);
            }
            catch (Exception ex)
            {
                GeneradorLog.EscribirLog(logFileName, null, string.Format("ERROR. Ha ocurrido un error en el método ObtenerDatosPedidoAccCuotas - Message. {0}", ex.Message), null, ex);
                GeneradorLog.EscribirLog(logFileName, null, string.Format("ERROR. Ha ocurrido un error en el método ObtenerDatosPedidoAccCuotas - StackTrace. {0}", ex.StackTrace), null, ex);
                throw;
            }
            finally
            {
                GeneradorLog.EscribirLog(logFileName, null, string.Format("{0} {1} {0}", "********************", "Fin método ObtenerDatosPedidoAccCuotas"));
            }
            // PROY-140743 IDEA141192 NUEVAS VARIABLES BRMS VTACUO VICTOR CANCHICA FIN


        }

        #endregion

        #region [Metodos POST]

        public RegistrarVariablesResponse RegistrarVariablesBRMS(Dictionary<string, string> parametros, RegistrarVariablesRequest objRequest, BEAuditoriaRequest objBEAuditoriaRequest)
        {
            string logFileName = string.Format("{0} - {1}", strLogNameCapaWS, "Metodo RegistrarVariablesBRMS");
            this.SetLogFileName(logFileName);
            GeneradorLog.EscribirLog(logFileName, null, string.Format("{0} {1} {0}", "********************", "Inicio método RegistrarVariablesBRMS"));
            try
            {
                BEAuditoriaRequest objAuditoria = GetAuditoria(objBEAuditoriaRequest);
                RestServiceDPGeneral.strArchivo = logFileName;

                return RestServiceDPGeneral.HttpCallInvoque<RegistrarVariablesResponse>(WebRequestMethods.Http.Post, objBEAuditoriaRequest.urlRest, parametros, objRequest, null, objAuditoria);
            }
            catch (Exception ex)
            {
                GeneradorLog.EscribirLog(logFileName, null, string.Format("ERROR. Ha ocurrido un error en el método RegistrarVariablesBRMS - Message. {0}", ex.Message), null, ex);
                GeneradorLog.EscribirLog(logFileName, null, string.Format("ERROR. Ha ocurrido un error en el método RegistrarVariablesBRMS - StackTrace. {0}", ex.StackTrace), null, ex);
                throw;
            }
            finally
            {
                GeneradorLog.EscribirLog(logFileName, null, string.Format("{0} {1} {0}", "********************", "Fin método RegistrarVariablesBRMS"));
            }
        }

        public RegistrarVentaAccCuotasResponse RegistrarVtaAccCuotas(Dictionary<string, string> parametros, RegistrarVentaAccCuotasRequest objRequest, BEAuditoriaRequest objBEAuditoriaRequest)
        {
            string logFileName = string.Format("{0} - {1}", strLogNameCapaWS, "Metodo RegistrarVariablesBRMS");
            this.SetLogFileName(logFileName);
            GeneradorLog.EscribirLog(logFileName, null, string.Format("{0} {1} {0}", "********************", "Inicio método RegistrarVariablesBRMS"));
            try
            {
                BEAuditoriaRequest objAuditoria = GetAuditoria(objBEAuditoriaRequest);
                RestServiceDPGeneral.strArchivo = logFileName;

                return RestServiceDPGeneral.HttpCallInvoque<RegistrarVentaAccCuotasResponse>(WebRequestMethods.Http.Post, objBEAuditoriaRequest.urlRest, parametros, objRequest, null, objAuditoria);
            }
            catch (Exception ex)
            {
                GeneradorLog.EscribirLog(logFileName, null, string.Format("ERROR. Ha ocurrido un error en el método RegistrarVariablesBRMS - Message. {0}", ex.Message), null, ex);
                GeneradorLog.EscribirLog(logFileName, null, string.Format("ERROR. Ha ocurrido un error en el método RegistrarVariablesBRMS - StackTrace. {0}", ex.StackTrace), null, ex);
                throw;
            }
            finally
            {
                GeneradorLog.EscribirLog(logFileName, null, string.Format("{0} {1} {0}", "********************", "Fin método RegistrarVariablesBRMS"));
            }
        }

        public ValdCuoAccPendXLineaResponse ValidarCuoAccPendXLinea(Dictionary<string, string> parametros, ValdCuoAccPendXLineaRequest objRequest, BEAuditoriaRequest objBEAuditoriaRequest)
        {
            string logFileName = string.Format("{0} - {1}", strLogNameCapaWS, "Metodo RegistrarVariablesBRMS");
            this.SetLogFileName(logFileName);
            GeneradorLog.EscribirLog(logFileName, null, string.Format("{0} {1} {0}", "********************", "Inicio método ValidarCuoAccPendXLinea"));
            try
            {
                BEAuditoriaRequest objAuditoria = GetAuditoria(objBEAuditoriaRequest);
                RestServiceDPGeneral.strArchivo = logFileName;

                return RestServiceDPGeneral.HttpCallInvoque<ValdCuoAccPendXLineaResponse>(WebRequestMethods.Http.Post, objBEAuditoriaRequest.urlRest, parametros, objRequest, null, objAuditoria);
            }
            catch (Exception ex)
            {
                GeneradorLog.EscribirLog(logFileName, null, string.Format("ERROR. Ha ocurrido un error en el método RegistrarVariablesBRMS - Message. {0}", ex.Message), null, ex);
                GeneradorLog.EscribirLog(logFileName, null, string.Format("ERROR. Ha ocurrido un error en el método RegistrarVariablesBRMS - StackTrace. {0}", ex.StackTrace), null, ex);
                throw;
            }
            finally
            {
                GeneradorLog.EscribirLog(logFileName, null, string.Format("{0} {1} {0}", "********************", "Fin método ValidarCuoAccPendXLinea"));
            }
        }

        #region [CONSULTAR DATOS SOT] - [Para temas de Fija y ver si tienen SOT ACTIVAS, ETC]
        public GetDataConsultaSotResponse ConsultarDataSOT(Dictionary<string, string> parametros, GetDataConsultaSotRequest objRequest, BEAuditoriaRequest objBEAuditoriaRequest) 
        {
            string logFileName = string.Format("{0} - {1}", strLogNameCapaWS, "Metodo ConsultarDataSOT");
            this.SetLogFileName(logFileName);
            GeneradorLog.EscribirLog(logFileName, null, string.Format("{0} {1} {0}", "********************", "Inicio método ConsultarDataSOT"));

            try
            {
                BEAuditoriaRequest objAuditoria = GetAuditoria(objBEAuditoriaRequest);
                RestServiceDPGeneral.strArchivo = logFileName;

                return RestServiceDPGeneral.HttpCallInvoque<GetDataConsultaSotResponse>(WebRequestMethods.Http.Post, objBEAuditoriaRequest.urlRest, parametros, objRequest, null, objAuditoria);
            }
            catch (Exception ex)
            {
                GeneradorLog.EscribirLog(logFileName, null, string.Format("ERROR. Ha ocurrido un error en el método ConsultarDataSOT - Message. {0}", ex.Message), null, ex);
                GeneradorLog.EscribirLog(logFileName, null, string.Format("ERROR. Ha ocurrido un error en el método ConsultarDataSOT - StackTrace. {0}", ex.StackTrace), null, ex);
                throw;
            }
            finally
            {
                GeneradorLog.EscribirLog(logFileName, null, string.Format("{0} {1} {0}", "********************", "Fin método ConsultarDataSOT"));
            }


        }

        #endregion

        #endregion

    }

    #endregion

}
