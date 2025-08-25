using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity;
using Claro.SISACT.Data;
using System.Data;
using System.Configuration;
//PROY-32439 MAS INI 
using System.Web;
using Claro.SISACT.Common;
//PROY-32439 MAS FIN
//INICIO PROY-140546 FASE 2
using Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest.DataPower.RegistroPA;
using Claro.SISACT.Business.RestReferences;
using Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest;
using PAGOANTICIPADO_ENTITY = Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest;
using Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest.DataPower.ActualizaPA;
using Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest.DataPower.RegistraHistorial;
using System.Net.Mail;
using System.Text.Json;


namespace Claro.SISACT.Business
{
    public class BLConsumer
    {
        string constTipoProductoDTH = ConfigurationManager.AppSettings["constTipoProductoDTH"].ToString();
        string constTipoProducto3Play = ConfigurationManager.AppSettings["consTipoProducto3Play"].ToString();
        string constTipoProductoVentaVarios = ConfigurationManager.AppSettings["constTipoProductoVentaVarios"].ToString();
        string constTipoProductoFTTH = ConfigurationManager.AppSettings["constTipoProductoFTTH"]; //FTTH
        string constTipoProducto3PlayInalam = ConfigurationManager.AppSettings["constTipoProducto3PlayInalam"].ToString();
        public BLConsumer() { }

        public bool ConsultaValidacionCliente(string tipoDocumento, string nroDocumento, string nrotelefono, out string flagValida, out string msgText)
        {
            var obj = new DAConsumer();
            return obj.ConsultaValidacionCliente(tipoDocumento, nroDocumento, nrotelefono, out flagValida, out msgText);
        }
		
        public Int64 validaSolPendientePagoMig(string tipoDocumento, string nroDocumento, string nroTelefono, out bool flgRpta, out string msgRpta)
        {
            Int64 nroSEC = 0;
            bool rptaValidacion = true;
            flgRpta = true;
            msgRpta = "OK";

            BEItemMensaje objMensaje = new BEItemMensaje();


            if (!string.IsNullOrEmpty(nroTelefono))
            {
                nroSEC = new DAConsumer().validaSolPendientePagoMig(tipoDocumento, nroDocumento, nroTelefono);
                if (nroSEC > 0) rptaValidacion = false;
            }

            if (!rptaValidacion)
            {
                flgRpta = false;
                msgRpta = "ERROR";
            }

            return nroSEC;
        }

        public Int64 validarSecPendMig(string tipoDocumento, string nroDocumento, string listaTelefono)
        {
            Int64 nroSEC = 0;
            try
            {
                string[] arrTelefono_ingresado = listaTelefono.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < arrTelefono_ingresado.Count(); j++)
                {
                    nroSEC = new DAConsumer().validarSecPendMig(tipoDocumento, nroDocumento, arrTelefono_ingresado[j].ToString());
                }
            }
            catch (Exception ex)
            {
                nroSEC = 0;
                throw ex;
            }
            return nroSEC;
        }
		
        public DataSet ListarDetalleLineaBSCS(int tipoDocumento, string nroDocumento)
        {
            return new DAConsumer().ListarDetalleLineaBSCS(tipoDocumento, nroDocumento);
        }
        public DataSet ListarDetalleLineaSGA(string tipoDocumento, string nroDocumento, int intCantMes)
        {
            return new DAConsumer().ListarDetalleLineaSGA(tipoDocumento, nroDocumento, intCantMes);
        }
        public DataSet ListarDetalleLineaSISACT(string tipoDocumento, string nroDocumento, string strTelefonos)
        {
            return new DAConsumer().ListarDetalleLineaSISACT(tipoDocumento, nroDocumento, strTelefonos);
        }
        public DataTable ListarCantPlanxBilleteraBSCS(int tipoDocumento, string nroDocumento)
        {
            return new DAConsumer().ListarCantPlanxBilleteraBSCS(tipoDocumento, nroDocumento);
        }
        public List<BEPlanBilletera> ListarDetallePlanesCorporativo(int tipoDocumento, string nroDocumento)
        {
            return new DAConsumer().ListarDetallePlanesCorporativo(tipoDocumento, nroDocumento);
        }
        public DataTable ListarDetalleLineaFraude(string strFlagBuscar, int tipoDocumento, string nroDocumento)
        {
            return new DAConsumer().ListarDetalleLineaFraude(strFlagBuscar, tipoDocumento, nroDocumento);
        }
        public DataTable ListarDetalleLineaBloqueo(string strFlagBuscar, int tipoDocumento, string nroDocumento)
        {
            return new DAConsumer().ListarDetalleLineaBloqueo(strFlagBuscar, tipoDocumento, nroDocumento);
        }
        public List<BEPlan> ListarDetallePlanCF(string dni, int meses, int condicion)
        {
            return new DAConsumer().ListarDetallePlanCF(dni, meses, condicion);
        }
        public DataTable ListarCantidadLineasActivas(Int64 nroSEC)
        {
            return new DAConsumer().ListarCantidadLineasActivas(nroSEC);
        }

        public int ObtenerComportamientoPago(int tipoDocumento, string strNroDoc, ref BEItemMensaje objMensaje)
        {
            return new DAConsumer().ObtenerComportamientoPago(tipoDocumento, strNroDoc, ref objMensaje);
        }
        public Int64 ObtenerSOTxMigracion(string tipoDocumento, string nroDocumento)
        {
            return new DAConsumer().ObtenerSOTxMigracion(tipoDocumento, nroDocumento);
        }
        public List<BEPuntoVenta> ListarBlackListPdv()
        {
            return new DAConsumer().ListarBlackListPdv();
        }
        public int ValidarBlackListPdv(string strCodCanal, string strCodPdv)
        {
            return new DAConsumer().ValidarBlackListPdv(strCodCanal, strCodPdv);
        }
        public bool ValidarVendedor(string dniVendedor, string strOficina, ref string strMensaje, ref string strIdVendedor)
        {
            return new DAConsumer().ValidarVendedor(dniVendedor, strOficina, ref strMensaje, ref strIdVendedor);
        }
        public void ConsultaTopBlackWhiteList(string tipoDocumento, string strNroDoc, int intDiasDeuda, double dblDeuda, int intLineasActivas,
                                                int intLineasBloqueo, ref bool blnBlack, ref bool blnWhite, ref bool blnTop)
        {
            new DAConsumer().ConsultaTopBlackWhiteList(tipoDocumento, strNroDoc, intDiasDeuda, dblDeuda, intLineasActivas, intLineasBloqueo, ref blnBlack, ref blnWhite, ref blnTop);
        }
        public string ConsultaBlackListComisiones(string tipoDocumento, string strNroDoc)
        {
            return new DAConsumer().ConsultaBlackListComisiones(tipoDocumento, strNroDoc);
        }

