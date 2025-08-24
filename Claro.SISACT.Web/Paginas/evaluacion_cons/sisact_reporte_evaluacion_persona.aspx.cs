using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Claro.SISACT.Business;
using Claro.SISACT.Common;
using Claro.SISACT.Entity;
using Claro.SISACT.Web.Base;
using Claro.SISACT.WS;
using Claro.SISACT.Web.Comun;
using System.Collections;
using System.Text;
using Claro.SISACT.Business.RestReferences;//PROY-140546 Cobro Anticipado de Instalacion
using PAGOANTICIPADO_ENTITY = Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest;//PROY-140546 Cobro Anticipado de Instalacion
using Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest.DataPower.RegistraHistorial;//PROY-140546
using Claro.SISACT.Entity.DataPowerRest;
using Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest.DataPower.ConsultaHistorico.Request;
using Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest.DataPower.ConsultaHistorico.Response;
using Claro.SISACT.WS.RestReferences;
using System.Net;//PROY-140546 Cobro Anticipado de Instalacion
using System.Web.Script.Serialization; //INC000003467242

namespace Claro.SISACT.Web.Paginas.evaluacion_cons //PROY-24724-IDEA-28174
{
    public partial class sisact_reporte_evaluacion_persona : Sisact_Webbase
    {
        GeneradorLog objLog = new GeneradorLog("    sisact_reporte_evaluacion_persona   ", null, null, "WEB");
        List<BEParametro> lstBEEvaluacionProactiva = new List<BEParametro>();//Proy-30748
        #region [Declaracion de Constantes - Config]

        double dblMontoGarantiaCred = 0;
        double dblMontoGarantiaEval = 0;
        double dblCFTotal = 0.0;
        double dblCFTotalHFC = 0.0;
        double dblTotalCostoInstalacion = 0.0;
        double dblTotalCostoInstalacionHFC = 0.0;
        double dblTotalCostoInstalacionHFC1 = 0.0; //PROY-140546

        GridViewRow dvrItem;
        //PROY-29215 INICIO
        List<BEItemGenerico> objFormaPago = new List<BEItemGenerico>();
        List<BEItemGenerico> objCuotas = new List<BEItemGenerico>();
        List<BEItemGenerico> objConsultaFC = new List<BEItemGenerico>();
        //PROY-29215 FIN

        List<BETipoGarantia> objTipoGarantia = new List<BETipoGarantia>();
        string constClaseEmpresaTop = ConfigurationManager.AppSettings["constCodClasificacionEmpresaTop"].ToString();
        string constFlgPortabilidad = ConfigurationManager.AppSettings["FlagPortabilidad"].ToString();
        string constTipoProductoDTH = ConfigurationManager.AppSettings["constTipoProductoDTH"].ToString();
        string constTipoProductoHFC = ConfigurationManager.AppSettings["constTipoProductoHFC"].ToString();
        string constTipoProductoVentaVarios = ConfigurationManager.AppSettings["constTipoProductoVentaVarios"].ToString();
        string constModalidadCuota = ConfigurationManager.AppSettings["constCodModalidadCuota"].ToString();
        //emergencia-29215 -inicio
        string constTipoProductoFTTH = ConfigurationManager.AppSettings["constTipoProductoFTTH"].ToString();
        //emergencia-29215 -fin

        string constTipoOperMigracion = ConfigurationManager.AppSettings["constTipoOperacionMIG"].ToString();
        string constEstadoPoolEvaluador = ConfigurationManager.AppSettings["constcodEstadoPOOLEVALUADORCONSUMER"].ToString();
        string constEstadoAprobado = ConfigurationManager.AppSettings["constEstadoAPR"].ToString();

        string constTipoProducto3PlayInalam = ConfigurationManager.AppSettings["constTipoProducto3PlayInalam"];
        //Inicio IDEA-30067
        string constFlagPortaAutomatico = ConfigurationManager.AppSettings["constFlagPortaAutomatico"].ToString();
        //Fin IDEA-30067

        //PROY-26963 - IDEA-34399: Mejora Portabilidad Despacho PDV
        BeConsultaPrevia datos = new BeConsultaPrevia();
        //PROY-26963 - IDEA-34399: Mejora Portabilidad Despacho PDV

        List<BEPrima> lstObjPrima = new List<BEPrima>(); //PROY-24724-IDEA-28174 - INICIO
        List<BEParametro> lstBEParametroProteccionMovil = new List<BEParametro>();
        bool blnSecTieneProteccionMovil = false; //PROY-24724-IDEA-28174 - FIN

        bool bConsultaPAtieneData = false; //PROY-140546 

        //PROY-140743 - INI
        string strTipoOperacion = string.Empty;
        string strPromociones = string.Empty;
        string strProd_Facturar = string.Empty;
        //PROY-140743 - INI

        #endregion [Declaracion de Constantes - Config]

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write("<script language='javascript' src='../../Scripts/funciones_sec.js'></script>");

            if (Funciones.CheckStr(HttpContext.Current.Request.QueryString["cu"]).Length == 0)
            {
                Response.Write("<script language=javascript>validarUrl();</script>");
            }
            else
            {
                Response.Write("<script language='javascript'>restringirEventos();</script>");
            }

            GeneradorLog objLogINC = new GeneradorLog(null, "[INC000003427525]", null, " SEC USUARIO NO AUTORIZADO ");
            if (Session["Usuario"] == null)
            {
                string codUsuarioExt = Request.QueryString["cu"];
                if (!Base.AccesoUsuario.ValidarAcceso(codUsuarioExt, CurrentUser))
                {
                    string strRutaSite = ConfigurationManager.AppSettings["RutaSite"];
                    Response.Redirect(strRutaSite);
                    return;
                }
            }
            else
            {
                objLogINC.CrearArchivolog("[INC000003427525] - Page_Load() -Session[Usuario] != null  ", null, null);
            }


            //INC000003427525 - INI

            objLogINC.CrearArchivolog("[INC000003427525] - Page_Load() -flgSoloConsulta ==> " + Funciones.CheckStr(Request.QueryString["flgSoloConsulta"]), null, null);
            BEUsuarioSession objUser = null;
            objUser = (BEUsuarioSession)Session["Usuario"];
            string cuSession = Funciones.CheckStr(objUser.idUsuario);
            string cuQueryStr = Funciones.CheckStr(Request.QueryString["cu"]) == string.Empty ? cuSession : Funciones.CheckStr(Request.QueryString["cu"]);

            objLogINC.CrearArchivolog("[INC000003427525] - Page_Load() - Request.QueryString[cu] ==> " + Funciones.CheckStr(Request.QueryString["cu"]), null, null);
            objLogINC.CrearArchivolog("[INC000003427525] - Page_Load() -objUser.idUsuario (cu) ==> " + cuSession, null, null);
            objLogINC.CrearArchivolog("[INC000003427525] - Page_Load() - cuQueryStr [cu] ==> " + cuQueryStr, null, null);

            if (cuSession == cuQueryStr)
            {
                if (Funciones.CheckStr(Request.QueryString["flgSoloConsulta"]) != "S")
                {
                    if (!validaPermisos(Funciones.CheckStr(cuSession)))
                    {
                        objLogINC.CrearArchivolog("[INC000003427525] - Page_Load() - flgSoloConsulta ==> Redireccionando al inicio", null, null);
                        string strRutaSite = Funciones.CheckStr(ConfigurationManager.AppSettings["RutaSite"].ToString());
                        Response.Redirect(strRutaSite);
                        return;
                    }
                }
            }
            else
            {
                objLogINC.CrearArchivolog("[INC000003427525] - Page_Load() -CUs no coinciden ==> Redireccionando al inicio ", null, null);
                string strRutaSite = Funciones.CheckStr(ConfigurationManager.AppSettings["RutaSite"].ToString());
                Response.Redirect(strRutaSite);
                return;
            }
            //INC000003427525 - FIN


            if (!Page.IsPostBack)
            {
                Session["AprobarSEC"] = "PENDIENTE";
                Inicio();
            }
            else
            {
                GeneradorLog objLog = new GeneradorLog(null, "[Verificacion aprobarSEC][aprobarSEC]", null, "DATA_LOG_APROBAR_SEC");
                objLog.CrearArchivolog("[Session[AprobarSEC]]: " + Funciones.CheckStr(Session["AprobarSEC"]), null, null); //ES -
                if (Convert.ToString(Session["AprobarSEC"]) == "PENDIENTE")
                {
                    objLog.CrearArchivolog("[Ingreso AL METODO aprobarSEC][Ingreso aprobarSEC]", null, null);
                    objLog.CrearArchivolog("[Ejecuto aprobarSEC() : " + Funciones.CheckStr(Request.QueryString["nroSEC"]) + "]", "SI", null);
                    aprobarSEC();
                }
                else
                {
                    objLog.CrearArchivolog("La SEC ya se encuentra aprobada.", null, null); //ES -
                }
            }

            //PROY-140439 BRMS CAMPAÑA NAVIDEÑA::INI
            hidFlagBRMSCamp.Value = Funciones.CheckStr(ReadKeySettings.Key_flagBRMSCamp);
            //PROY-140439 BRMS CAMPAÑA NAVIDEÑA::FIN

            string tipooperacion = txtTipoOperacion.Text; //PROY-140743
        }

        private void Inicio()
        {
            GeneradorLog objLog2 = new GeneradorLog(null, null, null, "DATA_LOG");

            string flgSoloConsulta = Request.QueryString["flgSoloConsulta"];
            string flgOrigenPagina = Request.QueryString["flgOrigenPagina"];
            Int64 nroSEC = Funciones.CheckInt64(Request.QueryString["nroSEC"]);
            // string strIdProd = Request.QueryString["strIdProd"];

            objLog2.CrearArchivolog("[INC000003467242]", string.Format("{0}:{1}", "[sisact_reporte_evaluacion_persona.aspx.cs][Inicio] flgSoloConsulta: ", Funciones.CheckStr(flgSoloConsulta)), null);
            objLog2.CrearArchivolog("[INC000003467242]", string.Format("{0}:{1}", "[sisact_reporte_evaluacion_persona.aspx.cs][Inicio] flgOrigenPagina: ", Funciones.CheckStr(flgOrigenPagina)), null);
            objLog2.CrearArchivolog("[INC000003467242]", string.Format("{0}:{1}", "[sisact_reporte_evaluacion_persona.aspx.cs][Inicio] nroSEC: ", nroSEC.ToString()), null);

            GeneradorLog objLog = new GeneradorLog(null, nroSEC.ToString(), null, "DATA_LOG");
            objLog.CrearArchivolog("[Inicio][Inicio]", null, null);
            objLog.CrearArchivolog("[SOLO_CONSULTA]", flgSoloConsulta.ToString(), null);
            objLog.CrearArchivolog("[ORIG_PAGINA]", flgOrigenPagina.ToString(), null);
            objLog.CrearArchivolog("[SEC]", nroSEC.ToString(), null);
            // objLog.CrearArchivolog("[ID_PROD]", strIdProd.ToString(), null);

            //Inicio IDEA-30067
            //hidProductoPortAuto.Value = strIdProd;
            //Fin IDEA-30067

            BEUsuarioSession objUsuarioSession = (BEUsuarioSession)Session["Usuario"];

            BLConsumer objConsumer = new BLConsumer();
            BLSolicitud objSolicitud = new BLSolicitud();
            DataTable dtDatosCreditos, dtDatosBilletera, dtDatosLineas, dtDatosGarantia, dtDetallePlan;
            DataRow drSolicitud = null;

            List<BEEstado> objListaEstado = new List<BEEstado>();
            List<BESolicitudEmpresa> objListaHistorico = new List<BESolicitudEmpresa>();
            List<BEComentario> objListaComentario = new List<BEComentario>();

            // Creación Log Aplicación
            // Consulta Datos Solicitud
            objLog.CrearArchivolog("[ObtenerSolicitudPersona]", nroSEC.ToString(), null);
            drSolicitud = objSolicitud.ObtenerSolicitudPersona(nroSEC).Rows[0];

            objLog.CrearArchivolog("[INC000003467242]", string.Format("{0}:{1}", "[sisact_reporte_evaluacion_persona][Inicio] drSolicitud", Funciones.CheckStr(new JavaScriptSerializer().Serialize(Funciones.ConvertirDataRowADictionary(drSolicitud)))), null);
            
            //PROY-140126 - IDEA 140248 INICIO
            DataTable dtSolicitud = new DataTable();
            objLog.CrearArchivolog("[ObtenerSolicitudPersona Ini]", nroSEC.ToString(), null);
            dtSolicitud = objSolicitud.ObtenerSolicitudPersona(nroSEC);
            objLog.CrearArchivolog("[ObtenerSolicitudPersona Fin]", nroSEC.ToString(), null);
            //PROY-140126 - IDEA 140248  FIN
            HttpContext.Current.Session["SolicitudPersona"] = null;//INC000002528095

            //Inicio IDEA-30067
            hidProductoPortAuto.Value = drSolicitud["PRDC_CODIGO"].ToString();
            objLog.CrearArchivolog("[ID_PROD]", hidProductoPortAuto.Value.ToString(), null);
            //Fin IDEA-30067

            //Inicio 29121
            hidDeudaCliente.Value = drSolicitud["SOLIC_DEUDA_CLIENTE"].ToString();
            //Fin 29121

            hidEstadoPa.Value = "";//PROY-140546 PA

            string resp = "";

            BLSolicitud.verificarPago(nroSEC, ref resp);

            if (resp == "SI")
                lblPago.ForeColor = System.Drawing.Color.Red;
            else
                lblPago.ForeColor = System.Drawing.Color.Black;

            lblPago.Text = resp;

            // Asignar Usuario
            hidTiempoInicio.Value = DateTime.Now.ToString();
            hidFlgConsulta.Value = flgSoloConsulta;
            hidflgOrigenPagina.Value = flgOrigenPagina;
            hidListaPerfiles.Value = objUsuarioSession.CadenaOpcionesPagina;
            hidUsuarioRed.Value = CurrentUser;

            if (flgOrigenPagina == "P" && flgSoloConsulta != "S")
                objSolicitud.AsignarUsuarioSEC(nroSEC, objUsuarioSession.idCuentaRed, drSolicitud["CLIEC_NUM_DOC"].ToString(), "E");

            // Consulta Datos Planes
            objLog.CrearArchivolog("[ObtenerDetallePlanes][SEC]   ", nroSEC.ToString(), null);
            //   objLog.CrearArchivolog("[ObtenerDetallePlanes][ID_PROD]", strIdProd.ToString(), null);

            dtDetallePlan = (new BLEvaluacion()).ObtenerDetallePlanes(nroSEC, 0);
            hidModVenta.Value = dtDetallePlan.Rows[0]["ID_MODALIDAD_VENTA"].ToString(); //PROY-140223 IDEA-140462
            //PROY-26963 - IDEA-34399: Mejora Portabilidad Despacho PDV
            HttpContext.Current.Session["SolicitudPersona"] = dtSolicitud; //PROY-140126
            //PROY-26963 - IDEA-34399: Mejora Portabilidad Despacho PDV

            objLog.CrearArchivolog("[INC000003467242]", string.Format("{0}:{1}", "[sisact_reporte_evaluacion_persona][Inicio] dtSolicitud", Funciones.CheckStr(new JavaScriptSerializer().Serialize(Funciones.ConvertirDataTableAListaDictionary(dtSolicitud)))), null);            

            // Consulta Datos Creditos
            objLog.CrearArchivolog("[ObtenerInformacionCrediticia][SEC]", nroSEC.ToString(), null);
            dtDatosCreditos = objConsumer.ObtenerInformacionCrediticia(nroSEC);

            // Consulta Datos Monto Facturado, NO Facturado y LC Disponible
            objLog.CrearArchivolog("[ObtenerInformacionBilletera][SEC]", nroSEC.ToString(), null);
            dtDatosBilletera = objConsumer.ObtenerInformacionBilletera(nroSEC);

            // Consulta Datos Conteo de Líneas
            objLog.CrearArchivolog("[ListarCantidadLineasActivas][nroSEC]", nroSEC.ToString(), null);
            dtDatosLineas = objConsumer.ListarCantidadLineasActivas(nroSEC);

            // Consulta Datos Garantía
            objLog.CrearArchivolog("[ObtenerInformacionGarantiaII][SEC]", nroSEC.ToString(), null);
            dtDatosGarantia = objConsumer.ObtenerInformacionGarantiaII(nroSEC);

            // Consulta Datos Historico
            objLog.CrearArchivolog("[ObtenerHistoricoPersona][DOCC_CODIGO]", drSolicitud["TDOCC_CODIGO"].ToString(), null);
            objLog.CrearArchivolog("[ObtenerHistoricoPersona][CLIEC_NUM_DOC]", drSolicitud["CLIEC_NUM_DOC"].ToString(), null);
            objListaHistorico = objSolicitud.ObtenerHistoricoPersona(0, drSolicitud["TDOCC_CODIGO"].ToString(), drSolicitud["CLIEC_NUM_DOC"].ToString(), Funciones.CheckDate(""), Funciones.CheckDate(""), "00");

            // Consulta Log Estado
            objLog.CrearArchivolog("[ObtenerLogEstados][SEC]", nroSEC.ToString(), null);
            objListaEstado = objSolicitud.ObtenerLogEstados(nroSEC);

            // Consulta Tipo Garantia
            objLog.CrearArchivolog("[ListaTipoGarantia]", null, null);
            objTipoGarantia = (new BLGeneral()).ListaTipoGarantia(string.Empty, "1");

            //PROY-31948 INI
            //String tipoOperacion = Convert.ToString(drSolicitud.ItemArray[86]);
            objLog.CrearArchivolog("[PROY-31948 IniciarControlCuota][SEC]", nroSEC.ToString(), null);
            strTipoOperacion =  Funciones.CheckStr(drSolicitud["TOPEN_CODIGO"]); //PROY-140743
            IniciarControlCuota(nroSEC);
            //PROY-31948 FIN

            // Datos de la SEC
            objLog.CrearArchivolog("[IniciarControlGeneral]", null, null);
            IniciarControlGeneral(drSolicitud, dtDatosCreditos);

            // Datos del Cliente
            objLog.CrearArchivolog("[IniciarControlPersona]", null, null);
            IniciarControlPersona(drSolicitud, dtDatosLineas);

            //PROY-140743 - AE INICIO
            hidNroDoc_Cliente.Value = Convert.ToString(dgCliente.Rows[0].Cells[4].Text); //dgCliente.CurrentRow.Cells(3).Value
            //PROY-140743 - AE FIN

            //PROY-29215 INICIO
            string TipoProducto = drSolicitud["PRDC_CODIGO"].ToString();
            // EMERGENCIA-29215-INICIO
            if (TipoProducto == constTipoProductoDTH || TipoProducto == constTipoProductoHFC || TipoProducto == constTipoProducto3PlayInalam || TipoProducto == constTipoProductoFTTH)
            // EMERGENCIA-29215-FIN
            {
                objLog.CrearArchivolog("[ListarParametroGeneral-FormaPago]", null, null);
                objFormaPago = (new BLGeneral()).ListarParametroGeneral(Funciones.CheckStr(ConfigurationManager.AppSettings["strCodigoFormaPago"])); //"123"

                objLog.CrearArchivolog("[ListarParametroGeneral-Cuotas]", null, null);
                objCuotas = (new BLGeneral()).ListarParametroGeneral(Funciones.CheckStr(ConfigurationManager.AppSettings["strCodigoCuota"])); // "122"
                string vcadenacuota = "";
                for (int cu = 0; cu < objCuotas.Count; cu++)
                {

                    vcadenacuota += objCuotas[cu].Codigo + ';' + objCuotas[cu].Valor + '|';

                }
                hidCuotaparam.Value = vcadenacuota;

                objLog.CrearArchivolog("[Consulta FormaPago-Cuotas]", null, null);
                objConsultaFC = (new BLConsumer()).ConsultaModoPagoyCuota(nroSEC);

                hidFormaPagoActual.Value = objConsultaFC[0].Descripcion;
                hidCuotaActual.Value = objConsultaFC[0].Descripcion2;
            }
            //PROY-29215 FIN 

            objLog.CrearArchivolog(string.Format("{0}{1}", "[BuscarProteccionMovil] nroSEC ", nroSEC), null, null); //PROY-24724-IDEA-28174 - INICIO
            BuscarProteccionMovil(Funciones.CheckStr(nroSEC));

            string strCodGrupoParamProteccionMovil = Funciones.CheckStr(ConfigurationManager.AppSettings["consCodGrupoParamProteccionMovil"]);
            if (!string.IsNullOrEmpty(strCodGrupoParamProteccionMovil))
            {
                objLog.CrearArchivolog(string.Format("{0}{1}", "[ListaParametrosGrupo] strCodigo ", strCodGrupoParamProteccionMovil), null, null);
                lstBEParametroProteccionMovil = (new BLGeneral()).ListaParametrosGrupo(Funciones.CheckInt64(strCodGrupoParamProteccionMovil));
                objLog.CrearArchivolog(string.Format("{0}{1}", "[ListaParametrosGrupo] lstBEParametroProteccionMovil.Count ", lstBEParametroProteccionMovil.Count), null, null);
                if (Session["ListaParametrosPM"] == null) Session["ListaParametrosPM"] = lstBEParametroProteccionMovil;
            } //PROY-24724-IDEA-28174 - FIN
            //EMMH I
            CargarParametrosEvaluacionProactiva();
            //EMMH F
            // Condiciones de Venta
            objLog.CrearArchivolog("[IniciarControlCondiciones]", null, null);
            IniciarControlCondiciones(drSolicitud, dtDetallePlan, nroSEC); //PROY-140743

            // Información Crediticia
            objLog.CrearArchivolog("[IniciarControlCrediticio]", null, null);
            IniciarControlCrediticio(drSolicitud, dtDatosCreditos, dtDatosBilletera);

            // Información del Evaluador
            objLog.CrearArchivolog("[IniciarControlEvaluador]", null, null);
            IniciarControlEvaluador(drSolicitud, dtDatosCreditos, dtDatosGarantia);

            // Información de Portabilidad
            objLog.CrearArchivolog("[IniciarControlPortabilidad]", null, null);
            IniciarControlPortabilidad(drSolicitud);

            // Historico del Cliente
            dgHistorico.DataSource = objListaHistorico;
            dgHistorico.DataBind();

            // Log Estado del Cliente
            dgEstados.DataSource = objListaEstado;
            dgEstados.DataBind();

            // Log Estado del Cliente SGA
            objLog.CrearArchivolog("[IniciarHistoricoEstadoSOT]", null, null);
            if (flgOrigenPagina != "P")
                IniciarHistoricoEstadoSOT(dtDetallePlan);

            objLog.CrearArchivolog("[Fin][Inicio]", null, null);
        }

