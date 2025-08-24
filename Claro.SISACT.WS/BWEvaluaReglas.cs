using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Common;
using Claro.SISACT.Entity;
using System.Configuration;
using Claro.SISACT.WS.WebReferenceEvaluarReglas;
using System.IO;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;
using Claro.SISACT.Business;
using System.Web;  //PROY-140736

namespace Claro.SISACT.WS
{

    public class BWEvaluaReglas
    {

        BSS_EvaluaReglasSOAP11BindingQSService _objTransaccion = new BSS_EvaluaReglasSOAP11BindingQSService();
        
        GeneradorLog _objLog = null;
        string _input = null;
        string _idAplicacion = null;
        string _usuAplicacion = null;
        string _idTransaccion = null;
        List<BEParametro> lstBEEvaluacionProactiva = new List<BEParametro>();
        public BWEvaluaReglas()
        {
            //_objTransaccion.Url = ConfigurationManager.AppSettings["RutaWS_EvaluaReglas"].ToString();
            //_objTransaccion.Timeout = Funciones.CheckInt(ConfigurationManager.AppSettings["TimeOut_EvaluaReglas"].ToString());
            //EMMH I
            List<BEParametro> lstBEParametroEvaluacionProactiva = new List<BEParametro>();
            string strCodGrupoParamEvaluacionProactiva = Funciones.CheckStr(ConfigurationManager.AppSettings["consCodGrupoParamEvaluacionProactiva"]);
            if (!string.IsNullOrEmpty(strCodGrupoParamEvaluacionProactiva))
                lstBEEvaluacionProactiva = (new BLGeneral()).ListaParametrosGrupo(Funciones.CheckInt64(strCodGrupoParamEvaluacionProactiva));
            if (lstBEEvaluacionProactiva.Count > 0) lstBEEvaluacionProactiva = lstBEEvaluacionProactiva.OrderBy(p => p.Valor1).ToList();
            _objTransaccion.Url = lstBEEvaluacionProactiva.Where(p => p.Valor1 == "RutaWS_EvaluaReglas").SingleOrDefault().Valor;
            _objTransaccion.Timeout = Convert.ToInt32(lstBEEvaluacionProactiva.Where(p => p.Valor1 == "TimeOut_EvaluaReglas").SingleOrDefault().Valor);
            //EMMH F 

            _objTransaccion.Credentials = System.Net.CredentialCache.DefaultCredentials;
            _idAplicacion = ConfigurationManager.AppSettings["CodigoAplicacion"].ToString();
            _usuAplicacion = ConfigurationManager.AppSettings["constNombreAplicacion"].ToString();
            _idTransaccion = DateTime.Now.ToString("hhmmssfff");
        }

