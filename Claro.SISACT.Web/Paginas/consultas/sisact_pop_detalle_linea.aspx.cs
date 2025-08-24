using System;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Claro.SISACT.Business;
using Claro.SISACT.Common;
using Claro.SISACT.Entity;
using Claro.SISACT.WS;
using Claro.SISACT.Web.Base; //PROY-140126 - IDEA 140248 
using Claro.SISACT.Entity.VentasCuotas.ObtenerDatosPedidoAccCuotas.Response; //PROY-140126 - IDEA 140248 

namespace Claro.SISACT.Web.Paginas.consultas
{
    public partial class sisact_pop_detalle_linea : Sisact_Webbase //PROY-140126 - IDEA 140248  
    {
        #region [Declaracion de Constantes - Config]

        String tipoDocumento;
        String nroDocumento;
        String constCodTipoDocRUC = ConfigurationManager.AppSettings["TipoDocumentoRUC"].ToString();
        DataTable dtDetalleBSCS;
        DataTable dtDetalleSGA;
        DataTable dtDetalleSISACT;
        DataTable dtDetalleAccesorios;//PROY-140743
        List<BEObtenerDatosPedidoAccCuotas> DetalleAccesorios;//PROY-140743

        #endregion [Declaracion de Constantes - Config]

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

            BEClienteCuenta objCliente = null;
            //this.tipoDocumento = Request.QueryString["tipoDocumento"];
            this.nroDocumento = (string) HttpContext.Current.Session["docCliente"]; 
            int intCPnulo = Convert.ToInt32(ConfigurationManager.AppSettings["consCPnulo"]);
            //PROY-29121-INI
            BEUsuarioSession ojbUsuario = new BEUsuarioSession();
            ojbUsuario = (BEUsuarioSession)HttpContext.Current.Session["Usuario"];
            //PROY-29121-FIN

            objCliente = (BEClienteCuenta)HttpContext.Current.Session["objCliente" + nroDocumento];
            CrearLogStatic("").CrearArchivolog(string.Format("{0}{1}", "[INC000003443673 - sisact_pop_detalle_linea - Page_Load() - objCliente.nombres: ", Funciones.CheckStr(objCliente.nombres)), null, null);
            CrearLogStatic("").CrearArchivolog(string.Format("{0}{1}", "[INC000003443673 - sisact_pop_detalle_linea - Page_Load() - objCliente.apellidoPaterno: ", Funciones.CheckStr(objCliente.apellidoPaterno)), null, null);
            CrearLogStatic("").CrearArchivolog(string.Format("{0}{1}", "[INC000003443673 - sisact_pop_detalle_linea - Page_Load() - objCliente.apellidoMaterno: ", Funciones.CheckStr(objCliente.apellidoMaterno)), null, null);

            GeneradorLog _objLog = new GeneradorLog("", nroDocumento, null, "WEB_DetalleLinea");
            _objLog.CrearArchivolog("[Inicio][DetalleLinea]", null, null);

            _objLog.CrearArchivolog("HttpContext.Current.Session[docCliente]=>" + Funciones.CheckStr(HttpContext.Current.Session["docCliente"]), null, null);

            BEClienteCuenta objClienteConsulta = new BEClienteCuenta();
            objClienteConsulta = (BEClienteCuenta)HttpContext.Current.Session["objCliente" + nroDocumento];
            _objLog.CrearArchivolog("objClienteConsulta.tipoDoc =>" + Funciones.CheckStr(objClienteConsulta.tipoDoc), null, null);
            _objLog.CrearArchivolog("objClienteConsulta.nroDoc =>" + Funciones.CheckStr(objClienteConsulta.nroDoc), null, null);

            CrearLogStatic("").CrearArchivolog(string.Format("{0}{1}", "[INC000003443673 - sisact_pop_detalle_linea - Page_Load() - origen: ", Funciones.CheckStr(Request.QueryString["origen"])), null, null);
            if (Request.QueryString["origen"] == "P")
                objCliente = (new BLDatosCliente()).ConsultarDatosCliente(Claro.SISACT.Web.Base.Sisact_Webbase.CurrentNodo, Claro.SISACT.Web.Base.Sisact_Webbase.CurrentUsers, Funciones.CheckStr(objCliente.tipoDoc), Funciones.CheckStr(objCliente.nroDoc), intCPnulo, Comun.AppSettings.consAntiguedadDeuda, Comun.AppSettings.consFlagFlexibilidad, ojbUsuario, false);//PROY-29121//PROY-26963 - GPRD - PROMFACT //PROY-32439
            //else
            //    objCliente = (BEClienteCuenta)HttpContext.Current.Session["objCliente" + nroDocumento];

            Mostrar(objCliente);
        }

