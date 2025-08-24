//INI PROY-31948_Migracion
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
using System.Text;
using System.Collections;
using Claro.SISACT.Web.Comun;
using System.Net;  //PROY-31948_Migracion

namespace Claro.SISACT.Web.Paginas.evaluacion_cons
{
    public partial class sisact_reporte_solicitud_corporativo_Pdv_Renovacion : Sisact_Webbase
    {
        GeneradorLog objLog = new GeneradorLog("    sisact_reporte_solicitud_corporativo_Pdv_Renovacion  ", null, null, "WEB");
        #region [Declaracion de Constantes - Config]

        double dblMontoGarantiaCred = 0;
        double dblMontoGarantiaEval = 0;
        double dblCFTotal = 0.0;
        double dblCFTotalHFC = 0.0;
        double dblTotalCostoInstalacion = 0.0;
        double dblTotalCostoInstalacionHFC = 0.0;

        GridViewRow dvrItem;
        List<BETipoGarantia> objTipoGarantia = new List<BETipoGarantia>();
        string constClaseEmpresaTop = Funciones.CheckStr(ConfigurationManager.AppSettings["constCodClasificacionEmpresaTop"]);
        string constFlgPortabilidad = Funciones.CheckStr(ConfigurationManager.AppSettings["FlagPortabilidad"]);
        string constTipoProductoDTH = Funciones.CheckStr(ConfigurationManager.AppSettings["constTipoProductoDTH"]);
        string constTipoProductoHFC = Funciones.CheckStr(ConfigurationManager.AppSettings["constTipoProductoHFC"]);
        string constTipoProductoVentaVarios = Funciones.CheckStr(ConfigurationManager.AppSettings["constTipoProductoVentaVarios"]);
        string constModalidadCuota = Funciones.CheckStr(ConfigurationManager.AppSettings["constCodModalidadCuota"]);        

        string constTipoProducto3PlayInalam = Funciones.CheckStr(ConfigurationManager.AppSettings["constTipoProducto3PlayInalam"]);        
        string constFlagPortaAutomatico = Funciones.CheckStr(ConfigurationManager.AppSettings["constFlagPortaAutomatico"]);


        //INI PROY-31948_Migracion
        string strEstado = Funciones.CheckStr(ConfigurationManager.AppSettings["constcodEstadoPOOLEVALUADORCONSUMER"]);
        string strEstadoAPR = Funciones.CheckStr(ConfigurationManager.AppSettings["constEstadoAPR"]);
        string strDesEstadoAPR = Funciones.CheckStr(ConfigurationManager.AppSettings["constDesResultadoFinalAPR"]);
        string strEstadoRZD = Funciones.CheckStr(ConfigurationManager.AppSettings["constcodEstadoRECHAZADO"]);
        string strDesEstadoRZD = Funciones.CheckStr(ConfigurationManager.AppSettings["constdesEstadoRECHAZADO"]);
        //FIN PROY-31948_Migracion

        
        BESolicitudPersona datos = new BESolicitudPersona();
        

        GeneradorLog _objLog = null;

        List<BEPrima> lstObjPrima = new List<BEPrima>();
        List<BEParametro> lstBEParametroProteccionMovil = new List<BEParametro>();
        bool blnSecTieneProteccionMovil = false;

        #endregion [Declaracion de Constantes - Config]
        List<BEParametro> lstBEEvaluacionProactiva = new List<BEParametro>();
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


            if (Session["Usuario"] == null)
            {
                string strUsuarioExt = Request.QueryString["cu"];
                hidUsuarioExt.Value = strUsuarioExt;
                if (!Base.AccesoUsuario.ValidarAcceso(strUsuarioExt, CurrentUser))
                {
                    string strRutaSite = ConfigurationManager.AppSettings["RutaSite"];
                    Response.Redirect(strRutaSite);
                    return;
                }
            }

            if (!Page.IsPostBack)
            {
                Inicio();
            }
            else
            {
                string accion = hidProceso.Value;
                hidProceso.Value = string.Empty;
                switch (accion)
                {
                    case "A":
                        grabar(accion);
                        break;
                    case "O":
                    case "S":
                    case "R":
                        grabar(accion);
                        break;
                    case "C":
                        grabar(accion);
                        break;
                }
            }
        }

