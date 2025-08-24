using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Claro.SISACT.Business;
using Claro.SISACT.Common;
using Claro.SISACT.Entity;
using Claro.SISACT.Web.Comun;
using Claro.SISACT.Web.Base;
using System.Data;
//INICIO PROY-140546
using Claro.SISACT.Business.RestReferences;
using PAGOANTICIPADO_ENTITY = Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest;
using Claro.SISACT.Entity.DataPowerRest;
using Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest.DataPower.RegistraHistorial;
using Claro.SISACT.WS.RestReferences;
using Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest.DataPower.ConsultaHistorico.Response;
using Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest.DataPower.ConsultaHistorico.Request;
//FIN PROY-140546

namespace Claro.SISACT.Web.Paginas.evaluacion_cons
{
    public partial class sisact_edicion_garantia : Sisact_Webbase
    {
        GeneradorLog objLog = new GeneradorLog("    sisact_edicion_garantia   ", null, null, "WEB");

        #region [Declaracion de Constantes - Config]

        // INC000003135879 INICIO
        double dblCostoInstalacionAnterior = 0;
        double dblCostoInstalacionModifica = 0;
        // INC000003135879 FIN
        double dblMontoGarantiaCred = 0;
        double dblMontoGarantiaEval = 0;
        double dblTotalCostoInstalacion = 0.0;
        double dblTotalCostoInstalacionHFC = 0.0;
        //PROY-29215 INICIO
        double dblTotalCostoInstalacionHFC1 = 0.0; //PROY-140546
        List<BEItemGenerico> objFormaPago = new List<BEItemGenerico>();
        List<BEItemGenerico> objCuotas = new List<BEItemGenerico>();
        List<BEItemGenerico> objConsultaFC = new List<BEItemGenerico>();
        //PROY-29215 FIN
        List<BETipoGarantia> objTipoGarantia = new List<BETipoGarantia>();
        string constClaseEmpresaTop = ConfigurationManager.AppSettings["constCodClasificacionEmpresaTop"].ToString();
        string constFlgPortabilidad = ConfigurationManager.AppSettings["FlagPortabilidad"].ToString();
        string constTipoProductoDTH = ConfigurationManager.AppSettings["constTipoProductoDTH"].ToString();
        string constTipoProductoHFC = ConfigurationManager.AppSettings["constTipoProductoHFC"].ToString();
        //PROY-29215 INICIO
        string constTipoProducto3PlayInalam = ConfigurationManager.AppSettings["constTipoProducto3PlayInalam"];
        //PROY-29215 FIN

        // EMERGENCIA-29215-INICIO
        string constTipoProductoFTTH = ConfigurationManager.AppSettings["constTipoProductoFTTH"];
        // EMERGENCIA-29215-FIN


        string constTipoOperMigracion = ConfigurationManager.AppSettings["constTipoOperacionMIG"].ToString();

        static string constEstadoAprobado = ConfigurationManager.AppSettings["constEstadoAPR"].ToString();
        static string constEstadoAnulado = ConfigurationManager.AppSettings["constCodEstadoAnulado"].ToString();
        static string constEstadoRechazado = ConfigurationManager.AppSettings["constcodEstadoRECHAZADOEVALUACION"].ToString();
        static string constEstadoRechazadoNoAdj = ConfigurationManager.AppSettings["constcodEstadoRECHAZADOXNOADJSUSTENTO"].ToString();

        bool bConsultaPAtieneData = false; //PROY-140546 
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

            if (Page.IsPostBack)
            {
                string accion = hidProceso.Value;
                hidProceso.Value = string.Empty;
                switch (accion)
                {
                    case "B":
                        buscar();
                        break;
                    case "G":
                        grabar();
                        break;
                }
            }
        }

