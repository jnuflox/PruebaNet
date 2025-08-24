using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using Claro.SISACT.Business;
using Claro.SISACT.Common;
using Claro.SISACT.Entity;
using Claro.SISACT.WS;
using Claro.SISACT.WS.WebReferenceEvaluarReglas;
using System.Web;
namespace Claro.SISACT.WS
{
    public class BLEvaluaReglas
    {
        string consTipoProductoDTH = ConfigurationManager.AppSettings["consTipoProductoDTH"].ToString();
        string consTipoDocRUC = ConfigurationManager.AppSettings["TipoDocumentoRUC"].ToString();
        string consFormaPagoCuota = ConfigurationManager.AppSettings["constFormaPagoCuota"].ToString();
        string consFormaPagoContado = ConfigurationManager.AppSettings["constFormaPagoContado"].ToString();
        string consTipoProductoHFC = ConfigurationManager.AppSettings["consTipoProducto3Play"].ToString();
        //string consPopupEvaluacion = ConfigurationManager.AppSettings["constPopupEvaluacionProac"].ToString();
        string nroDocumento = string.Empty;
        GeneradorLog _objLog = null;

        //INICIATIVA 920 INI
        string consFormaPagoChip = ConfigurationManager.AppSettings["constFormaPagoChip"].ToString();
        string consFormaPagoContratoCode = ConfigurationManager.AppSettings["constFormaPagoContratoCode"].ToString();

