using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Claro.SISACT.Entity;
using Claro.SISACT.Common;
using Claro.SISACT.WS.WSReglasCrediticia;
//emmh i
using System.IO;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;
using Claro.SISACT.WS.RestReferences; //PROY-140743 - INI
using Claro.SISACT.Entity.VentasCuotas.ObtenerVariablesBRMS.Response;
using System.Web;
using Claro.SISACT.Entity.VentasCuotas.RegistrarVariablesBRMS.Response;
using Claro.SISACT.Entity.VentasCuotas.RegistrarVariablesBRMS.Request; //PROY-140743
using Claro.SISACT.Entity.VentasCuotas.ObtenerDatosPedidoAccCuotas.Response;//PROY-140743
//emmh f
namespace Claro.SISACT.WS
{
    public class BWReglasCreditica
    {
        #region [Declaracion de Constantes - Config]

        ClaroEvalClientesReglasDecisionService _objTransaccion = new ClaroEvalClientesReglasDecisionService();
        GeneradorLog _objLog = null;
        string _input = null;
        string _idAplicacion = null;
        string _usuAplicacion = null;
        string _idTransaccion = null;

        #endregion [Declaracion de Constantes - Config]

        public BWReglasCreditica()
        {
            _objTransaccion.Url = ConfigurationManager.AppSettings["constURLReglasCrediticiaBRMS"].ToString();
            _objTransaccion.Credentials = System.Net.CredentialCache.DefaultCredentials;
            _objTransaccion.Timeout = Funciones.CheckInt(ConfigurationManager.AppSettings["TimeOut_ReglasCrediticia"].ToString());

            _idAplicacion = ConfigurationManager.AppSettings["CodigoAplicacion"].ToString();
            _usuAplicacion = ConfigurationManager.AppSettings["constNombreAplicacion"].ToString();
            _idTransaccion = DateTime.Now.ToString("hhmmssfff");
        }

