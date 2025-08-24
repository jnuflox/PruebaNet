using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Claro.SISACT.Business;
using Claro.SISACT.Web.Base;
using Claro.SISACT.Entity;
using System.Text;
using System.Web.UI.HtmlControls;
using Claro.SISACT.Common;
using System.Configuration;
using Claro.SISACT.WS; 
using Claro.SISACT.Web.Comun; //PROY-31948
using Claro.SISACT.Web.Paginas;//PROY-140335 EJRC

namespace Claro.SISACT.Web.Paginas.evaluacion_cons
{
    public partial class sisact_popup_evaluacion_proactiva : Sisact_Webbase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            if (Session["Usuario"] == null)
            {
                string strRutaSite = ConfigurationManager.AppSettings["RutaSite"];
                Response.Redirect(strRutaSite);
                Response.End();
                return;
            }
            GeneradorLog objLog = new GeneradorLog(null, null, null, "WEB");
            objLog.CrearArchivolog("[Inicio][Carga - Evaluacion_Proactiva]", null, null);
            HidColorBloqueo.Value = ConfigurationManager.AppSettings["BloqueoEquipoSinStockColor"].ToString();
            HidMenColorBloqueo.Value = ConfigurationManager.AppSettings["constMsjEquipoSinStockSel"].ToString();
            string nroDocumento = Funciones.CheckStr(Request.QueryString["nroDocumento"]);
            string strCFServAdic = Funciones.CheckStr(Request.QueryString["strCFServAdic"]);
            string strServAdic = Funciones.CheckStr(Request.QueryString["strServAdic"]);
            string idtipoOper = Request.QueryString["idtipoopera"];
            string idmodventa = Request.QueryString["codmodalidad"];
            string familia = Request.QueryString["familia"];
            string idFlujo = Request.QueryString["idFlujo"];
            string strNewEquipo = Request.QueryString["strNewEquipo"];//PROY-30748-MGAMBINI
            string materialCanje = Funciones.CheckStr(Request.QueryString["materialCanje"]); //PROY-140736
            HttpContext.Current.Session["materialCanje"] = materialCanje;//PROY-140736
            //PROY-140335 - INICIO EJRC
            string cadenaPorta = Request.QueryString["cadenaPorta"];
            string strCodRespuesta = string.Empty;
            string strMsgRespuesta = string.Empty;
            BEPorttSolicitud objConsultaPrevia = new BEPorttSolicitud();
            //PROY-140335 - FIN EJRC

            objLog.CrearArchivolog("[nroDocumento] :"+nroDocumento, null, null);
            objLog.CrearArchivolog("[strCFServAdic]:"+strCFServAdic, null, null);
            objLog.CrearArchivolog("[idtipoOper]:"+idtipoOper, null, null);
            objLog.CrearArchivolog("[idmodventa]:"+idmodventa, null, null);
            objLog.CrearArchivolog("[familia]:" + familia, null, null);
            objLog.CrearArchivolog("[idFlujo]:" + idFlujo, null, null);
            //PROY-140335 - EJRC INICIO
            objLog.CrearArchivolog("[cadenaPorta]:" + cadenaPorta, null, null);
            HttpContext.Current.Session["flagEnvioCPProactivo"] = string.Empty;
            if (idFlujo == "2") {
                string[] arrConsultaPrevia = cadenaPorta.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                
                objConsultaPrevia.operadorCedente = Funciones.CheckStr(arrConsultaPrevia[1]);
                objConsultaPrevia.modalidadOrigen = Funciones.CheckStr(arrConsultaPrevia[2]);
                objConsultaPrevia.TipoDocumento = Funciones.CheckStr(arrConsultaPrevia[3]);
                objConsultaPrevia.NroDocumento = Funciones.CheckStr(arrConsultaPrevia[4]);
                objConsultaPrevia.numeroLinea = Funciones.CheckStr(arrConsultaPrevia[0]);
                BEPorttSolicitud DetalleCP = BLPortabilidad.ValidarRepositorioABDCP(objConsultaPrevia, ref strCodRespuesta, ref strMsgRespuesta);
                HttpContext.Current.Session["flagEnvioCPProactivo"] = DetalleCP.flagConsultaPrevia;
            }
            //PROY-140335 - EJRC INICIO

