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
//INICIO PROY-140546
using Claro.SISACT.Business.RestReferences;
using PAGOANTICIPADO_ENTITY = Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest;
using Claro.SISACT.Entity.DataPowerRest;
using Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest.DataPower.RegistraHistorial;
using Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest.DataPower.ConsultaHistorico.Request;
using Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest.DataPower.ConsultaHistorico.Response;
using Claro.SISACT.WS.RestReferences;
//FIN PROY-140546

namespace Claro.SISACT.Web.Paginas.evaluacion_cons
{
    public partial class sisact_reporte_evaluacion_empresa : Sisact_Webbase
    {
        GeneradorLog objLog = new GeneradorLog("    sisact_reporte_evaluacion_empresa  ", null, null, "WEB");
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

        // emergencia-29215-INICIO
        string constTipoProductoFTTH = ConfigurationManager.AppSettings["constTipoProductoFTTH"].ToString();
        //  emergencia-29215-FIN

        string constTipoProducto3PlayInalam = ConfigurationManager.AppSettings["constTipoProducto3PlayInalam"];
        //Inicio IDEA-30067
        string constFlagPortaAutomatico = ConfigurationManager.AppSettings["constFlagPortaAutomatico"].ToString();
        //Fin IDEA-30067

        //PROY-26963 - IDEA-34399: Mejora Portabilidad Despacho PDV
        bool flagActualizarCP = false;
        BESolicitudPersona datos = new BESolicitudPersona();
        //PROY-26963 - IDEA-34399: Mejora Portabilidad Despacho PDV

        GeneradorLog _objLog = null;

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
        List<BEParametro> lstBEEvaluacionProactiva = new List<BEParametro>();//Proy-30748
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
                //INC000003062381  - INICIO
                Int64 nroSEC = Funciones.CheckInt64(HidSecValidacion.Value);

                BLSolicitud objSolicitud = new BLSolicitud();
                BESolicitudEmpresa objCliente = objSolicitud.ObtenerSolicitudEmpresa(nroSEC);
                string descripcionSec = Funciones.CheckStr(objCliente.ESTAV_DESCRIPCION);

                objLog.CrearArchivolog("INC000003062381  - nroSEC", Funciones.CheckStr(nroSEC), null);
                objLog.CrearArchivolog("INC000003062381  - descripcionSec", descripcionSec, null);
                objLog.CrearArchivolog("INC000003062381  - objCliente.ESTAC_CODIGO", Funciones.CheckStr(objCliente.ESTAC_CODIGO), null);


                if (!string.IsNullOrEmpty(objCliente.ESTAC_CODIGO) && (ReadKeySettings.Key_EstadoPendienteAprobacion).IndexOf(objCliente.ESTAC_CODIGO) > -1)
                {

                    string accion = hidProceso.Value;
                    hidProceso.Value = string.Empty;
                    objLog.CrearArchivolog("INC000003062381  - Accion", Funciones.CheckStr(accion), null);
                    switch (accion)
                    {
                        case "A":
                        case "O":
                        case "S":
                        case "R":
                            grabar(accion);
                            break;
                    }
                }
                else
                {
                    string mensajeEstado = string.Format(Funciones.CheckStr(ReadKeySettings.Key_MensajeDeEstadoSec), Funciones.CheckStr(nroSEC), descripcionSec);
                    objLog.CrearArchivolog("INC000003062381  - mensajeEstado", Funciones.CheckStr(mensajeEstado), null);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ERROR", "alert('" + mensajeEstado + "');", true);
                }
                //INC000003062381  - FIN
            }