        public DataTable ObtenerInformacionCrediticia(Int64 nroSEC)
        {
            return new DAConsumer().ObtenerInformacionCrediticia(nroSEC);
        }
        public DataTable ObtenerInformacionBilletera(Int64 nroSEC)
        {
            return new DAConsumer().ObtenerInformacionBilletera(nroSEC);
        }
        public DataTable ObtenerInformacionGarantia(Int64 nroSEC)
        {
            return new DAConsumer().ObtenerInformacionGarantia(nroSEC);
        }
        public DataTable ObtenerInformacionGarantiaII(Int64 nroSEC)
        {
            return new DAConsumer().ObtenerInformacionGarantiaII(nroSEC);
        }
        public void ObtenerPlanesSolicitud(Int64 nroSEC, ref List<BEPlanDetalleVenta> listaPlanes, ref List<BESecServicio_AP> listaServicios)
        {
            new DAConsumer().ObtenerPlanesSolicitud(nroSEC, ref listaPlanes, ref listaServicios);
        }
        public void ObtenerCostoAlquilerInstalKIT(string idKit, string idCampana, string idPlazo, ref double pAlquiler, ref double pInstalacion)
        {
            new DAConsumer().ObtenerCostoAlquilerInstalKIT(idKit, idCampana, idPlazo, ref pAlquiler, ref pInstalacion);
        }