        public void buscar()
        {
            objLog.CrearArchivolog("    BUSCAR   ", null, null);
            Int64 nroSEC = Funciones.CheckInt64(txtBuscarNroSEC.Text);
            string mensaje = "";
            hidEstadoPa.Value = "";//PROY-140546 PA
            // Consulta Datos Solicitud
            objLog.CrearArchivolog("    BUSCAR/ObtenerSolicitudPersona/nroSEC   ", nroSEC.ToString(), null);

            DataTable dtSolicitud = new BLSolicitud().ObtenerSolicitudPersona(nroSEC);
            DataRow drSolicitud = null;

            //PROY-29215 INICIO
            string varPrdc_codigo = dtSolicitud.Rows[0]["PRDC_CODIGO"].ToString();
            hidPdvSEC.Value = dtSolicitud.Rows[0]["OVENC_CODIGO"].ToString(); //PROY-140546

            // EMERGENCIA-29215-INICIO
            if (varPrdc_codigo == constTipoProductoDTH || varPrdc_codigo == constTipoProductoHFC || varPrdc_codigo == constTipoProducto3PlayInalam || varPrdc_codigo == constTipoProductoFTTH)
            {
                // EMERGENCIA-29215-fin

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
            //PROY-29215 FIN


            if (dtSolicitud.Rows.Count > 0)
            {
                drSolicitud = dtSolicitud.Rows[0];

                if (Funciones.CheckInt64(drSolicitud["SOLIN_GRUPO_SEC"]) != nroSEC)
                {
                    mensaje = string.Format("La SEC Nro: {0} no existe", nroSEC);
                }
                if (drSolicitud["ESTAC_CODIGO"].ToString() == constEstadoAnulado)
                {
                    mensaje = string.Format("La SEC Nro: {0} se encuentra Anulada.", nroSEC);
                }
                if (drSolicitud["ESTAC_CODIGO"].ToString() == constEstadoAprobado && !string.IsNullOrEmpty(drSolicitud["SOLIV_NUM_CON"].ToString()))
                {
                    mensaje = string.Format("La SEC Nro: {0} ya fue utilizada en Venta.", nroSEC);
                }
                if (drSolicitud["ESTAC_CODIGO"].ToString() == constEstadoRechazadoNoAdj)
                {
                    mensaje = string.Format("La SEC Nro: {0} se encuentra Rechazada.", nroSEC);
                }
            }
            else
            {
                mensaje = string.Format("La SEC Nro: {0} no existe", nroSEC);
            }

            if (!string.IsNullOrEmpty(mensaje))
            {
                Script("alert('" + mensaje + "');");
                return;
            }

            if (drSolicitud["ESTAC_CODIGO"].ToString() == constEstadoRechazado)
            {
                hidFlgSecRechazada.Value = "S";
            }

            txtNroSEC.Text = nroSEC.ToString();

            string tipoDocumento = Funciones.CheckStr(drSolicitud["TDOCC_CODIGO"]);
            string nroDocumento = Funciones.CheckStr(drSolicitud["CLIEC_NUM_DOC"]);

            txtNroDocumento.Text = Funciones.FormatoNroDocumento(tipoDocumento, nroDocumento);

            if (drSolicitud["TDOCC_CODIGO"].ToString() == ConfigurationManager.AppSettings["TipoDocumentoRUC"].ToString())
            {
                txtNombres.Text = drSolicitud["CLIEV_RAZ_SOC"].ToString();
            }
            else
            {
                txtNombres.Text = drSolicitud["CLIEV_NOMBRE"].ToString() + " " + drSolicitud["CLIEV_APE_PAT"].ToString() + " " + drSolicitud["CLIEV_APE_MAT"].ToString();
            }

            // Consulta Tipo Garantia
            objLog.CrearArchivolog("    BUSCAR/ListaTipoGarantia   ", null, null);
            objTipoGarantia = (new BLGeneral()).ListaTipoGarantia(string.Empty, "1");

            // Consulta Datos Garantía
            objLog.CrearArchivolog("    BUSCAR/ObtenerInformacionGarantiaII/nroSEC   ", nroSEC.ToString(), null);
            DataTable dtDatosGarantia = new BLConsumer().ObtenerInformacionGarantiaII(nroSEC);
            string codProd = Funciones.CheckStr(drSolicitud["PRDC_CODIGO"]);
            IniciarControlEvaluador(dtDatosGarantia);

            objLog.CrearArchivolog("    BUSCAR/SALIDA   ", null, null);
            hidProceso.Value = "I";
        }

        private void IniciarControlEvaluador(DataTable dtDatosGarantia)
        {
            objLog.CrearArchivolog("    IniciarControlEvaluador   ", null, null);

            DataTable dtCostoInstalacion = new BLSolicitud().ObtenerCostoInstalacion(Funciones.CheckInt64(txtNroSEC.Text));
            if (dtCostoInstalacion.Rows.Count > 0) hidFlgProductoDTH.Value = "S";

            DataTable dtCostoInstalacionHFC = ObtenerDatosCostoInstalacion(Funciones.CheckInt64(txtNroSEC.Text));//PROY-140546 Cobro Anticipado de Instalacion
            if (dtCostoInstalacionHFC.Rows.Count > 0) hidFlgProductoHFC.Value = "S";

            if (hidFlgSecRechazada.Value != "S")
            {
                chkReingresoSec.Enabled = false;

                dgCostoInstalacion.DataSource = dtCostoInstalacion;
                dgCostoInstalacion.DataBind();

                dgCostoInstalacionHFC.DataSource = dtCostoInstalacionHFC;
                dgCostoInstalacionHFC.DataBind();
            }
            else
            {
                chkReingresoSec.Enabled = true;

                dgCostoInstalacion1.DataSource = dtCostoInstalacion;
                dgCostoInstalacion1.DataBind();

                dgCostoInstalacionHFC1.DataSource = dtCostoInstalacionHFC;
                dgCostoInstalacionHFC1.DataBind();
            }

            // INC000003135879 INICIO
            if (dtCostoInstalacion.Rows.Count > 0 && hidFlgProductoDTH.Value == "S")
            {
                objLog.CrearArchivolog("[Buscar] | INC000003135879 - INICIA DataRow (dtCostoInstalacion)", null, null);
                foreach (DataRow row in dtCostoInstalacion.Rows)
                {
                    hidCostoInstalacionAnterior.Value = Funciones.CheckStr(row[2].ToString());
                }
                objLog.CrearArchivolog(string.Format("{0}{1}", "[Buscar] | INC000003135879 hidCostoInstalacionAnterior: ", Funciones.CheckStr(hidCostoInstalacionAnterior.Value)), null, null);
                objLog.CrearArchivolog("[Buscar] | INC000003135879 - FINALIZA DataRow (dtCostoInstalacion)", null, null);
            }
            if (dtCostoInstalacionHFC.Rows.Count > 0 && hidFlgProductoHFC.Value == "S")
            {
                objLog.CrearArchivolog("[Buscar] | INC000003135879 - INICIA DataRow (dtCostoInstalacionHFC)", null, null);
                foreach (DataRow row in dtCostoInstalacionHFC.Rows)
                {
                    hidCostoInstalacionAnterior.Value = Funciones.CheckStr(row[3].ToString());
                }
                objLog.CrearArchivolog(string.Format("{0}{1}", "[Buscar] | INC000003135879 hidCostoInstalacionAnterior: ", Funciones.CheckStr(hidCostoInstalacionAnterior.Value)), null, null);
                objLog.CrearArchivolog("[Buscar] | INC000003135879 - FINALIZA DataRow (dtCostoInstalacionHFC)", null, null);
            }
            // INC000003135879 FIN

            bool flgGarantiaConvergente = true;
            hidContFila.Value = dtDatosGarantia.Rows.Count.ToString();

            if (dtDatosGarantia.Rows.Count == 0)
            {
                flgGarantiaConvergente = false;
                dtDatosGarantia = new BLConsumer().ObtenerInformacionGarantia(Funciones.CheckInt64(txtNroSEC.Text));
            }

            hidFlgConvergente.Value = flgGarantiaConvergente ? "S" : "N";

            if (hidFlgSecRechazada.Value != "S")
            {
                gvResultadoCredito.DataSource = null;
                gvResultadoCredito2.DataSource = null;
                if (flgGarantiaConvergente)
                {
                    gvResultadoCredito2.DataSource = dtDatosGarantia;
                }
                else
                {
                    gvResultadoCredito.DataSource = dtDatosGarantia;
                }
                gvResultadoCredito.DataBind();
                gvResultadoCredito2.DataBind();
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
            objLog.CrearArchivolog("    IniciarControlEvaluador/SALIDA   ", null, null);
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
                dblMontoGarantiaEval += Funciones.CheckDbl(((DataRowView)e.Row.DataItem).Row["SOLIN_IMP_DG"]);
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[6].Text = dblMontoGarantiaCred.ToString();
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
                dblMontoGarantiaEval += Funciones.CheckDbl(((DataRowView)e.Row.DataItem).Row["solin_importe"]);
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[7].Text = dblMontoGarantiaCred.ToString();
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
                dgCostoInstalacion.Controls[0].Controls.AddAt(0, dvItemHeader);
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtCostoInstalacion = (TextBox)e.Row.FindControl("txtCostoInstalacion");
                //PROY-29215 INICIO
                TextBox txtFormaPagoTVSAT = (TextBox)e.Row.FindControl("txtFormaPagoTVSAT");
                TextBox txtCuotaTVSAT = (TextBox)e.Row.FindControl("txtCuotaTVSAT");
                //PROY-29215 FIN

                txtCostoInstalacion.Text = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["COSTO_INSTAL"]);
                txtCostoInstalacion.Attributes.Add("onBlur", "calcularTotalCostoInstalacion()");//PROY-24740
                dblTotalCostoInstalacion += Funciones.CheckDbl(txtCostoInstalacion.Text);

                //PROY-29215 INICIO

                txtFormaPagoTVSAT.Text = objConsultaFC[0].Descripcion.ToUpper();
                txtFormaPagoTVSAT.ReadOnly = true;
                txtCuotaTVSAT.Text = objConsultaFC[0].Descripcion2;
                txtCuotaTVSAT.ReadOnly = true;

                DropDownList ddlFormaPagoTVSAT = (DropDownList)e.Row.FindControl("ddlFormaPagoTVSAT");
                DropDownList ddlNroCuotasTVSAT = (DropDownList)e.Row.FindControl("ddlNroCuotasTVSAT");

                ddlFormaPagoTVSAT.DataSource = objFormaPago;
                ddlFormaPagoTVSAT.DataTextField = "Valor";
                ddlFormaPagoTVSAT.DataValueField = "Codigo";

                ddlFormaPagoTVSAT.DataBind();
                for (int j = 0; j <= objFormaPago.Count - 1; j++)
                {
                    if (objFormaPago[j].Valor.ToUpper() == txtFormaPagoTVSAT.Text.ToUpper())
                    {
                        ddlFormaPagoTVSAT.SelectedValue = objFormaPago[j].Codigo;
                    }
                }

                // emergencia-29215-INICIO
                if (txtFormaPagoTVSAT.Text.ToUpper() == "CONTRATA")
                {
                    ddlNroCuotasTVSAT.DataSource = objCuotas.FindAll(X => X.Valor.ToUpper().Equals(txtCuotaTVSAT.Text.ToUpper()));
                    ddlNroCuotasTVSAT.DataTextField = "Valor";
                    ddlNroCuotasTVSAT.DataValueField = "Codigo";
                    ddlNroCuotasTVSAT.DataBind();
                    ddlNroCuotasTVSAT.SelectedValue = objCuotas.FindAll(X => X.Valor.ToUpper().Equals(txtCuotaTVSAT.Text.ToUpper()))[0].Codigo;
                }
                else
                {

                    ddlNroCuotasTVSAT.DataSource = objCuotas;
                    ddlNroCuotasTVSAT.DataTextField = "Valor";
                    ddlNroCuotasTVSAT.DataValueField = "Codigo";
                    ddlNroCuotasTVSAT.DataBind();
                    for (int k = 0; k <= objCuotas.Count - 1; k++)
                    {
                        if (objCuotas[k].Valor.ToUpper() == txtCuotaTVSAT.Text.ToUpper())
                        {
                            ddlNroCuotasTVSAT.SelectedValue = objCuotas[k].Codigo;
                        }
                    }
                    ListItem itemToRemove = ddlNroCuotasTVSAT.Items.FindByText("0");
                    if (itemToRemove != null)
                    {
                        ddlNroCuotasTVSAT.Items.Remove(itemToRemove);
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
                TextBox txtFormaPagoTVSAT1 = (TextBox)e.Row.FindControl("txtFormaPagoTVSAT1");
                TextBox txtCuotaTVSAT1 = (TextBox)e.Row.FindControl("txtCuotaTVSAT1");

                txtFormaPagoTVSAT1.Text = objConsultaFC[0].Descripcion.ToUpper();
                txtFormaPagoTVSAT1.ReadOnly = true;
                txtCuotaTVSAT1.Text = objConsultaFC[0].Descripcion2;
                txtCuotaTVSAT1.ReadOnly = true;
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
                txtCostoInstalacion.Attributes.Add("onBlur", "calcularTotalCostoInstalacion(1)"); //PROY-24740
                dblTotalCostoInstalacionHFC += Funciones.CheckDbl(txtCostoInstalacion.Text);

                dblTotalCostoInstalacionHFC1 += Funciones.CheckDbl(((DataRowView)e.Row.DataItem).Row["COSTO_INSTAL_EVAL"]); //FALLAS PROY-140546

                //PROY-29215 INICIO
                TextBox txtFormaPago = (TextBox)e.Row.FindControl("txtFormaPago");
                TextBox txtCuotaPago = (TextBox)e.Row.FindControl("txtCuotaPago");

                //INI PROY-140546 FALLA
                txtFormaPago.Text = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["FORMA_PAGO"]);
                txtFormaPago.ReadOnly = true;
                txtCuotaPago.Text = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["NRO_CUOTA"]);
                txtCuotaPago.ReadOnly = true;

                string formaPagoMan = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["FORMA_PAGO_MAN"]);
                string cuotaPagoMan = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["NRO_CUOTA_MAN"]);
                //FIN PROY-140546 FALLA