            //PROY-140439 BRMS CAMPAÑA NAVIDEÑA::INI
            hidFlagBRMSCamp.Value = Funciones.CheckStr(ReadKeySettings.Key_flagBRMSCamp);
            //PROY-140439 BRMS CAMPAÑA NAVIDEÑA::FIN
        }

        private void Inicio()
        {
            string flgSoloConsulta = Request.QueryString["flgSoloConsulta"];
            string flgOrigenPagina = Request.QueryString["flgOrigenPagina"]; //"P":CREDITOS/"C":CONSULTA SEC
            Int64 nroSEC = Funciones.CheckInt64(Request.QueryString["nroSEC"]);
            //string codProd = Request.QueryString["strIdProd"];

            HidSecValidacion.Value = Funciones.CheckStr(nroSEC); //INC000003062381 

            objLog.CrearArchivolog("    Inicio/flgSoloConsulta   ", flgSoloConsulta.ToString(), null);
            objLog.CrearArchivolog("    Inicio/flgOrigenPagina   ", flgOrigenPagina.ToString(), null);
            objLog.CrearArchivolog("    Inicio/nroSEC   ", nroSEC.ToString(), null);

            BEUsuarioSession objUsuarioSession = (BEUsuarioSession)Session["Usuario"];

            BLConsumer objConsumer = new BLConsumer();
            BLSolicitud objSolicitud = new BLSolicitud();
            DataTable dtDatosCreditos, dtDatosBilletera, dtDatosLineas, dtDatosGarantia, dtDetallePlan;

            List<BEEstado> objListaEstado = new List<BEEstado>();
            List<BESolicitudEmpresa> objListaHistorico = new List<BESolicitudEmpresa>();
            List<BEComentario> objListaComentario = new List<BEComentario>();

            // Creación Log Aplicación
            _objLog = CrearLog(nroSEC.ToString());

            // Consulta Datos Solicitud
            BESolicitudEmpresa objCliente = objSolicitud.ObtenerSolicitudEmpresa(nroSEC);


            hidEstadoPa.Value = "";//PROY-140546 PA

            //Inicio IDEA-30067
            hidProductoPortAuto.Value = objCliente.PRDC_CODIGO;
            objLog.CrearArchivolog("    Inicio/codProd  ", Funciones.CheckStr(objCliente.PRDC_CODIGO), null);
            //Fin IDEA-30067
            hidDeudaClienteEmpresa.Value = objCliente.SOLIC_DEUDA_CLIENTE; //PROY-29121
            // Asignar Usuario
            hidTiempoInicio.Value = DateTime.Now.ToString();
            hidFlgConsulta.Value = flgSoloConsulta;
            hidflgOrigenPagina.Value = flgOrigenPagina;
            hidListaPerfiles.Value = objUsuarioSession.CadenaOpcionesPagina;
            hidUsuario.Value = CurrentUser;

            if (flgOrigenPagina == "P" && flgSoloConsulta != "S")
                objSolicitud.AsignarUsuarioSEC(nroSEC, objUsuarioSession.idCuentaRed, objCliente.CLIEC_NUM_DOC, "E");

            hidCanalSEC.Value = objCliente.TOFIC_CODIGO;//PROY-140546

            // Consulta Datos Planes
            objLog.CrearArchivolog("    Inicio/ObtenerDetallePlanes/nroSEC   ", nroSEC.ToString(), null);
            dtDetallePlan = (new BLEvaluacion()).ObtenerDetallePlanes(nroSEC, 0);
            objCliente.ID_MODALIDAD_VENTA = dtDetallePlan.Rows[0]["ID_MODALIDAD_VENTA"].ToString(); //PROY-140223 IDEA-140462
            //PROY-26963 - IDEA-34399: Mejora Portabilidad Despacho PDV
            HttpContext.Current.Session["SolicitudEmpresa"] = objCliente;
            //PROY-26963 - IDEA-34399: Mejora Portabilidad Despacho PDV
            // Consulta Datos Creditos
            objLog.CrearArchivolog("    Inicio/ObtenerInformacionCrediticia/nroSEC   ", nroSEC.ToString(), null);
            dtDatosCreditos = objConsumer.ObtenerInformacionCrediticia(nroSEC);
            // Consulta Datos Monto Facturado, NO Facturado y LC Disponible
            objLog.CrearArchivolog("    Inicio/ObtenerInformacionBilletera/nroSEC   ", nroSEC.ToString(), null);
            dtDatosBilletera = objConsumer.ObtenerInformacionBilletera(nroSEC);
            // Consulta Datos Conteo de Líneas
            objLog.CrearArchivolog("    Inicio/ListarCantidadLineasActivas/nroSEC   ", nroSEC.ToString(), null);
            dtDatosLineas = objConsumer.ListarCantidadLineasActivas(nroSEC);
            // Consulta Datos Garantía
            objLog.CrearArchivolog("    Inicio/ObtenerInformacionGarantiaII/nroSEC   ", nroSEC.ToString(), null);
            dtDatosGarantia = objConsumer.ObtenerInformacionGarantiaII(nroSEC);
            // Consulta Datos Historico
            objLog.CrearArchivolog("    Inicio/ObtenerHistoricoEmpresa/TDOCC_CODIGO   ", objCliente.TDOCC_CODIGO.ToString(), null);
            objLog.CrearArchivolog("    Inicio/ObtenerHistoricoEmpresa/CLIEC_NUM_DOC   ", objCliente.CLIEC_NUM_DOC.ToString(), null);
            objListaHistorico = objSolicitud.ObtenerHistoricoEmpresa(0, objCliente.TDOCC_CODIGO, objCliente.CLIEC_NUM_DOC, Funciones.CheckDate(""), Funciones.CheckDate(""), "00");
            // Consulta Log Estado
            objLog.CrearArchivolog("    Inicio/ObtenerLogEstados/nroSEC   ", nroSEC.ToString(), null);
            objListaEstado = objSolicitud.ObtenerLogEstados(nroSEC);
            // Consulta Tipo Garantia
            objLog.CrearArchivolog("    Inicio/Consulta Tipo Garantia   ", null, null);
            objTipoGarantia = (new BLGeneral()).ListaTipoGarantia(string.Empty, "1");
            // Consulta Comentarios
            objLog.CrearArchivolog("    Inicio/ObtenerComentarioSEC   ", nroSEC.ToString(), null);
            objListaComentario = objSolicitud.ObtenerComentarioSEC(nroSEC, "01");

            //PROY-31948 INI
            objLog.CrearArchivolog("    PROY-31948 Inicio/IniciarControlCuota   ", nroSEC.ToString(), null);
            strTipoOperacion = Funciones.CheckStr(objCliente.TOPEN_CODIGO); //PROY-140743
            IniciarControlCuota(nroSEC);
            //PROY-31948 FIN

            // Datos de la SEC
            objLog.CrearArchivolog("    Inicio/IniciarControlGeneral   ", null, null);
            IniciarControlGeneral(objCliente);

            // Datos del Cliente
            objLog.CrearArchivolog("    Inicio/IniciarControlEmpresa   ", null, null); //PROY-24724-IDEA-28174
            IniciarControlEmpresa(objCliente, dtDatosLineas);

            objLog.CrearArchivolog("    Inicio/ConsultarProteccionMovil   ", null, null); //PROY-24724-IDEA-28174 - INICIO
            BuscarProteccionMovil(Funciones.CheckStr(nroSEC));


            //PROY-29215 INICIO
            string TipoProducto = objCliente.PRDC_CODIGO;
            // EMERGENCIA-29215-INICIO
            if (TipoProducto == constTipoProductoDTH || TipoProducto == constTipoProductoHFC || TipoProducto == constTipoProducto3PlayInalam || TipoProducto == constTipoProductoFTTH)
            // EMERGENCIA-29215-FIN 
            {
                hidAsesor.Value = Funciones.CheckStr(CurrentUserSession.Nombre + " " + CurrentUserSession.Apellido_Pat + " " + CurrentUserSession.Apellido_Mat);

                objLog.CrearArchivolog("[ListarParametroGeneral-FormaPago]", null, null);
                objFormaPago = (new BLGeneral()).ListarParametroGeneral(Funciones.CheckStr(ConfigurationManager.AppSettings["strCodigoFormaPago"]));

                objLog.CrearArchivolog("[ListarParametroGeneral-Cuotas]", null, null);
                objCuotas = (new BLGeneral()).ListarParametroGeneral(Funciones.CheckStr(ConfigurationManager.AppSettings["strCodigoCuota"]));

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
            ///PROY-29215 FIN

            string strCodGrupoParamProteccionMovil = Funciones.CheckStr(ConfigurationManager.AppSettings["consCodGrupoParamProteccionMovil"]);
            if (!string.IsNullOrEmpty(strCodGrupoParamProteccionMovil))
            {
                objLog.CrearArchivolog(string.Format("{0}{1}", "    Inicio/ListaParametrosGrupo strCodigo ", strCodGrupoParamProteccionMovil), null, null);
                lstBEParametroProteccionMovil = (new BLGeneral()).ListaParametrosGrupo(Funciones.CheckInt64(strCodGrupoParamProteccionMovil));
                objLog.CrearArchivolog(string.Format("{0}{1}", "    Fin/ListaParametrosGrupo lstBEParametroProteccionMovil.Count ", lstBEParametroProteccionMovil.Count), null, null);
                if (Session["ListaParametrosPM"] == null) Session["ListaParametrosPM"] = lstBEParametroProteccionMovil;
            } //PROY-24724-IDEA-28174 - FIN
            //EMMH I
            CargarParametrosEvaluacionProactiva();
            //EMMH F
            // Condiciones de Venta
            objLog.CrearArchivolog("    Inicio/IniciarControlCondiciones   ", null, null);
            IniciarControlCondiciones(objCliente, dtDetallePlan, nroSEC);//PROY-140743

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
            txtNroSEC.Text = objCliente.SOLIN_CODIGO.ToString();
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

            string strModventa = objCliente.ID_MODALIDAD_VENTA;   // Proy-140360
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

            //PROY_33313 INICIO 
            nroSEC = Funciones.CheckInt64(Request.QueryString["nroSEC"]);

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
            //PROY_140360   inicio
            new BLGeneral_II().ListarEstadoFlaj2(nroSEC, out P_RESULTADO1, out P_NRO_RESULTADO1, out P_DES_RESULTADO1);
            List<BEParametro> Key_ParanGroupContrato = (new BLGeneral()).ListaParametrosGrupo(Int64.Parse(ConfigurationManager.AppSettings["valorParanGroupContratoCode"].ToString()));

            string strContratoDescrip = Key_ParanGroupContrato.Where(p => p.Descripcion == "Key_Message_Pack").SingleOrDefault().Valor;
            string strFlagCAC1 = Key_ParanGroupContrato.Where(p => p.Descripcion == "Key_Flag_CAC").SingleOrDefault().Valor1;
            string strFlagDAC1 = Key_ParanGroupContrato.Where(p => p.Descripcion == "Key_Flag_DAC").SingleOrDefault().Valor1;
            bool blnEsContratoCode = false;
            if (((txtCanal.Text == "CAC" && strFlagCAC1 == "1") || (txtCanal.Text == "DAC" && strFlagDAC1 == "1")) && strModventa == "2") // Recordar probar ruc
            {
                if (P_NRO_RESULTADO1 == "0")
                {
                    switch (P_RESULTADO1)
                    {
                        case "1":
                            objLog.CrearArchivolog("    PROY-140360 LblMsjChipCuota   ", LblMsjChipCuota.Text.ToString(), null);
                            objLog.CrearArchivolog("    PROY-140360 LblMsjYN   ", LblMsjYN.Text.ToString(), null);
                            LblMsjChipCuota.Text = strContratoDescrip.ToString();
                            LblMsjYN.Text = "SI";
                            break;
                        case "0":
                            objLog.CrearArchivolog("    PROY-140360 LblMsjChipCuota   ", LblMsjChipCuota.Text.ToString(), null);
                            objLog.CrearArchivolog("    PROY-140360 LblMsjYN   ", LblMsjYN.Text.ToString(), null);
                            LblMsjChipCuota.Text = strContratoDescrip.ToString();
                            LblMsjYN.Text = "NO";
                            break;
                        default:
                            objLog.CrearArchivolog("    PROY-140360 LblMsjChipCuota   ", LblMsjChipCuota.Text.ToString(), null);
                            objLog.CrearArchivolog("    PROY-140360 LblMsjYN   ", LblMsjYN.Text.ToString(), null);
                            LblMsjChipCuota.Text = "";
                            LblMsjYN.Text = "";
                            break;
                    }
                    blnEsContratoCode = true;
                }
                else
                {
                    objLog.CrearArchivolog("    PROY-XXX Error al obtener los datos parametricos   ", P_RESULTADO1.ToString(), null);
                    objLog.CrearArchivolog("    PROY-XXX P_RESULTADO1   ", P_RESULTADO1.ToString(), null);
                    objLog.CrearArchivolog("    PROY-XXX P_NRO_RESULTADO1   ", P_NRO_RESULTADO1.ToString(), null);
                    objLog.CrearArchivolog("    PROY-XXX P_DES_RESULTADO1   ", P_DES_RESULTADO1.ToString(), null);
                    LblMsjChipCuota.Text = P_DES_RESULTADO1.ToString();
                    LblMsjYN.Text = P_NRO_RESULTADO1.ToString();
                }

            }
            if (!blnEsContratoCode)
            {//PROY_140360  Fin

                objLog.CrearArchivolog("    PROY-33313-INICIO  ", null, null);
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
                        objLog.CrearArchivolog("    PROY-33313 P_RESULTADO   ", P_RESULTADO.ToString(), null);
                        objLog.CrearArchivolog("    PROY-33313 P_NRO_RESULTADO   ", P_NRO_RESULTADO.ToString(), null);
                        objLog.CrearArchivolog("    PROY-33313 P_DES_RESULTADO   ", P_DES_RESULTADO.ToString(), null);
                        LblMsjChipCuota.Text = P_DES_RESULTADO.ToString();
                        LblMsjYN.Text = P_NRO_RESULTADO.ToString();
                    }
                }
                objLog.CrearArchivolog("    PROY-33313-FIN  ", null, null);
                //PROY_33313 FIN

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
        }//Proy-140360

        private void IniciarControlCondiciones(BESolicitudEmpresa objCliente, DataTable dtDetallePlan, Int64 nroSEC)//PROY-140743 
        {
            objLog.CrearArchivolog("    IniciarControlCondiciones    ", null, null);
            txtOferta.Text = objCliente.TPROV_DESCRIPCION;
            txtCombo.Text = objCliente.COMBV_DESCRIPCION;

            if (!string.IsNullOrEmpty(txtCombo.Text)) hidFlgCombo.Value = "S";

            txtTelefonoRenovar.Text = objCliente.TELEFONO;
            txtPlanComercial.Text = objCliente.PLAN_ACTUAL;
            txtCFActual.Text = string.Format("{0:#,#,#,0.00}", objCliente.RENOF_CFACTUAL);

            if (objCliente.TOPEN_CODIGO.ToString() == "25") //PROY-140743 - INICIO
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

                if (objCliente.TOPEN_CODIGO.ToString() == "25") //PROY-140743 - INICIO
                {

                    dr["PROMOCIONES"] = Funciones.CheckStr(strPromociones);
                    dr["PROD_FACTURAR"] = Funciones.CheckStr(strProd_Facturar);
                }
            }
            objLog.CrearArchivolog("    IniciarControlCondiciones/SALIDA    ", null, null);

            if (objCliente.TOPEN_CODIGO.ToString() == "25") //PROY-140743 - INICIO
            {
                //Mostrar Columnas de Promociones y Producto a Facturar
                dgPlanes.Columns[5].Visible = true;
                dgPlanes.Columns[6].Visible = true;
            }
            //PROY-140743 - FIN
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


            tipoDocumento = ConfigurationManager.AppSettings["TipoDocumentoRUC"];//
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
                    dgCostoInstalacionHFC.DataSource = ObtenerDatosCostoInstalacion(Funciones.CheckInt64(txtNroSEC.Text), "N");//PROY-140546 Cobro Anticipado de Instalacion
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
                    dgCostoInstalacionHFC1.DataSource = ObtenerDatosCostoInstalacion(Funciones.CheckInt64(txtNroSEC.Text), "S");//PROY-140546 Cobro Anticipado de Instalacion
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
                    if (txtComentarioPdv.Value == "")
                    {
                        txtComentarioPdv.Value = objComentario.comentario_final_pdv;
                    }
                    if (txtComentarioEvaluador.Value == "")
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
                txtModalidad.Text = ObtenerDescripcion("MO", objCliente.TLINC_CODIGO_CEDENTE, "");

                txtNroFormatoPort.Text = objCliente.PORT_SOLIN_NRO_FORMATO;
                txtCargoFijoCedente.Text = objCliente.PORT_CARGO_FIJO_OPE_CED;

                List<BEArchivo> objLista = (new BLPortabilidad()).ListarAchivosAdjunto(0, objCliente.SOLIN_CODIGO, "", "1");
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
                // EMERGENCIA-29215-INICIO
                if ((idProducto == constTipoProductoHFC) || (idProducto == constTipoProducto3PlayInalam) || (idProducto == constTipoProductoFTTH))
                // EMERGENCIA-29215-FIN
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
                    dgPlanes.Columns[18].Visible = false; //PROY-24724-IDEA-28174 - FIN - //PROY-30166-IDEA–38863 //PROY-140743
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
                //PROY-29215 INICIO
                td.ColumnSpan = 3;
                //PROY-29215 FIN
                dvItemHeader.Cells.Add(td);
                td = new TableCell();
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
                //INICIO PROY-140546
                var nCantColumnas = 1;
                if (bConsultaPAtieneData)
                {
                    nCantColumnas = 5;
                }
                else
                {
                    //PROY-29215 INICIO
                    nCantColumnas = 3;
                    //PROY-29215 FIN
                }
                //FIN PROY-140546

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
                td.ColumnSpan = nCantColumnas; //PROY-140546
                //PROY-29215 FIN
                td.Text = "Resultado Evaluación";
                dvItemHeader.Cells.Add(td);
                td = new TableCell();
                //PROY-29215 INICIO
                td.ColumnSpan = nCantColumnas; //PROY-140546
                //PROY-29215 FIN
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
                // emergencia-29215-INICIO
                if (txtFormaPago.Text.ToUpper() == "CONTRATA")
                {
                    ddlCuotas.DataSource = objCuotas.FindAll(X => X.Valor.ToUpper().Equals(txtCuota.Text.ToUpper()));
                    ddlCuotas.DataTextField = "Valor";
                    ddlCuotas.DataValueField = "Codigo";
                    ddlCuotas.DataBind();
                    ddlCuotas.SelectedValue = objCuotas.FindAll(X => X.Valor.ToUpper().Equals(txtCuota.Text.ToUpper()))[0].Codigo;
                }
                else
                {

                    ddlCuotas.DataSource = objCuotas;
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
                    ListItem itemToRemove = ddlCuotas.Items.FindByText("0");
                    if (itemToRemove != null)
                    {
                        ddlCuotas.Items.Remove(itemToRemove);
                    }
                }
                // emergencia-29215-FIN
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
                //INICIO PROY-140546
                var nCantColumnas = 1;
                if (bConsultaPAtieneData)
                {
                    nCantColumnas = 5;
                }
                else
                {
                    //PROY-29215 INICIO
                    nCantColumnas = 3;
                    //PROY-29215 FIN
                }
                //FIN PROY-140546

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
                td.ColumnSpan = nCantColumnas; //PROY-140546
                //PROY-29215 FIN
                td.Text = "Resultado Evaluación";
                dvItemHeader.Cells.Add(td);
                td = new TableCell();
                //PROY-29215 INICIO
                td.ColumnSpan = nCantColumnas; //PROY-140546
                //PROY-29215 FIN
                td.Text = "Resultado Creditos";
                dvItemHeader.Cells.Add(td);
                dgCostoInstalacionHFC1.Controls[0].Controls.AddAt(0, dvItemHeader);
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                dblTotalCostoInstalacionHFC1 += Funciones.CheckDbl(((DataRowView)e.Row.DataItem).Row["COSTO_INSTAL_EVAL"]); //PROY-140546
                dblTotalCostoInstalacionHFC += Funciones.CheckDbl(((DataRowView)e.Row.DataItem).Row["COSTO_INSTAL"]);

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

                txtTotalCuotasPendE.Text = Funciones.CheckStr(objCli[20]) != string.Empty ? Funciones.CheckStr(objCli[20]) : "0";
                txtCantLineasCuotaE.Text = Funciones.CheckStr(objCli[21]) != string.Empty ? Funciones.CheckStr(objCli[21]) : "0";
                txtCantMaxCuotasE.Text = Funciones.CheckStr(objCli[22]) != string.Empty ? Funciones.CheckStr(objCli[22]) : "0";
                txtTotalImportCuotaUltE.Text = Funciones.CheckStr(objCli[23]) != string.Empty ? Funciones.CheckStr(objCli[23]) : "0";
                txtCantTotalLineaCuotaUltE.Text = Funciones.CheckStr(objCli[24]) != string.Empty ? Funciones.CheckStr(objCli[24]) : "0";
                txtCantMaxCuotasGenUltE.Text = Funciones.CheckStr(objCli[25]) != string.Empty ? Funciones.CheckStr(objCli[25]) : "0";
                txtCantCuotasPendLineaE.Text = Funciones.CheckStr(objPlanA[5]) != string.Empty ? Funciones.CheckStr(objPlanA[5]) : "0";
                txtMontoPendCuotasLineaE.Text = Funciones.CheckStr(objPlanA[6]) != string.Empty ? Funciones.CheckStr(objPlanA[6]) : "0";

                if (Equals(strTipoOperacion, "25")) //PROY-140743 - INICIO
                {
                    DataRow objOferta = obj.Tables["CV_OFERTA"].Rows[0];
                    strPromociones = Funciones.CheckStr(objOferta[17]);
                    strProd_Facturar = Funciones.CheckStr(objOferta[18]);
                    txtMontoPendiente.Text = Funciones.CheckStr(objCli[37]) != string.Empty ? Funciones.CheckStr(objCli[39]) : "0";
                    txtCantidadPlanes.Text = Funciones.CheckStr(objCli[38]) != string.Empty ? Funciones.CheckStr(objCli[38]) : "0";
                    txtCantidadMaxima.Text = Funciones.CheckStr(objCli[39]) != string.Empty ? Funciones.CheckStr(objCli[39]) : "0";
                    txtMontoPenCuo.Text = Funciones.CheckStr(objCli[40]) != string.Empty ? Funciones.CheckStr(objCli[40]) : "0";
                    txtCantidadPlaCuo.Text = Funciones.CheckStr(objCli[41]) != string.Empty ? Funciones.CheckStr(objCli[41]) : "0";
                    txtCantidadMaxCuo.Text = Funciones.CheckStr(objCli[42]) != string.Empty ? Funciones.CheckStr(objCli[42]) : "0";
                }//PROY-140743 - FIN

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
                objLog.CrearArchivolog("    FIN/ObtenerDatosBRMS   ", nroSEC.ToString(), null);

            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog(" [ERROR ONBTENER DATOS BRMS] ", null, ex);
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
                objResponse.Boleano = (new BLSolicitud()).AsignarUsuarioSEC(Funciones.CheckInt64(nroSEC), "", nroDocumento, "E");
            }
            catch (Exception ex)
            {
                objResponse.Boleano = false;
                objResponse.DescripcionError = ex.Message;
                objResponse.Mensaje = ConfigurationManager.AppSettings["consMsjErrorEvaluadorEmpresa"].ToString();
                _objLog.CrearArchivolog("[ERROR METODO LIBERAR]", null, ex);
            }
            return objResponse;
        }

        [System.Web.Services.WebMethod()]
        //PROY-29215 INICIO
        public static BEResponseWebMethod grabarCreditos(string nroSEC, string flgConvergente, string strCadenaGarantia, string strCadenaCosto, string strUsuario, string formaPago, string cuota,
            string formaPagoActual, string cuotaActual, string strAsesor, string canalPDV, string maiNuevo, string maiOriginal, string costoInstalacion, string productoHFC, string estadoPa) //PROY-140546
        {
            //PROY-29215 FIN          
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

                objResponse.Mensaje = ConfigurationManager.AppSettings["consMsjEvaluadorEmpresa"].ToString();
                //PROY-29215 INICIO
                if (formaPagoActual != formaPago || cuotaActual != cuota)
                {
                    new BLConsumer().GrabarFormaPagoCuotaEmpresa(Funciones.CheckInt64(nroSEC), Funciones.CheckInt16(cuota), formaPago.ToUpper(), formaPagoActual.ToUpper(), cuotaActual.ToUpper(), strAsesor);
                }
                //PROY-29215 FIN

                //INICIO PROY-140546
                if (productoHFC == "S")
                {
                    _objLog.CrearArchivolog("[RegistraHistorialPagoAnticipado] INICIO", null, null);
                    string sCostoInstalacion = "0";
                    try
                    {
                        //INICIO INC000004292649
                        _objLog.CrearArchivolog(string.Format("{0}{1}", "[RegistraHistorialPagoAnticipado] | costoInstalacion: ", costoInstalacion), null, null);
                        _objLog.CrearArchivolog(string.Format("{0}{1}", "[RegistraHistorialPagoAnticipado] | strCadenaCosto: ", strCadenaCosto), null, null);
                        sCostoInstalacion = strCadenaCosto.Split(';')[1].Substring(0, strCadenaCosto.Split(';')[1].Length - 1);
                        //FIN INC000004292649
                    }
                    catch (Exception ex2)
                    {
                        _objLog.CrearArchivolog("[RegistraHistorialPagoAnticipado] " + "Error" + " - " + ex2.Message, null, null);
                        sCostoInstalacion = "0";
                    }

                    ActualizaMontoAnticipadoInstalacion(nroSEC, canalPDV, maiNuevo, maiOriginal, Funciones.CheckStr(estadoPa));
                    RegistraHistorialPagoAnticipado(Funciones.CheckInt64(nroSEC), strUsuario, Funciones.CheckDbl(sCostoInstalacion), formaPago, Funciones.CheckInt(cuota), Funciones.CheckDbl(maiNuevo));
                    _objLog.CrearArchivolog("[RegistraHistorialPagoAnticipado] FIN", null, null);
                }
                //FIN PROY-140546
            }
            catch (Exception ex)
            {
                objResponse.Boleano = false;
                objResponse.DescripcionError = ex.Message;
                objResponse.Mensaje = ConfigurationManager.AppSettings["consMsjErrorGeneral"].ToString();
                _objLog.CrearArchivolog("[ERROR METODO GRABAR_CREDITOS]", null, ex);
            }
            return objResponse;
        }
        //PROY-29215 INICIO
        public void grabarCostoInstalacion()
        {
            if (Funciones.CheckStr(hidFlgProductoDTH.Value) == "S" || Funciones.CheckStr(hidFlgProductoHFC.Value) == "S")
            {
                string nroCuota = hidCuota.Value;
                string formapago = hidFormaPago.Value.ToUpper();
                string formapagoActual = hidFormaPagoActual.Value.ToUpper();
                string cuotaActual = hidCuotaActual.Value;
                string Asesor = Funciones.CheckStr(CurrentUserSession.Nombre + " " + CurrentUserSession.Apellido_Pat + " " + CurrentUserSession.Apellido_Mat);

                if (formapagoActual != formapago || cuotaActual != Funciones.CheckStr(nroCuota))
                {
                    new BLConsumer().GrabarFormaPagoCuota(Funciones.CheckInt64(txtNroSEC.Text), Funciones.CheckInt16(nroCuota), formapago);

                    objLog.CrearArchivolog("[GrabarFormaPagoCuota] INICIO", null, null);
                    if (formapagoActual != formapago & cuotaActual == nroCuota)
                    {
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[GrabarFormaPagoCuota] | NOMBRE DEL ASESOR: ", Asesor), null, null);
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[GrabarFormaPagoCuota] | FORMA PAGO: ", formapago), null, null);

                    }
                    else if (cuotaActual != nroCuota & formapagoActual == formapago)
                    {
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[GrabarFormaPagoCuota] | NOMBRE DEL ASESOR: ", Asesor), null, null);
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[GrabarFormaPagoCuota] | NRO. CUOTAS: ", nroCuota.ToString()), null, null);
                    }
                    else if (formapagoActual != formapago & cuotaActual != nroCuota)
                    {
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[GrabarFormaPagoCuota] | NOMBRE DEL ASESOR: ", Asesor), null, null);
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[GrabarFormaPagoCuota] | FORMA PAGO: ", formapago), null, null);
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[GrabarFormaPagoCuota] | NRO. CUOTAS: ", nroCuota.ToString()), null, null);
                    }
                }

            }
        }
        //PROY-29215 FIN

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

                if (objSolEmp.ESTAC_CODIGO != ConfigurationManager.AppSettings["constEstadoAPR"].ToString())
                {

                    string tipoProductoMovil = ConfigurationManager.AppSettings["constTipoProductoMovil"].ToString();
                    string tipoProductoBAM = ConfigurationManager.AppSettings["constTipoProductoBAM"].ToString();
                    //PROY-26963 - IDEA-34399: Mejora Portabilidad Despacho PDV
                    string comentarioPDV = Funciones.CheckStr(txtComentarioAlPdv.Value).ToUpper();
                    string comentarioEvaluador = Funciones.CheckStr(txtComentarioEvaluador.Value).ToUpper();
                    string comentario_sistema = "";
                    double nroRA = 0;
                    string estado = "";
                    string estadoDes = "";
                    string rFlagProceso = "";
                    bool blnOK = false;
                    string estadoPort = "";
                    double totalGarantiaEvaluador = 0;

                    string mensajeCP = string.Empty; //IN000000772139 INI
                    string codigoCP = string.Empty; //IN000000772139 INI

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

                    if (idProceso == "R")
                    {
                        estado = ConfigurationManager.AppSettings["constcodEstadoRECHAZADOEVALUACION"].ToString();
                        estadoDes = ConfigurationManager.AppSettings["constdesEstadoRECHAZADOEVALUACION"].ToString();
                    }
                    else if (idProceso == "O")
                    {
                        estado = ConfigurationManager.AppSettings["constcodEstadoOBSERVADO"].ToString();
                        estadoDes = ConfigurationManager.AppSettings["constdesEstadoOBSERVADO"].ToString();
                    }
                    else if (idProceso == "S")
                    {
                        estado = ConfigurationManager.AppSettings["constcodEstadoOBSERVADOPort"].ToString();
                        estadoDes = ConfigurationManager.AppSettings["constdesEstadoOBSERVADOPort"].ToString();
                    }
                    else
                    {
                        if (hidFlagPortabilidad.Value == "S")
                        {
                            estado = ConfigurationManager.AppSettings["constCodEstadoENVIADOPOOLPORTA"].ToString();
                            estadoDes = ConfigurationManager.AppSettings["constDesEstadoENVIADOPOOLPORTA"].ToString();
                        }
                        else
                        {
                            estado = ConfigurationManager.AppSettings["constcodEstadoAPR"].ToString();
                            estadoDes = ConfigurationManager.AppSettings["constDesEstadoAPR"].ToString();
                        }
                    }

                    if (idProceso == "O")
                    {
                        estadoPort = ConfigurationManager.AppSettings["constEstadoEmitidoPortabilidad"].ToString();
                    }
                    else if (idProceso == "S")
                    {
                        estadoPort = ConfigurationManager.AppSettings["constEstadoObservadoPortabilidad"].ToString();
                    }
                    else if (idProceso == "R")
                    {
                        if (hidFlagPortabilidad.Value == "S")
                            estadoPort = ConfigurationManager.AppSettings["constEstadoRechazadoPortabilidad"].ToString();
                        else
                            estadoPort = ConfigurationManager.AppSettings["constEstadoEmitidoPortabilidad"].ToString();
                    }
                    else
                    {
                        estadoPort = ConfigurationManager.AppSettings["constEstadoEmitidoPortabilidad"].ToString();
                    }

                    string login = CurrentUser;
                    string loginAutorizador = hidUsuarioAutorizador.Value;

                    BLPortabilidad objPorta = new BLPortabilidad();

                    objLog.CrearArchivolog("[Inicio][GrabarDatosEvaluador().EnviarMP][estadoSol] " + estado, null, null);
                    blnOK = objPorta.GrabarDatosEvaluador(nroSEC, nroRA, strTipoGarantia, estado, estadoDes, totalGarantiaEvaluador, comentarioPDV, comentarioEvaluador, comentario_sistema, login, loginAutorizador, estadoPort, ref rFlagProceso);
                    objLog.CrearArchivolog("[Fin][GrabarDatosEvaluador().EnviarMP][codigoRpta] " + Funciones.CheckStr(blnOK), null, null);

                    // Envio directo a MP
                    if (blnOK && estado == ConfigurationManager.AppSettings["constCodEstadoENVIADOPOOLPORTA"].ToString())
                    {
                        //PROY-26963 - IDEA-34399: Mejora Portabilidad Despacho PDV
                        BESolicitudEmpresa datos = new BESolicitudEmpresa();
                        datos = (BESolicitudEmpresa)HttpContext.Current.Session["SolicitudEmpresa"];
                        objLog.CrearArchivolog("[BESolicitudEmpresa][PRDC_CODIGO] " + datos.PRDC_CODIGO, null, null);
                        if (datos.PRDC_CODIGO == tipoProductoMovil || datos.PRDC_CODIGO == tipoProductoBAM)
                        {
                            objLog.CrearArchivolog("[Inicio][WebComunes().ActualizarConsultaPrevia]", null, null);
                            BeConsultaPrevia objConsultaPrevia = new BeConsultaPrevia()
                            {
                                auditoria = new BEItemGenerico()
                                {
                                    Codigo = DateTime.Now.ToString("yyyyMMddhhmmssff"),
                                    Descripcion2 = CurrentTerminal,
                                    Descripcion = ConfigurationManager.AppSettings["constNombreAplicacion"].ToString(),
                                    Codigo2 = CurrentUser
                                },
                                numeroSEC = nroSEC,
                                codigoCedente = Funciones.CheckStr(datos.PORT_OPER_CED).ToString(),
                                modalidad = datos.TLINC_CODIGO_CEDENTE,
                                tipoDocumento = datos.TDOCC_CODIGO,
                                numeroDocumento = datos.CLIEC_NUM_DOC,
                                nombreRSAbonado = datos != null ? (!string.IsNullOrEmpty(datos.RAZON_SOCIAL) ? datos.RAZON_SOCIAL : datos.CLIEV_RAZ_SOC) : string.Empty,
                                modalidadVenta = datos.ID_MODALIDAD_VENTA, //PROY-140223 IDEA-140462
                                canalVenta = datos.TOFIC_CODIGO //PROY-140223 IDEA-140462
                            };

                            objLog.CrearArchivolog("[Inicio][WebComunes().ActualizarConsultaPrevia][idTransaccion] " + objConsultaPrevia.auditoria.Codigo, null, null);
                            objLog.CrearArchivolog("[Inicio][WebComunes().ActualizarConsultaPrevia][usuarioAplicacion] " + objConsultaPrevia.auditoria.Codigo2, null, null);
                            objLog.CrearArchivolog("[Inicio][WebComunes().ActualizarConsultaPrevia][solInCodigo] " + objConsultaPrevia.numeroSEC, null, null);
                            objLog.CrearArchivolog("[Inicio][WebComunes().ActualizarConsultaPrevia][numeroSecuencial] " + objConsultaPrevia.numeroSecuencial, null, null);

                            BEItemMensaje objMensaje = new WebComunes().ActualizarConsultaPrevia(objConsultaPrevia);
                            //IN000000772139 INI
                            mensajeCP = objMensaje.mensajeCliente;
                            codigoCP = objMensaje.codigo;
                            //IN000000772139 FIN

                            objLog.CrearArchivolog("[Fin][WebComunes().ActualizarConsultaPrevia][codigo] " + objMensaje.codigo, null, null);
                            objLog.CrearArchivolog("[Fin][WebComunes().ActualizarConsultaPrevia][descripcion] " + objMensaje.descripcion, null, null);
                        }

                    //PROY-26963 - IDEA-34399: Mejora Portabilidad Despacho PDV
                        else
                        {
                            blnOK = objPorta.EnviarMesaPortabilidad(nroSEC, login);

                            //Inicio IDEA-30067
                            if (constFlagPortaAutomatico == "S")
                            {
                                PortaConsultaPreviaAutomatico();
                            }
                            //Fin IDEA-30067
                        }
                    }

                    //PROY-29215 INICIO
                    if (Funciones.CheckStr(hidFlgProductoDTH.Value) == "S" || Funciones.CheckStr(hidFlgProductoHFC.Value) == "S")
                    {
                        string nroCuota = hidCuota.Value;
                        string formapago = hidFormaPago.Value.ToUpper();
                        string formapagoActual = hidFormaPagoActual.Value.ToUpper();
                        string cuotaActual = hidCuotaActual.Value;
                        string Asesor = Funciones.CheckStr(CurrentUserSession.Nombre + " " + CurrentUserSession.Apellido_Pat + " " + CurrentUserSession.Apellido_Mat);

                        if (formapagoActual != formapago || cuotaActual != Funciones.CheckStr(nroCuota))
                        {
                            new BLConsumer().GrabarFormaPagoCuota(Funciones.CheckInt64(txtNroSEC.Text), Funciones.CheckInt16(nroCuota), formapago);

                            objLog.CrearArchivolog("[GrabarFormaPagoCuota] INICIO", null, null);
                            if (formapagoActual != formapago & cuotaActual == nroCuota)
                            {
                                objLog.CrearArchivolog(string.Format("{0}{1}", "[GrabarFormaPagoCuota] | NOMBRE DEL ASESOR: ", Asesor), null, null);
                                objLog.CrearArchivolog(string.Format("{0}{1}", "[GrabarFormaPagoCuota] | FORMA PAGO: ", formapago), null, null);

                            }
                            else if (cuotaActual != nroCuota & formapagoActual == formapago)
                            {
                                objLog.CrearArchivolog(string.Format("{0}{1}", "[GrabarFormaPagoCuota] | NOMBRE DEL ASESOR: ", Asesor), null, null);
                                objLog.CrearArchivolog(string.Format("{0}{1}", "[GrabarFormaPagoCuota] | NRO. CUOTAS: ", nroCuota.ToString()), null, null);
                            }
                            else if (formapagoActual != formapago & cuotaActual != nroCuota)
                            {
                                objLog.CrearArchivolog(string.Format("{0}{1}", "[GrabarFormaPagoCuota] | NOMBRE DEL ASESOR: ", Asesor), null, null);
                                objLog.CrearArchivolog(string.Format("{0}{1}", "[GrabarFormaPagoCuota] | FORMA PAGO: ", formapago), null, null);
                                objLog.CrearArchivolog(string.Format("{0}{1}", "[GrabarFormaPagoCuota] | NRO. CUOTAS: ", nroCuota.ToString()), null, null);
                            }
                        }

                        //INICIO PROY-140546
                        if (Funciones.CheckStr(hidFlgProductoHFC.Value) == "S")
                        {
                            objLog.CrearArchivolog("[RegistraHistorialPagoAnticipado] INICIO", null, null);
                            string sCostoInstalacion = "0";
                            try
                            {
                                sCostoInstalacion = hidCostoInstalacion.Value.Split(';')[1].Substring(0, hidCostoInstalacion.Value.Split(';')[1].Length - 1);
                            }
                            catch (Exception ex2)
                            {
                                objLog.CrearArchivolog("[RegistraHistorialPagoAnticipado] " + "Error" + " - " + ex2.Message, null, null);
                                sCostoInstalacion = "0";
                            }

                            ActualizaMontoAnticipadoInstalacion(txtNroSEC.Text, hidCanalSEC.Value, hidMAI.Value, hidMAIOriginal.Value, Funciones.CheckStr(hidEstadoPa.Value));
                            RegistraHistorialPagoAnticipado(nroSEC, CurrentUser, Funciones.CheckDbl(sCostoInstalacion), formapago, Funciones.CheckInt(nroCuota), Funciones.CheckDbl(hidMAI.Value));
                            objLog.CrearArchivolog("[RegistraHistorialPagoAnticipado] FIN", null, null);
                        }
                        //FIN PROY-140546
                    }
                    //PROY-29215 FIN

                    //IN000000772139 INI                                                                                                                                               
                    if (codigoCP != "0")
                    {
                        //PROY-140223 IDEA-140462 //CAMBIADO POR EL PROY-140335 RF1
                        //if (AppSettings.consFlagConsultaPreviaChip == "1")
                        if (mensajeCP == null || Funciones.CheckStr(mensajeCP) == "")
                        {
                            hidnMensajeValue.Value = string.Format(ConfigurationManager.AppSettings["consMsjEvaluadorEmpresa"].ToString(), nroSEC.ToString()) + ".";
                        }
                        //FIN: PROY-140335 RF1
                        else
                        {
                            hidnMensajeValue.Value = ConfigurationManager.AppSettings["consMsjEvaluadorEmpresa"].ToString() +
                        "." + string.Format(mensajeCP.ToString(), nroSEC.ToString()) + ".";
                        }
                        //PROY-140223 IDEA-140462
                    }
                    else
                    {
                        hidnMensajeValue.Value = ConfigurationManager.AppSettings["consMsjEvaluadorEmpresa"].ToString();
                    }
                    //IN000000772139 FIN
                    //hidProceso.Value = "OK";

                    objLog.CrearArchivolog("[Fin][Grabar().EnviarMP][ExitoEnvioCP] " + codigoCP + " - " + mensajeCP, null, null);
                }
                else
                {
                    objLog.CrearArchivolog("[Fin][Grabar().EnviarMP][ErrorSolEstado] " + objSolEmp.SOLIN_CODIGO + " - " + objSolEmp.ESTAC_CODIGO, null, null);
                    hidnMensajeValue.Value = String.Format(ConfigurationManager.AppSettings["constMsjEstadoSec"].ToString(), nroSEC);
                }

                hidProceso.Value = "OK";
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[Fin][Grabar().EnviarMP][codigoError] " + ex.Message, null, null);
                hidnMensajeValue.Value = ConfigurationManager.AppSettings["consMsjErrorEvaluadorEmpresa"].ToString();
                hidProceso.Value = "OK";
            }

        }

        //Inicio IDEA-30067
        private void PortaConsultaPreviaAutomatico()
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

                string keyLog = string.Format("{0}|{1}|{2}", numeroSec.ToString(), "PROY-BLACKOUT", "PortaConsultaPreviaAutomatico()");
                CrearLogStatic(keyLog).CrearArchivolog("[INFO]-", "SE INICIO EL METODO", null);
                CrearLogStatic(keyLog).CrearArchivolog(string.Format("{0},{1} ", "[INFO]-", " objAudit.Estado => "), " " + objAudit.Estado, null);
                CrearLogStatic(keyLog).CrearArchivolog(string.Format("{0},{1} ", "[INFO]-", " objAudit.Valor  => "), " " + objAudit.Valor, null);
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

        //PROY-140546 Cobro Anticipado de Instalacion Inicio
        public DataTable ObtenerDatosCostoInstalacion(Int64 pNroSec, string flagConsulta)
        {
            DataTable dtResultado = new DataTable();

            GeneradorLog objlog = new GeneradorLog(CurrentUsers, null, null, "WEB");
            objlog.CrearArchivolog("PROY-140546|sisact_reporte_evaluacion_empresa|ObtenerDatosCostoInstalacion|-- INICIO --", null, null);
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
                string host = ReadKeySettings.ConsCurrentIP;// System.Net.Dns.GetHostEntry(nombreServer).AddressList[3].ToString();
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
            catch (Exception e)
            {
                objlog.CrearArchivolog(string.Format("{0} --> {1}", "[PROY-140546][ObtenerDatosCostoInstalacionEmp]", " ERROR[" + e.Message + "|" + e.StackTrace + "]"), null, null);
            }

            objlog.CrearArchivolog("- formaPago -> " + Funciones.CheckStr(formaPago), null, null);
            objlog.CrearArchivolog("- formaPagoMan -> " + Funciones.CheckStr(formaPagoMan), null, null);
            objlog.CrearArchivolog("- nroCuota -> " + Funciones.CheckStr(nroCuota), null, null);
            objlog.CrearArchivolog("- nroCuotaMan -> " + Funciones.CheckStr(nroCuotaMan), null, null);
            objlog.CrearArchivolog("- mai -> " + Funciones.CheckStr(mai), null, null);
            objlog.CrearArchivolog("- maiMan -> " + Funciones.CheckStr(maiMan), null, null);
            objlog.CrearArchivolog("- estado -> " + Funciones.CheckStr(estado), null, null);
            objlog.CrearArchivolog("- bConsultaPAtieneData -> " + Funciones.CheckStr(bConsultaPAtieneData), null, null);

            dtResultado = (new BLSolicitud()).ObtenerCostoInstalacionHFC(Funciones.CheckInt64(txtNroSEC.Text));

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

                hidMAIOriginal.Value = Funciones.CheckStr(mai);

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

            objlog.CrearArchivolog("PROY-140546|sisact_reporte_evaluacion_empresa|ObtenerDatosCostoInstalacion|-- FIN --", null, null);
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

        [System.Web.Services.WebMethod()]
        public static BEResponseWebMethod ActualizaMontoAnticipadoInstalacion(string pNroSEC, string pCanal_Sec, string pMAI, string pMAIOriginal,string estadoPa)
        {
            BEResponseWebMethod objResponse = new BEResponseWebMethod();
            GeneradorLog _objLog = CrearLogStatic(pNroSEC);
            try
            {
                bool bEsCanalPermitido = Funciones.EsValorPermitido(pCanal_Sec, ReadKeySettings.Key_CanalesPermitidosCAI, ",");
                bool bCambioMAI = (Funciones.CheckDbl(pMAI) != Funciones.CheckDbl(pMAIOriginal) ? true : false);

                if (bEsCanalPermitido && bCambioMAI)
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
                    oBeanRequest.estado = (Funciones.CheckStr(estadoPa) == "5") ? "8" : "0";
                    oBeanRequest.numeroSolicitud = Funciones.CheckInt64(pNroSEC);
                    oBeanRequest.usuarioActualizacion = CurrentUsers;
                    oBeanRequest.montoInicialModificado = pMAI;

                    oListaRequest.Add(oBeanRequest);

                    oMessageRequest.Body = new Entity.ClientePagoAnticipadoFijaRest.DataPower.ActualizaPA.ActualizaPABody();
                    oMessageRequest.Body.actualizaPARequest = new Entity.ClientePagoAnticipadoFijaRest.DataPower.ActualizaPA.ActualizaPARequest();
                    oMessageRequest.Body.actualizaPARequest.actualizaPARequestType = oListaRequest;

                    oGenericRequest.MessageRequest = oMessageRequest;

                    bool bResultado = oService.ActualizarPagoAnticipado(oGenericRequest);

                    objResponse.Error = false;
                }
            }
            catch (Exception ex)
            {
                objResponse.Error = true;
                objResponse.DescripcionError = ex.Message;
                objResponse.Mensaje = ConfigurationManager.AppSettings["consMsjErrorGeneral"].ToString();
                _objLog.CrearArchivolog("[ERROR METODO ActualizaMontoAnticipadoInstalacion]", null, ex);
            }
            return objResponse;
        }

        private static void RegistraHistorialPagoAnticipado(Int64 nroSec, string usuario, double costoInstMan, string formaPagoMan, int numCuotaMan, double montoAntiInst)
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