        //PROY-24740
        public bool GrabarPlanDetalle(ArrayList listaPlanDetalle, Int64 nroSEC, BEVistaEvaluacion objEvaluacion, string idOficina, string hdbuyback, RegistroPARequest objRegistroPA)//PROY-140736 fin //PROY-140546
        {
            //INICIO PROY-140546
            GeneradorLog objLog = new GeneradorLog("GrabarPlanDetalle", null, null, null);
            objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-140546][GrabarPlanDetalle INICIO", ""), null);
            //FIN PROY-140546

            objLog.CrearArchivolog("[INC000004091065][INC000003467242]", string.Format("{0}:{1}", "[BLConsumer][GrabarPlanDetalle] objEvaluacion:", Funciones.CheckStr(JsonSerializer.Serialize(objEvaluacion))), null);

            Int64 idSol = 0;
            bool salida = false;
            int contador = 0; //Contador que permitir� insertar secuencialmente desde el valor 1 los planes en el campo SOPLN_ORDEN
            int ant_sopln_orden; //Guardamos el valor del sopln_orden original para compararlo con tras variables ya almacenadas;

            foreach (BEPlanDetalleVenta objPlanDetalle in listaPlanDetalle)
            {
                objLog.CrearArchivolog("[INC000004091065][INC000003467242]", string.Format("{0}:{1}", "[BLConsumer][GrabarPlanDetalle] objPlanDetalle:", Funciones.CheckStr(JsonSerializer.Serialize(objPlanDetalle))), null);

                objPlanDetalle.SOLIN_CODIGO = nroSEC;

                contador++;
                ant_sopln_orden = objPlanDetalle.SOPLN_ORDEN;
                objPlanDetalle.SOPLN_ORDEN = contador;


                if (objPlanDetalle.PRDC_CODIGO == constTipoProducto3Play || objPlanDetalle.PRDC_CODIGO == constTipoProducto3PlayInalam || objPlanDetalle.PRDC_CODIGO == constTipoProductoFTTH) //FTTH
                {
                    objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-140546][GrabarPlanDetalle 3 PLAY]", ""), null); //PROY-140546

                    objLog.CrearArchivolog("[INC000004091065][INC000003467242]", string.Format("{0}:{1}", "[BLConsumer][GrabarPlanDetalle] condicion:", "objPlanDetalle.PRDC_CODIGO == constTipoProducto3Play || objPlanDetalle.PRDC_CODIGO == constTipoProducto3PlayInalam || objPlanDetalle.PRDC_CODIGO == constTipoProductoFTTH"), null);

                    GrabarPlanDetalleHFC(objPlanDetalle, ref idSol);
                                
                    //INICIO PROY-140546
                    double nMontoAnticipadoIns = objEvaluacion.montoAnticipadoInstalacion;
                    objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-140546][nMontoAnticipadoIns]: ", nMontoAnticipadoIns), null);
                    objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-140546][Key_CanalesPermitidosCAI]: ", ReadKeySettings.Key_CanalesPermitidosCAI), null);
                    objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-140546][tipoOficina]: ", objRegistroPA.tipoOficina), null);

                    if (Funciones.EsValorPermitido(objRegistroPA.tipoOficina, ReadKeySettings.Key_CanalesPermitidosCAI, ","))
                    {
                        double nmai = 0;
                        GrabarPagoAnticipado(objEvaluacion, nroSEC, idSol, objRegistroPA.tipoProducto,ref nmai);
                                                
                        var nroSecPadre = Funciones.CheckInt64(HttpContext.Current.Session["nroSecPadre"]);
                        ValidarPagoAnterior(nroSecPadre, nroSEC, objRegistroPA);
                        RegistraHistorialPagoAnticipado(objPlanDetalle, idSol, objRegistroPA, nmai);
                    }
                    //FIN PROY-140546
                }
                else if (objPlanDetalle.PRDC_CODIGO == constTipoProductoDTH)
                {
                    objLog.CrearArchivolog("[INC000004091065][INC000003467242]", string.Format("{0}:{1}", "[BLConsumer][GrabarPlanDetalle] condicion:", "objPlanDetalle.PRDC_CODIGO == constTipoProductoDTH"), null);
                    GrabarPlanDetalleDTH(objPlanDetalle, ref idSol);
                }
                else if (objPlanDetalle.PRDC_CODIGO == constTipoProductoVentaVarios)
                {
                    objLog.CrearArchivolog("[INC000004091065][INC000003467242]", string.Format("{0}:{1}", "[BLConsumer][GrabarPlanDetalle] condicion:", "objPlanDetalle.PRDC_CODIGO == constTipoProductoVentaVarios"), null);
                    GrabarDetalleVentaVarios(objPlanDetalle, ref idSol);
                }
                else
                {
                    objLog.CrearArchivolog("[INC000004091065][INC000003467242]", string.Format("{0}:{1}", "[BLConsumer][GrabarPlanDetalle] condicion:", "else"), null);

                    GrabarPlanDetalleMovil(objPlanDetalle, idOficina, ref idSol);

                    //PROY-140736 fin
                    //Aqui grabar buyback
                    if (!string.IsNullOrEmpty(hdbuyback))
                    {


                        int codigoRespuesta_ = 0;
                        var Buyback = hdbuyback.Split('|');
                        
                        var aplicacion = ConfigurationManager.AppSettings["constAplicacion"].ToString();

                        for (int i = 0; i < Buyback.Length; i++)
                        {
                            var arrFila = Buyback[i].Split(';');
                            if (arrFila[0] == ant_sopln_orden.ToString())
                            {
                                var cupon = Funciones.CheckStr(arrFila[1]);
                                var imei = Funciones.CheckStr(arrFila[2]);
                                var codmaterial = Funciones.CheckStr(arrFila[3]);
                                var GuardarBuyback = BLEvaluacion.RegistrarBuyBack(Convert.ToInt32(idSol), cupon, imei, codmaterial, aplicacion, objPlanDetalle.USUARIO, objPlanDetalle.strServidor, ref codigoRespuesta_);
                                break;
                            }
                            
                        }
                    }
                    //PROY-140736 fin
                }

                if (objPlanDetalle.PRDC_CODIGO != constTipoProductoVentaVarios)
                {
                    objLog.CrearArchivolog("[INC000004091065][INC000003467242]", string.Format("{0}:{1}", "[BLConsumer][GrabarPlanDetalle] condicion donde esta el registro:", "objPlanDetalle.PRDC_CODIGO != constTipoProductoVentaVarios"), null);

                    List<BEOfrecimiento> objOfrecimiento = (List<BEOfrecimiento>)objEvaluacion.oOfrecimiento;
                    List<BEGarantia> objGarantia = (List<BEGarantia>)objEvaluacion.oGarantia;

                    // GRABAR DATOS BRMS
                    foreach (BEOfrecimiento obj in objOfrecimiento.Where(w => w.IdFila == ant_sopln_orden))
                        {
                           //PROY-29215 INICIO
                            obj.FormaPago = objPlanDetalle.FORMA_PAGO;
                            obj.NroCuota = Convert.ToInt32(objPlanDetalle.CUOTA_PAGO);
                           //PROY-29215 FIN

                            objLog.CrearArchivolog("[INC000004091065][INC000003467242]", string.Format("{0}:{1}", "[BLConsumer][GrabarPlanDetalle] param in nroSEC", Funciones.CheckStr(nroSEC.ToString())), null);
                            objLog.CrearArchivolog("[INC000004091065][INC000003467242]", string.Format("{0}:{1}", "[BLConsumer][GrabarPlanDetalle] param in idSol", Funciones.CheckStr(idSol)), null);
                            objLog.CrearArchivolog("[INC000004091065][INC000003467242]", string.Format("{0}:{1}", "[BLConsumer][GrabarPlanDetalle] param in obj", Funciones.CheckStr(JsonSerializer.Serialize(obj))), null);

                            new BLEvaluacion().InsertarDatosBRMS(nroSEC, idSol, obj);
                        //PROY-32439 MAS INI Grabar tabla nuevo BRMS
                        string strTipoDoc = obj.In_doc_cliente.Split('|')[0];
                        ValidacionDeudaBRMS objVariablesEntradaNVoBRMS = new ValidacionDeudaBRMS();
                        objVariablesEntradaNVoBRMS = (ValidacionDeudaBRMS)HttpContext.Current.Session["ObjNvoBRMS"];
                        ObtenerParametrosNvoBRMS(objVariablesEntradaNVoBRMS, nroSEC);

                        objLog.CrearArchivolog("[INC000004091065][INC000003467242]", string.Format("{0}:{1}", "[BLConsumer][GrabarPlanDetalle] strTipoDoc", Funciones.CheckStr(strTipoDoc)), null);

                        if (strTipoDoc.IndexOf("RUC") > -1)
                        {
                            foreach (var objRRLL in objVariablesEntradaNVoBRMS.clientes)
                            {
                                ObtenerParametrosNvoBRMS(objRRLL, nroSEC);
                            }
                        }
                        //PROY-32439 MAS FIN Grabar tabla nuevo BRMS
                        }

                    // GRABAR DATOS COMBO
                    if (!string.IsNullOrEmpty(objPlanDetalle.IDCOMBO))
                    {
                        string idPlan = objPlanDetalle.PLANC_CODIGO;
                        string plan = objPlanDetalle.PLANV_DESCRIPCION;
                        if ((objPlanDetalle.PRDC_CODIGO == constTipoProducto3Play) || (objPlanDetalle.PRDC_CODIGO == constTipoProducto3PlayInalam) || objPlanDetalle.PRDC_CODIGO == constTipoProductoFTTH) //FTTH
                        {
                            idPlan = objPlanDetalle.PLAN_SOL_HFC.IdPlan;
                            plan = objPlanDetalle.PLAN_SOL_HFC.Plan;
                        }
                        new BLEvaluacion().InsertarDatosDescuento(nroSEC, idSol, objPlanDetalle.PRDC_CODIGO, idPlan, plan, objPlanDetalle.USUARIO, objPlanDetalle.IDCOMBO);
                    }

                    // GRABAR DATOS GARANTIA
                    foreach (BEGarantia obj in objGarantia.Where(w => w.IdFila == ant_sopln_orden))
                        {
                            obj.idPlan = objPlanDetalle.PLANC_CODIGO;
                            obj.plan = objPlanDetalle.PLANV_DESCRIPCION;
                            if ((objPlanDetalle.PRDC_CODIGO == constTipoProducto3Play) || (objPlanDetalle.PRDC_CODIGO == constTipoProducto3PlayInalam) || objPlanDetalle.PRDC_CODIGO == constTipoProductoFTTH) //FTTH
                            {
                                obj.idPlan = objPlanDetalle.PLAN_SOL_HFC.IdPlan;
                                obj.plan = objPlanDetalle.PLAN_SOL_HFC.Plan;
                            }

                            new DAEvaluacion().InsertarDatosGarantia(nroSEC, idSol, obj);
                        }
                    }
                }
            return salida;
        }

        //INICIO PROY-140546 - FASE 2
        private void RegistraHistorialPagoAnticipado(BEPlanDetalleVenta objDetallePlan, long pIdSol, RegistroPARequest objRegistroPA, double nmai)
        {
            GeneradorLog objLog = new GeneradorLog("RegistraHistorialPagoAnticipado", null, null, null);
            objLog.CrearArchivolog(null, "RegistraHistorialPagoAnticipado", null);
            BLPagoAnticipado oBLPagoAnticipado = new BLPagoAnticipado();

            RegistraHistorialRequest oRequest = new RegistraHistorialRequest();
            oRequest.numeroSolicitud = objDetallePlan.SOLIN_CODIGO;
            oRequest.numeroSolicitudPlan = pIdSol;
            oRequest.tipoProducto = Funciones.CheckStr(objDetallePlan.PRDC_CODIGO);            
            oRequest.codigoPlan = Funciones.CheckStr(objDetallePlan.PLAN_SOL_HFC.IdPlan);
            oRequest.descripcionPlan = Funciones.CheckStr(objDetallePlan.PLAN_SOL_HFC.Plan);
            oRequest.fechaRegistro = DateTime.Today.ToString("yyyy-MM-dd");
            oRequest.costoInstalacion = Funciones.CheckDbl(objDetallePlan.SOLIN_COSTO_INST_DTH);
            oRequest.costoInstalacionManual = Funciones.CheckDbl(objDetallePlan.SOLIN_COSTO_INST_DTH);
            oRequest.formaPago = Funciones.CheckStr(objDetallePlan.FORMA_PAGO);
            oRequest.formaPagoManual = Funciones.CheckStr(objDetallePlan.FORMA_PAGO);
            oRequest.numeroCuotas = Funciones.CheckInt(objDetallePlan.CUOTA_PAGO);
            oRequest.numeroCuotasManual = Funciones.CheckInt(objDetallePlan.CUOTA_PAGO);
            oRequest.montoAnticipadoInstalacion = objRegistroPA.montoInicialInstalacion;
            oRequest.montoAnticipadoInstalacionManual = nmai;
            oRequest.usuarioActualizacion = Funciones.CheckStr(objDetallePlan.USUARIO);
            oRequest.grupoSolicitud = 0;
            oRequest.puntoVenta = objRegistroPA.ovencCodigo;

            oBLPagoAnticipado.RegistraHistorialPagoAnticipado(oRequest);
        }

        private void ValidarPagoAnterior(Int64 pNroSecPadre, Int64 pNroSecActual, RegistroPARequest objRegistroPA)
        {
            GeneradorLog objLog = new GeneradorLog("ValidarPagoAnterior", null, null, null);
            double nMaiAnterior = 0;
            string estadoAnterior = string.Empty; //FALLA PAGO
            double nMaiActual = objRegistroPA.montoInicialInstalacion;

            objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-140546][ValidarPagoAnterior Ini", ""), null);

            List<PagoAnticipado> objListPagosAnticipados = ListarPagosAnticipados(pNroSecPadre);
            
            foreach (var item in objListPagosAnticipados)
            {
                nMaiAnterior = Funciones.CheckDbl(item.montoInicialInstalacion);//falla pago
                estadoAnterior = Funciones.CheckStr(item.estado);//falla pago
            }

            objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-140546][pNroSecPadre: ", pNroSecPadre), null);
            objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-140546][pNroSecActual: ", pNroSecActual), null);
            objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-140546][nMaiActual: ", nMaiActual), null);
            objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-140546][nMaiAnterior: ", nMaiAnterior), null);
            objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-140546][estadoAnterior: ", estadoAnterior), null);

            if (pNroSecPadre != 0 && estadoAnterior == "1")
            {
                if (nMaiActual > 0)
                {
                    if (pNroSecPadre != 0 || pNroSecActual != 0)
                    {
                        objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-140546][ActualizarPagoAnticipado: Ini", ""), null);
                        if (ActualizarPagoAnticipado(pNroSecPadre, "6", 0))//FALLA PAGO
                        {
                            objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-140546][pNroSecPadre Estado: ", ReadKeySettings.ConsCadEstadoAnulado), null);
                        }
                        if (ActualizarPagoAnticipado(pNroSecActual, "7", nMaiActual)) //FALLA PAGO
                        {
                            objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-140546][pNroSecActual Estado: ", ReadKeySettings.ConsCadEstadoPagado), null);
                        }
                        objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-140546][ActualizarPagoAnticipado: Fin", ""), null);
                    }
                }
            }
            else
            {
                objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-140546][nMaiAnterior: ", nMaiAnterior), null);
                objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-140546][nMaiActual: ", nMaiActual), null);
            }
            objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-140546][ValidarPagoAnterior Fin", ""), null);
        }

        public List<PagoAnticipado> ListarPagosAnticipados(Int64 pNroSec)
        {
            string CurrentUsers = System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName())[0].ToString();

            RestConsultarPagoAnticipadoFija oServiceRequest = new RestConsultarPagoAnticipadoFija();
            BEAuditoriaRequest oAuditoriaRequest = new BEAuditoriaRequest();
            oAuditoriaRequest.userId = Funciones.CheckStr(ConfigurationManager.AppSettings["system_ConsultaClave"]);
            oAuditoriaRequest.ipApplication = CurrentUsers;

            PAGOANTICIPADO_ENTITY.DataPower.ConsultaPAGenericRequest oGlobalRequest = new PAGOANTICIPADO_ENTITY.DataPower.ConsultaPAGenericRequest();
            oGlobalRequest.MessageRequest = new PAGOANTICIPADO_ENTITY.DataPower.ConsultaPAMessageRequest();
            PAGOANTICIPADO_ENTITY.DataPower.ConsultaPAHeaderRequest oHeaderRequest = new PAGOANTICIPADO_ENTITY.DataPower.ConsultaPAHeaderRequest();
            oHeaderRequest.consumer = ReadKeySettings.ConsConsumerConsultaPA;
            oHeaderRequest.country = ReadKeySettings.ConsCountryConsultaPA;
            oHeaderRequest.dispositivo = ReadKeySettings.ConsDispositivoConsultaPA;
            oHeaderRequest.language = ReadKeySettings.ConsLanguageConsultaPA;
            oHeaderRequest.modulo = ReadKeySettings.ConsModuloConsultaPA;
            oHeaderRequest.msgType = ReadKeySettings.ConsMsgTypeConsultaPA;
            oHeaderRequest.operation = ReadKeySettings.ConsOperationConsultaPA;
            oHeaderRequest.pid = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            oHeaderRequest.system = ReadKeySettings.ConsSystemConsultaPA;
            oHeaderRequest.timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            oHeaderRequest.userId = CurrentUsers;
            oHeaderRequest.wsIp = ReadKeySettings.ConsCurrentIP;
            PAGOANTICIPADO_ENTITY.DataPower.ConsultaPAHeader oHeader = new PAGOANTICIPADO_ENTITY.DataPower.ConsultaPAHeader();
            oHeader.HeaderRequest = oHeaderRequest;

            PAGOANTICIPADO_ENTITY.DataPower.ConsultaPAMessageRequest oMessageRequest = new PAGOANTICIPADO_ENTITY.DataPower.ConsultaPAMessageRequest();
            PAGOANTICIPADO_ENTITY.DataPower.ConsultaPARequest oConsultaPARequest = new PAGOANTICIPADO_ENTITY.DataPower.ConsultaPARequest();
            PAGOANTICIPADO_ENTITY.DataPower.ConsultaPABody oBodyRequest = new PAGOANTICIPADO_ENTITY.DataPower.ConsultaPABody();

            oConsultaPARequest.numeroDocumento = "";
            oConsultaPARequest.numeroSolicitud = Funciones.CheckInt64(pNroSec);
            oConsultaPARequest.estado = "";
            oConsultaPARequest.tipoConsulta = "1"; //Por numero de Solicitud
            oBodyRequest.consultaPARequest = oConsultaPARequest;
            oMessageRequest.Header = oHeader;
            oMessageRequest.Body = oBodyRequest;
            oGlobalRequest.MessageRequest = oMessageRequest;

            List<PAGOANTICIPADO_ENTITY.PagoAnticipado> oListaRespuesta = oServiceRequest.ConsultarPagosAnticipados(oGlobalRequest);

            return oListaRespuesta;
        }

        private bool ActualizarPagoAnticipado(Int64 pNroSec, string pEstado, double maiActual)
        {
            BLPagoAnticipado oBLPagoAnticipado = new BLPagoAnticipado();
            BEUsuarioSession objUsuario = (BEUsuarioSession)HttpContext.Current.Session["Usuario"];
            ActualizaPARequestType oRequest = new ActualizaPARequestType();

            oRequest.estado = pEstado;
            oRequest.numeroSolicitud = Funciones.CheckInt64(pNroSec);
            oRequest.usuarioActualizacion = objUsuario.idCuentaRed;
            oRequest.montoInicialModificado = maiActual.ToString();//falla pago
            return oBLPagoAnticipado.ActualizaPagoAnticipado(oRequest);
        }

        private void GrabarPagoAnticipado(BEVistaEvaluacion objEvaluacion, long pNroSec, long pIdSol, string pTipoProducto, ref double nMaiModi)
        {
            BLPagoAnticipado oBLPagoAnticipado = new BLPagoAnticipado();
            GeneradorLog objLog = new GeneradorLog("GrabarPagoAnticipado", null, null, null);
            objLog.CrearArchivolog(null, "Inicio GrabarPagoAnticipado", null);
            string tipoBenefElegidoFC = Funciones.CheckStr((string)HttpContext.Current.Session["TipoBenefElegidoFC"]);
            objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-140546][tipoBenefElegidoFC: ", tipoBenefElegidoFC), null);
            objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-140546][txtFechaAgendamiento1: ", Funciones.CheckStr(HttpContext.Current.Request["txtFechaAgendamiento1"])), null);
            objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-140546][txtFechaAgendamiento2: ", Funciones.CheckStr(HttpContext.Current.Request["txtFechaAgendamiento2"])), null);
            objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-140546][txtFechaAgendamiento3: ", Funciones.CheckStr(HttpContext.Current.Request["txtFechaAgendamiento3"])), null);
            objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-140546][ddlFranja1: ", Funciones.CheckStr(HttpContext.Current.Request["ddlFranja1"])), null);
            objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-140546][ddlFranja2: ", Funciones.CheckStr(HttpContext.Current.Request["ddlFranja2"])), null);
            objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-140546][ddlFranja3: ", Funciones.CheckStr(HttpContext.Current.Request["ddlFranja3"])), null);
            objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-140546][hidNombre: ", Funciones.CheckStr(HttpContext.Current.Request["hidNombre"])), null);
            objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-140546][hidApePaterno: ", Funciones.CheckStr(HttpContext.Current.Request["hidApePaterno"])), null);
            objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-140546][hidApeMaterno: ", Funciones.CheckStr(HttpContext.Current.Request["hidApeMaterno"])), null);
            objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-140546][txtCasillaCorreoiClaro: ", Funciones.CheckStr(HttpContext.Current.Request["txtCasillaCorreoiClaro"])), null);
            objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-140546][chkPublicar: ", Funciones.CheckStr(HttpContext.Current.Request["chkPublicar"])), null);
            objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-140546][ddlMedioPago: ", Funciones.CheckStr(HttpContext.Current.Request["ddlMedioPago"])), null);

            BEUsuarioSession objUsuario = (BEUsuarioSession)HttpContext.Current.Session["Usuario"];
            RegistroPARequest oRequest = new RegistroPARequest();
            oRequest.numeroSolicitud = pNroSec;
            oRequest.numeroSolicitudPlan = pIdSol;
            oRequest.tipoProducto = pTipoProducto;
            oRequest.montoTotalInstalacion = objEvaluacion.oOfrecimiento[0].CostoDeInstalacion;

            double montoMai= Funciones.CheckDbl(HttpContext.Current.Request["hidMAI"]);
            oRequest.montoInicialInstalacion = montoMai;

            if (!string.IsNullOrEmpty(tipoBenefElegidoFC))
            {
                Int64 ConsParamCodCobroAnticipadoInst = Funciones.CheckInt64(ConfigurationManager.AppSettings["codigoParamCobroAnticipadoInstalacion"]);
                List<BEParametro> ListParamCodCobroAnticipado = new BLGeneral().ListaParametrosGrupo(ConsParamCodCobroAnticipadoInst);
                double MontoDescuentoPorFullClaroCAI = 0;

                if (ListParamCodCobroAnticipado != null && ListParamCodCobroAnticipado.Count > 0)
                {
                    MontoDescuentoPorFullClaroCAI = ListParamCodCobroAnticipado
                         .Where(w => w.Valor1 == "Key_MontoDescuentoPorFullClaroCAI").ToList().Count > 0 ?
                         Funciones.CheckDbl(ListParamCodCobroAnticipado.Where(w => w.Valor1 == "Key_MontoDescuentoPorFullClaroCAI").ToList()[0].Valor) : 0;
                }

                oRequest.montoInicialModificado = (montoMai <= MontoDescuentoPorFullClaroCAI) ? montoMai : MontoDescuentoPorFullClaroCAI;
            }
            else {
                oRequest.montoInicialModificado = montoMai;
            }

            nMaiModi = oRequest.montoInicialModificado;
            oRequest.porcentajeMontoInicial = objEvaluacion.montoAnticipadoInstalacion;
            oRequest.tipoInicial = objEvaluacion.tipoCobroAnticipadoInstalacion;

            oRequest.fechaProgramacion1 = Funciones.CheckDate(HttpContext.Current.Request["txtFechaAgendamiento1"]).ToString("yyyy-MM-dd");
            oRequest.franjaHoraria1 = HttpContext.Current.Request["ddlFranja1"];
            oRequest.fechaProgramacion2 = Funciones.CheckDate(HttpContext.Current.Request["txtFechaAgendamiento2"]).ToString("yyyy-MM-dd");
            oRequest.franjaHoraria2 = HttpContext.Current.Request["ddlFranja2"];
            oRequest.fechaProgramacion3 = Funciones.CheckDate(HttpContext.Current.Request["txtFechaAgendamiento3"]).ToString("yyyy-MM-dd");
            oRequest.franjaHoraria3 = HttpContext.Current.Request["ddlFranja3"];

            string sNombreCliente = HttpContext.Current.Request["hidNombre"];
            string sApellidoPaterno = HttpContext.Current.Request["hidApePaterno"];
            string sApellidoMaterno = HttpContext.Current.Request["hidApeMaterno"];
            oRequest.numeroDocumento = HttpContext.Current.Request["hidNroDocumento"];// sNumeroDoc;
            oRequest.nombreCliente = sNombreCliente + " " + sApellidoPaterno + " " + sApellidoMaterno;
            oRequest.correoiClaro = HttpContext.Current.Request["txtCasillaCorreoiClaro"];
            oRequest.flagPublicar = Funciones.CheckStr(HttpContext.Current.Request["chkPublicar"]) == "on" ? "1" : "0";
            oRequest.usuarioRegistro = objUsuario.idCuentaRed;
            oRequest.medioPago = (oRequest.montoInicialInstalacion) > 0 ? Funciones.CheckStr(HttpContext.Current.Request["ddlMedioPago"]) : "PAGO 0";
            oBLPagoAnticipado.RegistroPagoAnticipado(oRequest);
            objLog.CrearArchivolog(null, "Fin GrabarPagoAnticipado", null);
        }
        //FIN PROY-140546

        //PROY-32439 MAS INI Obtener Parametros Nvo BRMS Grabar
        public void ObtenerParametrosNvoBRMS(ValidacionDeudaBRMS objVariablesEntradaNVoBRMS, Int64 nroSEC)
        {
            GeneradorLog objLog = new GeneradorLog("Grabar Nvo BRMS", null, null, null);
            string strCliente = String.Empty;
            string strSolicitud = String.Empty;
            string strPdv = String.Empty;
            string strLinea = String.Empty;
            BEDatosClienteBrms objDatosClienteBrms = new BEDatosClienteBrms();
            objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][Grabar Nvo BRMS][INICIO]", ""), null);
            objDatosClienteBrms.DAVCN_SOLIN_CODIGO = nroSEC.ToString();
            //Cliente
            strCliente += objVariablesEntradaNVoBRMS.request.cliente.antiguedadDeuda + "|";
            string strBloqueos = string.Empty;

            if (!Object.Equals(objVariablesEntradaNVoBRMS.request.cliente.bloqueos, null))
            {
                foreach (var item in objVariablesEntradaNVoBRMS.request.cliente.bloqueos)
                {
                    strBloqueos = strBloqueos + item.tipo + "," + item.tipoLinea + ";";
                }
                if (!string.IsNullOrWhiteSpace(strBloqueos))
                {
                    strBloqueos = strBloqueos.Substring(0, strBloqueos.Length - 1);
                }
            }
            else
            {
                strBloqueos = "";
            }
            strCliente += strBloqueos + "|";
            strCliente += objVariablesEntradaNVoBRMS.request.cliente.cantidadDocumentosDeuda + "|";
            strCliente += objVariablesEntradaNVoBRMS.request.cliente.comportamientoPago + "|";
            strCliente += objVariablesEntradaNVoBRMS.request.cliente.disputa.antiguedad + "|";
            strCliente += objVariablesEntradaNVoBRMS.request.cliente.disputa.cantidad + "|";
            strCliente += objVariablesEntradaNVoBRMS.request.cliente.disputa.monto + "|";
            strCliente += objVariablesEntradaNVoBRMS.request.cliente.documento.numero + "|";
            strCliente += objVariablesEntradaNVoBRMS.request.cliente.documento.tipo + "|";
            strCliente += objVariablesEntradaNVoBRMS.request.cliente.flagBloqueos + "|";
            strCliente += objVariablesEntradaNVoBRMS.request.cliente.flagSuspensiones + "|";
            strCliente += objVariablesEntradaNVoBRMS.request.cliente.montoDeuda + "|";
            strCliente += objVariablesEntradaNVoBRMS.request.cliente.montoDeudaCastigada + "|";
            strCliente += objVariablesEntradaNVoBRMS.request.cliente.montoDeudaVencida + "|";
            strCliente += objVariablesEntradaNVoBRMS.request.cliente.montoTotalPago + "|";
            strCliente += objVariablesEntradaNVoBRMS.request.cliente.promedioFacturadoSoles + "|";
            string strSuspension = string.Empty;
            if (!Object.Equals(objVariablesEntradaNVoBRMS.request.cliente.suspensiones, null))
            {
                foreach (var item in objVariablesEntradaNVoBRMS.request.cliente.suspensiones)
                {
                    strSuspension = strSuspension + item.tipo + "," + item.tipoLinea + ";";
                }
                if (!string.IsNullOrWhiteSpace(strSuspension))
                {
                    strSuspension = strSuspension.Substring(0, strSuspension.Length - 1);
                }
            }
            else
            {
                strSuspension = "";
            }
            strCliente += strSuspension + "|";
            strCliente += objVariablesEntradaNVoBRMS.request.cliente.tiempoPermanencia + "|";
            string strFraude = string.Empty;
            if (!Object.Equals(objVariablesEntradaNVoBRMS.request.cliente.tiposFraude, null))
            {
                foreach (var item in objVariablesEntradaNVoBRMS.request.cliente.tiposFraude)
                {
                    if (Funciones.CheckStr(item).Length > 0)
                    {
                        strFraude = strFraude + item + ";";
                    }
                }
                if (!string.IsNullOrWhiteSpace(strFraude))
                {
                    strFraude = strFraude.Substring(0, strFraude.Length - 1);
                }
            }
            else
            {
                strFraude = "";
            }
            strCliente += strFraude + "|"; // PROY-140422

            //PROY-140743-INI
            string strMensajeNuevo = Funciones.CheckStr(HttpContext.Current.Session["strMensajeError"]);
            if (string.IsNullOrEmpty(strMensajeNuevo))
            {
                strCliente += Funciones.CheckDbl(objVariablesEntradaNVoBRMS.request.cliente.montoCuotasPendientesAcc) + "|";
                strCliente += Funciones.CheckInt64(objVariablesEntradaNVoBRMS.request.cliente.cantidadLineaCuotasPendientesAcc) + "|";
                strCliente += Funciones.CheckInt64(objVariablesEntradaNVoBRMS.request.cliente.cantidadMaximaCuotasPendientesAcc) + "|";
                strCliente += Funciones.CheckDbl(objVariablesEntradaNVoBRMS.request.cliente.montoCuotasPendientesAccUltiVenta) + "|";
                strCliente += Funciones.CheckInt64(objVariablesEntradaNVoBRMS.request.cliente.cantidadLineaCuotasPendientesAccUltiVenta) + "|";
                strCliente += Funciones.CheckInt64(objVariablesEntradaNVoBRMS.request.cliente.cantidadMaximaCuotasPendientesAccUltiVenta) + "|";
            }
            else
            {
                strCliente += "||||||";
            }
            //PROY-140743-FIN
            strCliente += ""; // PROY-140422 segmento del cliente

            objDatosClienteBrms.DAVCV_IN_CLIENTE = strCliente;
            //Renovacion
            if (!Object.Equals(objVariablesEntradaNVoBRMS.request.lineaARenovar, null))
            {
                strLinea += objVariablesEntradaNVoBRMS.request.lineaARenovar.fechaActivacion + "|";
                strLinea += objVariablesEntradaNVoBRMS.request.lineaARenovar.antiguedad;
                objDatosClienteBrms.DAVCV_IN_LINEA = strLinea;
            }
            //PDV
            strPdv += objVariablesEntradaNVoBRMS.request.puntoDeVenta.canal + "|";
            strPdv += objVariablesEntradaNVoBRMS.request.puntoDeVenta.codigo + "|";
            strPdv += objVariablesEntradaNVoBRMS.request.puntoDeVenta.departamento + "|";
            strPdv += objVariablesEntradaNVoBRMS.request.puntoDeVenta.nombre + "|";
            strPdv += objVariablesEntradaNVoBRMS.request.puntoDeVenta.region + "|";
            strPdv += objVariablesEntradaNVoBRMS.request.puntoDeVenta.vendedor.codigo + "|";
            strPdv += objVariablesEntradaNVoBRMS.request.puntoDeVenta.vendedor.nombre;
            objDatosClienteBrms.DAVCV_IN_PDV = strPdv;

            //Solicitud
            strSolicitud += objVariablesEntradaNVoBRMS.request.sistemaEvaluacion + "|";
            strSolicitud += objVariablesEntradaNVoBRMS.request.tipoOperacion;
            objDatosClienteBrms.DAVCV_IN_SOLICITUD = strSolicitud;

            objDatosClienteBrms.DAVCV_VAL_CLIENTE = Funciones.CheckStr(objVariablesEntradaNVoBRMS.response.validacionCliente);
            objDatosClienteBrms.DAVCV_MSJ_VALCLIENTE = objVariablesEntradaNVoBRMS.response.mensajeValidacionCliente;
            objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][Grabar Nvo BRMS][IN][objDatosClienteBrms.DAVCN_SOLIN_CODIGO]", objDatosClienteBrms.DAVCN_SOLIN_CODIGO), null);
            objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][Grabar Nvo BRMS][IN][objDatosClienteBrms.DAVCV_IN_CLIENTE]", objDatosClienteBrms.DAVCV_IN_CLIENTE), null);
            objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][Grabar Nvo BRMS][IN][objDatosClienteBrms.DAVCV_IN_LINEA]", objDatosClienteBrms.DAVCV_IN_LINEA), null);
            objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][Grabar Nvo BRMS][IN][objDatosClienteBrms.DAVCV_IN_PDV]", objDatosClienteBrms.DAVCV_IN_PDV), null);
            objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][Grabar Nvo BRMS][IN][objDatosClienteBrms.DAVCV_IN_SOLICITUD]", objDatosClienteBrms.DAVCV_IN_SOLICITUD), null);
            objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][Grabar Nvo BRMS][IN][objDatosClienteBrms.DAVCV_VAL_CLIENTE]", objDatosClienteBrms.DAVCV_VAL_CLIENTE), null);
            objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][Grabar Nvo BRMS][IN][objDatosClienteBrms.DAVCV_MSJ_VALCLIENTE]", objDatosClienteBrms.DAVCV_MSJ_VALCLIENTE), null);

            objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][Grabar Nvo BRMS][INICIO][InsertarDatosBRMSCliente]", ""), null);
            new BLReglaNegocio().InsertarDatosBRMSCliente(objDatosClienteBrms);
            objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][Grabar Nvo BRMS][FIN][InsertarDatosBRMSCliente]", ""), null);
        }
        //PROY-32439 MAS FIN Obtener Parametros Nvo BRMS Grabar

        public bool GrabarPlanDetalleMovil(BEPlanDetalleVenta objDetallePlan, string idOficina, ref Int64 idSol)
        {
            bool salida = false;
                idSol = 0;
                DAConsumer obj = new DAConsumer();

                if (obj.InsertarPlanSolicitud(objDetallePlan, ref idSol)) // GRABAR PLANES
                {
                    objDetallePlan.SOPLN_CODIGO = idSol;
                    obj.InsertarPlanDetalleVenta(objDetallePlan); // GRABAR DETALLE VENTA
                    if (objDetallePlan.SERVICIO != null && objDetallePlan.SERVICIO.Count > 0) // GRABAR SERVICIOS
                    {
                        obj.InsertarPlanServicio(objDetallePlan, idOficina);
                    }
                }
                salida = true;
            return salida;
        }

        public bool GrabarPlanDetalleDTH(BEPlanDetalleVenta objDetallePlan, ref Int64 idSol)
        {
            bool salida = false;
                idSol = 0;
                DAConsumer obj = new DAConsumer();

                if (obj.InsertarPlanSolicitud(objDetallePlan, ref idSol)) // GRABAR PLANES
                {
                    objDetallePlan.SOPLN_CODIGO = idSol;
                    obj.InsertarPlanDetalleVenta(objDetallePlan); // GRABAR DETALLE VENTA
                    if (objDetallePlan.SERVICIO != null && objDetallePlan.SERVICIO.Count > 0) // GRABAR SERVICIOS
                    {
                        obj.InsertarPlanServicio(objDetallePlan, string.Empty);
                    }
                }
                salida = true;
            return salida;
        }

        public bool GrabarPlanDetalleHFC(BEPlanDetalleVenta objDetallePlan, ref Int64 idSol)
        {
            bool salida = false;
                idSol = 0;
                DAConsumer obj = new DAConsumer();
                BEPlanSolicitudHFC oPlanDetalleHFC = objDetallePlan.PLAN_SOL_HFC;
                //PROY-29215 INICIO
                oPlanDetalleHFC.FormaPago = objDetallePlan.FORMA_PAGO;
                oPlanDetalleHFC.NroCuota = Convert.ToInt32(objDetallePlan.CUOTA_PAGO);
                //PROY-29215 FIN

                 if (obj.InsertarPlanHFC(oPlanDetalleHFC, objDetallePlan.SOLIN_CODIGO,objDetallePlan.PRDC_CODIGO, ref idSol)) // GRABAR PLANES
                {
                    objDetallePlan.SOPLN_CODIGO = idSol;
                    if (oPlanDetalleHFC.oServicio != null && oPlanDetalleHFC.oServicio.Count > 0) // GRABAR SERVICIOS
                    {
                         foreach (var item in oPlanDetalleHFC.oServicio)
                        {
                            item.Producto = objDetallePlan.PRDC_CODIGO;
                        }
                       obj.InsertarPlanServicioHFC(oPlanDetalleHFC, idSol);
                    }
                    if (oPlanDetalleHFC.oEquipo != null && oPlanDetalleHFC.oEquipo.Count > 0) // GRABAR EQUIPOS
                    {
                         foreach (var item in oPlanDetalleHFC.oEquipo)
                        {
                            item.Prdc_Codigo = objDetallePlan.PRDC_CODIGO;
                        }
                      
                        obj.InsertarPlanEquipoHFC(oPlanDetalleHFC, idSol);
                    }
                }
                salida = true;
            return salida;
        }

        public bool GrabarDetalleVentaVarios(BEPlanDetalleVenta objDetallePlan, ref Int64 idSol)
        {
            bool salida = false;
                idSol = 0;
                salida = new DAConsumer().InsertarDetalleVentaVarios(objDetallePlan, ref idSol);
            return salida;
        }

        public bool GrabarBolsaLinea(string strLineas, Int64 nroSEC, Int64 intVersion, string strUsuario)
        {
            return new DAConsumer().GrabarBolsaLinea(strLineas, nroSEC, intVersion, strUsuario);
        }

        public double CalcularLineaCreditoDescentralizado(int pIdTipoCliente, int pIdSegmento, int pIdTipoRiesgo, double pDeudaFinanciera)
        {
            double dLineaCredito = 0.0;
            double dPorcentajeLC = ObtenerPorcentajeLCD(pIdTipoCliente, pIdSegmento, pIdTipoRiesgo);
            if ((dPorcentajeLC >= 0) || (pDeudaFinanciera >= 0))
                dLineaCredito = (dPorcentajeLC * pDeudaFinanciera) / 100;

            return dLineaCredito;
        }
        public double ObtenerPorcentajeLCD(int pIdTipoCliente, int pIdSegmento, int pIdTipoRiesgo)
        {
            return new DAConsumer().ObtenerPorcentajeLCD(pIdTipoCliente, pIdSegmento, pIdTipoRiesgo);
        }
        public string ConsultaBlackListCuotaPdv(string oficina)
        {
            return new DAConsumer().ConsultaBlackListCuotaPdv(oficina);
        }
        //gaa20151201
        public DataTable ListarSecReno(string strNroDocIde)
        {
            return new DAConsumer().ListarSecReno(strNroDocIde);
        }
        //fin gaa20151201