        #region [Iniciar Controles]

        private void IniciarControlGeneral(DataRow drSolicitud, DataTable dtDatosCreditos)
        {
            txtNroSEC.Text = drSolicitud["SOLIN_CODIGO"].ToString();
            txtCasoEspecial.Text = drSolicitud["TCESC_DESCRIPCION"].ToString();
            txtCanal.Text = drSolicitud["TOFIV_DESCRIPCION"].ToString();
            txtPuntoVenta.Text = drSolicitud["OVENV_DESCRIPCION"].ToString();
            txtTipoOperacion.Text = drSolicitud["TOPEV_DESCRIPCION"].ToString();
            txtEstadoVenta.Text = drSolicitud["SOLIV_DES_EST"].ToString();
            string txtModalidadVenta = hidModVenta.Value.ToUpper(); //Proy-140360  
            bool blnAsignaDescMotivo7 = false;
            string respDataCredTipo1 = ConfigurationManager.AppSettings["constRespDataCredTipo1"].ToString();
            string respDataCredTipo1Desc = ConfigurationManager.AppSettings["constRespDataCredTipo1Desc"].ToString();
            string respDataCredMotivo = ConfigurationManager.AppSettings["constRespDataCredMotivo"].ToString();
            string respDataCredTipo6 = ConfigurationManager.AppSettings["constRespDataCredTipo6"].ToString();
            string respDataCredTipo6Desc = ConfigurationManager.AppSettings["constRespDataCredTipo6Desc"].ToString();
            string respDataCredTipo6Motivo = ConfigurationManager.AppSettings["constRespDataCredTipo6Motivo"].ToString();
            string respDataCredTipo7 = ConfigurationManager.AppSettings["constRespDataCredTipo7"].ToString();
            string respDataCredTipo7Desc = ConfigurationManager.AppSettings["constRespDataCredTipo7Desc"].ToString();
            string respDataCredTipo7Motivo = ConfigurationManager.AppSettings["constRespDataCredTipo7Motivo"].ToString();

            txtMotivoIrCreditos.Text = respDataCredMotivo;

            //  Para mostrar una solo vez la descripcion por cada motivo.
            if (Funciones.CheckStr(drSolicitud["DCREV_TIPO_RESPUESTA"]) == respDataCredTipo6)
            {
                txtTipoRespuestaDC.Text = respDataCredTipo6Desc;
                txtMotivoIrCreditos.Text = respDataCredTipo6Motivo;
            }
            if (Funciones.CheckStr(drSolicitud["DCREV_TIPO_RESPUESTA"]) == respDataCredTipo7)
            {
                txtTipoRespuestaDC.Text = respDataCredTipo7Desc;
                txtMotivoIrCreditos.Text = respDataCredTipo7Motivo;
                blnAsignaDescMotivo7 = true;
            }

            string flgValidarCliente = Funciones.CheckStr(drSolicitud["DCREC_VALIDAR_CLIENTE"]);

            if (Funciones.CheckDbl(drSolicitud["CLIEN_MONTO_VENCIDO"]) > 0 || drSolicitud["CLIEC_BLOQUEO"].ToString() == ConfigurationManager.AppSettings["2Play_MontoVencido_Bloqueo"].ToString())
            {
                txtMotivoIrCreditos.Text += " - " + ConfigurationManager.AppSettings["2Play_MontoVencido_Bloqueo"].ToString();
            }
            if (flgValidarCliente.Equals("S"))
                txtMotivoCorreccion.Text = "La sec ha sido enviada a C&A para validación y corrección de datos del Cliente";
            else if (flgValidarCliente.Equals("R"))
            {
                txtMotivoIrCreditos.Text += "-" + ConfigurationManager.AppSettings["constRespMotivoEsSaludSunat"].ToString();
                txtMotivoCorreccion.Text = "La sec ha sido enviada a C&A para validación y corrección de datos del Cliente y de Dependiente/Independiente.";
            }
            else if (flgValidarCliente.Equals("E"))
            {
                txtMotivoIrCreditos.Text += "-" + ConfigurationManager.AppSettings["constRespMotivoEsSaludSunat"].ToString();
                txtMotivoCorreccion.Text = "La sec ha sido enviada a C&A para validación y corrección Dependiente/Independiente.";
            }
            else if (flgValidarCliente.Equals("A"))
                txtMotivoCorreccion.Text = "La sec ha sido enviada a C&A por incumplimiento de autonomía de Edad";
            else if (flgValidarCliente.Equals("C"))
                txtMotivoCorreccion.Text = ConfigurationManager.AppSettings["ConstMensajeAutonomiaMaxPlanesCE"].ToString();

            if (flgValidarCliente.Equals("D") && !blnAsignaDescMotivo7)
            {
                if (txtMotivoIrCreditos.Text == "")
                    txtMotivoIrCreditos.Text += ConfigurationManager.AppSettings["constRespDataCredMotivo"].ToString() + " - " + respDataCredTipo7Motivo;
                else
                    txtMotivoIrCreditos.Text += " - " + respDataCredTipo7Motivo;
            }

            // Toma estos valores desde Proyecto Evaluación y Venta Unificada
            if (Funciones.CheckStr(drSolicitud["DCREC_VALIDAR_CLIENTE"]) == "X")
            {
                txtMotivoIrCreditos.Text = dtDatosCreditos.Rows[0]["CREDV_MOTIVO"].ToString();
                txtMotivoCorreccion.Text = drSolicitud["CREDV_OBS_FLEXIBILIZACION"].ToString();
            }

            //PROY 33313 INICIO 
            Int64 nroSEC = Funciones.CheckInt64(Request.QueryString["nroSEC"]);

            string P_RESULTADO = "";
            string P_NRO_RESULTADO = "";
            string P_DES_RESULTADO = "";
            //PROY_140360   inicio
            string P_RESULTADO1 = "";
            string P_NRO_RESULTADO1 = "";
            string P_DES_RESULTADO1 = "";
            //PROY_140360  fin 

            new BLGeneral_II().ListarEstadoFlaj(nroSEC, out P_RESULTADO, out P_NRO_RESULTADO, out P_DES_RESULTADO);
            List<BEParametro> Key_ParanGroupChipCuota = (new BLGeneral()).ListaParametrosGrupo(Int64.Parse(ConfigurationManager.AppSettings["valorParanGroupChipCuota"].ToString()));


            //PROY_140360  inicio

            new BLGeneral_II().ListarEstadoFlaj2(nroSEC, out P_RESULTADO1, out P_NRO_RESULTADO1, out P_DES_RESULTADO1);
            List<BEParametro> Key_ParanGroupContrato = (new BLGeneral()).ListaParametrosGrupo(Int64.Parse(ConfigurationManager.AppSettings["valorParanGroupContratoCode"].ToString()));

            string strContratoDescrip = Key_ParanGroupContrato.Where(p => p.Descripcion == "Key_Message_Pack").SingleOrDefault().Valor;
            string strFlagCAC1 = Key_ParanGroupContrato.Where(p => p.Descripcion == "Key_Flag_CAC").SingleOrDefault().Valor1;
            string strFlagDAC1 = Key_ParanGroupContrato.Where(p => p.Descripcion == "Key_Flag_DAC").SingleOrDefault().Valor1;
            bool blnEsContratoCode = false;
            if (((txtCanal.Text == "CAC" && strFlagCAC1 == "1") || (txtCanal.Text == "DAC" && strFlagDAC1 == "1")) && Funciones.CheckStr(hidModVenta.Value) == "2")
            {
                if (P_NRO_RESULTADO1 == "0")
                {
                    switch (P_RESULTADO1)
                    {
                        case "1":
                            objLog.CrearArchivolog("    PROY-XXX ContratoCode   ", LblMsjChipCuota.Text.ToString(), null);
                            objLog.CrearArchivolog("    PROY-XXX COntratoCode   ", LblMsjYN.Text.ToString(), null);
                            LblMsjChipCuota.Text = strContratoDescrip.ToString();
                            LblMsjYN.Text = "SI";
                            break;
                        case "0":
                            objLog.CrearArchivolog("    PROY-XXX ContratoCode   ", LblMsjChipCuota.Text.ToString(), null);
                            objLog.CrearArchivolog("    PROY-XXX ContratoCode   ", LblMsjYN.Text.ToString(), null);
                            LblMsjChipCuota.Text = strContratoDescrip.ToString();
                            LblMsjYN.Text = "NO";
                            break;
                        default:
                            objLog.CrearArchivolog("    PROY-XXX ContratoCode   ", LblMsjChipCuota.Text.ToString(), null);
                            objLog.CrearArchivolog("    PROY-XXX ContratoCode   ", LblMsjYN.Text.ToString(), null);
                            LblMsjChipCuota.Text = "";
                            LblMsjYN.Text = "";
                            break;
                    }
                    blnEsContratoCode = true;
                }
                else
                {
                    objLog.CrearArchivolog("    PROY-XXX Error al obtener los datos parametrica   ", P_RESULTADO1.ToString(), null);
                    objLog.CrearArchivolog("    PROY-XXX P_RESULTADO1   ", P_RESULTADO1.ToString(), null);
                    objLog.CrearArchivolog("    PROY-XXX P_NRO_RESULTADO1   ", P_NRO_RESULTADO1.ToString(), null);
                    objLog.CrearArchivolog("    PROY-XXX P_DES_RESULTADO1   ", P_DES_RESULTADO1.ToString(), null);
                    LblMsjChipCuota.Text = P_DES_RESULTADO1.ToString();
                    LblMsjYN.Text = P_NRO_RESULTADO1.ToString();
                }

            }

            if (!blnEsContratoCode)
            {//PROY_140360  FIN
                objLog.CrearArchivolog("    PROY-33313 INICIO  ", null, null);
                objLog.CrearArchivolog("    PROY-33313 nroSEC   ", nroSEC.ToString(), null);
                objLog.CrearArchivolog("    PROY-33313 P_RESULTADO   ", P_RESULTADO.ToString(), null);
                objLog.CrearArchivolog("    PROY-33313 P_NRO_RESULTADO   ", P_NRO_RESULTADO.ToString(), null);
                objLog.CrearArchivolog("    PROY-33313 P_DES_RESULTADO   ", P_DES_RESULTADO.ToString(), null);

                string strChipDescripcion = Key_ParanGroupChipCuota.Where(p => p.Codigo == 189521341).SingleOrDefault().Valor;
                string strFlagCAC = Key_ParanGroupChipCuota.Where(p => p.Codigo == 189521342).SingleOrDefault().flagSistema;
                string strFlagDAC = Key_ParanGroupChipCuota.Where(p => p.Codigo == 189521343).SingleOrDefault().flagSistema;

                if ((txtCanal.Text == "CAC" && strFlagCAC == "1") || (txtCanal.Text == "DAC" && strFlagDAC == "1"))
                {
                    if (P_NRO_RESULTADO == "0")
                    {
                        switch (P_RESULTADO)
                        {
                            case "1":
                                objLog.CrearArchivolog("    PROY-33313 LblMsjChipCuota   ", LblMsjChipCuota.Text.ToString(), null);
                                objLog.CrearArchivolog("    PROY-33313 LblMsjYN   ", LblMsjYN.Text.ToString(), null);
                                LblMsjChipCuota.Text = strChipDescripcion.ToString();
                                LblMsjYN.Text = "SI";
                                break;
                            case "0":
                                objLog.CrearArchivolog("    PROY-33313 LblMsjChipCuota   ", LblMsjChipCuota.Text.ToString(), null);
                                objLog.CrearArchivolog("    PROY-33313 LblMsjYN   ", LblMsjYN.Text.ToString(), null);
                                LblMsjChipCuota.Text = strChipDescripcion.ToString();
                                LblMsjYN.Text = "NO";
                                break;
                            default:
                                objLog.CrearArchivolog("    PROY-33313 LblMsjChipCuota   ", LblMsjChipCuota.Text.ToString(), null);
                                objLog.CrearArchivolog("    PROY-33313 LblMsjYN   ", LblMsjYN.Text.ToString(), null);
                                LblMsjChipCuota.Text = "";
                                LblMsjYN.Text = "";
                                break;
                        }
                    }
                    else
                    {
                        objLog.CrearArchivolog("    PROY-33313 Error al obtener los datos parametricos   ", P_RESULTADO.ToString(), null);
                        objLog.CrearArchivolog("    PROY-33313 P_RESULTADO   ", P_RESULTADO.ToString(), null);
                        objLog.CrearArchivolog("    PROY-33313 P_NRO_RESULTADO   ", P_NRO_RESULTADO.ToString(), null);
                        objLog.CrearArchivolog("    PROY-33313 P_DES_RESULTADO   ", P_DES_RESULTADO.ToString(), null);
                        LblMsjChipCuota.Text = P_DES_RESULTADO.ToString();
                        LblMsjYN.Text = P_NRO_RESULTADO.ToString();
                    }
                }
                objLog.CrearArchivolog("    PROY-33313-FIN  ", null, null);
                //PROY 33313 FIN
            }
        }//Proy-140360