        private void Inicio()
        {
            string flgSoloConsulta = Request.QueryString["flgSoloConsulta"]; //PROY-31948_HARDCODE s solo consulta n evaluar
            string flgOrigenPagina = Request.QueryString["flgOrigenPagina"]; //"P":CREDITOS/"C":CONSULTA SEC PROY-31948_HARDCODE
            Int64 nroSEC = Funciones.CheckInt64(Request.QueryString["nroSEC"]); //PROY-31948_HARDCODE
            //sec proactiva 25821386
            //string codProd = Request.QueryString["strIdProd"];
            string flagHistorico = Request.QueryString["flagHistorico"];

            string strNumeroSEC = Funciones.CheckStr(nroSEC);
            hdnflagHistorico.Value = flagHistorico;
           

            objLog.CrearArchivolog("    Inicio/flgSoloConsulta   ", flgSoloConsulta.ToString(), null);
            objLog.CrearArchivolog("    Inicio/flgOrigenPagina   ", flgOrigenPagina.ToString(), null);
            objLog.CrearArchivolog("    Inicio/nroSEC   ", strNumeroSEC, null);

            BEUsuarioSession objUsuarioSession = (BEUsuarioSession)Session["Usuario"];

            BLConsumer objConsumer = new BLConsumer();
            BLSolicitud objSolicitud = new BLSolicitud();
            DataTable dtDatosCreditos, dtDatosBilletera, dtDatosLineas, dtDatosGarantia, dtDetallePlan;

            List<BEEstado> objListaEstado = new List<BEEstado>();
            List<BESolicitudEmpresa> objListaHistorico = new List<BESolicitudEmpresa>();
            List<BEComentario> objListaComentario = new List<BEComentario>();

            // Creación Log Aplicación
            _objLog = CrearLog(strNumeroSEC);

            // Consulta Datos Solicitud
            BESolicitudEmpresa objCliente = objSolicitud.ObtenerSolicitudEmpresa(nroSEC);

            HttpContext.Current.Session["SolicitudEmpresa"] = objCliente;

            hidProductoPortAuto.Value = objCliente.PRDC_CODIGO;
            objLog.CrearArchivolog("    Inicio/codProd  ", Funciones.CheckStr(objCliente.PRDC_CODIGO), null);
            
            hidDeudaClienteEmpresa.Value = objCliente.SOLIC_DEUDA_CLIENTE; //PROY-29121
            // Asignar Usuario
            hidTiempoInicio.Value = DateTime.Now.ToString();
            hidFlgConsulta.Value = flgSoloConsulta;
            hidflgOrigenPagina.Value = flgOrigenPagina;
            hidListaPerfiles.Value = objUsuarioSession.CadenaOpcionesPagina;
            hidUsuario.Value = CurrentUser;

            if (flgOrigenPagina == "P" && flgSoloConsulta != "S")
                objSolicitud.AsignarUsuarioSEC(nroSEC, objUsuarioSession.idCuentaRed, objCliente.CLIEC_NUM_DOC, "E");

            // Consulta Datos Planes
            objLog.CrearArchivolog("    Inicio/ObtenerDetallePlanes/nroSEC   ", strNumeroSEC, null);
            dtDetallePlan = (new BLEvaluacion()).ObtenerDetallePlanes(nroSEC, 0);

            // Consulta Datos Creditos
            objLog.CrearArchivolog("    Inicio/ObtenerInformacionCrediticia/nroSEC   ", strNumeroSEC, null);
            dtDatosCreditos = objConsumer.ObtenerInformacionCrediticia(nroSEC);
            // Consulta Datos Monto Facturado, NO Facturado y LC Disponible
            objLog.CrearArchivolog("    Inicio/ObtenerInformacionBilletera/nroSEC   ", strNumeroSEC, null);
            dtDatosBilletera = objConsumer.ObtenerInformacionBilletera(nroSEC);
            // Consulta Datos Conteo de Líneas
            objLog.CrearArchivolog("    Inicio/ListarCantidadLineasActivas/nroSEC   ", strNumeroSEC, null);
            dtDatosLineas = objConsumer.ListarCantidadLineasActivas(nroSEC);
            // Consulta Datos Garantía
            objLog.CrearArchivolog("    Inicio/ObtenerInformacionGarantiaII/nroSEC   ", strNumeroSEC, null);
            dtDatosGarantia = objConsumer.ObtenerInformacionGarantiaII(nroSEC);
            // Consulta Datos Historico
            objLog.CrearArchivolog("    Inicio/ObtenerHistoricoEmpresa/TDOCC_CODIGO   ", objCliente.TDOCC_CODIGO.ToString(), null);
            objLog.CrearArchivolog("    Inicio/ObtenerHistoricoEmpresa/CLIEC_NUM_DOC   ", objCliente.CLIEC_NUM_DOC.ToString(), null);
            objListaHistorico = objSolicitud.ObtenerHistoricoEmpresa(0, objCliente.TDOCC_CODIGO, objCliente.CLIEC_NUM_DOC, Funciones.CheckDate(""), Funciones.CheckDate(""), "00");
            // Consulta Log Estado
            objLog.CrearArchivolog("    Inicio/ObtenerLogEstados/nroSEC   ", strNumeroSEC, null);
            objListaEstado = objSolicitud.ObtenerLogEstados(nroSEC);
            // Consulta Tipo Garantia
            objLog.CrearArchivolog("    Inicio/Consulta Tipo Garantia   ", null, null);
            objTipoGarantia = (new BLGeneral()).ListaTipoGarantia(string.Empty, "1");
            // Consulta Comentarios
            objLog.CrearArchivolog("    Inicio/ObtenerComentarioSEC   ", strNumeroSEC, null);
            objListaComentario = objSolicitud.ObtenerComentarioSEC(nroSEC, "01");

            //PROY-31948 INI
            objLog.CrearArchivolog("    PROY-31948 Inicio/IniciarControlCuota   ", strNumeroSEC, null);
            IniciarControlCuota(nroSEC);
            //PROY-31948 FIN

            // Datos de la SEC
            objLog.CrearArchivolog("    Inicio/IniciarControlGeneral   ", null, null);
            IniciarControlGeneral(objCliente);

            // Datos del Cliente
            objLog.CrearArchivolog("    Inicio/IniciarControlEmpresa   ", null, null);
            IniciarControlEmpresa(objCliente, dtDatosLineas);

            objLog.CrearArchivolog("    Inicio/ConsultarProteccionMovil   ", null, null);
            BuscarProteccionMovil(Funciones.CheckStr(nroSEC));

            string strCodGrupoParamProteccionMovil = Funciones.CheckStr(ConfigurationManager.AppSettings["consCodGrupoParamProteccionMovil"]);
            if (!string.IsNullOrEmpty(strCodGrupoParamProteccionMovil))
            {
                objLog.CrearArchivolog(string.Format("{0}{1}", "    Inicio/ListaParametrosGrupo strCodigo ", strCodGrupoParamProteccionMovil), null, null);
                lstBEParametroProteccionMovil = (new BLGeneral()).ListaParametrosGrupo(Funciones.CheckInt64(strCodGrupoParamProteccionMovil));
                objLog.CrearArchivolog(string.Format("{0}{1}", "    Fin/ListaParametrosGrupo lstBEParametroProteccionMovil.Count ", lstBEParametroProteccionMovil.Count), null, null);
                if (Session["ListaParametrosPM"] == null) Session["ListaParametrosPM"] = lstBEParametroProteccionMovil;
            }
            
            CargarParametrosEvaluacionProactiva();
            
            // Condiciones de Venta
            objLog.CrearArchivolog("    Inicio/IniciarControlCondiciones   ", null, null);
            IniciarControlCondiciones(objCliente, dtDetallePlan);

            // Información Crediticia
            objLog.CrearArchivolog("    Inicio/Información Crediticia   ", null, null);
            IniciarControlCrediticio(objCliente, dtDatosCreditos, dtDatosBilletera);

            // Información del Evaluador
            objLog.CrearArchivolog("    Inicio/Información del Evaluador   ", null, null);
            IniciarControlEvaluador(objCliente, dtDatosCreditos, dtDatosGarantia);

            // Información de Portabilidad
            objLog.CrearArchivolog("    Inicio/Información de Portabilidad   ", null, null);
            IniciarControlPortabilidad(objCliente);

            // Historico del Cliente
            dgHistorico.DataSource = objListaHistorico;
            dgHistorico.DataBind();

            // Log Estado del Cliente
            dgEstados.DataSource = objListaEstado;
            dgEstados.DataBind();

            // Historico Comentarios
            dgComentario.DataSource = objListaComentario;
            dgComentario.DataBind();

            // Log Estado del Cliente SGA
            IniciarHistoricoEstadoSOT(dtDetallePlan);

            objLog.CrearArchivolog("[SALIDA][Inicio]", null, null);
        }

        public void CargarParametrosEvaluacionProactiva() //EMMH - INICIO
        {
            string strCodGrupoParamEvaluacionProactiva = Funciones.CheckStr(ConfigurationManager.AppSettings["consCodGrupoParamEvaluacionProactiva"]);
            if (!string.IsNullOrEmpty(strCodGrupoParamEvaluacionProactiva))
                lstBEEvaluacionProactiva = (new BLGeneral()).ListaParametrosGrupo(Funciones.CheckInt64(strCodGrupoParamEvaluacionProactiva));
            if (lstBEEvaluacionProactiva.Count > 0) lstBEEvaluacionProactiva = lstBEEvaluacionProactiva.OrderBy(p => p.Valor1).ToList();
            if (Session["ListaParametrosEP"] == null) Session["ListaParametrosEP"] = lstBEEvaluacionProactiva;
            hidFlagPlanesProactivos.Value = lstBEEvaluacionProactiva.Where(p => p.Valor1 == "ConstFlagPlanesProactivos").SingleOrDefault().Valor;
        } //EMMH - FIN


        #region [Iniciar Controles]

        private void IniciarControlGeneral(BESolicitudEmpresa objCliente)
        {
            txtNroSEC.Text = Funciones.CheckStr(objCliente.SOLIN_CODIGO);
            txtCasoEspecial.Text = objCliente.TCESC_DESCRIPCION;
            txtCodClienteSAP.Text = objCliente.CLIEV_CODIGO_SAP;
            txtCanal.Text = objCliente.TOFIV_DESCRIPCION;
            txtTipoOperacion.Text = objCliente.TOPEV_DESCRIPCION;
            txtPuntoVenta.Text = objCliente.OVENV_DESCRIPCION;
            txtDistribuidorDes.Text = objCliente.DISTRIBUIDOR_DES;
            txtMotivo.Text = objCliente.CREDV_MOTIVO;
            txtObservacion.Text = objCliente.CREDV_OBS_FLEXIBILIZACION;
            txtEstadoVenta.Text = objCliente.ESTAV_DESCRIPCION;

            // Consulta Recibos
            hidListaRecibo.Value = "0";
            if (objCliente.FLAG_PORTABILIDAD == constFlgPortabilidad)
            {
                hidFlagPortabilidad.Value = "S";
                List<BEReciboDeposito> objListaRecibo = (new BLPortabilidad()).ListarRecibo(objCliente.SOLIN_CODIGO, 0, "");
                dgRecibo.DataSource = objListaRecibo;
                dgRecibo.DataBind();

                hidListaRecibo.Value = objListaRecibo.Count().ToString();
            }

            hidNroDoc.Value = objCliente.CLIEC_NUM_DOC;
        }