//gaa20151929
        public bool ValidacionAccesoOpcionEP(string strCanal, string strProducto, string strTipoOperacion, string strTipoDocumento, string strTipoValidacion)
        {
            return new DAConsumer().ValidacionAccesoOpcionEP(strCanal, strProducto, strTipoOperacion, strTipoDocumento, strTipoValidacion);
        }
//gaa20160414
        public List<BEItemGenerico> ConsultaEquiposAlternativos()
        {
            return new DAConsumer().ConsultaEquiposAlternativos();
        }
//fin gaa20160414

        //PROY-29215 INICIO
        public List<BEItemGenerico> ConsultaModoPagoyCuota(Int64 CodigoSEC)
        {
            return new DAConsumer().ConsultaModoPagoyCuota(CodigoSEC);
        }

        public bool GrabarFormaPagoCuota(Int64 nroSEC, Int64 strCuota, string strFormaPago)
        {
            return new DAConsumer().GrabarFormaPagoCuota(nroSEC, strCuota, strFormaPago);
        }

        public bool GrabarFormaPagoCuotaEmpresa(Int64 nroSEC, Int64 strCuota, string strFormaPago, string formaPagoActual, string cuotaActual, string Usuario)
        {
            return new DAConsumer().GrabarFormaPagoCuotaEmpresa(nroSEC, strCuota, strFormaPago, formaPagoActual, cuotaActual, Usuario);
        }
        //PROY-29215 FIN

        // INC000003135879 INICIO
        public bool GrabarAuditoriaCostoInstalacionCreditos(Int64 nroSEC, double costo_inst_anterior, double costo_inst_modifica, string strUsuario)
        {
            return new DAConsumer().GrabarAuditoriaCostoInstalacionCreditos(nroSEC, costo_inst_anterior, costo_inst_modifica, strUsuario);
        }
        // INC000003135879 FIN
    }
}