        private void IniciarControlPersona(DataRow drSolicitud, DataTable dtDatosLineas)
        {
            txtTipoCliente.Text = "CLIENTE NUEVO";
            if (Funciones.CheckStr(drSolicitud["SOLIC_EXI_BSC_FIN"]) == "1") txtTipoCliente.Text = "CLIENTE CLARO";

            txtMontoCFMayor.Text = string.Format("{0:#,#,#,0.00}", drSolicitud["CLIEN_CF_MAYORES"]);
            txtMontoCFMenor.Text = string.Format("{0:#,#,#,0.00}", drSolicitud["CLIEN_CF_MENORES"]);

            string nroDocumento = Funciones.FormatoNroDocumento(drSolicitud["TDOCC_CODIGO"].ToString(), drSolicitud["CLIEC_NUM_DOC"].ToString());
            hidTipoDoc.Value = drSolicitud["TDOCC_CODIGO"].ToString();
            hidNroDoc.Value = nroDocumento;

            HttpContext.Current.Session["docCliente"] = nroDocumento;

            if (HttpContext.Current.Session["objCliente" + nroDocumento] == null)
            {
                BEClienteCuenta objCliente = new BEClienteCuenta();
                objCliente.tipoDoc = drSolicitud["TDOCC_CODIGO"].ToString();
                objCliente.nroDoc = nroDocumento;
                HttpContext.Current.Session["objCliente" + nroDocumento] = objCliente;
            }

            objLog.CrearArchivolog("[Inicio][IniciarControlPersona]", null, null);

            HttpContext.Current.Session["docCliente"] = nroDocumento;
            objLog.CrearArchivolog("HttpContext.Current.Session[docCliente]=>" + Funciones.CheckStr(HttpContext.Current.Session["docCliente"]), null, null);

            BEClienteCuenta objClienteConsulta = new BEClienteCuenta();
            objClienteConsulta = (BEClienteCuenta)HttpContext.Current.Session["objCliente" + nroDocumento];
            objLog.CrearArchivolog("objClienteConsulta.tipoDoc =>" + Funciones.CheckStr(objClienteConsulta.tipoDoc), null, null);
            objLog.CrearArchivolog("objClienteConsulta.nroDoc =>" + Funciones.CheckStr(objClienteConsulta.nroDoc), null, null);

            hidTipoOperacion.Value = drSolicitud["TOPEN_CODIGO"].ToString();

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("CLIEV_NOMBRE", System.Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("CLIEV_APE_PAT", System.Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("CLIEV_APE_MAT", System.Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("TDOCV_DESCRIPCION", System.Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("CLIEC_NUM_DOC", System.Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("CLIED_FEC_NAC", System.Type.GetType("System.DateTime")));
            dt.Columns.Add(new DataColumn("CLIED_FEC_NAC_PDV", System.Type.GetType("System.DateTime")));

            DataRow dr = dt.NewRow();
            dr["CLIEV_NOMBRE"] = drSolicitud["CLIEV_NOMBRE"].ToString();
            dr["CLIEV_APE_PAT"] = drSolicitud["CLIEV_APE_PAT"].ToString();
            dr["CLIEV_APE_MAT"] = drSolicitud["CLIEV_APE_MAT"].ToString();
            dr["TDOCV_DESCRIPCION"] = drSolicitud["TDOCV_DESCRIPCION"].ToString();
            dr["CLIEC_NUM_DOC"] = nroDocumento;
            dr["CLIED_FEC_NAC"] = Funciones.CheckDate(drSolicitud["CLIED_FEC_NAC"]);
            dr["CLIED_FEC_NAC_PDV"] = Funciones.CheckDate(drSolicitud["CLIED_FEC_NAC_PDV"]);
            dt.Rows.Add(dr);

            hidNombres.Value = drSolicitud["CLIEV_NOMBRE"].ToString();
            hidApePaterno.Value = drSolicitud["CLIEV_APE_PAT"].ToString();
            hidApeMaterno.Value = drSolicitud["CLIEV_APE_MAT"].ToString();

            dgCliente.DataSource = dt;
            dgCliente.DataBind();

            dgNroLineaActivas.DataSource = dtDatosLineas;
            dgNroLineaActivas.DataBind();
        }

        private void IniciarControlCondiciones(DataRow drSolicitud, DataTable dtDetallePlan, Int64 nroSEC)//PROY-140743 
        {
            objLog.CrearArchivolog("    IniciarControlCondiciones/Inicio   ", null, null);

            txtOferta.Text = drSolicitud["TOFV_DESCRIPCION"].ToString();
            txtCombo.Text = drSolicitud["COMBV_DESCRIPCION"].ToString();

            if (!string.IsNullOrEmpty(txtCombo.Text)) hidFlgCombo.Value = "S";

            txtTelefonoRenovar.Text = drSolicitud["TELEFONO"].ToString();
            txtPlanComercial.Text = drSolicitud["PLAN_ACTUAL"].ToString();
            txtCFActual.Text = string.Format("{0:#,#,#,0.00}", drSolicitud["RENOF_CFACTUAL"]);

            if (drSolicitud["TOPEN_CODIGO"].ToString() == "25") //PROY-140743 - INICIO
            {
                DataColumn colPromocion = new DataColumn("PROMOCIONES", System.Type.GetType("System.String"));
                colPromocion.DefaultValue = "";
                dtDetallePlan.Columns.Add(colPromocion);
                DataColumn colProdFact = new DataColumn("PROD_FACTURAR", System.Type.GetType("System.String"));
                colProdFact.DefaultValue = "";
                dtDetallePlan.Columns.Add(colProdFact);
            }//PROY-140743 - FIN

            foreach (DataRow dr in dtDetallePlan.Rows)
            {
                if (dr["ID_PRODUCTO"].ToString() == constTipoProductoDTH)
                {
                    hidFlgProductoDTH.Value = "S";
                }
                // EMERGENCIA-29215-INICIO
                if ((dr["ID_PRODUCTO"].ToString() == constTipoProductoHFC) || (dr["ID_PRODUCTO"].ToString() == constTipoProducto3PlayInalam || (dr["ID_PRODUCTO"].ToString() == constTipoProductoFTTH)))
                // EMERGENCIA-29215-FIN
                {
                    hidFlgProductoHFC.Value = "S";
                }
                txtModalidadVenta.Text = dr["MODALIDAD_VENTA"].ToString();

                if (drSolicitud["TOPEN_CODIGO"].ToString() == "25") //PROY-140743 - INICIO
                {
                   dr["PROMOCIONES"] = Funciones.CheckStr(strPromociones);
                   dr["PROD_FACTURAR"] = Funciones.CheckStr(strProd_Facturar);
                }
            }
            objLog.CrearArchivolog("    IniciarControlCondiciones/Salida   ", null, null);

            if(drSolicitud["TOPEN_CODIGO"].ToString() == "25") //PROY-140743 - INICIO
            {
                //Mostrar Columnas de Promociones y Producto a Facturar
                dgPlanes.Columns[5].Visible = true;
                dgPlanes.Columns[6].Visible = true;     
            }
            //PROY-140743 - FIN
            dgPlanes.DataSource = dtDetallePlan;
            dgPlanes.DataBind();
        }

        private void IniciarControlCrediticio(DataRow drSolicitud, DataTable dtDatosCreditos, DataTable dtDatosBilletera)
        {
            txtBuro.Text = drSolicitud["BURO_DESCRIPCION"].ToString();
            txtRangoLC.Text = dtDatosCreditos.Rows[0]["CREDV_RANGO_LC"].ToString();
            txtRiesgoClaro.Text = drSolicitud["CLIEV_RIESGO_CLARO"].ToString();
            txtRiesgoBuro.Text = drSolicitud["SOLIV_RES_EXP_CON"].ToString();

            txtLCBuro.Text = string.Format("{0:#,#,#,0.00}", drSolicitud["SOLIN_LIM_CRE_CON"]);
            txtLCDisponible.Text = string.Format("{0:#,#,#,0.00}", dtDatosCreditos.Rows[0]["CREDN_FACT_PROMEDIO"]);
            txtLcDisponibleClie.Text = string.Format("{0:#,#,#,0.00}", dtDatosCreditos.Rows[0]["CREDN_FACT_PROMEDIO"]);

            string strArchivo = "Log_aprobarSEC";
            string idLog = string.Format("{0} - {1}", CurrentUsers, Funciones.CheckStr(drSolicitud["SOLIN_CODIGO"]));
            Session["FlagPortabilidad"] = "";

            if (drSolicitud["FLAG_PORTABILIDAD"].ToString() == constFlgPortabilidad)
            {
                hidFlagPortabilidad.Value = "S";

                Session["FlagPortabilidad"] = "S";

            }

            GeneradorLog.EscribirLog(strArchivo, idLog, "drSolicitud(FLAG_PORTABILIDAD)", Funciones.CheckStr(drSolicitud["FLAG_PORTABILIDAD"]));
            GeneradorLog.EscribirLog(strArchivo, idLog, "Session(FlagPortabilidad)", Funciones.CheckStr(Session["FlagPortabilidad"]));

            foreach (DataRow fila in dtDatosBilletera.Rows)
            {
                string strMontoFacturado = string.Format("{0:#,#,#,0.00}", fila["PRDN_PROM_FACT"]);
                string strMontoNoFacturado = string.Format("{0:#,#,#,0.00}", fila["PRDN_PROM_NO_FACT"]);
                string strLCDisponible = string.Format("{0:#,#,#,0.00}", fila["PRDN_LC_DISPONIBLE"]);

                switch (fila["PRDN_CODIGO"].ToString())
                {
                    case Constantes.constTipoBilleteraMovil:
                        txtFactMovil.Text = strMontoFacturado;
                        txtNoFactMovil.Text = strMontoNoFacturado;
                        txtLcDisponibleMovil.Text = strLCDisponible;
                        break;
                    case Constantes.constTipoBilleteraInter:
                        txtFactInternet.Text = strMontoFacturado;
                        txtNoFactInternet.Text = strMontoNoFacturado;
                        txtLcDisponibleInternet.Text = strLCDisponible;
                        break;
                    case Constantes.constTipoBilleteraTV:
                        txtFactClaroTV.Text = strMontoFacturado;
                        txtNoFactClaroTV.Text = strMontoNoFacturado;
                        txtLcDisponibleCable.Text = strLCDisponible;
                        break;
                    case Constantes.constTipoBilleteraTelef:
                        txtFactTelefonia.Text = strMontoFacturado;
                        txtNoFactTelefonia.Text = strMontoNoFacturado;
                        txtLcDisponibleTelefonia.Text = strLCDisponible;
                        break;
                    case Constantes.constTipoBilleteraBAM:
                        txtFactBAM.Text = strMontoFacturado;
                        txtNoFactBAM.Text = strMontoNoFacturado;
                        txtLcDisponibleBAM.Text = strLCDisponible;
                        break;
                }
            }
        }

        private void IniciarControlEvaluador(DataRow drSolicitud, DataTable dtDatosCreditos, DataTable dtDatosGarantia)
        {
            txtResultadoEval.Value = dtDatosCreditos.Rows[0]["CREDV_MSJ_AUTONOMIA"].ToString().ToUpper();
            if (drSolicitud["CLIEC_FLAG_EXONERAR_RA"].ToString() == "SI") chkExoneracionRA.Checked = true;
            txtRiesgo.Text = txtRiesgoBuro.Text;
            txtLcDisponibleClie.Text = txtLCDisponible.Text;
            txtComportamiento.Text = drSolicitud["CLIEV_COMPORTA_PAGO"].ToString();

            visualizaImagenCP(Funciones.CheckInt(drSolicitud["CLIEV_CALIFICACION_PAGO"].ToString()));

            if (hidflgOrigenPagina.Value == "P" && hidFlgConsulta.Value != "S")
            {
                if (hidFlgProductoDTH.Value == "S")
                {
                    dgCostoInstalacion.DataSource = (new BLSolicitud()).ObtenerCostoInstalacion(Funciones.CheckInt64(txtNroSEC.Text));
                    dgCostoInstalacion.DataBind();
                }
                if (hidFlgProductoHFC.Value == "S")
                {
                    dgCostoInstalacionHFC.DataSource = ObtenerDatosCostoInstalacion(Funciones.CheckInt64(txtNroSEC.Text), "N");//PROY-140546 Cobro Anticipado de Instalacion
                    dgCostoInstalacionHFC.DataBind();
                }
            }
            else
            {
                txtComentarioAlPdv.Disabled = true;
                txtComentarioEvaluador.Disabled = true;
                chkReingresoSec.Enabled = false;

                if (hidFlgProductoDTH.Value == "S")
                {
                    objLog.CrearArchivolog("    IniciarControlEvaluador/ObtenerCostoInstalacion     ", null, null);
                    dgCostoInstalacion1.DataSource = (new BLSolicitud()).ObtenerCostoInstalacion(Funciones.CheckInt64(txtNroSEC.Text));
                    dgCostoInstalacion1.DataBind();
                }
                if (hidFlgProductoHFC.Value == "S")
                {
                    objLog.CrearArchivolog("    IniciarControlEvaluador/ObtenerCostoInstalacion2     ", null, null);
                    dgCostoInstalacionHFC1.DataSource = ObtenerDatosCostoInstalacion(Funciones.CheckInt64(txtNroSEC.Text), "S");//PROY-140546 Cobro Anticipado de Instalacion
                    dgCostoInstalacionHFC1.DataBind();
                }
            }

            bool flgGarantiaConvergente = true;
            hidContFila.Value = dtDatosGarantia.Rows.Count.ToString();

            if (dtDatosGarantia.Rows.Count == 0)
            {
                flgGarantiaConvergente = false;
                dtDatosGarantia = new BLConsumer().ObtenerInformacionGarantia(Funciones.CheckInt64(txtNroSEC.Text));
            }

            hidFlgConvergente.Value = flgGarantiaConvergente ? "S" : "N";

            if (hidflgOrigenPagina.Value == "P" && hidFlgConsulta.Value != "S")
            {
                if (flgGarantiaConvergente)
                {
                    gvResultadoCredito2.DataSource = dtDatosGarantia;
                    gvResultadoCredito2.DataBind();
                }
                else
                {
                    gvResultadoCredito.DataSource = dtDatosGarantia;
                    gvResultadoCredito.DataBind();
                }
            }
            else
            {
                if (flgGarantiaConvergente)
                {
                    gvResultadoCredito3.DataSource = dtDatosGarantia;
                    gvResultadoCredito3.DataBind();
                }
                else
                {
                    gvResultadoCredito1.DataSource = dtDatosGarantia;
                    gvResultadoCredito1.DataBind();
                }
            }

            txtComentarioAlPdv.Value = drSolicitud["SOLIV_COM_PUN_VEN"].ToString();
            txtComentarioPdv.Value = dtDatosCreditos.Rows[0]["COMEV_COMENTARIO"].ToString();
            txtComentarioEvaluador.Value = drSolicitud["SOLIV_COM_EVAL"].ToString();

            objLog.CrearArchivolog("    IniciarControlEvaluador/ObtenerArchivos     ", null, null);

            List<BEArchivo> objLista = (new BLSolicitud()).ObtenerArchivos(Funciones.CheckInt64(txtNroSEC.Text));

            objLog.CrearArchivolog("    IniciarControlEvaluador/SALIDA     ", null, null);

            dgArchivos.DataSource = objLista;
            dgArchivos.DataBind();
        }

        private void IniciarControlPortabilidad(DataRow drSolicitud)
        {
            if (drSolicitud["FLAG_PORTABILIDAD"].ToString() == constFlgPortabilidad)
            {
                txtCedente.Text = ObtenerDescripcion("CO", drSolicitud["PORT_OPER_CED"].ToString(), "1");
                txtModalidad.Text = ObtenerDescripcion("MO", drSolicitud["TLINC_CODIGO_CEDENTE"].ToString(), "");

                txtNroFolio.Text = drSolicitud["PORT_SOLIN_NRO_FORMATO"].ToString();

                List<BEArchivo> objLista = (new BLPortabilidad()).ListarAchivosAdjunto(0, Funciones.CheckInt64(txtNroSEC.Text), "", "1");
                dgPortabilidad.DataSource = objLista;
                dgPortabilidad.DataBind();
            }
        }

        private void IniciarHistoricoEstadoSOT(DataTable dtDetallePlan)
        {
            List<BEEstado> objLista = new List<BEEstado>();
            List<BEEstado> objListaTmp = new List<BEEstado>();
            List<String> objListaSEC = new List<String>();
            string strSEC = string.Empty;
            Int64 nroSEC = 0;

            foreach (DataRow dr in dtDetallePlan.Rows)
            {
                strSEC = dr["SOLIN_CODIGO"].ToString();
                nroSEC = Funciones.CheckInt64(strSEC);

                if (!objListaSEC.Contains(strSEC))
                {
                    // EMERGENCIA-29215-INICIO
                    if (dr["ID_PRODUCTO"].ToString() == constTipoProductoHFC || dr["ID_PRODUCTO"].ToString() == constTipoProductoDTH || dr["ID_PRODUCTO"].ToString() == constTipoProducto3PlayInalam || dr["ID_PRODUCTO"].ToString() == constTipoProductoFTTH)
                    // EMERGENCIA-29215-FIN
                    {
                        objListaTmp = (new BLSolicitud()).ObtenerHistoricoEstadosSOT(nroSEC);
                        objListaSEC.Add(strSEC);
                        objLista.AddRange(objListaTmp);

                        if (objListaTmp.Count > 0)
                        {
                            Int64 nroSOT = ((BEEstado)objListaTmp[0]).NroSOT;
                            if (nroSOT > 0)
                            {
                                List<BEEstado> objListaEstadoSOT = (new BLSolicitud()).ObtenerEstadoSot(nroSEC, nroSOT);
                                if (objListaEstadoSOT.Count > 0)
                                    txtEstadoVenta.Text = ((BEEstado)objListaEstadoSOT[0]).ESTAV_DESCRIPCION;
                            }
                        }
                    }
                }
            }

            dgEstadosSGA.DataSource = objLista;
            dgEstadosSGA.DataBind();
        }

        private string ObtenerDescripcion(string prefijo, string valor, string ref1)
        {
            string strDescripcion = string.Empty;
            List<BEParametroPortabilidad> objLista = (new BLPortabilidad()).ListarParametroPortabilidad(prefijo, valor, ref1, "", "", 1);
            foreach (BEParametroPortabilidad obj in objLista)
            {
                if (obj.PK_PARAT_PARAC_COD == valor)
                    strDescripcion = ((BEParametroPortabilidad)(objLista[0])).DESCRIPCION;
            }
            return strDescripcion;
        }

        private void visualizaImagenCP(int CP)
        {
            switch (CP)
            {
                case -1: imgPagador.Src = "../../Imagenes/imgPagador0.gif"; break;
                case 0: imgPagador.Src = "../../Imagenes/imgPagador0.gif"; break;
                case 1: imgPagador.Src = "../../Imagenes/imgPagador1.gif"; break;
                case 2: imgPagador.Src = "../../Imagenes/imgPagador2.gif"; break;
                case 3: imgPagador.Src = "../../Imagenes/imgPagador3.gif"; break;
                case 4: imgPagador.Src = "../../Imagenes/imgPagador4.gif"; break;
                case 5: imgPagador.Src = "../../Imagenes/imgPagador5.gif"; break;
                //gaa20170510
                default: imgPagador.Src = "../../Imagenes/imgPagador0.gif"; break;
                //fin gaa20170510
            }
        }

        protected void dgCliente_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[7].Text = Comun.WebComunes.CalcularEdad(e.Row.Cells[5].Text).ToString();
            }
        }

        protected void dgPlanes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string idProducto = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["ID_PRODUCTO"]);

                if ((idProducto == constTipoProductoHFC) || (idProducto == constTipoProducto3PlayInalam))
                {
                    string idFila = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["ORDEN"]);
                    string strPaqueteActual = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["AGRUPA"]);

                    string idFilaPaquete = Comun.WebComunes.ObtenerIdFilaPaquete(idFila, strPaqueteActual);
                    idFila = string.Format("[{0}]", idFila);