                DropDownList ddlFormaPago = (DropDownList)e.Row.FindControl("ddlFormaPago");
                DropDownList ddlNroCuotas = (DropDownList)e.Row.FindControl("ddlNroCuotas");

                ddlFormaPago.DataSource = objFormaPago;
                ddlFormaPago.DataTextField = "Valor";
                ddlFormaPago.DataValueField = "Codigo";
                ddlFormaPago.DataBind();
                for (int j = 0; j <= objFormaPago.Count - 1; j++)
                {
                    if (objFormaPago[j].Valor.ToUpper() == formaPagoMan.ToUpper())  //FALLAS PROY-140546
                    {
                        ddlFormaPago.SelectedValue = objFormaPago[j].Codigo;
                    }
                }

                // emergencia-29215-INICIO                
                if (formaPagoMan.ToUpper() == "CONTRATA")   //FALLAS PROY-140546
                {
                    ddlNroCuotas.DataSource = objCuotas.FindAll(X => X.Valor.ToUpper().Equals(cuotaPagoMan.ToUpper()));  //FALLAS PROY-140546
                    ddlNroCuotas.DataTextField = "Valor";
                    ddlNroCuotas.DataValueField = "Codigo";
                    ddlNroCuotas.DataBind();
                    ddlNroCuotas.SelectedValue = objCuotas.FindAll(X => X.Valor.ToUpper().Equals(cuotaPagoMan.ToUpper()))[0].Codigo;  //FALLAS PROY-140546
                }
                else
                {

                    ddlNroCuotas.DataSource = objCuotas;
                    ddlNroCuotas.DataTextField = "Valor";
                    ddlNroCuotas.DataValueField = "Codigo";
                    ddlNroCuotas.DataBind();
                    for (int k = 0; k <= objCuotas.Count - 1; k++)
                    {
                        if (objCuotas[k].Valor.ToUpper() == cuotaPagoMan.ToUpper()) //FALLAS PROY-140546
                        {
                            ddlNroCuotas.SelectedValue = objCuotas[k].Codigo;
                        }
                    }
                    ListItem itemToRemove = ddlNroCuotas.Items.FindByText("0");
                    if (itemToRemove != null)
                    {
                        ddlNroCuotas.Items.Remove(itemToRemove);
                    }
                }
                // emergencia-29215-FIN