        private void IniciarControlEmpresa(BESolicitudEmpresa objCliente, DataTable dtDatosLineas)
        {
            //PROY-32438 INI
            Int64 nroSEC = Funciones.CheckInt64(Request.QueryString["nroSEC"]); // CODIGO_SEC 
            DataSet obj = new DataSet();

            obj = new BLEvaluacion().ObtenerDatosBRMS(nroSEC);

            string tipoCliente = Funciones.CheckStr(objCliente.SOLIC_EXI_BSC_FIN);
            if (tipoCliente == "0")
            {
                if (objCliente.CLASC_CODIGO == constClaseEmpresaTop)
                    txtTipoCliente.Text = "CLIENTE NUEVO TOP";
                else
                    txtTipoCliente.Text = "CLIENTE NUEVO";
            }
            else if (tipoCliente == "1")
            {
                if (objCliente.CLASC_CODIGO == constClaseEmpresaTop)
                    txtTipoCliente.Text = "CLIENTE CLARO TOP";
                else
                    txtTipoCliente.Text = "CLIENTE CLARO";
            }

            txtNroRUC.Text = objCliente.CLIEC_NUM_DOC;
            txtRazonSocial.Text = objCliente.CLIEV_RAZ_SOC;

            foreach (DataRow dr in obj.Tables[1].Rows)
            {

                txtTipoContribuyente.Text = dr["TIPCONTRIBUYENTE"].ToString();
                txtNombreContribuyente.Text = dr["NOMCOMERCIAL"].ToString();
                txtEstadoContribuyente.Text = dr["ESTCONTRIBUYENTE"].ToString();
                txtFechaActividades.Text = dr["FEC_INIACTIVIDADES"].ToString();
                txtCondicion.Text = dr["CONDCONTRIBUYENTE"].ToString();
                txtCIU.Text = dr["CIIUCONTRIBUYENTE"].ToString();
                txtCantTrabajadores.Text = dr["CANTTRABAJADORES"].ToString();
                txtComprobantes.Text = dr["EMISIONCOMP"].ToString();
                txtSistemaEmision.Text = dr["SIST_EMIELECTRONICA"].ToString();
            }
            //Fin 32438

            txtNroRRLL.Text = objCliente.REPRESENTANTE_LEGAL.Count.ToString();

            HttpContext.Current.Session["docCliente"] = objCliente.CLIEC_NUM_DOC;

            if (HttpContext.Current.Session["objCliente" + objCliente.CLIEC_NUM_DOC] == null)
            {
                BEClienteCuenta objClienteRUC = new BEClienteCuenta();
                objClienteRUC.tipoDoc = objCliente.TDOCC_CODIGO;
                objClienteRUC.nroDoc = objCliente.CLIEC_NUM_DOC;
                HttpContext.Current.Session["objCliente" + objCliente.CLIEC_NUM_DOC] = objClienteRUC;
            }

            objLog.CrearArchivolog("[Inicio][IniciarControlEmpresa]", null, null);

            objLog.CrearArchivolog("HttpContext.Current.Session[docCliente]=>" + Funciones.CheckStr(HttpContext.Current.Session["docCliente"]), null, null);

            BEClienteCuenta objClienteConsulta = new BEClienteCuenta();
            objClienteConsulta = (BEClienteCuenta)HttpContext.Current.Session["objCliente" + objCliente.CLIEC_NUM_DOC];
            objLog.CrearArchivolog("objClienteConsulta.tipoDoc =>" + Funciones.CheckStr(objClienteConsulta.tipoDoc), null, null);
            objLog.CrearArchivolog("objClienteConsulta.nroDoc =>" + Funciones.CheckStr(objClienteConsulta.nroDoc), null, null);

            hidTipoOperacion.Value = objCliente.TOPEN_CODIGO.ToString();

            dgNroLineaActivas.DataSource = dtDatosLineas;
            dgNroLineaActivas.DataBind();

            dgRepresentantes.DataSource = objCliente.REPRESENTANTE_LEGAL;
            dgRepresentantes.DataBind();

            // VMATOSSE - [PROY-32438 IDEA-41376 Nuevas Variables en Buro Corporativo y Evaluación BRMS] - INI
            if (objCliente.REPRESENTANTE_LEGAL != null)
            {
                objCliente.REPRESENTANTE_LEGAL.ForEach(rrll =>
                {
                    foreach (DataRow drRRLL in obj.Tables["CV_RRLL"].Rows)
                        if (rrll.APODV_NUM_DOC_REP.Equals(drRRLL["NUMERO_DOCUMENTO"].ToString().Replace("'", "")))
                        {
                            rrll.APODD_FECHA_NOMBRAMIENTO = drRRLL["FECHA_NOMBRAMIENTO_RRLL"].ToString();
                            rrll.APODI_CANT_MESES_NOMBRAMIENTO = drRRLL["MESES_NOMBRAMIENTO_RRLL"].ToString();
                        }
                });
            }
            // VMATOSSE - [PROY-32438 IDEA-41376 Nuevas Variables en Buro Corporativo y Evaluación BRMS] - FIN

            hidTipoOperacion.Value = objCliente.TOPEN_CODIGO.ToString();

            dgNroLineaActivas.DataSource = dtDatosLineas;
            dgNroLineaActivas.DataBind();

            dgRepresentantes.DataSource = objCliente.REPRESENTANTE_LEGAL;
            dgRepresentantes.DataBind();


        }

        private void IniciarControlCondiciones(BESolicitudEmpresa objCliente, DataTable dtDetallePlan)
        {
            objLog.CrearArchivolog("    IniciarControlCondiciones    ", null, null);
            txtOferta.Text = objCliente.TPROV_DESCRIPCION;
            txtCombo.Text = objCliente.COMBV_DESCRIPCION;

            if (!string.IsNullOrEmpty(txtCombo.Text)) hidFlgCombo.Value = "S";

            txtTelefonoRenovar.Text = objCliente.TELEFONO;
            txtPlanComercial.Text = objCliente.PLAN_ACTUAL;
            txtCFActual.Text = string.Format("{0:#,#,#,0.00}", objCliente.RENOF_CFACTUAL);

            foreach (DataRow dr in dtDetallePlan.Rows)
            {
                if (dr["ID_PRODUCTO"].ToString() == constTipoProductoDTH)
                {
                    hidFlgProductoDTH.Value = "S";
                }
                if ((dr["ID_PRODUCTO"].ToString() == constTipoProductoHFC) || (dr["ID_PRODUCTO"].ToString() == constTipoProducto3PlayInalam))
                {
                    hidFlgProductoHFC.Value = "S";
                }
                txtModalidadVenta.Text = dr["MODALIDAD_VENTA"].ToString();
            }
            objLog.CrearArchivolog("    IniciarControlCondiciones/SALIDA    ", null, null);
            dgPlanes.DataSource = dtDetallePlan;
            dgPlanes.DataBind();
        }

