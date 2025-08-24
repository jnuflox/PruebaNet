using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Claro.SISACT.Business;
using Claro.SISACT.Common;
using Claro.SISACT.Web.Base;
using Claro.SISACT.Entity;
using Claro.SISACT.WS;//PROY-140743
using Claro.SISACT.Entity.VentasCuotas.ObtenerDatosPedidoAccCuotas.Response;//PROY-140743 

namespace Claro.SISACT.Web.Paginas.consultas
{
    public partial class sisact_pop_consulta_brms : Sisact_Webbase
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
            Int64 nroSEC = Funciones.CheckInt64(Request.QueryString["nroSEC"]);
            var FlagProac = Funciones.CheckInt64(Request.QueryString["FlagProac"]); // PROY - 30748
            GeneradorLog _objLogPag = CrearLog(nroSEC.ToString());
            _objLogPag.CrearArchivolog(String.Format("sisact_pop_consulta_brms [INICIO] {0}", ""), null, null);
            _objLogPag.CrearArchivolog(String.Format("sisact_pop_consulta_brms [INICIO][SEC] -> {0}", nroSEC), null, null);
            _objLogPag.CrearArchivolog(String.Format("sisact_pop_consulta_brms [INICIO][FlagProac] -> {0}", Funciones.CheckInt64(Request.QueryString["FlagProac"])), null, null);

            List<BEParametro> lstBEEvaluacionProactiva = new List<BEParametro>();//Proy-36928
            DataSet obj = new DataSet();
            DataSet objAccesorio = new DataSet();//PROY-140743
            string tipoOperacion = string.Empty;//PROY-140743
            // INICIO - PROY - 30748
            obj = FlagProac == 1 ? (new BLEvaluacion()).ObtenerDatosBRMSPROA(nroSEC) : (new BLEvaluacion()).ObtenerDatosBRMS(nroSEC);
            // FIN - PROY - 30748
            try
            {
// INICIO - PROY - 30748
                if (FlagProac == 1)
                {

                    //EMMH I
                    string strCodGrupoParamEvaluacionProactiva = Funciones.CheckStr(ConfigurationManager.AppSettings["consCodGrupoParamEvaluacionProactiva"]);
                    if (!string.IsNullOrEmpty(strCodGrupoParamEvaluacionProactiva))
                        lstBEEvaluacionProactiva = (new BLGeneral()).ListaParametrosGrupo(Funciones.CheckInt64(strCodGrupoParamEvaluacionProactiva));
                    if (lstBEEvaluacionProactiva.Count > 0) lstBEEvaluacionProactiva = lstBEEvaluacionProactiva.OrderBy(p => p.Valor1).ToList();
                    string strMsgSinPlanProactivo = lstBEEvaluacionProactiva.Where(p => p.Valor1 == "constMsgSinPlanProactivo").SingleOrDefault().Valor;                    
                    //EMMH F 

                    if (obj.Tables["CV_PLAN_SOLICITADO"].Rows.Count == 0)
                    {                        
                        //EMMH I
                        //Response.Write("<script>alert('" + ConfigurationManager.AppSettings["constMsgSinPlanProactivo"] + "')</script>");
                        Response.Write("<script>alert('" + strMsgSinPlanProactivo + "')</script>");
                        //EMMH F
                        return;
                    }


                    obj.Tables["CV_CLIENTE"].AcceptChanges();
                    foreach (DataRow dr in obj.Tables["CV_CLIENTE"].Rows)
                    {
                        if (dr != obj.Tables["CV_CLIENTE"].Rows[0]) {
                            dr.Delete();
                        }
                    } 
                    obj.Tables["CV_CLIENTE"].AcceptChanges();

                    obj.Tables["CV_DIRECCION_CLIENTE"].AcceptChanges();
                    foreach (DataRow dr in obj.Tables["CV_DIRECCION_CLIENTE"].Rows)
                    {
                        if (dr != obj.Tables["CV_DIRECCION_CLIENTE"].Rows[0])
            {
                            dr.Delete();
                        }
                    }
                    obj.Tables["CV_DIRECCION_CLIENTE"].AcceptChanges();
                }
                // FIN - PROY - 30748

                dgPuntoVenta.DataSource = obj.Tables["CV_PUNTO_VENTA"];
                dgPuntoVenta.DataBind();
                dgDireccionPDV.DataSource = obj.Tables["CV_DIRECCION_PDV"];
                dgDireccionPDV.DataBind();
                dgSolicitud.DataSource = obj.Tables["CV_SOLICITUD"];
                dgSolicitud.DataBind();
                // INICIO PROY-29121
                obj.Tables["CV_CLIENTE"].Columns.Add("DEUDA");
                foreach (DataRow dr in obj.Tables["CV_CLIENTE"].Rows) 
                {
                    dr["DEUDA"] = Funciones.CheckStr(Request.QueryString["deudaCliente"]);
                }
                // FIN PROY-29121
                dgCliente.DataSource = obj.Tables["CV_CLIENTE"];
                dgCliente.DataBind();
                dgDireccionCliente.DataSource = obj.Tables["CV_DIRECCION_CLIENTE"];
                dgDireccionCliente.DataBind();
                dgDocumentoCliente.DataSource = obj.Tables["CV_DOCUMENTO_CLIENTE"];
                dgDocumentoCliente.DataBind();
                dgRRLL.DataSource = obj.Tables["CV_RRLL"];
                dgRRLL.DataBind();

                dgEquipos.DataSource = null;//PROY-140579 NN  
                dgEquipos.DataSource = obj.Tables["CV_EQUIPOS"];
                dgEquipos.DataBind();

                //PROY-140743 : INI
                dgOferta.Columns[7].Visible = true;
                dgOferta.Columns[8].Visible = true;
                //PROY-140743 : FIN

                dgOferta.DataSource = obj.Tables["CV_OFERTA"];
                dgOferta.DataBind();
                dgCampana.DataSource = obj.Tables["CV_CAMP"];
                dgCampana.DataBind();
                dgServicios.DataSource = obj.Tables["CV_SERVICIOS"];
                dgServicios.DataBind();

                dgPlanActual.DataSource = obj.Tables["CV_PLAN_ACTUAL"];
                dgPlanActual.DataBind();
                dgPlanSolicitado.DataSource = obj.Tables["CV_PLAN_SOLICITADO"];
                dgPlanSolicitado.DataBind();
                
                Response.Clear();
                Response.Charset = "UTF-8";
                                // INICIO - PROY - 30748
                Response.AddHeader("Content-Disposition",
                    FlagProac == 1
                        ? "Attachment;Filename=Reporte_PlanProacBRMS.xls"
                        : "Attachment;Filename=Reporte_BRMS.xls");
                // FIN - PROY - 30748
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.ContentEncoding = System.Text.Encoding.GetEncoding(1252);
            }
            catch (Exception ex)
            {
                GeneradorLog _objLog = CrearLog(nroSEC.ToString());
                _objLog.CrearArchivolog("[ERROR EXPORTAR BRMS]", null, ex);
            }
        }
    }
}
