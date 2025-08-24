using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Claro.SISACT.Common;
using Claro.SISACT.Entity;
using Claro.SISACT.Business;
using Claro.SISACT.WS;
using System.Data;
using Claro.SISACT.Web.Base;
using Claro.SISACT.Web.Comun;
using System.Text;
using Claro.SISACT.Web.Paginas.evaluacion_cons;//PROY-30748
using Claro.SISACT.WS.RestReferences;//PROY-140743 - INI
using Claro.SISACT.Entity.ConsultaSOT.Response;
using Claro.SISACT.Entity.ConsultaSOT.Request;//PROY-140743 - FIN

namespace Claro.SISACT.Web.frames
{
    public partial class sisact_ifr_consulta_unificada : Sisact_Webbase
    {
        #region [Declaracion de Constantes - Config]

        string strCodTipoProducto3Play = ConfigurationManager.AppSettings["consTipoProducto3Play"];
        string strCodTipoProductoDTH = ConfigurationManager.AppSettings["constTipoProductoDTH"];
        string strCodTipoProductoFijo = ConfigurationManager.AppSettings["constTipoProductoFijo"];
        string strCodTipoProductoVV = ConfigurationManager.AppSettings["constTipoProductoVentaVarios"];
        string strCodPlanVentaVarios = ConfigurationManager.AppSettings["constCodPlanVentaVarios"];
        string strCodParamCampanaxCuota = ConfigurationManager.AppSettings["constCodParamCampanaxCuota"];
        string strCodModalidadCuota = ConfigurationManager.AppSettings["constCodModalidadCuota"];
        string strCodModalidadCuotaSinCode = ConfigurationManager.AppSettings["constCodModalidadCuotaSinCode"];
        string strCodModalidadChipSuelto = ConfigurationManager.AppSettings["constCodModalidadChipSuelto"];
        string strCodTipoOpeMigracion = ConfigurationManager.AppSettings["constTipoOperacionMIG"].ToString();
        string strCodParamCampanaPortaMasTablet = ConfigurationManager.AppSettings["constCodParamCampanaPortaMasTablet"]; //CAMPAÑA PORTA+TABLET - INICIO
        string strCodParamNumDiasVigenciaCampanaPortaMasTablet = ConfigurationManager.AppSettings["constCodParamNumDiasVigenciaCampanaPortaMasTablet"]; //CAMPAÑA PORTA+TABLET - FIN
        string strFlagServicioRI = string.Empty;
        string strCumpleReglaA = string.Empty;//PROY-29121

        #endregion [Declaracion de Constantes - Config]

        //rsm 30859 INI
        public string canalesPermitidosBAM = String.Empty;
        public string tipoOperacionesPermitidosBAM = String.Empty;
        public string modalidadVentasPermitidasBAM = String.Empty;
        public string activacionValidacionMaterialesBAM = String.Empty;
        //rsm 30859 FIN

        private void Page_Load(System.Object sender, System.EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            InicializarParametrosBAM(); //rsm 30859
            BEFormEvaluacion obj = new BEFormEvaluacion();

            obj.idFila = Funciones.CheckInt(Request.QueryString["idFila"]);
            obj.metodo = Request.QueryString["strMetodo"];

   
            obj.nroSEC = Funciones.CheckInt64(Request.QueryString["strNroSec"]);

            obj.idOficina = Funciones.CheckStr(Request.QueryString["strOficina"]);
            obj.idFlujo = Funciones.CheckStr(Request.QueryString["strFlujo"]);
            obj.idOferta = Funciones.CheckStr(Request.QueryString["strOferta"]);
            obj.idTipoOperacion = Funciones.CheckStr(Request.QueryString["strTipoOperacion"]);
            obj.idCasoEspecial = Funciones.CheckStr(Request.QueryString["strCasoEspecial"]).Split('_')[0];
            obj.idCombo = Funciones.CheckStr(Request.QueryString["strCombo"]);
            obj.idTipoVenta = ConfigurationManager.AppSettings["strTVPostpago"].ToString();

            obj.flgPorta = (obj.idFlujo == "2") ? "S" : "N";

            obj.tipoDocumento = Funciones.CheckStr(Request.QueryString["strTipoDocumento"]);
            obj.nroDocumento = Funciones.CheckStr(Request.QueryString["strNroDoc"]);

            obj.idProducto = Funciones.CheckStr(Request.QueryString["strTipoProducto"]);
            obj.idCampana = Funciones.CheckStr(Request.QueryString["strCampana"]);

            if (obj.idCampana != "" && (obj.idProducto != strCodTipoProductoDTH || obj.idProducto != strCodTipoProducto3Play))
                obj.idCampanaSap = ObtenerCampanaSap(obj.idCampana);

            obj.idPlazo = Funciones.CheckStr(Request.QueryString["strPlazo"]);
            obj.idPaquete = Funciones.CheckStr(Request.QueryString["strPaquete"]);
            obj.nroSecuencia = Funciones.CheckInt(Request.QueryString["strSecuencia"]);
            obj.idPlan = Funciones.CheckStr(Request.QueryString["strPlan"]);
            obj.listaPlan = Funciones.CheckStr(Request.QueryString["strPlanes"]);
            obj.idPlanSap = Funciones.CheckStr(Request.QueryString["strPlanSap"]);
            obj.idMaterial = Funciones.CheckStr(Request.QueryString["strMaterial"]);
            obj.idListaPrecio = Funciones.CheckStr(Request.QueryString["strListaPrecio"]);
            obj.idCuota = Funciones.CheckStr(Request.QueryString["strCuota"]);

            obj.fechaSap = DateTime.Now.ToString("yyyyMMdd");
            obj.canalSap = Funciones.CheckStr(Request.QueryString["strCanalSap"]);
            obj.centroSap = Funciones.CheckStr(Request.QueryString["strCentro"]);
            obj.orgVentaSap = Funciones.CheckStr(Request.QueryString["strOrgVenta"]);
            obj.tipoDocumentoSap = Funciones.CheckStr(Request.QueryString["strTipoDocVentaSap"]);

            obj.evaluarFijo = Funciones.CheckStr(Request.QueryString["strEvaluarSoloFijo"]);
            obj.modalidadVenta = Request.QueryString["strModalidadVenta"];
            obj.idTipoOficina = Request.QueryString["strTipoOficina"];

            if (obj.tipoDocumento == ConfigurationManager.AppSettings["TipoDocumentoRUC"])
                obj.tipoDocumento = Funciones.TipoRUC1020(obj.nroDocumento);

            obj.idTipoOperacionSap = ConfigurationManager.AppSettings["constTipoOperacionAltaSap"].ToString();
            if (obj.idTipoOperacion == strCodTipoOpeMigracion)
                obj.idTipoOperacionSap = ConfigurationManager.AppSettings["constTipoOperacionMigracionSap"].ToString();
            //gaa20161020
            obj.idFamiliaPlan = Request.QueryString["strFamiliaPlan"];
            //fin gaa20161020

	    obj.codServicio = Request.QueryString["strCodServicio"]; //PROY-29296
            obj.idPromocion = Funciones.CheckStr(Request.QueryString["strPromocion"]);//PROY-140743
            obj.codProdLinAsociada = Funciones.CheckStr(Request.QueryString["strCodProdLinAsociada"]);//PROY-140743
            //INICIO PROY-21600 - IDEA-27430 Mejoras en el proceso de renovación masiva postpago] - Fase 4 - JAZ
            strFlagServicioRI = Funciones.CheckStr(Request.QueryString["strFlagServicioRI"]);
            //PROY-21600 - IDEA-27430 Mejoras en el proceso de renovación masiva postpago] - Fase 4 - JAZ
            hidMetodo.Value = obj.metodo;
            hidIdFila.Value = obj.idFila.ToString();

            // proy - 30748 INICIO 
            string strCodEquipo = Request.QueryString["strcodequipo"];
            string strDesEquipo = Request.QueryString["strdesquipo"];
            string strDesTabla = Request.QueryString["strtabla"];

            string strTipOperacion = Request.QueryString["strTipOperacion"];
            string strServAdicional = Request.QueryString["strServAdicional"];
            string strCFServAdic = Request.QueryString["strCFServAdic"];//cristhian

            string intFlagEquipo = Request.QueryString["intFlagEquipo"];
            string strNewEquipo = Request.QueryString["strNewEquipo"];
            string strfamilia = Request.QueryString["familia"];
            string strComparar = Request.QueryString["strComparar"];

            // proy - 30748 FIN

            //INC000002510501 - INI
            hidModalidadPortabi.Value = Request.QueryString["strModalidadPorta"];
            hidOperadorPortabi.Value = Request.QueryString["strOperadorPorta"];
            //INC000002510501 - FIN

            string strResultado = string.Empty;

            obj.CumpleReglaAClienteRRLL = Request.QueryString["strCumpleReglaAClienteRRLL"];//PROY-29121
            
            obj.strcampaniabuyback = Request.QueryString["strcampaniabuyback"];//PROY-140736
            obj.strmaterialbuyback = Funciones.CheckStr(Request.QueryString["strmaterialbuyback"]);//PROY-140736

            BEUsuarioSession ojbUsuario = new BEUsuarioSession();
            GeneradorLog _objLog = CrearLog(obj.nroDocumento);

            _objLog.CrearArchivolog("INC000003848031 - obj.metodo : ", Funciones.CheckStr(obj.metodo), null); //INC000003848031
            _objLog.CrearArchivolog("PROY-140743 - obj.idPromocion : ", Funciones.CheckStr(obj.idPromocion), null); //PROY-140743


            try
            {
                ojbUsuario = (BEUsuarioSession)HttpContext.Current.Session["Usuario"];
                if (BEGlobal.usuarioConsulta == ojbUsuario.idCuentaRed)
                {
                    _objLog.CrearArchivolog("[" + obj.metodo + "][IN]", obj, null);
                }

                switch (obj.metodo)
                {
                    case "LlenarPlan":
                        strResultado = LlenarPlan(obj);
                        break;
                    case "LlenarEquipoPrecio":
                        strResultado = LlenarEquipoPrecio(obj);
                        break;
                    case "LlenarMontoTopeConsumo":
                        strResultado = LlenarMontoTopeConsumo(obj);
                        break;
                    case "LlenarServCampCorpTot":
                        strResultado = LlenarServicioMaterialCorpTotal(obj);
                        break;
                    case "LlenarPaquetePlan":
                        strResultado = LlenarPaquetePlan(obj);
                        break;
                    case "LlenarPaquete3Play":
                        strResultado = LlenarPaquete3Play(obj);
                        break;
                    case "LlenarPlanPaq3Play":
                        strResultado = LlenarPlanesXPaquete3Play(obj);
                        break;
                    case "LlenarServicioHFC":
                        strResultado = LlenarServiciosXPlan3Play(obj);
                        break;
                    case "LlenarEquipo3Play":
                        strResultado = LlenarEquipo3Play(obj);
                        break;
                    case "LlenarPlanPaq":
                        strResultado = LlenarPlanPaq(obj);
                        break;
                    case "LlenarCampanaPlazo":
                        strResultado = LlenarCampanaPlazo(obj);
                        break;
                    case "LlenarServicioMaterial":
                        strResultado = LlenarServicioMaterial(obj);
                        break;
                    case "LlenarPlanesCombo":
                        strResultado = LlenarPlanesCombo(obj);
                        break;
                    case "LlenarServicioKit":
                        strResultado = LlenarServicioKit(obj);
                        break;
                    case "LlenarTopesConsumo":
                        strResultado = ListarTopesConsumo3Play(obj.idProducto); //PROY-29296
                        break;
                    case "MostrarSecPendiente":
                        strResultado = SECPendiente(obj);
                        break;
                    case "LlenarMaterial":
                        strResultado = LlenarMaterial(obj);
                        break;
                    case "LlenarListaPrecio":
                        strResultado = LlenarListaPrecio(obj);
                        break;
                    case "LlenarListaPrecioPrecio":
                        strResultado = LlenarPrecio(obj);
                        break;
                    case "LlenarFamiliaPlan": //gaa20161020
                        strResultado = LlenarFamiliaPlan(obj);
                        break;
                    //PROY - 30748 -  INICIO - strResultado = FLlenarEquipoPlan
                    case "LlenarPlanProactivo":
                        strResultado = FLlenarEquipoPlan(strCodEquipo, strDesEquipo, Funciones.CheckStr(Request.QueryString["strTipoModalidadVenta"]), strfamilia, strDesTabla, Request.QueryString["strNroDocumento"], strTipOperacion, strServAdicional, Funciones.CheckInt(intFlagEquipo), strNewEquipo, obj.idFlujo, strComparar, strCFServAdic);
                        //APOYO-PROY-30748-INICIO
                        strResultado = strResultado.Split('&')[0];
                        _objLog.CrearArchivolog("[PROY_30748][LlenarPlanProactivo]", strResultado, null);
                        //APOYO-PROY-30748-FIN
                        break;
                    //PROY - 30748 -  FIN 
                    case "LlenarMontosTopeConsumo":
                        strResultado = LlenarMontosTopeConsumo(obj); //PROY-29296
                        break;
                    case "LlenarMontosTopeConsumoLTE":
                        strResultado = LlenarMontosTopeConsumoLTE(obj);  //PROY-29296
                        break;
                    #region [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil]
                    case "llenarPromociones":
                        strResultado = llenarPromociones(obj);
                        break;
                    case "llenarLineasAsociadas":
                        strResultado = llenarLineasAsociadas(obj);
                        break;
                    #endregion
                    default:
                        _objLog.CrearArchivolog("[FALTA][" + obj.metodo + "]", null, null);
                        break;
                }
                hidnResultadoValue.Value = strResultado;
            }
            catch (Exception ex)
            {
                if (obj.metodo != "LlenarEquipoPrecio")
                {
                    hidnMensajeValue.Value = ConfigurationManager.AppSettings["constMsjErrorEvaluacion"];
                }
                else
                {
                    hidnMensajeValue.Value = ex.Message;
                }
                hidnResultadoValue.Value = "";

                _objLog.CrearArchivolog("[ERROR][" + obj.metodo + "]", null, ex);
            }
            finally
            {
                if (BEGlobal.usuarioConsulta == ojbUsuario.idCuentaRed)
                {
                    _objLog.CrearArchivolog("[" + obj.metodo + "][OUT]", strResultado, null);
                }
            }
        }

        #region [Movil]

            //Proy - 30748 inicio

        private string FLlenarEquipoPlan(string strCodEquipo, string strDesEquipo, string strTipoModalidadVenta, string strfamilia, string strDesTabla, string strNroDocumento, string strTipOperacion, string strServAdicional,
        int intFlagEquipo, string strNewEquipo, string idFlujo, string strComparar,string strCFServAdic)
        {
        hidcodmodalidad.Value = strTipoModalidadVenta;
        hiddestable.Value = strDesTabla;
        sisact_popup_evaluacion_proactiva frmfuncionconsulta = new sisact_popup_evaluacion_proactiva();

  
            bool blnflag = false;
            if (intFlagEquipo ==1)
            {
                blnflag = true;
            }

            return frmfuncionconsulta.FConsultaPlanes(strNroDocumento, strServAdicional, strCFServAdic, strTipOperacion,
                strTipoModalidadVenta, strfamilia, blnflag, strNewEquipo, idFlujo, strComparar).ToString();
        }
        //Proy - 30748 fin
        private string LlenarCampanaPlazo(BEFormEvaluacion objForm)
        {
            GeneradorLog objLog = new GeneradorLog(CurrentUser, objForm.nroDocumento.ToString() , null, "WEB");
            objLog.CrearArchivolog("[Inicio][LlenarCampanaPlazo]", null, null);
            string strResultado = string.Empty;
            //PROY-24740
            strResultado = string.Format("{0}¬{1}", ListarCampana(objForm),LlenarPlazo(objForm));            
            objLog.CrearArchivolog("[LISTAR_CAMPAÑA]", strResultado.ToString(), null);
            objLog.CrearArchivolog("[Fin][LlenarCampanaPlazo]", null, null);
            return strResultado;
            //PROY-24740
        }

        private string LlenarServicioMaterial(BEFormEvaluacion objForm)
        {
            string strResultado = string.Empty;
            if (objForm.modalidadVenta != strCodModalidadChipSuelto)
            {
                strResultado = string.Format("{0}~{1}", LlenarServicio(objForm), LlenarMaterial(objForm));                
            }
            else
            {
                strResultado = string.Format("{0}~", LlenarServicio(objForm));
            }            
            return strResultado;
        }

        //PROY-24740
        public string LlenarMaterial(BEFormEvaluacion objForm)
        {
            GeneradorLog objLog = new GeneradorLog(CurrentUser, objForm.nroDocumento.ToString(), null, "WEB"); // rsm 30859
            if (objForm.idProducto == strCodTipoProductoVV) { objForm.idPlanSap = strCodPlanVentaVarios; objForm.idPlan = strCodPlanVentaVarios; }
            string strResultado = string.Empty;
            BEConsultaDatosOficina oListParametrosOficina = BLSincronizaSap.ConsultaDatosOficina(objForm.idOficina, null);
            objForm.tipoOficina = oListParametrosOficina.TipoOficina;
            objForm.idDepartamento = oListParametrosOficina.CodigoRegion;
            //rsm 30859
            string canalActual = oListParametrosOficina.TipoOficina;
            objLog.CrearArchivolog("[PROY 30859 - PARAMETROS FILTRO MATERIALES BAM]", "", null);
            objLog.CrearArchivolog("[canalActual]", canalActual, null);
            string tipoOperacionActual = objForm.idTipoOperacion;
            objLog.CrearArchivolog("[tipoOperacionActual]", tipoOperacionActual, null);
            string modalidadVentaActual = objForm.modalidadVenta; 
            objLog.CrearArchivolog("[modalidadVentaActual]", modalidadVentaActual, null);
            //rsm 30859 FIN

            #region [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil]
            bool blVentVarias = (objForm.idTipoOperacion.Equals("25"));
            List<BEItemGenerico> objLista = null;

            if (blVentVarias)
            {
                objLista = ObtenerStockXMaterial(objForm, oListParametrosOficina);
            }
            else
            {
                objLista = ObtenerMaterialMSSAP(objForm);
            }
            #endregion

            StringBuilder sbMateriales = new StringBuilder();
            //PROY-30859-IDEA-39316-RU02-by-LCEJ INI   
            

            Boolean ValidBam = false;
            //rsm 30859 INI
            if (objForm.idProducto == ConfigurationManager.AppSettings["constTipoProductoBAM"])
            { 
                if (activacionValidacionMaterialesBAM == "1")
                {
                    if (canalesPermitidosBAM.IndexOf(canalActual) > -1)
                    {
                        if (tipoOperacionesPermitidosBAM.IndexOf(tipoOperacionActual) > -1)
                        {
                            if (modalidadVentasPermitidasBAM.IndexOf(modalidadVentaActual) > -1)
                            {
                                ValidBam = true;
                            }
                        }
                    }
                }
            }

            //rsm 30859 FIN
            if (!ValidBam)
            {
            if (objForm.idProducto == strCodTipoProductoVV)
            {
                List<BEItemGenerico> objListaEquipoCombo = new BLGeneral_II().ListarComboEquipo(objForm.idCombo);
                foreach (BEItemGenerico obj in objLista)
                {
                    if (objListaEquipoCombo.Count > 0)
                    {
                        foreach (BEItemGenerico objEquipo in objListaEquipoCombo.Where(w => w.Codigo == obj.Codigo))
                            {
                                sbMateriales.Append("|");
                                sbMateriales.Append(obj.Codigo);
                                sbMateriales.Append(";");
                                sbMateriales.Append(obj.Descripcion);
                            }
                        }
                    else
                    {
                        sbMateriales.Append("|");
                        sbMateriales.Append(obj.Codigo);
                        sbMateriales.Append(";");
                        sbMateriales.Append(obj.Descripcion);
                    }
                }
            }
            else
            {
                   
                foreach (BEItemGenerico obj in objLista)
                {
                    sbMateriales.Append("|");
                    sbMateriales.Append(obj.Codigo);
                    sbMateriales.Append(";");
                    sbMateriales.Append(obj.Descripcion);
                }
                //PROY-140743
                if (ValidarAccesoOpcion(objForm.idTipoOficina, objForm.idProducto, objForm.idTipoOperacion, objForm.tipoDocumento, ConfigurationManager.AppSettings["BloqueoEquipoPromoCod"]) && !blVentVarias)
                {
                DataTable dtGama = new BLGeneral_II().ListarEquipoGama();
                foreach (DataRow dr in dtGama.Rows)
                {
                    if (dr["eqgan_orden"].ToString() == "1")
                        {                            
                            sbMateriales.Append("|");
                            sbMateriales.Append(dr["eqgac_codigo"]);
                            sbMateriales.Append(";");
                            sbMateriales.Append(dr["eqgav_descripcion"]);
                        }
                    else
                        {                            
                            sbMateriales.Append("|");
                            sbMateriales.Append(dr["eqgac_codigo"]);
                            sbMateriales.Append(";");
                            sbMateriales.Append(dr["eqgav_descripcion"]);
                        }
                }
            }
            }
            }
            else
            {
                objLog.CrearArchivolog("[BOOL ValidBam TRUE]", "", null);
                foreach (BEItemGenerico obj in objLista)
                {

                    sbMateriales.Append("|");
                    sbMateriales.Append(obj.Codigo);
                    sbMateriales.Append(";");
                    sbMateriales.Append(obj.Descripcion);
                }
            }

            //INICIO|PROY-140533 - CONSULTA STOCK
            BEUsuarioSession objUsuarioSession = (BEUsuarioSession)Session["Usuario"];
            var strTipoProducto = String.Empty;
            strTipoProducto = Funciones.CheckStr(Request.QueryString["strTipoProducto"]);

            Int64 strParametrosDelivery = Funciones.CheckInt64(ConfigurationManager.AppSettings["Key_ParanGrupoDelivery"]);
            List<BEParametro> lstParametrosDelivery = new BLGeneral().ListaParametrosGrupo(Funciones.CheckInt(strParametrosDelivery));

            if (lstParametrosDelivery != null && lstParametrosDelivery.Count > 0)
            {
                AppSettings.Key_ProductosPermitidosPicking = lstParametrosDelivery
                  .Where(w => w.Valor1.Equals("Key_ProductosPermitidosPicking")).ToList().Count > 0 ?
                  Funciones.CheckStr(lstParametrosDelivery.Where(w => w.Valor1.Equals("Key_ProductosPermitidosPicking")).ToList()[0].Valor) : string.Empty;
            }

            if (Funciones.CheckStr(AppSettings.Key_ProductosPermitidosPicking).Contains(strTipoProducto) && !blVentVarias)//PROY-140743
            {
                var blFlagPicking = false;
                blFlagPicking = consultaFlagPicking();

                if (blFlagPicking)
                {
                    var strDatos = String.Empty;
                    strDatos = Funciones.CheckStr(sbMateriales);
                    var arrDatos = strDatos.Split('|');
                    var cont = arrDatos.Count();

                    StringBuilder sbMateriales2 = new StringBuilder();

                    if (cont > 0)
                    {
                        for (var x = 0; x < cont; x++)
                        {
                            var strMateriales = Funciones.CheckStr(arrDatos[x]);

                            if (!string.IsNullOrEmpty(strMateriales))
                            {
                                var strCodMaterial = Funciones.CheckStr(strMateriales.Split(';')[0]);
                                var strDesCodMaterial = Funciones.CheckStr(strMateriales.Split(';')[1]);

                                if (!strCodMaterial.Contains("^"))
                                {
                                    var strCantidad = ConsultarStock(Funciones.CheckStr(objUsuarioSession.OficinaVenta), Funciones.CheckStr(strCodMaterial), strTipoProducto);
                                    int intCantidad = Funciones.CheckInt(strCantidad);

                                    if (intCantidad <= 0)
                                    {
                                        strCodMaterial = "^" + strCodMaterial;
                                    }
                                }
                                sbMateriales2.Append("|");
                                sbMateriales2.Append(strCodMaterial);
                                sbMateriales2.Append(";");
                                sbMateriales2.Append(strDesCodMaterial);
                            }
                        }
                    }

                    sbMateriales = null;
                    sbMateriales = sbMateriales2;
                }

            }
            //FIN|PROY-140533 - CONSULTA STOCK

            return sbMateriales.ToString();
            //PROY-24740

            //PROY-30859-IDEA-39316-RU02-by-LCEJ FIN
        }

