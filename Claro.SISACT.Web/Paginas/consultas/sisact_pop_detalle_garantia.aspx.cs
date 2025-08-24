using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Claro.SISACT.Common;
using Claro.SISACT.Entity;
using System.Configuration;
using System.Collections;
using Claro.SISACT.Web.Base; //PROY-140126 - IDEA 140248 

namespace Claro.SISACT.Web.Paginas.consultas
{
    public partial class sisact_pop_detalle_garantia : Sisact_Webbase //PROY-140126 - IDEA 140248 
    {
        double dblMontoGarantia = 0.0;
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
            string nroDocumento = Request.QueryString["nroDocumento"];

            BEClienteCuenta objCliente = (BEClienteCuenta)HttpContext.Current.Session["objCliente" + nroDocumento];

            if (objCliente.oVistaEvaluacion.oGarantia.Count > 0)
            {
                gvGarantia.DataSource = objCliente.oVistaEvaluacion.oGarantia;
                gvGarantia.DataBind();
            }
        }

        protected void gvGarantia_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                dblMontoGarantia += Funciones.CheckDbl(e.Row.Cells[4].Text);
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[4].Text = string.Format("{0:#,#,#,0.00}", dblMontoGarantia);
            }
        }
    }
}