        public BEPlanProactivo obtenerEvaluacionProactiva(HeaderRequestType _requestHeader, obtenerEvaluacionProactivaRequestType _requestEval,
                                                          ref BEItemMensaje objMensaje, WS.WSReglasCrediticia.ClaroEvalClientesReglasRequest oRequestReglaCrediticia)
        {
            GeneradorLog objLog = new GeneradorLog(null, null, null, "Log_BWEvaluaReglas");
            objLog.CrearArchivolog("[Inicio][BWobtenerEvaluacionProactiva]", null, null);
            try
            {
                HeaderResponseType headerResponse = new HeaderResponseType();
                obtenerEvaluacionProactivaResponseType bodyResponse = new obtenerEvaluacionProactivaResponseType();

                try
                {
                    DatosPlanType datosPlan = new DatosPlanType();
                    datosPlan.modalidad_venta = _requestEval.datosPlan.modalidad_venta;
                    datosPlan.tipo_flujo =  _requestEval.datosPlan.tipo_flujo;

                    string strTipoDoc = "";
                    if (_requestEval.datosPlan.tipo_documento == ConfigurationManager.AppSettings["TipoDocumentoRUC"])
                        strTipoDoc = Funciones.TipoRUC1020(_requestEval.solicitud.cliente.documento.numero);
                    else
                        strTipoDoc = _requestEval.datosPlan.tipo_documento;
               
                    //datosPlan.tipo_documento = _requestEval.datosPlan.tipo_documento;
                    datosPlan.tipo_documento = strTipoDoc;
                    
                    datosPlan.tipo_oferta =  _requestEval.datosPlan.tipo_oferta;
                    datosPlan.tipo_operacion = _requestEval.datosPlan.tipo_operacion;
                    datosPlan.tipo_producto =  _requestEval.datosPlan.tipo_producto;
                    datosPlan.codigoEspecial = string.Empty;
                    datosPlan.campana = _requestEval.datosPlan.campana;
                    datosPlan.codigoplazo = _requestEval.datosPlan.codigoplazo;
                    datosPlan.combo = string.Empty;
                    datosPlan.familia = _requestEval.datosPlan.familia;
                    datosPlan.oficina = _requestEval.datosPlan.oficina;
                    //datosPlan.tipo_venta = _requestEval.datosPlan.tipo_venta
                    datosPlan.tipo_venta =  ConfigurationManager.AppSettings["strTVPostpago"].ToString();
                        
                    datosPlan.codigoMaterial = _requestEval.datosPlan.codigoMaterial;
                    datosPlan.codigoOperacion = _requestEval.datosPlan.codigoOperacion;
                    datosPlan.servicioAdiResq = _requestEval.datosPlan.servicioAdiResq;

                    Documento documento = new Documento();
                    documento.descripcion = _requestEval.solicitud.cliente.documento.descripcion;
                    documento.numero = _requestEval.solicitud.cliente.documento.numero;
                    Adicional adicional = new Adicional();
                    adicional.descripcion = string.Empty;                   
                    adicional.precio = _requestEval.solicitud.cliente.adicional.precio;

                    Cliente cliente = new Cliente();
                    //cliente.facturacionPromedioClaro = 0;
                    cliente.limiteDeCreditoDisponible = oRequestReglaCrediticia.solicitud.solicitud1.cliente.limiteDeCreditoDisponible;//PROY 30748 F2 MDE
                    //cliente.facturacionPromedioProducto = 0.0;
                    //cliente.comportamientoDePago = 0;
                    //cliente.cantidadDeLineasActivas = 0;
                    cliente.edad = _requestEval.solicitud.cliente.edad;
                    cliente.sexo = _requestEval.solicitud.cliente.sexo;
                    cliente.tipoCliente =_requestEval.solicitud.cliente.tipoCliente;
                    cliente.riesgo = _requestEval.solicitud.cliente.riesgo;
                    cliente.tiempoDePermanencia = _requestEval.solicitud.cliente.tiempoDePermanencia;
                    cliente.deuda =_requestEval.solicitud.cliente.deuda;
                    cliente.documento = documento;
                    cliente.adicional = adicional;
                    //EMMH I
                    cliente.representanteLega = _requestEval.solicitud.cliente.representanteLega;
                    //EMMH F
                    Equipo[] lequipo = new Equipo[_requestEval.solicitud.equipo.Length];
                    lequipo = _requestEval.solicitud.equipo;

                    l_cargoFijo[] listaPlanes = new l_cargoFijo[_requestEval.solicitud.cargoFijo.Length];
                    int count = 0;
                    foreach (var item in _requestEval.solicitud.cargoFijo)
                    {
                        if (item !=null)
                         {
                            l_cargoFijo cargofijo = new l_cargoFijo()
                            {
                                cargoFijo = item.cargoFijo,
                                descripcion = item.descripcion,
                                montoCFSEC = item.cargoFijo,
                                precioDeVenta = item.precioDeVenta
                            };
                            listaPlanes[count] = cargofijo;
                            count++;
                         }    
                    }
                    Oferta oferta = new Oferta();
                    oferta.casoEpecial = _requestEval.solicitud.oferta.casoEpecial;
                    oferta.controlDeConsumo = _requestEval.solicitud.oferta.controlDeConsumo;
                    //oferta.kitDeInstalacion = string.Empty;
                    oferta.plazoDeAcuerdo = _requestEval.solicitud.oferta.plazoDeAcuerdo;
                    oferta.productoComercial = _requestEval.solicitud.oferta.productoComercial;
                    //oferta.proteccionMovil = string.Empty;
                    oferta.segmentoDeOferta = _requestEval.solicitud.oferta.segmentoDeOferta;
                    //oferta.tipoDeOperacionEmpresa = string.Empty;
                    oferta.mesOperadorCedente = _requestEval.solicitud.oferta.mesOperadorCedente;
                    oferta.cantidadLineasSEC = _requestEval.solicitud.oferta.cantidadLineasSEC;
                    oferta.montoCFSEC =  _requestEval.solicitud.oferta.montoCFSEC ;
                    oferta.planActual = _requestEval.solicitud.oferta.planActual; //PROY-31948

                    Campana campana = new Campana();
                    //campana.tipo_campana = _requestEval.datosPlan.campana;
                    oferta.campana = _requestEval.solicitud.oferta.campana;
                    PlanSolicitado planSolicitado = new PlanSolicitado();
                    planSolicitado.cargoFijo = _requestEval.solicitud.oferta.planSolicitado.cargoFijo;
                    planSolicitado.descripcion = _requestEval.solicitud.oferta.planSolicitado.descripcion;
                    oferta.planSolicitado = planSolicitado;

                    PuntoDeVenta puntoDeVenta = new PuntoDeVenta();
                    puntoDeVenta = _requestEval.solicitud.puntoDeVenta;

                    Solicitud solicitud = new Solicitud();
                    solicitud.tipoDeOperacion = _requestEval.solicitud.tipoDeOperacion;
                    solicitud.totalPlanes = count.ToString();
                    solicitud.transaccion = _requestEval.solicitud.transaccion;
                    //solicitud.fechaEjecucion = DateTime.UtcNow;
                    solicitud.fechaEjecucion = DateTime.Now;
                    solicitud.horaEjecucion = DateTime.Now.Hour;                    
                    solicitud.tipoDeProducto = "MOVIL";

                    solicitud.tipoDeDespacho = _requestEval.solicitud.tipoDeDespacho;
                    
 
                    //--------------INICIO Datos de Request agregados de request BRMS-----------------
                    solicitud.buroConsultado = oRequestReglaCrediticia.solicitud.solicitud1.buroConsultado;
                    
                    cliente.facturacionPromedioClaro = oRequestReglaCrediticia.solicitud.solicitud1.cliente.facturacionPromedioClaro;                    
                    cliente.facturacionPromedioProducto = oRequestReglaCrediticia.solicitud.solicitud1.cliente.facturacionPromedioProducto;
                    cliente.comportamientoDePago = oRequestReglaCrediticia.solicitud.solicitud1.cliente.comportamientoDePago;
                    cliente.cantidadDeLineasActivas = oRequestReglaCrediticia.solicitud.solicitud1.cliente.cantidadDeLineasActivas;

                    oferta.combo = oRequestReglaCrediticia.solicitud.solicitud1.oferta.combo;
                    oferta.modalidadCedente = oRequestReglaCrediticia.solicitud.solicitud1.oferta.modalidadCedente;
                    oferta.operadorCedente = oRequestReglaCrediticia.solicitud.solicitud1.oferta.operadorCedente;
                    oferta.kitDeInstalacion = oRequestReglaCrediticia.solicitud.solicitud1.oferta.kitDeInstalacion;
                    oferta.proteccionMovil = Funciones.CheckStr(oRequestReglaCrediticia.solicitud.solicitud1.oferta.proteccionMovil);
                    oferta.tipoDeOperacionEmpresa = oRequestReglaCrediticia.solicitud.solicitud1.oferta.tipoDeOperacionEmpresa;
                    oferta.planSolicitado.paquete = Funciones.CheckDbl(oRequestReglaCrediticia.solicitud.solicitud1.oferta.planSolicitado.paquete);
                    cliente.representanteLega = _requestEval.solicitud.cliente.representanteLega;

                    cliente.contribuyentes = _requestEval.solicitud.cliente.contribuyentes;/*PROY - 30748 F2 MDE / PROY-32438 INI*/
                    //PROY 30748 F2 MDE / PROY-32439 MAS INI
                    cliente.montoDeudaVencida = _requestEval.solicitud.cliente.montoDeudaVencida;
                    cliente.montoDeudaCastigada = _requestEval.solicitud.cliente.montoDeudaCastigada;
                    cliente.montoDisputa = _requestEval.solicitud.cliente.montoDisputa;
                    cliente.cantidadMontoDisputa = _requestEval.solicitud.cliente.cantidadMontoDisputa;
                    cliente.antiguedadMontoDisputa = _requestEval.solicitud.cliente.antiguedadMontoDisputa;
                    cliente.montoTotalDeuda = _requestEval.solicitud.cliente.montoTotalDeuda;
                    cliente.antiguedadDeuda = _requestEval.solicitud.cliente.antiguedadDeuda;
                    //PROY 30748 F2 MDE / PROY-32439 MAS FIN
                    cliente.segmento = _requestEval.solicitud.cliente.segmento;//PROY 30748 F2 MDE / PROY-140230-MAS
                    //--------------FIN Datos de Request agregados del BRMS---------------
                                       
                    //PROY 30748 F2 FALLA MDE / PROY-31948 INI
                    cliente.montoPendienteCuotasSistema = _requestEval.solicitud.cliente.montoPendienteCuotasSistema;
                    cliente.cantidadPlanesCuotasPendientesSistema = _requestEval.solicitud.cliente.cantidadPlanesCuotasPendientesSistema;
                    cliente.cantidadMaximaCuotasPendientesSistema = _requestEval.solicitud.cliente.cantidadMaximaCuotasPendientesSistema;
                    //PROY 30748 F2 FALLA MDE / PROY-31948 FIN

                    cliente.flagWhitelist = _requestEval.solicitud.cliente.flagWhitelist;//PROY 140579 F2
                                       
                    solicitud.cliente = cliente;
                    solicitud.cliente.creditscore = _requestEval.solicitud.cliente.creditscore;
                    solicitud.cliente.cantidadDePlanesPorProducto = _requestEval.solicitud.cliente.cantidadDePlanesPorProducto;
                    solicitud.equipo = lequipo;
                    //Add ini
                    if (solicitud.equipo[0].costo_equipo == 0)
                    {
                        solicitud.equipo[0].costo_equipo = Funciones.CheckDbl(oRequestReglaCrediticia.solicitud.solicitud1.equipo[0].costo);

                    }
                    if (solicitud.equipo[0].precioDeVenta == 0)
                    {
                        solicitud.equipo[0].precioDeVenta = Funciones.CheckDecimal(oRequestReglaCrediticia.solicitud.solicitud1.equipo[0].precioDeVenta);
                    }
                    //add fin
                    solicitud.cargoFijo = listaPlanes;
                    solicitud.oferta = oferta;
                    solicitud.puntoDeVenta = puntoDeVenta;
                 
                    var Campania = HttpContext.Current.Session["ExisteBuyback"].ToString();  //PROY-140736 INI
                    //PROY-140335 RF1 INI
                    if (_requestEval.datosPlan.tipo_flujo == "2" || Campania == "SI") //PROY-140736 INI
                    {
                        solicitud.listaResquestOpcional = _requestEval.solicitud.listaResquestOpcional;
                        HttpContext.Current.Session["ExisteBuyback"] = string.Empty;
                    }
                    //PROY-140335 RF1 FIN

                    obtenerEvaluacionProactivaRequestType bodyRequest = new obtenerEvaluacionProactivaRequestType();

                    bodyRequest.origen = ConfigurationManager.AppSettings["AbreviaturaOpcion"].ToString();
                    bodyRequest.datosPlan = datosPlan;
                    bodyRequest.solicitud = solicitud;

                    System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(obtenerEvaluacionProactivaRequestType));
                    StringBuilder str = new StringBuilder();
                    TextWriter tw = new StringWriter(str);
                    x.Serialize(tw, bodyRequest);
                    tw.Close();
                    string xml = str.ToString();

                    objLog.CrearArchivolog("[xml][bodyRequestBSsEval]", null, null);
                    objLog.CrearArchivolog("[xml][Inicio]" + xml, null, null);


                 



                    bodyResponse = _objTransaccion.obtenerEvaluacionProactiva(bodyRequest);

                    
                    objLog.CrearArchivolog("[xml][Fin]", null, null);

                    System.Xml.Serialization.XmlSerializer xResp = new System.Xml.Serialization.XmlSerializer(typeof(obtenerEvaluacionProactivaResponseType));

                    StringBuilder strResp = new StringBuilder();
                    TextWriter twResp = new StringWriter(strResp);
                    xResp.Serialize(twResp, bodyResponse);
                    twResp.Close();
                    string xmlResp = strResp.ToString();
                    objLog.CrearArchivolog("[xml][bodyResponseBSsEval]", null, null);
                    objLog.CrearArchivolog("[xml][Inicio]" + xmlResp, null, null);
                    objLog.CrearArchivolog("[xml][Fin]", null, null);
                }

                catch (Exception ex)
                {
                    objLog.CrearArchivolog("[ERROR][BWobtenerEvaluacionProactiva]" +ex.Message, null, null);

                }


                List<BEPlanBSSEval> objPlanBSSEval = new List<BEPlanBSSEval>();
                BEPlanProactivo objPlanProactivo = new BEPlanProactivo();

                var listaPlanesProactivos = bodyResponse.responseData.listaPlanes.listaPlanesProactivos;
         
                // listaPlanesProactivos = bodyResponse.responseData.listaPlanes.listaPlanesProactivos.OrderBy(x => x.cargoFijo).ToArray();  
	                      

                if (listaPlanesProactivos == null)
                {
                    return new BEPlanProactivo() { 
                    PlanBSSEval = new List<BEPlanBSSEval>()};
                }

                foreach (var item in listaPlanesProactivos)
                {
                    BEPlanBSSEval objPlan = new BEPlanBSSEval();
                    objPlan.cantidadDeLineasAdicionalesRUC = item.cantidadDeLineasAdicionalesRUC;
                    objPlan.cantidadDeLineasMaximas = item.cantidadDeLineasMaximas;
                    objPlan.capacidadDePago = item.capacidadDePago;
                    objPlan.codigo = item.codigoPlan;
                    objPlan.descripcion = item.descripcion;
                    objPlan.factorDeEndeudamientoCliente = item.factorDeEndeudamientoCliente;
                    objPlan.factorDeRenovacionCliente = item.factorDeRenovacionCliente;
                    objPlan.montoCFParaRUC = item.montoCFParaRUC;
                    objPlan.montoDeGarantia = item.montoDeGarantia;
                    objPlan.ejecucionConsultaPrevia = _requestEval.datosPlan.tipo_flujo == "2" ? item.ejecucionConsultaPrevia : string.Empty; //PROY-140335 RF1
                   
                    //Cuota
                    
                    if (item.cuotas != null)
                    {
                        BECuota objCuota = new BECuota()
                        {
                            cuota = item.cuotas.cantidad,
                            porcenCuotaInicial = Funciones.CheckDbl(item.cuotas.porcentajeInicial)
                        };
                        objPlan.Cuota = objCuota;
                    }
                    objPlan.precioDeVentaTotalEquipos = item.precioDeVentaTotalEquipos;
                    objPlan.procesoIDValidator = item.procesoIDValidator;
                    objPlan.restriccion = item.restriccion;
                    objPlan.riesgoTotalEquipo = item.riesgoTotalEquipo;
                    objPlan.tipodecobro = item.tipodecobro;
                    objPlan.MontoRA =Funciones.CheckDbl(item.montoRA,2);
                    objPlan.PrecionVenta =  item.precioVenta;
                    objPlan.cargoFijo = item.cargoFijo;
                    objPlan.cantidad =item.cantidad;
                    objPlan.totalPagar = item.totalPagar;
                    objPlan.TipoDeAutonomiaCargoFijo = item.tipoDeAutonomiaCargoFijo;
                    objPlan.CostoEquipo = bodyResponse.responseData.listaPlanes.costoEquipo;// costoEquipo; //EMMH
//INICIO PROY-30748
                    //EMMH I

                    if (objPlan.CostoEquipo == 0)
                    {
                        if (_requestEval.datosPlan.codigoMaterial != "")
                        {
                        List<BEConsultarPrecioBase> oListConsultaPrecioBase = BLSincronizaSap.ConsultarPrecioBase(_requestEval.datosPlan.codigoMaterial, null);
                        double dblCosto = 0, dblCostoTmp = 0;
                        foreach (BEConsultarPrecioBase itemCosto in oListConsultaPrecioBase)
                        {
                            dblCostoTmp = Funciones.CheckDbl(itemCosto.PrecioCompra);
                            if (dblCostoTmp > dblCosto)
                            {
                                dblCosto = dblCostoTmp;
                            }
                        }
                        objPlan.CostoEquipo = dblCosto;
                    }
                        else {
                            objPlan.CostoEquipo = 0;
                        }
                    }

                    //Obtiene Id Lista Precio
                    BEFormEvaluacion objBEForm = new BEFormEvaluacion();

                    objBEForm.idOferta = _requestEval.datosPlan.tipo_oferta;
                    objBEForm.idTipoVenta = ConfigurationManager.AppSettings["strTVPostpago"].ToString();
                    objBEForm.idTipoOficina = _requestEval.datosPlan.oficina;
                    objBEForm.idMaterial = _requestEval.datosPlan.codigoMaterial;
                    objBEForm.idCampanaSap = _requestEval.datosPlan.campana;
                    objBEForm.idTipoOperacion = _requestEval.datosPlan.tipo_operacion;
                    objBEForm.idPlazo = _requestEval.datosPlan.codigoplazo;
                    objBEForm.idPlanSap = item.codigoPlan;
                    objPlan.IdListaPrecio = item.codigoProducto;//PROY 30748 F2 MDE
                    objPlan.CodListaPrecio = item.codigoProducto;//PROY 30748 F2 MDE
                    objPlan.DesListaPrecio = item.descripcionPrecio;//PROY 30748 F2 MDE

                    //EMMH F

                    //Servicios Adicionales
                    string [] strServ = item.servicioAdiResp.Split('|');
                    for(int i=0; i < strServ.Length; i++)
                    {
                        string [] strServCode = strServ[i].Split('_');
                        if(strServCode[0].Trim() == ConfigurationManager.AppSettings["constCodTopeAutomatico"])
                        {
                            objLog.CrearArchivolog("[topeMonto][plan]" + item.codigoPlan, null, null);
                            string strResultado = string.Empty;
                            List<BEItemGenerico> objLista = new BLGeneral_II().ListarTopeAutomatico(item.codigoPlan);
                            foreach (BEItemGenerico obj in objLista)
                            {
                                if (obj.Estado == "1")
                                {
                                    //strResultado = Funciones.CheckStr(obj.Monto);
                                    objPlan.TopeMonto = Funciones.CheckDbl(obj.Monto);
                                    objLog.CrearArchivolog("[topeMonto][monto]" + objPlan.TopeMonto, null, null);
                                    break;
                                }
                            }
                        }
                    }
                    
                    item.servicioAdiResp = (item.servicioAdiResp).Replace("|", " ");//PROY 30748 F2 MDE
                    objPlan.ServiciosAdicionales = item.servicioAdiResp;
//FIN PROY-30748
                    //INICIATIVA 920
                    if (_requestEval.datosPlan.modalidad_venta == "VENTA EN CUOTAS"
                        || _requestEval.datosPlan.modalidad_venta == "CUOTAS SIN CODE")
                    {
                        objPlan.cargoFijoBase = objPlan.cargoFijo - (objPlan.PrecionVenta / Convert.ToDouble(item.cuotas.cantidad));   
                    }

                    objPlan.mostrarMotivoDeRestriccion = item.mostrarMotivoDeRestriccion;//PROY-140579 F2
                    objPlan.motivoDeRestriccion = item.motivoDeRestriccion;//PROY-140579 F2
                   
                    objPlanBSSEval.Add(objPlan);

                    objLog.CrearArchivolog("[bodyResponseBSSEval][cantidadDeLineasAdicionalesRUC]: " + objPlan.cantidadDeLineasAdicionalesRUC, null, null);
                    objLog.CrearArchivolog("[bodyResponseBSSEval][cantidadDeLineasMaximas]: " + objPlan.cantidadDeLineasMaximas, null, null);
                    objLog.CrearArchivolog("[bodyResponseBSSEval][capacidadDePago]: " + objPlan.capacidadDePago, null, null);
                    objLog.CrearArchivolog("[bodyResponseBSSEval][codigo]: " + objPlan.codigo, null, null);
                    objLog.CrearArchivolog("[bodyResponseBSSEval][descripcion]: " + objPlan.descripcion, null, null);
                    objLog.CrearArchivolog("[bodyResponseBSSEval][factorDeEndeudamientoCliente]: " + Funciones.CheckStr(objPlan.factorDeEndeudamientoCliente), null, null);                                                 
                    objLog.CrearArchivolog("[bodyResponseBSSEval][factorDeRenovacionCliente]: " + Funciones.CheckStr(objPlan.factorDeRenovacionCliente), null, null);
                    objLog.CrearArchivolog("[bodyResponseBSSEval][montoCFParaRUC]: " + Funciones.CheckStr(objPlan.montoCFParaRUC), null, null);
                    objLog.CrearArchivolog("[bodyResponseBSSEval][montoDeGarantia]: " + Funciones.CheckStr(objPlan.montoDeGarantia), null, null);
                    objLog.CrearArchivolog("[bodyResponseBSSEval][precioDeVentaTotalEquipos]: " + Funciones.CheckStr(objPlan.precioDeVentaTotalEquipos), null, null);
                    objLog.CrearArchivolog("[bodyResponseBSSEval][procesoIDValidator]: " + objPlan.procesoIDValidator, null, null);
                    objLog.CrearArchivolog("[bodyResponseBSSEval][restriccion]: " + objPlan.restriccion, null, null);                                      
                    objLog.CrearArchivolog("[bodyResponseBSSEval][riesgoTotalEquipo]: " + Funciones.CheckStr(objPlan.riesgoTotalEquipo), null, null);
                    objLog.CrearArchivolog("[bodyResponseBSSEval][tipodecobro]: " + objPlan.tipodecobro, null, null);
                    objLog.CrearArchivolog("[bodyResponseBSSEval][MontoRA]: " + Funciones.CheckStr(objPlan.MontoRA), null, null);
                    objLog.CrearArchivolog("[bodyResponseBSSEval][PrecionVenta]: " + Funciones.CheckStr(objPlan.PrecionVenta), null, null);
                    objLog.CrearArchivolog("[bodyResponseBSSEval][cargoFijo]: " + Funciones.CheckStr(objPlan.cargoFijo), null, null);
                    objLog.CrearArchivolog("[bodyResponseBSSEval][cantidad]: " + objPlan.cantidad, null, null);
                    objLog.CrearArchivolog("[bodyResponseBSSEval][totalPagar]: " +  Funciones.CheckStr(objPlan.totalPagar), null, null);
                    objLog.CrearArchivolog("[bodyResponseBSSEval][TipoDeAutonomiaCargoFijo]: " + objPlan.TipoDeAutonomiaCargoFijo, null, null);
                    objLog.CrearArchivolog("[bodyResponseBSSEval][ejecucionConsultaPrevia]: " + objPlan.ejecucionConsultaPrevia, null, null); //PROY-140335 RF1
                    objLog.CrearArchivolog("[bodyResponseBSSEval][mostrarMotivoDeRestriccion]: " + Funciones.CheckStr(objPlan.mostrarMotivoDeRestriccion), null, null);//PROY-140579 F2
                    objLog.CrearArchivolog("[bodyResponseBSSEval][motivoDeRestriccion]: " + Funciones.CheckStr(objPlan.motivoDeRestriccion), null, null);//PROY-140579 F2
//                    objLog.CrearArchivolog("[bodyResponseBSSEval][ServiciosAdicionalesCompatibles]: " + Funciones.CheckStr(objPlan.ServiciosAdicionalesCompatibles), null, null);
                   //INICIATIVA 920
                    if (_requestEval.datosPlan.modalidad_venta == "VENTA EN CUOTAS" 
                        || _requestEval.datosPlan.modalidad_venta == "CUOTAS SIN CODE")
                    {
                        objLog.CrearArchivolog("[bodyResponseAdd][cargoFijoBase]: " + Funciones.CheckStr(objPlan.cargoFijoBase), null, null);
                    }
                   
                }

                //if (_requestEval.datosPlan.modalidad_venta == "VENTA EN CUOTAS")
                //{
                //    //objPlanBSSEval = objPlanBSSEval.OrderBy(x => x.cargoFijoBase).ToList(); 
                //}

                //else
                //{
                //    //objPlanBSSEval = objPlanBSSEval.OrderBy(x => x.cargoFijo).ToList(); 
                //}

                objPlanProactivo.PlanBSSEval = objPlanBSSEval;
                objPlanProactivo.BSSEvalTotalPlanes = Funciones.CheckInt(bodyResponse.responseData.listaPlanes.totalPlanes);
                objLog.CrearArchivolog("[bodyResponseBSSEval][BSSEvalTotalPlanes]:" + Funciones.CheckStr(objPlanProactivo.BSSEvalTotalPlanes), null, null);

                //PROY-140579 F2 INICIO
                if (bodyResponse.responseData.listaResponseOpcional.Length > 0)
                {
                    string concatenadoIdsHistoricoProactiva = string.Empty;
                    foreach (var paramOpc in bodyResponse.responseData.listaResponseOpcional)
                    {
                        if (paramOpc.campo == "idGenerados")
                        {
                            concatenadoIdsHistoricoProactiva = paramOpc.valor;
                        }
                    }

                    if (concatenadoIdsHistoricoProactiva != string.Empty)
                    {
                        string strIdsHistoricoProactiva = (String)HttpContext.Current.Session["objIdsHistoricoProactiva"];
                        objLog.CrearArchivolog("[PROY-140579][BWEvaluaReglas][strIdsHistoricoProactiva] -> " + Funciones.CheckStr(strIdsHistoricoProactiva), null, null);

                        if (Funciones.CheckStr(strIdsHistoricoProactiva) == string.Empty)
                        {
                            HttpContext.Current.Session["objIdsHistoricoProactiva"] = concatenadoIdsHistoricoProactiva;
                        }
                        else
                        {
                            string strIdsHistoricoProactiva2 = (String)HttpContext.Current.Session["objIdsHistoricoProactiva"];
                            HttpContext.Current.Session["objIdsHistoricoProactiva"] = strIdsHistoricoProactiva2 + "," + concatenadoIdsHistoricoProactiva;
                            strIdsHistoricoProactiva2 = (String)HttpContext.Current.Session["objIdsHistoricoProactiva"];
                        }

                        string strMostrarSessionLog = (String)HttpContext.Current.Session["objIdsHistoricoProactiva"];
                        objLog.CrearArchivolog("[PROY-140579][BWEvaluaReglas][SESSION IDS HISTORICO PROACTIVA] ->  " + Funciones.CheckStr(strMostrarSessionLog), null, null);
                    }
                    else
                    {
                        objLog.CrearArchivolog("[PROY-140579][BWEvaluaReglas][NO SE OBTUVIERON LOS IDS GENERADOS HISTORICO PROACTIVA]", null, null);
                    }
                }
                //PROY-140579 F2 FIN

                return objPlanProactivo;

            }
            catch (FaultException ex)
            {
                objLog.CrearArchivolog("[ERRORFaul][BWobtenerEvaluacionProactiva]" + ex.Message, null, null);
                    return null;
            }              
        }
    }



}
