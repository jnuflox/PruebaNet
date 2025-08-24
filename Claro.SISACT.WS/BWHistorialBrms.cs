//INI PROY-140579 NN
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using System.Configuration;
using Claro.SISACT.Entity;
using Claro.SISACT.Common;
using Claro.SISACT.WS.WSReglasCrediticia;
using System.Web;

//para el servicio dataPower
using Claro.SISACT.Entity.RegistrarEvaluacionBRMSDP.Response;
using Claro.SISACT.Entity.RegistrarEvaluacionBRMSDP.Request;
using Claro.SISACT.Entity.DataPowerRest;
using Claro.SISACT.Entity.ActualizarEvaluacionBRMSDP.Request; 
using Claro.SISACT.Entity.ActualizarEvaluacionBRMSDP.Response;
using System.Collections;
using System.Configuration;

namespace Claro.SISACT.WS
{
    public class BWHistorialBrms
    {
        //INSERTANDO EN EL HISTORIAL DE BRMS
        public bool GrabarLogHistorialConsultasBRMS(ClaroEvalClientesReglasRequest oEvaluacion, ClaroEvalClientesReglasResponse objResponse, string mensajeWS)
        {
            bool rptaMetodo = true;
            string nameArchivo = "GrabarLogHistorialConsultasBRMS";
            HelperLog.EscribirLog("", nameArchivo, "" + "[ -- INICIO GrabarLogHistorialConsultasBRMS -- ]" + " - CONSTRUYENDO REQUEST - 140579 -", false);

            try
            {
                string datosCliente = (String)HttpContext.Current.Session["objClienteDI"];
                string[] arrdatosCliente = datosCliente.Split(Convert.ToChar("|"));                
                string codigoTipoClienteBRMS = arrdatosCliente[0].Trim();
                string numeroDocumentoClienteBRMS = arrdatosCliente[1].Trim();
                string tipoTransaccionBRMS = arrdatosCliente[2].Trim();
                HelperLog.EscribirLog("", nameArchivo, "" + "[ -- GrabarLogHistorialConsultasBRMS -- ]" + " - TIPO DE TRANSACCION:  " + tipoTransaccionBRMS, false);

                if (tipoTransaccionBRMS == "Evaluar Cliente" ||  tipoTransaccionBRMS == "Validacion Campana")
                {
                    HelperLog.EscribirLog("", nameArchivo, "" + "[ -- GrabarLogHistorialConsultasBRMS -- ]" + " - TIPO DE TRANSACCION OBTENIDA:  " + tipoTransaccionBRMS, false);
                    return true;
                }
                else
                {
                    tipoTransaccionBRMS = (tipoTransaccionBRMS == "Evaluar") ? tipoTransaccionBRMS = "EVALUACION CREDITICIA" : tipoTransaccionBRMS = tipoTransaccionBRMS.ToString();
                    tipoTransaccionBRMS = (tipoTransaccionBRMS == "Evaluar Cuota") ? tipoTransaccionBRMS = "VENTA EN CUOTAS" : tipoTransaccionBRMS = tipoTransaccionBRMS.ToString();
                }
                
                StringBuilder strSolicitud = new StringBuilder();
                StringBuilder strCliente = new StringBuilder();
                StringBuilder strDirCliente = new StringBuilder();
                StringBuilder strDocCliente = new StringBuilder();
                StringBuilder strOferta = new StringBuilder();
                StringBuilder strCampana = new StringBuilder();
                //StringBuilder strPlanActual = new StringBuilder();
                string strPlanActual = string.Empty;
                StringBuilder strPlanSolicitado = new StringBuilder();
                StringBuilder strPuntoVenta = new StringBuilder();
                StringBuilder strPuntoVentaDireccion = new StringBuilder();
                StringBuilder strRepLegal = new StringBuilder();
                StringBuilder strServicio = new StringBuilder();
                StringBuilder strEquipo = new StringBuilder();
                StringBuilder strContribuyente = new StringBuilder();

                //INPUTS oEvaluacion

                if (oEvaluacion != null)
                {
                    if (oEvaluacion.solicitud != null)
                    {
                        if (oEvaluacion.solicitud.solicitud1 != null)
                        {
                            //IN_SOLICITUd
                            strSolicitud.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.buroConsultado));
                            strSolicitud.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cargoFijoDeBolsa));
                            strSolicitud.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.fechaEjecucion));
                            strSolicitud.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.flagDeLicitacion));
                            strSolicitud.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.horaEjecucion));
                            strSolicitud.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.tipoDeBolsa));
                            strSolicitud.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.tipoDeDespacho));
                            strSolicitud.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.tipoDeOperacion));
                            strSolicitud.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.totalPlanes));
                            strSolicitud.AppendFormat("{0}", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.transaccion));

                            if (oEvaluacion.solicitud.solicitud1.cliente != null)
                            {
                                //IN_CLIENTE
                                strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.antiguedadDeuda));
                                strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.antiguedadMontoDisputa));
                                strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.cantidadDeLineasActivas));
                                strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.cantidadDePlanesPorProducto));
                                strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.cantidadDeRepresentantesLegales));
                                strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.cantidadMaximaCuotasPendientes));
                                strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.cantidadMaximaCuotasPendientesSistema));
                                strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.cantidadMaximaCuotasPendientesUltimasVentas));
                                strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.cantidadMontoDisputa));
                                strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.cantidadPlanesCuotasPendientesSistema));
                                strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.cantidadPlanesCuotasPendientesUltimasVentas));
                                strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.cantidadTotalPlanesCuotasPendientes));
                                strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.comportamientoDePago));
                                strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.creditScore));
                                strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.creditScoreEntero));
                                strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.deuda));
                                strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.edad));
                                strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.facturacionPromedioClaro));
                                strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.facturacionPromedioProducto));
                                
                                strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.flagWhitelist));//140579 RESALTADO
                                strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.limiteDeCreditoDisponible));
                                strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.montoDeudaCastigada));
                                strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.montoDeudaVencida));
                                strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.montoDisputa));
                                strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.montoPendienteCuotasSistema));
                                strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.montoPendienteCuotasUltimasVentas));
                                strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.montoTotalDeuda));
                                strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.montoTotalPendienteCuotas));
                                strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.riesgo));
                                strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.riesgoTotalRepresentantesLegales));
                                strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.segmento));
                                strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.sexo));
                                strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.tiempoDePermanencia));
                                strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.tipo));

                                if (oEvaluacion.solicitud.solicitud1.cliente.contribuyente != null)
                                {
                                    strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.contribuyente.autorizacionImpresion));
                                    strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.contribuyente.cantidadMesesInicioActividades));
                                    strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.contribuyente.cantidadTrabajadores));
                                    strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.contribuyente.CIU));
                                    strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.contribuyente.condicion));
                                    strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.contribuyente.estado));
                                    strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.contribuyente.fechaInicioActividades));
                                    strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.contribuyente.nombreComercial));
                                    strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.contribuyente.sistemaEmisionElectronica));
                                    strCliente.AppendFormat("{0}", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.contribuyente.tipo));
                                }
                                else
                                {
                                    strCliente.AppendFormat("{0}|", "");
                                    strCliente.AppendFormat("{0}|", "");
                                    strCliente.AppendFormat("{0}|", "");
                                    strCliente.AppendFormat("{0}|", "");
                                    strCliente.AppendFormat("{0}|", "");
                                    strCliente.AppendFormat("{0}|", "");
                                    strCliente.AppendFormat("{0}|", "");//40
                                    strCliente.AppendFormat("{0}|", "");
                                    strCliente.AppendFormat("{0}|", "");                                    
                                    strCliente.AppendFormat("{0}|", "");//PROY-140743 - VENTA CUOTAS
                                }

                                #region [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] - [Obtener los datos de para el registro de las nuevas variables] - [Cliente]

                                bool strMensajeBrms = (bool)HttpContext.Current.Session["strRespuestaBRMS"];
                                if (strMensajeBrms)
                                {
                                    strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.montoCuotasPendientesAcc));//44
                                    strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.cantidadLineaCuotasPendientesAcc));//45
                                    strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.cantidadMaximaCuotasPendientesAcc));//46
                                    strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.montoCuotasPendientesAccUltiVenta));//47
                                    strCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.cantidadLineaCuotasPendientesAccUltiVenta));//48
                                    strCliente.AppendFormat("{0}", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.cantidadMaximaCuotasPendientesAccUltiVenta));//49
                                }
                                else
                                {
                                    strCliente.AppendFormat("{0}|", "");
                                    strCliente.AppendFormat("{0}|", "");
                                    strCliente.AppendFormat("{0}|", "");
                                    strCliente.AppendFormat("{0}|", "");
                                    strCliente.AppendFormat("{0}|", "");
                                    strCliente.AppendFormat("{0}", "");
                                }

                                #endregion

                                if (oEvaluacion.solicitud.solicitud1.cliente.direccion != null)
                                {
                                    //IN_IRECCION_CLIENTE
                                    strDirCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.direccion.codigoDePlano));
                                    strDirCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.direccion.departamento));
                                    strDirCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.direccion.distrito));
                                    strDirCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.direccion.provincia));
                                    strDirCliente.AppendFormat("{0}", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.direccion.region));
                                }

                                if (oEvaluacion.solicitud.solicitud1.cliente.documento != null)
                                {
                                    //IN_DOCUMENTO_CLIENTE
                                    strDocCliente.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.documento.descripcion));
                                    strDocCliente.AppendFormat("{0}", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.cliente.documento.numero));
                                }

                                if (oEvaluacion.solicitud.solicitud1.cliente.representanteLegal != null)
                                {
                                    //IN_RRLL_CLIENTE
                                    foreach (WS.WSReglasCrediticia.representanteLegal rrll in oEvaluacion.solicitud.solicitud1.cliente.representanteLegal)
                                    {
                                        strRepLegal.AppendFormat("{0}|", Funciones.CheckStr(rrll.cargo));
                                        strRepLegal.AppendFormat("{0}|", Funciones.CheckStr(rrll.documento.descripcion));
                                        strRepLegal.AppendFormat("{0}|", Funciones.CheckStr(rrll.documento.numero));
                                        strRepLegal.AppendFormat("{0}|", Funciones.CheckStr(rrll.fechaNombramiento.ToString("dd/MM/yyyy")));
                                        strRepLegal.AppendFormat("{0}|", Funciones.CheckStr(rrll.cantidadMesesNombramiento));
                                        strRepLegal.AppendFormat("{0}|", Funciones.CheckStr(rrll.riesgo));
                                    }
                                }

                            }

                            if (oEvaluacion.solicitud.solicitud1.equipo != null)
                            {
                                //IN_EQUIPO
                                foreach (WS.WSReglasCrediticia.equipo oEquipo in oEvaluacion.solicitud.solicitud1.equipo)
                                {
                                    strEquipo.AppendFormat("{0}|", Funciones.CheckStr(oEquipo.costo));
                                    strEquipo.AppendFormat("{0}|", Funciones.CheckStr(oEquipo.cuotas));
                                    strEquipo.AppendFormat("{0}|", Funciones.CheckStr(oEquipo.factorDePagoInicial));
                                    strEquipo.AppendFormat("{0}|", Funciones.CheckStr(oEquipo.factorDeSubsidio));
                                    strEquipo.AppendFormat("{0}|", Funciones.CheckStr(oEquipo.formaDePago));
                                    strEquipo.AppendFormat("{0}|", Funciones.CheckStr(oEquipo.gama));
                                    strEquipo.AppendFormat("{0}|", Funciones.CheckStr(oEquipo.modelo));
                                    strEquipo.AppendFormat("{0}|", Funciones.CheckStr(oEquipo.montoDeCuota));
                                    strEquipo.AppendFormat("{0}|", Funciones.CheckStr(oEquipo.montoDeCuotaComercial));
                                    strEquipo.AppendFormat("{0}|", Funciones.CheckStr(oEquipo.montoDeCuotaInicialComercial));
                                    strEquipo.AppendFormat("{0}|", Funciones.CheckStr(oEquipo.porcentajecuotaInicial));
                                    strEquipo.AppendFormat("{0}|", Funciones.CheckStr(oEquipo.precioDeVenta));
                                    strEquipo.AppendFormat("{0}|", Funciones.CheckStr(oEquipo.tipoDeDeco));
                                    strEquipo.AppendFormat("{0}", Funciones.CheckStr(oEquipo.tipoOperacionKit));//PROY-140743

                                    strEquipo = strEquipo.Replace("\t", " ");
                                }
                            }

                            if (oEvaluacion.solicitud.solicitud1.oferta != null)
                            {
                                //IN_OFERTA
                                strOferta.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.oferta.cantidadDeDecos));
                                strOferta.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.oferta.cantidadLineasSEC));
                                strOferta.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.oferta.casoEpecial));
                                strOferta.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.oferta.combo));
                                strOferta.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.oferta.controlDeConsumo));
                                strOferta.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.oferta.kitDeInstalacion));
                                strOferta.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.oferta.mesOperadorCedente));
                                strOferta.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.oferta.modalidadCedente));
                                strOferta.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.oferta.montoCFSEC));
                                strOferta.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.oferta.operadorCedente));
                                strOferta.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.oferta.plazoDeAcuerdo));
                                strOferta.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.oferta.productoComercial));
                                strOferta.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.oferta.proteccionMovil));
                                strOferta.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.oferta.riesgo));
                                strOferta.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.oferta.segmentoDeOferta));
                                strOferta.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.oferta.tipoDeOperacionEmpresa));
                                strOferta.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.oferta.tipoDeProducto));

                                #region [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] - [Obtener los datos de para el registro de las nuevas variables] - [OFERTA]
                                bool strMensajeBrms = (bool)HttpContext.Current.Session["strRespuestaBRMS"];
                                if (strMensajeBrms)
                                {
                                    strOferta.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.oferta.promociones));//18
                                    strOferta.AppendFormat("{0}", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.oferta.productoCuentaAFacturar));//19
                                }
                                else
                                {
                                    strOferta.AppendFormat("{0}|", "");
                                    strOferta.AppendFormat("{0}", "");
                                }
                                #endregion

                                if (oEvaluacion.solicitud.solicitud1.oferta.campana != null)
                                {
                                    //IN_CAMPANA
                                    strCampana.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.oferta.campana.grupo));
                                    strCampana.AppendFormat("{0}", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.oferta.campana.tipo));
                                }

                                if (oEvaluacion.solicitud.solicitud1.oferta.planActual != null)
                                {
                                    //IN_PLAN_ACTUAL
                                    //strPlanActual.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.oferta.planActual.cantidadCuotasPendientes));
                                    //strPlanActual.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.oferta.planActual.cargoFijo));
                                    //strPlanActual.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.oferta.planActual.descripcion));
                                    //strPlanActual.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.oferta.planActual.grupos));
                                    //strPlanActual.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.oferta.planActual.mesesParaCubrirApadece));
                                    //strPlanActual.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.oferta.planActual.modalidadCedenteLineaRenovar));//140579 RESALTADO
                                    //strPlanActual.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.oferta.planActual.montoCuotaActualRenovacion));//140579 RESALTADO
                                    //strPlanActual.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.oferta.planActual.montoPendienteCuotas));
                                    //strPlanActual.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.oferta.planActual.motivoActivacionLineaRenovar));//140579 RESALTADO                                    
                                    //strPlanActual.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.oferta.planActual.plazoDeAcuerdo));
                                    //strPlanActual.AppendFormat("{0}_", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.oferta.planActual.tiempoPermanencia));
                                }

                                if (oEvaluacion.solicitud.solicitud1.oferta.planSolicitado != null)
                                {
                                    //IN_PLAN_SOLICITADO
                                    strPlanSolicitado.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.oferta.planSolicitado.cargoFijo));
                                    strPlanSolicitado.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.oferta.planSolicitado.descripcion));
                                    strPlanSolicitado.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.oferta.planSolicitado.grupos));
                                    strPlanSolicitado.AppendFormat("{0}", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.oferta.planSolicitado.paquete));

                                    if (oEvaluacion.solicitud.solicitud1.oferta.planSolicitado.servicio != null)
                                    {
                                        //IN_PLAN_SERVICIO
                                        foreach (WS.WSReglasCrediticia.servicio oServicio in oEvaluacion.solicitud.solicitud1.oferta.planSolicitado.servicio)
                                        {
                                            strServicio.AppendFormat("{0}|", Funciones.CheckStr(oServicio.nombre));
                                            strServicio.AppendFormat("{0}", Funciones.CheckStr(oServicio.grupo));
                                        }
                                    }
                                }
                            }
                            if (oEvaluacion.solicitud.solicitud1.puntodeVenta != null)
                            {
                                //IN_PUNTO_DE_VENTA
                                strPuntoVenta.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.puntodeVenta.calidadDeVendedor));
                                strPuntoVenta.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.puntodeVenta.canal));
                                strPuntoVenta.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.puntodeVenta.codigo));
                                strPuntoVenta.AppendFormat("{0}", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.puntodeVenta.descripcion));

                                if (oEvaluacion.solicitud.solicitud1.puntodeVenta.direccion != null)
                                {
                                    //IN_PUNTO_DE_VENTA_DIRECCION
                                    strPuntoVentaDireccion.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.puntodeVenta.direccion.codigoDePlano));
                                    strPuntoVentaDireccion.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.puntodeVenta.direccion.departamento));
                                    strPuntoVentaDireccion.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.puntodeVenta.direccion.distrito));
                                    strPuntoVentaDireccion.AppendFormat("{0}|", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.puntodeVenta.direccion.provincia));
                                    strPuntoVentaDireccion.AppendFormat("{0}", Funciones.CheckStr(oEvaluacion.solicitud.solicitud1.puntodeVenta.direccion.region));
                                }
                            }
                        }
                    }
                }

                //OUTPUTS objResponse			
                StringBuilder strCuotaCantResponse = new StringBuilder();
                StringBuilder strCuotaPorcentResponse = new StringBuilder();
                string cantidadDeLineasAdicionalesRUC = "";
                string cantidadDeLineasMaximas = "";
                string cantidadDeLineasRenovaciones = "";
                string montoCFParaRUC = "";
                string tipoDeAutonomiaCargoFijo = "";
                string campaniasConRestriccion = "";
                StringBuilder cantidadCuotasCi = new StringBuilder();
                string cobroAnticipadoInstalacion = "";
                string controlDeConsumo = "";
                string costoDeInstalacion = "";
                string ejecucionDeConsultaBuro = "";
                StringBuilder formaPagoCi = new StringBuilder();
                string cantidadDeAplicacionesRenta = "";
                string frecuenciaDeAplicacionMensual = "";
                string mesInicioRentas = "";
                string montoDeGarantia = "";
                string tipodecobro = "";
                string tipoDeGarantia = "";
                string limiteDeCreditoCobranza = "";
                string limiteDeCreditoDisponible = "";
                string montoTopeAutomatico = "";
                string mostrarMotivoDeRestriccion = "";
                string motivoDeRestriccion = "";
                string cuotaMaxima = "";
                string mostrarRespuesta = "";
                string topeMaximo = "";
                string prioridadPublicar = "";
                string procesoDeExoneracionDeRentas = "";
                string procesoIDValidator = "";
                string procesoValidacionInternaClaro = "";
                string publicar = "";
                string restriccion = "";
                string resultadoEvaluacionCuotas = "";
                string tipoCobroAnticipadoInstalacion = "";
                string capacidadDePago = "";
                string comportamientoConsolidado = "";
                string comportamientoDePagoC1 = "";
                string costoTotalEquipos = "";
                string factorDeEndeudamientoCliente = "";
                string factorDeRenovacionCliente = "";
                string precioDeVentaTotalEquipos = "";
                string riesgoEnClaro = "";
                string riesgoOferta = "";
                string riesgoTotalEquipo = "";
                string riesgoTotalRepLegales = "";

                if (objResponse != null)
                {
                    if (objResponse.ofrecimiento != null)
                    {
                        if (objResponse.ofrecimiento.ofrecimiento1 != null)
                        {
                            if (objResponse.ofrecimiento.ofrecimiento1.autonomia != null)
                            {
                                cantidadDeLineasAdicionalesRUC = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.autonomia.cantidadDeLineasAdicionalesRUC);
                                cantidadDeLineasMaximas = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.autonomia.cantidadDeLineasMaximas);
                                cantidadDeLineasRenovaciones = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.autonomia.cantidadDeLineasRenovaciones);
                                montoCFParaRUC = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.autonomia.montoCFParaRUC);
                                tipoDeAutonomiaCargoFijo = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.autonomia.tipoDeAutonomiaCargoFijo);
                            }
                                                        
                            campaniasConRestriccion = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.campaniasConRestriccion);                            
                            cobroAnticipadoInstalacion = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.cobroAnticipadoInstalacion);//140579 RESALTADO //PROY-140546
                            controlDeConsumo = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.controlDeConsumo);
                            costoDeInstalacion = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.costoDeInstalacion);

                            if (objResponse.ofrecimiento.ofrecimiento1.cuota != null)
                            {
                                for (int row = 0; row < objResponse.ofrecimiento.ofrecimiento1.cuota.Length; row++)
                                {
                                    strCuotaCantResponse.AppendFormat("{0}|", Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.cuota[row].cantidad));
                                }
                                for (int row = 0; row < objResponse.ofrecimiento.ofrecimiento1.cuota.Length; row++)
                                {
                                    strCuotaPorcentResponse.AppendFormat("{0}|", Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.cuota[row].porcentajeInicial));
                                }
                            }
                                                        
                            //ejecucionDeConsultaBuro = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.ejecucionDeConsultaBuro);//140579 RESALTADO
                            ejecucionDeConsultaBuro = "";
                            
                            if (objResponse.ofrecimiento.ofrecimiento1.cantidadCuotasCi != null)
                            {
                                foreach (var objcuota in objResponse.ofrecimiento.ofrecimiento1.cantidadCuotasCi)
                                {
                                    cantidadCuotasCi.AppendFormat("{0}|", Funciones.CheckStr(objcuota));
                                }
                            }

                            if (objResponse.ofrecimiento.ofrecimiento1.formaPagoCi != null)
                            {
                                foreach (var objFormapago in objResponse.ofrecimiento.ofrecimiento1.formaPagoCi)
                                {
                                    formaPagoCi.AppendFormat("{0}|", Funciones.CheckStr(objFormapago));
                                }
                            }
                            cantidadDeAplicacionesRenta = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.garantia.cantidadDeAplicacionesRenta);
                            frecuenciaDeAplicacionMensual = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.garantia.frecuenciaDeAplicacionMensual);
                            mesInicioRentas = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.garantia.mesInicioRentas);

                            montoDeGarantia = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.garantia.montoDeGarantia);
                            tipodecobro = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.garantia.tipodecobro);
                            tipoDeGarantia = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.garantia.tipoDeGarantia);
                            limiteDeCreditoCobranza = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.limiteDeCreditoCobranza);

                            limiteDeCreditoDisponible = "";
                            //limiteDeCreditoDisponible = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.limiteDeCreditoDisponible);

                            montoTopeAutomatico = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.montoTopeAutomatico);
                            mostrarMotivoDeRestriccion = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.mostrarMotivoDeRestriccion);//140579 RESALTADO
                            motivoDeRestriccion = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.motivoDeRestriccion);//140579 RESALTADO                           
                            cuotaMaxima = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.opcionDeCuotas.cuotaMaxima);
                            mostrarRespuesta = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.opcionDeCuotas.mostrarRespuesta);

                            topeMaximo = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.opcionDeCuotas.topeMaximo);
                            prioridadPublicar = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.prioridadPublicar);
                            procesoDeExoneracionDeRentas = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.procesoDeExoneracionDeRentas);
                            procesoIDValidator = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.procesoIDValidator);
                            procesoValidacionInternaClaro = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.procesoValidacionInternaClaro);

                            publicar = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.publicar);
                            restriccion = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.restriccion);
                            resultadoEvaluacionCuotas = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.resultadoEvaluacionCuotas);

                            tipoCobroAnticipadoInstalacion = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.tipoCobroAnticipadoInstalacion);//140579 RESALTADO//PROY-140546
                            
                            if (objResponse.ofrecimiento.ofrecimiento1.resultadosAdicionales != null)
                            {
                                capacidadDePago = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.resultadosAdicionales.capacidadDePago);

                                comportamientoConsolidado = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.resultadosAdicionales.comportamientoConsolidado);
                                comportamientoDePagoC1 = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.resultadosAdicionales.comportamientoDePagoC1);
                                costoTotalEquipos = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.resultadosAdicionales.costoTotalEquipos);
                                factorDeEndeudamientoCliente = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.resultadosAdicionales.factorDeEndeudamientoCliente);
                                factorDeRenovacionCliente = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.resultadosAdicionales.factorDeRenovacionCliente);

                                precioDeVentaTotalEquipos = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.resultadosAdicionales.precioDeVentaTotalEquipos);
                                riesgoEnClaro = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.resultadosAdicionales.riesgoEnClaro);
                                riesgoOferta = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.resultadosAdicionales.riesgoOferta);
                                riesgoTotalEquipo = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.resultadosAdicionales.riesgoTotalEquipo);
                                riesgoTotalRepLegales = Funciones.CheckStr(objResponse.ofrecimiento.ofrecimiento1.resultadosAdicionales.riesgoTotalRepLegales);
                            }
                        }
                    }
                }

                HelperLog.EscribirLog("", nameArchivo, "" + "[ -- FIN GrabarLogHistorialConsultasBRMS -- ]" + " - CONSTRUYENDO REQUEST EXITOSO - 140579 - RESPUESTA: " + rptaMetodo, false);

                /* -- ENVIANDO DATOS AL DATAPOWER [ INICIO ] -- */
                HelperLog.EscribirLog("", nameArchivo, "" + "[ -- INICIO GrabarLogHistorialConsultasBRMS -- ]" + " - ENVIANDO AL DATAPOWER- 140579 -", false);

                RegistrarEvaluacionBRMSDPRequest request = new RegistrarEvaluacionBRMSDPRequest();
                RegistrarEvaluacionBRMSDPResponse response = new RegistrarEvaluacionBRMSDPResponse();
                HeaderRequest objHeaderRequest = new HeaderRequest();

                RestReferences.RestRegistrarEvaluacionBRMS objRegistrarEvaluacionBRMS = new RestReferences.RestRegistrarEvaluacionBRMS();

                //REGION HEADER - INI
                objHeaderRequest.consumer = ConfigurationManager.AppSettings["constRBRMS"].ToString();
                objHeaderRequest.country = ConfigurationManager.AppSettings["BRMS_country"].ToString();
                objHeaderRequest.dispositivo = ConfigurationManager.AppSettings["constRBRMS"].ToString();
                objHeaderRequest.language = ConfigurationManager.AppSettings["BRMS_language"].ToString();
                objHeaderRequest.modulo = ConfigurationManager.AppSettings["BRMS_modulo"].ToString();
                objHeaderRequest.msgType = ConfigurationManager.AppSettings["BRMS_msgType"].ToString();
                objHeaderRequest.operation = ConfigurationManager.AppSettings["constRBRMS"].ToString();
                objHeaderRequest.pid = Convert.ToString(DateTime.Now.Year + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second);
                objHeaderRequest.system = ConfigurationManager.AppSettings["BRMS_codSystem"].ToString();
                objHeaderRequest.timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");
                objHeaderRequest.userId = ConfigurationManager.AppSettings["constRBRMS"].ToString();
                objHeaderRequest.wsIp = ConfigurationManager.AppSettings["BRMSwip"].ToString();
                HelperLog.EscribirLog(" ---[GrabarLogHistorialConsultasBRMS] - ", nameArchivo, String.Format("{0} : {1}", "IP SERVER: ", objHeaderRequest.wsIp), false);
                
                request.MessageRequest.header.HeaderRequest = objHeaderRequest;                
                RegistrarEvaluacionBrmsRequest objRegEvaluacion = new RegistrarEvaluacionBrmsRequest();

                string strCorrelativoBrmsHistorial = (String)HttpContext.Current.Session["objCorrelativoBrmsHistorial"];
                int intCorrelativoBrmsHistorial = 0;
                if ((Funciones.CheckStr(strCorrelativoBrmsHistorial)).Equals(string.Empty))
                {
                    intCorrelativoBrmsHistorial = 1;
                }
                else
                {
                    intCorrelativoBrmsHistorial = Funciones.CheckInt(strCorrelativoBrmsHistorial) + 1;
                }
                HttpContext.Current.Session["objCorrelativoBrmsHistorial"] = intCorrelativoBrmsHistorial.ToString();

                objRegEvaluacion.codigoTipoCliente = codigoTipoClienteBRMS;
                objRegEvaluacion.numeroDocumentoCliente = numeroDocumentoClienteBRMS;                
                objRegEvaluacion.tipoTransaccion = tipoTransaccionBRMS;
                objRegEvaluacion.codigoSolin = intCorrelativoBrmsHistorial.ToString();
                objRegEvaluacion.codigoSlpl = "0";

                objRegEvaluacion.solicitud = strSolicitud.ToString();
                objRegEvaluacion.cliente = strCliente.ToString();
                objRegEvaluacion.direccionCliente = strDirCliente.ToString();
                objRegEvaluacion.documentoCliente = strDocCliente.ToString();
                objRegEvaluacion.clienteRrll = strRepLegal.ToString();
                objRegEvaluacion.equipo = strEquipo.ToString();
                objRegEvaluacion.oferta = strOferta.ToString();
                objRegEvaluacion.campana = strCampana.ToString();
                objRegEvaluacion.planActual = Funciones.CheckStr(strPlanActual);
                objRegEvaluacion.planSolicitada = strPlanSolicitado.ToString();
                objRegEvaluacion.servicio = strServicio.ToString();
                objRegEvaluacion.pdv = strPuntoVenta.ToString();
                objRegEvaluacion.direccionPdv = strPuntoVentaDireccion.ToString();
                objRegEvaluacion.cantidadLineasAdicionales = cantidadDeLineasAdicionalesRUC;
                objRegEvaluacion.cantidadLineasMaxima = cantidadDeLineasMaximas;
                objRegEvaluacion.cantidadLineasRenovacion = cantidadDeLineasRenovaciones;
                objRegEvaluacion.montoCfRuc = montoCFParaRUC;
                objRegEvaluacion.tipoCargoFijo = tipoDeAutonomiaCargoFijo;
                objRegEvaluacion.campaniaRestrinccion = campaniasConRestriccion;
                objRegEvaluacion.cantidadCuotasCi = cantidadCuotasCi.ToString();
                objRegEvaluacion.cobroAnticipadoInstalacion = cobroAnticipadoInstalacion;
                objRegEvaluacion.controlConsumo = controlDeConsumo;
                objRegEvaluacion.costoInstalacion = costoDeInstalacion;
                objRegEvaluacion.cuotaCantidad = strCuotaCantResponse.ToString();
                objRegEvaluacion.cuotaPorcentajeInicial = strCuotaPorcentResponse.ToString();
                objRegEvaluacion.ejecucionConsultaBuro = ejecucionDeConsultaBuro;
                objRegEvaluacion.formaPagoCi = formaPagoCi.ToString();
                objRegEvaluacion.cantidadAplicacionRenta = cantidadDeAplicacionesRenta;
                objRegEvaluacion.frencuenciaApliMensual = frecuenciaDeAplicacionMensual;
                objRegEvaluacion.mesInicioRenta = mesInicioRentas;
                objRegEvaluacion.montoGarantia = montoDeGarantia;
                objRegEvaluacion.tipoCobro = tipodecobro;
                objRegEvaluacion.tipoGarantia = tipoDeGarantia;
                objRegEvaluacion.limiteCreditoCobranza = limiteDeCreditoCobranza;
                objRegEvaluacion.limiteCreditoDisponible = limiteDeCreditoDisponible;
                objRegEvaluacion.montoAutomatico = montoTopeAutomatico;
                objRegEvaluacion.mostrarMotivoRestriccion = mostrarMotivoDeRestriccion;
                objRegEvaluacion.motivoRestriccion = motivoDeRestriccion;
                objRegEvaluacion.opCuotaMaxima = cuotaMaxima;
                objRegEvaluacion.opCuotaMostrarRespuesta = mostrarRespuesta;
                objRegEvaluacion.opCuotaTopeMaximo = topeMaximo;
                objRegEvaluacion.prioridadPublica = prioridadPublicar;
                objRegEvaluacion.procesoExoneraRenta = procesoDeExoneracionDeRentas;
                objRegEvaluacion.procesoIdValitor = procesoIDValidator;
                objRegEvaluacion.validacionInternaClaro = procesoValidacionInternaClaro;
                objRegEvaluacion.publicar = publicar;
                objRegEvaluacion.restriccion = restriccion;
                objRegEvaluacion.resultadoEvaluacionCuota = resultadoEvaluacionCuotas;
                objRegEvaluacion.tipoCobroAnticipadoIns = tipoCobroAnticipadoInstalacion;
                objRegEvaluacion.resCapacidadPago = capacidadDePago;
                objRegEvaluacion.resComportamientoConsolidado = comportamientoConsolidado;
                objRegEvaluacion.resComportamientoPago = comportamientoDePagoC1;
                objRegEvaluacion.resCostoTotalEquipo = costoTotalEquipos;
                objRegEvaluacion.resFactorDeudamientoCli = factorDeEndeudamientoCliente;
                objRegEvaluacion.resFactorRenovacionCli = factorDeRenovacionCliente;
                objRegEvaluacion.resPrecioVentaEquipo = precioDeVentaTotalEquipos;
                objRegEvaluacion.resRiesgoClaro = riesgoEnClaro;
                objRegEvaluacion.resRiesgoOferta = riesgoOferta;
                objRegEvaluacion.resRiesgoTotalEquipo = riesgoTotalEquipo;
                objRegEvaluacion.resRiesgoTotalReplegable = riesgoTotalRepLegales;
                objRegEvaluacion.respuestaWs = mensajeWS;

                request.MessageRequest.body.registrarDatosRequest = objRegEvaluacion;
                response = objRegistrarEvaluacionBRMS.registrarEvaluacionBRMS(request);

                string codRespuesta = response.MessageResponse.body.registrarEvaluacionBRMSResponse.responseStatus.codigoRespuesta;
                string msgRespuesta = response.MessageResponse.body.registrarEvaluacionBRMSResponse.responseStatus.mensajeRespuesta;
                string idBrmsHist = Funciones.CheckStr(response.MessageResponse.body.resultado);

                HelperLog.EscribirLog("", nameArchivo, "" + "[ -- RESPUESTA DATAPOWER INSERTAR HISTORIAL BRMS 140579 -- ]" + " - codRespuesta: -" + codRespuesta, false);
                HelperLog.EscribirLog("", nameArchivo, "" + "[ -- RESPUESTA DATAPOWER INSERTAR HISTORIAL BRMS 140579 -- ]" + " - msgRespuesta: -" + msgRespuesta, false);
                HelperLog.EscribirLog("", nameArchivo, "" + "[ -- RESPUESTA DATAPOWER INSERTAR HISTORIAL BRMS 140579 -- ]" + " - idBrmsHist: -" + idBrmsHist, false);

                if (codRespuesta == "0" && idBrmsHist.ToString() != "")
                {
                    string strCodigosBrmsHistorial = (String)HttpContext.Current.Session["objCodigosBrmsHistorial"];
                    if ((Funciones.CheckStr(strCodigosBrmsHistorial)).Equals(string.Empty))
                    {
                        HttpContext.Current.Session["objCodigosBrmsHistorial"] = idBrmsHist;
                        HelperLog.EscribirLog("", nameArchivo, "" + "[ -- RESPUESTA DATAPOWER INSERTAR HISTORIAL BRMS 140579 -- ]" + " - objCodigosBrmsHistorial: -" + idBrmsHist, false);
                    }
                    else
                    {
                        string strCodigosBrmsHistorial2 = (String)HttpContext.Current.Session["objCodigosBrmsHistorial"];
                        HttpContext.Current.Session["objCodigosBrmsHistorial"] = strCodigosBrmsHistorial2 + "," + idBrmsHist;
                        strCodigosBrmsHistorial2 = (String)HttpContext.Current.Session["objCodigosBrmsHistorial"];
                        HelperLog.EscribirLog("", nameArchivo, "" + "[ -- RESPUESTA DATAPOWER INSERTAR HISTORIAL BRMS 140579 -- ]" + " - strCodigosBrmsHistorial2: -" + strCodigosBrmsHistorial2, false);
                    }
                    rptaMetodo = true;
                }
                else
                {
                    rptaMetodo = false;
                }

                HelperLog.EscribirLog("", nameArchivo, "" + "[ -- FIN GrabarLogHistorialConsultasBRMS -- ]" + " - ENVIANDO AL DATAPOWER- EXITOSO - 140579 - RESPUESTA: " + rptaMetodo, false);
                /* -- ENVIANDO DATOS AL DATAPOWER [ FIN ] -- */

            }
            catch (Exception ex)
            {
                rptaMetodo = false;
                HelperLog.EscribirLog("", nameArchivo, "" + "[ -- ERROR GrabarLogHistorialConsultasBRMS -- ]" + ex.Message.ToString(), false);
            }
            HelperLog.EscribirLog("", nameArchivo, "" + "[ -- FIN GrabarLogHistorialConsultasBRMS -- ] - Respuesta :" + rptaMetodo, false);
            
            return rptaMetodo;
        }

        //ACTUALIZANDO SEC DE LOS REGISTROS INGRESADOS AL HISTORIAL DE BRMS
        public bool ActualizarSecHistorialBRMS(string strSec, string prodFacturar)//PROY-140743
        {
            bool rptaMetodo = true;
            string nameArchivo = "ActualizarSecHistorialBRMS";
            HelperLog.EscribirLog("", nameArchivo, "" + "[ -- INICIO ActualizarSecHistorialBRMS -- ]" + " - CONSTRUYENDO REQUEST - 140579 -", false);           

            try
            {
                string strCodigosHistorialBRMS = (String)HttpContext.Current.Session["objCodigosBrmsHistorial"];
                HelperLog.EscribirLog("", nameArchivo, "" + "[ -- Datos a enviar 140579 ActualizarSecHistorialBRMS -- ] objCodigosBrmsHistorial: " + strCodigosHistorialBRMS, false);
                HelperLog.EscribirLog("", nameArchivo, "" + "[ -- Datos a enviar 140579 ActualizarSecHistorialBRMS -- ] strSec: " + strSec, false);

                if (!(Funciones.CheckStr(strCodigosHistorialBRMS)).Equals(string.Empty) && !(Funciones.CheckStr(strSec)).Equals(string.Empty))
                {
                    ActualizarEvaluacionBrmsDPRequest request = new ActualizarEvaluacionBrmsDPRequest();
                    ActualizarEvaluacionBrmsDPResponse response = new ActualizarEvaluacionBrmsDPResponse();
                    RestReferences.RestActualizarEvaluacionBRMS objActualizarEvaluacionBRMS = new RestReferences.RestActualizarEvaluacionBRMS();
                    HeaderRequest objHeaderRequest = new HeaderRequest();
                    //REGION HEADER - INI
                    objHeaderRequest.consumer = ConfigurationManager.AppSettings["constRBRMS"].ToString();
                    objHeaderRequest.country = ConfigurationManager.AppSettings["BRMS_country"].ToString();
                    objHeaderRequest.dispositivo = ConfigurationManager.AppSettings["constRBRMS"].ToString();
                    objHeaderRequest.language = ConfigurationManager.AppSettings["BRMS_language"].ToString();
                    objHeaderRequest.modulo = ConfigurationManager.AppSettings["BRMS_modulo"].ToString();
                    objHeaderRequest.msgType = ConfigurationManager.AppSettings["BRMS_msgType"].ToString();
                    objHeaderRequest.operation = ConfigurationManager.AppSettings["constRBRMS"].ToString();
                    objHeaderRequest.pid = Convert.ToString(DateTime.Now.Year + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second);
                    objHeaderRequest.system = ConfigurationManager.AppSettings["BRMS_codSystem"].ToString();
                    objHeaderRequest.timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");
                    objHeaderRequest.userId = ConfigurationManager.AppSettings["constRBRMS"].ToString();
                    objHeaderRequest.wsIp = ConfigurationManager.AppSettings["BRMSwip"].ToString();
                    HelperLog.EscribirLog(" ---[ActualizarSecHistorialBRMS] - ", nameArchivo, String.Format("{0} : {1}", "IP SERVER: ", objHeaderRequest.wsIp), false);
                    request.MessageRequest.header.HeaderRequest = objHeaderRequest;

                    ActualizarEvaluacionBrmsRequest objBrmsRquest = new ActualizarEvaluacionBrmsRequest();
                    objBrmsRquest.codigoIOBHN = strCodigosHistorialBRMS;
                    objBrmsRquest.codigoSolin = strSec;

                    request.MessageRequest.body.actualizarEvaluacionRequest = objBrmsRquest;
                    response = objActualizarEvaluacionBRMS.actualizarEvaluacionBRMS(request);
                    string codRespuesta = response.MessageResponse.body.actualizarBRMSResponse.responseStatus.codigoRespuesta;
                    string msgRespuesta = response.MessageResponse.body.actualizarBRMSResponse.responseStatus.mensajeRespuesta;


                    if ((Funciones.CheckStr(codRespuesta)).Equals("0"))
                    {
                        rptaMetodo = true;
                        HelperLog.EscribirLog("", nameArchivo, "" + "[ -- RESPUESTA DATAPOWER ACTUALIZAR HISTORIAL BRMS 140579 -- ]" + msgRespuesta, false);
                    }
                    else
                    {
                        rptaMetodo = false;
                        HelperLog.EscribirLog("", nameArchivo, "" + "[ -- RESPUESTA DATAPOWER ACTUALIZAR HISTORIAL BRMS 140579 -- ] - Codigo Respuesta: " + codRespuesta, false);
                        HelperLog.EscribirLog("", nameArchivo, "" + "[ -- RESPUESTA DATAPOWER ACTUALIZAR HISTORIAL BRMS 140579 -- ] - Mensaje Respuesta: " + msgRespuesta, false);
                    }                    

                    #region [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] [REGISTRO ANULADO]
                    //BWReglasCreditica RegCrediticia = new BWReglasCreditica();
                    //ClaroEvalClientesReglasDevRequest objRequest = null;
                    //bool rptaRegVarBRMSVtaCVuotas = RegCrediticia.RegistrarVariablesBRMSVtaCuotas(objRequest, strSec, Funciones.CheckStr(prodFacturar)); // [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil]
                    //HttpContext.Current.Session["objCodigosBrmsHistorialVV"] = null;
                    #endregion

                }
                else
                {
                    HelperLog.EscribirLog("", nameArchivo, "" + "[ -- No se logro ingresar al metodo ActualizarSecHistorialBRMS 140579. -- ]", false);
                    rptaMetodo = false;
                }                
            }
            catch (Exception ex)
            {
                rptaMetodo = false;
                HelperLog.EscribirLog("", nameArchivo, "" + "[ -- ERROR ActualizarSecHistorialBRMS -- ]" + ex.Message.ToString(), false);
            }

            return rptaMetodo;
        }
    }
}
//FIN PROY-140579 NN