        public BEPlanProactivo obtenerEvaluacionProactiva(BEClienteCuenta objCliente, BEPlanProactivo objBEPlanProactivo, List<BEDireccionCliente> objDireccion, string strDatosGeneral, string strPlanesDetalle, string strServiciosDetalle,
                                            string strEquiposDetalle, string strFlgCuota, string strTieneProteccionMovil, string strUsuario, WS.WSReglasCrediticia.ClaroEvalClientesReglasRequest oEvaluacionCliente,BECuota objCuotaOAC, BECuota objCuotaPVU)//PROY-31948
        {
            obtenerEvaluacionProactivaRequestType _requestEval = new obtenerEvaluacionProactivaRequestType();
            obtenerEvaluacionProactivaResponseType _responseEval = new obtenerEvaluacionProactivaResponseType();
            HeaderRequestType _requestHeader = new HeaderRequestType();

            _requestHeader.canal = ConfigurationManager.AppSettings["constNombreAplicacion"].ToString();
            _requestHeader.idAplicacion = ConfigurationManager.AppSettings["CodigoAplicacion"].ToString();
            _requestHeader.usuarioAplicacion = ConfigurationManager.AppSettings["constNombreAplicacion"].ToString();
            _requestHeader.usuarioSesion = strUsuario;

            Solicitud solicitud = new Solicitud();
            Cliente cliente = new Cliente();
            Oferta oferta = new Oferta();
            DatosPlanType datosPlan = new DatosPlanType();
            PuntoDeVenta puntoDeVenta = new PuntoDeVenta();
            Direccion direccion = new Direccion();
            Documento documento = new Documento();
            Campana campana = new Campana();
            PlanSolicitado planSolicitado = new PlanSolicitado();
            BLEvaluacion objEvaluacion = new BLEvaluacion();
            List<BEOfertaBRMS> objListaOfertaEvaluado = new List<BEOfertaBRMS>();
            BEPlanProactivo objBSSPlanProactivo = new BEPlanProactivo();

            _objLog = new GeneradorLog(null, null, null, "WEB");

            string strCodOficina = objCliente.oficina;
            string strCodTipoDoc = objCliente.tipoDoc;
            string strNroDocumento = objCliente.nroDoc;
            string strNroOperacion = objCliente.nroOperacionBuro;
            this.nroDocumento = strNroDocumento;

            string[] arrDatosGeneral = strDatosGeneral.Split('|');
            string strTipoOperacion = arrDatosGeneral[0];
            string strOferta = arrDatosGeneral[1];
            string strCasoEspecial = arrDatosGeneral[2];
            string strTipoModalidad = arrDatosGeneral[3].ToUpper();
            string strOperadorCedente = arrDatosGeneral[4].ToUpper();
            string strModalidadVenta = arrDatosGeneral[5].ToUpper();
            string strCombo = arrDatosGeneral[6];
            string strComboTexto = arrDatosGeneral[7].ToUpper();
            string strCodMaterial = string.Empty;

            //EMMH I
            List<BEParametro> lstBEEvaluacionProactiva = new List<BEParametro>();//Proy-36928
            string strCodGrupoParamEvaluacionProactiva = Funciones.CheckStr(ConfigurationManager.AppSettings["consCodGrupoParamEvaluacionProactiva"]);
            if (!string.IsNullOrEmpty(strCodGrupoParamEvaluacionProactiva))
                lstBEEvaluacionProactiva = (new BLGeneral()).ListaParametrosGrupo(Funciones.CheckInt64(strCodGrupoParamEvaluacionProactiva));
            if (lstBEEvaluacionProactiva.Count > 0) lstBEEvaluacionProactiva = lstBEEvaluacionProactiva.OrderBy(p => p.Valor1).ToList();
            //EMMH F

            if (objBEPlanProactivo.idmodventa != ConfigurationManager.AppSettings["constCodModalidadChipSuelto"])
            {
                string[] arrEquipos = strEquiposDetalle.Split('|');
                string[] arrEquipoCod = arrEquipos[0].Split(';');
                strCodMaterial = arrEquipoCod[2].ToString();
            }

            _requestEval.origen = ConfigurationManager.AppSettings["constNombreAplicacion"].ToString();

            // Detalle Planes Evaluados
            List<BEOfertaBRMS> objListaOferta = ObtenerDetalleEvaluacion(strPlanesDetalle, strServiciosDetalle, strEquiposDetalle);

            // Productos de Planes Evaluados
            List<BEPlanBilletera> objProductoPlanesEval = ObtenerProductosPlanesEval(objListaOferta);

            List<BEPlanBilletera> objBilleteraPlanesActivo = null;

            // Nro Planes Activos
            if (objCliente.oPlanesActivosxBilletera != null)
            {
                objBilleteraPlanesActivo = new List<BEPlanBilletera>(objCliente.oPlanesActivosxBilletera);
                if (objBilleteraPlanesActivo != null && !string.IsNullOrEmpty(strCasoEspecial))
                {
                    List<BEItemGenerico> objListaPlanes = objEvaluacion.ObtenerPlanesBSCSxCE(strCasoEspecial);
                    if (objListaPlanes != null)
                    {
                        objBilleteraPlanesActivo.Clear();
                        foreach (BEItemGenerico obj in objListaPlanes)
                        {
                            foreach (BEPlanBilletera objPlan in objCliente.oPlanesActivosxBilletera)
                            {
                                if (objPlan.tipoFacturador == BEPlanBilletera.TIPO_FACTURADOR.BSCS && objPlan.plan == obj.Codigo)
                                    objBilleteraPlanesActivo.Add(objPlan);
                            }
                        }
                    }
                }
            }

            // Modalidad de Venta
            //INCIATIVA 920
            if (strModalidadVenta == consFormaPagoChip || strModalidadVenta == consFormaPagoContratoCode)
                strModalidadVenta = consFormaPagoContado;

            DataSet dsDatosEvaluacionCliente = (new BLEvaluacion()).ObtenerDatosEvaluacion(strCodOficina, strCodTipoDoc, strNroDocumento, strNroOperacion);
            if (dsDatosEvaluacionCliente != null)
            {
                DataTable dtOficina = dsDatosEvaluacionCliente.Tables[0];
                DataTable dtCliente = dsDatosEvaluacionCliente.Tables[1];
                DataTable dtRepresentante = dsDatosEvaluacionCliente.Tables[2];

                Direccion direccionOficina = new Direccion();
                RepresentanteLegal oRRLL = new RepresentanteLegal();
                RepresentanteLegal[] oListaRRLL = new RepresentanteLegal[dtRepresentante.Rows.Count];
                Documento oDocumentoRRLL = new Documento();

                // Información Punto de Venta
                puntoDeVenta.canal = Funciones.CheckStr(dtOficina.Rows[0]["CANAL"]);
                puntoDeVenta.codigo = Funciones.CheckStr(dtOficina.Rows[0]["CODIGO"]);
                puntoDeVenta.descripcion = Funciones.CheckStr(dtOficina.Rows[0]["PDV"]);

                // Información Direccion Punto de Venta
                direccionOficina.codigoPlano = string.Empty;
                direccionOficina.departamento = Funciones.CheckStr(dtOficina.Rows[0]["DEPARTAMENTO"]);
                direccionOficina.distrito = Funciones.CheckStr(dtOficina.Rows[0]["DISTRITO"]);
                direccionOficina.provincia = Funciones.CheckStr(dtOficina.Rows[0]["PROVINCIA"]);
                direccionOficina.region = Funciones.CheckStr(dtOficina.Rows[0]["REGION"]);
                puntoDeVenta.direccion = direccionOficina;

                // Información Cliente
                documento.descripcion = Funciones.CheckStr(ObtenerTipoDocumento(strCodTipoDoc, strNroDocumento));
                documento.numero = Funciones.CheckStr(dtCliente.Rows[0]["NRO_DOCUMENTO"]);
                cliente.documento = documento;
                cliente.edad = Funciones.CheckStr(dtCliente.Rows[0]["EDAD"]);
                cliente.riesgo = Funciones.CheckStr(dtCliente.Rows[0]["RIESGO"]);
                cliente.sexo = Funciones.CheckStr(dtCliente.Rows[0]["SEXO"]);

                // Información Lista de Representantes Legales
                if (strCodTipoDoc == consTipoDocRUC)
                {
                    /*PROY - 30748 F2 MDE / PROY-32438 INI*/
                    int CantidadMesesRRLL;
                    string cargoRR, fechnomRR;
                    var flagApagado = HttpContext.Current.Session["FlagApagado32438"] ?? ""; //PROY-32438 "Flag Apagado/Encendido"
                    /*PROY - 30748 F2 MDE / PROY-32438 FIN*/
                    int idx = 0;
                    foreach (DataRow dr in dtRepresentante.Rows)
                    {
                        List<BERepresentanteLegal> beRep = new List<BERepresentanteLegal>();/*PROY - 30748 F2 MDE / PROY-32438*/
                        oDocumentoRRLL.numero = Funciones.CheckStr(dr["NUMERO_DOCUMENTO"]);
                        oDocumentoRRLL.descripcion = Funciones.CheckStr(ObtenerTipoDocumento(dr["TIPO_DOCUMENTO"].ToString(), dr["NUMERO_DOCUMENTO"].ToString()));
                        oRRLL.documento = oDocumentoRRLL;
                        oRRLL.riesgo = Funciones.CheckStr(dr["RIESGO"]);

                        /*PROY - 30748 F2 MDE / PROY-32438 INI*/
                        beRep = objCliente.oRepresentanteLegal.Where(w => w.APODV_NUM_DOC_REP == oDocumentoRRLL.numero).ToList();
                        CantidadMesesRRLL = Convert.ToInt32(beRep[0].APODI_CANT_MESES_NOMBRAMIENTO);
                        cargoRR = beRep[0].APODV_CAR_REP;
                        fechnomRR = beRep[0].APODD_FECHA_NOMBRAMIENTO;

                        //PROY-32438 "Flag Apagado/Encendido" INI
                        if (flagApagado.ToString().Equals("0"))
                        {
                            oRRLL.cantidadMesesNombramiento = CantidadMesesRRLL;
                            oRRLL.cargo = cargoRR;
                            if (fechnomRR!=null) { oRRLL.fechaNombramiento = DateTime.Parse(fechnomRR); }
                        }
                        //PROY-32438 "Flag Apagado/Encendido" FIN

                        /*PROY - 30748 F2 MDE / PROY-32438 FIN*/

                        oListaRRLL[idx] = new RepresentanteLegal();
                        oListaRRLL[idx] = oRRLL;
                        idx++;
                    }

                    Contribuyente objContribuyente = new Contribuyente();/*PROY - 30748 F2 MDE / PROY-32438*/
                    Contribuyente[] lobjContribuyente = new Contribuyente[1];
                    cliente.representanteLega = oListaRRLL;

                    /*PROY - 30748 F2 MDE / PROY-32438 INI*/
                    if (flagApagado.ToString().Equals("0")) //PROY-32438 "Flag Apagado/Encendido" INI
                    {
                        objContribuyente.tipo = objCliente.TipContribuyente;
                        objContribuyente.nombreComercial = objCliente.NomComercial;
                        objContribuyente.fechaInicioActividades = Convert.ToDateTime(objCliente.FecIniActividades);
                        objContribuyente.estado = objCliente.EstContribuyente;
                        objContribuyente.condicion = objCliente.CondContribuyente;
                        objContribuyente.CIU = objCliente.CiiuContribuyente;
                        objContribuyente.cantidadTrabajadores = Convert.ToInt16(Funciones.CheckInt16(objCliente.CantTrabajadores));
                        if (objCliente.EmisionComp != null)
                        {
                            objContribuyente.autorizacionImpresion = objCliente.EmisionComp;
                        } // 32438
                        objContribuyente.sistemaEmisionElectronica = objCliente.SistEmielectronica;
                        objContribuyente.cantidadMesesInicioActividades = objCliente.CantMesIniActividades;
                        lobjContribuyente[0] = objContribuyente;
                        cliente.contribuyentes = lobjContribuyente;
                    } //PROY-32438 "Flag Apagado/Encendido" FIN
                    /*PROY - 30748 F2 MDE / PROY-32438 FIN*/
                }
                // Información Solicitud
                solicitud.flagDeLicitacion = "NO";
                solicitud.tipoDeBolsa = string.Empty;
                solicitud.puntoDeVenta = puntoDeVenta;
            }
            //PROY-30748 F2 MDE INI
            double dblCFEvaluadoTotal = 0;
            List<BEOfertaBRMS> objListaOfertaBRMS = new List<BEOfertaBRMS>();
            objListaOfertaBRMS = (List<BEOfertaBRMS>)HttpContext.Current.Session["objListaOfertaEvaluadoProa"];
            for (int i = 0; i < objListaOfertaBRMS.Count() - 1; i++)
            {
                dblCFEvaluadoTotal += objListaOfertaBRMS[i].cargoFijo;
            }

            //PROY-30748 F2 MDE FIN 
         
            foreach (BEOfertaBRMS objOferta in objListaOferta)
            {
                string idPlan = objOferta.idPlan;

                // Modalidad / Operador Cedente
                oferta.operadorCedente = strOperadorCedente;
                oferta.modalidadCedente = strTipoModalidad;

                oferta.campana = new Campana();
                oferta.campana.tipo_campana = objOferta.campana;
                oferta.casoEpecial = (string.IsNullOrEmpty(strCasoEspecial)) ? "REGULAR" : strCasoEspecial;
                oferta.controlDeConsumo = String.Empty;//PROY 30748 F2 MDE                              
                oferta.kitDeInstalacion = String.Empty;

                // LISTA EQUIPOS
                List<BEEquipoBRMS> objListaEquipo = objOferta.oEquipo;
                if (objListaEquipo != null)
                {
                    int idx = 0;

                    Equipo[] oListaEquipo = new Equipo[objListaEquipo.Count];
                    foreach (BEEquipoBRMS oPlanEquipo in objListaEquipo)
                    {
                        oListaEquipo[idx] = new Equipo();
                        oListaEquipo[idx].cuotas = new Cuotas();
                        oListaEquipo[idx].costo_equipo = oPlanEquipo.costo;
                        oListaEquipo[idx].cuotas.numeroCuotas = Funciones.CheckStr(oPlanEquipo.cantidadDeCuotas);
                        oListaEquipo[idx].formaDePago = strModalidadVenta;
                        oListaEquipo[idx].gama = oPlanEquipo.gama;
                        oListaEquipo[idx].modelo = oPlanEquipo.modelo;
                        oListaEquipo[idx].cuotas.montoDeCuota = Funciones.CheckDecimal(oPlanEquipo.montoDeCuota);
                        oListaEquipo[idx].cuotas.porcentajecuotaInicial = oPlanEquipo.porcentajeCuotaInicial; //PROY 30748 F2 SAMA
                        oListaEquipo[idx].precioDeVenta = Funciones.CheckDecimal(oPlanEquipo.precioDeVenta);
                        oListaEquipo[idx].tipodeDeco = oPlanEquipo.tipoDeDeco;
                        oListaEquipo[idx].tipoOperacionKit = oPlanEquipo.tipoDeOperacionKit;
                        idx++;
                    }
                    solicitud.equipo = oListaEquipo;

                }
                if (solicitud.equipo.Length == 0)
                {
                    string[] arrEquipos = strEquiposDetalle.Split(';');
                    Equipo[] oListaEquipo = new Equipo[1];

                    oListaEquipo[0] = new Equipo();
                    oListaEquipo[0].costo_equipo = 0.00;
                    oListaEquipo[0].modelo = arrEquipos[3];

                    oListaEquipo[0].cuotas = new Cuotas();
                    oListaEquipo[0].cuotas.numeroCuotas = Funciones.CheckStr(objBEPlanProactivo.NroCuota);
                    solicitud.equipo = oListaEquipo;
                }

                //DIRECCION CLIENTE
                if (objDireccion != null)
                {
                    foreach (BEDireccionCliente oDirCliente in objDireccion)
                    {
                        string departamento = null, provincia = null, distrito = null;
                        (new BLGeneral()).ConsultarDatosDireccion(oDirCliente.IdDepartamento, oDirCliente.IdProvincia, oDirCliente.IdDistrito,
                                                                  ref departamento, ref provincia, ref distrito);
                        direccion.codigoPlano = oDirCliente.IdPlano;
                        direccion.departamento = departamento;
                        direccion.distrito = departamento;
                        direccion.provincia = provincia;
                        direccion.region = string.Empty;
                        cliente.direccion = direccion;
                        break;
                    }
                }

                oferta.planSolicitado = new PlanSolicitado();
                oferta.planSolicitado.cargoFijo = objOferta.cargoFijo;
                oferta.planSolicitado.descripcion = objOferta.plan;
                oferta.plazoDeAcuerdo = objOferta.plazo;
                oferta.productoComercial = objOferta.producto;
                oferta.proteccionMovil = strTieneProteccionMovil;
                oferta.segmentoDeOferta = strOferta;
                oferta.tipoDeOperacionEmpresa = String.Empty;
                oferta.combo = strComboTexto;
                oferta.mesOperadorCedente = objOferta.cantidadMesesOperadorCedente;
                //gaa20170215
                oferta.cantidadLineasSEC = objOferta.cantidadLineasSEC;
                oferta.montoCFSEC = objOferta.montoCFSEC;

                Campana campania = new Campana();

                string[] arrPlanesDetalle = strPlanesDetalle.Split(';');
                campania.tipo_campana = arrPlanesDetalle[9];
                oferta.campana = campania;

                // Productos Plan [Tipos de Productos que componen al Plan]
                List<BEBilletera> objBilleteraPlan = objProductoPlanesEval.Find(delegate(BEPlanBilletera obj) { return obj.plan == idPlan; }).oBilletera;
                // Nro Planes = Sumatoria Planes [Productos que componen al Plan]
                int intPlanesActivo = CalcularNroPlanesActivoxProducto(objBilleteraPlanesActivo, objBilleteraPlan);
                objListaOfertaEvaluado = (List<BEOfertaBRMS>)HttpContext.Current.Session["objListaOfertaEvaluadoProa"];//PROY 30748 F2 MDE
                int intPlanesEvaluado = CalcularNroPlanesxProducto(objListaOfertaEvaluado, objBilleteraPlan);
                int intPlanesTotal = intPlanesActivo + intPlanesEvaluado;

                _objLog.CrearArchivolog("[xml][intPlanesActivo]" + intPlanesActivo.ToString(), null, null);
                _objLog.CrearArchivolog("[xml][intPlanesEvaluado]" + intPlanesEvaluado.ToString(), null, null);
                _objLog.CrearArchivolog("[xml][intPlanesTotal]" + intPlanesTotal.ToString(), null, null);

                // LC Disponible Plan = Sumatoria [LC - CF] [Productos que componen al Plan]
                double dblLCDisponible = CalcularMontoxProducto(objCliente.oLCDisponiblexBilletera, objBilleteraPlan);
                double dblCFEvaluado = CalcularCFEvaluado(objListaOfertaEvaluado, objBilleteraPlan);
                double dblCF = dblCFEvaluado + objOferta.cargoFijo;
                // Monto Facturado [Productos que componen al Plan]
                double dblMontoFacturado = CalcularMontoxProducto(objCliente.oMontoFacturadoxBilletera, objBilleteraPlan);
                double dblMontoNoFacturado = CalcularMontoxProducto(objCliente.oMontoNoFacturadoxBilletera, objBilleteraPlan);


                cliente.cantidadDeLineasActivas = objCliente.nroPlanesActivos;
                cliente.comportamientoDePago = objCliente.comportamientoPago;
                cliente.facturacionPromedioClaro = Funciones.CheckDbl(objCliente.montoFacturadoTotal + objCliente.montoNoFacturadoTotal, 1);//PROY-140579 FASE 2 REDONDEAR
                cliente.facturacionPromedioProducto = Funciones.CheckDbl(dblMontoFacturado + dblMontoNoFacturado, 2);
                cliente.limiteDeCreditoDisponible = (dblLCDisponible - dblCFEvaluado > 0) ? (dblLCDisponible - dblCFEvaluado) : 0;
                cliente.limiteDeCreditoDisponible = Funciones.CheckDbl(cliente.limiteDeCreditoDisponible, 2);
                cliente.tiempoDePermanencia = objCliente.tiempoPermanencia;
                cliente.tipoCliente = objCliente.tipoCliente;
                cliente.deuda = objCliente.Deuda;
                cliente.adicional = new Adicional();
                cliente.adicional.precio = Funciones.CheckStr(objBEPlanProactivo.CFServAdic);

                //PROY 30748 F2 MDE / PROY-32439 MAS INI
                ValidacionDeudaBRMS objVariablesEntradaNVoBRMS = new ValidacionDeudaBRMS();
                objVariablesEntradaNVoBRMS = (ValidacionDeudaBRMS)HttpContext.Current.Session["ObjNvoBRMS"];
                cliente.montoDeudaVencida = objVariablesEntradaNVoBRMS.request.cliente.montoDeudaVencida;
                cliente.montoDeudaCastigada = objVariablesEntradaNVoBRMS.request.cliente.montoDeudaCastigada;
                cliente.montoDisputa = objVariablesEntradaNVoBRMS.request.cliente.disputa.monto;
                cliente.cantidadMontoDisputa = objVariablesEntradaNVoBRMS.request.cliente.disputa.cantidad;
                cliente.antiguedadMontoDisputa = objVariablesEntradaNVoBRMS.request.cliente.disputa.antiguedad;
                cliente.montoTotalDeuda = objVariablesEntradaNVoBRMS.request.cliente.montoDeuda;
                cliente.antiguedadDeuda = objVariablesEntradaNVoBRMS.request.cliente.antiguedadDeuda;
                //PROY 30748 F2 MDE / PROY-32439 MAS FIN

                cliente.segmento = Funciones.CheckStr(HttpContext.Current.Session["strClienteSegmento"]);//PROY 30748 F2 MDE / PROY-140230-MAS

                cliente.flagWhitelist = Funciones.CheckStr((String)HttpContext.Current.Session["SessionIsWhiteList"]);//PROY-140579 F2

                //PROY-31948 INI

                _objLog.CrearArchivolog("[PROY-31948 - INICIO consultarCuotaClienteOAC]", null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-31948 - montoPendienteCuotasSistema]", objCuotaOAC.montoPendienteCuotasSistema), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-31948 - cantidadPlanesCuotasPendientesSistema]", objCuotaOAC.cantidadPlanesCuotasPendientesSistema), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-31948 - cantidadMaximaCuotasPendientesSistema]", objCuotaOAC.cantidadMaximaCuotasPendientesSistema), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-31948 - cantidadCuotasPendientes]", objCuotaOAC.cantidadCuotasPendientes), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-31948 - montoPendienteCuotas]", objCuotaOAC.montoPendienteCuotas), null, null);
                _objLog.CrearArchivolog("[PROY-31948 - FIN consultarCuotaClienteOAC]", null, null);

                cliente.montoPendienteCuotasSistema = objCuotaOAC.montoPendienteCuotasSistema;
                cliente.cantidadPlanesCuotasPendientesSistema = objCuotaOAC.cantidadPlanesCuotasPendientesSistema;
                cliente.cantidadMaximaCuotasPendientesSistema = objCuotaOAC.cantidadMaximaCuotasPendientesSistema;

                /*PROY 30748 F2 MDE INI*/
                PlanActual oPlanActual = new PlanActual();
                PlanActual[] loPlanActual = new PlanActual[1];
                oPlanActual.cantidadCuotasPendientes = objCuotaOAC.cantidadCuotasPendientes;
                oPlanActual.montoPendienteCuotas = objCuotaOAC.montoPendienteCuotas;
                loPlanActual[0] = oPlanActual;
                oferta.planActual = loPlanActual;
                /*PROY 30748 F2 MDE FIN*/

                _objLog.CrearArchivolog("[PROY-31948 - INICIO ConsultaCuotasPendientesPVU]", null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-31948 - montoPendienteCuotasUltimasVentas]", objCuotaPVU.montoPendienteCuotasUltimasVentas), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-31948 - cantidadPlanesCuotasPendientesUltimasVentas]", objCuotaPVU.cantidadPlanesCuotasPendientesUltimasVentas), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-31948 - cantidadMaximaCuotasPendientesUltimasVentas]", objCuotaPVU.cantidadMaximaCuotasPendientesUltimasVentas), null, null);
                _objLog.CrearArchivolog("[PROY-31948 - FIN ConsultaCuotasPendientesPVU]", null, null);

                cliente.montoPendienteCuotasUltimasVentas = objCuotaPVU.montoPendienteCuotasUltimasVentas;
                cliente.cantidadPlanesCuotasPendientesUltimasVentas = objCuotaPVU.cantidadPlanesCuotasPendientesUltimasVentas;
                cliente.cantidadMaximaCuotasPendientesUltimasVentas = objCuotaPVU.cantidadMaximaCuotasPendientesUltimasVentas;
                //PROY-31948 FIN

                solicitud.cliente = cliente;
                solicitud.tipoDeOperacion = strTipoOperacion;
                solicitud.totalPlanes = Funciones.CheckStr(objCliente.totalplanes);
                solicitud.fechaEjecucion = DateTime.Now.Date;
                solicitud.horaEjecucion = DateTime.Now.Hour;
                solicitud.oferta = oferta;

                solicitud.transaccion = ConfigurationManager.AppSettings["constTrxEvaluacion"].ToString();
                
                //INICIATIVA 920
                if (objBEPlanProactivo.idmodventa == ConfigurationManager.AppSettings["constCodModalidadCuota"]
                    || objBEPlanProactivo.idmodventa == ConfigurationManager.AppSettings["constCodModalidadCuotaSinCode"])
                    solicitud.transaccion = ConfigurationManager.AppSettings["constTrxVentaCuotas"].ToString();

                if (objBEPlanProactivo.idmodventa == ConfigurationManager.AppSettings["constCodModalidadChipSuelto"])
                    //EMMH I
                    //datosPlan.modalidad_venta = ConfigurationManager.AppSettings["constDescEvalProacChipSuelto"];
                    datosPlan.modalidad_venta = lstBEEvaluacionProactiva.Where(p => p.Valor1 == "constDescEvalProacChipSuelto").SingleOrDefault().Valor;
                //EMMH F
                else if (objBEPlanProactivo.idmodventa == ConfigurationManager.AppSettings["constCodModalidadContrato"])
                    //EMMH I
                    datosPlan.modalidad_venta = lstBEEvaluacionProactiva.Where(p => p.Valor1 == "constDescEvalProacContratoCode").SingleOrDefault().Valor;
                    //datosPlan.modalidad_venta = ConfigurationManager.AppSettings["constDescEvalProacContratoCode"];
                //EMMH F
                //INICIATIVA 920 INICIO
                else if (objBEPlanProactivo.idmodventa == ConfigurationManager.AppSettings["constCodModalidadContratoSinCode"])
                    //EMMH I
                    datosPlan.modalidad_venta = lstBEEvaluacionProactiva.Where(p => p.Valor1 == "constDescEvalProacContratoSinCode").SingleOrDefault().Valor;
                //EMMH F
                else if (objBEPlanProactivo.idmodventa == ConfigurationManager.AppSettings["constCodModalidadCuotaSinCode"])
                    //EMMH I
                    datosPlan.modalidad_venta = lstBEEvaluacionProactiva.Where(p => p.Valor1 == "constDescEvalProacCuotasSinCode").SingleOrDefault().Valor;
                //INICIATIVA 920 FIN
                else
                    //EMMH I
                    datosPlan.modalidad_venta = lstBEEvaluacionProactiva.Where(p => p.Valor1 == "constDescEvalProacCuotas").SingleOrDefault().Valor;
                    //datosPlan.modalidad_venta = ConfigurationManager.AppSettings["constDescEvalProacCuotas"];
                    //EMMH F

                datosPlan.tipo_flujo = objBEPlanProactivo.idFLujo;
                datosPlan.tipo_documento = objCliente.tipoDoc;// "01"; //doc objCliente
                //EMMH I
                datosPlan.tipo_oferta = lstBEEvaluacionProactiva.Where(p => p.Valor1 == "Const_ProaOfertaMasivo").SingleOrDefault().Valor;//MASIVO  
                //datosPlan.tipo_oferta = ConfigurationManager.AppSettings["Const_ProaOfertaMasivo"]; ;//MASIVO  
                //EMMH F
                datosPlan.tipo_operacion = objBEPlanProactivo.idtipoOper;
                datosPlan.tipo_producto = objOferta.idProducto;
                datosPlan.campana = objOferta.idCampana;
                datosPlan.codigoMaterial = strCodMaterial;
                datosPlan.codigoOperacion = Funciones.CheckDecimal(objBEPlanProactivo.idtipoOper);
                datosPlan.codigoplazo = objOferta.idPlazo;
                datosPlan.familia = objBEPlanProactivo.familia;
                datosPlan.tipo_venta = objBEPlanProactivo.idmodventa;
                datosPlan.oficina = objCliente.oficina;
                datosPlan.servicioAdiResq = objBEPlanProactivo.ServAdic;



                solicitud.cargoFijo = new l_cargoFijo[objBEPlanProactivo.Planes.Count];
                int cont = 0;
                foreach (var item in objBEPlanProactivo.Planes)
                {
                    string strFiltro = string.Empty;

                    l_cargoFijo lcargoFijo = new l_cargoFijo()
                     {
                         descripcion = item.PLANV_DESCRIPCION,
                     };
                    solicitud.cargoFijo[cont] = lcargoFijo;
                    cont = cont + 1;
                }

                //PROY-140736 INI
                var index = 0;
                int longitud = 0;
                RequestOpcionalTypeRequestOpcional1[] objRequestOptional = null;
                if (objBEPlanProactivo.idFLujo == "2")
                {
                    longitud = 1;
                }

                var Campania = HttpContext.Current.Session["ExisteBuyback"].ToString();
                if (Campania == "SI")
                {
                    longitud = 3;
                }
                if (longitud > 0)
                {
                    objRequestOptional = new RequestOpcionalTypeRequestOpcional1[longitud];
                }

                //PROY-140736 FIN

                //PROY-140335 INICIO EJRC
                if (objBEPlanProactivo.idFLujo == "2")
                {
                      index = 1;
                     _objLog.CrearArchivolog("[PROY-140335 RF1(BLEvalugaReglas) - INICIO ENVIO DE FLAG CONSULTA PREVIA PROACTIVO]", null, null);
                   
                    var flagEnvioCPProactivo = HttpContext.Current.Session["flagEnvioCPProactivo"].ToString();
                    objRequestOptional[0] = new RequestOpcionalTypeRequestOpcional1();
                    objRequestOptional[0].campo = "flagConsultaPrevia";
                    objRequestOptional[0].valor = flagEnvioCPProactivo;
                    solicitud.listaResquestOpcional = objRequestOptional;
            


                    _objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-140335 RF1 - flagConsultaPrevia-PROACTIVO]-->", flagEnvioCPProactivo), null, null);
                    _objLog.CrearArchivolog("[PROY-140335 RF1(BLEvalugaReglas) - FIN ENVIO DE FLAG CONSULTA PREVIA PROACTIVO]", null, null);

                  


                }

                //PROY-140736 INI
                if (Campania == "SI")
                {
                    _objLog.CrearArchivolog("[PROY-140736(BLEvalugaReglas) - INICIO DE ENVIO AL REQUEST OPCIONAL]", null, null);
                    _objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-140736  - Es campaña Buyback]-->", Campania), null, null);
                    var materialCanje = HttpContext.Current.Session["materialCanje"].ToString();
                    _objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-140736  - materialCanje]-->", materialCanje), null, null);
                    _objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-140736  -INI Creación de Request Opcional Buyback]-->", ""), null, null);
                    objRequestOptional[index] = new RequestOpcionalTypeRequestOpcional1();
                    objRequestOptional[index].campo = "materialCanje";
                    objRequestOptional[index].valor = materialCanje;
                    index++;
                    objRequestOptional[index] = new RequestOpcionalTypeRequestOpcional1();
                    objRequestOptional[index].campo = "flagByBack";
                    objRequestOptional[index].valor = "SI";
                    solicitud.listaResquestOpcional = objRequestOptional;
                    HttpContext.Current.Session["materialCanje"] = string.Empty;
                    _objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-140736  -FIN Creación de Request Opcional Buyback]-->", solicitud.listaResquestOpcional), null, null);
              
                   
                }
                //PROY-140736 FIN


                //PROY-140335 FIN EJRC

                _requestEval.solicitud = solicitud;
                _requestEval.datosPlan = datosPlan;

                //_requestEval.solicitud.oferta.planActual


                _requestEval.solicitud.tipoDeDespacho = objBEPlanProactivo.PuntodeDespacho;
                _requestEval.solicitud.cliente.creditscore = objBEPlanProactivo.creditScore;
                _requestEval.solicitud.cliente.cantidadDePlanesPorProducto = objBEPlanProactivo.cantidadDePlanesPorProducto;

                BEItemMensaje objMensaje = new BEItemMensaje();
                
                objBSSPlanProactivo = (new WS.BWEvaluaReglas()).obtenerEvaluacionProactiva(_requestHeader, _requestEval, ref objMensaje, oEvaluacionCliente);
                

                foreach (var item in objBSSPlanProactivo.PlanBSSEval)
                {
                    if (strCodTipoDoc == consTipoDocRUC)
                        item.TieneAutonomia = ResultadoAutonomia(item, strCodTipoDoc, intPlanesEvaluado, (item.cargoFijo + dblCFEvaluadoTotal), dblMontoFacturado);//PROY 30748 F2 MDE
                    else
                        item.TieneAutonomia = ResultadoAutonomia(item, strCodTipoDoc, intPlanesTotal, (item.cargoFijo + dblCFEvaluadoTotal), dblMontoFacturado);//PROY 30748 F2 MDE
                }
            }
            return objBSSPlanProactivo;
        }

        private double CalcularMontoxProducto(List<BEBilletera> objLista, List<BEBilletera> objBilleteraActual)
        {
            double dblMonto = 0.0;
            if (objLista != null)
            {
                foreach (BEBilletera obj in objLista)
                {
                    foreach (BEBilletera obj1 in objBilleteraActual)
                    {
                        if (obj.idBilletera == obj1.idBilletera)
                        {
                            dblMonto += obj.monto;
                            break;
                        }
                    }
                }
            }
            return dblMonto;
        }

        private WS.WSReglasCrediticia.tipoDeDocumento ObtenerTipoDocumento(string strTipoDoc, string strNroDoc)
        {
            if (strTipoDoc == ConfigurationManager.AppSettings["constCodTipoDocumentoDNI"].ToString())
                return WS.WSReglasCrediticia.tipoDeDocumento.DNI;

            if (strTipoDoc == ConfigurationManager.AppSettings["constCodTipoDocumentoCEX"].ToString())
                return WS.WSReglasCrediticia.tipoDeDocumento.CE;

            if (strTipoDoc == ConfigurationManager.AppSettings["constCodTipoDocumentoRUC"].ToString())
            {
                if (strNroDoc.Substring(0, 1) == "1") return WS.WSReglasCrediticia.tipoDeDocumento.RUC10;
                else return WS.WSReglasCrediticia.tipoDeDocumento.RUC20;
            }
            return WS.WSReglasCrediticia.tipoDeDocumento.DNI;
        }
        public List<BEOfertaBRMS> ObtenerDetalleEvaluacion(string strPlanesDetalle, string strServiciosDetalle, string strEquiposDetalle)
        {
            List<BEOfertaBRMS> objDetalleEval = new List<BEOfertaBRMS>();
            List<BEOfertaBRMS> objPlanDetalle = new List<BEOfertaBRMS>();
            objPlanDetalle = ObtenerDetallePlanesEval(strPlanesDetalle);
            objDetalleEval = ObtenerDetallePlanEquipoEval(objPlanDetalle, strServiciosDetalle, strEquiposDetalle);
            return objDetalleEval;
        }

        private List<BEOfertaBRMS> ObtenerDetallePlanesEval(String strPlanesDetalle)
        {
            List<BEOfertaBRMS> listaPlanDetalle = new List<BEOfertaBRMS>();

            string[] arrPlanDetalle = strPlanesDetalle.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            BEOfertaBRMS objOferta;
            List<BEItemGenerico> objListaPlazo = new BLGeneral().ListarPlazoAcuerdo("");
            List<BEItemGenerico> objListaProducto = new BLGeneral().ListarProducto();
            foreach (string strDetalle in arrPlanDetalle)
            {
                objOferta = new BEOfertaBRMS();
                string[] objPlan = strDetalle.ToUpper().Split(';');
                objOferta.idFila = Funciones.CheckInt(objPlan[0]);
                objOferta.idProducto = objPlan[1];
                // Producto
                foreach (BEItemGenerico producto in objListaProducto)
                {
                    if (producto.Codigo == objOferta.idProducto)
                    {
                        objOferta.producto = producto.Descripcion;
                        break;
                    }
                }
                // Plazo Acuerdo
                objOferta.idPlazo = objPlan[2];
                foreach (BEItemGenerico plazo in objListaPlazo)
                {
                    if (plazo.Codigo == objOferta.idPlazo)
                    {
                        objOferta.plazo = plazo.Descripcion;
                        break;
                    }
                }
                objOferta.idPaquete = objPlan[3].Split('_')[0];
                objOferta.paquete = objPlan[4];
                objOferta.idPlan = objPlan[5].Split('_')[0];
                objOferta.plan = objPlan[6];

                objOferta.topeConsumo = ConfigurationManager.AppSettings["ConstTextSinTopeConsumo"].ToString();
                if (objPlan[7] == ConfigurationManager.AppSettings["constCodTopeCeroServicio"].ToString())
                    objOferta.topeConsumo = "TOPE DE CONSUMO CERO";

                if (objPlan[7] == ConfigurationManager.AppSettings["constCodTopeSinCFServicio"].ToString())
                    objOferta.topeConsumo = "TOPE DE CONSUMO SIN CF";

                if (objPlan[7] == ConfigurationManager.AppSettings["constCodTopeAutomatico"].ToString())
                    objOferta.topeConsumo = "TOPE DE CONSUMO AUTOMATICO";

                if (objOferta.idProducto == consTipoProductoHFC)
                    objOferta.topeConsumo = objPlan[7];

                objOferta.idCampana = objPlan[8];
                objOferta.campana = objPlan[9];
                objOferta.cargoFijo = Funciones.CheckDbl(objPlan[10], 2);
                //gaa20170215
                objOferta.cantidadLineasSEC = Funciones.CheckInt(objPlan[12]);
                objOferta.montoCFSEC = Funciones.CheckDbl(objPlan[11], 2);
                //fin gaa20170215
                objOferta.cantidadMesesOperadorCedente = new BEPorttSolicitud() { fechaActivacionCP = objPlan[13] }.cantidadMesesOperadorCedente;
                listaPlanDetalle.Add(objOferta);
            }
            listaPlanDetalle = listaPlanDetalle.OrderBy(o => o.idFila).ToList();

            return listaPlanDetalle;
        }

        private List<BEOfertaBRMS> ObtenerDetallePlanEquipoEval(List<BEOfertaBRMS> objPlanDetalle, string strServiciosDetalle, string strEquiposDetalle)
        {
            List<BEOfertaBRMS> listaPlanDetalle = new List<BEOfertaBRMS>();

            string idFila = String.Empty;
            string[] arrPlanEquipo = strEquiposDetalle.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            string[] arrPlanServicio = strServiciosDetalle.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            BEEquipoBRMS objEquipo;
            BEItemGenerico objServicio;
            List<BEEquipoBRMS> objListaEquipo;
            List<BEItemGenerico> objListaServicio;

            foreach (BEOfertaBRMS objOferta in objPlanDetalle)
            {
                objListaEquipo = new List<BEEquipoBRMS>();
                foreach (string strEquipo in arrPlanEquipo)
                {
                    idFila = strEquipo.Split(';')[0];
                    if (objOferta.idFila.ToString() == idFila)
                    {
                        Double number;

                        objEquipo = new BEEquipoBRMS();
                        objEquipo.idFila = strEquipo.Split(';')[0];
                        objEquipo.idProducto = strEquipo.Split(';')[1];
                        objEquipo.idEquipo = strEquipo.Split(';')[2];
                        if (Double.TryParse(strEquipo.Split(';')[4], out number))
                        {
                            objEquipo.costo = Funciones.CheckDbl(strEquipo.Split(';')[4], 2);
                        }
                        // objEquipo.costo = Funciones.CheckDbl(strEquipo.Split(';')[4], 2);
                        objEquipo.modelo = strEquipo.Split(';')[3];
                        objEquipo.montoDeCuota = Funciones.CheckDbl(strEquipo.Split(';')[8], 2);
                        objEquipo.precioDeVenta = Funciones.CheckDbl(strEquipo.Split(';')[5], 2);
                        objEquipo.cantidadDeCuotas = Funciones.CheckInt(strEquipo.Split(';')[6]);
                        objEquipo.porcentajeCuotaInicial = Funciones.CheckDbl(strEquipo.Split(';')[7], 2);
                        if (objEquipo.cantidadDeCuotas > 0)
                        {
                            objEquipo.formaDePago = ConfigurationManager.AppSettings["constFormaPagoCuota"].ToString();
                        }
                        if (objEquipo.idProducto == ConfigurationManager.AppSettings["consTipoProductoDTH"].ToString())
                        {
                            objEquipo.tipoDeOperacionKit = ConfigurationManager.AppSettings["constTipoKitDECO"];
                            objEquipo.formaDePago = ConfigurationManager.AppSettings["constFormaPagoComodato"];
                        }
                        objListaEquipo.Add(objEquipo);
                    }
                }
                objOferta.oEquipo = objListaEquipo;

                objListaServicio = new List<BEItemGenerico>();
                foreach (string strServicio in arrPlanServicio)
                {
                    idFila = strServicio.Split(';')[0];
                    if (objOferta.idFila.ToString() == idFila)
                    {
                        string[] arrServicio = strServicio.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                        for (int i = 1; i < arrServicio.Length; i++)
                        {
                            objServicio = new BEItemGenerico();
                            objServicio.Descripcion = arrServicio[i];
                            objListaServicio.Add(objServicio);
                        }
                    }
                }
                objOferta.oServicio = objListaServicio;

                listaPlanDetalle.Add(objOferta);
            }
            return listaPlanDetalle;
        }

        private List<BEPlanBilletera> ObtenerProductosPlanesEval(List<BEOfertaBRMS> objPlanDetalle)
        {
            string strPlanesEvaluados = string.Empty;
            int intSistemaSISACT = (int)BEPlanBilletera.TIPO_SISTEMA.SISACT;
            List<BEPlanBilletera> objProductoPlanesEval = new List<BEPlanBilletera>();
            foreach (BEOfertaBRMS item in objPlanDetalle)
            {
                strPlanesEvaluados = string.Format("{0}|{1};{2}", strPlanesEvaluados, item.idPlan, intSistemaSISACT.ToString());
            }

            objProductoPlanesEval = (new BLEvaluacion()).ObtenerBilleteraxPlan(strPlanesEvaluados);
            return objProductoPlanesEval;
        }
        private double CalcularCFEvaluado(List<BEOfertaBRMS> objListaEvaluado, List<BEBilletera> objBilleteraActual)
        {
            double dblCF = 0.0;
            foreach (BEBilletera obj in objBilleteraActual)
            {
                foreach (BEOfertaBRMS obj1 in objListaEvaluado)
                {
                    foreach (BEBilletera obj2 in obj1.oBilletera)
                    {
                        if (obj.idBilletera == obj2.idBilletera)
                        {
                            dblCF += obj1.cargoFijo;
                            break;
                        }
                    }
                }
            }
            return dblCF;
        }
        private string ResultadoAutonomia(BEPlanBSSEval objPlanProac, string strTipoDoc, int nroPlanes, double dblCF, double dblMontoFacturado)
        {
            string strAutonomia = "N";
            double dblMontoCFPermitido;

            if (Funciones.CheckStr(objPlanProac.restriccion) != "NO")
                strAutonomia = "NO_CONDICION";
            else
            {
                if (strTipoDoc == consTipoDocRUC)
                {
                    if (Funciones.CheckInt(objPlanProac.cantidadDeLineasAdicionalesRUC) >= nroPlanes)
                    {
                        if (objPlanProac.TipoDeAutonomiaCargoFijo == "REFERENCIAL")
                            dblMontoCFPermitido = Funciones.CheckDbl(objPlanProac.montoCFParaRUC * (dblMontoFacturado / 100), 2);
                        else
                            dblMontoCFPermitido = objPlanProac.montoCFParaRUC;

                        if (dblMontoCFPermitido >= dblCF)
                            strAutonomia = "S";
                    }
                }
                else if (Funciones.CheckInt(objPlanProac.cantidadDeLineasMaximas) >= nroPlanes)
                    strAutonomia = "S";
            }

            return strAutonomia;
        }

        private int CalcularNroPlanesxProducto(List<BEOfertaBRMS> objLista, List<BEBilletera> objBilleteraActual)
        {
            int nroPlanes = objBilleteraActual.Count;
            //PROY 30748 F2 INI MDE
            foreach (BEBilletera obj in objBilleteraActual)
            {
                for (int i = 0; i < objLista.Count() - 1; i++ )
                {
                    foreach (BEBilletera obj2 in objLista[i].oBilletera)
                    {
                        if (obj.idBilletera == obj2.idBilletera) nroPlanes++;
                    }
                }
            }
            //PROY 30748 F2 FIN MDE
            return nroPlanes;
        }
        private int CalcularNroPlanesActivoxProducto(List<BEPlanBilletera> objLista, List<BEBilletera> objBilleteraActual)
        {
            int nroPlanes = 0;
            if (objLista != null)
            {
                foreach (BEBilletera obj in objBilleteraActual)
                {
                    foreach (BEPlanBilletera obj1 in objLista)
                    {
                        foreach (BEBilletera obj2 in obj1.oBilletera)
                        {
                            if (obj.idBilletera == obj2.idBilletera)
                                nroPlanes += obj1.nroPlanes;
                        }
                    }
                }
            }
            return nroPlanes;
        }
    }
}
