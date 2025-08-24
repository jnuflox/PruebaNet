using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using Claro.SISACT.Business;
using Claro.SISACT.Common;
using Claro.SISACT.Web.Base; //PROY-140126 - IDEA 140248 

namespace Claro.SISACT.Web.Paginas.consultas
{
    public partial class sisact_pop_consulta_sec : Sisact_Webbase //PROY-140126 - IDEA 140248 
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

        public void Inicio()
        {
            string tipoDocumento = Request.QueryString["tipoDocumento"];
            string nroDocumento = Request.QueryString["nroDocumento"];

            DataTable dt = (new BLSolicitud()).ObtenerSECPendiente(tipoDocumento, nroDocumento);
            lblFilas.Text = string.Format("{0} registro(s) encontrado(s)", dt.Rows.Count.ToString());

            gvPool.DataSource = dt;
            gvPool.DataBind();
        }

        protected void gvPool_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string tipoDocumento = Funciones.CheckStr(e.Row.Cells[10].Text);
                string nroDocumento = Funciones.CheckStr(e.Row.Cells[3].Text);
                e.Row.Cells[3].Text = Funciones.FormatoNroDocumento(tipoDocumento, nroDocumento);
                if (e.Row.Cells[9].Text.Equals("VENDIDO"))
                {
                    string script = "document.getElementById('rbtSEC').disabled='true'";
                    ClientScript.RegisterStartupScript(Page.GetType(), "script", "<script>" + script + ";</script>");
                }
            }
        }
    }
}