        private void Mostrar(BEClienteCuenta objCliente)
        {
            // INI PROY-26963 - GPRD - PROMFACT
            GeneradorLog _objLog = new GeneradorLog("", nroDocumento, null, "WEB_DetalleLinea");
            _objLog.CrearArchivolog("[Inicio][Mostrar]", null, null);
//FIN PROY-26963 - GPRD - PROMFACT

            gvListaBSCS.DataSource = null;
            gvListaSGA.DataSource = null;
            gvListaSISACT.DataSource = null;
            gvListaPrepago.DataSource = null;
            gvDetalleAccesorios.DataSource = null;//PROY-140743

            if (objCliente != null)
            {
                dtDetalleSGA = objCliente.lineaSGA;
                dtDetalleBSCS = objCliente.lineaBSCS;
                dtDetalleSISACT = objCliente.lineaSISACT;

                DataTable dtListaBSCS = new DataTable();
                DataTable dtListaSGA = new DataTable();
                DataTable dtListaSISACT = new DataTable();
                DataTable dtListaPendienteSISACT = new DataTable();// PROY-26963 - GPRD - PROMFACT
                DataTable dtDListaDetalleAccesorios = new DataTable();//PROY-140743

                WS.WSOAC.DetalleCuentaType[] objDetalleOAC = null;
                if (objCliente.oOAC != null && objCliente.oOAC.Count > 0)
                    objDetalleOAC = (WS.WSOAC.DetalleCuentaType[])objCliente.oOAC[0];

                _objLog.CrearArchivolog("[INC000003443673 - sisact_pop_detalle_linea - Mostrar() - objCliente.tipoDoc:] " + objCliente.tipoDoc, null, null);
                _objLog.CrearArchivolog("[INC000003443673 - sisact_pop_detalle_linea - Mostrar() - objCliente.nroDoc:] " + objCliente.nroDoc, null, null);
                _objLog.CrearArchivolog("[INC000003443673 - sisact_pop_detalle_linea - Mostrar() - objCliente.razonSocial: ] " + objCliente.razonSocial, null, null);
                _objLog.CrearArchivolog("[INC000003443673 - sisact_pop_detalle_linea - Mostrar() - objCliente.nombres:] " + objCliente.nombres, null, null);
                _objLog.CrearArchivolog("[INC000003443673 - sisact_pop_detalle_linea - Mostrar() - objCliente.apellidos:] " + objCliente.apellidos, null, null);

                txtNroDocumento.Text = objCliente.nroDoc;
                txtTipoDocumento.Text = objCliente.tipoDocDes;
                txtCantidadPlan.Text = objCliente.nroPlanesActivos.ToString();
                txtCargoFijo.Text = string.Format("{0:#,#,#,0.00}", objCliente.CF_Total);
                txtDeudaVencida.Text = string.Format("{0:#,#,#,0.00}", objCliente.deudaVencida);
                txtDeudaCastigada.Text = string.Format("{0:#,#,#,0.00}", objCliente.deudaCastigada);
                txtNombre.Text = (objCliente.tipoDoc == constCodTipoDocRUC) ? objCliente.razonSocial : objCliente.nombres + " " + objCliente.apellidos;
                txtBloqueo.Text = (objCliente.nroBloqueo > 0 ? "SI" : "NO").ToString();
                txtSuspension.Text = (objCliente.nroSuspension > 0 ? "SI" : "NO").ToString();
                txtTipoDocumentoAnexo.Text = ConfigurationManager.AppSettings["TipoDocuIdentidad"].ToString();
                txtNroDocumentoAnexo.Text = objCliente.nroDocAsociado;

                trAnexoDocumento.Visible = false;
                trCantLineaMayorMenor.Visible = false;
                if (objCliente.tipoDoc == constCodTipoDocRUC)
                {
                    trCantLineaMayorMenor.Visible = true;
                    if (objCliente.nroDoc.Substring(0, 1) != ConfigurationManager.AppSettings["constRUCInicio"])
                        trAnexoDocumento.Visible = true;
                }

                lblCantLineaMenor.Text = objCliente.nroRangoDiasBSCS.ToString();
                lblCantLineaMayor.Text = objCliente.nroRangoDiasBSCS.ToString();
                txtCantBloqueoMovil.Text = objCliente.nroBloqueo.ToString();
                txtCantSuspMovil.Text = objCliente.nroSuspension.ToString();
                txtCantLineaMenor.Text = objCliente.nroLineaMenor90Dia.ToString();
                txtCantLineaMayor.Text = objCliente.nroLineaMayor90Dia.ToString();

// INI PROY-26963 - GPRD - PROMFACT
                _objLog.CrearArchivolog("[Inicio][createTableLineas]", null, null);

                createTableLineas(objCliente.nroDoc, objCliente.tipoDoc, objDetalleOAC, objCliente.lineaBSCS, objCliente.lineaSGA, objCliente.DatosPedidoAccCuotas, ref dtListaBSCS, ref dtListaSGA, ref dtDListaDetalleAccesorios); //PROY-26963  objCliente.tipoDoc //PROY-140743

                _objLog.CrearArchivolog("[FIN][createTableLineas]", null, null);

                // createTableLineasSISACT(ref dtListaSISACT, objCliente.lineaSISACT);//PROY-26963_F1 - GPRD

                string TipoEvalAlta = "P";//PROY-26963 - GPRD

                //EVALUACIONES CON VENTA=>V
                //EVALUACIONES SIN VENTA=>P
                _objLog.CrearArchivolog("[Inicio][createTableLineasSISACT1]", null, null);
                createTableLineasSISACT(ref dtListaSISACT, objCliente.lineaSISACT, TipoEvalAlta);//PROY-26963_F1 - GPRD
                _objLog.CrearArchivolog("[FIN][createTableLineasSISACT1]", null, null);
                _objLog.CrearArchivolog("[INICIO][createTableLineasSISACT2]", null, null);
                createTableLineasSISACT(ref dtListaPendienteSISACT, objCliente.lineaSISACT);//PROY-26963_F1 - GPRD
                _objLog.CrearArchivolog("[FIN][createTableLineasSISACT2]", null, null);
                gvListaBSCS.DataSource = dtListaBSCS;
                gvListaSGA.DataSource = dtListaSGA;
                gvListaSISACT.DataSource = dtListaSISACT;//PROY-26963_F1 - GPRD
                gvListaPendienteSISACT.DataSource = dtListaPendienteSISACT;//PROY-26963_F1 - GPRD
                gvListaPrepago.DataSource = objCliente.lineaPrepago;
                gvDetalleAccesorios.DataSource = dtDListaDetalleAccesorios;//PROY-140743

                _objLog.CrearArchivolog("[FIN][Mostrar]", null, null);
            }

            gvListaBSCS.DataBind();
            _objLog.CrearArchivolog("[FIN][gvListaBSCS]", null, null);
            gvListaSGA.DataBind();
            _objLog.CrearArchivolog("[FIN][gvListaSGA]", null, null);
            gvListaSISACT.DataBind();
            _objLog.CrearArchivolog("[FIN][gvListaSISACT]", null, null);
            gvListaPendienteSISACT.DataBind();//PROY-26963 - GPRD
            _objLog.CrearArchivolog("[FIN][gvListaPendienteSISACT]", null, null);
            gvDetalleAccesorios.DataBind();//PROY-140743
            _objLog.CrearArchivolog("[FIN][gvDetalleAccesorios]", null, null);//PROY-140743
            gvListaPrepago.DataBind();
            _objLog.CrearArchivolog("[FIN][gvListaPrepago]", null, null);
// FIN PROY-26963 - GPRD - PROMFACT
        }

        protected void gvListaBSCS_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblCodCuenta = (Label)e.Row.FindControl("lblCodCuenta");
                Image imgMostrarDetalle = (Image)e.Row.FindControl("btnMostrarDetalle");

