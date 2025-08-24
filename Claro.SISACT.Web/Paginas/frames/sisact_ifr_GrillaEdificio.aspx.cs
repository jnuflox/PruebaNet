using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using Claro.SISACT.Business;
using Claro.SISACT.Common;
using Claro.SISACT.Web.Base; //PROY-140126 - IDEA 140248 

namespace Claro.SISACT.Web.Paginas.frames
{
    public partial class sisact_ifr_GrillaEdificio : Sisact_Webbase //System.Web.UI.Page - PROY-140126 - IDEA 140248  
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

            llenarGrilla();
        }

        public void llenarGrilla()
        {
            try
            {
                string strCodPlano = Request.QueryString["strCodPlano"];
                string strCodEdificio = Request.QueryString["strCodEdificio"];
                int codError = 0;
                DataTable dt = new BLSolicitudNegocios().ListarEdificio(strCodPlano, strCodEdificio.Trim().ToUpper());
                int intCount = 0;

                if (codError != -1)
                {
                    intCount = dt.Rows.Count;
                    hidNroFilas.Value = Funciones.CheckStr(intCount);
                    if (intCount > 0)
                    {
                        this.dgPDV.DataSource = dt;
                        this.dgPDV.DataBind();
                        hidNResponseValue.Value = "TIENEDATOS";
                        hidDatosRetorno.Value = "";
                    }
                    else
                    {
                        hidNResponseValue.Value = "ERROR";
                        hidDatosRetorno.Value = "";
                    }
                }
                else
                {
                    hidNResponseValue.Value = "ERROR";
                    hidDatosRetorno.Value = "";
                }
            }
            catch (Exception)
            {
                hidNResponseValue.Value = "ERROR";
                hidDatosRetorno.Value = "";
            }
        }

        protected void dgPDV_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            dgPDV.CurrentPageIndex = e.NewPageIndex;
            llenarGrilla();
        }
    }
}