        private void IniciarControlCrediticio(BESolicitudEmpresa objCliente, DataTable dtDatosCreditos, DataTable dtDatosBilletera)
        {
            txtBuro.Text = objCliente.BURO_DESCRIPCION;
            txtRangoLC.Text = dtDatosCreditos.Rows[0]["CREDV_RANGO_LC"].ToString();
            txtRiesgoClaro.Text = objCliente.CLIEV_RIESGO_CLARO;
            txtRiesgoBuro.Text = objCliente.BURO_DESCRIPCION;

            txtRiesgoBuro.Text = objCliente.TIPO_RIESGO_DES.ToUpper();
            if (objCliente.TRIEC_CODIGO == "4")
            {
                txtRiesgoBuro.Text = "SIN HISTORIAL";
            }

            String tipoDocumento;
            String nroDocumento;


            tipoDocumento = Funciones.CheckStr(ConfigurationManager.AppSettings["TipoDocumentoRUC"]);//
            nroDocumento = objCliente.CLIEC_NUM_DOC;
            objLog.CrearArchivolog("    Ws/nroDocumento   ", nroDocumento, null);
            BWOAC osw = new BWOAC();
            int diasvencidos = 0;
            Claro.SISACT.WS.WSOAC.DetalleClienteType[] oDetalleCliente = osw.detalleClienteOAC(tipoDocumento, nroDocumento);
            if (oDetalleCliente[0].xCuentas.Length > 0)
            {
                diasvencidos = Funciones.CheckInt(oDetalleCliente[0].xAntiguedadDeuda);
            }


            txtDeudaFinanciera.Text = string.Format("{0:#,#,#,0.00}", objCliente.SOLIN_DEUDA_CLIENTE);
            txtLineaCredito.Text = string.Format("{0:#,#,#,0.00}", objCliente.SOLIN_LINEA_CREDITO_CALC);
            txtMontoVencido.Text = string.Format("{0:#,#,#,0.00}", objCliente.CLIEN_MONTO_VENCIDO);
            txtDiasVencimiento.Text = diasvencidos.ToString();
            objLog.CrearArchivolog("    Ws/Dias vencidos   ", diasvencidos.ToString(), null);

            txtLCDisponible.Text = String.Format("{0:#,#,#,0.00}", dtDatosCreditos.Rows[0]["CREDN_FACT_PROMEDIO"]);
            txtLcDisponibleClie.Text = String.Format("{0:#,#,#,0.00}", dtDatosCreditos.Rows[0]["CREDN_FACT_PROMEDIO"]);

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

        private void IniciarControlEvaluador(BESolicitudEmpresa objCliente, DataTable dtDatosCreditos, DataTable dtDatosGarantia)
        {
            objLog.CrearArchivolog("    IniciarControlEvaluador   ", null, null);

            txtResultadoEval.Value = dtDatosCreditos.Rows[0]["CREDV_MSJ_AUTONOMIA"].ToString().ToUpper();

            if (objCliente.CLIEC_FLAG_EXONERAR_RA == "SI") chkExoneracionRA.Checked = true;
            if (objCliente.RGLPC_PODERES == "1") chkPresentaPoderes.Checked = true;

            txtRiesgo.Text = txtRiesgoBuro.Text;
            txtLcDisponibleClie.Text = string.Format("{0:#,#,#,0.00}", txtLCDisponible.Text);
            txtComportamiento.Text = objCliente.CLIEV_COMPORTA_PAGO;

            visualizaImagenCP(Funciones.CheckInt(objCliente.CLIEV_CALIFICACION_PAGO));

            if (hidflgOrigenPagina.Value == "P" && hidFlgConsulta.Value != "S")
            {
                if (hidFlgProductoDTH.Value == "S")
                {
                    dgCostoInstalacion.DataSource = (new BLSolicitud()).ObtenerCostoInstalacion(Funciones.CheckInt64(txtNroSEC.Text));
                    dgCostoInstalacion.DataBind();
                }
                if (hidFlgProductoHFC.Value == "S")
                {
                    dgCostoInstalacionHFC.DataSource = (new BLSolicitud()).ObtenerCostoInstalacionHFC(Funciones.CheckInt64(txtNroSEC.Text));
                    dgCostoInstalacionHFC.DataBind();
                }
            }
            else
            {
                if (hidFlgProductoDTH.Value == "S")
                {
                    dgCostoInstalacion1.DataSource = (new BLSolicitud()).ObtenerCostoInstalacion(Funciones.CheckInt64(txtNroSEC.Text));
                    dgCostoInstalacion1.DataBind();
                }
                if (hidFlgProductoHFC.Value == "S")
                {
                    dgCostoInstalacionHFC1.DataSource = (new BLSolicitud()).ObtenerCostoInstalacionHFC(Funciones.CheckInt64(txtNroSEC.Text));
                    dgCostoInstalacionHFC1.DataBind();
                }
            }

            bool flgGarantiaConvergente = true;
            hidContFila.Value = dtDatosGarantia.Rows.Count.ToString();

            if (dtDatosGarantia.Rows.Count == 0)
            {
                flgGarantiaConvergente = false;
                objLog.CrearArchivolog("    IniciarControlEvaluador/ObtenerInformacionGarantia   ", null, null);
                dtDatosGarantia = new BLConsumer().ObtenerInformacionGarantia(Funciones.CheckInt64(txtNroSEC.Text));
            }

            hidFlgConvergente.Value = flgGarantiaConvergente ? "S" : "N";
            /*
                    gvResultadoCredito1.DataSource = dtDatosGarantia;
                    gvResultadoCredito1.DataBind();*/

            //PROY-31948_Migracion_2 ini 

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

            //PROY-31948_Migracion_2 fin

            txtComentarioAlPdv.Value = objCliente.SOLIV_COM_PUN_VEN;
            txtComentarioPdv.Value = dtDatosCreditos.Rows[0]["COMEV_COMENTARIO"].ToString();
            txtComentarioEvaluador.Value = objCliente.SOLIV_COM_EVALUADOR;

            /*  REVISANDO EL FLUJO CONCLUSIONES:
                PORTABILIDAD : LOS COMENTARIOS SE GUARDAN EN LA TABLA SECT_SOLICITUD [NO TIENE CHECKLIST]
                ALTA : LOS COMENTARIOS SE GUARDAN EN LA TABLA SISACT_EVALUACION MIENTRAS NO ES RECHAZADA, CUANDO SE RECHAZA SE GUARDAN EN LA SECT_SOLICITUD
            */
            if (objCliente.FLAG_PORTABILIDAD != constFlgPortabilidad)
            {
                BEEvaluacion objComentario = (new BLSolicitud()).ObtenerComentarioSEC(objCliente.SOLIN_CODIGO);
                if (objComentario != null)
                {
                    if (txtComentarioPdv.Value == string.Empty)
                    {
                        txtComentarioPdv.Value = objComentario.comentario_final_pdv;
                    }
                    if (txtComentarioEvaluador.Value == string.Empty)
                    {
                        txtComentarioEvaluador.Value = objComentario.comentario_final_ca;
                    }
                }
            }
            objLog.CrearArchivolog("    IniciarControlEvaluador/ObtenerArchivos   ", null, null);
            List<BEArchivo> objLista = (new BLSolicitud()).ObtenerArchivos(Funciones.CheckInt64(txtNroSEC.Text));
            objLog.CrearArchivolog("    IniciarControlEvaluador/SALIDA   ", null, null);
            dgArchivos.DataSource = objLista;
            dgArchivos.DataBind();
        }

        private void IniciarControlPortabilidad(BESolicitudEmpresa objCliente)
        {
            if (objCliente.FLAG_PORTABILIDAD == constFlgPortabilidad)
            {
                txtCedente.Text = ObtenerDescripcion("CO", objCliente.PORT_OPER_CED.ToString(), "1");
                txtModalidad.Text = ObtenerDescripcion("MO", objCliente.TLINC_CODIGO_CEDENTE, string.Empty);

                txtNroFormatoPort.Text = objCliente.PORT_SOLIN_NRO_FORMATO;
                txtCargoFijoCedente.Text = objCliente.PORT_CARGO_FIJO_OPE_CED;

                List<BEArchivo> objLista = (new BLPortabilidad()).ListarAchivosAdjunto(0, objCliente.SOLIN_CODIGO, string.Empty, "1");
                dgListaArchivos.DataSource = objLista;
                dgListaArchivos.DataBind();
            }
            CargarComentario();
        }

        private void CargarComentario()
        {
            List<BEComentario> lstComentario = new BLSolicitud().ObtenerComentarioSEC(Funciones.CheckInt64(txtNroSEC.Text), "01");

            dgComentarioPorta.DataSource = lstComentario;
            dgComentarioPorta.DataBind();
        }

        protected void btnComentario_Click(object sender, EventArgs e)
        {
            CargarComentario();
        }

        private void IniciarHistoricoEstadoSOT(DataTable dtDetallePlan)
        {
            List<BEEstado> objLista = new List<BEEstado>();
            List<BEEstado> objListaTmp = new List<BEEstado>();
            List<String> objListaSEC = new List<String>();
            Int64 nroSEC = Funciones.CheckInt64(txtNroSEC.Text);
            string strSEC = string.Empty;

            foreach (DataRow dr in dtDetallePlan.Rows)
            {
                strSEC = dr["SOLIN_CODIGO"].ToString();
                if (!objListaSEC.Contains(strSEC))
                {
                    if (dr["ID_PRODUCTO"].ToString() == constTipoProductoHFC || dr["ID_PRODUCTO"].ToString() == constTipoProductoDTH || dr["ID_PRODUCTO"].ToString() == constTipoProducto3PlayInalam)
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
            List<BEParametroPortabilidad> objLista = (new BLPortabilidad()).ListarParametroPortabilidad(prefijo, valor, ref1, string.Empty, string.Empty, 1);
            if (objLista != null && objLista.Count > 0)
            {
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

                string idEquipo = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["ID_EQUIPO"]);
                if (String.IsNullOrEmpty(idEquipo))
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
                    e.Row.Cells[12].Text = "0";//PROY-31948_Migracion
                }

                dblCFTotal += Funciones.CheckDbl(((DataRowView)e.Row.DataItem).Row["CF_TOTAL_MENSUAL"]);
                txtCFTotal.Text = string.Format("{0:#,#,#,0.00}", dblCFTotal);

                string strCodPlanSolicitud = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["SOPLN_CODIGO"]); //PROY-24724-IDEA-28174 - INICIO
                string strCodMaterial = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["ID_EQUIPO"]);
                string strConcatItemPopUp = string.Empty;
                string strDescMaterial = string.Empty;
                BLSincronizaSap objBLSincronizaSap = new BLSincronizaSap();
                DataTable dtDatosMaterial = new DataTable();
                List<BEParametro> lstParametrosPM = new List<BEParametro>();

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
                    dgPlanes.Columns[16].Visible = false; //PROY-24724-IDEA-28174 - FIN - //PROY-30166-IDEA–38863
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
                td.Text = "Resultado Creditos";
                dvItemHeader.Cells.Add(td);
                dgCostoInstalacion.Controls[0].Controls.AddAt(0, dvItemHeader);
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtCostoInstalacion = (TextBox)e.Row.FindControl("txtCostoInstalacion");

                txtCostoInstalacion.Text = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["COSTO_INSTAL"]);
                txtCostoInstalacion.Attributes.Add("onBlur", "calcularTotalCostoInstalacion()");
                dblTotalCostoInstalacion += Funciones.CheckDbl(txtCostoInstalacion.Text);
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
                td.Text = "Resultado Evaluación";
                dvItemHeader.Cells.Add(td);
                td = new TableCell();
                td.Text = "Resultado Creditos";
                dvItemHeader.Cells.Add(td);
                dgCostoInstalacion1.Controls[0].Controls.AddAt(0, dvItemHeader);
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                dblTotalCostoInstalacion += Funciones.CheckDbl(((DataRowView)e.Row.DataItem).Row["COSTO_INSTAL"]);
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
                td.Text = string.Empty;
                dvItemHeader.Cells.Add(td);
                td = new TableCell();
                td.Text = "Costo de Instalación";
                dvItemHeader.Cells.Add(td);
                td = new TableCell();
                td.Text = "Resultado Evaluación";
                dvItemHeader.Cells.Add(td);
                td = new TableCell();
                td.Text = "Resultado Creditos";
                dvItemHeader.Cells.Add(td);
                dgCostoInstalacionHFC.Controls[0].Controls.AddAt(0, dvItemHeader);
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtCostoInstalacion = (TextBox)e.Row.FindControl("txtCostoInstalacion");