        public BEOfrecimiento ConsultaReglaCrediticia(string nroDocumento,string prodFacturar,  ClaroEvalClientesReglasRequest objRequest, ref BEItemMensaje objMensaje)//PROY-140743
        {
            BEOfrecimiento objOfrecimiento = new BEOfrecimiento();
            _objLog = new GeneradorLog(null, nroDocumento, _idTransaccion, "WEB");
            //PROY-32439 V.2 :: INI
            objRequest.solicitud.solicitud1.cliente.antiguedadDeudaSpecified = true;
            objRequest.solicitud.solicitud1.cliente.antiguedadMontoDisputaSpecified = true;
            objRequest.solicitud.solicitud1.cliente.cantidadMontoDisputaSpecified = true;
            objRequest.solicitud.solicitud1.cliente.montoDeudaCastigadaSpecified = true;
            objRequest.solicitud.solicitud1.cliente.montoDeudaVencidaSpecified = true;
            objRequest.solicitud.solicitud1.cliente.montoDisputaSpecified = true;
            objRequest.solicitud.solicitud1.cliente.montoTotalDeudaSpecified = true;
            //PROY-32439 V.2 :: FIN

            ClaroEvalClientesReglasResponse objResponse = new ClaroEvalClientesReglasResponse();//PROY-140579

            try
            {
                _objLog.CrearArchivologWS("INICIO WS BRMS", _objTransaccion.Url, _input, null);
                //emmh I
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(ClaroEvalClientesReglasRequest));
                StringBuilder str = new StringBuilder();
                TextWriter tw = new StringWriter(str);
                x.Serialize(tw, objRequest);
                tw.Close();
                string xml = str.ToString();

                _objLog.CrearArchivolog("[xml][bodyRequestBRMS]", null, null);
                _objLog.CrearArchivolog("[xml][Inicio]" + xml, null, null);
                //emmh F
                //ClaroEvalClientesReglasResponse objResponse = new ClaroEvalClientesReglasResponse();

                objResponse = _objTransaccion.ClaroEvalClientesReglas(objRequest);

                //emmh I
                _objLog.CrearArchivolog("[xml][Fin]", null, null); 
                
                System.Xml.Serialization.XmlSerializer xResp = new System.Xml.Serialization.XmlSerializer(typeof(ClaroEvalClientesReglasResponse));

                StringBuilder strResp = new StringBuilder();
                TextWriter twResp = new StringWriter(strResp);
                xResp.Serialize(twResp, objResponse);
                twResp.Close();
                string xmlResp = strResp.ToString();
                _objLog.CrearArchivolog("[xml][bodyResponseBRMS]", null, null);
                _objLog.CrearArchivolog("[xml][Inicio]" + xmlResp, null, null);
                _objLog.CrearArchivolog("[xml][Fin]", null, null);
                //emmh F


                if (objRequest.solicitud.solicitud1.transaccion == ConfigurationManager.AppSettings["constTrxVentaCuotas"].ToString())
                {
                    BECuota objCuota;

                    if (objResponse.ofrecimiento.ofrecimiento1.cuota != null)
                    {
                        objOfrecimiento.ListaCuotas = new List<BECuota>();
                        foreach (cuota obj in objResponse.ofrecimiento.ofrecimiento1.cuota)
                        {
                            objCuota = new BECuota();
                            objCuota.nroCuota = obj.cantidad;
                            objCuota.porcenCuotaInicial = obj.porcentajeInicial;

                            //PROY-29123 Venta en Cuotas
                            objCuota.maximoCuotas = objResponse.ofrecimiento.ofrecimiento1.opcionDeCuotas.cuotaMaxima;
                            objCuota.maximoPrecioSoles = objResponse.ofrecimiento.ofrecimiento1.opcionDeCuotas.topeMaximo;
                            objCuota.mensajeCuota = objResponse.ofrecimiento.ofrecimiento1.opcionDeCuotas.mostrarRespuesta; //PROY-29123 Agregar Mensaje Devuelto por BRMS

                            objOfrecimiento.ListaCuotas.Add(objCuota);
                        }


                    }

                    if (objResponse.ofrecimiento.ofrecimiento1.cuota == null)
                    {
                        objOfrecimiento.MaximoCuotas = objResponse.ofrecimiento.ofrecimiento1.opcionDeCuotas.cuotaMaxima;
                        objOfrecimiento.PrecioEquipoMaximo = objResponse.ofrecimiento.ofrecimiento1.opcionDeCuotas.topeMaximo;
                        objOfrecimiento.MostrarMensaje = objResponse.ofrecimiento.ofrecimiento1.opcionDeCuotas.mostrarRespuesta; //PROY-29123 Agregar Mensaje Devuelto por BRMS
                    }

                    //PROY-31948 INI
                    objOfrecimiento.ResultadoEvaluacionCuotas = objResponse.ofrecimiento.ofrecimiento1.resultadoEvaluacionCuotas;
                    //PROY-31948 FIN
                }
                else
                {
                    objOfrecimiento.CantidadDeLineasAdicionalesRUC = objResponse.ofrecimiento.ofrecimiento1.autonomia.cantidadDeLineasAdicionalesRUC;
                    objOfrecimiento.CantidadDeLineasMaximas = objResponse.ofrecimiento.ofrecimiento1.autonomia.cantidadDeLineasMaximas;
                    objOfrecimiento.AutonomiaRenovacion = objResponse.ofrecimiento.ofrecimiento1.autonomia.cantidadDeLineasRenovaciones;
                    objOfrecimiento.MontoCFParaRUC = objResponse.ofrecimiento.ofrecimiento1.autonomia.montoCFParaRUC;
                    objOfrecimiento.TipoDeAutonomiaCargoFijo = objResponse.ofrecimiento.ofrecimiento1.autonomia.tipoDeAutonomiaCargoFijo;
                    objOfrecimiento.ControlDeConsumo = objResponse.ofrecimiento.ofrecimiento1.controlDeConsumo;
                    objOfrecimiento.CostoDeInstalacion = objResponse.ofrecimiento.ofrecimiento1.costoDeInstalacion;
                    objOfrecimiento.CantidadDeAplicacionesRenta = objResponse.ofrecimiento.ofrecimiento1.garantia.cantidadDeAplicacionesRenta;
                    objOfrecimiento.FrecuenciaDeAplicacionMensual = objResponse.ofrecimiento.ofrecimiento1.garantia.frecuenciaDeAplicacionMensual;
                    objOfrecimiento.MesInicioRentas = objResponse.ofrecimiento.ofrecimiento1.garantia.mesInicioRentas;
                    objOfrecimiento.MontoDeGarantia = objResponse.ofrecimiento.ofrecimiento1.garantia.montoDeGarantia;
                    objOfrecimiento.Tipodecobro = objResponse.ofrecimiento.ofrecimiento1.garantia.tipodecobro.ToString();
                    objOfrecimiento.TipoDeGarantia = objResponse.ofrecimiento.ofrecimiento1.garantia.tipoDeGarantia;
                    objOfrecimiento.LimiteDeCreditoCobranza = objResponse.ofrecimiento.ofrecimiento1.limiteDeCreditoCobranza;
                    objOfrecimiento.MontoTopeAutomatico = objResponse.ofrecimiento.ofrecimiento1.montoTopeAutomatico;
                    objOfrecimiento.PrioridadPublicar = objResponse.ofrecimiento.ofrecimiento1.prioridadPublicar.ToString();
                    objOfrecimiento.ProcesoDeExoneracionDeRentas = objResponse.ofrecimiento.ofrecimiento1.procesoDeExoneracionDeRentas.ToString();
                    objOfrecimiento.ProcesoIDValidator = objResponse.ofrecimiento.ofrecimiento1.procesoIDValidator.ToString();
                    objOfrecimiento.ProcesoValidacionInternaClaro = objResponse.ofrecimiento.ofrecimiento1.procesoValidacionInternaClaro.ToString();
                    objOfrecimiento.Publicar = objResponse.ofrecimiento.ofrecimiento1.publicar.ToString();
                    objOfrecimiento.Restriccion = objResponse.ofrecimiento.ofrecimiento1.restriccion.ToString();
                    objOfrecimiento.CapacidadDePago = objResponse.ofrecimiento.ofrecimiento1.resultadosAdicionales.capacidadDePago;
                    objOfrecimiento.ComportamientoConsolidado = objResponse.ofrecimiento.ofrecimiento1.resultadosAdicionales.comportamientoConsolidado;
                    objOfrecimiento.ComportamientoDePagoC1 = objResponse.ofrecimiento.ofrecimiento1.resultadosAdicionales.comportamientoDePagoC1;
                    objOfrecimiento.CostoTotalEquipos = objResponse.ofrecimiento.ofrecimiento1.resultadosAdicionales.costoTotalEquipos;
                    objOfrecimiento.FactorDeEndeudamientoCliente = objResponse.ofrecimiento.ofrecimiento1.resultadosAdicionales.factorDeEndeudamientoCliente;
                    objOfrecimiento.FactorDeRenovacionCliente = objResponse.ofrecimiento.ofrecimiento1.resultadosAdicionales.factorDeRenovacionCliente;
                    objOfrecimiento.PrecioDeVentaTotalEquipos = objResponse.ofrecimiento.ofrecimiento1.resultadosAdicionales.precioDeVentaTotalEquipos;
                    objOfrecimiento.RiesgoEnClaro = objResponse.ofrecimiento.ofrecimiento1.resultadosAdicionales.riesgoEnClaro;
                    objOfrecimiento.RiesgoOferta = objResponse.ofrecimiento.ofrecimiento1.resultadosAdicionales.riesgoOferta;
                    objOfrecimiento.RiesgoTotalEquipo = objResponse.ofrecimiento.ofrecimiento1.resultadosAdicionales.riesgoTotalEquipo;
                    objOfrecimiento.RiesgoTotalRepLegales = objResponse.ofrecimiento.ofrecimiento1.resultadosAdicionales.riesgoTotalRepLegales;
                    //Proy 29123 Venta en Cuotas
                    objOfrecimiento.MaximoCuotas = objResponse.ofrecimiento.ofrecimiento1.opcionDeCuotas.cuotaMaxima;
                    objOfrecimiento.PrecioEquipoMaximo = objResponse.ofrecimiento.ofrecimiento1.opcionDeCuotas.topeMaximo;
                    objOfrecimiento.MostrarMensaje = objResponse.ofrecimiento.ofrecimiento1.opcionDeCuotas.mostrarRespuesta;

                    //INI PROY-30748

                    List<BEPlan> Listplan = new List<BEPlan>();
                    if (objResponse.ofrecimiento.ofrecimiento1.v_LISTA_PLAN != null)
                    {

                        for (int i = 0; i <= objResponse.ofrecimiento.ofrecimiento1.v_LISTA_PLAN.Length - 1; i++)
                        {
                            BEPlan objplan = new BEPlan();
                            objplan.PLANV_DESCRIPCION = ((Claro.SISACT.WS.WSReglasCrediticia.respuestaProactiva[])(objResponse.ofrecimiento.ofrecimiento1.v_LISTA_PLAN))[i].descripcion;
                            _objLog.CrearArchivologWS("Planes Proactivos: ", objplan.PLANV_DESCRIPCION, null, null);
                            Listplan.Add(objplan);
                        }
                    }
                    objOfrecimiento.ListaPlanProac = Listplan;
                    //FIN PROY-30748
                    
                    //PROY-31948 INI 
                    objOfrecimiento.ResultadoEvaluacionCuotas = objResponse.ofrecimiento.ofrecimiento1.resultadoEvaluacionCuotas;
                    //PROY-31948 FIN

                    //PROY-29215 INICIO                
                    objOfrecimiento.ListarCuotasPago = new List<Int32>();
                    objOfrecimiento.ListaFormaPago = new List<String>();
                    if (objResponse.ofrecimiento.ofrecimiento1.cantidadCuotasCi != null && objResponse.ofrecimiento.ofrecimiento1.formaPagoCi != null)
                    {
                        foreach (var objcuota in objResponse.ofrecimiento.ofrecimiento1.cantidadCuotasCi)
                        {
                            objOfrecimiento.ListarCuotasPago.Add(Convert.ToInt32(objcuota));
                        }
                        objOfrecimiento.ListarCuotasPago = objOfrecimiento.ListarCuotasPago.OrderByDescending(p => p).ToList();
                    
                        foreach (var objFormapago in objResponse.ofrecimiento.ofrecimiento1.formaPagoCi)
                        {
                            objOfrecimiento.ListaFormaPago.Add(objFormapago);
                        }
                        objOfrecimiento.ListaFormaPago = objOfrecimiento.ListaFormaPago.OrderByDescending(p => p).ToList();
                   }
                    //PROY-29215 FIN
                    objOfrecimiento.ejecucionConsultaPrevia = objResponse.ofrecimiento.ofrecimiento1.ejecucionConsultaPrevia == WS.WSReglasCrediticia.tipoSiNo.SI ? "SI" : "NO"; //PROY-140335 RF1


                }
                //PROY-140439 BRMS CAMPAÑA NAVIDEÑA::INI
                if (objRequest.solicitud.solicitud1.transaccion == ConfigurationManager.AppSettings["constTrxValidacionCamp"].ToString())
                {
                    objOfrecimiento.CampanasNavidad = objResponse.ofrecimiento.ofrecimiento1.campaniasConRestriccion;
                }
                //PROY-140439 BRMS CAMPAÑA NAVIDEÑA::FIN

                //PROY-140579 INICIO RF02
                objOfrecimiento.MotivoDeRestriccion = objResponse.ofrecimiento.ofrecimiento1.motivoDeRestriccion;
                objOfrecimiento.MostrarMotivoDeRestriccion = objResponse.ofrecimiento.ofrecimiento1.mostrarMotivoDeRestriccion.ToString();
                //PROY-140579 FIN RF02

				//PROY-140546 Cobro Anticipado de Instalacion
                objOfrecimiento.TipoCobroAnticipadoInstalacion = Funciones.CheckStr( objResponse.ofrecimiento.ofrecimiento1.tipoCobroAnticipadoInstalacion);
                objOfrecimiento.MontoAnticipadoInstalacion = objResponse.ofrecimiento.ofrecimiento1.cobroAnticipadoInstalacion;
                _objLog.CrearArchivologWS("PROY-140546 TipoCobroAnticipadoInstalacion: ", Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.tipoCobroAnticipadoInstalacion), null, null);
                _objLog.CrearArchivologWS("PROY-140546 MontoAnticipadoInstalacion: ", Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.cobroAnticipadoInstalacion), null, null);
                //PROY-140546 Cobro Anticipado de Instalacion