                    if (idFilaPaquete.IndexOf(idFila) > -1)
                    {
                        dblCFTotalHFC += Funciones.CheckDbl(((DataRowView)e.Row.DataItem).Row["CF_TOTAL_MENSUAL"]);

                        if (idFilaPaquete.IndexOf(idFila) == 0)
                            dvrItem = e.Row;
                        else
                            e.Row.Visible = false;

                        dvrItem.Cells[6].Text = string.Format("{0:#,#,#,0.00}", dblCFTotalHFC);
                    }
                    else
                        dblCFTotalHFC = 0.0;
                }
                if (!(idProducto == constTipoProductoHFC || idProducto == constTipoProductoDTH) || hidflgOrigenPagina.Value == "P")
                {
                    Label lblAnular = default(Label);
                    lblAnular = (Label)e.Row.FindControl("lblAnular");
                    lblAnular.Text = string.Empty;
                }

                string idEquipo = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["ID_EQUIPO"]);
                if (idEquipo == "")
                {
                    e.Row.Cells[7].Text = "&nbsp;";
                }

                if (idProducto == constTipoProductoVentaVarios)
                {
                    e.Row.Cells[6].Text = "&nbsp;";
                    e.Row.Cells[14].Text = "&nbsp;";
                }

                string idModalidadVenta = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["ID_MODALIDAD_VENTA"]);
                if (idModalidadVenta != constModalidadCuota)
                {
                    e.Row.Cells[12].Text = "&nbsp;";
                }

                dblCFTotal += Funciones.CheckDbl(((DataRowView)e.Row.DataItem).Row["CF_TOTAL_MENSUAL"]);
                txtCFTotal.Text = string.Format("{0:#,#,#,0.00}", dblCFTotal);

                string strCodPlanSolicitud = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["SOPLN_CODIGO"]); //PROY-24724-IDEA-28174 - INICIO
                string strCodMaterial = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["ID_EQUIPO"]);
                string strConcatItemPopUp = string.Empty;
                string strDescMaterial = string.Empty;
                BLSincronizaSap objBLSincronizaSap = new BLSincronizaSap();
                DataTable dtDatosMaterial = new DataTable();

                if (blnSecTieneProteccionMovil)
                {
                    foreach (BEPrima objPrima in lstObjPrima)
                    {
                        if (strCodPlanSolicitud == objPrima.SoplnCodigo)
                        {
                            if (strCodMaterial == objPrima.CodMaterial)
                                strDescMaterial = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["EQUIPO"]);
                            else
                            {
                                dtDatosMaterial = objBLSincronizaSap.ConsultarDatosMaterial(objPrima.CodMaterial);
                                if (dtDatosMaterial.Rows.Count > 0)
                                    strDescMaterial = Funciones.CheckStr(dtDatosMaterial.Rows[0]["MATEV_DESCMATERIAL"]);
                            }
                            strConcatItemPopUp = String.Format(lstBEParametroProteccionMovil.Where(p => p.Valor1 == "9").SingleOrDefault().Valor, strDescMaterial, objPrima.MontoPrima, objPrima.DeducibleRobo, objPrima.DeducibleDanio);
                            strConcatItemPopUp = strConcatItemPopUp.Replace(' ', '_');
                            e.Row.Cells[15].Text = "<img runat='server' alt='Protección Móvil' id='imgDetalle' style='cursor: hand' src='../../Imagenes/ico_lupa.gif' onclick=mostrarPopSeguros(" + '"' + strConcatItemPopUp + '"' + "); />";
                        }
                    }
                }
                else
                    dgPlanes.Columns[18].Visible = false; //PROY-24724-IDEA-28174 - FIN //PROY-30166-IDEA–38863 //PROY-140743
            }
        }

        protected void gvResultadoCredito_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow dvItemHeader = new GridViewRow(0, 0, DataControlRowType.Header, 0);

                TableCell td = default(TableCell);
                td = new TableCell();
                td.ColumnSpan = 3;
                td.Style.Add("visibility", "hidden");
                dvItemHeader.Cells.Add(td);
                td = new TableCell();
                td.ColumnSpan = 2;
                td.Text = "Resultado Evaluación";
                dvItemHeader.Cells.Add(td);
                td = new TableCell();
                td.ColumnSpan = 2;
                td.Text = "Resultado Creditos";
                dvItemHeader.Cells.Add(td);
                gvResultadoCredito.Controls[0].Controls.AddAt(0, dvItemHeader);
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtNroCargoFijo = (TextBox)e.Row.FindControl("txtGvNroCargosFijos");
                TextBox txtMontoGarantia = (TextBox)e.Row.FindControl("txtGvMontoGarantias");
                DropDownList ddlTipoGarantia = (DropDownList)e.Row.FindControl("ddlTipoGarantia");

                txtNroCargoFijo.Text = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["NRO_CF_CRED"]);
                txtMontoGarantia.Text = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["SOLIN_IMP_DG_MAN"]);
                txtTipoGarantia.Text = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["GARANTIA_EVAL"]);

                txtNroCargoFijo.Attributes.Add("onBlur", string.Format("calcularMontoGarantias(this.value, document.getElementById('{0}'), {1}, '{2}', '{3}', '{4}')", txtMontoGarantia.ClientID, e.Row.Cells[9].Text, e.Row.Cells[7].Text, gvResultadoCredito.ClientID, 1));
                txtMontoGarantia.Attributes.Add("onBlur", string.Format("calcularNroCargosFijos(this.value, document.getElementById('{0}'), {1}, '{2}', '{3}', '{4}', '{5}')", txtMontoGarantia.ClientID, txtNroCargoFijo.ClientID, e.Row.Cells[9].Text, e.Row.Cells[7].Text, gvResultadoCredito.ClientID, 1));

                txtMontoGarantia.Enabled = false;
                txtMontoGarantia.CssClass = "clsInputDisable";

                dblMontoGarantiaCred += Funciones.CheckDbl(((DataRowView)e.Row.DataItem).Row["SOLIN_IMP_DG_MAN"]);
                dblMontoGarantiaEval += Funciones.CheckDbl(((DataRowView)e.Row.DataItem).Row["SOLIN_IMP_DG"]);

                ddlTipoGarantia.DataSource = objTipoGarantia;
                ddlTipoGarantia.DataTextField = "TCARV_DESCRIPCION";
                ddlTipoGarantia.DataValueField = "TCARC_CODIGO";
                ddlTipoGarantia.DataBind();

                ddlTipoGarantia.SelectedValue = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["TIPO_GAR_CRED"]);
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[6].Text = dblMontoGarantiaCred.ToString();
                txtMontoTotalDG.Text = string.Format("{0:#,#,#,0.00}", dblMontoGarantiaEval);
            }
        }

        protected void gvResultadoCredito1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow dvItemHeader = new GridViewRow(0, 0, DataControlRowType.Header, 0);

                TableCell td = default(TableCell);
                td = new TableCell();
                td.ColumnSpan = 3;
                td.Style.Add("visibility", "hidden");
                dvItemHeader.Cells.Add(td);
                td = new TableCell();
                td.ColumnSpan = 2;
                td.Text = "Resultado Evaluación";
                dvItemHeader.Cells.Add(td);
                td = new TableCell();
                td.ColumnSpan = 2;
                td.Text = "Resultado Creditos";
                dvItemHeader.Cells.Add(td);
                gvResultadoCredito1.Controls[0].Controls.AddAt(0, dvItemHeader);
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                dblMontoGarantiaCred += Funciones.CheckDbl(((DataRowView)e.Row.DataItem).Row["SOLIN_IMP_DG_MAN"]);
                txtTipoGarantia.Text = e.Row.Cells[2].Text;
                dblMontoGarantiaEval += Funciones.CheckDbl(((DataRowView)e.Row.DataItem).Row["SOLIN_IMP_DG"]);
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[6].Text = dblMontoGarantiaCred.ToString();
                txtMontoTotalDG.Text = string.Format("{0:#,#,#,0.00}", dblMontoGarantiaEval);
            }
        }

        protected void gvResultadoCredito2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow dvItemHeader = new GridViewRow(0, 0, DataControlRowType.Header, 0);

                TableCell td = default(TableCell);
                td = new TableCell();
                td.ColumnSpan = 4;
                td.Style.Add("visibility", "hidden");
                dvItemHeader.Cells.Add(td);
                td = new TableCell();
                td.ColumnSpan = 2;
                td.Text = "Resultado Evaluación";
                dvItemHeader.Cells.Add(td);
                td = new TableCell();
                td.ColumnSpan = 2;
                td.Text = "Resultado Creditos";
                dvItemHeader.Cells.Add(td);
                gvResultadoCredito2.Controls[0].Controls.AddAt(0, dvItemHeader);
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtNroCargoFijo = (TextBox)e.Row.FindControl("txtGvNroCargosFijos");
                TextBox txtMontoGarantia = (TextBox)e.Row.FindControl("txtGvMontoGarantias");
                DropDownList ddlTipoGarantia = (DropDownList)e.Row.FindControl("ddlTipoGarantia");

                txtNroCargoFijo.Text = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["solin_num_cf_man"]);
                txtMontoGarantia.Text = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["solin_importe_man"]);
                txtTipoGarantia.Text = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["garantia"]);

                txtNroCargoFijo.Attributes.Add("onBlur", string.Format("calcularMontoGarantias(this.value, document.getElementById('{0}'), {1}, '{2}', '{3}', '{4}')", txtMontoGarantia.ClientID, e.Row.Cells[10].Text, e.Row.Cells[8].Text, gvResultadoCredito2.ClientID, 2));
                txtMontoGarantia.Attributes.Add("onBlur", string.Format("calcularNroCargosFijos(this.value, document.getElementById('{0}'), {1}, '{2}', '{3}', '{4}', '{5}')", txtMontoGarantia.ClientID, txtNroCargoFijo.ClientID, e.Row.Cells[10].Text, e.Row.Cells[8].Text, gvResultadoCredito2.ClientID, 2));

                txtMontoGarantia.Enabled = false;
                txtMontoGarantia.CssClass = "clsInputDisable";

                dblMontoGarantiaCred += Funciones.CheckDbl(((DataRowView)e.Row.DataItem).Row["solin_importe_man"]);
                dblMontoGarantiaEval += Funciones.CheckDbl(((DataRowView)e.Row.DataItem).Row["solin_importe"]);

                ddlTipoGarantia.DataSource = objTipoGarantia;
                ddlTipoGarantia.DataTextField = "TCARV_DESCRIPCION";
                ddlTipoGarantia.DataValueField = "TCARC_CODIGO";
                ddlTipoGarantia.DataBind();

                ddlTipoGarantia.SelectedValue = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["tcarc_codigo_man"]);
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[7].Text = dblMontoGarantiaCred.ToString();
                txtMontoTotalDG.Text = string.Format("{0:#,#,#,0.00}", dblMontoGarantiaEval);
            }
        }

        protected void gvResultadoCredito3_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow dvItemHeader = new GridViewRow(0, 0, DataControlRowType.Header, 0);

                TableCell td = default(TableCell);
                td = new TableCell();
                td.ColumnSpan = 4;
                td.Style.Add("visibility", "hidden");
                dvItemHeader.Cells.Add(td);
                td = new TableCell();
                td.ColumnSpan = 2;
                td.Text = "Resultado Evaluación";
                dvItemHeader.Cells.Add(td);
                td = new TableCell();
                td.ColumnSpan = 2;
                td.Text = "Resultado Creditos";
                dvItemHeader.Cells.Add(td);
                gvResultadoCredito3.Controls[0].Controls.AddAt(0, dvItemHeader);
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                dblMontoGarantiaCred += Funciones.CheckDbl(((DataRowView)e.Row.DataItem).Row["solin_importe_man"]);
                txtTipoGarantia.Text = e.Row.Cells[3].Text;
                dblMontoGarantiaEval += Funciones.CheckDbl(((DataRowView)e.Row.DataItem).Row["solin_importe"]);
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[7].Text = dblMontoGarantiaCred.ToString();
                txtMontoTotalDG.Text = string.Format("{0:#,#,#,0.00}", dblMontoGarantiaEval);
            }
        }

        protected void dgCostoInstalacion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow dvItemHeader = new GridViewRow(0, 0, DataControlRowType.Header, 0);

                TableCell td = default(TableCell);
                td = new TableCell();
                td.Text = "";
                dvItemHeader.Cells.Add(td);
                td = new TableCell();
                td.Text = "Costo de Instalación";
                dvItemHeader.Cells.Add(td);
                td = new TableCell();
                td.Text = "Resultado Evaluación";
                dvItemHeader.Cells.Add(td);
                td = new TableCell();
                //PROY-29215 INICIO
                td.ColumnSpan = 3;
                //PROY-29215 FIN
                td.Text = "Resultado Creditos";
                dvItemHeader.Cells.Add(td);
                //PROY-29215 INICIO
                td.ColumnSpan = 3;
                //PROY-29215 FIN
                dgCostoInstalacion.Controls[0].Controls.AddAt(0, dvItemHeader);
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtCostoInstalacion = (TextBox)e.Row.FindControl("txtCostoInstalacion");

                txtCostoInstalacion.Text = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["COSTO_INSTAL"]);
                txtCostoInstalacion.Attributes.Add("onBlur", "calcularTotalCostoInstalacion()");
                dblTotalCostoInstalacion += Funciones.CheckDbl(txtCostoInstalacion.Text);

                //PROY-29215 INICIO
                TextBox txtFormaPagoTVSAT = (TextBox)e.Row.FindControl("txtFormaPagoTVSAT");
                TextBox txtCuotaTVSAT = (TextBox)e.Row.FindControl("txtCuotasTVSAT");
                DropDownList ddlFormaPago = (DropDownList)e.Row.FindControl("ddlFormaPagoTVSAT");
                DropDownList ddlCuotas = (DropDownList)e.Row.FindControl("ddlCuotasTVSAT");

                txtFormaPagoTVSAT.Text = objConsultaFC[0].Descripcion;
                txtFormaPagoTVSAT.ReadOnly = true;
                txtCuotaTVSAT.Text = objConsultaFC[0].Descripcion2;
                txtCuotaTVSAT.ReadOnly = true;

                ddlFormaPago.DataSource = objFormaPago;
                ddlFormaPago.DataTextField = "Valor";
                ddlFormaPago.DataValueField = "Codigo";
                ddlFormaPago.DataBind();
                for (int j = 0; j <= objFormaPago.Count - 1; j++)
                {
                    if (objFormaPago[j].Valor.ToUpper() == txtFormaPagoTVSAT.Text.ToUpper())
                    {
                        ddlFormaPago.SelectedValue = objFormaPago[j].Codigo;
                    }
                }
                // emergencia-29215-INICIO
                if (txtFormaPagoTVSAT.Text.ToUpper() == "CONTRATA")
                {
                    ddlCuotas.DataSource = objCuotas.FindAll(X => X.Valor.ToUpper().Equals(txtCuotaTVSAT.Text.ToUpper()));
                    ddlCuotas.DataTextField = "Valor";
                    ddlCuotas.DataValueField = "Codigo";
                    ddlCuotas.DataBind();
                    ddlCuotas.SelectedValue = objCuotas.FindAll(X => X.Valor.ToUpper().Equals(txtCuotaTVSAT.Text.ToUpper()))[0].Codigo;
                }
                else
                {

                    ddlCuotas.DataSource = objCuotas;
                    ddlCuotas.DataTextField = "Valor";
                    ddlCuotas.DataValueField = "Codigo";
                    ddlCuotas.DataBind();
                    for (int k = 0; k <= objCuotas.Count - 1; k++)
                    {
                        if (objCuotas[k].Valor.ToUpper() == txtCuotaTVSAT.Text.ToUpper())
                        {
                            ddlCuotas.SelectedValue = objCuotas[k].Codigo;
                        }
                    }
                    ListItem itemToRemove = ddlCuotas.Items.FindByText("0");
                    if (itemToRemove != null)
                    {
                        ddlCuotas.Items.Remove(itemToRemove);
                    }
                }
                // emergencia-29215-FIN

                //PROY-29215 FIN
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                TextBox txtTotalCostoInstalacion = (TextBox)e.Row.FindControl("txtTotalCostoInstalacion");

                txtTotalCostoInstalacion.Text = string.Format("{0:#,#,#,0.00}", dblTotalCostoInstalacion);
            }
        }

        protected void dgCostoInstalacion1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow dvItemHeader = new GridViewRow(0, 0, DataControlRowType.Header, 0);

                TableCell td = default(TableCell);
                td = new TableCell();
                td.Text = "";
                dvItemHeader.Cells.Add(td);
                td = new TableCell();
                td.Text = "Costo de Instalación";
                dvItemHeader.Cells.Add(td);
                td = new TableCell();
                //PROY-29215 INICIO
                td.ColumnSpan = 3;
                //PROY-29215 FIN
                td.Text = "Resultado Evaluación";
                dvItemHeader.Cells.Add(td);
                td = new TableCell();
                //PROY-29215 INICIO
                td.ColumnSpan = 3;
                //PROY-29215 FIN
                td.Text = "Resultado Creditos";
                dvItemHeader.Cells.Add(td);
                dgCostoInstalacion1.Controls[0].Controls.AddAt(0, dvItemHeader);
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                dblTotalCostoInstalacion += Funciones.CheckDbl(((DataRowView)e.Row.DataItem).Row["COSTO_INSTAL"]);

                //PROY-29215 INICIO
                TextBox txtFormaPagoTVSAT = (TextBox)e.Row.FindControl("txtFormaPagoTVSAT1");
                TextBox txtCuotaTVSAT = (TextBox)e.Row.FindControl("txtCuotasTVSAT1");

                txtFormaPagoTVSAT.Text = objConsultaFC[0].Descripcion;
                txtFormaPagoTVSAT.ReadOnly = true;
                txtCuotaTVSAT.Text = objConsultaFC[0].Descripcion2;
                txtCuotaTVSAT.ReadOnly = true;
                //PROY-29215 FIN

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label txtTotalCostoInstalacion = (Label)e.Row.FindControl("txtTotalCostoInstalacion");

                txtTotalCostoInstalacion.Text = string.Format("{0:#,#,#,0.00}", dblTotalCostoInstalacion);
            }
        }

        //INICIO: NUEVO by EJ
        protected void dgCostoInstalacionHFC_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow dvItemHeader = new GridViewRow(0, 0, DataControlRowType.Header, 0);

                TableCell td = default(TableCell);
                td = new TableCell();
                td.Text = "";
                dvItemHeader.Cells.Add(td);
                td = new TableCell();
                td.Text = "Costo de Instalación";
                dvItemHeader.Cells.Add(td);
                td = new TableCell();
                td.Text = "Resultado Evaluación";

                //INICIO PROY-140546
                if (bConsultaPAtieneData)
                {
                    td.ColumnSpan = 5;
                }
                else
                {
                    //PROY-29215 INICIO
                    td.ColumnSpan = 3;
                    //PROY-29215 FIN
                }
                //FIN PROY-140546

                dvItemHeader.Cells.Add(td);
                td = new TableCell();
                td.Text = "Resultado Creditos";

                //INICIO PROY-140546
                if (bConsultaPAtieneData)
                {
                    td.ColumnSpan = 5;
                }
                else
                {
                    //PROY-29215 INICIO
                    td.ColumnSpan = 3;
                    //PROY-29215 FIN
                }
                //FIN PROY-140546

                dvItemHeader.Cells.Add(td);
                dgCostoInstalacionHFC.Controls[0].Controls.AddAt(0, dvItemHeader);
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtCostoInstalacion = (TextBox)e.Row.FindControl("txtCostoInstalacion");
                txtCostoInstalacion.Text = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["COSTO_INSTAL"]);
                txtCostoInstalacion.Attributes.Add("onBlur", "calcularTotalCostoInstalacion(1)");
                dblTotalCostoInstalacionHFC += Funciones.CheckDbl(txtCostoInstalacion.Text);

                //PROY-29215 INICIO
                DropDownList ddlFormaPago = (DropDownList)e.Row.FindControl("ddlFormaPago");
                DropDownList ddlCuotas = (DropDownList)e.Row.FindControl("ddlCuotas");
                TextBox txtFormaPago = (TextBox)e.Row.FindControl("txtFormaPago");
                TextBox txtCuota = (TextBox)e.Row.FindControl("txtCuotas");

                txtFormaPago.Text = objConsultaFC[0].Descripcion;
                txtFormaPago.ReadOnly = true;
                txtCuota.Text = objConsultaFC[0].Descripcion2;
                txtCuota.ReadOnly = true;

                ddlFormaPago.DataSource = objFormaPago;
                ddlFormaPago.DataTextField = "Valor";
                ddlFormaPago.DataValueField = "Codigo";
                ddlFormaPago.DataBind();
                for (int j = 0; j <= objFormaPago.Count - 1; j++)
                {
                    if (objFormaPago[j].Valor.ToUpper() == txtFormaPago.Text.ToUpper())
                    {
                        ddlFormaPago.SelectedValue = objFormaPago[j].Codigo;
                    }
                }
                List<BEItemGenerico> objCuotasNew = new List<BEItemGenerico>();
                if (txtFormaPago.Text.ToUpper() == "CONTRATA")
                {
                    for (int x = 0; x <= objCuotas.Count - 1; x++)
                    {
                        if (objCuotas[x].Valor.ToUpper() == "0")
                        {
                            objCuotasNew.Add(objCuotas[x]);
                        }
                    }
                }
                else
                {
                    for (int x = 0; x <= objCuotas.Count - 1; x++)
                    {
                        if (objCuotas[x].Valor != "0")
                        {
                            objCuotasNew.Add(objCuotas[x]);
                        }
                    }
                }
                ddlCuotas.DataSource = objCuotasNew;
                ddlCuotas.DataTextField = "Valor";
                ddlCuotas.DataValueField = "Codigo";
                ddlCuotas.DataBind();
                for (int k = 0; k <= objCuotas.Count - 1; k++)
                {
                    if (objCuotas[k].Valor.ToUpper() == txtCuota.Text.ToUpper())
                    {
                        ddlCuotas.SelectedValue = objCuotas[k].Codigo;
                    }
                }
                //PROY-29215 FIN

                //PROY-140546 Cobro Anticipado de Instalacion Inicio
                TextBox txtMAI = (TextBox)e.Row.FindControl("txtMAINoEdit");
                txtMAI.Text = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["MAI"]);

                double nMAI = Funciones.CheckDbl(txtMAI.Text);

                int nNumCuotas = Funciones.CheckInt(txtCuota.Text);

                TextBox txtCM = (TextBox)e.Row.FindControl("txtCMNoEdit");
                txtCM.Text = CalcularCM(dblTotalCostoInstalacionHFC, nMAI, nNumCuotas).ToString();

                TextBox txtMAIEdit = (TextBox)e.Row.FindControl("txtMAIEdit");
                txtMAIEdit.Text = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["MAI_MAN"]);

                double nMAI2 = Funciones.CheckDbl(txtMAIEdit.Text);

                TextBox txtCMEdit = (TextBox)e.Row.FindControl("txtCMEdit");
                txtCMEdit.Text = CalcularCM(dblTotalCostoInstalacionHFC, nMAI2, nNumCuotas).ToString();
                //PROY-140546 Cobro Anticipado de Instalacion Fin
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                TextBox txtTotalCostoInstalacion = (TextBox)e.Row.FindControl("txtTotalCostoInstalacion");

                txtTotalCostoInstalacion.Text = string.Format("{0:#,#,#,0.00}", dblTotalCostoInstalacionHFC);
            }
        }

        protected void dgCostoInstalacionHFC1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow dvItemHeader = new GridViewRow(0, 0, DataControlRowType.Header, 0);

                TableCell td = default(TableCell);
                td = new TableCell();
                td.Text = "";
                dvItemHeader.Cells.Add(td);
                td = new TableCell();
                td.Text = "Costo de Instalación";
                dvItemHeader.Cells.Add(td);
                td = new TableCell();

                //INICIO PROY-140546
                if (bConsultaPAtieneData)
                {
                    td.ColumnSpan = 5;
                }
                else
                {
                    //PROY-29215 INICIO
                    td.ColumnSpan = 3;
                    //PROY-29215 FIN
                }
                //INICIO PROY-140546

                td.Text = "Resultado Evaluación";
                dvItemHeader.Cells.Add(td);
                td = new TableCell();

                //INICIO PROY-140546
                if (bConsultaPAtieneData)
                {
                    td.ColumnSpan = 5;
                }
                else
                {
                    //PROY-29215 INICIO
                    td.ColumnSpan = 3;
                    //PROY-29215 FIN
                }
                //FIN PROY-140546

                td.Text = "Resultado Creditos";
                dvItemHeader.Cells.Add(td);
                dgCostoInstalacionHFC1.Controls[0].Controls.AddAt(0, dvItemHeader);
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                dblTotalCostoInstalacionHFC1 += Funciones.CheckDbl(((DataRowView)e.Row.DataItem).Row["COSTO_INSTAL_EVAL"]); //PROY-140546
                dblTotalCostoInstalacionHFC += Funciones.CheckDbl(((DataRowView)e.Row.DataItem).Row["COSTO_INSTAL"]);//

                //PROY-29215 INICIO
                TextBox txtFormaPago = (TextBox)e.Row.FindControl("txtFormaPago1");
                TextBox txtCuota = (TextBox)e.Row.FindControl("txtCuotas1");
                //PROY-29215 FIN                

                //PROY-140546 Cobro Anticipado de Instalacion
                txtFormaPago.Text = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["FORMA_PAGO"]); ;
                txtFormaPago.ReadOnly = true;
                txtCuota.Text = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["NRO_CUOTA"]); ;
                txtCuota.ReadOnly = true;

                TextBox txtMAI = (TextBox)e.Row.FindControl("txtMAINoEdit");
                txtMAI.Text = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["MAI"]);
                double nMAI = Funciones.CheckDbl(txtMAI.Text);

                int nNumCuotas = Funciones.CheckInt(txtCuota.Text);
                TextBox txtCM = (TextBox)e.Row.FindControl("txtCMNoEdit");
                txtCM.Text = CalcularCM(dblTotalCostoInstalacionHFC1, nMAI, nNumCuotas).ToString();

                TextBox txtFormaPago2 = (TextBox)e.Row.FindControl("txtFormaPago2");
                TextBox txtCuota2 = (TextBox)e.Row.FindControl("txtCuotas2");

                txtFormaPago2.Text = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["FORMA_PAGO_MAN"]); ;
                txtFormaPago2.ReadOnly = true;
                txtCuota2.Text = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["NRO_CUOTA_MAN"]); ;
                txtCuota2.ReadOnly = true;

                TextBox txtMAI2 = (TextBox)e.Row.FindControl("txtMAINoEdit2");
                txtMAI2.Text = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["MAI_MAN"]);
                double nMAI2 = Funciones.CheckDbl(txtMAI2.Text);

                int nNumCuotas2 = Funciones.CheckInt(txtCuota2.Text);

                TextBox txtCM2 = (TextBox)e.Row.FindControl("txtCMNoEdit2");
                txtCM2.Text = CalcularCM(dblTotalCostoInstalacionHFC, nMAI2, nNumCuotas2).ToString();
                //PROY-140546 Cobro Anticipado de Instalacion
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label txtTotalCostoInstalacion = (Label)e.Row.FindControl("txtTotalCostoInstalacion");

                txtTotalCostoInstalacion.Text = string.Format("{0:#,#,#,0.00}", dblTotalCostoInstalacionHFC);
            }
        }

        //PROY-31948 INI
        private void IniciarControlCuota(Int64 nroSEC)
        {

            // Consulta Datos BRMS
            objLog.CrearArchivolog("    Inicio/ObtenerDatosBRMS   ", nroSEC.ToString(), null);
            DataSet obj = (new BLEvaluacion()).ObtenerDatosBRMS(nroSEC);
            DataRow objCli;
            DataRow objPlanA;

            try
            {
                objCli = obj.Tables["CV_CLIENTE"].Rows[0];
                objPlanA = obj.Tables["CV_PLAN_ACTUAL"].Rows[0];

                txtTotalCuotasPend.Text = Funciones.CheckStr(objCli[20]) != string.Empty ? Funciones.CheckStr(objCli[20]) : "0";
                txtCantLineasCuota.Text = Funciones.CheckStr(objCli[21]) != string.Empty ? Funciones.CheckStr(objCli[21]) : "0";
                txtCantMaxCuotas.Text = Funciones.CheckStr(objCli[22]) != string.Empty ? Funciones.CheckStr(objCli[22]) : "0";
                txtTotalImportCuotaUlt.Text = Funciones.CheckStr(objCli[23]) != string.Empty ? Funciones.CheckStr(objCli[23]) : "0";
                txtCantTotalLineaCuotaUlt.Text = Funciones.CheckStr(objCli[24]) != string.Empty ? Funciones.CheckStr(objCli[24]) : "0";
                txtCantMaxCuotasGenUlt.Text = Funciones.CheckStr(objCli[25]) != string.Empty ? Funciones.CheckStr(objCli[25]) : "0";
                txtCantCuotasPendLinea.Text = Funciones.CheckStr(objPlanA[5]) != string.Empty ? Funciones.CheckStr(objPlanA[5]) : "0";
                txtMontoPendCuotasLinea.Text = Funciones.CheckStr(objPlanA[6]) != string.Empty ? Funciones.CheckStr(objPlanA[6]) : "0";

                //PROY-140743 - INICIO
                if (Equals(strTipoOperacion, "25")) 
                {
                    DataRow objOferta = obj.Tables["CV_OFERTA"].Rows[0];
                    strPromociones = Funciones.CheckStr(objOferta[17]);
                    strProd_Facturar = Funciones.CheckStr(objOferta[18]);
                    txtMontoPendiente.Text = Funciones.CheckStr(objCli[37]) != string.Empty ? Funciones.CheckStr(objCli[37]) : "0";
                    txtCantidadPlanes.Text = Funciones.CheckStr(objCli[38]) != string.Empty ? Funciones.CheckStr(objCli[38]) : "0";
                    txtCantidadMaxima.Text = Funciones.CheckStr(objCli[39]) != string.Empty ? Funciones.CheckStr(objCli[39]) : "0";
                    txtMontoPenCuo.Text = Funciones.CheckStr(objCli[40]) != string.Empty ? Funciones.CheckStr(objCli[40]) : "0";
                    txtCantidadPlaCuo.Text = Funciones.CheckStr(objCli[41]) != string.Empty ? Funciones.CheckStr(objCli[41]) : "0";
                    txtCantidadMaxCuo.Text = Funciones.CheckStr(objCli[41]) != string.Empty ? Funciones.CheckStr(objCli[42]) : "0";
                }
                //PROY-140743 - FIN

                //PROY-140579 RU07 NN INI
                txtWhitelist.Text = Funciones.CheckStr(objCli[36]);

                txtModaCedenteLineaReno.Text = Funciones.CheckStr(objPlanA[7]);
                txtMontoCuotaActual.Text = Funciones.CheckStr(objPlanA[8]);
                txtActiLineaReno.Text = Funciones.CheckStr(objPlanA[9]);

                objLog.CrearArchivolog("txtWhitelist=", Funciones.CheckStr(objCli[36]), null);
                objLog.CrearArchivolog("txtModaCedenteLineaReno=", Funciones.CheckStr(objPlanA[7]), null);
                objLog.CrearArchivolog("txtMontoCuotaActual=", Funciones.CheckStr(objPlanA[8]), null);//BIEN
                objLog.CrearArchivolog("txtActiLineaReno=", Funciones.CheckStr(objPlanA[9]), null);//BIEN
                //PROY-140579 RU07 NN FIN
                objLog.CrearArchivolog("    FIN/ObtenerDatosBRMS   ", nroSEC.ToString(), null);

            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("    [ERROR ONBTENER DATOS BRMS]", null, ex);
            }

        }
        //PROY-31948 FIN

        #endregion [Iniciar Controles]

        private void aprobarSEC()
        {
            //INC000002396378
            Int64 nroSEC = Funciones.CheckInt64(txtNroSEC.Text);
            BLSolicitud objSolicitud = new BLSolicitud();
            DataRow drSolicitud = null;

            string strArchivo = "Log_aprobarSEC";
            string idLog = string.Format("{0} - {1}", CurrentUsers, nroSEC);

            drSolicitud = objSolicitud.ObtenerSolicitudPersona(nroSEC).Rows[0];

            objLog.CrearArchivolog("[INC000003467242]", string.Format("{0}:{1}", "[sisact_reporte_evaluacion_persona][aprobarSEC] drSolicitud", Funciones.CheckStr(new JavaScriptSerializer().Serialize(Funciones.ConvertirDataRowADictionary(drSolicitud)))), null);            

            string EstadoSec = drSolicitud["ESTAC_CODIGO"].ToString();
            string EstadoSecRechazado = ReadKeySettings.Key_EstadoSecRechazado.ToString();
            string sTOFIC_CODIGO = Funciones.DevolverCodigoTipoOficina(drSolicitud["TOFIV_DESCRIPCION"].ToString()); //PROY-140546 Codigo Canal

            string Valor = string.Empty;
            string[] arrEstados = EstadoSecRechazado.Split('|');
            foreach (string EstadosRechazados in arrEstados)
            {
                if (EstadosRechazados == EstadoSec)
                {
                    Valor = "S";
                    break;
                }
            }
            GeneradorLog.EscribirLog(strArchivo, idLog, "[Valor de la Sec:]", Valor.ToString());


            GeneradorLog.EscribirLog(strArchivo, idLog, "[****** INICIO aprobarSEC() ******]", null);
            GeneradorLog.EscribirLog(strArchivo, idLog, "[nroSEC]", nroSEC.ToString());

            //INC000002396378

            if (Valor != "S")
            {
                Int64 nAprobado = 0;
                GeneradorLog.EscribirLog(strArchivo, idLog, "[Ingreso a Metodo][aprobarSEC]", null);

                string mensajeCP = string.Empty; //IN000000772139 INI
                string codigoCP = string.Empty; //IN000000772139 INI

                bool flagCPMovilBam = false;
                string tipoProductoMovil = ConfigurationManager.AppSettings["constTipoProductoMovil"].ToString();
                string tipoProductoBAM = ConfigurationManager.AppSettings["constTipoProductoBAM"].ToString();
                //PROY-26963 - IDEA-34399: Mejora Portabilidad Despacho PDV

                GeneradorLog.EscribirLog(strArchivo, idLog, "[tipoProductoMovil]", tipoProductoMovil.ToString());
                GeneradorLog.EscribirLog(strArchivo, idLog, "[tipoProductoBAM]", tipoProductoBAM.ToString());

                try
                {
                    GeneradorLog.EscribirLog(strArchivo, idLog, "[chkEditarNombre]", chkEditarNombre.Checked.ToString());

                    if (chkEditarNombre.Checked)
                    {

                        GeneradorLog.EscribirLog(strArchivo, idLog, "[hidTipoDoc]", hidTipoDoc.Value);
                        GeneradorLog.EscribirLog(strArchivo, idLog, "[hidNroDoc]", hidNroDoc.Value);
                        GeneradorLog.EscribirLog(strArchivo, idLog, "[hidNombres]", hidNombres.Value);
                        GeneradorLog.EscribirLog(strArchivo, idLog, "[hidApePaterno]", hidApePaterno.Value);
                        GeneradorLog.EscribirLog(strArchivo, idLog, "[hidApeMaterno]", hidApeMaterno.Value);
                        GeneradorLog.EscribirLog(strArchivo, idLog, "[txtNombreEditar]", Funciones.CheckStr(txtNombreEditar.Text).ToUpper());
                        GeneradorLog.EscribirLog(strArchivo, idLog, "[txtApePaternoEditar]", Funciones.CheckStr(txtApePaternoEditar.Text).ToUpper());
                        GeneradorLog.EscribirLog(strArchivo, idLog, "[txtApeMaternoEditar]", Funciones.CheckStr(txtApeMaternoEditar.Text).ToUpper());
                        GeneradorLog.EscribirLog(strArchivo, idLog, "[txtNroSEC]", Funciones.CheckStr(txtNroSEC.Text).ToUpper());
                        GeneradorLog.EscribirLog(strArchivo, idLog, "[CurrentTerminal]", CurrentTerminal);
                        GeneradorLog.EscribirLog(strArchivo, idLog, "[CurrentUser]", CurrentUser);
                        GeneradorLog.EscribirLog(strArchivo, idLog, "[txtFechaNac]", txtFechaNac.Text);

                        List<string> objEditarNombre = new List<string>();
                        objEditarNombre.Add(hidTipoDoc.Value);
                        objEditarNombre.Add(hidNroDoc.Value);
                        objEditarNombre.Add(hidNombres.Value);
                        objEditarNombre.Add(hidApePaterno.Value);
                        objEditarNombre.Add(hidApeMaterno.Value);
                        objEditarNombre.Add(Funciones.CheckStr(txtNombreEditar.Text).ToUpper());
                        objEditarNombre.Add(Funciones.CheckStr(txtApePaternoEditar.Text).ToUpper());
                        objEditarNombre.Add(Funciones.CheckStr(txtApeMaternoEditar.Text).ToUpper());
                        objEditarNombre.Add(txtNroSEC.Text);
                        objEditarNombre.Add(CurrentTerminal);
                        objEditarNombre.Add(CurrentUser);
                        objEditarNombre.Add(txtFechaNac.Text);

                        GeneradorLog.EscribirLog(strArchivo, idLog, "[****** INICIO  objSolicitud.Insertar_Correccion_Nombres(objEditarNombre) ******]", null);
                        objSolicitud.Insertar_Correccion_Nombres(objEditarNombre);
                        GeneradorLog.EscribirLog(strArchivo, idLog, "[****** FIN  objSolicitud.Insertar_Correccion_Nombres(objEditarNombre) ******]", null);
                    }

                    GeneradorLog.EscribirLog(strArchivo, idLog, "[txtNroSEC]", txtNroSEC.Text);
                    GeneradorLog.EscribirLog(strArchivo, idLog, "[hidFlagPortabilidad]", hidFlagPortabilidad.Value);
                    GeneradorLog.EscribirLog(strArchivo, idLog, "[hidFlgConvergente]", hidFlgConvergente.Value);
                    GeneradorLog.EscribirLog(strArchivo, idLog, "[txtComentarioAlPdv]", Funciones.CheckStr(txtComentarioAlPdv.Value).ToUpper());
                    GeneradorLog.EscribirLog(strArchivo, idLog, "[txtComentarioEvaluador]", Funciones.CheckStr(txtComentarioEvaluador.Value).ToUpper());
                    GeneradorLog.EscribirLog(strArchivo, idLog, "[hidGarantia]", hidGarantia.Value);
                    GeneradorLog.EscribirLog(strArchivo, idLog, "[hidCostoInstalacion]", hidCostoInstalacion.Value);
                    GeneradorLog.EscribirLog(strArchivo, idLog, "[CurrentUser]", CurrentUser);

                    List<string> objEvaluacion = new List<string>();

                    objEvaluacion = new List<string>();
                    objEvaluacion.Add(txtNroSEC.Text);
                    objEvaluacion.Add(hidFlagPortabilidad.Value);
                    objEvaluacion.Add(hidFlgConvergente.Value);
                    objEvaluacion.Add(Funciones.CheckStr(txtComentarioAlPdv.Value).ToUpper());
                    objEvaluacion.Add(Funciones.CheckStr(txtComentarioEvaluador.Value).ToUpper());
                    objEvaluacion.Add(Funciones.CheckStr(hidGarantia.Value));
                    objEvaluacion.Add(Funciones.CheckStr(hidCostoInstalacion.Value));
                    objEvaluacion.Add(CurrentUser);

                    GeneradorLog.EscribirLog(strArchivo, idLog, "Session(FlagPortabilidad)", Funciones.CheckStr(Session["FlagPortabilidad"]));
                    /*      GeneradorLog.EscribirLog(strArchivo, idLog, "txtNroSEC", Funciones.CheckStr(txtNroSEC.Text)); 
                          GeneradorLog.EscribirLog(strArchivo, idLog, "hidFlagPortabilidad", Funciones.CheckStr(hidFlagPortabilidad.Value)); 
                          GeneradorLog.EscribirLog(strArchivo, idLog, "hidFlgConvergente", Funciones.CheckStr(hidFlgConvergente.Value)); 
                          GeneradorLog.EscribirLog(strArchivo, idLog, "txtComentarioAlPdv", Funciones.CheckStr(txtComentarioAlPdv.Value)); 
                          GeneradorLog.EscribirLog(strArchivo, idLog, "txtComentarioEvaluador", Funciones.CheckStr(txtComentarioEvaluador.Value)); 
                          GeneradorLog.EscribirLog(strArchivo, idLog, "hidGarantia", Funciones.CheckStr(hidGarantia.Value)); 
                          GeneradorLog.EscribirLog(strArchivo, idLog, "hidCostoInstalacion", Funciones.CheckStr(hidCostoInstalacion.Value)); 
                          GeneradorLog.EscribirLog(strArchivo, idLog, "CurrentUser", Funciones.CheckStr(CurrentUser)); */


                    try
                    {
                        GeneradorLog.EscribirLog(strArchivo, idLog, "[****** INICIO objSolicitud.AprobarCreditos(objEvaluacion) ******]", null);
                        objSolicitud.AprobarCreditos(objEvaluacion);
                        nAprobado = 1;
                        GeneradorLog.EscribirLog(strArchivo, idLog, "[nAprobado]", nAprobado);
                        GeneradorLog.EscribirLog(strArchivo, idLog, "[****** FIN objSolicitud.AprobarCreditos(objEvaluacion) ******]", null);
                    }
                    catch (Exception ex)
                    {
                        GeneradorLog.EscribirLog(strArchivo, idLog, "ERROR EN AprobarCreditos(objEvaluacion)", ex.Message);
                    }

                    GeneradorLog.EscribirLog(strArchivo, idLog, "hidFlagPortabilidad", Funciones.CheckStr(hidFlagPortabilidad.Value));

                    // Envio Pool Port-IN
                    if (Funciones.CheckStr(hidFlagPortabilidad.Value) == "S" || Funciones.CheckStr(Session["FlagPortabilidad"]) == "S")
                    {
                        //PROY-26963 - IDEA-34399: Mejora Portabilidad Despacho PDV
                        GeneradorLog.EscribirLog(strArchivo, idLog, "INICIO ObtenerDatosPorta", nroSEC);
                        //PROY-140126 - IDEA 140248 INICIO
                        GeneradorLog.EscribirLog(strArchivo, idLog, "INICIO DatosSolicitud", nroSEC);
                        DataTable DatosSolicitud = null;
                        DatosSolicitud = (DataTable)HttpContext.Current.Session["SolicitudPersona"];
                        GeneradorLog.EscribirLog(strArchivo, idLog, "Session[SolicitudPersona]", nroSEC);
                        BeConsultaPrevia objConsultaPrevia = ObtenerDatosPorta(DatosSolicitud.Rows[0]);
                        GeneradorLog.EscribirLog(strArchivo, idLog, "FIN DatosSolicitud", Funciones.CheckStr(objConsultaPrevia.tipoProdcuto));
                        //PROY-140126 - IDEA 140248 FIN
                        GeneradorLog.EscribirLog(strArchivo, idLog, "ObtenerDatosPorta[tipoProdcuto]", Funciones.CheckStr(objConsultaPrevia.tipoProdcuto));
                        if (objConsultaPrevia.tipoProdcuto == tipoProductoMovil || objConsultaPrevia.tipoProdcuto == tipoProductoBAM)
                        {
                            flagCPMovilBam = true;
                            objConsultaPrevia.auditoria = new BEItemGenerico()
                            {
                                Codigo = DateTime.Now.ToString("yyyyMMddhhmmssff"),
                                Descripcion2 = CurrentTerminal,
                                Descripcion = ConfigurationManager.AppSettings["constNombreAplicacion"].ToString(),
                                Codigo2 = CurrentUser
                            };

                            GeneradorLog.EscribirLog(strArchivo, idLog, "ActualizarConsultaPrevia[idTransaccion]", objConsultaPrevia.auditoria.Codigo);
                            GeneradorLog.EscribirLog(strArchivo, idLog, "ActualizarConsultaPrevia[Usuario]", objConsultaPrevia.auditoria.Codigo2);

                            //INC000002528095 INI
                            GeneradorLog.EscribirLog(strArchivo, idLog, "[INC000002528095]- ActualizarConsultaPrevia | objConsultaPrevia.numeroSEC: ", Funciones.CheckStr(objConsultaPrevia.numeroSEC));
                            if (objConsultaPrevia.numeroSEC != nroSEC)
                            {
                                GeneradorLog.EscribirLog(strArchivo, idLog, "INICIO Recargar DatosSolicitud | nroSEC: ", nroSEC);
                                DataTable dtSolicitudR = new DataTable();
                                dtSolicitudR = objSolicitud.ObtenerSolicitudPersona(nroSEC);
                                GeneradorLog.EscribirLog(strArchivo, idLog, "dtSolicitudR: ", Funciones.CheckStr(dtSolicitudR.Rows[0]));
                                objConsultaPrevia = ObtenerDatosPorta(dtSolicitudR.Rows[0]);
                                GeneradorLog.EscribirLog(strArchivo, idLog, "[INC000002528095]- FIN Recargar DatosSolicitud ", Funciones.CheckStr(objConsultaPrevia.numeroSEC));
                            }
                            //INC000002528095 FIN

                            BEItemMensaje objMensaje = new WebComunes().ActualizarConsultaPrevia(objConsultaPrevia);
                            //IN000000772139 INI
                            mensajeCP = objMensaje.mensajeCliente;
                            codigoCP = objMensaje.codigo;
                            //IN000000772139 FIN
                            GeneradorLog.EscribirLog(strArchivo, idLog, "ActualizarConsultaPrevia[objMensaje.codigo]", objMensaje.codigo);
                            GeneradorLog.EscribirLog(strArchivo, idLog, "ActualizarConsultaPrevia[objMensaje.descripcion]", objMensaje.descripcion);
                        }
                        else
                        {
                            new BLPortabilidad().EnviarMesaPortabilidad(nroSEC, CurrentUser);
                        }

                        GeneradorLog.EscribirLog(strArchivo, idLog, "FIN ObtenerDatosPorta", nroSEC);
                    }
                    //PROY-26963 - IDEA-34399: Mejora Portabilidad Despacho PDV
                    if (nAprobado == 1)
                    {
                        // Registro Historico
                        Int64 intentoEstadoPoolEvaluador = 1;
                        Int64 intentoEstadoAprobado = 1;
                        GeneradorLog.EscribirLog(strArchivo, idLog, "[****** INICIO objSolicitud.GrabarLogHistorico(nroSEC, constEstadoPoolEvaluador, CurrentUser) ******]", Funciones.CheckStr(nroSEC));
                        GeneradorLog.EscribirLog(strArchivo, idLog, "[constEstadoPoolEvaluador]", constEstadoPoolEvaluador);
                        GeneradorLog.EscribirLog(strArchivo, idLog, "[CurrentUser]", CurrentUser);
                        try
                        {
                            GeneradorLog.EscribirLog(strArchivo, idLog, "[GrabarLogHistorico EstadoPoolEvaluador intento ]", intentoEstadoPoolEvaluador);
                            objSolicitud.GrabarLogHistorico(nroSEC, constEstadoPoolEvaluador, CurrentUser);
                        }
                        catch (Exception exH1)
                        {
                            intentoEstadoPoolEvaluador = 2;
                            GeneradorLog.EscribirLog(strArchivo, idLog, "[Exception GrabarLogHistorico EstadoPoolEvaluador Intento]" + intentoEstadoPoolEvaluador, exH1.Message);
                        }
                        if (intentoEstadoPoolEvaluador == 2)
                        {
                            try
                            {
                                GeneradorLog.EscribirLog(strArchivo, idLog, "[GrabarLogHistorico EstadoPoolEvaluador intento: ]", intentoEstadoPoolEvaluador);
                                objSolicitud.GrabarLogHistorico(nroSEC, constEstadoPoolEvaluador, CurrentUser);
                            }
                            catch (Exception exH2)
                            {
                                GeneradorLog.EscribirLog(strArchivo, idLog, "[Exception GrabarLogHistorico EstadoPoolEvaluador Intento]" + intentoEstadoPoolEvaluador, exH2.Message);
                            }
                        }
                        GeneradorLog.EscribirLog(strArchivo, idLog, "[****** FIN objSolicitud.GrabarLogHistorico(nroSEC, constEstadoPoolEvaluador, CurrentUser) ******]", Funciones.CheckStr(nroSEC));

                        // Registro Estado Aprobado
                        GeneradorLog.EscribirLog(strArchivo, idLog, "Registro en Historico: [hidFlagPortabilidad]", hidFlagPortabilidad.Value);
                        GeneradorLog.EscribirLog(strArchivo, idLog, "[constEstadoAprobado]", constEstadoAprobado);
                        GeneradorLog.EscribirLog(strArchivo, idLog, "[CurrentUser]", CurrentUser);
                        if (hidFlagPortabilidad.Value != "S")
                        {
                            try
                            {
                                GeneradorLog.EscribirLog(strArchivo, idLog, "[GrabarLogHistorico EstadoAprobado intento ]", intentoEstadoAprobado);
                                objSolicitud.GrabarLogHistorico(nroSEC, constEstadoAprobado, CurrentUser);
                            }
                            catch (Exception exH1)
                            {
                                intentoEstadoAprobado = 2;
                                GeneradorLog.EscribirLog(strArchivo, idLog, "[Exception GrabarLogHistorico EstadoAprobado Intento]" + intentoEstadoAprobado, exH1.Message);
                            }
                            if (intentoEstadoAprobado == 2)
                            {
                                try
                                {
                                    GeneradorLog.EscribirLog(strArchivo, idLog, "[GrabarLogHistorico EstadoAprobado intento: ]", intentoEstadoAprobado);
                                    objSolicitud.GrabarLogHistorico(nroSEC, constEstadoAprobado, CurrentUser);
                                }
                                catch (Exception exH2)
                                {
                                    GeneradorLog.EscribirLog(strArchivo, idLog, "[Exception GrabarLogHistorico EstadoAprobado Intento]" + intentoEstadoAprobado, exH2.Message);
                                }
                            }
                        }
                        Session["AprobarSec"] = "OK";
                        GeneradorLog.EscribirLog(strArchivo, idLog, "[Session[AprobarSec]] - CAMBIO DE VALOR ", Funciones.CheckStr(Session["AprobarSec"]));
                    }
                    // Registro Tiempo de Atención
                    GeneradorLog.EscribirLog(strArchivo, idLog, "[****** INICIO registrarTiempoAprobar(nroSEC) ******]", Funciones.CheckStr(nroSEC));
                    try
                    {
                        registrarTiempoAprobar(nroSEC);
                    }
                    catch (Exception exRTA)
                    {
                        GeneradorLog.EscribirLog(strArchivo, idLog, "[****** EXCEPTION registrarTiempoAprobar(nroSEC) ******]", exRTA.Message);
                    }

                    GeneradorLog.EscribirLog(strArchivo, idLog, "[****** FIN registrarTiempoAprobar(nroSEC) ******]", Funciones.CheckStr(nroSEC));

                    //Inicio IDEA-30067
                    GeneradorLog.EscribirLog(strArchivo, idLog, "[constFlagPortaAutomatico]", Funciones.CheckStr(constFlagPortaAutomatico));
                    GeneradorLog.EscribirLog(strArchivo, idLog, "[flagCPMovilBam]", Funciones.CheckStr(flagCPMovilBam));
                    if (constFlagPortaAutomatico == "S" && flagCPMovilBam == false)
                    {
                        GeneradorLog.EscribirLog(strArchivo, idLog, "[****** INICIO PortaConsultaPreviaAutomatico() ******]", Funciones.CheckStr(nroSEC));

                        PortaConsultaPreviaAutomatico(); //****cambio

                        GeneradorLog.EscribirLog(strArchivo, idLog, "[****** FIN PortaConsultaPreviaAutomatico() ******]", Funciones.CheckStr(nroSEC));
                    }
                    //Fin IDEA-30067

                    //PROY-29215 INICIO 
                    objLog.CrearArchivolog(string.Format("[INICIO] | Grabar Forma de Pago y Nro de Cuota: "), null, null);
                    if (Funciones.CheckStr(hidFlgProductoDTH.Value) == "S" || Funciones.CheckStr(hidFlgProductoHFC.Value) == "S")
                    {
                        string nroCuota = hidCuota.Value.ToUpper();
                        string formapago = hidFormaPago.Value.ToUpper();
                        string formapagoActual = hidFormaPagoActual.Value.ToUpper();
                        string cuotaActual = hidCuotaActual.Value;
                        string Asesor = Funciones.CheckStr(CurrentUserSession.Nombre + " " + CurrentUserSession.Apellido_Pat + " " + CurrentUserSession.Apellido_Mat);

                        objLog.CrearArchivolog(string.Format("{0}", "Nro Cuota", nroCuota), null, null);
                        objLog.CrearArchivolog(string.Format("{0}", "Forma de Pago", formapago), null, null);
                        objLog.CrearArchivolog(string.Format("{0}", "Forma Pago Actual", formapagoActual), null, null);
                        objLog.CrearArchivolog(string.Format("{0}", "Cuota Actual", cuotaActual), null, null);
                        objLog.CrearArchivolog(string.Format("{0}", "Asesor", Asesor), null, null);

                        if (formapagoActual.ToUpper() != formapago.ToUpper() || cuotaActual != nroCuota)
                        {
                            objLog.CrearArchivolog(string.Format("Inicio Grabar"), null, null);
                            new BLConsumer().GrabarFormaPagoCuota(Funciones.CheckInt64(txtNroSEC.Text), Funciones.CheckInt16(nroCuota), formapago);
                            objLog.CrearArchivolog(string.Format("Fin Grabar"), null, null);

                            objLog.CrearArchivolog("[GrabarFormaPagoCuota] INICIO", null, null);
                            if (formapagoActual != formapago & cuotaActual == nroCuota)
                            {
                                objLog.CrearArchivolog(string.Format("{0}", "[GrabarFormaPagoCuota] | NOMBRE DEL ASESOR: ", Asesor), null, null);
                                objLog.CrearArchivolog(string.Format("{0}", "[GrabarFormaPagoCuota] | FORMA PAGO: ", formapago), null, null);
                            }
                            else if (cuotaActual != nroCuota & formapagoActual == formapago)
                            {
                                objLog.CrearArchivolog(string.Format("{0}", "[GrabarFormaPagoCuota] | NOMBRE DEL ASESOR: ", Asesor), null, null);
                                objLog.CrearArchivolog(string.Format("{0}", "[GrabarFormaPagoCuota] | NRO. CUOTAS: ", nroCuota.ToString()), null, null);
                            }
                            else if (formapagoActual != formapago & cuotaActual != nroCuota)
                            {
                                objLog.CrearArchivolog(string.Format("{0}", "[GrabarFormaPagoCuota] | NOMBRE DEL ASESOR: ", Asesor), null, null);
                                objLog.CrearArchivolog(string.Format("{0}", "[GrabarFormaPagoCuota] | FORMA PAGO: ", formapago), null, null);
                                objLog.CrearArchivolog(string.Format("{0}", "[GrabarFormaPagoCuota] | NRO. CUOTAS: ", nroCuota.ToString()), null, null);
                            }
                        }

                        //INICIO PROY-140546
                        bool bEsCanalPermitido = Funciones.EsValorPermitido(sTOFIC_CODIGO, ReadKeySettings.Key_CanalesPermitidosCAI, ",");
                        if (bEsCanalPermitido && Funciones.CheckStr(hidFlgProductoHFC.Value) == "S")
                        {
                            RestConsultarPagoAnticipadoFija oService = new RestConsultarPagoAnticipadoFija();

                            BEAuditoriaRequest oAuditoriaRequest = new BEAuditoriaRequest();
                            oAuditoriaRequest.userId = Funciones.CheckStr(ConfigurationManager.AppSettings["system_ConsultaClave"]);
                            oAuditoriaRequest.ipApplication = CurrentServer;

                            PAGOANTICIPADO_ENTITY.DataPower.ActualizaPA.ActualizaPAGenericRequest oGenericRequest = new Entity.ClientePagoAnticipadoFijaRest.DataPower.ActualizaPA.ActualizaPAGenericRequest();
                            PAGOANTICIPADO_ENTITY.DataPower.ActualizaPA.ActualizaPAMessageRequest oMessageRequest = new Entity.ClientePagoAnticipadoFijaRest.DataPower.ActualizaPA.ActualizaPAMessageRequest();

                            HeaderRequest oHeaderRequest = new HeaderRequest();
                            oHeaderRequest.consumer = ReadKeySettings.ConsConsumerConsultaPA;
                            oHeaderRequest.country = ReadKeySettings.ConsCountryConsultaPA;
                            oHeaderRequest.dispositivo = ReadKeySettings.ConsDispositivoConsultaPA;
                            oHeaderRequest.language = ReadKeySettings.ConsLanguageConsultaPA;
                            oHeaderRequest.modulo = ReadKeySettings.ConsModuloConsultaPA;
                            oHeaderRequest.msgType = ReadKeySettings.ConsMsgTypeConsultaPA;
                            oHeaderRequest.operation = "actualizaPA";
                            oHeaderRequest.pid = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                            oHeaderRequest.system = ReadKeySettings.ConsSystemConsultaPA;
                            oHeaderRequest.timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
                            oHeaderRequest.userId = CurrentUsers;
                            oHeaderRequest.wsIp = ReadKeySettings.ConsCurrentIP;

                            oMessageRequest.Header = new HeadersRequest();
                            oMessageRequest.Header.HeaderRequest = oHeaderRequest;

                            List<PAGOANTICIPADO_ENTITY.DataPower.ActualizaPA.ActualizaPARequestType> oListaRequest = new List<Entity.ClientePagoAnticipadoFijaRest.DataPower.ActualizaPA.ActualizaPARequestType>();
                            PAGOANTICIPADO_ENTITY.DataPower.ActualizaPA.ActualizaPARequestType oBeanRequest = new Entity.ClientePagoAnticipadoFijaRest.DataPower.ActualizaPA.ActualizaPARequestType();
                            oBeanRequest.estado = (Funciones.CheckStr(hidEstadoPa.Value) == "5") ? "8" : "0";
                            oBeanRequest.numeroSolicitud = nroSEC;
                            oBeanRequest.usuarioActualizacion = CurrentUser;
                            oBeanRequest.montoInicialModificado = hidMAI.Value;

                            oListaRequest.Add(oBeanRequest);

                            oMessageRequest.Body = new Entity.ClientePagoAnticipadoFijaRest.DataPower.ActualizaPA.ActualizaPABody();
                            oMessageRequest.Body.actualizaPARequest = new Entity.ClientePagoAnticipadoFijaRest.DataPower.ActualizaPA.ActualizaPARequest();
                            oMessageRequest.Body.actualizaPARequest.actualizaPARequestType = oListaRequest;

                            oGenericRequest.MessageRequest = oMessageRequest;

                            bool bResultado = oService.ActualizarPagoAnticipado(oGenericRequest); //COMENTADO LMEDRANO
                            
                            if (bResultado)
                            {
                                /* rmr ca fase 2: ini */
                                GeneradorLog.EscribirLog(strArchivo, idLog, "[RegistraHistorialPagoAnticipado]", "Ini");

                                string sCostoInstalacion = "0";
                                try
                                {
                                    sCostoInstalacion = hidCostoInstalacion.Value.Split(';')[1].Substring(0, hidCostoInstalacion.Value.Split(';')[1].Length - 1);
                                }
                                catch (Exception ex2)
                                {
                                    GeneradorLog.EscribirLog(strArchivo, idLog, "ex2" + ex2.Message, "Error");
                                    sCostoInstalacion = "0";
                                }

                                RegistraHistorialPagoAnticipado(nroSEC, CurrentUser, Funciones.CheckDbl(sCostoInstalacion), formapago, Funciones.CheckInt(nroCuota), Funciones.CheckDbl(hidMAI.Value));
                                GeneradorLog.EscribirLog(strArchivo, idLog, "[RegistraHistorialPagoAnticipado]", "Fin");
                                /* rmr ca fase 2: fin */
                            }
                        }
                        //FIN PROY-140546
                    }
                    //PROY-29215 FIN

                    hidProceso.Value = "OK";
                    GeneradorLog.EscribirLog(strArchivo, idLog, "[hidProceso.Value]", Funciones.CheckStr(hidProceso.Value));
                    GeneradorLog.EscribirLog(strArchivo, idLog, "[codigoCP]", Funciones.CheckStr(codigoCP));
                    //IN000000772139 INI
                    if (codigoCP != "0")
                    {
                        //PROY-140223 IDEA-140462
                        if (AppSettings.consFlagConsultaPreviaChip == "1")
                        {
                            hidnMensajeValue.Value = string.Format(ConfigurationManager.AppSettings["consMsjAprobarEvaluadorPersona"].ToString(), nroSEC.ToString()) + ".";

                        }//PROY-140223 IDEA-140462
                        else
                        {
                            hidnMensajeValue.Value = string.Format(ConfigurationManager.AppSettings["consMsjAprobarEvaluadorPersona"].ToString(), nroSEC.ToString()) +
                            "." + string.Format(mensajeCP.ToString(), nroSEC.ToString()) + ".";
                        }
                    }
                    else
                    {
                        hidnMensajeValue.Value = string.Format(ConfigurationManager.AppSettings["consMsjAprobarEvaluadorPersona"].ToString(), nroSEC.ToString()); //PROY-24740
                    }
                    //IN000000772139 FIN
                    GeneradorLog.EscribirLog(strArchivo, idLog, "[hidnMensajeValue.Value]", Funciones.CheckStr(hidnMensajeValue.Value));
                }
                catch (Exception ex)
                {
                    GeneradorLog.EscribirLog(strArchivo, idLog, "[****** EXCEPCION EN LA APROBACION DE LA SEC ******]", Funciones.CheckStr(nroSEC));
                    Session["AprobarSec"] = "PENDIENTE";
                    GeneradorLog.EscribirLog(strArchivo, idLog, "[Session[AprobarSec]] - Exception", Funciones.CheckStr(Session["AprobarSec"]));
                    GeneradorLog.EscribirLog(strArchivo, idLog, "ERROR APROBAR SEC - Exception", ex.Message);
                }
                finally
                {
                    string strCodApl = ConfigurationManager.AppSettings["CodigoAplicacion"].ToString();
                    string strNomApl = ConfigurationManager.AppSettings["constNombreAplicacion"].ToString();
                    string strDesTrx = "Aceptacion o Rechazo de la SEC|" + strCodApl + "|" + strNomApl + "|9206|POOL DE EVALUACION|";
                    WebComunes.Auditoria(ConfigurationManager.AppSettings["CONS_COD_SACT_EVALS"].ToString(), strDesTrx);
                    GeneradorLog.EscribirLog(strArchivo, idLog, "****** FIN aprobarSEC() ******", null);
                }
            }
            else
            {
                GeneradorLog.EscribirLog(strArchivo, idLog, "****** Error aprobarSEC() ******", null);
                GeneradorLog.EscribirLog(strArchivo, idLog, "****** La SEC no fue aprobada porque esta en estado******", EstadoSec);
                GeneradorLog.EscribirLog(strArchivo, idLog, "****** FIN aprobarSEC() ******", null);
            }
        }

        [System.Web.Services.WebMethod()]
        public static BEResponseWebMethod liberarSEC(string nroSEC)
        {
            BEResponseWebMethod objResponse = new BEResponseWebMethod();
            GeneradorLog _objLog = CrearLogStatic(nroSEC);
            try
            {
                objResponse.TipoRespuesta = "B";
                objResponse.Boleano = new BLSolicitud().LiberarSEC(Funciones.CheckInt64(nroSEC));
            }
            catch (Exception ex)
            {
                objResponse.Boleano = false;
                objResponse.DescripcionError = ex.Message;
                objResponse.Mensaje = ConfigurationManager.AppSettings["consMsjErrorEvaluadorPersona"].ToString();
                _objLog.CrearArchivolog("[ERROR METODO LIBERAR_SEC]", null, ex);
            }
            return objResponse;
        }

        [System.Web.Services.WebMethod()]
        public static BEResponseWebMethod rechazarSEC(string strSEC, string flgConvergente, string strCadenaGarantia, string strCadenaCosto, string usuario,
                                                      string txtComentarioAlPdv, string txtComentarioEvaluador, string strFlgReingreso, string tiempoInicio)
        {
            BEResponseWebMethod objResponse = new BEResponseWebMethod();
            BLSolicitud objSolicitud = new BLSolicitud();
            GeneradorLog _objLog = CrearLogStatic(strSEC);
            try
            {
                objResponse.TipoRespuesta = "B";

                Int64 nroSEC = Funciones.CheckInt64(strSEC);
                txtComentarioAlPdv = Funciones.CheckStr(txtComentarioAlPdv).ToUpper();
                txtComentarioEvaluador = Funciones.CheckStr(txtComentarioEvaluador).ToUpper();
                string strEstadoRechazar = ConfigurationManager.AppSettings["constcodEstadoRECHAZADOEVALUACION"].ToString();

                // Grabar Rentas / Costo Instalación
                List<string> objEvaluacion = new List<string>();
                objEvaluacion = new List<string>();
                objEvaluacion.Add(strSEC);
                objEvaluacion.Add(flgConvergente);
                objEvaluacion.Add(Funciones.CheckStr(strCadenaGarantia));
                objEvaluacion.Add(Funciones.CheckStr(strCadenaCosto));
                objEvaluacion.Add(usuario);

                new BLSolicitud().ActualizarGarantia(objEvaluacion);

                objSolicitud.RechazarSEC(nroSEC, txtComentarioAlPdv, txtComentarioEvaluador, usuario, strFlgReingreso);
                objSolicitud.GrabarLogHistorico(nroSEC, strEstadoRechazar, usuario);

                objResponse.Boleano = true;
                objResponse.Mensaje = string.Format(ConfigurationManager.AppSettings["consMsjRechazarEvaluadorPersona"].ToString(), strSEC);

                registrarTiempoRechazar(nroSEC, usuario, tiempoInicio);
            }
            catch (Exception ex)
            {
                objResponse.Boleano = false;
                objResponse.DescripcionError = ex.Message;
                objResponse.Mensaje = ConfigurationManager.AppSettings["consMsjErrorEvaluadorPersona"].ToString();
                _objLog.CrearArchivolog("[ERROR METODO RECHAZAR_SEC]", null, ex);
            }
            finally
            {
                string strCodApl = ConfigurationManager.AppSettings["CodigoAplicacion"].ToString();
                string strNomApl = ConfigurationManager.AppSettings["constNombreAplicacion"].ToString();
                string strDesTrx = "Aceptacion o Rechazo de la SEC|" + strCodApl + "|" + strNomApl + "|9206|POOL DE EVALUACION|";
                WebComunes.Auditoria(ConfigurationManager.AppSettings["CONS_COD_SACT_EVALS"].ToString(), strDesTrx);
            }
            return objResponse;
        }

        [System.Web.Services.WebMethod()]
        public static BEResponseWebMethod anularSot(string tipoProducto, string nroSEC)
        {
            BEResponseWebMethod objResponse = new BEResponseWebMethod();
            GeneradorLog _objLog = CrearLogStatic(nroSEC);
            objResponse.TipoRespuesta = "B";
            string strArchivo = "Log_AnulacionSot";
            string idLog = CurrentUsers + " - " + nroSEC;
            try
            {
                DataRow drSolicitud = null;
                BLSolicitud objSolicitud = new BLSolicitud();
                drSolicitud = objSolicitud.ObtenerSolicitudPersona(Funciones.CheckInt64(nroSEC)).Rows[0];

                _objLog.CrearArchivolog("[INC000003467242]", string.Format("{0}:{1}", "[sisact_reporte_evaluacion_persona][anularSOT] drSolicitud", Funciones.CheckStr(new JavaScriptSerializer().Serialize(Funciones.ConvertirDataRowADictionary(drSolicitud)))), null);            

                string codEstadoSec = drSolicitud["ESTAC_CODIGO"].ToString();

                string strUsuarioRegistro = Funciones.CheckStr(drSolicitud["SOLIN_USU_VEN"]);

                if (CurrentUsers.ToUpper() != strUsuarioRegistro.ToUpper())
                {
                    objResponse.Boleano = false;
                    objResponse.Mensaje = "Usted no está autorizado para realizar la anulación de la SOT.\nEsta acción lo puede realizar el usuario " + strUsuarioRegistro;
                }
                else
                {
                    if (codEstadoSec == ConfigurationManager.AppSettings["constCodEstadoAnulado"])
                    {
                        objResponse.Boleano = false;
                        objResponse.Mensaje = "La SEC ya se encuentra anulada.";
                    }
                    else
                    {
                        GeneradorLog.EscribirLog(strArchivo, idLog, "Inicio ObtenerHistoricoEstadosSOT", nroSEC);
                        List<BEEstado> listEstodosSot = (new BLSolicitud()).ObtenerHistoricoEstadosSOT(Funciones.CheckInt64(nroSEC));
                        GeneradorLog.EscribirLog(strArchivo, idLog, "Fin ObtenerHistoricoEstadosSOT", listEstodosSot.Count);
                        if (listEstodosSot.Count > 0)
                        {
                            bool blnAnular = true;

                            BEAcuerdo oAcuerdo = new BEAcuerdo();
                            List<BEAcuerdoDetalle> listAcuerdoDetalle = new List<BEAcuerdoDetalle>();
                            List<BEAcuerdoServicio> listAcuerdoServicio = new List<BEAcuerdoServicio>();

                            GeneradorLog.EscribirLog(strArchivo, idLog, "Inicio ObtenerAcuerdosBySec (sisact_pkg_acuerdo.sp_con_acuerdos_x_sec)", nroSEC);
                            BLSolicitud.ObtenerAcuerdosBySec(Funciones.CheckInt64(nroSEC), ref oAcuerdo, ref listAcuerdoDetalle, ref listAcuerdoServicio);
                            GeneradorLog.EscribirLog(strArchivo, idLog, "Fin ObtenerAcuerdosBySec", listAcuerdoDetalle.Count);

                            foreach (BEAcuerdoDetalle oDetalle in listAcuerdoDetalle)
                            {
                                if (oDetalle.Prdc_codigo != ConfigurationManager.AppSettings["consTipoProducto3Play"] && oDetalle.Co_id > 0)
                                {
                                    blnAnular = false;
                                }
                            }
                            GeneradorLog.EscribirLog(strArchivo, idLog, "blnAnular", blnAnular);
                            if (blnAnular)
                            {
                                Int64 nroSot = ((BEEstado)listEstodosSot[0]).NroSOT;

                                GeneradorLog.EscribirLog(strArchivo, idLog, "Inicio ObtenerEstadoSot (PQ_CONVERGENTE.p_estado_sot)", nroSEC, nroSot);
                                listEstodosSot = (new BLSolicitud()).ObtenerEstadoSot(Funciones.CheckInt64(nroSEC), nroSot);
                                GeneradorLog.EscribirLog(strArchivo, idLog, "Fin ObtenerEstadoSot", listEstodosSot.Count);

                                string strEstadoSot = ((BEEstado)listEstodosSot[0]).ESTAC_CODIGO;
                                string strDesEstadoSot = ((BEEstado)listEstodosSot[0]).ESTAV_DESCRIPCION;

                                GeneradorLog.EscribirLog(strArchivo, idLog, "strEstadoSot: " + strEstadoSot);
                                GeneradorLog.EscribirLog(strArchivo, idLog, "strDesEstadoSot: " + strDesEstadoSot);

                                string cadenaEstadosSot = ConfigurationManager.AppSettings["constCodEstadosSot_PermiteAnular"];
                                if (cadenaEstadosSot.Contains(strEstadoSot))
                                {
                                    string codResp = "", msgResp = "";
                                    GeneradorLog.EscribirLog(strArchivo, idLog, "Inicio AnularSot (PQ_CONVERGENTE.p_anular_solot)", nroSot);
                                    codResp = BLSolicitud.AnularSot(nroSot, ref msgResp);
                                    GeneradorLog.EscribirLog(strArchivo, idLog, "Fin AnularSot", codResp, msgResp);

                                    if (codResp == "0")
                                    {
                                        string nroDocCliente = Funciones.FormatoNroDocumento(drSolicitud["TDOCC_CODIGO"].ToString(), drSolicitud["CLIEC_NUM_DOC"].ToString());

                                        GeneradorLog.EscribirLog(strArchivo, idLog, "Inicio AnularSEC (sisact_pkg_dth.MANTSM_ANULAR_SEC)", nroSEC, nroDocCliente);
                                        BLSolicitud.AnularSEC(nroSEC, nroDocCliente, CurrentUsers);
                                        GeneradorLog.EscribirLog(strArchivo, idLog, "Fin AnularSEC");

                                        objResponse.Boleano = true;
                                        objResponse.Mensaje = "Se anuló correctamente la SOT";

                                        string strCodApl = ConfigurationManager.AppSettings["CodigoAplicacion"].ToString();
                                        string strNomApl = ConfigurationManager.AppSettings["constNombreAplicacion"].ToString();
                                        string strDesTrx = "Anulación de SOT " + nroSot.ToString();
                                        WebComunes.Auditoria(ConfigurationManager.AppSettings["CONS_COD_SACT_EVALS"].ToString(), strDesTrx);
                                    }
                                    else
                                    {
                                        objResponse.Boleano = false;
                                        objResponse.Mensaje = msgResp;
                                    }
                                }
                                else
                                {
                                    objResponse.Boleano = false;
                                    objResponse.Mensaje = "No es posible anular la SOT. La SOT " + nroSot.ToString() + " se encuentra en estado " + strDesEstadoSot;
                                }
                            }
                            else
                            {
                                objResponse.Boleano = false;
                                objResponse.Mensaje = "No es posible anular la SOT. La SEC tiene productos activados.";
                            }
                        }
                        else
                        {
                            objResponse.Boleano = false;
                            objResponse.Mensaje = "No existe SOT.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GeneradorLog.EscribirLog(strArchivo, idLog, "ERROR anularSot", ex.Message.ToString(), ex.StackTrace.ToString());
                objResponse.Boleano = false;
                objResponse.DescripcionError = ex.Message;
                objResponse.Mensaje = "Ocurrió un error en la anulación de la SOT.";
                _objLog.CrearArchivolog("[ERROR METODO anularSot]", null, ex);
            }
            return objResponse;
        }

        private void registrarTiempoAprobar(Int64 nroSEC)
        {
            bool blnOK;
            string strFecHrIni = hidTiempoInicio.Value;
            string strFecHrFin = DateTime.Now.ToString();
            string strCodPuntoVenta = string.Empty;
            string strCodCanal = string.Empty;
            string strCodVendedor = string.Empty;

            BLMaestro obj = new BLMaestro();
            DataTable dt = obj.ObtenerDatosRegistroTiempo(nroSEC);
            if (dt.Rows.Count > 0)
            {
                strCodPuntoVenta = dt.Rows[0]["ovenc_codigo"].ToString();
                strCodCanal = dt.Rows[0]["canac_codigo"].ToString();
                strCodVendedor = Funciones.CheckStr(dt.Rows[0]["solin_usu_ven"]);
            }

            blnOK = obj.RegistroTiempoPoolEval(nroSEC, strCodPuntoVenta, strCodCanal, strCodVendedor, CurrentUser, strFecHrIni, strFecHrFin, "0");
            blnOK = obj.RegistroTiempoActivaIni(nroSEC, strCodPuntoVenta, strCodCanal, strCodVendedor, strFecHrFin);
        }

        public static void registrarTiempoRechazar(Int64 nroSEC, string usuario, string strFecHrIni)
        {
            bool blnOK;
            string strFecHrFin = DateTime.Now.ToString();
            string strCodPuntoVenta = string.Empty;
            string strCodCanal = string.Empty;
            string strCodVendedor = string.Empty;

            BLMaestro obj = new BLMaestro();
            DataTable dt = obj.ObtenerDatosRegistroTiempo(nroSEC);
            if (dt.Rows.Count > 0)
            {
                strCodPuntoVenta = dt.Rows[0]["ovenc_codigo"].ToString();
                strCodCanal = dt.Rows[0]["canac_codigo"].ToString();
                strCodVendedor = dt.Rows[0]["solin_usu_ven"].ToString();
            }

            blnOK = obj.RegistroTiempoPoolEval(nroSEC, strCodPuntoVenta, strCodCanal, strCodVendedor, usuario, strFecHrIni, strFecHrFin, "1");
        }

        //Inicio IDEA-30067
        private void PortaConsultaPreviaAutomatico()
        {
            string strArchivo = "Log_aprobarSEC";
            string idLog = string.Format("{0} - {1}", CurrentUsers, Funciones.CheckStr(txtNroSEC.Text));

            try
            {

                objLog.CrearArchivolog("[Inicio][PortaConsultaPreviaAutomatico]", null, null);

                string constProductoPortAuto = ConfigurationManager.AppSettings["constProductoPortAuto"].ToString();

                bool blnPortabilidad = (hidFlagPortabilidad.Value == "S");
                bool blnProductoPortAuto = (constProductoPortAuto.IndexOf(hidProductoPortAuto.Value) >= 0);

                objLog.CrearArchivolog("[blnPortabilidad]", blnPortabilidad.ToString(), null);
                objLog.CrearArchivolog("[blnProductoPortAuto]", blnProductoPortAuto.ToString(), null);

                if (blnPortabilidad && blnProductoPortAuto)
                {
                    BWEnvioPorta objEnvioPortal = new BWEnvioPorta();
                    BEItemMensaje objMensaje = new BEItemMensaje(false);
                    string numeroSec = txtNroSEC.Text;
                    string observaciones = ConfigurationManager.AppSettings["consObservacionEnvioPorta"].ToString();

                    objLog.CrearArchivolog("[Cambio Portabilidad]", null, null);
                    objLog.CrearArchivolog("[numeroSec]", numeroSec.ToString(), null);
                    objLog.CrearArchivolog("[observaciones]", observaciones.ToString(), null);

                    BEItemGenerico objAudit = new BEItemGenerico();
                    objAudit.Codigo = txtNroSEC.Text + "_" + DateTime.Now.ToString("yyyyMMddhhmmss");
                    objAudit.Codigo2 = CurrentUser;
                    objAudit.Codigo3 = ConfigurationManager.AppSettings["CodigoAplicacion"].ToString();
                    objAudit.Descripcion = ConfigurationManager.AppSettings["ConstSistemaConsumer"].ToString();
                    objAudit.Descripcion2 = CurrentTerminal;
                    //INI: PROY-BLACKOUT
                    objAudit.Estado = AppSettings.consFlagBlackOut.ToString();
                    objAudit.Valor = AppSettings.consMensajeCPExitosoBlackOut;
                    string keyLog = string.Format("{0}|{1}|{2}", Funciones.CheckStr(txtNroSEC.Text), "PROY-BLACKOUT", "PortaConsultaPreviaAutomatico()");
                    CrearLogStatic(keyLog).CrearArchivolog("[INFO]-", "SE INICIO EL METODO", null);
                    CrearLogStatic(keyLog).CrearArchivolog(string.Format("{0},{1} ", "[INFO]-", " objAudit.Estado => "), " " + objAudit.Estado, null);
                    CrearLogStatic(keyLog).CrearArchivolog(string.Format("{0},{1} ", "[INFO]-", " objAudit.Valor => "), " " + objAudit.Valor, null);
                    CrearLogStatic(keyLog).CrearArchivolog("[INFO]-", "FIN DE PARAMETROS BLACKOUT CAPTURADOS", null);
                    //FIN : PROY-BLACKOUT
                    objMensaje = objEnvioPortal.realizarConsultaPrevia(numeroSec, observaciones, objAudit);

                    objLog.CrearArchivolog("[objMensaje.exito]", Funciones.CheckStr(objMensaje.exito), null);
                    objLog.CrearArchivolog("[objMensaje.codigo]", Funciones.CheckStr(objMensaje.codigo), null);
                    objLog.CrearArchivolog("[objMensaje.descripcion]", Funciones.CheckStr(objMensaje.descripcion), null);

                }
                else
                {
                    objLog.CrearArchivolog("[Sin Cambio Portabilidad]", null, null);
                    //objLog.CrearArchivolog("[ERROR][PortaConsultaPreviaAutomatico]", null, ex);
                }

                objLog.CrearArchivolog("[Fin][PortaConsultaPreviaAutomatico]", null, null);

            }

            catch (Exception ex)
            {

                GeneradorLog.EscribirLog(strArchivo, idLog, "Sec : " + txtNroSEC.Text + " - Error en PortaConsultaPreviaAutomatico() : ", ex.Message);

            }
        }
        //Fin IDEA-30067

        private void BuscarProteccionMovil(string strNroSec) //PROY-24724-IDEA-28174 - INICIO
        {
            GeneradorLog objLog = new GeneradorLog(CurrentUser, strNroSec, null, "WEB");
            objLog.CrearArchivolog(string.Format("{0}{1}", "[Inicio][BuscarProteccionMovil][NroSec] ", strNroSec), null, null);
            BEItemMensaje objMensaje = new BEItemMensaje(false);
            BWGestionaProteccionMovil objGestionaProteccionMovil = new BWGestionaProteccionMovil();
            BLProteccionMovil objProteccionMovil = new BLProteccionMovil();
            BEItemGenerico objAudit = new BEItemGenerico();
            objAudit.Codigo = string.Format("{0}{1}", Funciones.CheckStr(strNroSec), DateTime.Now.ToString("yyyyMMddhhmmss"));
            objAudit.Descripcion = Funciones.CheckStr(ConfigurationManager.AppSettings["ConstSistemaConsumer"]);
            objAudit.Codigo2 = CurrentUser;
            objAudit.Descripcion2 = CurrentTerminal;
            string strServicio = string.Empty;
            string strMetodo = string.Empty;
            string strCodRespuesta = string.Empty;
            string strMsjRespuesta = string.Empty;

            try
            {
                Claro.SISACT.WS.WSGestionaProteccionMovil.ObjetoPrimaType[] arrObjPrima = null;
                strMetodo = "buscarProteccionMovil";
                objMensaje = objGestionaProteccionMovil.BuscarProteccionMovil(strNroSec, objAudit, ref arrObjPrima);

                if (objMensaje.exito)
                {
                    objLog.CrearArchivolog("[Resultado OK][BuscarProteccionMovil]", null, null);
                    foreach (Claro.SISACT.WS.WSGestionaProteccionMovil.ObjetoPrimaType obj in arrObjPrima)
                    {
                        blnSecTieneProteccionMovil = true;
                        lstObjPrima.Add(new BEPrima()
                        {
                            CodEval = obj.codEval,
                            CodMaterial = obj.codMaterial,
                            SoplnCodigo = obj.codPlanSolicitud,
                            DeducibleDanio = obj.deducibleDanio,
                            DeducibleRobo = obj.deducibleRobo,
                            DescProd = obj.descProd,
                            DescProt = obj.descProt,
                            FechaCreacion = obj.fechaCreacion,
                            FechaEvaluacion = obj.fechaEvaluacion,
                            FechaModif = obj.fechaModif,
                            FlagEstado = obj.flagEstado,
                            IncidenciaTipoDanio = obj.incidenciaTipoDanio,
                            IncidenciaTipoRobo = obj.incidenciaTipoRobo,
                            Ip = obj.ip,
                            MontoPrima = obj.montoPrima,
                            NombreProd = obj.nombreProd,
                            NroCertif = obj.nroCertif,
                            NroDoc = obj.nroDoc,
                            NroSec = strNroSec,
                            Resultado = obj.resultado,
                            TipoCliente = obj.tipoCliente,
                            TipoDoc = obj.tipoDoc,
                            TipoOperacion = obj.tipoOperacion,
                            UsrAplicacion = obj.usrAplicacion,
                            UsrMod = obj.usrModif
                        });
                    }
                }
                else
                {
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[Error][BuscarProteccionMovil][Codigo] ", objMensaje.codigo), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[Error][BuscarProteccionMovil][Descripcion] ", objMensaje.descripcion), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[Error][BuscarProteccionMovil][MensajeCliente] ", objMensaje.mensajeCliente), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[Error][BuscarProteccionMovil][MensajeSistema] ", objMensaje.mensajeSistema), null, null);

                    objLog.CrearArchivolog(string.Format("{0}{1}", "[Inicio][BuscarProteccionMovilPvu][NroSec] ", strNroSec), null, null);
                    strMetodo = "BuscarProteccionMovilPvu";
                    lstObjPrima = objProteccionMovil.BuscarProteccionMovilPvu("", strNroSec, "", ref strCodRespuesta, ref strMsjRespuesta);
                    strMetodo = string.Empty;

                    if (strCodRespuesta != "0")
                    {
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[Error][BuscarProteccionMovilPvu][strCodRespuesta] ", strCodRespuesta), null, null);
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[Error][BuscarProteccionMovilPvu][strMsjRespuesta] ", strMsjRespuesta), null, null);
                        if (strCodRespuesta != "1") Alert(lstBEParametroProteccionMovil.Where(p => p.Valor1 == "37").SingleOrDefault().Valor);
                    }
                    else
                        objLog.CrearArchivolog("[Resultado OK][BuscarProteccionMovilPvu]", null, null);
                }
            }
            catch (Exception ex)
            {
                if (strMetodo == "BuscarProteccionMovilPvu")
                {
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[Error][BuscarProteccionMovil][MensajeSistema] ", ex.Message), null, null);
                }
                else if (strMetodo == "buscarProteccionMovil")
                {
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[Error][BuscarProteccionMovil][MensajeCliente] ", Funciones.CheckStr(ConfigurationManager.AppSettings["consGestionaProteccionMovilWS_Error"])), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[Error][BuscarProteccionMovil][MensajeSistema] ", ex.Message), null, null);

                    objLog.CrearArchivolog(string.Format("{0}{1}", "[Inicio][BuscarProteccionMovilPvu][NroSec] ", strNroSec), null, null);
                    strMetodo = "BuscarProteccionMovilPvu";
                    lstObjPrima = objProteccionMovil.BuscarProteccionMovilPvu("", strNroSec, "", ref strCodRespuesta, ref strMsjRespuesta);
                    strMetodo = string.Empty;

                    if (strCodRespuesta != "0")
                    {
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[Error][BuscarProteccionMovilPvu][strCodRespuesta] ", strCodRespuesta), null, null);
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[Error][BuscarProteccionMovilPvu][strMsjRespuesta] ", strMsjRespuesta), null, null);
                        if (strCodRespuesta != "1") Alert(lstBEParametroProteccionMovil.Where(p => p.Valor1 == "37").SingleOrDefault().Valor);
                    }
                    else
                    {
                        blnSecTieneProteccionMovil = true;
                        objLog.CrearArchivolog("[Resultado OK][BuscarProteccionMovilPvu]", null, null);
                    }
                }
            }
            finally
            {
                objLog.CrearArchivolog("[Fin][BuscarProteccionMovil]", null, null);
            }
        } //PROY-24724-IDEA-28174 - FIN

        //PROY-26963 - IDEA-34399: Mejora Portabilidad Despacho PDV
        public BeConsultaPrevia ObtenerDatosPorta(DataRow DatosSolicitud)
        {
            BeConsultaPrevia objLista = new BeConsultaPrevia();
            objLista.numeroSEC = Funciones.CheckInt64(DatosSolicitud["SOLIN_CODIGO"]);
            objLista.nombreRSAbonado = string.Format("{0} {1} {2}", DatosSolicitud["CLIEV_NOMBRE"], DatosSolicitud["CLIEV_APE_PAT"], DatosSolicitud["CLIEV_APE_MAT"]);
            objLista.codigoCedente = Funciones.CheckStr(DatosSolicitud["PORT_OPER_CED"].ToString());
            objLista.modalidad = Funciones.CheckStr(DatosSolicitud["TLINC_CODIGO_CEDENTE"].ToString());
            objLista.tipoProdcuto = Funciones.CheckStr(DatosSolicitud["PRDC_CODIGO"].ToString());
            objLista.tipoDocumento = DatosSolicitud["TDOCC_CODIGO"].ToString();
            objLista.numeroDocumento = Funciones.FormatoNroDocumento(DatosSolicitud["TDOCC_CODIGO"].ToString(), DatosSolicitud["CLIEC_NUM_DOC"].ToString());

            string canalVenta = Funciones.CheckStr(DatosSolicitud["TOFIV_DESCRIPCION"].ToString().ToUpper());
            objLista.modalidadVenta = Funciones.CheckStr(hidModVenta.Value);
            //PROY-140223 IDEA-140462
            switch (canalVenta)
            {
                case "CAC":
                    objLista.canalVenta = Funciones.CheckStr(ConfigurationManager.AppSettings["constCodTipoOficinaCAC"].ToString());

                    break;
                case "DAC":
                    objLista.canalVenta = Funciones.CheckStr(ConfigurationManager.AppSettings["constCodTipoOficinaDAC"].ToString());
                    break;
                case "CORNER":
                    objLista.canalVenta = Funciones.CheckStr(ConfigurationManager.AppSettings["constCodTipoOficinaCorner"].ToString());
                    break;
                default:
                    objLista.canalVenta = "";
                    break;

            }
            //PROY-140223 IDEA-140462

            objLog.CrearArchivolog("[INICIO][ObtenerDatosPorta]", null, null); // INC000001017818

            objLog.CrearArchivolog(string.Format("{0}{1}", "[ObtenerDatosPorta][numeroSEC] :  ", objLista.numeroSEC), null, null);
            objLog.CrearArchivolog(string.Format("{0}{1}", "[ObtenerDatosPorta][nombreRSAbonado] :  ", objLista.nombreRSAbonado), null, null);
            objLog.CrearArchivolog(string.Format("{0}{1}", "[ObtenerDatosPorta][codigoCedente] :  ", objLista.codigoCedente), null, null);
            objLog.CrearArchivolog(string.Format("{0}{1}", "[ObtenerDatosPorta][modalidad] :  ", objLista.modalidad), null, null);
            objLog.CrearArchivolog(string.Format("{0}{1}", "[ObtenerDatosPorta][tipoProdcuto] :  ", objLista.tipoProdcuto), null, null);
            objLog.CrearArchivolog(string.Format("{0}{1}", "[ObtenerDatosPorta][tipoDocumento] :  ", objLista.tipoDocumento), null, null);
            objLog.CrearArchivolog(string.Format("{0}{1}", "[ObtenerDatosPorta][numeroDocumento] :  ", objLista.numeroDocumento), null, null);

            objLog.CrearArchivolog("[FIN][ObtenerDatosPorta]", null, null); // INC000001017818


            return objLista;
        }
        //PROY-26963 - IDEA-34399: Mejora Portabilidad Despacho PDV

        public void CargarParametrosEvaluacionProactiva() //EMMH - INICIO
        {
            string strCodGrupoParamEvaluacionProactiva = Funciones.CheckStr(ConfigurationManager.AppSettings["consCodGrupoParamEvaluacionProactiva"]);
            if (!string.IsNullOrEmpty(strCodGrupoParamEvaluacionProactiva))
                lstBEEvaluacionProactiva = (new BLGeneral()).ListaParametrosGrupo(Funciones.CheckInt64(strCodGrupoParamEvaluacionProactiva));
            if (lstBEEvaluacionProactiva.Count > 0) lstBEEvaluacionProactiva = lstBEEvaluacionProactiva.OrderBy(p => p.Valor1).ToList();
            if (Session["ListaParametrosEP"] == null) Session["ListaParametrosEP"] = lstBEEvaluacionProactiva;
            hidFlagPlanesProactivos.Value = lstBEEvaluacionProactiva.Where(p => p.Valor1 == "ConstFlagPlanesProactivos").SingleOrDefault().Valor;
        } //EMMH - FIN

        //INC000003427525 - INI
        public static bool validaPermisos(string strCuentaRed)
        {
            bool respuesta = true;
            string strAbreviaturaPaginasN2 = string.Empty;
            GeneradorLog objLogINC = new GeneradorLog(null, "[INC000003427525]", null, " SEC USUARIO NO AUTORIZADO ");

            try
            {
                objLogINC.CrearArchivolog("************************** INICIO METODO VALIDAPERMISOS *************" + "", null, null);

                objLogINC.CrearArchivolog("[INC000003427525] - validaPermisos() -strCuentaRed ==> " + Funciones.CheckStr(strCuentaRed), null, null);

                if (!AccesoUsuario.LeerOpcionesPorUsuario(strCuentaRed))
                {
                    respuesta = false;
                }

                strAbreviaturaPaginasN2 = Funciones.CheckStr(HttpContext.Current.Session["AbreviaturaPaginasN2"]);

                objLogINC.CrearArchivolog("[INC000003427525] - validaPermisos() - strAbreviaturaPaginasN2 ==> " + Funciones.CheckStr(strAbreviaturaPaginasN2), null, null);

                if (strAbreviaturaPaginasN2.IndexOf(Funciones.CheckStr(AppSettings.Key_PaginaSec_AntiFraude)) == -1)
                {
                    respuesta = false;
                }

            }
            catch (Exception ex)
            {
                respuesta = false;
                objLogINC.CrearArchivolog("[INC000003427525] - validaPermisos() - ex.Message ==> " + Funciones.CheckStr(ex.Message), null, null);
                objLogINC.CrearArchivolog("[INC000003427525] - validaPermisos() - ex.StackTrace ==> " + Funciones.CheckStr(ex.StackTrace), null, null);
            }

            objLogINC.CrearArchivolog("[INC000003427525] - validaPermisos() -strCuentaRed ==> " + Funciones.CheckStr(respuesta), null, null);
            objLogINC.CrearArchivolog("************************** FIN METODO VALIDAPERMISOS *************" + "", null, null);

            return respuesta;
        }
        //INC000003427525 - FIN

        //PROY-140546 Cobro Anticipado de Instalacion Inicio
        public DataTable ObtenerDatosCostoInstalacion(Int64 pNroSec, string flagConsulta)
        {
            DataTable dtResultado = new DataTable();

            GeneradorLog objlog = new GeneradorLog(CurrentUsers, null, null, "WEB");
            objlog.CrearArchivolog("PROY-140546|sisact_reporte_evaluacion_persona|consultaHistorico|-- INICIO --", null, null);
            bool retorno = false;

            string formaPago = "", formaPagoMan = "", estado = "";
            int nroCuota = 0, nroCuotaMan = 0;
            double mai = 0, maiMan = 0;

            try
            {
                ConsultaHistGenericRequest oGenericRequest = new ConsultaHistGenericRequest();
                oGenericRequest.MessageRequest = new ConsultaHistMessageRequest();
                oGenericRequest.MessageRequest.Header = new Entity.ClientePagoAnticipadoFijaRest.DataPower.ConsultaPAHeader();
                oGenericRequest.MessageRequest.Header.HeaderRequest = new Entity.ClientePagoAnticipadoFijaRest.DataPower.ConsultaPAHeaderRequest();

                oGenericRequest.MessageRequest.Header.HeaderRequest.country = ReadKeySettings.ConsCountryConsultaPA;
                oGenericRequest.MessageRequest.Header.HeaderRequest.language = ReadKeySettings.ConsLanguageConsultaPA;
                oGenericRequest.MessageRequest.Header.HeaderRequest.consumer = ReadKeySettings.ConsConsumerConsultaPA;
                oGenericRequest.MessageRequest.Header.HeaderRequest.system = ReadKeySettings.ConsSystemConsultaPA;
                oGenericRequest.MessageRequest.Header.HeaderRequest.modulo = ReadKeySettings.ConsModuloConsultaPA;
                oGenericRequest.MessageRequest.Header.HeaderRequest.pid = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                oGenericRequest.MessageRequest.Header.HeaderRequest.userId = CurrentUsers;
                oGenericRequest.MessageRequest.Header.HeaderRequest.dispositivo = ReadKeySettings.ConsDispositivoConsultaPA;
                oGenericRequest.MessageRequest.Header.HeaderRequest.wsIp = ReadKeySettings.ConsCurrentIP;
                oGenericRequest.MessageRequest.Header.HeaderRequest.operation = ReadKeySettings.Key_OperationConsultaHistorico;
                oGenericRequest.MessageRequest.Header.HeaderRequest.timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
                oGenericRequest.MessageRequest.Header.HeaderRequest.msgType = ReadKeySettings.ConsMsgTypeConsultaPA;

                objlog.CrearArchivolog("- country -> " + Funciones.CheckStr(oGenericRequest.MessageRequest.Header.HeaderRequest.country), null, null);
                objlog.CrearArchivolog("- language -> " + Funciones.CheckStr(oGenericRequest.MessageRequest.Header.HeaderRequest.language), null, null);
                objlog.CrearArchivolog("- consumer -> " + Funciones.CheckStr(oGenericRequest.MessageRequest.Header.HeaderRequest.consumer), null, null);
                objlog.CrearArchivolog("- system -> " + Funciones.CheckStr(oGenericRequest.MessageRequest.Header.HeaderRequest.system), null, null);
                objlog.CrearArchivolog("- modulo -> " + Funciones.CheckStr(oGenericRequest.MessageRequest.Header.HeaderRequest.modulo), null, null);
                objlog.CrearArchivolog("- pid -> " + Funciones.CheckStr(oGenericRequest.MessageRequest.Header.HeaderRequest.pid), null, null);
                objlog.CrearArchivolog("- userId -> " + Funciones.CheckStr(oGenericRequest.MessageRequest.Header.HeaderRequest.userId), null, null);
                objlog.CrearArchivolog("- dispositivo -> " + Funciones.CheckStr(oGenericRequest.MessageRequest.Header.HeaderRequest.dispositivo), null, null);
                objlog.CrearArchivolog("- wsIp -> " + Funciones.CheckStr(oGenericRequest.MessageRequest.Header.HeaderRequest.wsIp), null, null);
                objlog.CrearArchivolog("- operation -> " + Funciones.CheckStr(oGenericRequest.MessageRequest.Header.HeaderRequest.operation), null, null);
                objlog.CrearArchivolog("- timestamp -> " + Funciones.CheckStr(oGenericRequest.MessageRequest.Header.HeaderRequest.timestamp), null, null);
                objlog.CrearArchivolog("- msgType -> " + Funciones.CheckStr(oGenericRequest.MessageRequest.Header.HeaderRequest.msgType), null, null);

                oGenericRequest.MessageRequest.Body = new ConsultaHistBodyRequest();

                objlog.CrearArchivolog("- INICIO MÉTODO consultaHistorico - ", null, null);
                objlog.CrearArchivolog("- pNroSec -> " + Funciones.CheckStr(pNroSec), null, null);

                oGenericRequest.MessageRequest.Body.consultaHistRequest = new ConsultaHistRequest();
                oGenericRequest.MessageRequest.Body.consultaHistRequest.solinCodigo = pNroSec;

                //string nombreServer = System.Net.Dns.GetHostName();
                string host = ReadKeySettings.ConsCurrentIP; //System.Net.Dns.GetHostEntry(nombreServer).AddressList[3].ToString();
                objlog.CrearArchivolog("- host -> " + Funciones.CheckStr(host), null, null);

                ConsultaHistGenericResponse outResponse = new ConsultaHistGenericResponse();
                RestReferencesConsultaHist oService = new RestReferencesConsultaHist();

                retorno = oService.consultarHistorico(oGenericRequest, host, ref outResponse);

                objlog.CrearArchivolog("- retorno -> " + Funciones.CheckStr(retorno), null, null);
                objlog.CrearArchivolog("- consultaHistoral.Length -> " + Funciones.CheckStr(outResponse.MessageResponse.Body.consultaHistResponseType.responseData.consultaHistoral.Length), null, null);
                hidEstadoPa.Value = "";
                if (retorno == true && outResponse.MessageResponse.Body.consultaHistResponseType.responseData.consultaHistoral.Length > 0)
                {
                    formaPago = outResponse.MessageResponse.Body.consultaHistResponseType.responseData.consultaHistoral[0].solivFormaPago;
                    formaPagoMan = outResponse.MessageResponse.Body.consultaHistResponseType.responseData.consultaHistoral[0].solivFormaPagoMan;
                    nroCuota = Funciones.CheckInt(outResponse.MessageResponse.Body.consultaHistResponseType.responseData.consultaHistoral[0].solinNumeroCuota);
                    nroCuotaMan = Funciones.CheckInt(outResponse.MessageResponse.Body.consultaHistResponseType.responseData.consultaHistoral[0].solinNumeroCuotaMan);
                    mai = Funciones.CheckDbl(outResponse.MessageResponse.Body.consultaHistResponseType.responseData.consultaHistoral[0].solinMontoAntiInst);
                    maiMan = Funciones.CheckDbl(outResponse.MessageResponse.Body.consultaHistResponseType.responseData.consultaHistoral[0].solinMontoAntInstMan);
                    estado = outResponse.MessageResponse.Body.consultaHistResponseType.responseData.consultaHistoral[0].pacEstado;
                    hidEstadoPa.Value = Funciones.CheckStr(estado);
                    bConsultaPAtieneData = true;
                }
            }
            catch(Exception e){
                objlog.CrearArchivolog(string.Format("{0} --> {1}", "[PROY-140546][ObtenerDatosCostoInstalacionPer]", " ERROR[" + e.Message + "|" + e.StackTrace + "]"), null, null);
            }

            objlog.CrearArchivolog("- formaPago -> " + Funciones.CheckStr(formaPago), null, null);
            objlog.CrearArchivolog("- formaPagoMan -> " + Funciones.CheckStr(formaPagoMan), null, null);
            objlog.CrearArchivolog("- nroCuota -> " + Funciones.CheckStr(nroCuota), null, null);
            objlog.CrearArchivolog("- nroCuotaMan -> " + Funciones.CheckStr(nroCuotaMan), null, null);
            objlog.CrearArchivolog("- mai -> " + Funciones.CheckStr(mai), null, null);
            objlog.CrearArchivolog("- maiMan -> " + Funciones.CheckStr(maiMan), null, null);
            objlog.CrearArchivolog("- estado -> " + Funciones.CheckStr(estado), null, null);
            objlog.CrearArchivolog("- bConsultaPAtieneData -> " + Funciones.CheckStr(bConsultaPAtieneData), null, null);

            dtResultado = (new BLSolicitud()).ObtenerCostoInstalacionHFC(Funciones.CheckInt64(pNroSec));

            objlog.CrearArchivolog("- flagConsulta -> " + Funciones.CheckStr(flagConsulta), null, null);
            if (flagConsulta == "S")
            {
                DataColumn dcFormaPago = new DataColumn("FORMA_PAGO", typeof(string));
                dcFormaPago.AllowDBNull = true;
                DataColumn dcFormaPagoMan = new DataColumn("FORMA_PAGO_MAN", typeof(string));
                dcFormaPagoMan.AllowDBNull = true;
                DataColumn dcNroCuota = new DataColumn("NRO_CUOTA", typeof(string));
                dcNroCuota.AllowDBNull = true;
                DataColumn dcNroCuotaMan = new DataColumn("NRO_CUOTA_MAN", typeof(string));
                dcNroCuotaMan.AllowDBNull = true;
                DataColumn dcMAI = new DataColumn("MAI", typeof(string));
                dcMAI.AllowDBNull = true;
                DataColumn dcMAIMan = new DataColumn("MAI_MAN", typeof(string));
                dcMAIMan.AllowDBNull = true;

                dtResultado.Columns.Add(dcFormaPago);
                dtResultado.Columns.Add(dcFormaPagoMan);
                dtResultado.Columns.Add(dcNroCuota);
                dtResultado.Columns.Add(dcNroCuotaMan);
                dtResultado.Columns.Add(dcMAI);
                dtResultado.Columns.Add(dcMAIMan);
            }
            else
            {
                DataColumn dcMAI = new DataColumn("MAI", typeof(string));
                dcMAI.AllowDBNull = true;
                DataColumn dcMAIMan = new DataColumn("MAI_MAN", typeof(string));
                dcMAIMan.AllowDBNull = true;

                dtResultado.Columns.Add(dcMAI);
                dtResultado.Columns.Add(dcMAIMan);
            }

            objlog.CrearArchivolog("- dtResultado.Rows.Count -> " + Funciones.CheckStr(dtResultado.Rows.Count), null, null);

            foreach (DataRow item in dtResultado.Rows)
            {
                if (flagConsulta == "S")
                {
                    item["FORMA_PAGO"] = formaPago;
                    item["FORMA_PAGO_MAN"] = formaPagoMan;
                    item["NRO_CUOTA"] = nroCuota;
                    item["NRO_CUOTA_MAN"] = nroCuotaMan;
                    item["MAI"] = mai;
                    item["MAI_MAN"] = maiMan;
                }
                else
                {
                    item["MAI"] = mai;
                    item["MAI_MAN"] = maiMan;
                }

                if (estado == "")
                {
                    lblFlagEstadoPagoCAI.Text = "NO";
                }
                else {
                    if (ReadKeySettings.Key_EstadosPago.IndexOf(estado) > -1)
                    {
                        lblFlagEstadoPagoCAI.Text = "SI";
                    }
                    else
                    {
                        lblFlagEstadoPagoCAI.Text = "NO";
                    }
                }
            }

            objlog.CrearArchivolog("PROY-140546|sisact_reporte_evaluacion_persona|ObtenerDatosCostoInstalacion|-- FIN --", null, null);
            return dtResultado;
        }

        private double CalcularCM(double nTotalCostoInstalacion, double nMAI, int nNumCuotas)
        {
            double nCuotaMensual = 0;
            double nSubTotalCostoInstalacion = 0;

            try
            {
                nSubTotalCostoInstalacion = nTotalCostoInstalacion - nMAI;

                if (nNumCuotas != 0)
                {
                    nCuotaMensual = nSubTotalCostoInstalacion / nNumCuotas;
                }
            }
            catch (Exception ex)
            {
                nCuotaMensual = 0;
            }

            return nCuotaMensual;
        }

        private void RegistraHistorialPagoAnticipado(Int64 nroSec, string usuario, double costoInstMan, string formaPagoMan, int numCuotaMan, double montoAntiInst)
        {
            BLPagoAnticipado oBLPagoAnticipado = new BLPagoAnticipado();

            RegistraHistorialRequest oRequest = new RegistraHistorialRequest();
            oRequest.numeroSolicitud = nroSec;
            oRequest.numeroSolicitudPlan = 0;
            oRequest.tipoProducto = "";
            oRequest.codigoPlan = "";
            oRequest.descripcionPlan = "";
            oRequest.fechaRegistro = "";
            oRequest.costoInstalacion = 0;
            oRequest.costoInstalacionManual = costoInstMan;//
            oRequest.formaPago = "";
            oRequest.formaPagoManual = formaPagoMan;//
            oRequest.numeroCuotas = 0;
            oRequest.numeroCuotasManual = numCuotaMan;//
            oRequest.montoAnticipadoInstalacion = 0;
            oRequest.montoAnticipadoInstalacionManual = montoAntiInst;//
            oRequest.usuarioActualizacion = usuario;
            oRequest.grupoSolicitud = 0;
            oRequest.puntoVenta = "";

            oBLPagoAnticipado.RegistraHistorialPagoAnticipado(oRequest);
        }
        //PROY-140546 Cobro Anticipado de Instalacion Fin
    }
}