        private string ListarCampana(BEFormEvaluacion objForm)
        {
            string strResultado = string.Empty;

//PROY-140743 - INI
            bool blVentVarias = (objForm.idTipoOperacion.Equals("25"));
            List<BEItemGenerico> objLista = null;

            if (blVentVarias)
            {
                objLista = ((from p in BLMaestro.ConsultaCampanaXTipoVenta(new BEParametrosMSSAP() { TipoVenta = null, EstadoGenerico = ConfigurationManager.AppSettings["ConsEstadoCampanias"] })
                                                  select new BEItemGenerico() { Codigo = p.Codigo, Descripcion = p.Descripcion, Codigo3 = p.Codigo3 }).ToList<BEItemGenerico>()).GroupBy(dis => dis.Codigo).Select(rs => rs.First()).ToList();

                HttpContext.Current.Session["lstCampaniasXTipoVenta"] = objLista;
            }
            else
            {
                objLista = new BLGeneral_II().ListarCampana(objForm.idCombo, objForm.idOficina, objForm.idOferta, objForm.idProducto, objForm.idCasoEspecial, objForm.modalidadVenta, objForm.idFlujo, objForm.idTipoOperacion);
            }
//PROY-140743 - FIN
            
            GeneradorLog objLog = new GeneradorLog(CurrentUser, objForm.nroDocumento.ToString(), null, "WEB"); //PROY-XXXX BRMS CAMPAÑAS

			//Inicio - FTTH -Validacion PLANO --evalenzs
            if (objForm.idProducto == strCodTipoProducto3Play && !string.IsNullOrEmpty(Funciones.CheckStr(ConfigurationManager.AppSettings["ConsPlanoCampanaFTTH"])))
            {
                List<BEDireccionCliente> objListaDireccion =
                    (List<BEDireccionCliente>) Session["objDireccion" + objForm.nroDocumento];
                if (objListaDireccion != null)
                {
                    string[] arrsPlanoCampanaFTTH = ConfigurationManager.AppSettings["ConsPlanoCampanaFTTH"].Split('|');
                    BEDireccionCliente obj= objListaDireccion.Last();

                        if (obj.IdPlano.ToUpper().Contains(arrsPlanoCampanaFTTH[1].ToUpper()))
                        {
                            objLista = (from p in objLista
                                    where p.Descripcion.ToUpper().Contains(arrsPlanoCampanaFTTH[0].ToUpper())
                                select p).ToList();
                        }
                        else
                        {
                            objLista = (from p in objLista
                                    where !p.Descripcion.ToUpper().Contains(arrsPlanoCampanaFTTH[0].ToUpper())
                                select p).ToList();
                        }

                }
            }
            //Fin - FTTH -Validacion PLANO -evalenzs
			
            string codCampanaX = string.Empty; //CAMPAÑA PORTA+TABLET
            int quitarCampanaX = 0;
            codCampanaX = ConfigurationManager.AppSettings["CampanaDiaEnamoradosAsociada"];
            quitarCampanaX = 1;

 /*CNH 2016-05-06 INI*/
            // Datos PDV x Usuario                      

            BLGeneral objConsulta = new BLGeneral();
            BEUsuarioSession objUsuarioSession = (BEUsuarioSession)Session["Usuario"];
            Int64 idUsuario = objUsuarioSession.idUsuarioSisact;
            string constTipoProductoCON = ConfigurationManager.AppSettings["constTipoProductoCON"];
            List<BEPuntoVenta> lstOficina = objConsulta.ConsultaPDVUsuario(idUsuario, constTipoProductoCON);
            //List<BEItemGenerico> lstTipoOficina = objConsulta.ConsultaTipoOficinaUsuario(idUsuario, constTipoProductoCON);
            // Listado de PDVs
            string strListaOficina = string.Empty;
            bool bolTV = false;


            foreach (BEPuntoVenta obj in lstOficina)
            {
                //MT,TV
                if (ConfigurationManager.AppSettings["constCodCodCanalTeleventa"].Contains(obj.CanacCodigo))
                {
                    bolTV = true;
                    break;
                }
            }


            Int64 intCodParam = Convert.ToInt64(ConfigurationManager.AppSettings["CodigoParamMigraCamapana"]);
            List<BEParametro> lstParametro = objConsulta.ListaParametros(intCodParam);
            if (lstParametro.Count > 0)
            {
                string strParaValor = "";
                foreach (BEParametro obj in lstParametro) strParaValor = obj.Valor;
                string[] ParamValors = strParaValor.Split(';');
                /*Condiciones de Venta Codigo*/
                string strConVent = ParamValors[0];
                string[] ValuesConVent = strConVent.Split('|');
                ValuesConVent = ValuesConVent.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                /*Campañas Codigo*/
                string strCamp = ParamValors[1];
                string[] ValuesCampVent = strCamp.Split('|');
                ValuesCampVent = ValuesCampVent.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                /*MIGRACION;MASIVO;CHIP SUELTO*/
                if (objForm.idTipoOperacion == ValuesConVent[0] && objForm.idOferta == ValuesConVent[1] && objForm.modalidadVenta == ValuesConVent[2])
                {
                    /*VALIDAR CAMPAÑA CLARO CONEXIÓN 29 MIGRA*/
                    if (bolTV == false)
                    {
                        //- Canal de venta: diferente a call center (CAC)                      
                        objLista = objLista.Where(o => !ValuesCampVent.Contains(o.Codigo)).ToList();

                }
                else
                {
                        //El Asesor al ingresa los datos del cliente diferente a DNI.
                        if (objForm.tipoDocumento != ConfigurationManager.AppSettings["constCodTipoDocumentoDNI"].ToString())
                    {
                            objLista = objLista.Where(o => !ValuesCampVent.Contains(o.Codigo)).ToList();
                        }
                    }
                }
            }
            /*CNH 2016-05-06 FIN*/

            //PROY-140439 BRMS CAMPAÑA NAVIDEÑA::INI
            //Llamar al metodo validacion de parametrica
            objLog.CrearArchivolog("[INICIO][ListarCampana][Validacion Campañas BRMS]", null, null);
            string[] campNavidad = (string[])HttpContext.Current.Session["CampanasNavidad"];
            if (campNavidad != null)
            {
                if (campNavidad.Length > 0)
                {
                    for (int i = 0; i < campNavidad.Length; i++)
                    {
                        objLog.CrearArchivolog(string.Format("{0} => {1}", "[Validacion Campañas BRMS] - Listado Campañas Restricción", Funciones.CheckStr(campNavidad[i])), null, null);
                        string descCamp = campNavidad[i];
						//INICIO - CAMBIOS 21092020
                        objLista = objLista.Where(o => !descCamp.Equals(o.Descripcion)).ToList();
                        //objLista = objLista.Where(o => !descCamp.Contains(o.Descripcion)).ToList();
                        //FIN - CAMBIOS 21092020
                    }

                }
                else
                {
                    objLog.CrearArchivolog(string.Format("{0} => {1}", "[Validacion Campañas BRMS] - Listado Campañas Restricción", "Cliente no tiene campañas restringidas"), null, null);
                }
            }
            else
            {
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[Validacion Campañas BRMS] - Listado Campañas Restricción", "Cliente no tiene campañas restringidas"), null, null);
            }
            HttpContext.Current.Session["CampanasNavidad"] = null;
            objLog.CrearArchivolog("[FIN][ListarCampana][Validacion Campañas BRMS]", null, null);
            //PROY-140439 BRMS CAMPAÑA NAVIDEÑA::FIN

            //INC000002510501 - INI
            objLog.CrearArchivolog("[INICIO][ListarCampana][INC000002510501]", null, null);
            if (Funciones.CheckStr(ReadKeySettings.Key_R_Flag) == "1")
            {
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[ListarCampana][INC000002510501]", "[Flag encendido]"), null, null);
                string modalidadPorta = Funciones.CheckStr(hidModalidadPortabi.Value);
                string operadorPorta = Funciones.CheckStr(hidOperadorPortabi.Value);
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[ListarCampana][INC000002510501][Modalidad Portabilidad]", modalidadPorta), null, null);
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[ListarCampana][INC000002510501][Operador Portabilidad]", operadorPorta), null, null);
                if (!string.IsNullOrEmpty(modalidadPorta) && !string.IsNullOrEmpty(operadorPorta) && ReadKeySettings.Key_R_Modalidad.IndexOf(modalidadPorta) > -1 && ReadKeySettings.Key_R_Operador.IndexOf(operadorPorta) > -1)
                {
                    objLog.CrearArchivolog(string.Format("{0} => {1}", "[ListarCampana][INC000002510501]", "Cumple condiciones de tabla de Parametros"), null, null);
                    string desCampana = Funciones.CheckStr(ReadKeySettings.Key_R_Campana);
                    if (!string.IsNullOrEmpty(desCampana))
                    {
                        objLog.CrearArchivolog(string.Format("{0} => {1}", "[ListarCampana][INC000002510501]", "Si tiene campañas cargadas en tabla de parametros"), null, null);
                        string[] desCampIni = desCampana.Split('|');
                        if (desCampIni.Length > 0)
                        {
                            for (int i = 0; i < desCampIni.Length; i++)
                            {
                                objLog.CrearArchivolog(string.Format("{0} => {1}", "[ListarCampana][INC000002510501][Campaña]", Funciones.CheckStr(desCampIni[i])), null, null);
                                string descCamp = desCampIni[i];
								//INICIO - CAMBIOS 21092020
                                objLista = objLista.Where(o => !descCamp.Equals(o.Descripcion)).ToList();
                                //objLista = objLista.Where(o => !descCamp.Contains(o.Descripcion)).ToList();
                                //FIN - CAMBIOS 21092020
                            }
                        }
                        else
                        {
                            objLog.CrearArchivolog(string.Format("{0} => {1}", "[ListarCampana][INC000002510501]", "Cliente no tiene campañas restringidas"), null, null);
                        }
                    }
                    else
                    {
                        objLog.CrearArchivolog(string.Format("{0} => {1}", "[ListarCampana][INC000002510501]", "Cliente no tiene campañas restringidas"), null, null);
                    }
                }
                else 
                {
                    objLog.CrearArchivolog(string.Format("{0} => {1}", "[ListarCampana][INC000002510501]", "Cliente no cumple con validaciones de parametros"), null, null);
                }
            }
            else 
            {
                objLog.CrearArchivolog(string.Format("{0} => {1}","[ListarCampana][INC000002510501]","[Flag apagado]"), null, null);
            }
            objLog.CrearArchivolog("[FIN][ListarCampana][INC000002510501]", null, null);
            //INC000002510501 - FIN

            if (!blVentVarias)//PROY-140743 - INI
            {
            try
            {
                objLog.CrearArchivolog("INICIATIVA 888 - Beneficio Lineas Adicionales (ListarCampana) [Las lineas SI cumplen el requisito del cargo fijo]", null, null);

            bool blMostraCampanas = (bool)HttpContext.Current.Session["blMostrarCampanasDescuento"];
            objLog.CrearArchivolog(String.Format("{0} --> {1}", "[INICIATIVA 888 - Beneficio Lineas Adicionales (ListarCampana)][blMostraCampanas]", Funciones.CheckStr(blMostraCampanas)), null, null);

                string strFlagGeneralCampanasDcto = ReadKeySettings.Key_FlagGeneralCampanasDcto;
                string strFlagOcultarCampanasRegulares = ReadKeySettings.Key_FlagOcultarCampanasRegulares;

                objLog.CrearArchivolog(String.Format("{0} --> {1}", "[INICIATIVA 888 - Beneficio Lineas Adicionales (ListarCampana)][strFlagGeneralCampanasDcto]", Funciones.CheckStr(strFlagGeneralCampanasDcto)), null, null);
                 objLog.CrearArchivolog(String.Format("{0} --> {1}", "[INICIATIVA 888 - Beneficio Lineas Adicionales (ListarCampana)][strFlagOcultarCampanasRegulares]", Funciones.CheckStr(strFlagOcultarCampanasRegulares)), null, null);
                objLog.CrearArchivolog(String.Format("{0} --> {1}", "[INICIATIVA 888 - Beneficio Lineas Adicionales (ListarCampana)][ReadKeySettings.Key_CampanasDscto]", Funciones.CheckStr(ReadKeySettings.Key_CampanasDscto)), null, null);

                if (strFlagGeneralCampanasDcto == "1")
                {
                    objLog.CrearArchivolog("INICIATIVA 888 - Beneficio Lineas Adicionales (ListarCampana) [Flag Encendido]", null, null);

                    objLog.CrearArchivolog(String.Format("{0} --> {1}", "[INICIATIVA 888 - Beneficio Lineas Adicionales (ListarCampana)][objForm.idTipoOperacion]", Funciones.CheckStr(objForm.idTipoOperacion)), null, null);
                    objLog.CrearArchivolog(String.Format("{0} --> {1}", "[INICIATIVA 888 - Beneficio Lineas Adicionales (ListarCampana)][objForm.flgPorta]", Funciones.CheckStr(objForm.flgPorta)), null, null);
                    objLog.CrearArchivolog(String.Format("{0} --> {1}", "[INICIATIVA 888 - Beneficio Lineas Adicionales (ListarCampana)][objForm.modalidadVenta]", Funciones.CheckStr(objForm.modalidadVenta)), null, null);
                    objLog.CrearArchivolog(String.Format("{0} --> {1}", "[INICIATIVA 888 - Beneficio Lineas Adicionales (ListarCampana)][objForm.idOferta]", Funciones.CheckStr(objForm.idOferta)), null, null);

                        BEItemGenerico objCampanaDcto = new BEItemGenerico();

                        if (!String.IsNullOrEmpty(ReadKeySettings.Key_CampanasDscto))
                    {
                            List<BEItemGenerico> objNew = new List<BEItemGenerico>();

                            string[] valuesCampanas = ReadKeySettings.Key_CampanasDscto.Split('|').Where(x => !string.IsNullOrEmpty(x)).ToArray();

                        if (blMostraCampanas)
                        {

                            objNew = objLista.Where(o => valuesCampanas.Contains(o.Codigo)).ToList();

                            if (objNew.Count() > 0)
                            {
                                objLog.CrearArchivolog("INICIATIVA 888 - Beneficio Lineas Adicionales (ListarCampana) [Campanas de descuento SI existen en el flujo regular]", null, null);

                                if (strFlagOcultarCampanasRegulares == "1")
                        {
                                    objLog.CrearArchivolog("INICIATIVA 888 - Beneficio Lineas Adicionales (ListarCampana) [Campanas de descuento reemplazan a las existentes]", null, null);
                            objLista = new List<BEItemGenerico>();
                                    objLista.AddRange(objNew);
                        }
                                else
                        {
                                    objLog.CrearArchivolog("INICIATIVA 888 - Beneficio Lineas Adicionales (ListarCampana) [Campanas de descuento se adicionan a las ya existentes]", null, null);
                                    objLista.AddRange(objNew);
                                }
                        }
                        else
                        {
                                objLog.CrearArchivolog("INICIATIVA 888 - Beneficio Lineas Adicionales (ListarCampana) [Campanas de descuento NO existen en el flujo regular]", null, null);
                        }

                    }
                    else
                    {
                            objLog.CrearArchivolog("INICIATIVA 888 - Beneficio Lineas Adicionales (ListarCampana) [Las lineas NO cumplen el requisito del cargo fijo]", null, null);
                            objLog.CrearArchivolog("INICIATIVA 888 - Beneficio Lineas Adicionales (ListarCampana) [Se quitan las campanas de descuento]", null, null);

                            objLista = objLista.Where(o => !valuesCampanas.Contains(o.Codigo)).ToList();

                        }
                    }
                    else
                    {
                            objLog.CrearArchivolog("INICIATIVA 888 - Beneficio Lineas Adicionales (ListarCampana) [No existente campanas de descuento]", null, null);
                    }
                }
                else
                {
                    objLog.CrearArchivolog("INICIATIVA 888 - Beneficio Lineas Adicionales (ListarCampana) [Flag General Campanas Dcto Apagado]", null, null);
                }
            }
            catch (Exception)
            {

                objLog.CrearArchivolog("INICIATIVA 888 - Beneficio Lineas Adicionales (ListarCampana) [Ocurrio un error al validar las campanas de descuento]", null, null);
            }
            }//PROY-140743


            //PROY-24740
            StringBuilder sbCampanas= new StringBuilder();
            foreach (BEItemGenerico obj in objLista)
            {
                if (quitarCampanaX == 0)
                {                    
                    sbCampanas.Append("|");
                    sbCampanas.Append(obj.Codigo);
                    sbCampanas.Append(";");
                    sbCampanas.Append(obj.Descripcion);
                }
                else
                {
                    if (obj.Codigo != codCampanaX)
                    {                        
                        sbCampanas.Append("|");
                        sbCampanas.Append(obj.Codigo);
                        sbCampanas.Append(";");
                        sbCampanas.Append(obj.Descripcion);
            }
            }
            }
            return sbCampanas.ToString();
            //PROY-24740
        }

        private string LlenarPlazo(BEFormEvaluacion objForm)
        {
            //PROY-24740
            GeneradorLog objLog = new GeneradorLog(CurrentUser,objForm.nroDocumento.ToString(),null,"WEB");
            objLog.CrearArchivolog("[Inicio][LlenarPlazo]",null,null);
            StringBuilder sbPlazo = new StringBuilder();
            List<BEItemGenerico> objLista = new BLGeneral_II().ListarPlazo(objForm.idProducto, objForm.idCasoEspecial, objForm.modalidadVenta);

            //INICIATIVA 920 
            string[] cuotasSinCode = AppSettings.Key_CuotaSinCode_Permitido.Split('|');

            if (objForm.modalidadVenta == "5")
            {
                foreach (BEItemGenerico obj in objLista)
                {
                    foreach (string cuota in cuotasSinCode)
                    {
                        sbPlazo.Append("|");
                        sbPlazo.Append(obj.Codigo + "-" + cuota);
                        sbPlazo.Append(";");
                        sbPlazo.Append(obj.Descripcion + " - cuotas " + cuota);
                    }
                }
            }
            else
            {
                //PROY-140743 - INICIO
                if (objForm.idTipoOperacion == "25")
                { 
                    objLista = (from p in objLista
                                where Funciones.CheckStr(ReadKeySettings.Key_PlazoAccCuota).Contains(p.Codigo)
                                     select p).ToList();
                }
                //PROY-140743 - FIN

                foreach (BEItemGenerico obj in objLista)
                {
                    sbPlazo.Append("|");
                    sbPlazo.Append(obj.Codigo);
                    sbPlazo.Append(";");
                    sbPlazo.Append(obj.Descripcion);
                }
            }
            objLog.CrearArchivolog("[RESULTADO]", sbPlazo.ToString(), null);
            objLog.CrearArchivolog("[Fin][LlenarPlazo]", null, null);
            return sbPlazo.ToString();
            //PROY-24740
        }

        private string LlenarPlan(BEFormEvaluacion objForm)
        {
// [INC000002442213]
            GeneradorLog objLog = new GeneradorLog(CurrentUser, objForm.nroDocumento.ToString(), null, "WEB");
            objLog.CrearArchivolog("[Inicio][LlenarPlan]", null, null);
           // [INC000002442213]
            string strFiltro = string.Empty;
            bool flgOK = true;
            string idPlazo = "";
            //gaa20161020
            //List<BEPlan> objLista = new BLGeneral_II().ListarPlanTarifario(objForm.idFlujo, objForm.tipoDocumento, objForm.idOferta, objForm.idTipoOperacion, objForm.idProducto,
            //                                                                objForm.idCasoEspecial, objForm.idCampana, objForm.idPlazo, objForm.idOficina, objForm.idCombo, ref strFiltro);

            string strFxD = ConfigurationManager.AppSettings["FamiliaPlanxDefecto"];
            string strFamiliaPlanOferta = ConfigurationManager.AppSettings["FamiliaPlanOferta"];
            string strFamiliaPlanProducto = ConfigurationManager.AppSettings["FamiliaPlanProducto"];

            if (objForm.idOferta == strFamiliaPlanOferta && objForm.idProducto == strFamiliaPlanProducto && objForm.idFamiliaPlan == null)
                objForm.idFamiliaPlan = strFxD;
            else
            {
                if (objForm.idFamiliaPlan == null)
                    objForm.idFamiliaPlan = string.Empty;
            }
            //INICIATIVA 920 - VALIDACION DE PLAZOS
            if (objForm.modalidadVenta == "5")
                idPlazo = "00";//el plazo por tiempo ilimitado es único
            else
                idPlazo = objForm.idPlazo; 

            objLog.CrearArchivolog("[LlenarPlan][objForm.idFlujo]", objForm.idFlujo, null);
            objLog.CrearArchivolog("[LlenarPlan][objForm.tipoDocumento]", objForm.tipoDocumento, null);
            objLog.CrearArchivolog("[LlenarPlan][objForm.idOferta]", objForm.idOferta, null);
            objLog.CrearArchivolog("[LlenarPlan][objForm.idTipoOperacion]", objForm.idTipoOperacion, null);
            objLog.CrearArchivolog("[LlenarPlan][objForm.idProducto]", objForm.idProducto, null);
            objLog.CrearArchivolog("[LlenarPlan][objForm.idCasoEspecial]", objForm.idCasoEspecial, null);
            objLog.CrearArchivolog("[LlenarPlan][objForm.idCampana]", objForm.idCampana, null);
            objLog.CrearArchivolog("[LlenarPlan][objForm.idPlazo]", idPlazo, null);
            objLog.CrearArchivolog("[LlenarPlan][objForm.idOficina]", objForm.idOficina, null);
            objLog.CrearArchivolog("[LlenarPlan][objForm.idCombo]", objForm.idCombo, null);
            objLog.CrearArchivolog("[LlenarPlan][objForm.idFamiliaPlan]", objForm.idFamiliaPlan, null);
            objLog.CrearArchivolog("[LlenarPlan][objForm.evaluarFijo]", objForm.evaluarFijo, null);

            List<BEPlan> objLista = new BLGeneral_II().ListarPlanTarifario(objForm.idFlujo, objForm.tipoDocumento, objForm.idOferta, objForm.idTipoOperacion, objForm.idProducto,
                                                                objForm.idCasoEspecial, objForm.idCampana, idPlazo, objForm.idOficina, objForm.idCombo, objForm.idFamiliaPlan, ref strFiltro);
            //fin gaa20161020
            //PROY-24740
            StringBuilder sbPlan = new StringBuilder();
            if (strFiltro == "S")
            {
                List<BEItemGenerico> objListaFiltro = (List<BEItemGenerico>)Session["FiltroPlanxPdv_" + objForm.idOficina];
                objLista = (from plan in objLista
                            join filtro in objListaFiltro on plan.PLANC_CODIGO equals filtro.Codigo
                            select plan).OrderBy(x => x.PLANV_DESCRIPCION).ToList<BEPlan>();
            }


//PROY-29121-INI
                if (objForm.evaluarFijo == "T") // Validación de Planes de Telefonía Fija
                {
               
                List<BEItemGenerico> objListaCodSoloPlanFijo = new BLGeneral_II().ListarPlanTelFija(objForm.idProducto);
                objLista = (from plan in objLista
                            join planFijo in objListaCodSoloPlanFijo on plan.PLANC_CODIGO equals planFijo.Codigo
                            select plan).ToList();

                }
            //PROY-29121-FIN


            objLista = objLista.OrderByDescending(x => x.PLANN_CAR_FIJ).ToList(); //PROY-140439

            //[INC000002442213]INC FALLA CARGO FIJO INI
            objLog.CrearArchivolog("[INC000002442213]----------------------------------------------------------------{fraude}", null, null);
            Session["objplan" + objForm.idFila] = objLista;
            objLog.CrearArchivolog("[INC000002442213][RESULTADO OBJ: ]", objLista.Count, null);
            objLog.CrearArchivolog("[INC000002442213][Fin][LlenarPlan]", null, null);
            //[INC000002442213]INC FALLA CARGO FIJO FIN
            foreach (BEPlan obj in objLista)
            {
               

                if (flgOK)
                {
                    //PROY-24740
                    sbPlan.Append("|");                    
                    sbPlan.Append(obj.PLANC_CODIGO);
                    sbPlan.Append("_");
                    sbPlan.Append(obj.PLANN_CAR_FIJ);
                    sbPlan.Append("_");
                    sbPlan.Append(obj.PLANC_EQUI_SAP);
                    sbPlan.Append("_");
                    sbPlan.Append(obj.PLNN_TIPO_PLAN);
                    sbPlan.Append("_");
                    sbPlan.Append(string.Empty);
                    sbPlan.Append("_");
                    sbPlan.Append(obj.GPLNV_DESCRIPCION);
                    sbPlan.Append("_");
                    sbPlan.Append(obj.CODIGO_BSCS);
                    sbPlan.Append("_");
                    sbPlan.Append(obj.TIPO_PRODUCTOS);
                    sbPlan.Append("_");
                    sbPlan.Append(obj.PRDC_CODIGO);
                    sbPlan.Append(";");
                    sbPlan.Append(obj.PLANV_DESCRIPCION);
                }
            }
            return sbPlan.ToString();
            //PROY-24740
        }