            hidTablePlan.Value = FConsultaPlanes(nroDocumento, strServAdic, strCFServAdic, idtipoOper, idmodventa, familia, false, strNewEquipo, idFlujo, "");//PROY-30748-MGAMBINI
            objLog.CrearArchivolog("[hidTablePlan] :" + hidTablePlan.Value, null, null);
            objLog.CrearArchivolog("[Fin][Carga - Evaluacion_Proactiva]", null, null);
            //EMMH I
            List<BEParametro> lstBEParametroEvaluacionProactiva = new List<BEParametro>();            
            lstBEParametroEvaluacionProactiva = (List<BEParametro>)Session["ListaParametrosEP"];            
            hidMsgEquipoNoPlanesProac.Value = lstBEParametroEvaluacionProactiva.Where(p => p.Valor1 == "constMsjEquipoNoPlanesProac").SingleOrDefault().Valor;
            //EMMH F
        }
        
        public string FConsultaPlanes(string nroDocumento,string strServAdic, string strCFServAdic, string idtipoOper, string idmodventa, string familia, bool blnActualizar,string strNewEquipo, string idFlujo, string strComparar)
        {

            GeneradorLog objLog = new GeneradorLog(null, null, null, "Log_BWEvaluaReglas");
            BEPlanProactivo objBEPlanProactivo = new BEPlanProactivo();
            objBEPlanProactivo = (BEPlanProactivo)HttpContext.Current.Session["BEPlanProactivo"];
            BEClienteCuenta objCliente = (BEClienteCuenta)HttpContext.Current.Session["objCliente" + nroDocumento];
            objBEPlanProactivo.CFServAdic = Funciones.CheckDbl(strCFServAdic);
            objBEPlanProactivo.ServAdic = strServAdic;
            objBEPlanProactivo.idtipoOper = idtipoOper;
            objBEPlanProactivo.idmodventa = idmodventa;
            objBEPlanProactivo.familia = familia;
            objBEPlanProactivo.idFLujo = idFlujo;

            objLog.CrearArchivolog("[FLAG Comparar Equipo: ]" + strComparar, null, null);
            //Consultar equipo con busqueda comparativa
            if (blnActualizar)
            {
                if (strComparar == "1")
                {
                    objLog.CrearArchivolog("[INI][Comparar Equipo 1 :]" + strComparar, null, null);
                    objBEPlanProactivo.CadenaEquipo = strNewEquipo;
                }
                else
                {
                    objLog.CrearArchivolog("[INI][Comparar Equipo 2 :]" + strComparar, null, null);
                    objBEPlanProactivo.CadenaEquipo = strNewEquipo;
                }
             
            }
            else//PROY-30748-INICIO-MGAMBINI
            {
                objLog.CrearArchivolog("[INI][blnActualizar]" + blnActualizar, null, null);
                objLog.CrearArchivolog("[INI][strNewEquipo]" + strNewEquipo, null, null);
                objBEPlanProactivo.CadenaEquipo = strNewEquipo;
            }
            //PROY-30748-FIN-MGAMBINI

            string[] arrayEquipo = objBEPlanProactivo.CadenaEquipo.Split('|');
            string strEquiposDetalle = arrayEquipo[arrayEquipo.Length - 2];

            string[] arrayEquipoSplit = objBEPlanProactivo.CadenaEquipo.Split(';'); //EMMH
            string strEquipo = arrayEquipoSplit[arrayEquipoSplit.Length - 6]; //EMMH
            
            string strDatosGeneral = objBEPlanProactivo.CadenaDatos;
           
            string[] arrayPlan = objBEPlanProactivo.CadenaPlan.Split('|');
            string strPlanesDetalle = arrayPlan[arrayPlan.Length - 2];
            string strServiciosDetalle;

            //EMMH I
            List<BEParametro> lstBEParametroEvaluacionProactiva = new List<BEParametro>();
            lstBEParametroEvaluacionProactiva = (List<BEParametro>)Session["ListaParametrosEP"];
            objBEPlanProactivo.PuntodeDespacho = lstBEParametroEvaluacionProactiva.Where(p => p.Valor1 == "constPuntodeDespachoBssEvalProa").SingleOrDefault().Valor;
            //objBEPlanProactivo.PuntodeDespacho = ConfigurationManager.AppSettings["constPuntodeDespachoBssEvalProa"].ToString(); // PROY-30748 - Putno de Despacho
            //EMMH F



            if (objBEPlanProactivo.CadenaServicio != null && objBEPlanProactivo.CadenaServicio != "")
            {
                string[] arrayServiciosDetalle = objBEPlanProactivo.CadenaServicio.Split('|');
                 strServiciosDetalle = arrayServiciosDetalle[arrayServiciosDetalle.Length - 2];
            }
            else
            {
                strServiciosDetalle = string.Empty;
            }
            List<BEPlan> lstPlan = new List<BEPlan>();
            string strTablePlan = string.Empty;
            try{

                BEPlanProactivo objPlanProactivo = FConsultaDetallePlan(objBEPlanProactivo, nroDocumento, strDatosGeneral, strEquiposDetalle, strPlanesDetalle, strServiciosDetalle);


                if (blnActualizar)
                {
                    if (strComparar == "1")
                    {
                        objLog.CrearArchivolog("[FIN][Comparar Equipo 1 :]" + strComparar, null, null);
                    }
                    else
                    {
                        objLog.CrearArchivolog("[FIN][Comparar Equipo 2 :]" + strComparar, null, null);
                    }

                }
                string srvDisponible;
                if (objPlanProactivo.PlanBSSEval.Count == 0)
                    return "VALIDACION";
                
                    foreach (BEPlanBSSEval item in objPlanProactivo.PlanBSSEval)
                {
                    //PROY 30748 F2 INI EMMH
                        //TODO: CALCULO DE CUOTA INICIAL
                    double dblCuotaInicial = Math.Round((item.PrecionVenta * item.Cuota.porcenCuotaInicial) / 100);
                        //TODO: CALCULO DEL TOTAL A PAGAR
                    double dblTotalPagar = (item.PrecionVenta - dblCuotaInicial);
                    //PROY 30748 F2 FIN EMMH

                    if (item.ServiciosAdicionales == "")
                    {
                        srvDisponible = "SSA";
                    }
                    else
                    {
                        srvDisponible = item.ServiciosAdicionales;
                    }

                        strTablePlan += string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};" +
                                                      "{11};{12};{13};{14};{15};{16};{17};{18};{19};" +
                                                      "{20};{21};{22};{23};{24};{25};{26};{27};{28};{29};{30};{31};{32}|",////PROY 30748 F2 INI MDE //PROY-140335 RF1
                                                       item.codigo, item.descripcion, Funciones.CheckStr(Math.Round(item.MontoRA)), Funciones.CheckStr(item.PrecionVenta), //0,1,2,3
                                                       item.Cuota.cuota, Funciones.CheckStr(dblCuotaInicial), Funciones.CheckStr(dblTotalPagar), //4,5,6 //PROY 30748 F2 SAMA
                                                       item.cantidad, item.cantidadDeLineasAdicionalesRUC,item.cantidadDeLineasMaximas,item.capacidadDePago, Funciones.CheckStr(item.cargoFijo), //7,8,9,10,11
                                                       item.Cuota.cuota,item.factorDeEndeudamientoCliente, item.factorDeRenovacionCliente,item.montoCFParaRUC, //12,13,14,15
                                                       item.montoDeGarantia, Funciones.CheckStr(item.precioDeVentaTotalEquipos), item.procesoIDValidator, item.restriccion, //16,17,18,19
                                                       item.riesgoTotalEquipo, item.TieneAutonomia, item.TipoDeAutonomiaCargoFijo, item.tipodecobro, //20,21,22,23
                                                       Funciones.CheckStr(item.totalPagar), item.CostoEquipo, srvDisponible, item.TopeMonto,  //24//25 //26 //27
                                                       item.IdListaPrecio, item.DesListaPrecio, item.CodListaPrecio, item.Cuota.porcenCuotaInicial,item.ejecucionConsultaPrevia); //28 // 29 //30 //PROY 30748 F2 MDE //32 PROY-140335 RF1
                        item.Equipo = strEquipo;//EMMH
                        objLog.CrearArchivolog("[EMMH][item.strTablePlan :]" + strTablePlan, null, null); //PROY-140439 BRMS CAMPAÑA NAVIDEÑA
                        objLog.CrearArchivolog("[EMMH][item.descripcion :]" + item.descripcion, null, null);
                        objLog.CrearArchivolog("[EMMH][item.TopeMonto :]" + item.TopeMonto, null, null);
                        objLog.CrearArchivolog("[EMMH][strEquipo :]" + strEquipo, null, null);

                    }
                    strTablePlan = strTablePlan.Substring(0, strTablePlan.ToString().Length - 1) + "&" + objBEPlanProactivo.NroCuota +"&" + objCliente.oficina;

                    List<BEPlanBSSEval> ListIntermBEPlanes = new List<BEPlanBSSEval>();

                    if (blnActualizar)
                    {
                        var temp = (BEPlanProactivo)HttpContext.Current.Session["BEPlanProactivo"];

                        ListIntermBEPlanes = temp.PlanBSSEval;

                        foreach (BEPlanBSSEval item in objPlanProactivo.PlanBSSEval)
                        {
                            ListIntermBEPlanes.Add(item);
                }

                        objBEPlanProactivo.PlanBSSEval = ListIntermBEPlanes;
                    }
                    else
                    {

                objBEPlanProactivo.PlanBSSEval = objPlanProactivo.PlanBSSEval;

                    }

                HttpContext.Current.Session["BEPlanProactivo"] = objBEPlanProactivo;
                
                return strTablePlan;
            }
            catch (Exception ex){

                return string.Empty;
            }
        }

        public BEPlanProactivo FConsultaDetallePlan( BEPlanProactivo objBEPlanProactivo, string nroDocumento, string strDatosGeneral, string strEquiposDetalle, string strPlanesDetalle, string strServiciosDetalle) 
        {
            GeneradorLog objLog = new GeneradorLog(null, null, null, "Log_BWEvaluaReglas");
            List<BEPlan> objListPlan = new List<BEPlan>();
            BEClienteCuenta objCliente = (BEClienteCuenta)HttpContext.Current.Session["objCliente" + nroDocumento];
            //PROY 30748 F2 MDE INI
            List<BEDireccionCliente> objDireccion;
            try
            {
                if (HttpContext.Current.Session["objDireccion" + nroDocumento] != null)
                {
                objDireccion = (List<BEDireccionCliente>)HttpContext.Current.Session["objDireccion" + nroDocumento];
            }
            else
            {
                objDireccion = null;
            }
            }
            catch (Exception ex)
            {
                objDireccion = null;
                objLog.CrearArchivolog("FConsultaDetallePlan - ERROR en objDireccion.", null, null);
            }
       
            //PROY 30748 F2 MDE FIN
            
            string strUsuario = ((BEUsuarioSession)(HttpContext.Current.Session["Usuario"])).Login;
            string strFlgCuota = "N";
            string strFlgProteccionMovil = string.Empty;

            //EMMH I
            //objCliente.Deuda = ConfigurationManager.AppSettings["constDeudaEvalBRMS"].ToString(); // PROY-30748 - DEUDA
            //EMMH F

            //PROY-31948 INI
            BECuota objCuotaOAC = new BECuota();
            BECuota objCuotaPVU = new BECuota();

            string strTipoDocumento = objCliente.tipoDoc;
            string strNroDocumento = objCliente.nroDoc;
            string strNroLinea = string.Empty;

            objCuotaOAC = (BECuota)HttpContext.Current.Session["objCuotaOAC"];
            objCuotaPVU = (BECuota)HttpContext.Current.Session["objCuotaPVU"];

            if (objCuotaOAC == null && objCuotaPVU == null)
            {
                WebComunes.ConsultarCuotasPendientes(strTipoDocumento, strNroDocumento, strNroLinea, ref objCuotaOAC, ref objCuotaPVU);//4ta llamada
            }
            //PROY-31948 FIN
            WS.WSReglasCrediticia.ClaroEvalClientesReglasRequest oRequestReglasCrediticia = (WS.WSReglasCrediticia.ClaroEvalClientesReglasRequest)HttpContext.Current.Session["RequestReglaCrediticia" + nroDocumento];
            //PROY-INI-140736
            string []strcodcampania = strPlanesDetalle.Split(';');
            string[] strcodbuyback = ReadKeySettings.Key_CodCampaniaBuyBack.Split('|');
            bool ExisteBuyback = false;
            int i = 0;
            for (i = 0; i < strcodbuyback.Length; i++)
            {
                if(strcodbuyback[i] == strcodcampania[8]){
                    ExisteBuyback = true;
                }
            }
            if (ExisteBuyback)
            {
                HttpContext.Current.Session["ExisteBuyback"] ="SI" ;


            }
            else {
                HttpContext.Current.Session["ExisteBuyback"] = "NO";    
                }
            //PROY-FIN-140736
    
            BEPlanProactivo objBSSPlanProactivo = (new BLEvaluaReglas()).obtenerEvaluacionProactiva(objCliente, objBEPlanProactivo, objDireccion, strDatosGeneral, strPlanesDetalle, strServiciosDetalle,
                                                 strEquiposDetalle, strFlgCuota, strFlgProteccionMovil, strUsuario, oRequestReglasCrediticia, objCuotaOAC, objCuotaPVU);//PROY-31948
            return objBSSPlanProactivo;


        }

    
    }
}