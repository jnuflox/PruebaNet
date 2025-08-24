using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Claro.SISACT.Common;
using System.Data;
using Claro.SISACT.Business;
using Claro.SISACT.WS;//PROY-140743
using Claro.SISACT.Entity.VentasCuotas.ObtenerDatosPedidoAccCuotas.Response;//PROY-140743 
using Claro.SISACT.Entity;//PROY-140743 
using Claro.SISACT.Web.Base;//PROY-140743 

namespace Claro.SISACT.Web.Paginas.consultas
{
    public partial class sisact_pop_consulta_campnavidad_brms : System.Web.UI.Page
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

            Inicio();
        }

        private void Inicio()
        {
            GeneradorLog objLog = new GeneradorLog(null, Funciones.CheckStr(Request.QueryString["nroSEC"]), null, "[BRMS]");
            objLog.CrearArchivolog(string.Format("[Inicio][Parámetros ODM Validación Campaña]"), null, null);

            Int64 nroSEC = Funciones.CheckInt64(Request.QueryString["nroSEC"]);
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-140439 BRMS CAMPAÑA NAVIDEÑA][nroSEC]", Funciones.CheckStr(nroSEC)), null, null);
            string deudaCliente = Funciones.CheckStr(Request.QueryString["deudaCliente"]);
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-140439 BRMS CAMPAÑA NAVIDEÑA][deudaCliente]", Funciones.CheckStr(deudaCliente)), null, null);

            try
            {
                DataSet obj = new DataSet();
                DataSet objAccesorio = new DataSet();//PROY-140743
                string tipoOperacion = string.Empty;//PROY-140743
                obj = BLEvaluacion.ObtenerDatosBRMSValidacionCampana(nroSEC);

                if (obj.Tables["CV_SOLICITUD"].Rows.Count == 0)
                {
                    var mensaje = Funciones.CheckStr(ReadKeySettings.Key_msjSinValidacionCamp);
                    Response.Write("<script>alert('" + mensaje + "')</script>");
                    return;
                }

                DataTable dtAuxSolicitud = obj.Tables["CV_SOLICITUD"];
                dgSolicitud.DataSource = dtAuxSolicitud;
                dgSolicitud.DataBind();

                DataTable dtAuxCliente = obj.Tables["CV_CLIENTE"];
                dtAuxCliente.Columns.Add("DEUDA", typeof(string));
                foreach (DataRow dr in dtAuxCliente.Rows)
                {
                    dr["DEUDA"] = Funciones.CheckStr(Request.QueryString["deudaCliente"]);
                }
                dgCliente.DataSource = dtAuxCliente;
                dgCliente.DataBind();

                DataTable dtAuxDireccion = obj.Tables["CV_DIRECCION_CLIENTE"];
                dgDireccion.DataSource = dtAuxDireccion;
                dgDireccion.DataBind();

                DataTable dtAuxDocumento = obj.Tables["CV_DOCUMENTO_CLIENTE"];
                dgDocumento.DataSource = dtAuxDocumento;
                dgDocumento.DataBind();

                DataTable dtAuxRepresentante = obj.Tables["CV_RRLL"];
                dgRepresentantes.DataSource = dtAuxRepresentante;
                dgRepresentantes.DataBind();

                DataTable dtAuxEquipos = obj.Tables["CV_EQUIPOS"];
                dgEquipos.DataSource = dtAuxEquipos;
                dgEquipos.DataBind();

                //PROY-140743 : INI
                dgOferta.Columns[6].Visible = true;
                dgOferta.Columns[7].Visible = true;
                //PROY-140743 : FIN

                DataTable dtAuxOferta = obj.Tables["CV_OFERTA"];
                dgOferta.DataSource = dtAuxOferta;
                dgOferta.DataBind();

                DataTable dtAuxCampana = obj.Tables["CV_CAMP"];
                dgCampana.DataSource = dtAuxCampana;
                dgCampana.DataBind();

                DataTable dtAuxServicios = obj.Tables["CV_SERVICIOS"];
                dgServicios.DataSource = dtAuxServicios;
                dgServicios.DataBind();

                DataTable dtAuxtPlanActual = obj.Tables["CV_PLAN_ACTUAL"];
                dgPlanActual.DataSource = dtAuxtPlanActual;
                dgPlanActual.DataBind();

                DataTable dtAuxPlanSolicitado = obj.Tables["CV_PLAN_SOLICITADO"];
                dgPlanSolicitado.DataSource = dtAuxPlanSolicitado;
                dgPlanSolicitado.DataBind();

                DataTable dtAuxPuntoVenta = obj.Tables["CV_PUNTO_VENTA"];
                dgPuntoVenta.DataSource = dtAuxPuntoVenta;
                dgPuntoVenta.DataBind();

                DataTable dtAuxDireccionPdV = obj.Tables["CV_DIRECCION_PDV"];
                dgDireccionPDV.DataSource = dtAuxDireccionPdV;
                dgDireccionPDV.DataBind();

                Response.Clear();
                Response.Charset = "UTF-8";
                Response.AddHeader("Content-Disposition", "Attachment;Filename=Reporte_Validacion_Campana.xls");
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.ContentEncoding = System.Text.Encoding.GetEncoding(1252);
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-140439 BRMS CAMPAÑA NAVIDEÑA][Documento generado correctamente]", "[Exito]"), null, null);
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog(string.Format("{0} => {1}|{2}", "[ERROR AL GENERAR DOCUMENTO]", ex.Message, ex.StackTrace), null, null);
            }
            objLog.CrearArchivolog(string.Format("[Fin][Parámetros ODM Validación Campaña]"), null, null);
        }
    }
}