                //PROY-29215 FIN

                //PROY-140546 Cobro Anticipado de Instalacion Inicio
                TextBox txtMAI = (TextBox)e.Row.FindControl("txtMAINoEdit");
                txtMAI.Text = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["MAI"]);

                double nMAI = Funciones.CheckDbl(txtMAI.Text);

                int nNumCuotas = Funciones.CheckInt(txtCuotaPago.Text);

                int nNumCuotas2 = Funciones.CheckInt(cuotaPagoMan);  //FALLAS PROY-140546

                TextBox txtCM = (TextBox)e.Row.FindControl("txtCMNoEdit");
                txtCM.Text = CalcularCM(dblTotalCostoInstalacionHFC1, nMAI, nNumCuotas).ToString(); //FALLAS PROY-140546
                
                TextBox txtMAIEdit = (TextBox)e.Row.FindControl("txtMAIEdit");
                txtMAIEdit.Text = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["MAI_MAN"]); //FALLAS PROY-140546

                double nMAI2 = Funciones.CheckDbl(txtMAIEdit.Text); //FALLAS PROY-140546

                TextBox txtCMEdit = (TextBox)e.Row.FindControl("txtCMEdit");
                txtCMEdit.Text = CalcularCM(dblTotalCostoInstalacionHFC, nMAI2, nNumCuotas2).ToString(); //FALLAS PROY-140546
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
                TextBox txtFormaPago1 = (TextBox)e.Row.FindControl("txtFormaPago1");
                TextBox txtCuotaPago1 = (TextBox)e.Row.FindControl("txtCuotaPago1");
                //PROY-29215 FIN

                //PROY-140546 Cobro Anticipado de Instalacion
                txtFormaPago1.Text = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["FORMA_PAGO"]); ;
                txtFormaPago1.ReadOnly = true;
                txtCuotaPago1.Text = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["NRO_CUOTA"]); ;
                txtCuotaPago1.ReadOnly = true;

                TextBox txtMAI = (TextBox)e.Row.FindControl("txtMAINoEdit");
                txtMAI.Text = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["MAI"]);
                double nMAI = Funciones.CheckDbl(txtMAI.Text);

                int nNumCuotas = Funciones.CheckInt(txtCuotaPago1.Text);
                TextBox txtCM = (TextBox)e.Row.FindControl("txtCMNoEdit");
                txtCM.Text = CalcularCM(dblTotalCostoInstalacionHFC1, nMAI, nNumCuotas).ToString();

                TextBox txtFormaPago2 = (TextBox)e.Row.FindControl("txtFormaPago2");
                TextBox txtCuotaPago2 = (TextBox)e.Row.FindControl("txtCuotaPago2");

                txtFormaPago2.Text = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["FORMA_PAGO_MAN"]); ;
                txtFormaPago2.ReadOnly = true;
                txtCuotaPago2.Text = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["NRO_CUOTA_MAN"]); ;
                txtCuotaPago2.ReadOnly = true;

                TextBox txtMAI2 = (TextBox)e.Row.FindControl("txtMAINoEdit2");
                txtMAI2.Text = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["MAI_MAN"]);
                double nMAI2 = Funciones.CheckDbl(txtMAI2.Text);

                int nNumCuotas2 = Funciones.CheckInt(txtCuotaPago2.Text);

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

        public void grabar()
        {
            List<string> objEvaluacion = new List<string>();

            objLog.CrearArchivolog("[Grabar] | INC000003135879 - INICIO", null, null);
            objLog.CrearArchivolog(string.Format("{0}{1}", "[Grabar] | INC000003135879 - hidFlgSecRechazada: ", hidFlgSecRechazada.Value.ToString()), null, null);

            if (hidFlgSecRechazada.Value == "S")
            {
                string flagReingresoSec = chkReingresoSec.Checked ? "1" : "";
                new BLSolicitud().ActualizarReingresoSEC(Funciones.CheckInt64(txtNroSEC.Text), flagReingresoSec);
            }
            else
            {
                objEvaluacion = new List<string>();
                objEvaluacion.Add(txtNroSEC.Text);
                objEvaluacion.Add(hidFlgConvergente.Value);
                objEvaluacion.Add(Funciones.CheckStr(hidGarantia.Value));
                objEvaluacion.Add(Funciones.CheckStr(hidCostoInstalacion.Value));
                objEvaluacion.Add(CurrentUser);

                objLog.CrearArchivolog("[Grabar] | INC000003135879 - INICIO ActualizarGarantia", null, null);
                new BLSolicitud().ActualizarGarantia(objEvaluacion);
                objLog.CrearArchivolog("[Grabar] | INC000003135879 - FIN ActualizarGarantia", null, null);

                //PROY-29215 INICIO

                if (Funciones.CheckStr(hidFlgProductoDTH.Value) == "S" || Funciones.CheckStr(hidFlgProductoHFC.Value) == "S")
                {
                    // INC000003135879 INICIO
                    string stconvertirmontoinstalacion = hidCostoInstalacion.Value;
                    int pos00 = stconvertirmontoinstalacion.IndexOf(";"); // Campo: SEC
                    int pos01 = stconvertirmontoinstalacion.IndexOf("|", pos00 + 1, (stconvertirmontoinstalacion.Length - (pos00 + 1))); // Campo: CostoInstalacion
                    dblCostoInstalacionModifica = Funciones.CheckDbl(stconvertirmontoinstalacion.Substring(pos00 + 1, pos01 - (pos00 + 1)));

                    objLog.CrearArchivolog("[Grabar] | INC000003135879 - INICIA Auditoria CostoInstalacion", null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[Grabar] | INC000003135879 - hidCostoInstalacionAnterior: ", Funciones.CheckStr(hidCostoInstalacionAnterior.Value)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[Grabar] | INC000003135879 - dblCostoInstalacionModifica: ", Funciones.CheckStr(dblCostoInstalacionModifica)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[Grabar] | INC000003135879 -  CurrentUserSession.idCuentaRed: ", Funciones.CheckStr(CurrentUserSession.idCuentaRed.ToString())), null, null);

                    double CostoInstalacionAnterior = Funciones.CheckDbl(hidCostoInstalacionAnterior.Value);
                    double CostoInstalacionModifica = dblCostoInstalacionModifica;
                    string strUsuario = CurrentUserSession.idCuentaRed.ToString();

                    new BLConsumer().GrabarAuditoriaCostoInstalacionCreditos(Funciones.CheckInt64(txtNroSEC.Text), CostoInstalacionAnterior, CostoInstalacionModifica, Funciones.CheckStr(strUsuario));
                    hidCostoInstalacionAnterior.Value = "0";
                    objLog.CrearArchivolog("[Grabar] | INC000003135879 - FINALIZA Auditoria CostoInstalacion", null, null);
                    // INC000003135879 FIN 

                    string nroCuota = hidCuota.Value;
                    string formapago = hidFormaPago.Value.ToUpper();
                    string formapagoActual = hidFormaPagoActual.Value.ToUpper();
                    string cuotaActual = hidCuotaActual.Value;
                    string Asesor = Funciones.CheckStr(CurrentUserSession.Nombre + " " + CurrentUserSession.Apellido_Pat + " " + CurrentUserSession.Apellido_Mat);

                    if (formapagoActual != formapago || cuotaActual != Funciones.CheckStr(nroCuota))
                    {
                        new BLConsumer().GrabarFormaPagoCuota(Funciones.CheckInt64(txtNroSEC.Text), Funciones.CheckInt16(nroCuota), formapago);

                        objLog.CrearArchivolog("[GrabarFormaPagoCuota] INICIO", null, null);
                        if (formapagoActual != formapago & cuotaActual == Funciones.CheckStr(nroCuota))
                        {
                            objLog.CrearArchivolog(string.Format("{0}{1}", "[GrabarFormaPagoCuota] | NOMBRE DEL ASESOR: ", Asesor), null, null);
                            objLog.CrearArchivolog(string.Format("{0}{1}", "[GrabarFormaPagoCuota] | FORMA PAGO: ", formapago), null, null);

                        }
                        else if (cuotaActual != Funciones.CheckStr(nroCuota) & formapagoActual == formapago)
                        {
                            objLog.CrearArchivolog(string.Format("{0}{1}", "[GrabarFormaPagoCuota] | NOMBRE DEL ASESOR: ", Asesor), null, null);
                            objLog.CrearArchivolog(string.Format("{0}{1}", "[GrabarFormaPagoCuota] | NRO. CUOTAS: ", nroCuota.ToString()), null, null);
                        }
                        else if (formapagoActual != formapago & cuotaActual != Funciones.CheckStr(nroCuota))
                        {
                            objLog.CrearArchivolog(string.Format("{0}{1}", "[GrabarFormaPagoCuota] | NOMBRE DEL ASESOR: ", Asesor), null, null);
                            objLog.CrearArchivolog(string.Format("{0}{1}", "[GrabarFormaPagoCuota] | FORMA PAGO: ", formapago), null, null);
                            objLog.CrearArchivolog(string.Format("{0}{1}", "[GrabarFormaPagoCuota] | NRO. CUOTAS: ", nroCuota.ToString()), null, null);
                        }

                    }

                    //PROY-140546 Cobro Anticipado de Instalacion Inicio
                    if (Funciones.CheckStr(hidFlgProductoHFC.Value) == "S")
                    {
                        ActualizaMontoAnticipadoInstalacion(txtNroSEC.Text, hidPdvSEC.Value, hidMAI.Value, hidMAIOriginal.Value);
                        RegistraHistorialPagoAnticipado(Funciones.CheckInt64(txtNroSEC.Text), CurrentUser, CostoInstalacionModifica, formapago, Funciones.CheckInt(nroCuota), Funciones.CheckDbl(hidMAI.Value));
                    }
                    //PROY-140546 Cobro Anticipado de Instalacion Fin
                }

                //PROY-29215 FIN
            }

            Script("alert('Se grabó satisfactoriamente los cambios en la SEC: " + txtNroSEC.Text + "');");
        }

        //PROY-140546 Cobro Anticipado de Instalacion Inicio
        public DataTable ObtenerDatosCostoInstalacion(Int64 pNroSec)
        {
            DataTable dtResultado = new DataTable();

            GeneradorLog objlog = new GeneradorLog(CurrentUsers, null, null, "WEB");
            objlog.CrearArchivolog("PROY-140546|sisact_edicion_garantia|ObtenerDatosCostoInstalacion|-- INICIO --", null, null);
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

                string host = ReadKeySettings.ConsCurrentIP;
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
                objlog.CrearArchivolog(string.Format("{0} --> {1}", "[PROY-140546][ObtenerDatosCostoInstalacionGar]", " ERROR[" + e.Message + "|" + e.StackTrace + "]"), null, null);
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

            objlog.CrearArchivolog("- hidFlgSecRechazada.Value -> " + Funciones.CheckStr(hidFlgSecRechazada.Value), null, null);

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

            objlog.CrearArchivolog("- dtResultado.Rows.Count -> " + Funciones.CheckStr(dtResultado.Rows.Count), null, null);

            foreach (DataRow item in dtResultado.Rows)
            {
                item["FORMA_PAGO"] = formaPago;
                item["FORMA_PAGO_MAN"] = formaPagoMan;
                item["NRO_CUOTA"] = nroCuota;
                item["NRO_CUOTA_MAN"] = nroCuotaMan;
                item["MAI"] = mai;
                item["MAI_MAN"] = maiMan;

                hidMAIOriginal.Value = Funciones.CheckStr(mai);
            }

            objlog.CrearArchivolog("PROY-140546|sisact_edicion_garantia|ObtenerDatosCostoInstalacion|-- FIN --", null, null);
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

        public bool ActualizaMontoAnticipadoInstalacion(string pNroSEC, string pOvenc_Codigo, string pMAI, string pMAIOriginal)
        {
            GeneradorLog _objLog = CrearLogStatic(pNroSEC);
            bool bResultado = false;
            try
            {
                var oListConsultaDatosOficina = BLSincronizaSap.ConsultaDatosOficina(Funciones.CheckStr(pOvenc_Codigo), null);
                var canalPDV = oListConsultaDatosOficina.TipoOficina;

                bool EsCanalPermitido = Funciones.EsValorPermitido(canalPDV, ReadKeySettings.Key_CanalesPermitidosCAI, ",");
                bool bCambioMAI = (Funciones.CheckDbl(pMAI) != Funciones.CheckDbl(pMAIOriginal) ? true : false);

                if (EsCanalPermitido && bCambioMAI)
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
                    oBeanRequest.numeroSolicitud = Funciones.CheckInt64(pNroSEC);
                    oBeanRequest.usuarioActualizacion = CurrentUsers;
                    oBeanRequest.montoInicialModificado = pMAI;

                    oListaRequest.Add(oBeanRequest);

                    oMessageRequest.Body = new Entity.ClientePagoAnticipadoFijaRest.DataPower.ActualizaPA.ActualizaPABody();
                    oMessageRequest.Body.actualizaPARequest = new Entity.ClientePagoAnticipadoFijaRest.DataPower.ActualizaPA.ActualizaPARequest();
                    oMessageRequest.Body.actualizaPARequest.actualizaPARequestType = oListaRequest;

                    oGenericRequest.MessageRequest = oMessageRequest;

                    bResultado = oService.ActualizarPagoAnticipado(oGenericRequest);
                }

            }
            catch (Exception ex)
            {
                bResultado = false;
                _objLog.CrearArchivolog("[ERROR METODO ActualizaMontoAnticipadoInstalacion]", null, ex);
            }
            return bResultado;
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
            oRequest.costoInstalacionManual = costoInstMan;
            oRequest.formaPago = "";
            oRequest.formaPagoManual = formaPagoMan;
            oRequest.numeroCuotas = 0;
            oRequest.numeroCuotasManual = numCuotaMan;
            oRequest.montoAnticipadoInstalacion = 0;
            oRequest.montoAnticipadoInstalacionManual = montoAntiInst;
            oRequest.usuarioActualizacion = usuario;
            oRequest.grupoSolicitud = 0;
            oRequest.puntoVenta = "";

            oBLPagoAnticipado.RegistraHistorialPagoAnticipado(oRequest);
        }
        //PROY-140546 Cobro Anticipado de Instalacion Fin
    }
}