                objMensaje = new BEItemMensaje();
            }
            catch (Exception ex)
            {
                objOfrecimiento.TieneAutonomia = "SIN_REGLAS";
                objOfrecimiento.Mensaje = ex.Message;
                objMensaje = new BEItemMensaje(ex.Source, ex.Message, false);

                _objLog.CrearArchivologWS("ERROR WS BRMS", null, null, ex);
            }
            finally
            {
                _objLog.CrearArchivologWS("FIN WS BRMS", null, null, null);
                _objTransaccion.Dispose();
            }

            #region [METODO INSERTAR DATOS EN HISTORIAL BRMS INICIO]
            try
            {
                //INI PROY-140579 NN                
                _objLog.CrearArchivologWS("INICIO LogHistorialConsultasBRMS", null, null, null);
                BWHistorialBrms objHistBrms = new BWHistorialBrms();
                string mensajeWS = Funciones.CheckStr(objOfrecimiento.Mensaje);
                bool resputa = objHistBrms.GrabarLogHistorialConsultasBRMS(objRequest, objResponse, mensajeWS);
                _objLog.CrearArchivologWS("FIN LogHistorialConsultasBRMS", null, null, null);
                //FIN PROY-140579 NN
            }
            catch (Exception ex)
            {
                _objLog.CrearArchivologWS("ERROR al insertar en Historial BRMS 140579 Message: ", null, null, ex);
            }
            #endregion [METODO INSERTAR DATOS EN HISTORIAL BRMS FIN]            

            return objOfrecimiento;
        }

        #region [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] | [Invocacion a servicios de Vta Cuotas]
        public ClaroEvalClientesReglasRequest ObtenerVariablesBRMSVtaCuotas(ClaroEvalClientesReglasRequest objRequest, string prodFacturar)
        {
            RestVentasCuotas rService = new RestVentasCuotas();
            ObtenerVariablesBRMSResponse objResponse = new ObtenerVariablesBRMSResponse();
            Dictionary<string, string> dcParameters = new Dictionary<string, string>();

            try
            {
                _objLog.CrearArchivologWS("PROY-140743 - IDEA-141192 - Venta Cuotas: ", string.Format("{0}{1}{0}", "**********************************************", " INICIO - ObtenerVariablesBRMSVtaCuotas "), null, null);
                string nroDocumento = Funciones.CheckStr(objRequest.solicitud.solicitud1.cliente.documento.numero);
                BEClienteCuenta objClienteConsulta = (BEClienteCuenta)HttpContext.Current.Session["objCliente" + nroDocumento];
                string strTipoDOC = Funciones.CheckStr(objClienteConsulta.tipoDoc);

                dcParameters.Add("tipoDoc", strTipoDOC);
                dcParameters.Add("numeroDoc", nroDocumento);
                dcParameters.Add("linea", string.Empty);
                dcParameters.Add("rangoHoras", ReadKeySettings.Key_RangoHoras);
                HttpContext.Current.Session["strMensajeError"] = string.Empty;

                #region Datos Auditoria
                BEAuditoriaRequest objBeAuditoriaRequest = new BEAuditoriaRequest();
                objBeAuditoriaRequest.idTransaccion = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                objBeAuditoriaRequest.timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
                objBeAuditoriaRequest.userId = Funciones.CheckStr(CurrentUsuario);
                objBeAuditoriaRequest.msgid = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                objBeAuditoriaRequest.dataPower = true;
                objBeAuditoriaRequest.accept = "application/json";
                objBeAuditoriaRequest.urlTimeOut_Rest = Funciones.CheckStr(ConfigurationManager.AppSettings["ConsMejorasPorta_TimeOut"]);
                objBeAuditoriaRequest.wsIp = Funciones.CheckStr(ConfigurationManager.AppSettings["conswsipPedidosTienda"]);
                objBeAuditoriaRequest.ipTransaccion = Funciones.CheckStr(HttpContext.Current.Session["CurrentTerminal"]);
                objBeAuditoriaRequest.usuarioAplicacion = Funciones.CheckStr(CurrentUsuario);
                objBeAuditoriaRequest.urlRest = "urlObtVarBRMS";
                objBeAuditoriaRequest.ipApplication = CurrentServer;

                _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0} ===> {1}", " [ObtenerVariablesBRMSVtaCuotas] [idTransaccion] ", objBeAuditoriaRequest.idTransaccion), null, null);
                _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0} ===> {1}", " [ObtenerVariablesBRMSVtaCuotas] [timestamp] ", objBeAuditoriaRequest.timestamp), null, null);
                _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0} ===> {1}", " [ObtenerVariablesBRMSVtaCuotas] [userId] ", objBeAuditoriaRequest.userId), null, null);
                _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0} ===> {1}", " [ObtenerVariablesBRMSVtaCuotas] [msgid] ", objBeAuditoriaRequest.msgid), null, null);
                _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0} ===> {1}", " [ObtenerVariablesBRMSVtaCuotas] [dataPower] ", Funciones.CheckStr(objBeAuditoriaRequest.dataPower)), null, null);
                _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0} ===> {1}", " [ObtenerVariablesBRMSVtaCuotas] [accept] ", objBeAuditoriaRequest.accept), null, null);
                _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0} ===> {1}", " [ObtenerVariablesBRMSVtaCuotas] [urlTimeOut_Rest] ", objBeAuditoriaRequest.urlTimeOut_Rest), null, null);
                _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0} ===> {1}", " [ObtenerVariablesBRMSVtaCuotas] [wsIp] ", objBeAuditoriaRequest.wsIp), null, null);
                _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0} ===> {1}", " [ObtenerVariablesBRMSVtaCuotas] [ipTransaccion] ", objBeAuditoriaRequest.ipTransaccion), null, null);
                _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0} ===> {1}", " [ObtenerVariablesBRMSVtaCuotas] [usuarioAplicacion] ", objBeAuditoriaRequest.usuarioAplicacion), null, null);
                _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0} ===> {1}", " [ObtenerVariablesBRMSVtaCuotas] [urlRest] ", objBeAuditoriaRequest.urlRest), null, null);
                _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0} ===> {1}", " [ObtenerVariablesBRMSVtaCuotas] [ipApplication] ", objBeAuditoriaRequest.ipApplication), null, null);

                #endregion

                #region response servicio

                objResponse = rService.ObtenerVariablesBRMS(dcParameters, objBeAuditoriaRequest);

                string strCodRespuesta = Funciones.CheckStr(objResponse.MessageResponse.Body.variablesBRMSResponse.responseStatus.codigoRespuesta);
                string strMsjRespuesta = Funciones.CheckStr(objResponse.MessageResponse.Body.variablesBRMSResponse.responseStatus.mensajeRespuesta);

                _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0} ===> {1}", " [ObtenerVariablesBRMSVtaCuotas] [codigoRespuesta] ", strCodRespuesta), null, null);
                _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0} ===> {1}", " [ObtenerVariablesBRMSVtaCuotas] [mensajeRespuesta] ", strMsjRespuesta), null, null);

                if (strCodRespuesta.Equals("0"))
                {
                    objRequest.solicitud.solicitud1.cliente.montoCuotasPendientesAcc = Funciones.CheckDbl(objResponse.MessageResponse.Body.variablesBRMSResponse.responseData.montoPendCuotas);
                    objRequest.solicitud.solicitud1.cliente.cantidadLineaCuotasPendientesAcc = Funciones.CheckInt(objResponse.MessageResponse.Body.variablesBRMSResponse.responseData.cantPendCuotas); ;
                    objRequest.solicitud.solicitud1.cliente.cantidadMaximaCuotasPendientesAcc = Funciones.CheckInt(objResponse.MessageResponse.Body.variablesBRMSResponse.responseData.cantMaxPendCuotas);
                    objRequest.solicitud.solicitud1.cliente.montoCuotasPendientesAccUltiVenta = Funciones.CheckDbl(objResponse.MessageResponse.Body.variablesBRMSResponse.responseData.montoPendUltVentas);
                    objRequest.solicitud.solicitud1.cliente.cantidadLineaCuotasPendientesAccUltiVenta = Funciones.CheckInt(objResponse.MessageResponse.Body.variablesBRMSResponse.responseData.cantPendUltVentas);
                    objRequest.solicitud.solicitud1.cliente.cantidadMaximaCuotasPendientesAccUltiVenta = Funciones.CheckInt(objResponse.MessageResponse.Body.variablesBRMSResponse.responseData.cantMaxPendUltVentas);
                    
                    objRequest.solicitud.solicitud1.oferta.promociones = Funciones.CheckStr(HttpContext.Current.Session["strPromocionVV"]);
                    objRequest.solicitud.solicitud1.oferta.productoCuentaAFacturar = Funciones.CheckStr(prodFacturar);
                }
                else
                {
                    HttpContext.Current.Session["strMensajeError"] = Funciones.CheckStr(ReadKeySettings.Key_MsjNoVariablesBRMS);
                }
                #endregion
            }
            catch (Exception ex)
            {
                HttpContext.Current.Session["strMensajeError"] = Funciones.CheckStr(ReadKeySettings.Key_MsjNoVariablesBRMS);
                _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0}{1}{0}", "**********************************************", " ERROR - ObtenerVariablesBRMSVtaCuotas "), null, null);
                _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0} ===> {1}", " [ObtenerVariablesBRMSVtaCuotas] [Message] ", ex.Message), null, null);
                _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0} ===> {1}", " [ObtenerVariablesBRMSVtaCuotas] [StackTrace] ", ex.StackTrace), null, null);
            }

            _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0}{1}{0}", "**********************************************", " FIN - ObtenerVariablesBRMSVtaCuotas "), null, null);

            return objRequest;
        }

        public static List<BEObtenerDatosPedidoAccCuotas> ObtenerDatosPedidoAccCuotas(string numeroPedido, string lineaFacturar, string docCliente, string secCliente, string usuario, string CurrentUsers)
        {
            GeneradorLog _objLog = new GeneradorLog(CurrentUsers, null, null, "WEB");

            List<BEObtenerDatosPedidoAccCuotas> objRespuesta = new List<BEObtenerDatosPedidoAccCuotas>();
            string codigoRespuestaServidor = string.Empty;
            string mensajeRespuestaServidor = string.Empty;
            try
            {
                ObtenerDatosPedidoAccCuotasResponse objObtenerDatos = new ObtenerDatosPedidoAccCuotasResponse();
                BEAuditoriaRequest objBEAuditoriaRequest = new BEAuditoriaRequest();
                RestVentasCuotas objRestVentasCuo = new RestVentasCuotas();

                Dictionary<string, string> parametros = new Dictionary<string, string>();
                #region Auditoria
                objBEAuditoriaRequest.idTransaccion = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                objBEAuditoriaRequest.timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
                objBEAuditoriaRequest.userId = Funciones.CheckStr(usuario);
                objBEAuditoriaRequest.msgid = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                objBEAuditoriaRequest.dataPower = true;
                objBEAuditoriaRequest.accept = "application/json";
                objBEAuditoriaRequest.urlRest = "constUrlObtenerDatosPedidoAccCuotas";
                objBEAuditoriaRequest.urlTimeOut_Rest = Funciones.CheckStr(ConfigurationManager.AppSettings["TimeOutVtaCuoAcc"]);
                objBEAuditoriaRequest.wsIp = Funciones.CheckStr(ConfigurationManager.AppSettings["conswsipPedidosTienda"]);
                objBEAuditoriaRequest.ipTransaccion = Funciones.CheckStr(HttpContext.Current.Session["CurrentTerminal"]);
                objBEAuditoriaRequest.usuarioAplicacion = Funciones.CheckStr(usuario);
                objBEAuditoriaRequest.ipApplication = Funciones.CheckStr(CurrentServer);
                _objLog.CrearArchivolog(string.Format("{0} : {1}", "[PROY-140743][sisact_evaluacion_unificada][objObtenerDatos]|Auditoria, idTransaccion: ", Funciones.CheckStr(objBEAuditoriaRequest.idTransaccion)), null, null);
                _objLog.CrearArchivolog(string.Format("{0} : {1}", "[PROY-140743][sisact_evaluacion_unificada][objObtenerDatos]|Auditoria timestamp: ", Funciones.CheckStr(objBEAuditoriaRequest.timestamp)), null, null);
                _objLog.CrearArchivolog(string.Format("{0} : {1}", "[PROY-140743][sisact_evaluacion_unificada][objObtenerDatos]|Auditoria userId: ", Funciones.CheckStr(objBEAuditoriaRequest.userId)), null, null);
                _objLog.CrearArchivolog(string.Format("{0} : {1}", "[PROY-140743][sisact_evaluacion_unificada][objObtenerDatos]|Auditoria msgid: ", Funciones.CheckStr(objBEAuditoriaRequest.msgid)), null, null);
                _objLog.CrearArchivolog(string.Format("{0} : {1}", "[PROY-140743][sisact_evaluacion_unificada][objObtenerDatos]|Auditoria accept: ", Funciones.CheckStr(objBEAuditoriaRequest.accept)), null, null);
                _objLog.CrearArchivolog(string.Format("{0} : {1}", "[PROY-140743][sisact_evaluacion_unificada][objObtenerDatos]|Auditoria wsIp: ", Funciones.CheckStr(objBEAuditoriaRequest.wsIp)), null, null);
                _objLog.CrearArchivolog(string.Format("{0} : {1}", "[PROY-140743][sisact_evaluacion_unificada][objObtenerDatos]|Auditoria ipTransaccion: ", Funciones.CheckStr(objBEAuditoriaRequest.ipTransaccion)), null, null);
                _objLog.CrearArchivolog(string.Format("{0} : {1}", "[PROY-140743][sisact_evaluacion_unificada][objObtenerDatos]|Auditoria usuarioAplicacion: ", Funciones.CheckStr(objBEAuditoriaRequest.usuarioAplicacion)), null, null);
                _objLog.CrearArchivolog(string.Format("{0} : {1}", "[PROY-140743][sisact_evaluacion_unificada][objObtenerDatos]|Auditoria ipApplication: ", Funciones.CheckStr(objBEAuditoriaRequest.ipApplication)), null, null);

                #endregion

                #region Body
                _objLog.CrearArchivolog("---- Parametros de entrada Inicio URL: ----", null, null);
                _objLog.CrearArchivolog(string.Format("{0} : {1}", "[PROY-140743][sisact_evaluacion_unificada][objObtenerDatos]|Parametros, numeroPedido: ", numeroPedido), null, null);
                _objLog.CrearArchivolog(string.Format("{0} : {1}", "[PROY-140743][sisact_evaluacion_unificada][objObtenerDatos]|Auditoria lineaFacturar: ", lineaFacturar), null, null);
                _objLog.CrearArchivolog(string.Format("{0} : {1}", "[PROY-140743][sisact_evaluacion_unificada][objObtenerDatos]|Auditoria docCliente: ", docCliente), null, null);
                parametros.Add("numeroPedido", numeroPedido);
                parametros.Add("lineaFacturar", lineaFacturar);
                parametros.Add("docCliente", docCliente);
                parametros.Add("nroSec", secCliente);
                #endregion

                objObtenerDatos = objRestVentasCuo.ObtenerDatosPedidoAccCuotas(parametros, objBEAuditoriaRequest);

                codigoRespuestaServidor = Funciones.CheckStr(objObtenerDatos.MessageResponse.Body.datosPedidoResponse.responseStatus.codigoRespuesta);
                mensajeRespuestaServidor = Funciones.CheckStr(objObtenerDatos.MessageResponse.Body.datosPedidoResponse.responseStatus.mensajeRespuesta);

                _objLog.CrearArchivolog(string.Format("codigoRespuestaServidor => {0}", codigoRespuestaServidor), null, null);
                _objLog.CrearArchivolog(string.Format("mensajeRespuestaServidor => {0}", mensajeRespuestaServidor), null, null);

                if (codigoRespuestaServidor == "0")
                {
                    objRespuesta = (objObtenerDatos.MessageResponse.Body.datosPedidoResponse.responseData);
                    _objLog.CrearArchivolog(string.Format("{0} : {1}", "[PROY-140743][sisact_evaluacion_unificada][objObtenerDatos]|objRespuesta, idVentaCuota: ", Funciones.CheckStr(objRespuesta[0].idVentaCuota)), null, null);
                    _objLog.CrearArchivolog(string.Format("{0} : {1}", "[PROY-140743][sisact_evaluacion_unificada][objObtenerDatos]|objRespuesta, numeroPedido: ", Funciones.CheckStr(objRespuesta[0].numeroPedido)), null, null);
                    _objLog.CrearArchivolog(string.Format("{0} : {1}", "[PROY-140743][sisact_evaluacion_unificada][objObtenerDatos]|objRespuesta, lineaFacturar: ", Funciones.CheckStr(objRespuesta[0].lineaFacturar)), null, null);
                    _objLog.CrearArchivolog(string.Format("{0} : {1}", "[PROY-140743][sisact_evaluacion_unificada][objObtenerDatos]|objRespuesta, modalidadVenta: ", Funciones.CheckStr(objRespuesta[0].modalidadVenta)), null, null);
                    _objLog.CrearArchivolog(string.Format("{0} : {1}", "[PROY-140743][sisact_evaluacion_unificada][objObtenerDatos]|objRespuesta, numeroCuotas: ", Funciones.CheckStr(objRespuesta[0].numeroCuotas)), null, null);
                    _objLog.CrearArchivolog(string.Format("{0} : {1}", "[PROY-140743][sisact_evaluacion_unificada][objObtenerDatos]|objRespuesta, flagCargoRecibo: ", Funciones.CheckStr(objRespuesta[0].flagCargoRecibo)), null, null);
                    _objLog.CrearArchivolog(string.Format("{0} : {1}", "[PROY-140743][sisact_evaluacion_unificada][objObtenerDatos]|objRespuesta, cuotaInicialFinal: ", Funciones.CheckStr(objRespuesta[0].cuotaInicialFinal)), null, null);
                    _objLog.CrearArchivolog(string.Format("{0} : {1}", "[PROY-140743][sisact_evaluacion_unificada][objObtenerDatos]|objRespuesta, tipoDocEvalCred: ", Funciones.CheckStr(objRespuesta[0].tipoDocEvalCred)), null, null);
                    _objLog.CrearArchivolog(string.Format("{0} : {1}", "[PROY-140743][sisact_evaluacion_unificada][objObtenerDatos]|objRespuesta, numDocEvalCred: ", Funciones.CheckStr(objRespuesta[0].numDocEvalCred)), null, null);
                    _objLog.CrearArchivolog(string.Format("{0} : {1}", "[PROY-140743][sisact_evaluacion_unificada][objObtenerDatos]|objRespuesta, numeroSec: ", Funciones.CheckStr(objRespuesta[0].numeroSec)), null, null);
                    _objLog.CrearArchivolog(string.Format("{0} : {1}", "[PROY-140743][sisact_evaluacion_unificada][objObtenerDatos]|objRespuesta, cuocCodigo: ", Funciones.CheckStr(objRespuesta[0].cuocCodigo)), null, null);
                    _objLog.CrearArchivolog(string.Format("{0} : {1}", "[PROY-140743][sisact_evaluacion_unificada][objObtenerDatos]|objRespuesta, cuotasBrms: ", Funciones.CheckStr(objRespuesta[0].cuotasBrms)), null, null);
                    _objLog.CrearArchivolog(string.Format("{0} : {1}", "[PROY-140743][sisact_evaluacion_unificada][objObtenerDatos]|objRespuesta, precioLista: ", Funciones.CheckStr(objRespuesta[0].precioLista)), null, null);
                    _objLog.CrearArchivolog(string.Format("{0} : {1}", "[PROY-140743][sisact_evaluacion_unificada][objObtenerDatos]|objRespuesta, cargoFijo: ", Funciones.CheckStr(objRespuesta[0].cargoFijo)), null, null);
                    _objLog.CrearArchivolog(string.Format("{0} : {1}", "[PROY-140743][sisact_evaluacion_unificada][objObtenerDatos]|objRespuesta, cuotaInicial: ", Funciones.CheckStr(objRespuesta[0].cuotaInicial)), null, null);
                    _objLog.CrearArchivolog(string.Format("{0} : {1}", "[PROY-140743][sisact_evaluacion_unificada][objObtenerDatos]|objRespuesta, flagRRLLEvalCred: ", Funciones.CheckStr(objRespuesta[0].flagRRLLEvalCred)), null, null);
                    _objLog.CrearArchivolog(string.Format("{0} : {1}", "[PROY-140743][sisact_evaluacion_unificada][objObtenerDatos]|objRespuesta, codPromocion: ", Funciones.CheckStr(objRespuesta[0].codPromocion)), null, null);
                    _objLog.CrearArchivolog(string.Format("{0} : {1}", "[PROY-140743][sisact_evaluacion_unificada][objObtenerDatos]|objRespuesta, descPromocion: ", Funciones.CheckStr(objRespuesta[0].descPromocion)), null, null);
                    _objLog.CrearArchivolog(string.Format("{0} : {1}", "[PROY-140743][sisact_evaluacion_unificada][objObtenerDatos]|objRespuesta, codAccesorio: ", Funciones.CheckStr(objRespuesta[0].codAccesorio)), null, null);
                    _objLog.CrearArchivolog(string.Format("{0} : {1}", "[PROY-140743][sisact_evaluacion_unificada][objObtenerDatos]|objRespuesta, grupoMaterial: ", Funciones.CheckStr(objRespuesta[0].grupoMaterial)), null, null);
                    _objLog.CrearArchivolog(string.Format("{0} : {1}", "[PROY-140743][sisact_evaluacion_unificada][objObtenerDatos]|objRespuesta, servidorSEC: ", Funciones.CheckStr(objRespuesta[0].servidorSEC)), null, null);
                    _objLog.CrearArchivolog(string.Format("{0} : {1}", "[PROY-140743][sisact_evaluacion_unificada][objObtenerDatos]|objRespuesta, tipoProdFacturar: ", Funciones.CheckStr(objRespuesta[0].tipoProdFacturar)), null, null);
                    _objLog.CrearArchivolog(string.Format("{0} : {1}", "[PROY-140743][sisact_evaluacion_unificada][objObtenerDatos]|objRespuesta, servidorVenta: ", Funciones.CheckStr(objRespuesta[0].servidorVenta)), null, null);
                    _objLog.CrearArchivolog(string.Format("{0} : {1}", "[PROY-140743][sisact_evaluacion_unificada][objObtenerDatos]|objRespuesta, flagPagoSec: ", Funciones.CheckStr(objRespuesta[0].descPlanFijo)), null, null);
                    _objLog.CrearArchivolog(string.Format("{0}{1}{0}", "*********************", "FIN MÉTODO objObtenerDatos"), null, null);
                }
            }
            catch (Exception ex)
            {
                _objLog.CrearArchivolog(string.Format("Error => {0}", ex.Message), null, null);
                _objLog.CrearArchivolog(string.Format("Error => {0}", ex.StackTrace), null, null);
            }
            return objRespuesta;
        }

        static string CurrentUsuario
        {
            get
            {
                string strDomainUser = HttpContext.Current.Request.ServerVariables["LOGON_USER"];
                string strUser = strDomainUser.Substring(strDomainUser.IndexOf("\\", System.StringComparison.Ordinal) + 1);
                return strUser.ToUpper();
            }
        }

        static string CurrentServer
        {
            get
            {
                String nombreHost = System.Net.Dns.GetHostName();
                String nombreServer = System.Net.Dns.GetHostName();
                String ipServer = System.Net.Dns.GetHostAddresses(nombreServer)[0].ToString();
                return ipServer;
            }
        }

        static string CurrentTerminal
        {
            get
            {
                string ip_cliente = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(ip_cliente))
                {
                    ip_cliente = HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
                }
                return ip_cliente;
            }
        }

        static string InvertObtenerDoc(string ObtenerDocumento)
        {
            if (ObtenerDocumento.Equals(WS.WSReglasCrediticia.tipoDeDocumento.DNI))
                return Funciones.CheckStr(ConfigurationManager.AppSettings["constCodTipoDocumentoDNI"]);
            if (ObtenerDocumento.Equals(WS.WSReglasCrediticia.tipoDeDocumento.RUC10) || ObtenerDocumento.Equals(WS.WSReglasCrediticia.tipoDeDocumento.RUC20))
                return Funciones.CheckStr(ConfigurationManager.AppSettings["constCodTipoDocumentoRUC"]);
            if (ObtenerDocumento.Equals(WS.WSReglasCrediticia.tipoDeDocumento.CE))
                return Funciones.CheckStr(ConfigurationManager.AppSettings["constCodTipoDocumentoCEX"]);
            if (ObtenerDocumento.Equals(WS.WSReglasCrediticia.tipoDeDocumento.CIRE))
                return ReadKeySettings.Key_codigoDocCIRE;
            if (ObtenerDocumento.Equals(WS.WSReglasCrediticia.tipoDeDocumento.CIE))
                return ReadKeySettings.Key_codigoDocCIE;
            if (ObtenerDocumento.Equals(WS.WSReglasCrediticia.tipoDeDocumento.CPP))
                return ReadKeySettings.Key_codigoDocCPP;
            if (ObtenerDocumento.Equals(WS.WSReglasCrediticia.tipoDeDocumento.CTM))
                return ReadKeySettings.Key_codigoDocCTM;

            return Funciones.CheckStr(ConfigurationManager.AppSettings["constCodTipoDocumentoDNI"]);
        }

        #endregion
    }
}