        //PROY-24740
        private string LlenarServicio(BEFormEvaluacion objForm)
        {
            GeneradorLog objLog = new GeneradorLog(CurrentUser, objForm.nroDocumento.ToString(), null, "WEB");//INC000002245048


            objLog.CrearArchivolog("[LlenarServicio]-[INC000003848031- objForm.idProducto]", Funciones.CheckStr(objForm.idProducto), null); //INC000003848031
            objLog.CrearArchivolog("[LlenarServicio]-[INC000003848031 - objForm.idPlan]", Funciones.CheckStr(objForm.idPlan), null); //INC000003848031
            objLog.CrearArchivolog("[LlenarServicio]-[INC000003848031 -  objForm.idCasoEspecial]", Funciones.CheckStr(objForm.idCasoEspecial), null); //INC000003848031


            List<BESecServicio_AP> objLista = new BLGeneral_II().ListarServiciosXPlan(objForm.idProducto, objForm.idPlan);



            List<BESecServicio_AP> objListaTopeConfig = new BLGeneral_II().ListarPlanTopeConfig(objForm.idPlan, objForm.idCasoEspecial);

            StringBuilder sbServicioSI = new StringBuilder();
            StringBuilder sbServicioNO = new StringBuilder();

            objLog.CrearArchivolog("[LlenarServicio]-[INC000003848031 -  objLista.count]", Funciones.CheckStr(objLista.Count), null); //INC000003848031
            objLog.CrearArchivolog("[LlenarServicio]-[INC000003848031 -  objListaTopeConfig.count]", Funciones.CheckStr(objListaTopeConfig.Count), null); //INC000003848031

            foreach (BESecServicio_AP obj in objLista)
            {
               

                foreach (BESecServicio_AP objTope in objListaTopeConfig)
                {
                    if (obj.SERVV_CODIGO == objTope.SERVV_CODIGO)
                    {
                        obj.SELECCIONABLE_BASE = objTope.SELECCIONABLE_BASE;
                        break;
                    }
                }

                objLog.CrearArchivolog("[LlenarServicio]-[INC000003848031 -  obj.SELECCIONABLE_BASE]", Funciones.CheckStr(obj.SELECCIONABLE_BASE), null); //INC000003848031
                objLog.CrearArchivolog("[LlenarServicio]-[INC000003848031 -  obj.SERVV_CODIGO]", Funciones.CheckStr(obj.SERVV_CODIGO), null); //INC000003848031
                objLog.CrearArchivolog("[LlenarServicio]-[INC000003848031 -  obj.PLNV_CODIGO]", Funciones.CheckStr(obj.PLNV_CODIGO), null);    //INC000003848031


                if (Funciones.CheckInt(obj.SELECCIONABLE_BASE) == (int)BESecServicio_AP.TIPO_SELECCION.SELECCIONADO || 
              Funciones.CheckInt(obj.SELECCIONABLE_BASE) == (int)BESecServicio_AP.TIPO_SELECCION.SELECCIONADO_EDITABLE)
                {
                    sbServicioSI.Append("|");
                    sbServicioSI.Append(obj.SERVN_ORDEN);
                    sbServicioSI.Append("_");
                    sbServicioSI.Append(obj.GSRVC_CODIGO);
                    sbServicioSI.Append("_");
                    sbServicioSI.Append(obj.SELECCIONABLE_BASE);
                    sbServicioSI.Append("_");
                    sbServicioSI.Append(obj.SERVV_CODIGO);
                    sbServicioSI.Append("_");
                    sbServicioSI.Append(obj.CARGO_FIJO_BASE);
                    sbServicioSI.Append("_"); //PROY-31812 - IDEA-43340
                    sbServicioSI.Append(obj.GSRVV_DESCRIPCION); //PROY-31812 - IDEA-43340
                    sbServicioSI.Append(";");
                    sbServicioSI.Append("(*) ");
                    sbServicioSI.Append(obj.SERVV_DESCRIPCION);


                    objLog.CrearArchivolog("[INC000002442213]****************************************************************{fraude}", null, null);
                    objLog.CrearArchivolog("[INC000002442213]objplan + objForm.CARGO_FIJO_BASE" + obj.CARGO_FIJO_BASE, null, null);
                    objLog.CrearArchivolog("[INC000002442213]objplan + objForm.CARGO_FIJO_EN_PAQUETE" + obj.CARGO_FIJO_EN_PAQUETE, null, null);
                    objLog.CrearArchivolog("[INC000002442213]objplan + objForm.CARGO_FIJO_EN_SEC" + obj.CARGO_FIJO_EN_SEC, null, null);
              
                    

                    objLog.CrearArchivolog("[INC000002442213]objplan + objForm.idFila" + objForm.idFila, null, null);
                    List<BEPlan> objPLan = (List<BEPlan>)HttpContext.Current.Session["objplan" + Funciones.CheckStr(objForm.idFila)];
                    for (int i = 0; i <= objPLan.Count - 1; i++)
                    {

                        objLog.CrearArchivolog("[INC000002442213]obj.SERVV_CODIGO - objPLan[i].SERV_CODIGO:" + "-" + obj.SERVV_CODIGO + "-" + objPLan[i].SERV_CODIGO, null, null);
                        objLog.CrearArchivolog("[INC000002442213]obj.SERVV_CODIGO - objPLan[i].SERV_CODIGO:" + "-" + obj.PLNV_CODIGO + "-" + objPLan[i].PLANN_CODIGO, null, null);


                        if (obj.PLNV_CODIGO == objPLan[i].PLANN_CODIGO)
                        {

                            objLog.CrearArchivolog("[INC000002442213]objplan + objForm.idFila - ENTRO:" + i + "-" + obj.CARGO_FIJO_BASE, null, null);

                            if (obj.CARGO_FIJO_BASE > 0)
                                objPLan[i].PLANN_CAR_FIJ = obj.CARGO_FIJO_BASE;
                            else
                                objLog.CrearArchivolog("[INC000002442213]obj.CARGO_FIJO_BASE: IGUAL A 0" + obj.CARGO_FIJO_BASE, null, null);

                            
                        }


                    }

                    HttpContext.Current.Session["objplan" + Funciones.CheckStr(objForm.idFila)] = objPLan;


                }
                else
                {
                    if (obj.SERVV_CODIGO != ConfigurationManager.AppSettings["codServRoamingI"] ||
                        (obj.SERVV_CODIGO == ConfigurationManager.AppSettings["codServRoamingI"] && strFlagServicioRI == ConfigurationManager.AppSettings["constFlagRIActivo"]))
                    {
                        sbServicioNO.Append("|");
                        sbServicioNO.Append(obj.SERVN_ORDEN);
                        sbServicioNO.Append("_");
                        sbServicioNO.Append(obj.GSRVC_CODIGO);
                        sbServicioNO.Append("_");
                        sbServicioNO.Append(obj.SELECCIONABLE_BASE);
                        sbServicioNO.Append("_");
                        sbServicioNO.Append(obj.SERVV_CODIGO);
                        sbServicioNO.Append("_");
                        sbServicioNO.Append(obj.CARGO_FIJO_BASE);
                        sbServicioNO.Append("_"); //PROY-31812 - IDEA-43340
                        sbServicioNO.Append(obj.GSRVV_DESCRIPCION); //PROY-31812 - IDEA-43340                        
                        sbServicioNO.Append(";");
                        sbServicioNO.Append(obj.SERVV_DESCRIPCION);
                    }
                }
            }
            //INI-INC000002245048
            objLog.CrearArchivolog("[LlenarServicio]-[sbServicioSI]", sbServicioSI.ToString(), null);
            objLog.CrearArchivolog("[LlenarServicio]-[sbServicioNO]", sbServicioNO.ToString(), null);
            //FIN-INC000002245048
            return string.Format("{0}¬{1}", sbServicioNO.ToString(), sbServicioSI.ToString());
            //PROY-24740
        }

        private string LlenarEquipoPrecio(BEFormEvaluacion objForm)
        {
            GeneradorLog objLog = new GeneradorLog(CurrentUser, objForm.nroDocumento.ToString(), null, "BUYBACK");//140736
            string strTipoVenta = ConfigurationManager.AppSettings["strTVPostpago"].ToString();
            string strTipoOperacion =  objForm.idTipoOperacion;           
            List<BEItemGenerico> arrListaPrecio = new List<BEItemGenerico>();
            List<BEItemGenerico> arrListaPrecioBuyBack = new List<BEItemGenerico>();

            string strResultado = string.Empty;

            DataTable dtGama = (new BLGeneral_II()).ListarEquipoGama();
            foreach (DataRow dr in dtGama.Rows)
            {
                if (dr["eqgac_codigo"].ToString() == objForm.idMaterial)
                {
                        strResultado = string.Format("{0}_{1}_{2}_{3}", dr["eqgan_precio"], dr["eqgac_codigo"], dr["eqgav_descripcion"], dr["eqgan_costo"]);
                    return strResultado;
                }
            }

            List<BEConsultarPrecioBase> oListConsultaPrecioBase = BLSincronizaSap.ConsultarPrecioBase(objForm.idMaterial, null);
            double dblCosto = 0, dblCostoTmp = 0;
            foreach (BEConsultarPrecioBase item in oListConsultaPrecioBase)
            {
                dblCostoTmp = Funciones.CheckDbl(item.PrecioCompra);
                if (dblCostoTmp > dblCosto)
                {
                    dblCosto = dblCostoTmp;
                }
            }

            string strCosto = Funciones.CheckStr(dblCosto);
            arrListaPrecio = ObtenerListaPrecioPVUDB(objForm);

            //PROY-140736 ini
            objLog.CrearArchivolog(string.Format("[LlenarEquipoPrecio]-[BUYBACK] {0}", objForm.idCampana), null, null);
            objLog.CrearArchivolog("[LlenarEquipoPrecio]-[ReadKeySettings.Key_CodCampaniaBuyBack]", ReadKeySettings.Key_CodCampaniaBuyBack, null);
            string fila = objForm.idFila.ToString();
            string [] arrcampBuyback = Funciones.CheckStr(ReadKeySettings.Key_CodCampaniaBuyBack).Split('|');
            bool isBuyback = false;
            for (int i = 0; i < arrcampBuyback.Length; i++)
                {
                if (objForm.idCampana == arrcampBuyback[i])
                    {
                    isBuyback = true;
                    break;
                    }
                       
                    }
            objLog.CrearArchivolog(string.Format("[LlenarEquipoPrecio]-[isBuyback] [{0}]", isBuyback),null, null);
            if (isBuyback)
            {
               
                objLog.CrearArchivolog(string.Format("[LlenarEquipoPrecio]-[strmaterialbuyback] [{0}]", objForm.strmaterialbuyback), null, null);

                arrListaPrecioBuyBack = ObtenerListaPrecioBuyBack(objForm); //codmaterial Buyback 140736

                
                
                List<BEItemGenerico> objListBuy = (from lp in arrListaPrecio
                                                   join pb in arrListaPrecioBuyBack on lp.Codigo equals pb.Codigo
                                                         select new BEItemGenerico(lp.Codigo, lp.Descripcion,lp.Monto)).GroupBy(g => g.Codigo).Select(s => s.First()).ToList<BEItemGenerico>();


                objLog.CrearArchivolog(string.Format("[LlenarEquipoPrecio]-[objListBuy.Count][{0}]", objListBuy.Count),null, null);
                if (objListBuy.Count<=0)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), new Guid().ToString(), "EliminarBuyBack('"+ fila + "');", true);
                    throw new Exception(Funciones.CheckStr(ReadKeySettings.Key_Msj_Error_LP_BuyBack));
                   
                }