                if (tipoDocumento != constCodTipoDocRUC)
                {
                    imgMostrarDetalle.Attributes.Add("onclick", "mostrarDetalle('" + lblCodCuenta.Text + "')");
                    e.Row.Cells[14].Text = tableDetalleBSCS(lblCodCuenta.Text);
                }
                else
                    imgMostrarDetalle.Visible = false;
            }
        }

        protected void gvListaSGA_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblCodCuenta = (Label)e.Row.FindControl("lblCodCuenta");
                Image imgMostrarDetalle = (Image)e.Row.FindControl("btnMostrarDetalle");

                if (tipoDocumento != constCodTipoDocRUC)
                {
                    imgMostrarDetalle.Attributes.Add("onclick", "mostrarDetalle('" + lblCodCuenta.Text + "')");
                    e.Row.Cells[14].Text = tableDetalleSGA(lblCodCuenta.Text);
                }
                else
                    imgMostrarDetalle.Visible = false;
            }
        }
        // INI PROY-26963 - GPRD - PROMFACT
        protected void gvListaPendienteSISACT_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblCodCuenta = (Label)e.Row.FindControl("lblCodCuenta");
                Image imgMostrarDetalle = (Image)e.Row.FindControl("btnMostrarDetalle");

                imgMostrarDetalle.Attributes.Add("onclick", "mostrarDetalle('" + lblCodCuenta.Text + "')");
                e.Row.Cells[14].Text = tableDetalleSISACT(lblCodCuenta.Text);
            }
        }
        // FIN PROY-26963 - GPRD - PROMFACT
        protected void gvListaSISACT_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblCodCuenta = (Label)e.Row.FindControl("lblCodCuenta");
                Image imgMostrarDetalle = (Image)e.Row.FindControl("btnMostrarDetalle");

                imgMostrarDetalle.Attributes.Add("onclick", "mostrarDetalle('" + lblCodCuenta.Text + "')");
                e.Row.Cells[14].Text = tableDetalleSISACT(lblCodCuenta.Text);
            }
        }
        //PROY-140743 INI
        protected void gvDetalleAccesorios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblCodCuenta = (Label)e.Row.FindControl("lblCodCuenta");
                e.Row.Cells[14].Text = tableDetalleAccesorios(lblCodCuenta.Text);
            }
        }
        //PROY-140743 FIN
        private string tableDetalleBSCS(string id)
        {
            StringBuilder strTableHTML = new StringBuilder();

            strTableHTML.Append("<tr style='display:none' id='" + id + "'>");
            strTableHTML.Append("<td colspan='100%'>");
            strTableHTML.Append("<table align='center' width='85%' cellspacing='0' rules='all' border='1' style='border-collapse:collapse;Z-INDEX: 0'>");

            strTableHTML.Append("<tr class='TablaTitulos' nowrap='nowrap' align='Center'>");
            strTableHTML.Append("<td>Plan / Solucion</td>");
            strTableHTML.Append("<td>Número</td>");
            strTableHTML.Append("<td>Productos / servicios</td>");
            strTableHTML.Append("<td>Cargo Fijo</td>");
            strTableHTML.Append("<td>Fecha Activación</td>");
            strTableHTML.Append("<td>Fecha Estado</td>");
            strTableHTML.Append("<td>Estado</td>");
            strTableHTML.Append("<td>Motivo Bloqueo</td>");
            strTableHTML.Append("<td>Motivo Suspensión</td>");
            strTableHTML.Append("<td>Campaña</td>");
            strTableHTML.Append("</tr>");

            StringBuilder strTableContentID = new StringBuilder();

            if (dtDetalleBSCS != null)
            {
                foreach (DataRow dr in dtDetalleBSCS.Rows)
                {
                    if (Funciones.CheckStr(dr["CUENTA"]).Equals(id))
                    {
                        if (((Funciones.CheckStr(dr["ESTADO"]).ToUpper().Equals("DESACTIVO") || Funciones.CheckStr(dr["ESTADO"]).ToUpper().Equals("INACTIVO") || Funciones.CheckStr(dr["ESTADO"]).ToUpper().Equals("SUSPENDIDO"))) || Funciones.CheckStr(dr["ESTADO"]).ToUpper().Equals("ACTIVO")) // INICIATIVA-219
                        {
                        strTableContentID.Append("<tr class='Arial10B' align='Center'>");
                        strTableContentID.Append("<td>" + Funciones.CheckStr(dr["PLAN"]) + "</td>");

                        string numero = Funciones.CheckStr(dr["NUMERO"]);
                        if (numero.Length > 6) numero = numero.Substring(0, numero.Length - 3) + "xxx"; 

                        strTableContentID.Append("<td>" + numero + "</td>");
                        strTableContentID.Append("<td><div style='word-wrap:break-word;width:160px'>" + Funciones.CheckStr(dr["PRODUCTO/SERVICIO"]) + "</div></td>");
                        strTableContentID.Append("<td>" + string.Format("{0:#,#,#,0.00}", Funciones.CheckDbl(dr["CF_CONTRATO"], 2)) + "</td>");
                        strTableContentID.Append("<td>" + Funciones.CheckStr(dr["FECHA_ACTIVACION"]) + "</td>");
                        strTableContentID.Append("<td>" + Funciones.CheckStr(dr["FECHA_ESTADO"]) + "</td>");
                        strTableContentID.Append("<td>" + Funciones.CheckStr(dr["ESTADO"]) + "</td>");
                        strTableContentID.Append("<td>" + Funciones.CheckStr(dr["MOT_BLOQ"]) + "</td>");
                        strTableContentID.Append("<td>" + Funciones.CheckStr(dr["MOT_SUSP"]) + "</td>");
                        strTableContentID.Append("<td>" + Funciones.CheckStr(dr["CAMPANA"]) + "</td>");
                        strTableContentID.Append("</tr>");
                    }
                }
            }
            }

            strTableHTML.Append(strTableContentID);
            strTableHTML.Append("</table>");
            strTableHTML.Append("<br/>");
            strTableHTML.Append("</td>");
            strTableHTML.Append("</tr>");

            return strTableHTML.ToString();
        }

        private string tableDetalleSGA(string id)
        {
            StringBuilder strTableHTML = new StringBuilder();

            strTableHTML.Append("<tr style='display:none' id='" + id + "'>");
            strTableHTML.Append("<td colspan='100%'>");
            strTableHTML.Append("<table align='center' width='85%' cellspacing='0' rules='all' border='1' style='border-collapse:collapse;Z-INDEX: 0'>");

            strTableHTML.Append("<tr class='TablaTitulos' nowrap='nowrap' align='Center'>");
            strTableHTML.Append("<td>Plan / Solucion</td>");
            strTableHTML.Append("<td>Número</td>");
            strTableHTML.Append("<td>Productos / servicios</td>");
            strTableHTML.Append("<td>Cargo Fijo</td>");
            strTableHTML.Append("<td>Fecha Activación</td>");
            strTableHTML.Append("<td>Fecha Estado</td>");
            strTableHTML.Append("<td>Estado</td>");
            strTableHTML.Append("<td>Motivo Bloqueo</td>");
            strTableHTML.Append("<td>Motivo Suspensión</td>");
            strTableHTML.Append("<td>Campaña</td>");
            strTableHTML.Append("</tr>");

            StringBuilder strTableContentID = new StringBuilder();

            if (dtDetalleSGA != null)
            {
                foreach (DataRow dr in dtDetalleSGA.Rows)
                {
                    if (Funciones.CheckStr(dr["CODCLI"]).Equals(id))
                    {
                        if (((Funciones.CheckStr(dr["ESTADO"]).ToUpper().Equals("DESACTIVO") || Funciones.CheckStr(dr["ESTADO"]).ToUpper().Equals("INACTIVO") || Funciones.CheckStr(dr["ESTADO"]).ToUpper().Equals("SUSPENDIDO")) && (Funciones.CheckDbl(dr["MONTO_VENCIDO"], 2) > 0 || Funciones.CheckDbl(dr["MONTO_CASTIGO"], 2) > 0)) || Funciones.CheckStr(dr["ESTADO"]).ToUpper().Equals("ACTIVO"))
                        {
                        strTableContentID.Append("<tr class='Arial10B' align='Center'>");
                        strTableContentID.Append("<td>" + Funciones.CheckStr(dr["PAQUETE"]) + "</td>");
                        strTableContentID.Append("<td>&nbsp;</td>");
                        strTableContentID.Append("<td><div style='word-wrap:break-word;width:160px'>" + Funciones.CheckStr(dr["SERVICIO"]) + "</div></td>");
                        strTableContentID.Append("<td>" + string.Format("{0:#,#,#,0.00}", Funciones.CheckDbl(dr["MONTO_CR"], 2)) + "</td>");
                        strTableContentID.Append("<td>" + Funciones.CheckStr(dr["FECHA_ACTIVACION"]) + "</td>");
                        strTableContentID.Append("<td>" + Funciones.CheckStr(dr["FECHA_ACTIVACION"]) + "</td>");
                        strTableContentID.Append("<td>" + Funciones.CheckStr(dr["ESTADO"]).ToLower() + "</td>");
                        strTableContentID.Append("<td>" + "" + "</td>");
                        strTableContentID.Append("<td>" + "" + "</td>");
                        strTableContentID.Append("<td>" + Funciones.CheckStr("") + "</td>");
                        strTableContentID.Append("</tr>");
                    }
                }
            }
            }

            strTableHTML.Append(strTableContentID);
            strTableHTML.Append("</table>");
            strTableHTML.Append("<br/>");
            strTableHTML.Append("</td>");
            strTableHTML.Append("</tr>");

            return strTableHTML.ToString();
        }

        private string tableDetalleSISACT(string id)
        {
            StringBuilder strTableHTML = new StringBuilder();

            strTableHTML.Append("<tr style='display:none' id='" + id + "'>");
            strTableHTML.Append("<td colspan='100%'>");
            strTableHTML.Append("<table align='center' width='85%' cellspacing='0' rules='all' border='1' style='border-collapse:collapse;Z-INDEX: 0'>");

            strTableHTML.Append("<tr class='TablaTitulos' nowrap='nowrap' align='Center'>");
            strTableHTML.Append("<td>Plan / Solucion</td>");
            strTableHTML.Append("<td>Número</td>");
            strTableHTML.Append("<td>Productos / servicios</td>");
            strTableHTML.Append("<td>Cargo Fijo</td>");
            strTableHTML.Append("<td>Fecha Activación</td>");
            strTableHTML.Append("<td>Fecha Estado</td>");
            strTableHTML.Append("<td>Estado</td>");
            strTableHTML.Append("<td>Motivo Bloqueo</td>");
            strTableHTML.Append("<td>Motivo Suspensión</td>");
            strTableHTML.Append("<td>Campaña</td>");
            strTableHTML.Append("</tr>");

            StringBuilder strTableContentID = new StringBuilder();

            if (dtDetalleSISACT != null)
            {
                foreach (DataRow dr in dtDetalleSISACT.Rows)
                {
                    if (Funciones.CheckStr(dr["CUENTA"]).Equals(id))
                    {
                        strTableContentID.Append("<tr class='Arial10B' align='Center'>");
                        strTableContentID.Append("<td>" + Funciones.CheckStr(dr["PLAN"]) + "</td>");

                        string numero = Funciones.CheckStr(dr["TELEFONO"]);
                        if (numero.Length > 6) numero = numero.Substring(0, numero.Length - 3) + "xxx"; 

                        strTableContentID.Append("<td>" + numero + "</td>");
                        strTableContentID.Append("<td><div style='word-wrap:break-word;width:160px'>" + Funciones.CheckStr(dr["SERVICIO"]) + "</div></td>");
                        strTableContentID.Append("<td>" + string.Format("{0:#,#,#,0.00}", Funciones.CheckDbl(dr["CARGO_FIJO"], 2)) + "</td>");
                        strTableContentID.Append("<td>" + Funciones.CheckStr(dr["FECHA_ACTIVACION"]) + "</td>");
                        strTableContentID.Append("<td>" + Funciones.CheckStr(dr["FECHA_ACTIVACION"]) + "</td>");
                        strTableContentID.Append("<td>" + Funciones.CheckStr(dr["ESTADO"]) + "</td>");
                        strTableContentID.Append("<td>" + "" + "</td>");
                        strTableContentID.Append("<td>" + "" + "</td>");
                        strTableContentID.Append("<td>" + Funciones.CheckStr(dr["CAMPANA"]) + "</td>");
                        strTableContentID.Append("</tr>");
                    }
                }
            }

            strTableHTML.Append(strTableContentID);
            strTableHTML.Append("</table>");
            strTableHTML.Append("<br/>");
            strTableHTML.Append("</td>");
            strTableHTML.Append("</tr>");

            return strTableHTML.ToString();
        }

        private void createTableLineas(string numDoc, string TipoDoc, WS.WSOAC.DetalleCuentaType[] listaCuentas, DataTable dtListaBSCS, DataTable dtListaSGA, List<BEObtenerDatosPedidoAccCuotas> ObtenerDatosPedidoAccCuotas,
                                        ref DataTable dtLineaFormatBSCS, ref DataTable dtLineaFormatSGA, ref DataTable dtDatos) //PROY-26963 TipoDoc //PROY-140743
        {

            // INI PROY-26963 - GPRD - PROMFACT
            GeneradorLog _objLog = new GeneradorLog("", nroDocumento, null, "WEB_DetalleLinea");
            _objLog.CrearArchivolog("[INICIO][METODO createTableLineas]", null, null);
// FIN PROY-26963 - GPRD - PROMFACT
            estructuraTable(ref dtDatos);//PROY-140743
            estructuraTable(ref dtLineaFormatBSCS);
            estructuraTable(ref dtLineaFormatSGA);

            string strCuenta = string.Empty;
            if (dtListaBSCS != null)
            {
                foreach (DataRow drBscs in dtListaBSCS.Rows)
                {
                    // INICIO - JCFM - INC000000681349
                    int contEstadoActivo = 0;
                    int contEstadoSuspendido = 0;
                    int contEstadoDesactivo = 0;
                    int contEstadoInactivo = 0;
                    // FIN - JCFM - INC000000681349
                    if (drBscs["CUENTA"].ToString() != strCuenta)
                    {
                        strCuenta = drBscs["CUENTA"].ToString();

                        DataRow dr = dtLineaFormatBSCS.NewRow();
                        string strEstado = string.Empty;
                        double dblCF = 0.0;
                        double dblNroServicio = 0.0;
                        double dblPromFacturado = 0.0;
                        double dblDeudaReintegro = 0.0;
                        double dblDeudaVencida = 0.0;
                        double dblDeudaCastigada = 0.0;
                        double dblMontoNoFacturado = 0.0;
                        double dblNroBloqueos = 0.0;
                        double dblNroSuspensiones = 0.0;
                        string strCustomerID = string.Empty;
                        int contBscsMostrar;

                        //Consulta Deuda OAC
                        if (listaCuentas != null)
                        {
                            foreach (WS.WSOAC.DetalleCuentaType item in listaCuentas)
                            {
                                if (item.xTipoServicio == "MOVIL")
                                {
                                    if (item.xCodCuenta.Equals(strCuenta))
                                    {
                                        dblDeudaVencida = Funciones.CheckDbl(item.xDeudaVencida, 2);
                                        dblDeudaCastigada = Funciones.CheckDbl(item.xDeudaCastigada, 2);
                                        dblDeudaReintegro = Funciones.CheckDbl(dblDeudaReintegro, 2);
                                        break;
                                    }
                                }
                            }
                        }
                        contBscsMostrar = 0;
                        foreach (DataRow fila in dtListaBSCS.Rows)
                        {
                            if (strCuenta.Equals(fila["CUENTA"]))
                            {
                                if (((Funciones.CheckStr(fila["ESTADO"]).ToUpper().Equals("DESACTIVO") || Funciones.CheckStr(fila["ESTADO"]).ToUpper().Equals("INACTIVO") || Funciones.CheckStr(fila["ESTADO"]).ToUpper().Equals("SUSPENDIDO"))) || Funciones.CheckStr(fila["ESTADO"]).ToUpper().Equals("ACTIVO")) // INICIATIVA-219
                                {
                                strCustomerID = Funciones.CheckStr(fila["CUSTOMER_ID"]);
                                dblCF += Funciones.CheckDbl(fila["CF_CONTRATO"]);
                                dblPromFacturado = Funciones.CheckDbl(fila["PROM_FACT"]); // INICIATIVA-219 : += //INC000003538204 : =
                                dblMontoNoFacturado = Funciones.CheckDbl(fila["MONTO_NO_FACT"]); // INICIATIVA-219 : += //INC000003538204 : =
                                strEstado = Funciones.CheckStr(fila["ESTADO"]);
                                // INICIO - JCFM - INC000000681349
                                    switch (strEstado.ToUpper())
                                    {
                                        case "ACTIVO":
                                            contEstadoActivo++;
                                            break;
                                        case "SUSPENDIDO":
                                            contEstadoSuspendido++;
                                            break;
                                        case "DESACTIVO":
                                            contEstadoDesactivo++;
                                            break;
                                        case "INACTIVO":
                                            contEstadoInactivo++;
                                            break;
                                    }
                                    // FIN INC000000681349
                                    //PROY-26963 JCC-RF08
                                    if (TipoDoc == ConfigurationManager.AppSettings["TipoDocumentoRUC"].ToString())
                                    {
                                        dblNroServicio = Funciones.CheckDbl(fila["NRO_PLANES"]);
                                    }
                                    else
                                    {
                                    dblNroServicio = contEstadoActivo; // INICIO - JCFM - INC000000681349
                                    }
                                    //PROY-26963 JCC-RF08 
                                    //dblNroServicio = contEstadoActivo; // INICIO - JCFM - INC000000681349
                                    dblNroBloqueos += Funciones.CheckDbl(fila["NRO_BLOQ"]); // INICIATIVA-219 : +=
                                    dblNroSuspensiones += Funciones.CheckDbl(fila["NRO_SUSP"]); // INICIATIVA-219 : +=
                                    contBscsMostrar++;
                                }
                            }
                        }

                        dr["CODIGO_CUENTA"] = strCuenta;

                        // INICIO - JCFM - INC000000681349
                        if (contEstadoActivo > 0)
                        {
                            dr["ESTADO_CUENTA"] = ConfigurationManager.AppSettings["varEstadoAct"].ToString();
                        }
                        else if (contEstadoSuspendido > 0)
                        {
                            dr["ESTADO_CUENTA"] = ConfigurationManager.AppSettings["varEstadoSus"].ToString();
                        }
                        else if (contEstadoDesactivo > 0 )
                        {
                            dr["ESTADO_CUENTA"] = ConfigurationManager.AppSettings["varEstadoDes"].ToString();
                        }
                        else if (contEstadoInactivo > 0)
                        {
                            dr["ESTADO_CUENTA"] = ConfigurationManager.AppSettings["varEstadoIna"].ToString();
                        }
                        // FIN INC000000681349

                        dr["CUSTOMER_ID"] = strCustomerID;
                        dr["CENTRAL_RIESGO"] = "";
                        dr["CF"] = string.Format("{0:#,#,#,0.00}", Funciones.CheckDbl(dblCF, 2));
                        dr["PROMEDIO_FACTURADO"] = string.Format("{0:#,#,#,0.00}", Funciones.CheckDbl(dblPromFacturado, 2));
                        dr["MONTO_NO_FACTURADO"] = string.Format("{0:#,#,#,0.00}", Funciones.CheckDbl(dblMontoNoFacturado, 2));
                        dr["MONTO_VENCIDO"] = string.Format("{0:#,#,#,0.00}", Funciones.CheckDbl(dblDeudaVencida, 2));
                        dr["MONTO_CASTIGADO"] = string.Format("{0:#,#,#,0.00}", Funciones.CheckDbl(dblDeudaCastigada, 2));
                        dr["DEUDA_REINTEGRO"] = string.Format("{0:#,#,#,0.00}", Funciones.CheckDbl(dblDeudaReintegro, 2));
                        dr["CANTIDAD_SERVICIOS"] = dblNroServicio;
                        dr["CANTIDAD_BLOQUEOS"] = dblNroBloqueos;
                        dr["CANTIDAD_SUSPENDIDOS"] = dblNroSuspensiones;
                        if (contBscsMostrar != 0)
                        {
                        dtLineaFormatBSCS.Rows.Add(dr);
                    }
                }
            }
            }

            _objLog.CrearArchivolog("[INICIO][METODO createTableLineas_SGA]", null, null); // PROY-26963 - GPRD - PROMFACT

            strCuenta = string.Empty;
            if (dtListaSGA != null)
            {
                // INI PROY-26963 - GPRD - PROMFACT
                int contEstadoActivo = 0;
                int contEstadoSuspendido = 0;
                int contEstadoDesactivo = 0;
                int contEstadoInactivo = 0;
                _objLog.CrearArchivolog("[INICIO][Tiene datos]", null, null);
                _objLog.CrearArchivolog(String.Format("{0} {1}","[INICIO][Filas]",dtListaSGA.Rows.Count), null, null);
// FIN PROY-26963 - GPRD - PROMFACT

                foreach (DataRow drSGA in dtListaSGA.Rows)
                {
                    if (drSGA["CODCLI"].ToString() != strCuenta)
                    {

                        _objLog.CrearArchivolog("[INICIO][Codigo cliente diferente a cuenta]", null, null); // PROY-26963 - GPRD - PROMFACT

                        strCuenta = drSGA["CODCLI"].ToString();

                        DataRow dr = dtLineaFormatSGA.NewRow();
                        string strEstado = string.Empty;
                        double dblCF = 0.0;
                        double dblPromFacturado = 0.0;
                        double dblDeudaReintegro = 0.0;
                        double dblDeudaVencida = 0.0;
                        double dblDeudaCastigada = 0.0;
                        double dblMontoNoFacturado = 0.0;
                        double dblNroServicios = 0.0;
                        double dblNroBloqueos = 0.0;
                        double dblNroSuspensiones = 0.0;
                        string strCustomerID = string.Empty;
                        int contSgaMostrar;

                        //Consulta Deuda OAC
                        if (listaCuentas != null)
                        {
                            foreach (WS.WSOAC.DetalleCuentaType item in listaCuentas)
                            {
                                if (item.xTipoServicio == "FIJA")
                                {
                                    if (item.xCodCuenta.Equals(strCuenta))
                                    {
                                        dblDeudaVencida = Funciones.CheckDbl(item.xDeudaVencida, 2);
                                        dblDeudaCastigada = Funciones.CheckDbl(item.xDeudaCastigada, 2);
                                        dblDeudaReintegro = Funciones.CheckDbl(dblDeudaReintegro, 2);
                                        break;
                                    }
                                }
                            }
                        }
                        contSgaMostrar = 0;
                        foreach (DataRow fila in dtListaSGA.Rows)
                        {
                            if (strCuenta.Equals(fila["CODCLI"]))
                            {
                                if (((Funciones.CheckStr(fila["ESTADO"]).ToUpper().Equals("DESACTIVO") || Funciones.CheckStr(fila["ESTADO"]).ToUpper().Equals("INACTIVO") || Funciones.CheckStr(fila["ESTADO"]).ToUpper().Equals("SUSPENDIDO")) && (Funciones.CheckDbl(fila["MONTO_VENCIDO"], 2) > 0 || Funciones.CheckDbl(fila["MONTO_CASTIGO"], 2) > 0)) || Funciones.CheckStr(fila["ESTADO"]).ToUpper().Equals("ACTIVO"))
                                {
                                dblCF += Funciones.CheckDbl(fila["MONTO_CR"]);
// INI PROY-26963 - GPRD - PROMFACT
                                if (Funciones.CheckStr(fila["ESTADO"]).ToUpper().Equals("ACTIVO"))
                                {
                                dblPromFacturado += Funciones.CheckDbl(fila["PROM_FAC"]);
                                }
                                else {
                                    dblPromFacturado += 0;
                                }
// FIN PROY-26963 - GPRD - PROMFACT
                                dblMontoNoFacturado += Funciones.CheckDbl(fila["MONTO_NO_FAC"]);
                                //dblNroServicios += Funciones.CheckDbl(fila["CANT_SRV"]); // PROY-26963 - GPRD - PROMFACT
                                strEstado = Funciones.CheckStr(fila["ESTADO"]);
                                dblNroBloqueos += Funciones.CheckDbl(fila["CANTIDAD_BLOQ"]);
                                dblNroSuspensiones += Funciones.CheckDbl(fila["CANTIDAD_SUSP"]);
// INI PROY-26963 - GPRD - PROMFACT
                                strEstado = Funciones.CheckStr(fila["ESTADO"]);    
                                switch (strEstado.ToUpper())
                                {
                                    case "ACTIVO":
                                        contEstadoActivo++;
                                        break;
                                    case "SUSPENDIDO":
                                        contEstadoSuspendido++;
                                        break;
                                    case "DESACTIVO":
                                        contEstadoDesactivo++;
                                        break;
                                    case "INACTIVO":
                                        contEstadoInactivo++;
                                        break;
                                }
                                dblNroServicios = contEstadoActivo;
                                // FIN PROY-26963 - GPRD - PROMFACT
                                    contSgaMostrar++;
                                }
                            }
                        }

                        dr["CODIGO_CUENTA"] = strCuenta;

                        // INI PROY-26963 - GPRD - PROMFACT
                        if (contEstadoActivo > 0)
                        {
                            dr["ESTADO_CUENTA"] = ConfigurationManager.AppSettings["varEstadoAct"].ToString();
                        }
                        else if (contEstadoSuspendido > 0)
                        {
                            dr["ESTADO_CUENTA"] = ConfigurationManager.AppSettings["varEstadoSus"].ToString();
                        }
                        else if (contEstadoDesactivo > 0)
                        {
                            dr["ESTADO_CUENTA"] = ConfigurationManager.AppSettings["varEstadoDes"].ToString();
                        }
                        else if (contEstadoInactivo > 0)
                        {
                            dr["ESTADO_CUENTA"] = ConfigurationManager.AppSettings["varEstadoIna"].ToString();
                        }

                        //dr["ESTADO_CUENTA"] = strEstado;

                        // FIN PROY-26963 - GPRD - PROMFACT
                        dr["CUSTOMER_ID"] = strCustomerID;
                        dr["CENTRAL_RIESGO"] = "";
                        dr["CF"] = string.Format("{0:#,#,#,0.00}", Funciones.CheckDbl(dblCF, 2));
                        dr["PROMEDIO_FACTURADO"] = string.Format("{0:#,#,#,0.00}", Funciones.CheckDbl(dblPromFacturado, 2));
                        dr["MONTO_NO_FACTURADO"] = string.Format("{0:#,#,#,0.00}", Funciones.CheckDbl(dblMontoNoFacturado, 2));
                        dr["MONTO_VENCIDO"] = string.Format("{0:#,#,#,0.00}", Funciones.CheckDbl(dblDeudaVencida, 2));
                        dr["MONTO_CASTIGADO"] = string.Format("{0:#,#,#,0.00}", Funciones.CheckDbl(dblDeudaCastigada, 2));
                        dr["DEUDA_REINTEGRO"] = string.Format("{0:#,#,#,0.00}", Funciones.CheckDbl(dblDeudaReintegro, 2));
                        dr["CANTIDAD_SERVICIOS"] = dblNroServicios;
                        dr["CANTIDAD_BLOQUEOS"] = dblNroBloqueos;
                        dr["CANTIDAD_SUSPENDIDOS"] = dblNroSuspensiones;
                        if (contSgaMostrar != 0)
                        {
                        dtLineaFormatSGA.Rows.Add(dr);
                    }
// INI PROY-26963 - GPRD - PROMFACT
                        _objLog.CrearArchivolog(String.Format("{0} {1}", "[INICIO][Codigo cliente diferente a cuenta]", dr["CODIGO_CUENTA"]), null, null);
                        _objLog.CrearArchivolog(String.Format("{0} {1}", "[INICIO][Codigo cliente diferente a cuenta]", dr["ESTADO_CUENTA"]), null, null);
                        _objLog.CrearArchivolog(String.Format("{0} {1}", "[INICIO][Codigo cliente diferente a cuenta]", dr["CUSTOMER_ID"]), null, null);
                        _objLog.CrearArchivolog(String.Format("{0} {1}", "[INICIO][Codigo cliente diferente a cuenta]", dr["CANTIDAD_SUSPENDIDOS"]), null, null);
// FIN PROY-26963 - GPRD - PROMFACT
                }
            }
        }

            //PROY-140743 IDEA141192 NUEVAS VARIABLES BRMS VTACUO VICTOR CANCHICA 
            strCuenta = string.Empty;
            if (ObtenerDatosPedidoAccCuotas != null)
            {
                double dblDeudaReintegro = 0.0;
                double dblPromFacturado = 0;
                double dblMontoNoFacturado = 0;
                double dblDeudaVencida = 0;
                double dblDeudaCastigada = 0;
                double dblNroBloqueos = 0;
                double dblNroSuspensiones = 0;

                foreach (var item in ObtenerDatosPedidoAccCuotas)
                {
                    if (Funciones.CheckStr(item.descPlanFijo).Equals("X") && !string.IsNullOrEmpty(item.numeroPedido))
                    {
                        DataRow row = dtDatos.NewRow();
                        row["CF"] = item.cargoFijo;
                        row["CODIGO_CUENTA"] = strCuenta;
                        row["CODIGO_CUENTA"] = item.cuentaFacturar;
                        row["DEUDA_REINTEGRO"] = string.Format("{0:#,#,#,0.00}", Funciones.CheckDbl(dblDeudaReintegro, 2));
                        row["CUSTOMER_ID"] = item.customerID;
                        row["CENTRAL_RIESGO"] = "";

                        row["PROMEDIO_FACTURADO"] = string.Format("{0:#,#,#,0.00}", Funciones.CheckDbl(dblPromFacturado, 2));
                        row["MONTO_NO_FACTURADO"] = string.Format("{0:#,#,#,0.00}", Funciones.CheckDbl(dblMontoNoFacturado, 2));
                        row["MONTO_VENCIDO"] = string.Format("{0:#,#,#,0.00}", Funciones.CheckDbl(dblDeudaVencida, 2));
                        row["MONTO_CASTIGADO"] = string.Format("{0:#,#,#,0.00}", Funciones.CheckDbl(dblDeudaCastigada, 2));
                        row["DEUDA_REINTEGRO"] = string.Format("{0:#,#,#,0.00}", Funciones.CheckDbl(dblDeudaReintegro, 2));
                        row["CANTIDAD_SERVICIOS"] = 1;
                        row["CANTIDAD_BLOQUEOS"] = dblNroBloqueos;
                        row["CANTIDAD_SUSPENDIDOS"] = dblNroSuspensiones;

                        dtDatos.Rows.Add(row);
                    }                    
                }
            }
            //PROY-140743 IDEA141192 NUEVAS VARIABLES BRMS VTACUO VICTOR CANCHICA 

            _objLog.CrearArchivolog("[FIN][METODO createTableLineas_SGA]", null, null); // PROY-26963 - GPRD - PROMFACT
        }

        private void createTableLineasSISACT(ref DataTable dtLineaFormatSISACT, DataTable dtListaSISACT, string tipoEval = "V") // PROY-26963 - GPRD - PROMFACT
        {
            estructuraTable(ref dtLineaFormatSISACT);

            string strCuenta = "";
            if (dtListaSISACT != null)
            {
                foreach (DataRow drSISACT in dtListaSISACT.Rows)
                {
                    if (Funciones.CheckStr(drSISACT["CUENTA"]) != strCuenta && Funciones.CheckStr(drSISACT["TIPO_EVAL"]) == tipoEval)// PROY-26963 - GPRD - PROMFACT
                    {
                        strCuenta = Funciones.CheckStr(drSISACT["CUENTA"]);
                        double dblCF = 0;
                        double dblPromFacturado = 0;
                        double dblDeudaReintegro = 0;
                        double dblDeudaVencida = 0;
                        double dblDeudaCastigada = 0;
                        double dblMontoNoFacturado = 0;
                        double dblNroServicios = 0;
                        double dblNroBloqueos = 0;
                        double dblNroSuspensiones = 0;
                        string strCustomerID = String.Empty;

                        foreach (DataRow dr1 in dtListaSISACT.Rows)
                        {
                            if (Funciones.CheckStr(drSISACT["CUENTA"]) == Funciones.CheckStr(dr1["CUENTA"]))
                            {
                                dblNroServicios++;
                                dblCF += Funciones.CheckDbl(dr1["CARGO_FIJO"], 2);
                                dblPromFacturado += 0;
                                dblMontoNoFacturado += Funciones.CheckDbl(dr1["CARGO_FIJO"], 2);
                            }
                        }

                        DataRow dr = dtLineaFormatSISACT.NewRow();
                        dr["CODIGO_CUENTA"] = strCuenta;
                        dr["ESTADO_CUENTA"] = drSISACT["ESTADO"];
                        dr["CUSTOMER_ID"] = strCustomerID;
                        dr["CENTRAL_RIESGO"] = "";
                        dr["CF"] = string.Format("{0:#,#,#,0.00}", Funciones.CheckDbl(dblCF, 2));
                        dr["PROMEDIO_FACTURADO"] = string.Format("{0:#,#,#,0.00}", Funciones.CheckDbl(dblPromFacturado, 2));
                        dr["MONTO_NO_FACTURADO"] = string.Format("{0:#,#,#,0.00}", Funciones.CheckDbl(dblMontoNoFacturado, 2));
                        dr["MONTO_VENCIDO"] = string.Format("{0:#,#,#,0.00}", Funciones.CheckDbl(dblDeudaVencida, 2));
                        dr["MONTO_CASTIGADO"] = string.Format("{0:#,#,#,0.00}", Funciones.CheckDbl(dblDeudaCastigada, 2));
                        dr["DEUDA_REINTEGRO"] = string.Format("{0:#,#,#,0.00}", Funciones.CheckDbl(dblDeudaReintegro, 2));
                        dr["CANTIDAD_SERVICIOS"] = dblNroServicios;
                        dr["CANTIDAD_BLOQUEOS"] = dblNroBloqueos;
                        dr["CANTIDAD_SUSPENDIDOS"] = dblNroSuspensiones;

                        dtLineaFormatSISACT.Rows.Add(dr);
                    }
                }
            }
        }
        //PROY-140743 INI
        private string tableDetalleAccesorios(string id)
        {
            StringBuilder strTableHTML = new StringBuilder();

            strTableHTML.Append("<tr style='display:none' id='" + id + "'>");
            strTableHTML.Append("<td colspan='100%'>");
            strTableHTML.Append("<table align='center' width='85%' cellspacing='0' rules='all' border='1' style='border-collapse:collapse;Z-INDEX: 0'>");

            strTableHTML.Append("<tr class='TablaTitulos' nowrap='nowrap' align='Center'>");
            strTableHTML.Append("<td>Plan / Solucion</td>");
            strTableHTML.Append("<td>Número</td>");
            strTableHTML.Append("<td>Productos / servicios</td>");
            strTableHTML.Append("<td>Cargo Fijo</td>");
            strTableHTML.Append("<td>Fecha Activación</td>");
            strTableHTML.Append("<td>Fecha Estado</td>");
            strTableHTML.Append("<td>Estado</td>");
            strTableHTML.Append("<td>Motivo Bloqueo</td>");
            strTableHTML.Append("<td>Motivo Suspensión</td>");
            strTableHTML.Append("<td>Campaña</td>");
            strTableHTML.Append("</tr>");

            StringBuilder strTableContentID = new StringBuilder();

            if (dtDetalleAccesorios != null)
            {
                foreach (DataRow dr in dtDetalleAccesorios.Rows)
                {
                    if (Funciones.CheckStr(dr["CUENTA"]).Equals(id))
                    {
                        if (((Funciones.CheckStr(dr["ESTADO"]).ToUpper().Equals("DESACTIVO") || Funciones.CheckStr(dr["ESTADO"]).ToUpper().Equals("INACTIVO") || Funciones.CheckStr(dr["ESTADO"]).ToUpper().Equals("SUSPENDIDO"))) || Funciones.CheckStr(dr["ESTADO"]).ToUpper().Equals("ACTIVO")) // INICIATIVA-219
                        {
                            strTableContentID.Append("<tr class='Arial10B' align='Center'>");
                            strTableContentID.Append("<td>" + Funciones.CheckStr(dr["PLAN"]) + "</td>");

                            string numero = Funciones.CheckStr(dr["NUMERO"]);
                            if (numero.Length > 6) numero = numero.Substring(0, numero.Length - 3) + "xxx";

                            strTableContentID.Append("<td>" + numero + "</td>");
                            strTableContentID.Append("<td><div style='word-wrap:break-word;width:160px'>" + Funciones.CheckStr(dr["PRODUCTO/SERVICIO"]) + "</div></td>");
                            strTableContentID.Append("<td>" + string.Format("{0:#,#,#,0.00}", Funciones.CheckDbl(dr["CF_CONTRATO"], 2)) + "</td>");
                            strTableContentID.Append("<td>" + Funciones.CheckStr(dr["FECHA_ACTIVACION"]) + "</td>");
                            strTableContentID.Append("<td>" + Funciones.CheckStr(dr["FECHA_ESTADO"]) + "</td>");
                            strTableContentID.Append("<td>" + Funciones.CheckStr(dr["ESTADO"]) + "</td>");
                            strTableContentID.Append("<td>" + Funciones.CheckStr(dr["MOT_BLOQ"]) + "</td>");
                            strTableContentID.Append("<td>" + Funciones.CheckStr(dr["MOT_SUSP"]) + "</td>");
                            strTableContentID.Append("<td>" + Funciones.CheckStr(dr["CAMPANA"]) + "</td>");
                            strTableContentID.Append("</tr>");
                        }
                    }
                }
            }

            strTableHTML.Append(strTableContentID);
            strTableHTML.Append("</table>");
            strTableHTML.Append("<br/>");
            strTableHTML.Append("</td>");
            strTableHTML.Append("</tr>");

            return strTableHTML.ToString();
        }
        //PROY-140743 FIN

        private void estructuraTable(ref DataTable dt)
        {
            dt.Columns.Add(new DataColumn("CODIGO_CUENTA", System.Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("ESTADO_CUENTA", System.Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("CUSTOMER_ID", System.Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("CENTRAL_RIESGO", System.Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("CF", System.Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("PROMEDIO_FACTURADO", System.Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("MONTO_NO_FACTURADO", System.Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("MONTO_VENCIDO", System.Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("MONTO_CASTIGADO", System.Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("DEUDA_REINTEGRO", System.Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("CANTIDAD_SERVICIOS", System.Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("CANTIDAD_BLOQUEOS", System.Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("CANTIDAD_SUSPENDIDOS", System.Type.GetType("System.String")));
        }
    }
}