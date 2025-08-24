using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Claro.SISACT.Entity;
using Claro.SISACT.Web.Base;//PROY-140126 - IDEA 140248 

namespace Claro.SISACT.Web.Paginas.consultas
{
    public partial class sisact_pop_detalle_linea_prepago : Sisact_Webbase//PROY-140126 - IDEA 140248  
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
            string nroDocumento = Request.QueryString["nroDocumento"];
            BEClienteCuenta objCliente = (BEClienteCuenta)HttpContext.Current.Session["objCliente" + nroDocumento];

            labelMsgSuccessListaLineas.InnerText = ConfigurationManager.AppSettings["consSuccessLineasDisponibles"].ToString();

            gvListaPrepago.DataSource = objCliente.lineaPrepago;
            gvListaPrepago.DataBind();
        }
    }
}