                arrListaPrecio = objListBuy;
            }
            //PROY-140736 fin
            if (arrListaPrecio.Count == 0)
            {
                //INICIATIVA 920
                if (objForm.modalidadVenta != strCodModalidadCuota && objForm.modalidadVenta != strCodModalidadCuotaSinCode)
                    throw new Exception(ConfigurationManager.AppSettings["constMsjErrorConfigListaPrecio"].ToString());
            }
            else
            {
                decimal dblPrecio = 0;
                string strCodLPrecio = null;
                string strDesLPrecio = null;
                decimal dblPrecioFinal = 0;
                decimal dblPrecioMenor = 999999;
                decimal dblPrecioMayor = 0;
                BEItemGenerico objLP = null;

                foreach (BEItemGenerico item in arrListaPrecio)
                {
                    dblPrecio = Funciones.CheckDecimal(item.Monto);
                    //INICIATIVA 920
                    if (objForm.modalidadVenta != strCodModalidadCuota && objForm.modalidadVenta != strCodModalidadCuotaSinCode)
                    {
                    if (dblPrecio < dblPrecioMenor)
                    {
                        dblPrecioMenor = dblPrecio;
                        strCodLPrecio = Funciones.CheckStr(item.Codigo);
                        strDesLPrecio = Funciones.CheckStr(item.Descripcion);
                        dblPrecioFinal = dblPrecio;

                        objLP = item;
                    }
                }
                    else
                    {
                        if (dblPrecio > dblPrecioMayor)
                        {
                            dblPrecioMayor = dblPrecio;
                            strCodLPrecio = Funciones.CheckStr(item.Codigo);
                            strDesLPrecio = Funciones.CheckStr(item.Descripcion);
                            dblPrecioFinal = dblPrecio;

                            objLP = item;
                        }
                    }
                }

                //gaa20160211
                string lpNew = string.Empty;

                if (objForm.idCampanaSap == ConfigurationManager.AppSettings["CampanaDiaEnamorados"])
                {
                    objForm.idCampanaSap = ConfigurationManager.AppSettings["CampanaDiaEnamoradosAsociada"];
                    lpNew = LlenarEquipoPrecio2(objForm);

                    strResultado = string.Format("{0}_{1}_{2}_{3}|{4}", dblPrecioFinal, strCodLPrecio, strDesLPrecio, dblCosto.ToString(), lpNew);
                }
                else
                //fin gaa20160211
                    strResultado = string.Format("{0}_{1}_{2}_{3}", dblPrecioFinal, strCodLPrecio, strDesLPrecio, dblCosto.ToString());

            }

            return strResultado;
        }

        private string LlenarEquipoPrecio2(BEFormEvaluacion objForm)
        {
            string strTipoVenta = ConfigurationManager.AppSettings["strTVPostpago"].ToString();
            string strTipoOperacion = objForm.idTipoOperacion;
            List<BEItemGenerico> arrListaPrecio = new List<BEItemGenerico>();
            string strResultado = string.Empty;

            DataTable dtGama = (new BLGeneral_II()).ListarEquipoGama();
            foreach (DataRow dr in dtGama.Rows)
            {
                if (dr["eqgac_codigo"].ToString() == objForm.idMaterial)
                {
                    strResultado = string.Format("{0}_{1}_{2}_{3}", dr["eqgan_precio"], dr["eqgac_codigo"], dr["eqgav_descripcion"], dr["eqgan_costo"]);
                    return strResultado;
                }
            }

            List<BEConsultarPrecioBase> oListConsultaPrecioBase = BLSincronizaSap.ConsultarPrecioBase(objForm.idMaterial, null);
            double dblCosto = 0, dblCostoTmp = 0;
            foreach (BEConsultarPrecioBase item in oListConsultaPrecioBase)
            {
                dblCostoTmp = Funciones.CheckDbl(item.PrecioCompra);
                if (dblCostoTmp > dblCosto)
                {
                    dblCosto = dblCostoTmp;
                }
            }

            string strCosto = Funciones.CheckStr(dblCosto);
            arrListaPrecio = ObtenerListaPrecioPVUDB(objForm);

            if (arrListaPrecio.Count == 0)
            {
                //INICIATIVA 920
                if (objForm.modalidadVenta != strCodModalidadCuota && objForm.modalidadVenta != strCodModalidadCuotaSinCode)
                    throw new Exception(ConfigurationManager.AppSettings["constMsjErrorConfigListaPrecio"].ToString());
            }
            else
            {
                decimal dblPrecio = 0;
                string strCodLPrecio = null;
                string strDesLPrecio = null;
                decimal dblPrecioFinal = 0;
                decimal dblPrecioMenor = 999999;
                decimal dblPrecioMayor = 0;
                BEItemGenerico objLP = null;

                foreach (BEItemGenerico item in arrListaPrecio)
                {
                    dblPrecio = Funciones.CheckDecimal(item.Monto);
                    //INICIATIVA 920
                    if (objForm.modalidadVenta != strCodModalidadCuota && objForm.modalidadVenta != strCodModalidadCuotaSinCode)
                    {
                        if (dblPrecio < dblPrecioMenor)
                        {
                            dblPrecioMenor = dblPrecio;
                            strCodLPrecio = Funciones.CheckStr(item.Codigo);
                            strDesLPrecio = Funciones.CheckStr(item.Descripcion);
                            dblPrecioFinal = dblPrecio;

                            objLP = item;
                        }
                    }
                    else
                    {
                        if (dblPrecio > dblPrecioMayor)
                        {
                            dblPrecioMayor = dblPrecio;
                            strCodLPrecio = Funciones.CheckStr(item.Codigo);
                            strDesLPrecio = Funciones.CheckStr(item.Descripcion);
                            dblPrecioFinal = dblPrecio;

                            objLP = item;
                        }
                    }
                }

                strResultado = string.Format("{0}_{1}_{2}_{3}", dblPrecioFinal, strCodLPrecio, strDesLPrecio, dblCosto.ToString());
            }

            return strResultado;
        }

        private string LlenarMontoTopeConsumo(BEFormEvaluacion objForm)
        {
            string strResultado = string.Empty;

            //INI: INICIATIVA-219
            GeneradorLog _objLog = new GeneradorLog(CurrentUser, "", null, "LlenarMontoTopeConsumo");
            String flagCBIO = Funciones.CheckStr(HttpContext.Current.Session["flagCBIO"]);
            String WhiteListFlagCBIO = Funciones.CheckStr(HttpContext.Current.Session["WhiteListFlagCBIO"]);
            string codBSCS = string.Empty;

            _objLog.CrearArchivolog("INICIATIVA-219 | FlagCBIO         : " + flagCBIO, null, null);
            _objLog.CrearArchivolog("INICIATIVA-219 | WhiteListFlagCBIO: " + WhiteListFlagCBIO, null, null);
            _objLog.CrearArchivolog("INICIATIVA-219 | idPlan: " + Funciones.CheckStr(objForm.idPlan) , null, null);

            if (flagCBIO == "1" && WhiteListFlagCBIO == "1")
            {
                _objLog.CrearArchivolog("INICIATIVA-219 | Metodo: ListarTopeAutomaticoCBIO()  ", null, null);
                codBSCS = ObtenerPlanBSCS(Funciones.CheckStr(objForm.idProducto), Funciones.CheckStr(objForm.idPlan));
                _objLog.CrearArchivolog("INICIATIVA-219 | codBSCS: " + Funciones.CheckStr(codBSCS), null, null);

                if (codBSCS != "")
                {
                    BEItemGenerico objCBIO = new BLGeneral_II().ListarTopeAutomaticoCBIO(codBSCS, 0);//0: consulta por TMCODE, 1: consulta por POID

                _objLog.CrearArchivolog("INICIATIVA-219 | Metodo: ListarTopeAutomaticoCBIO() |   CodigoRespuesta:" + Funciones.CheckStr(objCBIO.Codigo2), null, null);
                _objLog.CrearArchivolog("INICIATIVA-219 | Metodo: ListarTopeAutomaticoCBIO() |   MsgRespuesta:" + Funciones.CheckStr(objCBIO.Descripcion2), null, null);
                
                if (objCBIO.Codigo2 == "0")
                {
                    strResultado = Funciones.CheckStr(objCBIO.Monto);
                    _objLog.CrearArchivolog("INICIATIVA-219 | Metodo: ListarTopeAutomaticoCBIO() |   MontoTope:" + Funciones.CheckStr(strResultado), null, null);
                }
            }
            }
            else
            {
                _objLog.CrearArchivolog("INICIATIVA-219 | Metodo: ListarTopeAutomatico()  ", null, null);
                //FIN: INICIATIVA-219

            List<BEItemGenerico> objLista = new BLGeneral_II().ListarTopeAutomatico(objForm.idPlan);
            foreach (BEItemGenerico obj in objLista)
            {
                if (obj.Estado == "1")
                {
                    strResultado = Funciones.CheckStr(obj.Monto);
                    break;
                }
            }

            }//FIN: INICIATIVA-219

            return strResultado;
        }

        //INI: INICIATIVA-219
        private string ObtenerPlanBSCS(string strtipoServ, string strcodPlan)
        {
            string codBSCS = string.Empty;
            string codRespuesta = string.Empty;
            string msjRespuesta = string.Empty;
            GeneradorLog _objLog = new GeneradorLog(CurrentUser, "", null, "ObtenerPlanBSCS");
            strtipoServ = ((strtipoServ == Funciones.CheckStr(ConfigurationManager.AppSettings["constTipoProductoMovil"])) ||
                           (strtipoServ == Funciones.CheckStr(ConfigurationManager.AppSettings["constTipoProductoBAM"]))) ? "M" : "F";
            _objLog.CrearArchivolog("INICIATIVA-219 | strtipoServ: " + strtipoServ, null, null);
            _objLog.CrearArchivolog("INICIATIVA-219 | strcodPlan: " + strcodPlan, null, null);

            try
            {
                BEPlan objPlan = new BLGeneral_II().ObtenerPlanBSCS(strtipoServ, strcodPlan, ref codRespuesta, ref msjRespuesta);
                _objLog.CrearArchivolog("INICIATIVA-219 | Metodo: ObtenerPlanBSCS() |   CodigoRespuesta:" + Funciones.CheckStr(codRespuesta), null, null);
                _objLog.CrearArchivolog("INICIATIVA-219 | Metodo: ObtenerPlanBSCS() |   MsgRespuesta:" + Funciones.CheckStr(msjRespuesta), null, null);
                if (codRespuesta == "0")
                {
                    codBSCS = Funciones.CheckStr(objPlan.CODIGO_BSCS);
                    _objLog.CrearArchivolog("INICIATIVA-219 | Metodo: ObtenerPlanBSCS() |   codBSCS:" + Funciones.CheckStr(codBSCS), null, null);
                }
            }
            catch (Exception ex)
            {
                _objLog.CrearArchivolog("[Error][INICIATIVA-219]", null, ex);
            }

            return codBSCS;
        }
        //FIN: INICIATIVA-219

        #endregion [Movil]

        #region [3Play]

        private string LlenarPaquete3Play(BEFormEvaluacion objForm)
        {
            GeneradorLog objLog = new GeneradorLog(CurrentUser,objForm.nroDocumento.ToString(), null, "WEB");
            objLog.CrearArchivolog("[Inicio][LlenarPaquete]",null,null);
            objLog.CrearArchivolog("[CAMPAÑA]", objForm.idCampana.ToString(), null);
            objLog.CrearArchivolog("[PLAZO]", objForm.idPlazo.ToString(), null);
            objLog.CrearArchivolog("[PRODUCTO]", objForm.idProducto.ToString(), null);

            
            List<BEItemGenerico> objLista = new BLGeneral_II().ListarPaquete3Play(objForm.idCombo, objForm.idCampana, objForm.idPlazo,objForm.idProducto);
            //PROY-24740
            StringBuilder sbPaquete3Play = new StringBuilder();
            foreach (BEItemGenerico obj in objLista)
            {
                sbPaquete3Play.Append("|");
                sbPaquete3Play.Append(obj.Codigo);
                sbPaquete3Play.Append(";");
                sbPaquete3Play.Append(obj.Descripcion);
            }
            objLog.CrearArchivolog("[RESULTADO]", sbPaquete3Play.ToString(), null);
            objLog.CrearArchivolog("[Fin][LlenarPaquete3Play]", null, null);
            return sbPaquete3Play.ToString();
            //PROY-24740            
        }

        private string LlenarPlanesXPaquete3Play(BEFormEvaluacion objForm)
        {
            GeneradorLog objLog = new GeneradorLog(CurrentUser,objForm.nroDocumento.ToString(),null,"WEB");
            objLog.CrearArchivolog("[Inicio][LlenarPlanesXPaquete3Play]",null,null);
            objLog.CrearArchivolog("[CAMPAÑA]", objForm.idCampana, null);
            objLog.CrearArchivolog("[PLAZO]", objForm.idPlazo, null);
            objLog.CrearArchivolog("[PAQUETE]", objForm.idPaquete, null);
            objLog.CrearArchivolog("[TIPO_OPER.]", objForm.idTipoOperacion, null);
            objLog.CrearArchivolog("[FLG_PORTA]", objForm.flgPorta, null);
            objLog.CrearArchivolog("[ID_PROD]", objForm.idProducto, null);

            List<BEPlan> objLista = new BLGeneral_II().ListarPlanesXPaquete3Play(objForm.idCombo, objForm.idCampana, objForm.idPlazo, objForm.idPaquete, objForm.idTipoOperacion, objForm.flgPorta, objForm.idProducto);
            
            //PROY-29121-INI
            if (objForm.evaluarFijo == "T")
            {
                List<BEItemGenerico> objListaCodSoloPlanFijo = new BLGeneral_II().ListarPlanTelFija(objForm.idProducto);
                objLista= (from plan in objLista
                           join planFijo in objListaCodSoloPlanFijo on plan.PLANC_CODIGO equals planFijo.Codigo
                           select plan).ToList();

            }
            //PROY-29121-FIN
			//INICIO - PROY-32581
            else if (objForm.idProducto == Funciones.CheckStr(ConfigurationManager.AppSettings["constTipoProducto3PlayInalam"])
                && objForm.flgPorta == "S")
            {
                string pstrGsrvcCodigo = Funciones.CheckStr(ConfigurationManager.AppSettings["GSRVC_CODIGO_LTE"]);
                objLista = new BLGeneral_II().ListarPlanesXPaqueteLTE(objForm.idCombo, objForm.idCampana, objForm.idPlazo, objForm.idPaquete, objForm.idTipoOperacion, objForm.flgPorta, objForm.idProducto, pstrGsrvcCodigo);
            }
			//FIN - PROY-32581
            //PROY-24740
            StringBuilder sbPlanesBSCS = new StringBuilder();
            foreach (BEPlan obj in objLista)
            {
                    sbPlanesBSCS.Append("|");
                    sbPlanesBSCS.Append(obj.PLANC_CODIGO);
                    sbPlanesBSCS.Append(";");
                    sbPlanesBSCS.Append(obj.PLANV_DESCRIPCION);
                }
            objLog.CrearArchivolog("[RESULTADO]", sbPlanesBSCS.ToString(), null);
            objLog.CrearArchivolog("[Fin][LlenarPlanesXPaquete3Play]", null, null);
            return sbPlanesBSCS.ToString();
            //PROY-24740
        }

        private string LlenarServiciosXPlan3Play(BEFormEvaluacion objForm)
        {
            GeneradorLog objLog = new GeneradorLog(CurrentUser,objForm.nroDocumento.ToString(),null,"WEB");
            objLog.CrearArchivolog("[Inicio][LlenarServiciosXPlan3Play]",null,null);
            objLog.CrearArchivolog("[ID_PLAN]", objForm.idPlan.ToString(), null);
            objLog.CrearArchivolog("[ID_PROD.]", objForm.idProducto.ToString(), null);
            List<BEServicioHFC> objLista = new BLGeneral_II().ListarServiciosXPlan3Play(objForm.idPlan, objForm.idProducto);
            //PROY-24740
            StringBuilder sbServiciosXPlan3Play = new StringBuilder();
            foreach (BEServicioHFC obj in objLista)
            {
                sbServiciosXPlan3Play.Append("|");
                sbServiciosXPlan3Play.Append(obj.IdServicio);
                sbServiciosXPlan3Play.Append("_");
                sbServiciosXPlan3Play.Append(obj.CF_Precio);
                sbServiciosXPlan3Play.Append("___");
                sbServiciosXPlan3Play.Append(obj.Grupo);
                sbServiciosXPlan3Play.Append("_");
                sbServiciosXPlan3Play.Append(obj.GrupoDescripcion);
                sbServiciosXPlan3Play.Append("_");
                sbServiciosXPlan3Play.Append(obj.FlagDefecto);
                sbServiciosXPlan3Play.Append("_");
                sbServiciosXPlan3Play.Append(obj.FlagPrincipal);
                sbServiciosXPlan3Play.Append("_");
                sbServiciosXPlan3Play.Append(obj.FlagOpcional);
                sbServiciosXPlan3Play.Append("_");
                sbServiciosXPlan3Play.Append(obj.IdServicio);
                sbServiciosXPlan3Play.Append("_");
                sbServiciosXPlan3Play.Append(obj.FlagVOD);
                sbServiciosXPlan3Play.Append(";");
                sbServiciosXPlan3Play.Append(obj.Servicio);
            }
            objLog.CrearArchivolog("[RESULTADO]", sbServiciosXPlan3Play.ToString(), null);
            objLog.CrearArchivolog("[Fin][LlenarServiciosXPlan3Play]", null, null);
            return sbServiciosXPlan3Play.ToString();
            //PROY-24740
        }

        private string LlenarEquipo3Play(BEFormEvaluacion objForm)
        {
            GeneradorLog objLog = new GeneradorLog(CurrentUser,objForm.nroDocumento.ToString(),null,"WEB");
            objLog.CrearArchivolog("[Inicio][LlenarEquipo3Play]",null,null);
            objLog.CrearArchivolog("[ID_PLAN]", objForm.idPlan.ToString(), null);
            objLog.CrearArchivolog("[ID_PROD.]", objForm.idProducto.ToString(), null);

            string strResultado = string.Format("[{0}]{1}[/{0}]", objForm.idPlan, ListarEquipo3Play(objForm.idPlan,objForm.idProducto,objForm.nroDocumento));
            objLog.CrearArchivolog("[RESULTADO]", strResultado.ToString(), null);
            objLog.CrearArchivolog("[Fin][LlenarEquipo3Play]", null, null);
            return strResultado;
        }

        private string ListarEquipo3Play(string strPlan, string idProducto, string nroDocumento)
        {
            GeneradorLog objLog = new GeneradorLog(CurrentUser, nroDocumento, null, "WEB");
            objLog.CrearArchivolog("[Inicio][ListarEquipo3Play]",null,null);
            objLog.CrearArchivolog("[ID_PROD.]", idProducto.ToString(), null);
            objLog.CrearArchivolog("[PLAN]", strPlan.ToString(), null);
            
            List<BEEquipo> arlResultado = new BLGeneral_II().ListarEquipo3Play(idProducto, strPlan);
            //PROY-24740
            StringBuilder sbEquipo3Play = new StringBuilder();
            foreach (BEEquipo obj in arlResultado)
            {
                sbEquipo3Play.Append("|");
                sbEquipo3Play.Append(obj.NRO_SERIE);
                sbEquipo3Play.Append("_");
                sbEquipo3Play.Append(obj.PRECIO_VENTA);
                sbEquipo3Play.Append("_");
                sbEquipo3Play.Append(obj.GRUPO_MATERIAL);
                sbEquipo3Play.Append(";");
                sbEquipo3Play.Append(obj.EQUIPO_INFO);
            }
            objLog.CrearArchivolog("[RESULTADO]", sbEquipo3Play.ToString(), null);
            objLog.CrearArchivolog("[Fin][ListarEquipo3Play]", null, null);
            return sbEquipo3Play.ToString();
            //PROY-24740
        }

        private string ListarTopesConsumo3Play(string prod) //PROY-29296
        {
            List<BEItemGenerico> objLista = new BLGeneral().ListarTopesConsumoHFC(prod); //PROY-29296
            //PROY-24740
            StringBuilder sbTopesConsumo3Play = new StringBuilder();
            foreach (BEItemGenerico item in objLista)
            {
                sbTopesConsumo3Play.Append("|");
                sbTopesConsumo3Play.Append(item.Codigo);
                sbTopesConsumo3Play.Append(";");
                sbTopesConsumo3Play.Append(item.Descripcion);
            }
            return sbTopesConsumo3Play.ToString();
            //PROY-24740
        }

        #endregion [3Play]

        #region [Corporativo]

        private string LlenarPaquete(BEFormEvaluacion objForm)
        {
            //PROY-24740
            StringBuilder sbPaquete = new StringBuilder();
            if (objForm.idProducto == ConfigurationManager.AppSettings["constTipoProductoMovil"])
            {
                List<BEItemGenerico> arrPaquete = (new BLGeneral_II()).ListarPaquete(objForm.tipoDocumento, objForm.idOferta, objForm.idPlazo);
                foreach (BEItemGenerico obj in arrPaquete)
                {
                    sbPaquete.Append("|");
                    sbPaquete.Append(obj.Codigo);
                    sbPaquete.Append(";");
                    sbPaquete.Append(obj.Descripcion);
                }
            }
            return sbPaquete.ToString();
            //PROY-24740
        }

        private string LlenarPaquetePlan(BEFormEvaluacion objForm)
        {
            //PROY-24740
            return string.Format("{0}¬{1}",LlenarPaquete(objForm),LlenarPlan(objForm));
        }

        private string LlenarPlanPaq(BEFormEvaluacion objForm)
        {
            List<BESecPlan_AP> objLista = new BLGeneral_II().ListarPlanesXPaquete(objForm.idPaquete);
            //PROY-24740
            StringBuilder sbPlanPaq = new StringBuilder();
            foreach (BESecPlan_AP obj in objLista)
            {
                sbPlanPaq.Append("|");
                sbPlanPaq.Append(obj.PLNV_CODIGO);
                sbPlanPaq.Append("_");
                sbPlanPaq.Append(obj.CARGO_FIJO_EN_PAQUETE);
                sbPlanPaq.Append("_");
                sbPlanPaq.Append(obj.PLANC_EQUI_SAP);
                sbPlanPaq.Append("_");
                sbPlanPaq.Append(obj.PLNN_TIPO_PLAN);
                sbPlanPaq.Append("_");
                sbPlanPaq.Append(obj.PAQPN_SECUENCIA);
                sbPlanPaq.Append("_");
                sbPlanPaq.Append(obj.GPLNV_DESCRIPCION);
                sbPlanPaq.Append("_");
                sbPlanPaq.Append(obj.CODIGO_BSCS);
                sbPlanPaq.Append("_");
                sbPlanPaq.Append(string.Empty);
                sbPlanPaq.Append("_");
                sbPlanPaq.Append(obj.PRDC_CODIGO);
                sbPlanPaq.Append("_");
                sbPlanPaq.Append(";");
                sbPlanPaq.Append(obj.PLNV_DESCRIPCION);
            }
            return sbPlanPaq.ToString();
            //PROY-24740
        }


        private string LlenarServicioMaterialCorpTotal(BEFormEvaluacion objForm)
        {
            //PROY-24740            
            StringBuilder sbServicioMaterialCorp = new StringBuilder();
            string[] arrPlanes = objForm.listaPlan.Split('|');
            foreach (string strPlan in arrPlanes)
            {
                if (strPlan.Length > 0)
                {
                    string[] arrItem = strPlan.Split(',');

                    objForm.idPlan = arrItem[0];
                    objForm.idPlazo = arrItem[1];
                    objForm.idFila = Funciones.CheckInt(arrItem[2]);
                    objForm.idPlanSap = arrItem[3];
                    objForm.idPaquete = arrItem[4];
                    objForm.nroSecuencia = Funciones.CheckInt(arrItem[5]);
                    objForm.idCampana = arrItem[6];
                    objForm.idCampanaSap = ObtenerCampanaSap(objForm.idCampana);
                    objForm.idProducto = arrItem[7];
                    //PROY-24740
                    sbServicioMaterialCorp.Append(LlenarServicioMaterialCorp(objForm));
                }
            }
            return sbServicioMaterialCorp.ToString();
            //PROY-24740
        }

        private string LlenarServicioMaterialCorp(BEFormEvaluacion objForm)
        {
            //PROY-24740
            return string.Format("°{0}!{1}~{2}", objForm.idFila, LlenarMaterial(objForm), LlenarServicioCorp(objForm));
        }

        private string LlenarServicioCorp(BEFormEvaluacion objForm)
        {
            //PROY-24740
            string strResultado = string.Empty;
            StringBuilder sbServicioSI = new StringBuilder();
            StringBuilder sbServicioNO = new StringBuilder();

            ArrayList objLista = new BLGeneral_II().ListarServiciosXPaqPlan(objForm.idPaquete, objForm.idPlan, objForm.nroSecuencia);
            if (objLista.Count == 0)
            {
                strResultado = LlenarServicio(objForm);
            }
            else
            {
                foreach (BESecServicio_AP obj in objLista)
                {
                    if ((Funciones.CheckInt(obj.SELECCIONABLE_EN_PAQUETE) & (Funciones.CheckInt(SERVICIOS_SELECCIONABLE.OBLIGATORIO) | Funciones.CheckInt(SERVICIOS_SELECCIONABLE.SELECCIONADO))) != 0)
                    {
                        sbServicioSI.Append("|"); 
                        sbServicioSI.Append(obj.SERVN_ORDEN); 
                        sbServicioSI.Append("_");
                        sbServicioSI.Append(obj.GSRVC_CODIGO);
                        sbServicioSI.Append("_");
                        sbServicioSI.Append(Funciones.CheckStr(SERVICIOS_SELECCIONABLE.OBLIGATORIO));
                        sbServicioSI.Append("_");
                        sbServicioSI.Append(obj.SERVV_CODIGO);
                        sbServicioSI.Append("_");
                        sbServicioSI.Append(obj.CARGO_FIJO_EN_PAQUETE);
                        sbServicioSI.Append(";");
                        sbServicioSI.Append("(*) ");
                        sbServicioSI.Append(obj.SERVV_DESCRIPCION);
                    }
                    else
                    {
                        sbServicioNO.Append("|");
                        sbServicioNO.Append(obj.SERVN_ORDEN);
                        sbServicioNO.Append("_");
                        sbServicioNO.Append(obj.GSRVC_CODIGO);
                        sbServicioNO.Append("_");
                        sbServicioNO.Append(obj.SELECCIONABLE_BASE);
                        sbServicioNO.Append("_");
                        sbServicioNO.Append(obj.SERVV_CODIGO);
                        sbServicioNO.Append("_");
                        sbServicioNO.Append(obj.CARGO_FIJO_EN_PAQUETE);
                        sbServicioNO.Append(";");
                        sbServicioNO.Append(obj.SERVV_DESCRIPCION);
                    }
                }

                strResultado = string.Format("{0}{1}¬{2}", strResultado, sbServicioNO.ToString(), sbServicioSI.ToString());
            }

            return strResultado;
            //PROY-24740
        }

        #endregion [Corporativo]

        #region [Claro TV]

        private string LlenarServicioKit(BEFormEvaluacion objForm)
        {
            //PROY-24740   
            return string.Format("{0}~{1}",LlenarServicio(objForm),ListarKitDTH(objForm));
        }

        private string ListarKitDTH(BEFormEvaluacion objForm)
        {
            //PROY-24740 
            string strTipoOperacion = ConfigurationManager.AppSettings["contTipOperaDTH_Alta"].ToString();
            StringBuilder sbKitsDTH = new StringBuilder();
            ArrayList arrKit = new BLGeneral_II().ListarKitsDTH(strTipoOperacion, objForm.idCampana, objForm.idPlazo, objForm.idPlan);
            foreach (BESecKit_AP obj in arrKit)
            {
                sbKitsDTH.Append("|");
                sbKitsDTH.Append(obj.KITV_CODIGO);
                sbKitsDTH.Append("_");
                sbKitsDTH.Append(obj.TKITC_CODIGO);
                sbKitsDTH.Append("_");
                sbKitsDTH.Append(obj.CARGO_FIJO_EN_SEC);
                sbKitsDTH.Append("_");
                sbKitsDTH.Append(obj.KITN_COSTO_INST);
                sbKitsDTH.Append("_");
                sbKitsDTH.Append(";");
                sbKitsDTH.Append(obj.KITV_DESCRIPCION);
            }
            // Kit Cambio Titularidad
            sbKitsDTH.Append("|");
            sbKitsDTH.Append(ConfigurationManager.AppSettings["CodKITCambioTitularidad"]);
            sbKitsDTH.Append("_");
            sbKitsDTH.Append("");
            sbKitsDTH.Append("_");
            sbKitsDTH.Append("0"); 
            sbKitsDTH.Append("_");
            sbKitsDTH.Append("0");
            sbKitsDTH.Append("_");
            sbKitsDTH.Append(";");
            sbKitsDTH.Append(ConfigurationManager.AppSettings["KITCambioTitularidad"].ToString().ToUpper());

            return sbKitsDTH.ToString();
            //PROY-24740 
        }

        #endregion [Claro TV]

        #region [Combo]

        private string LlenarPlanesCombo(BEFormEvaluacion objForm)
        {
            string strResultado = "{0}~{1}~{2}~{3}";

            strResultado = string.Format(strResultado,
                    new BLGeneral_II().ListarCampanasCombo(objForm.idCombo),
                    new BLGeneral_II().ListarPlazosCombo(objForm.idCombo),
                    ListarPlanesCombo(objForm.idCombo),
                    new BLGeneral_II().ListarComboxProducto(objForm.idCombo));

            return strResultado;
        }

        private string ListarPlanesCombo(string strCombo)
        {
            //PROY-24740 
            StringBuilder sbPlanesCombo = new StringBuilder();
            List<BEPlan> objLista = new BLGeneral_II().ListarPlanesCombo(strCombo);
            foreach (BEPlan objPlan in objLista)
            {
                sbPlanesCombo.Append("|");
                sbPlanesCombo.Append(objPlan.PLANC_CODIGO);
                sbPlanesCombo.Append("_");
                sbPlanesCombo.Append(objPlan.PLANN_CAR_FIJ);
                sbPlanesCombo.Append("_");
                sbPlanesCombo.Append(objPlan.PLANC_EQUI_SAP); 
                sbPlanesCombo.Append("_");
                sbPlanesCombo.Append(objPlan.PLNN_TIPO_PLAN); 
                sbPlanesCombo.Append("_");
                sbPlanesCombo.Append(""); 
                sbPlanesCombo.Append("_");
                sbPlanesCombo.Append(objPlan.GPLNV_DESCRIPCION);
                sbPlanesCombo.Append("_");
                sbPlanesCombo.Append(objPlan.CODIGO_BSCS); 
                sbPlanesCombo.Append("_");
                sbPlanesCombo.Append(objPlan.TIPO_PRODUCTOS);
                sbPlanesCombo.Append("_");
                sbPlanesCombo.Append(objPlan.PRDC_CODIGO);
                sbPlanesCombo.Append("_");
                sbPlanesCombo.Append(objPlan.CMBV_CODIGO);
                sbPlanesCombo.Append(";");
                sbPlanesCombo.Append(objPlan.PLANV_DESCRIPCION);
            }

            return sbPlanesCombo.ToString();
            //PROY-24740 
        }

        #endregion [Combo]

        #region [Venta Varios]

        private string LlenarListaPrecio(BEFormEvaluacion objForm)
        {
            //PROY-24740 
            StringBuilder sbListaPrecio = new StringBuilder();
            List<BEItemGenerico> objLista = ObtenerListaPrecioPVUDB(objForm);
            foreach (BEItemGenerico obj in objLista)
            {
                sbListaPrecio.Append("|");
                sbListaPrecio.Append(obj.Codigo);
                sbListaPrecio.Append(";");
                sbListaPrecio.Append(obj.Descripcion);                
            }
            return sbListaPrecio.ToString();
            //PROY-24740 
        }

        private string LlenarPrecio(BEFormEvaluacion objForm)
        {
            decimal decPrecioIncIGV;
            string strResultado = string.Empty;
            double dblCosto = 0, dblCostoTmp = 0;

            List<BEConsultarPrecioBase> oListConsultaPrecioBase = BLSincronizaSap.ConsultarPrecioBase(objForm.idMaterial, null);

            foreach (BEConsultarPrecioBase item in oListConsultaPrecioBase)
            {
                dblCostoTmp = Funciones.CheckDbl(item.PrecioCompra);
                if (dblCostoTmp > dblCosto)
                {
                    dblCosto = dblCostoTmp;
                }
            }
            decPrecioIncIGV = Funciones.CheckDecimal(ObtenerListaPrecioPVUDB(objForm).Where(q => q.Codigo == objForm.idListaPrecio).FirstOrDefault().Monto);
            strResultado = string.Format("{0}_{1}_{2}_{3}", decPrecioIncIGV, objForm.idListaPrecio, string.Empty, Funciones.CheckStr(dblCosto));
            return strResultado;
        }
        //gaa20161020
        private string LlenarFamiliaPlan(BEFormEvaluacion objForm)
        {
            StringBuilder sbFamilia = new StringBuilder();
            List<BEItemGenerico> lstItem = new BLGeneral_II().ListarFamiliaPlan(objForm.modalidadVenta, objForm.idCampana);

            foreach (BEItemGenerico obj in lstItem)
            {
                sbFamilia.Append("|");
                sbFamilia.Append(obj.Codigo);
                sbFamilia.Append(";");
                sbFamilia.Append(obj.Descripcion);
            }

            sbFamilia.Append("¬");
            sbFamilia.Append(LlenarPlan(objForm));

            return sbFamilia.ToString();
        }
        //fin gaa20161020
        private string LlenarMontosTopeConsumo(BEFormEvaluacion objForm) // INI PROY-29296
        {
            StringBuilder sbMontosTopeConsumo;
            try
            {
                sbMontosTopeConsumo = new StringBuilder();
                List<BETopeConsumo> listTopeConsumo = new List<BETopeConsumo>();
                BETopeConsumo objTopeConsumo = new BETopeConsumo();
                BEItemMensaje objItemMensaje = new BEItemMensaje();

                objTopeConsumo.CodigoProducto = objForm.idProducto;
                objTopeConsumo.CodigoPlan = objForm.idPlan;
                objTopeConsumo.CodigoServicio = objForm.codServicio;

                listTopeConsumo = new BWTopeConsumo().LlenarMontosTopeConsumo(objTopeConsumo, ref objItemMensaje);


                foreach (BETopeConsumo objTope in listTopeConsumo)
                {
                    sbMontosTopeConsumo.Append(objTope.ID);
                    sbMontosTopeConsumo.Append(";");
                    sbMontosTopeConsumo.Append(objForm.idPlan);
                    sbMontosTopeConsumo.Append(";");
                    sbMontosTopeConsumo.Append(objTope.CodigoServicio);
                    sbMontosTopeConsumo.Append(";");
                    sbMontosTopeConsumo.Append(objTope.DescripcionServicio);
                    sbMontosTopeConsumo.Append(";");
                    sbMontosTopeConsumo.Append(objTope.Monto);
                    sbMontosTopeConsumo.Append(";");
                    sbMontosTopeConsumo.Append(objTope.Minuto);
                    sbMontosTopeConsumo.Append("¬");
                }
            }
            catch (Exception ex)
            {
                sbMontosTopeConsumo = new StringBuilder();
                sbMontosTopeConsumo.Append(ex.ToString());
                sbMontosTopeConsumo.Append("~");
            }
            return sbMontosTopeConsumo.ToString();
        }
        // FIN PROY-29296
        private string LlenarMontosTopeConsumoLTE(BEFormEvaluacion objForm)
        {
            StringBuilder sbMontosTopeConsumo = new StringBuilder();
            BETopeConsumo objTopeConsumo = new BETopeConsumo();
            BEItemMensaje objItemMensaje = new BEItemMensaje();

            objTopeConsumo.CodigoProducto = objForm.idProducto;
            objTopeConsumo.CodigoPlan = objForm.idPlan;
            objTopeConsumo.CodigoServicio = objForm.codServicio;

            List<BETopeConsumo> listTopeConsumo = new BWTopeConsumo().LlenarMontosTopeConsumo(objTopeConsumo, ref objItemMensaje);


            foreach (BETopeConsumo objTope in listTopeConsumo)
            {
                sbMontosTopeConsumo.Append(objTope.ID);
                sbMontosTopeConsumo.Append(";");
                sbMontosTopeConsumo.Append(objForm.idPlan);
                sbMontosTopeConsumo.Append(";");
                sbMontosTopeConsumo.Append(objTope.CodigoServicio);
                sbMontosTopeConsumo.Append(";");
                sbMontosTopeConsumo.Append(objTope.DescripcionServicio);
                sbMontosTopeConsumo.Append(";");
                sbMontosTopeConsumo.Append(objTope.Monto);
                sbMontosTopeConsumo.Append(";");
                sbMontosTopeConsumo.Append(objTope.Minuto);
                sbMontosTopeConsumo.Append("¬");
            }

            return sbMontosTopeConsumo.ToString();
        }
        #endregion [Venta Varios]

        private string SECPendiente(BEFormEvaluacion objForm)
        {
            string idOficina = objForm.idOficina;
            string tipoDocumento = objForm.tipoDocumento;
            string nroDocumento = objForm.nroDocumento;
            Int64 nroSEC = objForm.nroSEC;

            string motivo = string.Empty;
            string idOferta = string.Empty;
            string idTipoOperacion = string.Empty;
            string idPlazo = string.Empty;
            string idCampana = string.Empty;
            string modalidadVenta = string.Empty;
            string idCasoEspecial = string.Empty;
            string idCombo = string.Empty;
            string agrupa_paquete = string.Empty;
            string idFlujo = "1";
            int idFila = 0;

            List<BEItemGenerico> objListPaquete = null;
            List<BEItemGenerico> objListCampana = null;
            List<BESecServicio_AP> objListServicio = null;
            List<BEPlan> objListPlan = null;
            List<BEItemGenerico> objListEquipo = null;
            List<BEItemGenerico> objListPrecio = null;
            List<BESecPlan_AP> objListPaquetexPlan = null;

            BEItemGenerico objFindCampana = null;
            BEPlan objFindPlan = null;
            BESecServicio_AP objFindServicio = null;
            BESecPlan_AP objFindPlanAP = null;
            //gaa20161020
            List<BEItemGenerico> objListFamiliaPlan = null;
            BEItemGenerico objFindFamiliaPlan = null;
            //fin gaa20161020
            //PROY-24740
            StringBuilder sbLstGeneral = new StringBuilder();
            StringBuilder sbLstPlan = new StringBuilder();
            StringBuilder sbLstServicio = new StringBuilder();
            StringBuilder sbLstEquipo = new StringBuilder();
            StringBuilder sbLstBuyback = new StringBuilder();//PROY-140736 

            //PROY-24740
            StringBuilder sblistaProducto = new StringBuilder();
            StringBuilder sblistaPaquete = new StringBuilder();
            StringBuilder sblistaPlan = new StringBuilder();

            GeneradorLog objLog = new GeneradorLog(CurrentUser,nroDocumento,null,"WEB");
            objLog.CrearArchivolog("[Inicio - INC000003848031][SECPendiente]",null,null);
            try
            {
                bool Isvigente = false;
                DataSet ds = new BLSolicitud().ObtenerDetalleSECPendiente(nroSEC);

               
              
                if (ds != null && ds.Tables.Count == 3)
                {
                    DataTable dtSec = ds.Tables[0];
                    DataTable dtPlan = ds.Tables[1];
                    DataTable dtServicio = ds.Tables[2];

                    motivo = dtSec.Rows[0]["respuesta"].ToString();

                  

                    if (string.IsNullOrEmpty(motivo))
                    {
                        idOferta = dtSec.Rows[0]["tproc_codigo"].ToString();
                        idTipoOperacion = dtSec.Rows[0]["topen_codigo"].ToString();
                        modalidadVenta = dtSec.Rows[0]["modalidad_venta"].ToString();

                        objForm.idOferta = idOferta;
                        objForm.idTipoOperacion = idTipoOperacion;
                        objForm.modalidadVenta = modalidadVenta;

                        sbLstGeneral.Append("©");
                        sbLstGeneral.Append(idTipoOperacion);
                        sbLstGeneral.Append("©");
                        sbLstGeneral.Append(idOferta);
                        sbLstGeneral.Append("©");
                        sbLstGeneral.Append(idCasoEspecial);
                        sbLstGeneral.Append("©");
                        sbLstGeneral.Append(idCombo);
                        sbLstGeneral.Append("©");
                        sbLstGeneral.Append(modalidadVenta);
                        sbLstGeneral.Append("©");
                        sbLstGeneral.Append(BLReglaNegocio.LlenarTipoProductoxOferta(idOferta, idFlujo, idTipoOperacion, "", tipoDocumento, idCasoEspecial, modalidadVenta));
                        sbLstGeneral.Append("¬");
                        sbLstGeneral.Append(BLReglaNegocio.LlenarCasoEspecial(idOferta, idFlujo, idTipoOperacion, idOficina));
                        sbLstGeneral.Append("¬");
                        sbLstGeneral.Append(BLReglaNegocio.ListarCombo(idOficina, idOferta, idTipoOperacion, idFlujo, tipoDocumento, modalidadVenta));

                        // Validación Campañas Vigentes
                        foreach (DataRow dr in dtPlan.Rows)
                        {
                            string idProducto = dr["idproducto"].ToString();
                            string strFiltro = string.Empty;

                            if (sblistaProducto.ToString().IndexOf(idProducto) == -1)
                            {
                                sblistaProducto.Append("|");
                                sblistaProducto.Append(idProducto);
                                objListCampana = null;
                                objListPlan = null;
                                //gaa20150714
                                //objListCampana = new BLGeneral_II().ListarCampana(idCombo, idOficina, idOferta, idProducto, idCasoEspecial, modalidadVenta);
                                objListCampana = new BLGeneral_II().ListarCampana(idCombo, idOficina, idOferta, idProducto, idCasoEspecial, modalidadVenta, idFlujo, idTipoOperacion);
                                //fin gaa20150714
                                //gaa20161020
                                //objListPlan = new BLGeneral_II().ListarPlanTarifario(idFlujo, tipoDocumento, idOferta, idTipoOperacion, dr["idproducto"].ToString(),
                                //                                                    idCasoEspecial, dr["idcampana"].ToString(), dr["idplazo"].ToString(), idOficina, idCombo, dr["idFamiliaPlan"].ToString(), ref strFiltro);
                                objListPlan = new BLGeneral_II().ListarPlanTarifario(idFlujo, tipoDocumento, idOferta, idTipoOperacion, dr["idproducto"].ToString(),
                                                                                    idCasoEspecial, dr["idcampana"].ToString(), dr["idplazo"].ToString(), idOficina, idCombo, dr["FAMILIA_PLAN"].ToString(), ref strFiltro);

                                objListFamiliaPlan = new BLGeneral_II().ListarFamiliaPlan(modalidadVenta, dr["idcampana"].ToString());
                                //fin gaa20161020
                                if (strFiltro == "S")
                                {
                                    List<BEItemGenerico> objListaFiltro = (List<BEItemGenerico>)Session["FiltroPlanxPdv_" + idOficina];
                                    objListPlan = (from plan in objListPlan
                                                   join filtro in objListaFiltro on plan.PLANC_CODIGO equals filtro.Codigo
                                                   select plan).OrderBy(x => x.PLANV_DESCRIPCION).ToList<BEPlan>();
                                }
                            }

                            objFindCampana = objListCampana.Find(x => x.Codigo == dr["idcampana"].ToString());
                            if (objFindCampana == null)
                            {
                                motivo = string.Format("Regla. Campaña {0} no presenta mismas condiciones.", dr["idcampana"].ToString());
                                throw new Exception(motivo);
                            }
                            //gaa20160211
                            if (dr["idcampana"].ToString() == ConfigurationManager.AppSettings["CampanaDiaEnamorados"])
                            {
                                motivo = string.Format("Regla. Campaña {0} no debe pasar como sec pendiente.", dr["idcampana"].ToString());
                                throw new Exception(motivo);
                            }
                            //fin gaa20160211
                            //gaa20161020
                            objFindFamiliaPlan = objListFamiliaPlan.Find(x => x.Codigo == dr["FAMILIA_PLAN"].ToString());
                            if (objFindFamiliaPlan == null)
                            {
                                motivo = string.Format("Regla. Familia {0} no presenta mismas condiciones.", dr["FAMILIA_PLAN"].ToString());
                                throw new Exception(motivo);
                            }
                            //fin gaa20161020
                            // Validación Paquete Vigente
                            string idPaquete = dr["idPaquete"].ToString();

                            if (!string.IsNullOrEmpty(idPaquete))
                            {
                                if (idProducto != strCodTipoProducto3Play)
                                {
                                    if (sblistaPaquete.ToString().IndexOf(idPaquete) == -1)
                                    {
                                        sblistaPaquete.Append("|");
                                        sblistaPaquete.Append(idPaquete);
                                        objListPaquete = null;
                                        objListPaquete = new BLGeneral_II().ListarPaquete(tipoDocumento, idOferta, dr["idplazo"].ToString());
                                        objListPaquetexPlan = new BLGeneral_II().ListarPlanesXPaquete(dr["idPaquete"].ToString());
                                    }

                                    Isvigente = objListPaquete.Any(x => x.Codigo == dr["idPaquete"].ToString());
                                    if (!Isvigente)
                                    {
                                        motivo = string.Format("Regla. Paquete {0} no presenta mismas condiciones.", dr["idPaquete"].ToString());
                                        throw new Exception(motivo);
                                    }

                                    // Validación Planes x Paquete
                                    objFindPlanAP = objListPaquetexPlan.Find(x => x.PAQUETE.PAQTV_CODIGO == dr["idPaquete"].ToString() && x.PLNV_CODIGO == dr["idPlan"].ToString() && x.PAQPN_SECUENCIA.ToString() == dr["sopln_orden"].ToString());
                                    if (objFindPlanAP == null)
                                    {
                                        motivo = string.Format("Regla. El plan {0} del Paquete {1} no presenta mismas condiciones.", dr["idPlan"].ToString(), dr["idPaquete"].ToString());
                                        throw new Exception(motivo);
                                    }
                                }
                            }
                            else
                            {
                                // Validación Planes Individuales
                                objFindPlan = objListPlan.Find(x => x.PLANC_CODIGO == dr["idplan"].ToString() && x.PLANN_CAR_FIJ == Funciones.CheckDbl(dr["cargo_fijo"]));
                                if (objFindPlan == null)
                                {
                                    motivo = string.Format("Regla. Plan {0} no presenta mismas condiciones. CF {1}", dr["idplan"].ToString(), dr["cargo_fijo"].ToString());
                                    throw new Exception(motivo);
                                }
                            }

                            sbLstPlan.Append("©");
                            sbLstPlan.Append(dr["sopln_orden"]);
                            sbLstPlan.Append("~");
                            sbLstPlan.Append(idProducto);
                            sbLstPlan.Append("~");
                            sbLstPlan.Append(objFindCampana.Codigo);
                            sbLstPlan.Append(";");
                            sbLstPlan.Append(objFindCampana.Descripcion);
                            sbLstPlan.Append("~");
                            sbLstPlan.Append(dr["idplazo"]);
                            sbLstPlan.Append(";");
                            sbLstPlan.Append(dr["plazo"]);
                            sbLstPlan.Append("~");
                            sbLstPlan.Append(dr["idpaquete"]);
                            sbLstPlan.Append(";");
                            sbLstPlan.Append(dr["paquete"]);


                            if (string.IsNullOrEmpty(idPaquete))
                            {                                
                                sbLstPlan.Append("~");
                                sbLstPlan.Append(objFindPlan.PLANC_CODIGO);
                                sbLstPlan.Append("_");
                                sbLstPlan.Append(objFindPlan.PLANN_CAR_FIJ);
                                sbLstPlan.Append("_");
                                sbLstPlan.Append(objFindPlan.PLANC_EQUI_SAP);
                                sbLstPlan.Append("_");
                                sbLstPlan.Append(objFindPlan.PLNN_TIPO_PLAN);
                                sbLstPlan.Append("_");
                                sbLstPlan.Append(string.Empty);
                                sbLstPlan.Append("_");
                                sbLstPlan.Append(objFindPlan.GPLNV_DESCRIPCION);
                                sbLstPlan.Append("_");
                                sbLstPlan.Append(objFindPlan.CODIGO_BSCS);
                                sbLstPlan.Append("_");
                                sbLstPlan.Append(objFindPlan.TIPO_PRODUCTOS);
                                sbLstPlan.Append("_");
                                sbLstPlan.Append(objFindPlan.PRDC_CODIGO);
                                sbLstPlan.Append(";");
                                sbLstPlan.Append(objFindPlan.PLANV_DESCRIPCION);
                            }
                            else
                            {                                
                                sbLstPlan.Append("~");
                                sbLstPlan.Append(objFindPlanAP.PLNV_CODIGO);
                                sbLstPlan.Append("_");
                                sbLstPlan.Append(objFindPlanAP.CARGO_FIJO_EN_PAQUETE);
                                sbLstPlan.Append("_");
                                sbLstPlan.Append(objFindPlanAP.PLANC_EQUI_SAP);
                                sbLstPlan.Append("_");
                                sbLstPlan.Append(objFindPlanAP.PLNN_TIPO_PLAN);
                                sbLstPlan.Append("_");
                                sbLstPlan.Append(objFindPlanAP.PAQPN_SECUENCIA);
                                sbLstPlan.Append("_");
                                sbLstPlan.Append(objFindPlanAP.GPLNV_DESCRIPCION);
                                sbLstPlan.Append("_");
                                sbLstPlan.Append(objFindPlanAP.CODIGO_BSCS);
                                sbLstPlan.Append("_");
                                sbLstPlan.Append(string.Empty);
                                sbLstPlan.Append("_");
                                sbLstPlan.Append(objFindPlanAP.PRDC_CODIGO);
                                sbLstPlan.Append("_");
                                sbLstPlan.Append(";");
                                sbLstPlan.Append(objFindPlanAP.PLNV_DESCRIPCION);
                            }

                            sbLstPlan.Append("~");
                            sbLstPlan.Append(dr["cargo_fijo_linea"]);

                            if (Funciones.CheckInt(dr["idtope"]) == (int)BEServicio.TIPO_TOPE_CONSUMO.TOPE_CONSUMO_ADICIONAL) //PROY-29296
                            {                                
                                sbLstPlan.Append("~");
                                sbLstPlan.Append(dr["monto_tope"]);
                            }
                            else
                            {
                                sbLstPlan.Append("~");
                            }
                            if (Funciones.CheckInt(dr["sopln_orden"]) > idFila)
                                idFila = Funciones.CheckInt(dr["sopln_orden"]);

                            agrupa_paquete = dr["agrupa_paquete"].ToString();
//gaa20161020
                            sbLstPlan.Append("~");
                            //objFindFamiliaPlan
                            sbLstPlan.Append(objFindFamiliaPlan.Codigo);
                            sbLstPlan.Append(";");
                            sbLstPlan.Append(objFindFamiliaPlan.Descripcion);
//fin gaa20161020
                        }

                        sbLstGeneral.Append("©");
                        sbLstGeneral.Append(idFila);
                        sbLstGeneral.Append("©");
                        sbLstGeneral.Append(agrupa_paquete);

                        // Validación Servicio Vigente
                        string idOrden = string.Empty;
                        foreach (DataRow dr in dtServicio.Rows)
                        {
                            string plan = dr["idplan"].ToString();

                            if (sblistaPlan.ToString().IndexOf(plan) == -1)
                            {
                                sblistaPlan.Append("|"); 
                                sblistaPlan.Append(plan);
                                objListServicio = null;
                                objListServicio = new BLGeneral_II().ListarServiciosXPlan(dr["idproducto"].ToString(), dr["idplan"].ToString());
                            }

                            objFindServicio = objListServicio.Find(x => x.SERVV_CODIGO == dr["idservicio"].ToString() && x.CARGO_FIJO_BASE == Funciones.CheckDbl(dr["cf"]));
                            if (objFindServicio == null)
                            {
                                motivo = string.Format("Regla. Servicio {0} no presenta mismas condiciones.", dr["idservicio"].ToString());
                                throw new Exception(motivo);
                            }

                            if (dr["sopln_orden"].ToString() != idOrden)
                            {
                                sbLstServicio.Append("*ID*");
                                sbLstServicio.Append(dr["sopln_orden"]);
                            }

                            objFindServicio.SELECCIONABLE_BASE = ((int)BESecServicio_AP.TIPO_SELECCION.SELECCIONADO).ToString();
                            
                            sbLstServicio.Append("|");
                            sbLstServicio.Append(objFindServicio.SERVN_ORDEN);
                            sbLstServicio.Append("_");
                            sbLstServicio.Append(objFindServicio.GSRVC_CODIGO);
                            sbLstServicio.Append("_");
                            sbLstServicio.Append(objFindServicio.SELECCIONABLE_BASE);
                            sbLstServicio.Append("_");
                            sbLstServicio.Append(objFindServicio.SERVV_CODIGO);
                            sbLstServicio.Append("_");
                            sbLstServicio.Append(objFindServicio.CARGO_FIJO_BASE);
                            sbLstServicio.Append(";");
                            sbLstServicio.Append(objFindServicio.SERVV_DESCRIPCION);

                            idOrden = dr["sopln_orden"].ToString();
                        }

                        
                       

                        // Validación Equipo/Lista Precio Vigente
                        sblistaPlan = new StringBuilder();
                        foreach (DataRow dr in dtPlan.Rows)
                        {
                            if (dr["idequipo"].ToString() != "")
                            {
                                string tipoProducto = dr["idproducto"].ToString();
                                string plan = dr["idplan"].ToString();
                                string idCampanaSap = ObtenerCampanaSap(dr["idcampana"].ToString());

                                objForm.idCampana = dr["idcampana"].ToString();
                                objForm.idCampanaSap = idCampanaSap;
                                objForm.idPlazo = dr["idplazo"].ToString();
                                objForm.idPlan = dr["idplan"].ToString();
                                objForm.idMaterial = dr["idequipo"].ToString();

                                if (dr["idequipo"].ToString() != ConfigurationManager.AppSettings["constCodEquipoProm"].ToString())
                                {
                                    if (sblistaPlan.ToString().IndexOf(plan) == -1)
                                    {
                                        sblistaPlan.Append("|");
                                        sblistaPlan.Append(plan);

                                        objForm.idCampanaSap = idCampanaSap;
                                        objForm.idPlan = plan;
                                        objForm.idPlazo = dr["idplazo"].ToString();
                                        objListEquipo = ObtenerMaterialMSSAP(objForm);
                                    }

                                    Isvigente = objListEquipo.Any(x => x.Codigo == dr["idequipo"].ToString());
                                    if (!Isvigente)
                                    {
                                        motivo = string.Format("Regla. Equipo {0} no tiene stock.", dr["idequipo"].ToString());
                                        throw new Exception(motivo);
                                    }

                                    objListPrecio = null;

                                    objForm.idMaterial = dr["idequipo"].ToString();
                                    objForm.idCampanaSap = idCampanaSap;
                                    objForm.idPlazo = dr["idplazo"].ToString();

                                    objListPrecio = ObtenerListaPrecioPVUDB(objForm);

                                    Isvigente = objListPrecio.Any(x => x.Codigo == dr["idlista_precio"].ToString() && x.Monto == Funciones.CheckDbl(dr["precio_venta"]));
                                    if (!Isvigente)
                                    {
                                        motivo = string.Format("Regla. Equipo {0} no tiene el mismo precio. Lista precio {1}.", dr["idequipo"].ToString(), dr["idlista_precio"].ToString());
                                        throw new Exception(motivo);
                                    }
                                }

                                sbLstEquipo.Append("©"); 
                                sbLstEquipo.Append(dr["sopln_orden"]);
                                sbLstEquipo.Append("~");
                                sbLstEquipo.Append(dr["idequipo"]);
                                sbLstEquipo.Append(";");
                                sbLstEquipo.Append(dr["equipo"]);
                                sbLstEquipo.Append("~");
                                sbLstEquipo.Append(dr["precio_venta"]);
                                sbLstEquipo.Append("_");
                                sbLstEquipo.Append(dr["idlista_precio"]);
                                sbLstEquipo.Append("_");
                                sbLstEquipo.Append(dr["lista_precio"]);
                                sbLstEquipo.Append("_");
                                sbLstEquipo.Append(dr["costo"]);
                            }
                        }
                        //PROY-140736 INI
                        List<BEItemGenerico> itemBuyback = null;
                        string buyback = "";
                        string buybackordenado = "";
                        //int codigo=0;
                        //string msjrpta = "";

                        itemBuyback = new BLGeneral().ListarBuyback(nroSEC);
                        foreach (BEItemGenerico item in itemBuyback) {
                            buyback = item.Codigo + ";" + item.Codigo2 + ";" + item.Descripcion + ";" + item.Descripcion2 + "|";
                            buybackordenado = buybackordenado + buyback;


                        }
                        hdbuyback_frame.Value = buybackordenado;
                        if (hdbuyback_frame.Value!="")
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), new Guid().ToString(), "EnviarDatosBuyback('');", true);
                        }
                        //PROY-140736 FIN
                        hidnResultadoValue.Value = string.Format("{0}#{1}#{2}#{3}", sbLstGeneral.ToString(), sbLstPlan.ToString(), sbLstServicio.ToString(), sbLstEquipo.ToString());
                        return hidnResultadoValue.Value;
            }
                }
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[Error][SECPendiente]", null, ex);
            }

            return string.Empty;
            //PROY-24740
        }

        private string ObtenerCampanaSap(string strCampana)
        {
            return new BLGeneral_II().ObtenerCampanaSap(strCampana);
        }

        private List<BEItemGenerico> ObtenerMaterialMSSAP(BEFormEvaluacion objForm)
        {

            GeneradorLog objLog = new GeneradorLog(CurrentUser, "", null, "ObtenerMaterialMSSAP");
            BEConsultaDatosOficina oListParametrosOficina = BLSincronizaSap.ConsultaDatosOficina(objForm.idOficina, null);

            //PROY-30859-IDEA-39316-RU02-by-LCEJ INI                 

            objLog.CrearArchivolog("Valida ingreso a Validación BAM 39316-RU02-> tipoProducto : " + objForm.idProducto, null, null);

            //rsm 30859 INI
            string canalActual = oListParametrosOficina.TipoOficina;
            objLog.CrearArchivolog("[PROY 30859 - PARAMETROS FILTRO MATERIALES BAM]", "", null);
            objLog.CrearArchivolog("[canalActual]", canalActual, null);
            string tipoOperacionActual = objForm.idTipoOperacion;
            objLog.CrearArchivolog("[tipoOperacionActual]", tipoOperacionActual, null);
            string modalidadVentaActual = objForm.modalidadVenta;
            objLog.CrearArchivolog("[modalidadVentaActual]", modalidadVentaActual, null);
            //rsm 30859 FIN

            objLog.CrearArchivolog("[activacionValidacionMaterialesBAM]", activacionValidacionMaterialesBAM, null);
            objLog.CrearArchivolog("[canalesPermitidosBAM]", canalesPermitidosBAM, null);
            objLog.CrearArchivolog("[tipoOperacionesPermitidosBAM]", tipoOperacionesPermitidosBAM, null);
            objLog.CrearArchivolog("[modalidadVentasPermitidasBAM]", modalidadVentasPermitidasBAM, null);
            Boolean ValidBam = false;
                //rsm 30859 INI
                if (activacionValidacionMaterialesBAM == "1")
                {
                    if (canalesPermitidosBAM.IndexOf(canalActual) > -1)
                    {
                        if (tipoOperacionesPermitidosBAM.IndexOf(tipoOperacionActual) > -1)
                        {
                            if (modalidadVentasPermitidasBAM.IndexOf(modalidadVentaActual) > -1)
                            {
                                ValidBam = true;
                            }
                        }
                    }
                }
                //rsm 30859 FIN                      
            
            if (objForm.idProducto == ConfigurationManager.AppSettings["constTipoProductoBAM"])
            {
                objForm.idPlan = objForm.idPlanSap;
            }
            List<BEConsultarMaterialXCampania> oListaMaterialesCampania;
            List<BEConsultaListaPrecios> oListaMaterialesConPrecio;

            if (!ValidarAccesoOpcion(objForm.idTipoOficina, objForm.idProducto, objForm.idTipoOperacion, objForm.tipoDocumento, ConfigurationManager.AppSettings["BloqueoEquipoSinStockCod"]))
            {
            if (ValidBam)
            {
                objLog.CrearArchivolog("[BOOL ValidBam TRUE]", "", null);
                BLSincronizaSap objSincSap = new BLSincronizaSap();                
                    string strEval = "1";
                objLog.CrearArchivolog("Fin a Valida BAM 39316-RU02", null, null);


                    oListaMaterialesCampania = (from p in objSincSap.ConsultaArticulosBam(objForm.idTipoOficina, objForm.idTipoOperacion, objForm.idTipoVenta, ConfigurationManager.AppSettings["FamiliaEquipoBam"], oListParametrosOficina.CodigoInterlocutor, null, oListParametrosOficina.CodCentro, oListParametrosOficina.CodAlmacen, strEval)
                                                where !ConfigurationManager.AppSettings["ConsTipoMaterialesCHIP"].Contains(p.TipoMaterial) && p.CodigoCentro == "1"
                                                select p).ToList();

                    List<BEConsultarMaterialXCampania> oListaMaterialesCampaniaBAM = (from p in objSincSap.ConsultaArticulosBam(objForm.idTipoOficina, objForm.idTipoOperacion, objForm.idTipoVenta, ConfigurationManager.AppSettings["FamiliaEquipoBam"], oListParametrosOficina.CodigoInterlocutor, null, oListParametrosOficina.CodCentro, oListParametrosOficina.CodAlmacen, strEval)
                                                                           where !ConfigurationManager.AppSettings["ConsTipoMaterialesCHIP"].Contains(p.TipoMaterial)
                                                                           select p).ToList();

                    oListaMaterialesConPrecio = (from p in BLSincronizaSap.ConsultarPrecio(objForm)
                                                 join planes in oListaMaterialesCampaniaBAM on p.CodigoMaterial equals planes.CodigoMaterial
                                                 select p).ToList();
                }
                else
                {

                    oListaMaterialesCampania = (from p in BLSincronizaSap.ConsultarMaterialXCampania(oListParametrosOficina.CodigoInterlocutor, null, oListParametrosOficina.CodCentro, oListParametrosOficina.CodAlmacen, oListParametrosOficina.TipoOficina)
                                                where !ConfigurationManager.AppSettings["ConsTipoMaterialesCHIP"].Contains(p.TipoMaterial)
                                                select p).ToList();



                    oListaMaterialesConPrecio = BLSincronizaSap.ConsultarPrecio(objForm);

                
                }

                BLConsumer objConsumer = new BLConsumer();
                List<BEItemGenerico> objEquipoAlt = objConsumer.ConsultaEquiposAlternativos();

                objLog.CrearArchivolog("[objEquipoAlt]", objEquipoAlt.Count.ToString(), null);
            
                List<BEItemGenerico> objListaEquipoAlt = (from materiales in objEquipoAlt
                                                          join planes in oListaMaterialesConPrecio on materiales.Codigo equals planes.CodigoMaterial
                                                          select new BEItemGenerico(materiales.Codigo, materiales.Descripcion)).GroupBy(g => g.Codigo).Select(s => s.First()).ToList<BEItemGenerico>();


                objLog.CrearArchivolog("[objListaEquipoAlt]", objListaEquipoAlt.Count.ToString(), null);

                List<BEItemGenerico> objListaEquipoAltfiltro = (from materiales in objListaEquipoAlt
                                                                join planes in oListaMaterialesCampania on materiales.Codigo equals planes.CodigoMaterial
                                                      
                                                                select new BEItemGenerico(materiales.Codigo, materiales.Descripcion)).GroupBy(g => g.Codigo).Select(s => s.First()).ToList<BEItemGenerico>();

                objLog.CrearArchivolog("[objListaEquipoAltfiltro]", objListaEquipoAltfiltro.Count.ToString(), null);

                List<BEItemGenerico> objListaNormal = (from materiales in oListaMaterialesCampania
                                                 join planes in oListaMaterialesConPrecio on materiales.CodigoMaterial equals planes.CodigoMaterial
                                                 select new BEItemGenerico(materiales.CodigoMaterial, materiales.DescripcionMaterial)).GroupBy(g => g.Codigo).Select(s => s.First()).ToList<BEItemGenerico>();

                objLog.CrearArchivolog("[objListaNormal]", objListaNormal.Count.ToString(), null);

                List<BEItemGenerico> objSinStock = new List<BEItemGenerico>();

                objSinStock.AddRange(objListaEquipoAlt.Where(p => objListaEquipoAltfiltro.All(p2 => p2.Codigo != p.Codigo)));

                objLog.CrearArchivolog("[objSinStock]", objSinStock.Count.ToString(), null);

                List<BEItemGenerico> objSinStockFinal = (from v in objSinStock select new BEItemGenerico("^" + v.Codigo, v.Descripcion)).ToList();

                objLog.CrearArchivolog("[objSinStockFinal]", objSinStockFinal.Count.ToString(), null);

                List<BEItemGenerico> objListaEquipoAlFinal = objListaEquipoAltfiltro.Union(objSinStockFinal).ToList();

                 List<BEItemGenerico> objListaMaterialConStock = new List<BEItemGenerico>();

                 objListaMaterialConStock.AddRange(objListaNormal.Where(p => objListaEquipoAltfiltro.All(p2 => p2.Codigo != p.Codigo)));

                List<BEItemGenerico> objListaFinal = objListaMaterialConStock.Union(objListaEquipoAlFinal).GroupBy(g => g.Codigo).Select(s => s.First()).OrderBy(v => v.Descripcion).ToList<BEItemGenerico>();


                objLog.CrearArchivolog("[objListaFinal]", objListaFinal.Count.ToString(), null);

                return objListaFinal;
            }
            else
            {

               if (ValidBam)
                {
                    objLog.CrearArchivolog("[BOOL ValidBam TRUE]", "", null);
                    BLSincronizaSap objSincSap = new BLSincronizaSap();
                    string strEval = "1";
                    objLog.CrearArchivolog("Fin a Valida BAM 39316-RU02", null, null);


                    oListaMaterialesCampania = (from p in objSincSap.ConsultaArticulosBam(objForm.idTipoOficina, objForm.idTipoOperacion, objForm.idTipoVenta, ConfigurationManager.AppSettings["FamiliaEquipoBam"], oListParametrosOficina.CodigoInterlocutor, null, oListParametrosOficina.CodCentro, oListParametrosOficina.CodAlmacen, strEval)
                                                where !ConfigurationManager.AppSettings["ConsTipoMaterialesCHIP"].Contains(p.TipoMaterial) && p.CodigoCentro == "1"
                                                select p).ToList();

                    List<BEConsultarMaterialXCampania> oListaMaterialesCampaniaBAM = (from p in objSincSap.ConsultaArticulosBam(objForm.idTipoOficina, objForm.idTipoOperacion, objForm.idTipoVenta, ConfigurationManager.AppSettings["FamiliaEquipoBam"], oListParametrosOficina.CodigoInterlocutor, null, oListParametrosOficina.CodCentro, oListParametrosOficina.CodAlmacen, strEval)
                                                                                      where !ConfigurationManager.AppSettings["ConsTipoMaterialesCHIP"].Contains(p.TipoMaterial)
                                                                                      select p).ToList();

                    oListaMaterialesConPrecio = (from p in BLSincronizaSap.ConsultarPrecio(objForm)
                                                 join planes in oListaMaterialesCampaniaBAM on p.CodigoMaterial equals planes.CodigoMaterial
                                                 select p).ToList();
            }
            else
            {
                    if (ValidBam)
                    {
                        objLog.CrearArchivolog("[BOOL ValidBam TRUE]", "", null);
                        BLSincronizaSap objSincSap = new BLSincronizaSap();
                        string strEval = "1";
                        objLog.CrearArchivolog("Fin a Valida BAM 39316-RU02", null, null);


                        oListaMaterialesCampania = (from p in objSincSap.ConsultaArticulosBam(objForm.idTipoOficina, objForm.idTipoOperacion, objForm.idTipoVenta, ConfigurationManager.AppSettings["FamiliaEquipoBam"], oListParametrosOficina.CodigoInterlocutor, null, oListParametrosOficina.CodCentro, oListParametrosOficina.CodAlmacen, strEval)
                                                    where !ConfigurationManager.AppSettings["ConsTipoMaterialesCHIP"].Contains(p.TipoMaterial) && p.CodigoCentro == "1"
                                                    select p).ToList();

                        List<BEConsultarMaterialXCampania> oListaMaterialesCampaniaBAM = (from p in objSincSap.ConsultaArticulosBam(objForm.idTipoOficina, objForm.idTipoOperacion, objForm.idTipoVenta, ConfigurationManager.AppSettings["FamiliaEquipoBam"], oListParametrosOficina.CodigoInterlocutor, null, oListParametrosOficina.CodCentro, oListParametrosOficina.CodAlmacen, strEval)
                                                                                          where !ConfigurationManager.AppSettings["ConsTipoMaterialesCHIP"].Contains(p.TipoMaterial)
                                                                                          select p).ToList();

                        oListaMaterialesConPrecio = (from p in BLSincronizaSap.ConsultarPrecio(objForm)
                                                     join planes in oListaMaterialesCampaniaBAM on p.CodigoMaterial equals planes.CodigoMaterial
                                                     select p).ToList();
                    }
                    else
                    {
                        oListaMaterialesCampania = (from p in BLSincronizaSap.ConsultarMaterialXCampania(oListParametrosOficina.CodigoInterlocutor, null, oListParametrosOficina.CodCentro, oListParametrosOficina.CodAlmacen, oListParametrosOficina.TipoOficina)
                                                                               where !ConfigurationManager.AppSettings["ConsTipoMaterialesCHIP"].Contains(p.TipoMaterial)
                                                                               select p).ToList();



                        oListaMaterialesConPrecio = BLSincronizaSap.ConsultarPrecio(objForm);

                    }
                }


            List<BEItemGenerico> objLista = (from materiales in oListaMaterialesCampania
                                             join planes in oListaMaterialesConPrecio on materiales.CodigoMaterial equals planes.CodigoMaterial
                                             select new BEItemGenerico(materiales.CodigoMaterial, materiales.DescripcionMaterial)).GroupBy(g => g.Codigo).Select(s => s.First()).ToList<BEItemGenerico>();

            return objLista;
        }
        }
        private List<BEItemGenerico> ObtenerListaPrecioPVUDB(BEFormEvaluacion objForm)
        {
            //string strTipoVenta = ConfigurationManager.AppSettings["strTVPostpago"].ToString();
            //string strTipoOperacion = ConfigurationManager.AppSettings["ConstTipoOperacionAlta"].ToString();
            BEConsultaDatosOficina oListParametrosOficina = BLSincronizaSap.ConsultaDatosOficina(objForm.idOficina, null);
            //objForm.idTipoVenta = strTipoVenta;
            //objForm.idTipoOperacion = strTipoOperacion;
            objForm.tipoOficina = oListParametrosOficina.TipoOficina;
            objForm.idDepartamento = oListParametrosOficina.CodigoRegion;

            if (objForm.idProducto == ConfigurationManager.AppSettings["constTipoProductoBAM"])
            {
                objForm.idPlan = objForm.idPlanSap;
            }
            List<BEItemGenerico> objLista = (from p in BLSincronizaSap.ConsultarPrecio(objForm)
                                             select new BEItemGenerico(p.CodigoListaPrecio, p.DescripcionListaPrecios, p.PrecioBase)).ToList<BEItemGenerico>();

            return objLista;
        }

        //PROY-140736 INI
        private List<BEItemGenerico> ObtenerListaPrecioBuyBack(BEFormEvaluacion objForm)
        {
            
          

            List<BEItemGenerico> objLista = (from p in BLSincronizaSap.ConsultarPrecioBuyback(objForm)
                                             select new BEItemGenerico(p.CodigoListaPrecio, p.DescripcionListaPrecios, p.PrecioBase)).ToList<BEItemGenerico>();

            return objLista;
        }
        //PROY-140736 FIN

        private bool ValidarAccesoOpcion(string strCanalCodigo, string strProductoCodigo, string strTipoOperacion, string strTipoDocumento, string strTipoValidacion)
        {
            bool booResultado = true;
            BLConsumer objConsumerNegocio;
                objConsumerNegocio = new BLConsumer();
                booResultado = objConsumerNegocio.ValidacionAccesoOpcionEP(strCanalCodigo, strProductoCodigo, strTipoOperacion, strTipoDocumento, strTipoValidacion);

            return booResultado;
        }

        //rsm 30859 INI
        public void InicializarParametrosBAM() 
        {
            try
            {
                GeneradorLog objLog = new GeneradorLog(CurrentUser, "PROY30859-PARAMETROS BAM", null, "WEB");
                long codParamGrupoMaterialesBAM = Funciones.CheckInt64(ConfigurationManager.AppSettings["codParamGrupoMaterialesBAM"]);
                List<BEParametro> listaConstantesMaterialesBAM = (new BLGeneral()).ListaParametrosGrupo(codParamGrupoMaterialesBAM);

                activacionValidacionMaterialesBAM = listaConstantesMaterialesBAM.Where(p => p.Valor1 == "FlagActivarBam").SingleOrDefault().Valor;
                tipoOperacionesPermitidosBAM = listaConstantesMaterialesBAM.Where(p => p.Valor1 == "FlagTOperacionPermitidaBam").SingleOrDefault().Valor;
                canalesPermitidosBAM = listaConstantesMaterialesBAM.Where(p => p.Valor1 == "FlagCanalPermitidoBam").SingleOrDefault().Valor;
                modalidadVentasPermitidasBAM = listaConstantesMaterialesBAM.Where(p => p.Valor1 == "FlagModVentaBam").SingleOrDefault().Valor;

                objLog.CrearArchivolog("[activacionValidacionMaterialesBAM]", activacionValidacionMaterialesBAM, null);
                objLog.CrearArchivolog("[tipoOperacionesPermitidosBAM]", tipoOperacionesPermitidosBAM, null);
                objLog.CrearArchivolog("[canalesPermitidosBAM]", canalesPermitidosBAM, null);
                objLog.CrearArchivolog("[modalidadVentasPermitidasBAM]", modalidadVentasPermitidasBAM, null);
            }
            catch (Exception e)
            {
                activacionValidacionMaterialesBAM = "1";
                tipoOperacionesPermitidosBAM = "|1||2||3||4|";
                canalesPermitidosBAM = "|01||02||03|";
                modalidadVentasPermitidasBAM = "|2||3|";
            }
           
        }
        //rsm 30859 FIN

        //INICIO|PROY-140533 - CONSULTA STOCK
        public static String ConsultarStock(String strPDV, String strCodMaterial, String strTipoProducto)
        {
            var strRespuestaConsulta = String.Empty;
            var strCodRpta = String.Empty;
            var strMsgRpta = String.Empty;
            var strCantidad = String.Empty;
            //var blFlagPicking = false;
            //strRespuestaConsulta = "NO PICKING";

            GeneradorLog objLog = new GeneradorLog(CurrentUsers, "ConsultarStock()", null, "WEB");

            //blFlagPicking = consultaFlagPicking();

            //if (blFlagPicking)
            //{
            //    if (Funciones.CheckStr(AppSettings.Key_ProductosPermitidosPicking).Contains(strTipoProducto))
            //    {
            BEConsultaDatosOficina oListConsultaDatosOficina = BLSincronizaSap.ConsultaDatosOficina(Funciones.CheckStr(strPDV), null);
            string codigoSinergia = Funciones.CheckStr(oListConsultaDatosOficina.CodigoInterlocutor);

            objLog.CrearArchivolog("[INPUT][codigoSinergia]: ", Funciones.CheckStr(codigoSinergia), null);
            objLog.CrearArchivolog("[INPUT][strCodMaterial]: ", Funciones.CheckStr(strCodMaterial), null);
            strCantidad = BLSincronizaSap.ConsultarStock(codigoSinergia, strCodMaterial, out  strCodRpta, out  strMsgRpta);
            objLog.CrearArchivolog("[OUTPUT][strCodRpta]: ", Funciones.CheckStr(strCodRpta), null);
            objLog.CrearArchivolog("[OUTPUT][strMsgRpta]: ", Funciones.CheckStr(strMsgRpta), null);

            var cantidad = Funciones.CheckInt(strCantidad);
            strRespuestaConsulta = Funciones.CheckStr(cantidad);
            //}
            //else
            //{
            //    strRespuestaConsulta = "NO PICKING";
            //}
            //}
            //else
            //{
            //    strRespuestaConsulta = "NO PICKING";
            //}

            objLog.CrearArchivolog("strRespuestaConsulta: ", Funciones.CheckStr(strRespuestaConsulta), null);

            return strRespuestaConsulta;
        }

        public static bool consultaFlagPicking()
        {
            var blFlagPicking = false;
            var strFlagPicking = String.Empty;
            var strFlagPickingDelivery = String.Empty;
            BEUsuarioSession oPuntoVenta = new BEUsuarioSession();
            oPuntoVenta = (BEUsuarioSession)HttpContext.Current.Session["Usuario"];

            GeneradorLog objLog = new GeneradorLog(CurrentUsers, "consultaFlagPicking()", null, "WEB");
            var oListConsultaDatosOficina = BLSincronizaSap.ConsultaDatosOficina(Funciones.CheckStr(oPuntoVenta.OficinaVenta), null);

            var strCanal = String.Empty;
            strCanal = Funciones.CheckStr(oListConsultaDatosOficina.TipoOficina);

            if (strCanal.Equals(Funciones.CheckStr(ConfigurationManager.AppSettings["constCodTipoOficinaCAC"])))
            {
                BEItemGenerico listPicking = new BEItemGenerico();
                string codigo_rptaPDK = string.Empty;
                string mensaje_rptaPDK = string.Empty;
                string descripcion_oficinaPDK = string.Empty;

                objLog.CrearArchivolog("[INPUT][oPuntoVenta.OficinaVenta]: ", Funciones.CheckStr(oPuntoVenta.OficinaVenta), null);
                listPicking = BLGeneral.ConsultarFlagsPicking(Funciones.CheckStr(oPuntoVenta.OficinaVenta), ref codigo_rptaPDK, ref mensaje_rptaPDK);
                objLog.CrearArchivolog("[OUTPUT][codigo_rptaPDK]: ", Funciones.CheckStr(codigo_rptaPDK), null);
                objLog.CrearArchivolog("[OUTPUT][mensaje_rptaPDK]: ", Funciones.CheckStr(mensaje_rptaPDK), null);

                if (listPicking != null)
                {
                    strFlagPicking = Funciones.CheckStr(listPicking.Codigo2);
                    strFlagPickingDelivery = Funciones.CheckStr(listPicking.Codigo3);

                    if (strFlagPicking == "1")
                    {
                        blFlagPicking = true;
                    }
                    else
                    {
                        blFlagPicking = false;
                    }
                }
            }

            objLog.CrearArchivolog("blFlagPicking: ", Funciones.CheckStr(blFlagPicking), null);

            return blFlagPicking;
        }
        //FIN|PROY-140533 - CONSULTA STOCK

        #region [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil]
        public List<BEItemGenerico> ObtenerStockXMaterial(BEFormEvaluacion objForm, BEConsultaDatosOficina oListParametrosOficina)
        {
            string strArchivo = "ObtenerStockXMaterial()";
            string strTipoMaterial = string.Empty;
            string strFlagProcesaPLC = string.Empty;
            string strFlagServicio = "1";
            string tipoOficina = objForm.tipoOficina;
            string strTipoMaterialesConSTock = ConfigurationManager.AppSettings["ConsMaterialesVentasVariasConStock"];
            string[] arrTipoMaterial = Funciones.CheckStr(ReadKeySettings.Key_TipoMatVentaVarias).Split('|');
            List<BEItemGenerico> oBeLstRpta = new List<BEItemGenerico>();
            List<BEItemGenerico> oBeListaChip = new List<BEItemGenerico>();
            List<BEItemGenerico> oBeListaChips = new List<BEItemGenerico>();
            List<BEItemGenerico> oBeListarArticulos = new List<BEItemGenerico>(); ;
            int codPromocion = Funciones.CheckInt(objForm.idPromocion);

            GeneradorLog _objLog = new GeneradorLog(CurrentUser, "", null, "ObtenerStockXMaterial");
            _objLog.CrearArchivolog("PROY-140743 - obj.idPromocion : ", Funciones.CheckStr(objForm.idPromocion), null);

            try
            {
                if (tipoOficina == ConfigurationManager.AppSettings["ConsTipoOficinaCAC"] && strTipoMaterialesConSTock.Contains(strTipoMaterial))
                    strFlagServicio = "0";

                for (int i = 0; i < arrTipoMaterial.Length; i++)
                {
                    strTipoMaterial = Funciones.CheckStr(arrTipoMaterial[i]);

                    if (strTipoMaterial == Funciones.CheckStr(ConfigurationManager.AppSettings["ConsTipoMaterialAccesorios"]))
                        strFlagProcesaPLC = "0";
                    else if (strTipoMaterial == Funciones.CheckStr(HttpContext.Current.Session["CodTipoMaterialPLC"]))
                    {
                        strTipoMaterial = Funciones.CheckStr(ConfigurationManager.AppSettings["ConsTipoMaterialAccesorios"]);
                        strFlagProcesaPLC = "1";
                    }

                    GeneradorLog.EscribirLog(strArchivo, "ObtenerStockXMaterial", string.Format("{0} {1}, {2} {3}, {4} {5}, {6} {7}, {8} {9}", "Inicio ConsultarStockXTipoMaterial -> K_OFICV_CODOFICINA:", oListParametrosOficina.CodigoInterlocutor,
                        "K_OFICC_TIPOOFICINA:", oListParametrosOficina.TipoOficina, "K_OFICC_FLAGSERVICIO:", strFlagServicio, "K_GRUPO_MATERIAL", strTipoMaterial, "K_FLAG_PROCESA_PLC", strFlagProcesaPLC));

                    oBeListarArticulos = (from p in BLSincronizaSap.ConsultarStockXTipoMaterial(new BEParametrosMSSAP() { CodigoOficina = oListParametrosOficina.CodigoInterlocutor, TipoOficina = oListParametrosOficina.TipoOficina, FlagServicio = strFlagServicio, tipoMaterial = strTipoMaterial, strFlagProcesaPLC = strFlagProcesaPLC })
                                          select new BEItemGenerico() { Codigo = p.CodigoMaterial, Descripcion = p.DescripcionMaterial, Tipo = p.TipoMaterial }).ToList<BEItemGenerico>();

                    if (strFlagProcesaPLC == "1")
                        strTipoMaterial = ConfigurationManager.AppSettings["ConsTipoMaterialAccesoriosFija"];

                    oBeListaChips = new List<BEItemGenerico>();

                    GeneradorLog.EscribirLog(strArchivo, "ObtenerStockXMaterial", string.Format("{0} {1}", "Inicio ListaChips ->", strTipoMaterial));
                    if (strTipoMaterial == ConfigurationManager.AppSettings["ConsTipoMaterialMerchandising"])
                    {
                        oBeListaChips = (from p in oBeListarArticulos
                                         where BLSincronizaSap.ListCodigosMerchandising().Contains(p.Tipo)
                                         select p).OrderBy(o => o.Descripcion).ToList();
                    }
                    else if (strTipoMaterial == ConfigurationManager.AppSettings["ConsTipoMaterialServicios"])
                    {
                        GeneradorLog.EscribirLog(strArchivo, "Inicio Método ConsultarParametroByGrupo(intCodGrupo)");
                        List<BEParametro> lstParamServicios = new BLGeneral().ListaParametrosGrupo(Funciones.CheckInt(ConfigurationManager.AppSettings["consParamGrupo_Servicios"]));
                        GeneradorLog.EscribirLog(strArchivo, "Fin Método ConsultarParametroByGrupo(intCodGrupo)");
                        List<BEItemGenerico> oBeListaServi = null;
                        oBeListaServi = (from p in oBeListarArticulos
                                         where BLSincronizaSap.ListCodigosServicios().Contains(p.Tipo)
                                         select p).ToList();

                        oBeListaChips = oBeListaServi.Where(a => lstParamServicios.Select(s => s.Valor).Contains(a.Codigo)).ToList();
                    }
                    else if (strTipoMaterial == ConfigurationManager.AppSettings["ConsTipoMaterialRecarga"])
                    {
                        oBeListaChips = (from p in oBeListarArticulos
                                         where BLSincronizaSap.ListCodigosRecarga().Contains(p.Tipo)
                                         select p).OrderBy(o => o.Descripcion).ToList();
                    }
                    else if (strTipoMaterial == ConfigurationManager.AppSettings["ConsTipoMaterialAccesorios"])
                    {
                        oBeListaChips = (from p in oBeListarArticulos
                                         where BLSincronizaSap.ListCodigosAccesorios().Contains(p.Tipo)
                                         select p).OrderBy(o => o.Descripcion).ToList();

                        List<BEPromocion> lstPromoVigente = new List<BEPromocion>();
                        List<BEPromocionCombinacion> lstCombinacionesPromoVigente = new List<BEPromocionCombinacion>();
                        List<BEItemGenerico> lstAccesoriosEnPromocionList = new List<BEItemGenerico>();
                        List<BESolicitud> lstSolicitudesCliente = new List<BESolicitud>();

                        GeneradorLog.EscribirLog(strArchivo, "ObtenerStockXMaterial", string.Format("{0} {1}", "codPromocion", codPromocion));
                        GeneradorLog.EscribirLog(strArchivo, "ObtenerStockXMaterial", string.Format("{0} {1}", "idCampana", objForm.idCampana));
                        if (codPromocion == Funciones.CheckInt(ConfigurationManager.AppSettings["ValorSeleccionar"]))
                        {
                            oBeListaChips = null;
                        }
                        else if (codPromocion == Funciones.CheckInt(ConfigurationManager.AppSettings["ConsCodSinPromocion"]) && Funciones.CheckInt(objForm.idCampana) != 1778)
                        {
                            //MBC
                            lstAccesoriosEnPromocionList = (List<BEItemGenerico>)HttpContext.Current.Session["ListaAccesoriosEnPromocion"];
                            oBeListaChips.RemoveAll(a => lstAccesoriosEnPromocionList.Any(b => a.Codigo == b.Codigo));
                            GeneradorLog.EscribirLog(strArchivo, "ListarArticulos-ConsCodSinPromocion", string.Format("{0} {1}", "oBeListaChips ->", oBeListaChips.Count()));
                        }
                        else if (codPromocion == Funciones.CheckInt(ConfigurationManager.AppSettings["ConsCodSinPromocion"]) && Funciones.CheckInt(objForm.idCampana) == 1778)
                        {
                            oBeListaChips = null;
                        }
                        else
                        {
                            lstPromoVigente = (List<BEPromocion>)HttpContext.Current.Session["ListaPromocionesVigentes"];
                            GeneradorLog.EscribirLog(strArchivo, "ObtenerStockXMaterial", string.Format("{0} {1}", "lstPromoVigente ->", lstPromoVigente.Count()));

                            lstPromoVigente = lstPromoVigente.Where(p => p.PACCN_CODIGO_PROMO == codPromocion).ToList();
                            GeneradorLog.EscribirLog(strArchivo, "ObtenerStockXMaterial", string.Format("{0} {1}", "lstPromoVigente ->", lstPromoVigente.Count()));

                            lstCombinacionesPromoVigente = (List<BEPromocionCombinacion>)HttpContext.Current.Session["ListaCombinacionesPromocionVigentes"];
                            GeneradorLog.EscribirLog(strArchivo, "ObtenerStockXMaterial", string.Format("{0} {1}", "lstCombinacionesPromoVigente ->", lstCombinacionesPromoVigente.Count()));

                            lstCombinacionesPromoVigente = lstCombinacionesPromoVigente.Where(a => a.PCOMN_CODIGO_PROMO == codPromocion).ToList();
                            GeneradorLog.EscribirLog(strArchivo, "ObtenerStockXMaterial", string.Format("{0} {1}", "lstCombinacionesPromoVigente ->", lstCombinacionesPromoVigente.Count()));

                            lstSolicitudesCliente = (List<BESolicitud>)HttpContext.Current.Session["ListaSolicitudesCliente"];
                            GeneradorLog.EscribirLog(strArchivo, "ObtenerStockXMaterial", string.Format("{0} {1}", "lstSolicitudesCliente ->", lstSolicitudesCliente.Count()));

                            List<BESolicitud> lstSolicitudesParaPromo = new List<BESolicitud>();
                            lstSolicitudesParaPromo = ((from s in lstSolicitudesCliente
                                                        from c in lstCombinacionesPromoVigente
                                                        where lstPromoVigente[0].PACCV_CONCAT_COD_CANAL.Contains(s.TOFIC_CODIGO)
                                                              && lstPromoVigente[0].PACCV_CONCAT_MOD_VENTA.Contains(Funciones.CheckStr(s.MODALIDAD_VENTA))
                                                              && lstPromoVigente[0].PACCV_CONCAT_COD_TIP_CLIEN.Contains(s.TCLIC_CODIGO)
                                                              && (lstPromoVigente[0].PACCV_CONCAT_COD_TIP_OPERA.Contains(s.TOPEN_CODIGO)
                                                               || lstPromoVigente[0].PACCV_CONCAT_COD_TIP_OPERA.Contains(s.FLAG_PORTABILIDAD))
                                                              && lstPromoVigente[0].PACCN_VIGENCIA_SEC >= s.DIAS_ANTIGUEDAD
                                                              && c.PCOMV_COD_PLAN == s.PLAN_TARIFARIO
                                                              && c.PCOMV_COD_MAT_EQUIPO == (c.PCOMV_COD_MAT_EQUIPO == ConfigurationManager.AppSettings["ConsCodMaterialTodosEquipos"] ? c.PCOMV_COD_MAT_EQUIPO : s.COD_EQUIPO)
                                                        select new BESolicitud
                                                        {
                                                            SOLIN_CODIGO = s.SOLIN_CODIGO,
                                                            NUM_CONTRATO = s.NUM_CONTRATO,
                                                            NUM_CORRELA_CONTRATO = s.NUM_CORRELA_CONTRATO,
                                                            NUM_VENTA = s.NUM_VENTA,
                                                            NUM_PEDIDO = s.NUM_PEDIDO,
                                                            COD_EQUIPO = s.COD_EQUIPO,
                                                            PRECIOBASE_EQUIPO = s.PRECIOBASE_EQUIPO,
                                                            SERIE_EQUIPO = s.SERIE_EQUIPO,
                                                            PLAN_TARIFARIO = s.PLAN_TARIFARIO,
                                                            PLANN_CAR_FIJ = s.PLANN_CAR_FIJ,
                                                            CAMPN_CODIGO = s.CAMPN_CODIGO,
                                                            TELEFONO = s.TELEFONO,
                                                            FECHA_CONTRATO = s.FECHA_CONTRATO,
                                                            USUARIO_CREA_CONTRATO = s.USUARIO_CREA_CONTRATO,
                                                            TOFIC_CODIGO = s.TOFIC_CODIGO,
                                                            TCLIC_CODIGO = s.TCLIC_CODIGO,
                                                            TOPEN_CODIGO = s.TOPEN_CODIGO,
                                                            FLAG_PORTABILIDAD = s.FLAG_PORTABILIDAD,
                                                            MODALIDAD_VENTA = s.MODALIDAD_VENTA,
                                                            DIAS_ANTIGUEDAD = s.DIAS_ANTIGUEDAD
                                                        }).GroupBy(s1 => new { s1.NUM_CONTRATO, s1.NUM_CORRELA_CONTRATO }).Select(s2 => s2.First())).ToList();

                            List<BEPromocionCombinacion> lstCombinacionesHabilitadas = new List<BEPromocionCombinacion>();
                            lstCombinacionesHabilitadas = (from c in lstCombinacionesPromoVigente
                                                           from s in lstSolicitudesParaPromo
                                                           where c.PCOMV_COD_PLAN == s.PLAN_TARIFARIO
                                                           && c.PCOMV_COD_MAT_EQUIPO == (c.PCOMV_COD_MAT_EQUIPO == ConfigurationManager.AppSettings["ConsCodMaterialTodosEquipos"] ? c.PCOMV_COD_MAT_EQUIPO : s.COD_EQUIPO)
                                                           select new BEPromocionCombinacion
                                                           {
                                                               PCOMN_CODIGO_PROMO = c.PCOMN_CODIGO_PROMO,
                                                               PCOMN_CODIGO_COMBINACION = c.PCOMN_CODIGO_COMBINACION,
                                                               PCOMV_COD_MAT_EQUIPO = c.PCOMV_COD_MAT_EQUIPO,
                                                               PCOMV_COD_PLAN = c.PCOMV_COD_PLAN,
                                                               PCOMV_COD_MAT_ACCESORIO = c.PCOMV_COD_MAT_ACCESORIO
                                                           }).ToList();

                            HttpContext.Current.Session["ListaSolicitudesClienteParaPromo"] = lstSolicitudesParaPromo;
                            HttpContext.Current.Session["ListaCombinacionesHabilitadas"] = lstCombinacionesHabilitadas;

                            oBeListaChips = oBeListaChips.Where(a => lstCombinacionesHabilitadas.Any(b => a.Codigo == b.PCOMV_COD_MAT_ACCESORIO)).ToList();
                        }
                    }
                    else if (strTipoMaterial == ConfigurationManager.AppSettings["ConsTipoMaterialesCHIP"])
                    {
                        oBeListaChips = (from p in oBeListarArticulos
                                         where BLSincronizaSap.ListCodigosTipoCHIP().Contains(p.Tipo)
                                         select p).OrderBy(o => o.Descripcion).ToList();
                    }
                    else if (strTipoMaterial == ConfigurationManager.AppSettings["ConsTipoMaterialEQUIPO"])
                    {
                        oBeListaChips = (from p in oBeListarArticulos
                                         where BLSincronizaSap.ListCodigosTipoEQUIPO().Contains(p.Tipo)
                                         select p).OrderBy(o => o.Descripcion).ToList();
                    }
                    else if (strTipoMaterial == ConfigurationManager.AppSettings["ConsTipoMaterialTarjetas"])
                    {
                        oBeListaChips = (from p in oBeListarArticulos
                                         where BLSincronizaSap.ListCodigosTarjetas().Contains(p.Tipo)
                                         select p).OrderBy(o => o.Descripcion).ToList();
                    }
                    else if (strTipoMaterial == Funciones.CheckStr(ConfigurationManager.AppSettings["ConsTipoMaterialAccesoriosFija"]))
                    {
                        oBeListaChips = (from p in oBeListarArticulos select p).OrderBy(o => o.Descripcion).ToList();
                    }
                    else if (strTipoMaterial == Funciones.CheckStr(ConfigurationManager.AppSettings["ConsTipoMaterialOIT"])) //CAMBIO MOSTRAR MATERIALES IOT INI
                    {
                        GeneradorLog.EscribirLog(strArchivo, "ObtenerStockXMaterial", string.Format("{0} {1}", "INICIO CONSULTA MATERIAL IOT ->", ConfigurationManager.AppSettings["ConsTipoMaterialOIT"]));
                        GeneradorLog.EscribirLog(strArchivo, "ObtenerStockXMaterial", string.Format("{0} {1}", "oBeListarArticulos ->", oBeListarArticulos.Count()));
                        oBeListaChips = (from p in oBeListarArticulos
                                         where Funciones.CheckStr(ConfigurationManager.AppSettings["ConsTipoMaterialOIT"]).Contains(p.Tipo)
                                         select p).OrderBy(o => o.Descripcion).ToList();
                        GeneradorLog.EscribirLog(strArchivo, "ObtenerStockXMaterial", string.Format("{0} {1}", "oBeListaChips ->", oBeListaChips.Count()));
                        GeneradorLog.EscribirLog(strArchivo, "ObtenerStockXMaterial", string.Format("{0} {1}", "FIN CONSULTA MATERIAL IOT ->", ConfigurationManager.AppSettings["ConsTipoMaterialOIT"]));
                    }

                    oBeListaChip.AddRange(oBeListaChips);
                }

                //FILTO ACC CUOTAS INI
                GeneradorLog.EscribirLog(strArchivo, "ObtenerStockXMaterial", string.Format("{0} {1}", "oBeListaChip Inicial ->", oBeListaChip.Count()));

                List<BEConsultaStock> lstAccesorioCuotas = new List<BEConsultaStock>();
                lstAccesorioCuotas = BLSincronizaSap.listarAccesoriosCuotas();

                GeneradorLog.EscribirLog(strArchivo, "ObtenerStockXMaterial", string.Format("{0} {1}", "lstAccesorioCuotas ->", lstAccesorioCuotas.Count()));

                oBeListaChip = oBeListaChip.Where(a => lstAccesorioCuotas.Any(b => a.Codigo == b.CodigoMaterial)).ToList();

                GeneradorLog.EscribirLog(strArchivo, "ObtenerStockXMaterial", string.Format("{0} {1}", "oBeListaChips Final ->", oBeListaChip.Count()));
                //FILTO ACC CUOTAS FIN

                oBeLstRpta.AddRange(oBeListaChip);
                HttpContext.Current.Session["oBeLstRptaVV"] = oBeLstRpta;

            }
            catch (Exception ex)
            {
                GeneradorLog.EscribirLog(strArchivo, "ObtenerStockXMaterial", string.Format("{0} {1}", "Error - StackTrace  ->", ex.Message));
                GeneradorLog.EscribirLog(strArchivo, "ObtenerStockXMaterial", string.Format("{0} {1}", "Error - Message  ->", ex.StackTrace));
            }

            return oBeLstRpta;
        }

        private string llenarPromociones(BEFormEvaluacion objForm)
        {
            GeneradorLog objLog = new GeneradorLog(CurrentUser, objForm.nroDocumento, null, "WEB");
            objLog.CrearArchivolog("[Inicio][llenarPromociones]", null, null);
            string strResultado = string.Empty;
            strResultado = ConsultarListaPromocionesHabilitadasParaCliente(objForm);
            objLog.CrearArchivolog("[LISTAR_PROMOCIONES]", strResultado.ToString(), null);
            objLog.CrearArchivolog("[Fin][llenarPromociones]", null, null);
            return strResultado;
        }

        private string ConsultarListaPromocionesHabilitadasParaCliente(BEFormEvaluacion objForm)
        {
            string strArchivo = "[PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] - [ConsultarListaPromocionesHabilitadasParaCliente]";
            string strCodigoCampania = objForm.idCampana;
            string strTipoOficina = objForm.idTipoOficina;
            string strNroDocCliente = objForm.nroDocumento;
            string strTipoDocCliente = objForm.tipoDocumento;
            GeneradorLog objLog = new GeneradorLog(CurrentUsers, "ConsultarListaPromocionesHabilitadasParaCliente()", null, "WEB");

            objLog.CrearArchivolog("[strCodigoCampania]: ", Funciones.CheckStr(strCodigoCampania), null);
            DataSet dsPromocionesTotal = BLGeneral.ConsultarPromocionesVigentesxCampania(strCodigoCampania);
            objLog.CrearArchivolog("Total dsPromocionesTotal: ", Funciones.CheckStr(dsPromocionesTotal.Tables.Count), null);
            List<BEPromocion> lstPromoVigxCampana = new List<BEPromocion>();
            List<BEPromocionCombinacion> lstPromoCombinVigxCampana = new List<BEPromocionCombinacion>();
            List<BEPromocion> lstPromoHabilCanal = new List<BEPromocion>();
            List<BEPromocion> lstPromoHabilCliente = new List<BEPromocion>();
            List<BEItemGenerico> lstPromoHabilitadasFinal = new List<BEItemGenerico>();
            List<BEItemGenerico> lstAccesoriosEnPromocion = new List<BEItemGenerico>();
            List<BESolicitud> lstSolicitudesCliente = new List<BESolicitud>();
            List<BESolicitud> lstSolicitudesPrepago = new List<BESolicitud>();
            List<BEItemGenerico> lstCampanias = new List<BEItemGenerico>();

            if (dsPromocionesTotal.Tables.Count > 0)
            {
                DataTable dtPromociones = dsPromocionesTotal.Tables[0];
                DataTable dtPromoCombinaciones = dsPromocionesTotal.Tables[1];
                DataTable dtAccesoriosPromo = dsPromocionesTotal.Tables[2];
                
                GeneradorLog.EscribirLog(strArchivo, string.Format("{0} {1}", "dtPromociones:", Funciones.CheckStr(dtPromociones.Rows.Count)));
                GeneradorLog.EscribirLog(strArchivo, string.Format("{0} {1}", "dtPromoCombinaciones:", Funciones.CheckStr(dtPromoCombinaciones.Rows.Count)));
                GeneradorLog.EscribirLog(strArchivo, string.Format("{0} {1}", "dtAccesoriosPromo:", Funciones.CheckStr(dtAccesoriosPromo.Rows.Count)));
                

                foreach (DataRow promo in dtPromociones.Rows)
                {
                    BEPromocion item = new BEPromocion();
                    item.PACCN_CODIGO_PROMO = Funciones.CheckInt(promo["PACCN_CODIGO_PROMO"]);
                    item.PACCV_DESC_PROMO = Funciones.CheckStr(promo["PACCV_DESC_PROMO"]);
                    item.PACCV_COD_CAMPANA = Funciones.CheckStr(promo["PACCV_COD_CAMPANA"]);
                    item.PACCV_CONCAT_COD_TIP_OPERA = Funciones.CheckStr(promo["PACCV_CONCAT_COD_TIP_OPERA"]);
                    item.PACCV_CONCAT_COD_TIP_CLIEN = Funciones.CheckStr(promo["PACCV_CONCAT_COD_TIP_CLIEN"]);
                    item.PACCV_CONCAT_COD_CANAL = Funciones.CheckStr(promo["PACCV_CONCAT_COD_CANAL"]);
                    item.PACCV_CONCAT_MOD_VENTA = Funciones.CheckStr(promo["PACCV_CONCAT_MOD_VENTA"]);
                    item.PACCN_VIGENCIA_SEC = Funciones.CheckInt(promo["PACCN_VIGENCIA_SEC"]);
                    lstPromoVigxCampana.Add(item);
                }

                lstPromoHabilCanal = lstPromoVigxCampana.Where(x => x.PACCV_CONCAT_COD_CANAL.Contains(strTipoOficina)).ToList();
                int intMaxDiasBuscaSEC = 0;

                GeneradorLog.EscribirLog(strArchivo, string.Format("{0} {1}", "lstPromoHabilCanal:", Funciones.CheckStr(lstPromoHabilCanal.Count)));

                foreach (BEPromocion promocion in lstPromoHabilCanal)
                    if (promocion.PACCN_VIGENCIA_SEC > intMaxDiasBuscaSEC) intMaxDiasBuscaSEC = promocion.PACCN_VIGENCIA_SEC;

                lstCampanias = (List<BEItemGenerico>)HttpContext.Current.Session["lstCampaniasXTipoVenta"];
                var TipoVenta = lstCampanias.Where(x => x.Codigo == strCodigoCampania).Select(x => x.Codigo3).First();
                GeneradorLog.EscribirLog(strArchivo, string.Format("{0} {1}", "lstSolicitudesCliente:", Funciones.CheckStr(lstSolicitudesCliente.Count)));
                GeneradorLog.EscribirLog(strArchivo, string.Format("{0} {1}", "tipoDeVentaCampana:", Funciones.CheckStr(TipoVenta.ToString())));  

                foreach (DataRow combin in dtPromoCombinaciones.Rows)
                {
                    BEPromocionCombinacion item = new BEPromocionCombinacion();

                    if (Funciones.CheckStr(combin["PCOMV_COD_TIPO_PROD"]) != "")
                    {
                        //MBC
                        #region [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] - [Logica consulta SOT]
                        if (validarSOT(strTipoDocCliente, strNroDocCliente, Funciones.CheckStr(combin["PCOMV_COD_TIPO_PROD"]), intMaxDiasBuscaSEC))
                        {
                            item.PCOMN_CODIGO_PROMO = Funciones.CheckInt(combin["PCOMN_CODIGO_PROMO"]);
                            item.PCOMV_COD_MAT_EQUIPO = Funciones.CheckStr(combin["PCOMV_COD_MAT_EQUIPO"]);
                            item.PCOMV_COD_PLAN = Funciones.CheckStr(combin["PCOMV_COD_PLAN"]);
                            item.PCOMV_COD_MAT_ACCESORIO = Funciones.CheckStr(combin["PCOMV_COD_MAT_ACCESORIO"]);
                            lstPromoCombinVigxCampana.Add(item);
                        }
                        #endregion
                    }
                    else
                    {
                        item.PCOMN_CODIGO_PROMO = Funciones.CheckInt(combin["PCOMN_CODIGO_PROMO"]);
                        item.PCOMV_COD_MAT_EQUIPO = Funciones.CheckStr(combin["PCOMV_COD_MAT_EQUIPO"]);
                        item.PCOMV_COD_PLAN = Funciones.CheckStr(combin["PCOMV_COD_PLAN"]);
                        item.PCOMV_COD_MAT_ACCESORIO = Funciones.CheckStr(combin["PCOMV_COD_MAT_ACCESORIO"]);
                        lstPromoCombinVigxCampana.Add(item);
                     }
                }
                    
                foreach (DataRow acc in dtAccesoriosPromo.Rows)
                {
                    BEItemGenerico item = new BEItemGenerico();
                    item.Codigo = Funciones.CheckStr(acc["PCOMV_COD_MAT_ACCESORIO"]);
                    lstAccesoriosEnPromocion.Add(item);
                }

                if (TipoVenta.ToString() == ConfigurationManager.AppSettings["ConsTipoVentaPost"].ToString()) 
                {
                    // POSTPAGO
                GeneradorLog.EscribirLog(strArchivo, "ConsultarSolicitudesCliente", string.Format("{0} {1}", "strNroDocCliente ->", Funciones.CheckStr(strNroDocCliente)));
                GeneradorLog.EscribirLog(strArchivo, "ConsultarSolicitudesCliente", string.Format("{0} {1}", "intMaxDiasBuscaSEC ->", Funciones.CheckStr(intMaxDiasBuscaSEC)));
                lstSolicitudesCliente = BLGeneral.ConsultarSolicitudesCliente(strNroDocCliente, intMaxDiasBuscaSEC); //CONSULTAR SECS CLIENTE CON MAX CANTIDAD DE DIAS
                if (lstSolicitudesCliente.Count > 0) //HACER MATCH CON LOS DATOS DE LAS PROMOCIONES                        
                    lstPromoHabilitadasFinal = ((from p in lstPromoHabilCanal
                                                 join c in lstPromoCombinVigxCampana on p.PACCN_CODIGO_PROMO equals c.PCOMN_CODIGO_PROMO
                                                 from s in lstSolicitudesCliente
                                                 where p.PACCV_CONCAT_COD_CANAL.Contains(s.TOFIC_CODIGO)
                                                     && p.PACCV_CONCAT_MOD_VENTA.Contains(Funciones.CheckStr(s.MODALIDAD_VENTA))
                                                     && p.PACCV_CONCAT_COD_TIP_CLIEN.Contains(s.TCLIC_CODIGO)
                                                     && (p.PACCV_CONCAT_COD_TIP_OPERA.Contains(s.TOPEN_CODIGO) || p.PACCV_CONCAT_COD_TIP_OPERA.Contains(s.FLAG_PORTABILIDAD))
                                                     && p.PACCN_VIGENCIA_SEC >= s.DIAS_ANTIGUEDAD
                                                     && c.PCOMV_COD_PLAN == s.PLAN_TARIFARIO
                                                     && c.PCOMV_COD_MAT_EQUIPO == (c.PCOMV_COD_MAT_EQUIPO == ConfigurationManager.AppSettings["ConsCodMaterialTodosEquipos"] ? c.PCOMV_COD_MAT_EQUIPO : s.COD_EQUIPO)
                                                 select new BEItemGenerico
                                                 {
                                                     Codigo = Funciones.CheckStr(p.PACCN_CODIGO_PROMO),
                                                     Descripcion = p.PACCV_DESC_PROMO
                                                 }).GroupBy(p1 => p1.Codigo).Select(p2 => p2.First())).ToList();
                HttpContext.Current.Session["ListaSolicitudesCliente"] = lstSolicitudesCliente;
                }
                else if (TipoVenta.ToString() == ConfigurationManager.AppSettings["constCodTipoVentaPrepago"].ToString()) 
                {
                    // PREPAGO
                    GeneradorLog.EscribirLog(strArchivo, "ConsultarSolicitudesPrepago", string.Format("{0} {1}", "strNroDocCliente ->", Funciones.CheckStr(strNroDocCliente)));
                    GeneradorLog.EscribirLog(strArchivo, "ConsultarSolicitudesPrepago", string.Format("{0} {1}", "intMaxDiasBuscaSEC ->", Funciones.CheckStr(intMaxDiasBuscaSEC)));
                    lstSolicitudesPrepago = BLGeneral.ConsultarSolicitudesPrepago(strNroDocCliente, intMaxDiasBuscaSEC); //CONSULTAR SECS CLIENTE CON MAX CANTIDAD DE DIAS
                    if (lstSolicitudesPrepago.Count > 0) //HACER MATCH CON LOS DATOS DE LAS PROMOCIONES                        
                        lstPromoHabilitadasFinal = ((from p in lstPromoHabilCanal
                                                     join c in lstPromoCombinVigxCampana on p.PACCN_CODIGO_PROMO equals c.PCOMN_CODIGO_PROMO
                                                     from s in lstSolicitudesPrepago
                                                     where p.PACCV_CONCAT_COD_CANAL.Contains(s.TOFIC_CODIGO)
                                                         && p.PACCV_CONCAT_MOD_VENTA.Contains(Funciones.CheckStr(s.MODALIDAD_VENTA))
                                                         && p.PACCV_CONCAT_COD_TIP_CLIEN.Contains(s.TCLIC_CODIGO)
                                                         && (p.PACCV_CONCAT_COD_TIP_OPERA.Contains(s.TOPEN_CODIGO) || p.PACCV_CONCAT_COD_TIP_OPERA.Contains(s.FLAG_PORTABILIDAD == "" ? " " : s.FLAG_PORTABILIDAD))
                                                         && p.PACCN_VIGENCIA_SEC >= s.DIAS_ANTIGUEDAD
                                                         && c.PCOMV_COD_PLAN == s.PLAN_TARIFARIO
                                                         && c.PCOMV_COD_MAT_EQUIPO == (c.PCOMV_COD_MAT_EQUIPO == ConfigurationManager.AppSettings["ConsCodMaterialTodosEquipos"] ? c.PCOMV_COD_MAT_EQUIPO : s.COD_EQUIPO)
                                                     select new BEItemGenerico
                                                     {
                                                         Codigo = Funciones.CheckStr(p.PACCN_CODIGO_PROMO),
                                                         Descripcion = p.PACCV_DESC_PROMO
                                                     }).GroupBy(p1 => p1.Codigo).Select(p2 => p2.First())).ToList();
                    HttpContext.Current.Session["ListaSolicitudesCliente"] = lstSolicitudesPrepago;
                }
            }

                HttpContext.Current.Session["ListaPromocionesVigentes"] = lstPromoHabilCanal;
                HttpContext.Current.Session["ListaCombinacionesPromocionVigentes"] = lstPromoCombinVigxCampana;
                HttpContext.Current.Session["ListaAccesoriosEnPromocion"] = lstAccesoriosEnPromocion;
                GeneradorLog.EscribirLog(strArchivo, string.Format("{0} {1}", "lstPromoHabilCanal:", Funciones.CheckStr(lstPromoHabilCanal.Count)));
                GeneradorLog.EscribirLog(strArchivo, string.Format("{0} {1}", "lstPromoCombinVigxCampana:", Funciones.CheckStr(lstPromoCombinVigxCampana.Count)));
                GeneradorLog.EscribirLog(strArchivo, string.Format("{0} {1}", "lstAccesoriosEnPromocion:", Funciones.CheckStr(lstAccesoriosEnPromocion.Count)));
                GeneradorLog.EscribirLog(strArchivo, string.Format("{0} {1}", "lstSolicitudesCliente:", Funciones.CheckStr(lstSolicitudesCliente.Count)));
                
            StringBuilder sbPromociones = new StringBuilder();
            string[] arrSinPromocion = null;

            if (lstPromoHabilitadasFinal.Count > 0)
            {
                foreach (BEItemGenerico obj in lstPromoHabilitadasFinal)
                {
                    sbPromociones.Append("|");
                    sbPromociones.Append(obj.Codigo);
                    sbPromociones.Append(";");
                    sbPromociones.Append(obj.Descripcion);
                }
            }
            else
            {
              arrSinPromocion =  Funciones.CheckStr(ReadKeySettings.Key_OpcionSinPromocion).Split('|');
              sbPromociones.Append("|");
              sbPromociones.Append(arrSinPromocion[1]);
              sbPromociones.Append(";");
              sbPromociones.Append(arrSinPromocion[0]);

            }
            return sbPromociones.ToString();
        }

        private string llenarLineasAsociadas(BEFormEvaluacion objForm)
        {
            GeneradorLog objLog = new GeneradorLog(CurrentUser, objForm.nroDocumento, null, "WEB");
            objLog.CrearArchivolog("[Inicio][llenarLineasAsociadas]", null, null);
            string strResultado = string.Empty;
            strResultado = ConsultarLineasAsociadas(objForm);
            objLog.CrearArchivolog("[LISTAR_PROMOCIONES]", strResultado.ToString(), null);
            objLog.CrearArchivolog("[Fin][llenarLineasAsociadas]", null, null);
            return strResultado;
        }

        private string ConsultarLineasAsociadas(BEFormEvaluacion objForm)
        {
            GeneradorLog objLog = new GeneradorLog(CurrentUser, objForm.nroDocumento, null, "WEB");
            BEClienteCuenta objCliente = null;
            DataTable dtDetalleBSCS;
            DataTable dtDetalleSISACT;
            StringBuilder sbPromociones = new StringBuilder();

            objLog.CrearArchivolog("[INICIO][ConsultarLineasAsociadas]", null, null);

            try
            {
                objCliente = (BEClienteCuenta)HttpContext.Current.Session["objCliente" + objForm.nroDocumento];

                if (objCliente != null)
                {

                    if (objCliente.lineaBSCS != null || objCliente.lineaSISACT != null)
                    {
                        dtDetalleBSCS = objCliente.lineaBSCS;
                        dtDetalleSISACT = objCliente.lineaSISACT;

                        var estado = string.Empty;
                        var codigo_cuenta = string.Empty;
                        var customer_id = string.Empty;
                        var estado_Fijo = Funciones.CheckStr(ConfigurationManager.AppSettings["varEstadoAct"]).ToUpper();

                        foreach (DataRow dr in dtDetalleBSCS.Rows)
                        {
                            estado = Funciones.CheckStr(dr["ESTADO"]).ToUpper();
                            codigo_cuenta = Funciones.CheckStr(dr["CODIGO_CUENTA"]);
                            customer_id = Funciones.CheckStr(dr["CUSTOMER_ID"]);

                            objLog.CrearArchivolog(string.Format("{0} ===> {1} ", "[PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil][CODIGO_CUENTA]", codigo_cuenta), null, null);
                            objLog.CrearArchivolog(string.Format("{0} ===> {1} ", "[PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil][CUSTOMER_ID]", customer_id), null, null);
                            objLog.CrearArchivolog(string.Format("{0} ===> {1} ", "[PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil][ESTADO]", estado), null, null);

                            if (estado.Equals(estado_Fijo))
                            {
                                sbPromociones.Append("|");
                                sbPromociones.Append(customer_id);
                                sbPromociones.Append(";");
                                sbPromociones.Append(codigo_cuenta);
                                objLog.CrearArchivolog(string.Format("{0} ===> {1} ", "[PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil][sbPromociones]", Funciones.CheckStr(sbPromociones.ToString())), null, null);
                            }
                        }
                    }
                }

            }
            catch (Exception)
            {
                objLog.CrearArchivolog("[ERROR][ConsultarLineasAsociadas]", null, null);
                
                throw;
            }
            objLog.CrearArchivolog("[FIN][ConsultarLineasAsociadas]", null, null);

            return sbPromociones.ToString();
        }

        private bool validarSOT(string strTipoDocumento, string strNroDocumento, string strTipoPorducto, int intMaxDiasBuscaSEC)
        {
            RestVentasCuotas rService = new RestVentasCuotas();
            GetDataConsultaSotResponse objResponse = new GetDataConsultaSotResponse();
            GetDataConsultaSotRequest objRequest = new GetDataConsultaSotRequest();
            Dictionary<string, string> dcParameters = new Dictionary<string, string>();
            string strCodRpst, strMsjRpst, strIdTransaccion = string.Empty;
            GeneradorLog _objLog = new GeneradorLog(CurrentUsers, Funciones.CheckStr(strNroDocumento), null, "validarSOT");
            GetDataConsultaSotBodyRequest objRequestBody = new GetDataConsultaSotBodyRequest();
            List<GetDataConsultaSotTypeResponse> objLisDatosSOT = new List<GetDataConsultaSotTypeResponse>();
            List<BESolicitud> lstSolicitudesCliente = new List<BESolicitud>();

            try
            {
                Claro.SISACT.Entity.DataPowerRest.Generic.HeaderRequest headersRequest = rService.GetHeader_v2();
                headersRequest.dispositivo = CurrentTerminal;
                headersRequest.userId = Funciones.CheckStr(ConfigurationManager.AppSettings["system_ConsultaClave"]);
                headersRequest.modulo = Funciones.CheckStr(ConfigurationManager.AppSettings["DP_consModulo_Generico"]);
                headersRequest.operation = Funciones.CheckStr(ReadKeySettings.Key_OperationServiceHeader);
                headersRequest.wsIp = Funciones.CheckStr(ReadKeySettings.Key_IpWhitelistDatosSOT);

                _objLog.CrearArchivolog(String.Format("{0} => {1}", "[PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] - [dispositivo] ", Funciones.CheckStr(headersRequest.dispositivo)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} => {1}", "[PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] - [userId] ", Funciones.CheckStr(headersRequest.userId)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} => {1}", "[PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] - [modulo] ", Funciones.CheckStr(headersRequest.modulo)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} => {1}", "[PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] - [operation] ", Funciones.CheckStr(headersRequest.operation)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} => {1}", "[PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] - [wsIp] ", Funciones.CheckStr(headersRequest.wsIp)), null, null);

                objRequestBody.numeroDocumento = Funciones.CheckStr(strNroDocumento);
                objRequestBody.tipoDocumento = Funciones.CheckStr(strTipoDocumento);
                objRequestBody.tipoOperacion = Funciones.CheckStr(ReadKeySettings.Key_CodOperacionSOT);
                objRequestBody.tipoProducto = Funciones.CheckStr(strTipoPorducto);


                objRequest.MessageRequest.Header.HeaderRequest = headersRequest;
                objRequest.MessageRequest.Body = objRequestBody;

                BEAuditoriaRequest objBeAuditoriaRequest = new BEAuditoriaRequest();
                objBeAuditoriaRequest.idTransaccion = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                objBeAuditoriaRequest.timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
                objBeAuditoriaRequest.userId = Funciones.CheckStr(CurrentUser);
                objBeAuditoriaRequest.msgid = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                objBeAuditoriaRequest.dataPower = true;
                objBeAuditoriaRequest.accept = "application/json";
                objBeAuditoriaRequest.urlTimeOut_Rest = Funciones.CheckStr(ConfigurationManager.AppSettings["ConsMejorasPorta_TimeOut"]);
                objBeAuditoriaRequest.wsIp = Funciones.CheckStr(ConfigurationManager.AppSettings["conswsipPedidosTienda"]);
                objBeAuditoriaRequest.ipTransaccion = Funciones.CheckStr(HttpContext.Current.Session["CurrentTerminal"]);
                objBeAuditoriaRequest.usuarioAplicacion = Funciones.CheckStr(CurrentUser);
                objBeAuditoriaRequest.urlRest = "urlConsultarDatosSOT";
                objBeAuditoriaRequest.ipApplication = CurrentServer;

                _objLog.CrearArchivolog(String.Format("{0} => {1}", "[PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] - [idTransaccion] ", Funciones.CheckStr(objBeAuditoriaRequest.idTransaccion)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} => {1}", "[PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] - [timestamp] ", Funciones.CheckStr(objBeAuditoriaRequest.timestamp)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} => {1}", "[PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] - [userId] ", Funciones.CheckStr(objBeAuditoriaRequest.userId)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} => {1}", "[PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] - [msgid] ", Funciones.CheckStr(objBeAuditoriaRequest.msgid)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} => {1}", "[PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] - [dataPower] ", Funciones.CheckStr(objBeAuditoriaRequest.dataPower)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} => {1}", "[PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] - [accept] ", Funciones.CheckStr(objBeAuditoriaRequest.accept)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} => {1}", "[PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] - [urlTimeOut_Rest] ", Funciones.CheckStr(objBeAuditoriaRequest.urlTimeOut_Rest)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} => {1}", "[PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] - [wsIp] ", Funciones.CheckStr(objBeAuditoriaRequest.wsIp)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} => {1}", "[PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] - [ipTransaccion] ", Funciones.CheckStr(objBeAuditoriaRequest.ipTransaccion)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} => {1}", "[PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] - [usuarioAplicacion] ", Funciones.CheckStr(objBeAuditoriaRequest.usuarioAplicacion)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} => {1}", "[PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] - [ipApplication] ", Funciones.CheckStr(objBeAuditoriaRequest.ipApplication)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} => {1}", "[PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] - [urlRest] ", Funciones.CheckStr(objBeAuditoriaRequest.urlRest)), null, null);

                objResponse = rService.ConsultarDataSOT(dcParameters, objRequest, objBeAuditoriaRequest);

                strCodRpst = Funciones.CheckStr(objResponse.MessageResponse.Body.auditResponse.codigoRespuesta);
                strMsjRpst = Funciones.CheckStr(objResponse.MessageResponse.Body.auditResponse.mensajeRespuesta);

                _objLog.CrearArchivolog(String.Format("{0} => {1}", "[PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] - [strCodRpst] ", Funciones.CheckStr(strCodRpst)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} => {1}", "[PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] - [strMsjRpst] ", Funciones.CheckStr(strMsjRpst)), null, null);

                if (strCodRpst.Equals("0"))
                {
                    objLisDatosSOT = objResponse.MessageResponse.Body.datosSotCliente;
                    foreach (GetDataConsultaSotTypeResponse itmServ in objLisDatosSOT)
                    {
                        lstSolicitudesCliente = BLGeneral.ConsultarSolicitudesCliente(strNroDocumento, intMaxDiasBuscaSEC);
                        foreach (BESolicitud itmClt in lstSolicitudesCliente)
                        {
                            if (itmServ.numeroSec.Equals(Funciones.CheckStr(itmClt.SOLIN_CODIGO)) && Equals(itmServ.estadoSot.ToUpper(), ReadKeySettings.Key_DesEstadoSOT))
                                return true; 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _objLog.CrearArchivolog(String.Format("{0} => {1}", "[PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] - [Message] ", Funciones.CheckStr(ex.Message)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} => {1}", "[PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] - [StackTrace] ", Funciones.CheckStr(ex.StackTrace)), null, null);
                throw;
            }
            return false;
        }
        #endregion
    }
}