                txtCostoInstalacion.Text = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["COSTO_INSTAL"]);
                txtCostoInstalacion.Attributes.Add("onBlur", "calcularTotalCostoInstalacion(1)");
                dblTotalCostoInstalacionHFC += Funciones.CheckDbl(txtCostoInstalacion.Text);
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
                td.Text = string.Empty;
                dvItemHeader.Cells.Add(td);
                td = new TableCell();
                td.Text = "Costo de Instalación";
                dvItemHeader.Cells.Add(td);
                td = new TableCell();
                td.Text = "Resultado Evaluación";
                dvItemHeader.Cells.Add(td);
                td = new TableCell();
                td.Text = "Resultado Creditos";
                dvItemHeader.Cells.Add(td);
                dgCostoInstalacionHFC1.Controls[0].Controls.AddAt(0, dvItemHeader);
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                dblTotalCostoInstalacionHFC += Funciones.CheckDbl(((DataRowView)e.Row.DataItem).Row["COSTO_INSTAL"]);

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
            objLog.CrearArchivolog("    Inicio/ObtenerDatosBRMS   ", Funciones.CheckStr(nroSEC), null);
            DataSet obj = (new BLEvaluacion()).ObtenerDatosBRMS(nroSEC);
            DataRow objCli;
            DataRow objPlanA;

            try
            {
                objCli = obj.Tables["CV_CLIENTE"].Rows[0];
                objPlanA = obj.Tables["CV_PLAN_ACTUAL"].Rows[0];

                txtTotalCuotasPendE.Text        = Funciones.CheckStr(objCli[20]) != string.Empty ? Funciones.CheckStr(objCli[20]) : "0";
                txtCantLineasCuotaE.Text        = Funciones.CheckStr(objCli[21]) != string.Empty ? Funciones.CheckStr(objCli[21]) : "0";
                txtCantMaxCuotasE.Text          = Funciones.CheckStr(objCli[22]) != string.Empty ? Funciones.CheckStr(objCli[22]) : "0";
                txtTotalImportCuotaUltE.Text    = Funciones.CheckStr(objCli[23]) != string.Empty ? Funciones.CheckStr(objCli[23]) : "0";
                txtCantTotalLineaCuotaUltE.Text = Funciones.CheckStr(objCli[24]) != string.Empty ? Funciones.CheckStr(objCli[24]) : "0";
                txtCantMaxCuotasGenUltE.Text    = Funciones.CheckStr(objCli[25]) != string.Empty ? Funciones.CheckStr(objCli[25]) : "0";
                txtCantCuotasPendLineaE.Text    = Funciones.CheckStr(objPlanA[5]) != string.Empty ? Funciones.CheckStr(objPlanA[5]) : "0";
                txtMontoPendCuotasLineaE.Text   = Funciones.CheckStr(objPlanA[6]) != string.Empty ? Funciones.CheckStr(objPlanA[6]) : "0";
                //PROY-140579 RU07 NN INI
                txtWhitelist.Text = Funciones.CheckStr(objCli[36]);
                txtModaCedenteLineaReno.Text = Funciones.CheckStr(objPlanA[7]);
                txtMontoCuotaActual.Text = Funciones.CheckStr(objPlanA[8]);
                txtActiLineaReno.Text = Funciones.CheckStr(objPlanA[9]);

                objLog.CrearArchivolog("txtWhitelist=", Funciones.CheckStr(objCli[36]), null);
                objLog.CrearArchivolog("txtModaCedenteLineaReno=", Funciones.CheckStr(objPlanA[7]), null);
                objLog.CrearArchivolog("txtMontoCuotaActual=", Funciones.CheckStr(objPlanA[8]), null);
                objLog.CrearArchivolog("txtActiLineaReno=", Funciones.CheckStr(objPlanA[9]), null);
                //PROY-140579 RU07 NN FIN
                objLog.CrearArchivolog("    FIN/ObtenerDatosBRMS   ", Funciones.CheckStr(nroSEC), null);

            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("    [ERROR OBTENER DATOS BRMS]", null, ex);
            }
        }
        //PROY-31948 FIN

        #endregion [Iniciar Controles]

        [System.Web.Services.WebMethod()]
        public static BEResponseWebMethod liberar(string nroSEC, string nroDocumento)
        {
            BEResponseWebMethod objResponse = new BEResponseWebMethod();
            GeneradorLog _objLog = CrearLogStatic(nroSEC);
            try
            {
                objResponse.TipoRespuesta = "B";
                objResponse.Boleano = (new BLSolicitud()).AsignarUsuarioSEC(Funciones.CheckInt64(nroSEC), string.Empty, nroDocumento, "E");
            }
            catch (Exception ex)
            {
                objResponse.Boleano = false;
                objResponse.DescripcionError = ex.Message;
                objResponse.Mensaje = Funciones.CheckStr(ConfigurationManager.AppSettings["consMsjErrorEvaluadorEmpresa"]);
                _objLog.CrearArchivolog("[ERROR METODO LIBERAR]", null, ex);
            }
            return objResponse;
        }

        [System.Web.Services.WebMethod()]
        public static BEResponseWebMethod grabarCreditos(string nroSEC, string flgConvergente, string strCadenaGarantia, string strCadenaCosto, string strUsuario)
        {
            BEResponseWebMethod objResponse = new BEResponseWebMethod();
            GeneradorLog _objLog = CrearLogStatic(nroSEC);
            try
            {
                List<string> objEvaluacion = new List<string>();
                objEvaluacion = new List<string>();
                objEvaluacion.Add(nroSEC);
                objEvaluacion.Add(flgConvergente);
                objEvaluacion.Add(Funciones.CheckStr(strCadenaGarantia));
                objEvaluacion.Add(Funciones.CheckStr(strCadenaCosto));
                objEvaluacion.Add(strUsuario);

                new BLSolicitud().ActualizarGarantia(objEvaluacion);

                objResponse.Mensaje = Funciones.CheckStr(ConfigurationManager.AppSettings["consMsjEvaluadorEmpresa"]);
            }
            catch (Exception ex)
            {
                objResponse.Boleano = false;
                objResponse.DescripcionError = ex.Message;
                objResponse.Mensaje = Funciones.CheckStr(ConfigurationManager.AppSettings["consMsjErrorGeneral"]);
                _objLog.CrearArchivolog("[ERROR METODO GRABAR_CREDITOS]", null, ex);
            }
            return objResponse;
        }

        private void grabar(string idProceso)
        {
            try
            {

                Int64 nroSEC = Funciones.CheckInt64(txtNroSEC.Text);

                BLSolicitud objSolicitud = new BLSolicitud();

                objLog.CrearArchivolog("[Inicio][grabar().EnviarMP]", null, null);
                objLog.CrearArchivolog("[Inicio][obtenerEstadoSolEmp().EnviarMP][nroSEC] " + Funciones.CheckStr(nroSEC), null, null);

                BESolicitudEmpresa objSolEmp = objSolicitud.obtenerEstadoSolEmp(nroSEC);

                objLog.CrearArchivolog("[Fin][obtenerEstadoSolEmp().EnviarMP][codigoSol] " + Funciones.CheckStr(objSolEmp.SOLIN_CODIGO), null, null);
                objLog.CrearArchivolog("[Fin][obtenerEstadoSolEmp().EnviarMP][estadoSol] " + objSolEmp.ESTAC_CODIGO, null, null);

                if (objSolEmp.ESTAC_CODIGO != Funciones.CheckStr(ConfigurationManager.AppSettings["constEstadoAPR"]))
                {

                    string tipoProductoMovil = Funciones.CheckStr(ConfigurationManager.AppSettings["constTipoProductoMovil"]);
                    string tipoProductoBAM = Funciones.CheckStr(ConfigurationManager.AppSettings["constTipoProductoBAM"]);                    
                    string comentarioPDV = Funciones.CheckStr(txtComentarioAlPdv.Value).ToUpper();
                    string comentarioEvaluador = Funciones.CheckStr(txtComentarioEvaluador.Value).ToUpper();
                    string comentario_sistema = string.Empty;
                    double nroRA = 0;
                    double totalGarantiaEvaluador = 0;

                    string mensajeCP = string.Empty;
                    string codigoCP = string.Empty;

                    string login = CurrentUser; //INI PROY-31948_Migracion                    
                    string loginAutorizador = hidUsuarioAutorizador.Value;
                    string rFlagProceso = string.Empty;
                    string rMsg = string.Empty;
                    double nroDGNuevo = 0;
                    string comentarioFinalPDV = Funciones.CheckStr(txtComentarioFinalPDV.Text).ToUpper();
                    string comentarioFinalCA = Funciones.CheckStr(txtComentarioFinalCA.Text).ToUpper();
                    double totalCFEvaluador = 0; //FIN PROY-31948_Migracion                    

                    string strTipoGarantia = string.Empty;
                    string[] arrGarantia = hidGarantia.Value.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string strGarantia in arrGarantia)
                    {
                        string[] strCampos = strGarantia.Split(';');
                        string strNroSec = strCampos[0];
                        if (!string.IsNullOrEmpty(strNroSec))
                        {
                            strTipoGarantia = strCampos[1];
                            nroRA = Funciones.CheckDbl(strCampos[2]);
                            totalGarantiaEvaluador = Funciones.CheckDbl(strCampos[3]);
                        }
                    }
                    //INI PROY-31948_Migracion
                    #region APROBAR
                    if (idProceso == "A")
                    {
                        guardarComentarios();
                        
                        objLog.CrearArchivolog(string.Format("{0}", "[Inicio][Metodo Aprobar SEC]"), null, null);
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[Metodo Aprobar SEC][SEC]", nroSEC), null, null);
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[Metodo Aprobar SEC][Usario Aprobador]", login), null, null);

                        BLSolicitud oSolicitud = new BLSolicitud();
                        BLMaestro objTiempoAtencion = new BLMaestro();
                        BESolicitudEmpresa oEvaluacion = new BESolicitudEmpresa();
                        oEvaluacion.SOLIN_CODIGO = nroSEC;
                        oEvaluacion.SOLIC_USU_CRE = login;

                        oSolicitud.AprobarCreditosReno(oEvaluacion);

                        objLog.CrearArchivolog(string.Format("{0}", "[Fin][Metodo Aprobar SEC]"), null, null);

                        objLog.CrearArchivolog(string.Format("{0}", "[Inicio][Metodo GrabarLogHistorico]"), null, null);
                        oSolicitud.GrabarLogHistorico(nroSEC, strEstado, login);
                        objLog.CrearArchivolog(string.Format("{0}", "[Fin][Metodo GrabarLogHistorico]"), null, null);

                        grabarTiempoPool(nroSEC, login);
                        RegistrarAuditoria(string.Empty, string.Empty);
                        
                        //PROY-31948_Migracion_2 INI
                        // Grabar Rentas / Costo Instalación
                        List<string> objEvaluacion = new List<string>();
                        objEvaluacion = new List<string>();
                        objEvaluacion.Add(Funciones.CheckStr(nroSEC));
                        objEvaluacion.Add(hidFlgConvergente.Value);
                        objEvaluacion.Add(Funciones.CheckStr(hidGarantia.Value));
                        objEvaluacion.Add(Funciones.CheckStr(hidCostoInstalacion.Value));
                        objEvaluacion.Add(login);

                        objLog.CrearArchivolog(string.Format("{0}", "[Inicio][ActualizarGarantia Aprobar_Reno]"), null, null);
                        new BLSolicitud().ActualizarGarantia(objEvaluacion);
                        objLog.CrearArchivolog(string.Format("{0}", "Fin][ActualizarGarantia Aprobar_Reno]"), null, null);
                        //PROY-31948_Migracion_2 FIN                                              

                        BLSolicitud.GrabarDatosEvaluadorCheckList(nroSEC, nroDGNuevo, nroRA, strEstadoAPR, strDesEstadoAPR, totalCFEvaluador, totalGarantiaEvaluador, comentarioPDV, comentarioEvaluador, comentario_sistema, login, loginAutorizador, ref rFlagProceso, ref rMsg);
                        
                        RegistrarAuditoria(Funciones.CheckStr(ConfigurationManager.AppSettings["CONS_COD_SISACT_ENV"]), String.Format("Se envió a activaciones la SEC número {0}", nroSEC));

                        hidnMensajeValue.Value = Funciones.CheckStr(ConfigurationManager.AppSettings["ConsMensajeAprobadoReno"]);

                    }
                    #endregion APROBAR

                    #region RECHAZAR
                    else if (idProceso == "R")
                    {
                        BLSolicitud oGrabar= new BLSolicitud();
                        BLGeneral oBLGeneral = new BLGeneral();
                        BLMaestro objTiempoAtencion = new BLMaestro();
                        bool resultado = false;
                        ArrayList correoUsuario;
                        string codUsuario = string.Empty;
                        string canal = string.Empty;
                        string Idcanal = string.Empty;
                        string idPtoVen = string.Empty;
                        Int64 idConsultor = 0, IdFlex = 0, idAutorizador = 0;                                     
                                                
                        string strCanalDirecto = Funciones.CheckStr(ConfigurationManager.AppSettings["Const_CodigoCanalDirecto"]);
                        string strRechazoActivo = Funciones.CheckStr(ConfigurationManager.AppSettings["CONS_COD_SISACT_REC_ACT"]);
                        string strMsjRechazarEvaluadorPersona = Funciones.CheckStr(ConfigurationManager.AppSettings["ConsMensajeRechazadoReno"]);

                        objLog.CrearArchivolog(string.Format("{0}", "[Inicio][Metodo Rechazar SEC]"), null, null);
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[Metodo Rechazar SEC][SEC]", nroSEC), null, null);
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[Metodo Rechazar SEC][Usario Rechazador]", login), null, null);
                         
                        resultado = BLSolicitud.GrabarRechazoSolicitud(nroSEC, login, string.Empty, string.Empty, strEstadoRZD, ref rFlagProceso, ref rFlagProceso); //(nroSEC, login, string.Empty, string.Empty, strEstadoRZD, ref rFlagProceso, ref rMsg);
                        objLog.CrearArchivolog(string.Format("{0}", "[Fin][Metodo Rechazar SEC]"), null, null);
                        if(resultado)
                        {
                            //actualiza comentarios en la SEC
                                objLog.CrearArchivolog(string.Format("{0}", "[Inicio][Metodo ActualizarComentarios]"), null, null);
                            BLSolicitud.ActualizarComentarios(nroSEC, comentarioFinalPDV, comentarioFinalCA);
                                objLog.CrearArchivolog(string.Format("{0}", "[Fin][Metodo ActualizarComentarios"), null, null);

                            //verificar si esta configurado para enviar mensaje
                                objLog.CrearArchivolog(string.Format("{0}", "[Inicio][Metodo ObtenerParamSolicitud]"), null, null);
                            resultado = BLSolicitud.ObtenerParamSolicitud(Funciones.CheckStr(nroSEC), ref Idcanal, ref idPtoVen, ref idConsultor, ref IdFlex, ref idAutorizador);
                                objLog.CrearArchivolog(string.Format("{0}", "[Fin][Metodo ObtenerParamSolicitud"), null, null);


                                //PROY-31948_Migracion_2 INI
                                // Grabar Rentas / Costo Instalación
                                List<string> objEvaluacion = new List<string>();
                                objEvaluacion = new List<string>();
                                objEvaluacion.Add(Funciones.CheckStr(nroSEC));
                                objEvaluacion.Add(hidFlgConvergente.Value);
                                objEvaluacion.Add(Funciones.CheckStr(hidGarantia.Value));
                                objEvaluacion.Add(Funciones.CheckStr(hidCostoInstalacion.Value));
                                objEvaluacion.Add(login);

                                objLog.CrearArchivolog(string.Format("{0}", "[Inicio][ActualizarGarantia Reno_Rechazo]"), null, null);
                                new BLSolicitud().ActualizarGarantia(objEvaluacion);
                                objLog.CrearArchivolog(string.Format("{0}", "Fin][ActualizarGarantia Reno_Rechazo]"), null, null);
                                //PROY-31948_Migracion_2 FIN  

                            if(resultado)
                            {
                                if(idConsultor > 0 || idAutorizador > 0)
                                {                                     
                                     List<BEParametro> arrParamMensajes = oBLGeneral.ListaParametrosGrupo(Funciones.CheckInt64(ConfigurationManager.AppSettings["constcodGrupoSMS"]));
                                     oBLGeneral = null;
                                
                                     sisact_SMS oSMS = new sisact_SMS();
                                    oSMS.EnvioSMS(nroSEC, idConsultor, idAutorizador, txtNroRUC.Text, txtRazonSocial.Text, string.Empty, string.Empty, string.Empty, Funciones.CheckInt64(strEstadoRZD), arrParamMensajes);
                                     oSMS = null;                            
                                    }
                                  }

                                    objLog.CrearArchivolog(string.Format("{0}", "[Inicio][Metodo obtenerUsuarioXSec]"), null, null);
                            codUsuario = BLSolicitud.obtenerUsuarioXSec(nroSEC, ref canal);
                            objLog.CrearArchivolog(string.Format("{0}{1}", "[Fin][Metodo obtenerUsuarioXSec][Canal]",canal), null, null);
                            objLog.CrearArchivolog(string.Format("{0}", "[Fin][Metodo obtenerUsuarioXSec]"), null, null);

                                    objLog.CrearArchivolog(string.Format("{0}", "[Inicio][Metodo obtenerDatoAuxiliarRepresentanteLegalD]"), null, null);
                            correoUsuario = BLSolicitud.obtenerDatoAuxiliarRepresentanteLegalD(nroSEC, codUsuario);
                            objLog.CrearArchivolog(string.Format("{0}", "[Fin][Metodo obtenerDatoAuxiliarRepresentanteLegalD][CorreoUsuario]", correoUsuario.Count), null, null);
                            objLog.CrearArchivolog(string.Format("{0}", "[Fin][Metodo obtenerDatoAuxiliarRepresentanteLegalD]"), null, null);                                

                            if (canal.Equals(strCanalDirecto) && correoUsuario.Count > 0 && (!string.IsNullOrEmpty(Funciones.CheckStr(correoUsuario[0]))))
                            {
                                                enviarMensajeConsultar("R", Funciones.CheckStr(correoUsuario[0]), nroSEC, strDesEstadoRZD, comentarioFinalPDV);
                                            }

                                    objLog.CrearArchivolog(string.Format("{0}", "[Inicio][Metodo RegistrarAuditoria]"), null, null);
                            RegistrarAuditoria(strRechazoActivo, String.Format("Se rechazó la SEC número {0}", nroSEC));
                                    objLog.CrearArchivolog(string.Format("{0}", "[Fin][Metodo RegistrarAuditoria"), null, null);
                                
                            grabarTiempoPool(nroSEC, login);
                
                            hidnMensajeValue.Value = strMsjRechazarEvaluadorPersona;               
                            }                   
                         }
                    #endregion RECHAZAR

                    #region CANCELAR
                    else if (idProceso == "C")
                    {
                        string GuardarAlCancelar = hdnGuardarAlCancelar.Value;
                        
                        BLSolicitud objActivacion = new BLSolicitud();
                            {
                                if ((GuardarAlCancelar == "SI"))
                                {
                                guardarComentarios();

                                    objLog.CrearArchivolog(string.Format("{0}", "[Inicio][Metodo RegistrarAuditoria]"), null, null);
                                RegistrarAuditoria(Funciones.CheckStr(ConfigurationManager.AppSettings["CONS_COD_SISACT_GC"]), string.Format("Se guardaron los cambios de la SEC número {0}", txtNroSEC.Text));
                                    objLog.CrearArchivolog(string.Format("{0}", "[Fin] Metodo RegistrarAuditoria"), null, null);
                                }
                                else
                                {
                                    objLog.CrearArchivolog(string.Format("{0}", "[Inicio][Metodo AsignarUsuarioSEC]"), null, null);
                                    bool rsptAsignarDespch = objActivacion.AsignarUsuarioSEC(Funciones.CheckInt64(nroSEC), login, txtNroRUC.Text, "E");
                                    objLog.CrearArchivolog(string.Format("{0}", "[Fin] Metodo AsignarUsuarioSEC"), null, null);

                                }
                                hidnMensajeValue.Value = Funciones.CheckStr(ConfigurationManager.AppSettings["ConsthidMensaje_CANCELADO"]);                            
                        }
                    }
                    #endregion CANCELAR

                    //FIN PROY-31948_Migracion

                    hidProceso.Value = "OK";
                }
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog(String.Format("{0} {1}", "[Fin][Grabar().EnviarMP][codigoError] ", ex.Message), null, null);
                hidnMensajeValue.Value = Funciones.CheckStr(ConfigurationManager.AppSettings["consMsjErrorEvaluadorEmpresa"]);                
            }
        }

        //Inicio IDEA-30067
        private void PortaConsultaPreviaAutomatico()
        {
            objLog.CrearArchivolog("[Inicio][PortaConsultaPreviaAutomatico]", null, null);

            string constProductoPortAuto = Funciones.CheckStr(ConfigurationManager.AppSettings["constProductoPortAuto"]);

            bool blnPortabilidad = (hidFlagPortabilidad.Value == "S");
            bool blnProductoPortAuto = (constProductoPortAuto.IndexOf(hidProductoPortAuto.Value) >= 0);

            objLog.CrearArchivolog("[blnPortabilidad]", blnPortabilidad.ToString(), null);
            objLog.CrearArchivolog("[blnProductoPortAuto]", blnProductoPortAuto.ToString(), null);

            if (blnPortabilidad && blnProductoPortAuto)
            {
                BWEnvioPorta objEnvioPortal = new BWEnvioPorta();
                BEItemMensaje objMensaje = new BEItemMensaje(false);
                string numeroSec = txtNroSEC.Text;
                string observaciones = Funciones.CheckStr(ConfigurationManager.AppSettings["consObservacionEnvioPorta"]);

                objLog.CrearArchivolog("[Cambio Portabilidad]", null, null);
                objLog.CrearArchivolog("[numeroSec]", numeroSec.ToString(), null);
                objLog.CrearArchivolog("[observaciones]", observaciones.ToString(), null);

                BEItemGenerico objAudit = new BEItemGenerico();
                objAudit.Codigo = String.Format("{0}_{1}", txtNroSEC.Text, DateTime.Now.ToString("yyyyMMddhhmmss"));
                objAudit.Codigo2 = CurrentUser;
                objAudit.Codigo3 = Funciones.CheckStr(ConfigurationManager.AppSettings["CodigoAplicacion"]);
                objAudit.Descripcion = Funciones.CheckStr(ConfigurationManager.AppSettings["ConstSistemaConsumer"]);
                objAudit.Descripcion2 = CurrentTerminal;

                objMensaje = objEnvioPortal.realizarConsultaPrevia(numeroSec, observaciones, objAudit);

                objLog.CrearArchivolog("[objMensaje.exito]", Funciones.CheckStr(objMensaje.exito), null);
                objLog.CrearArchivolog("[objMensaje.codigo]", Funciones.CheckStr(objMensaje.codigo), null);
                objLog.CrearArchivolog("[objMensaje.descripcion]", Funciones.CheckStr(objMensaje.descripcion), null);

            }
            else
            {
                objLog.CrearArchivolog("[Sin Cambio Portabilidad]", null, null);
            }

            objLog.CrearArchivolog("[Fin][PortaConsultaPreviaAutomatico]", null, null);
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
                    lstObjPrima = objProteccionMovil.BuscarProteccionMovilPvu(string.Empty, strNroSec, string.Empty, ref strCodRespuesta, ref strMsjRespuesta);
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

       
        //INI PROY-31948_Migracion
        private void enviarMensajeConsultar(string proceso, string correoConsultor, Int64 solin_codigo, string estadoDes, string comentario)
        {
            BLSolicitud oSolicitud = new BLSolicitud();
            BESolicitudEmpresa item = new BESolicitudEmpresa();

            objLog.CrearArchivolog(string.Format("{0}", "[Inicio][Metodo ObtenerSolicitudEmpresa]"), null, null);
            item = oSolicitud.ObtenerSolicitudEmpresa(Funciones.CheckInt64(txtNroSEC));
            objLog.CrearArchivolog(string.Format("{0}", "[Fin] Metodo ObtenerSolicitudEmpresa"), null, null);
                 
            string remitente = Funciones.CheckStr(ConfigurationManager.AppSettings["strEmailSisact"]);

            string asunto = string.Format("{0}{1}{2}{3}{4}{5}", "SEC:", Funciones.CheckStr(solin_codigo), " - ESTADO:", estadoDes, " - RAZON SOCIAL: ", item.CLIEV_RAZ_SOC);
            
            if (proceso == "R" || proceso == "O")
            {
                Funciones.EnviarEmail(remitente, correoConsultor, string.Empty, string.Empty, asunto, comentario, string.Empty); 
            }
        }
        
        public void RegistrarAuditoria(string strCodServ, string strDesTrans)
        {
            string nombreHost = Dns.GetHostName();
            string nombreServer = Dns.GetHostName();
            String ipServer = Dns.GetHostAddresses(nombreServer)[0].ToString();

            string usuario_id = CurrentUsers;
            string ipCliente = CurrentTerminal;

            string strCodAplica = Funciones.CheckStr(ConfigurationManager.AppSettings["CodigoAplicacion"]);
            string strNomAplica = Funciones.CheckStr(ConfigurationManager.AppSettings["constNombreAplicacion"]);
            string strCodigoSACTEVALS = Funciones.CheckStr(ConfigurationManager.AppSettings["CONS_COD_SACT_EVALS"]);

            if (string.IsNullOrEmpty(strDesTrans)) string.Format("{0}{1}{2}{3}{4}", "Aceptacion o Rechazo de la SEC|", strCodAplica, "|", strNomAplica, "|9206|POOL DE EVALUACION|");
            if (string.IsNullOrEmpty(strCodServ)) Funciones.CheckStr(ConfigurationManager.AppSettings["CONS_COD_SACT_SERV_RENO"]);

            string strTransaccion = strCodigoSACTEVALS;

            bool flag = false;

            flag = (new BWAuditoria()).registrarAuditoria(strTransaccion, strCodServ, ipCliente, nombreHost, ipServer, nombreServer, usuario_id, "", "0", strDesTrans);
        }
        
        private void grabarTiempoPool(Int64 strNroSec, string strLogin)
        {
                string fechaFin = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                objLog.CrearArchivolog(string.Format("{0}", "[Inicio][Metodo GrabarRegHorasPoll]"), null, null);
            BLMaestro.GrabarRegHorasPoll(strNroSec, strLogin, fechaFin, "EVALUACION");
                objLog.CrearArchivolog(string.Format("{0}", "[Fin][Metodo GrabarRegHorasPoll]"), null, null);
            }
 
        public void guardarComentarios()
         {
            BEEvaluacion objEva = new BEEvaluacion();

            objEva.flag_evaluacion = "1";
            objEva.adjuntar_voucher = false;
            objEva.nueva_propuesta = false;
            objEva.cod_analista = CurrentUser;
            objEva.cod_sec = Funciones.CheckInt(txtNroSEC.Text.ToString());

            objEva.correcion_comentario_ca = string.Empty;
            objEva.adjuntar_comentario_ca = string.Empty;
            objEva.propuesta_comentario_ca = string.Empty;
            objEva.comentario_final_ca = txtComentarioFinalCA.Text.ToString().Trim();
            objEva.comentario_final_pdv = txtComentarioFinalPDV.Text.ToString().Trim();
            objEva.comentario_final_credito = string.Empty;
            objEva.existe_rechazo = false;

            try
                 {
                objLog.CrearArchivolog(string.Format("{0}", "[Inicio][Metodo grabarComentarios]"), null, null);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[Metodo grabarComentarios][Flag Evaluacion] ", objEva.flag_evaluacion), null, null);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[Metodo grabarComentarios][Cod Analista] ", objEva.cod_analista), null, null);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[Metodo grabarComentarios][Cod Sec] ", objEva.cod_sec), null, null);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[Metodo grabarComentarios][Comentario Final CA] ", objEva.comentario_final_ca), null, null);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[Metodo grabarComentarios][Comentario Final PDV] ", objEva.comentario_final_pdv), null, null);

                BLSolicitud.grabarComentarios(objEva);
                grabarTiempoPool(Funciones.CheckInt64(objEva.cod_sec), objEva.cod_analista);
             
                objLog.CrearArchivolog(string.Format("{0}", "[Fin][Metodo grabarComentarios]"), null, null);
         }
            catch (Exception ex)
        {
                objLog.CrearArchivolog(string.Format("{0}{1}", "[Metodo grabarComentarios][Ocurrio un error al grabar] ", Funciones.CheckStr(ex)), null, null);
            }
            finally
            {
                objEva = null;
        }        
    }
        
              protected string FlagReingresoDes(object valor) //obs6
        {
            string v = Funciones.CheckStr(valor);
            string salida = "NO";
            if (v == "1")
                salida = "SI";
            return salida;
        }
                
        [System.Web.Services.WebMethod()]
        public static BEResponseWebMethod consultaOperacion(string nroSEC)
        {
            BEResponseWebMethod objResponse = new BEResponseWebMethod();
            BLSolicitud objSolicitud = new BLSolicitud();
            BESolicitudEmpresa objCliente = new BESolicitudEmpresa();            

            try
            {
                objCliente = objSolicitud.ObtenerSolicitudEmpresa(Funciones.CheckInt64(nroSEC));
                objResponse.Tipo = Funciones.CheckStr(objCliente.TOPEN_CODIGO);
                objResponse.Cadena = string.Format("{0}|{1}", objCliente.SOLIN_CODIGO, objCliente.PRDC_CODIGO);

                objResponse.Error = false;
            }
            catch
            {   
                objResponse.Error = true;
            }
            return objResponse;
        }

        //FIN PROY-31948_Migracion
    }
}
