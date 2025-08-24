using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Claro.SISACT.Common;
using System.Collections;
using Claro.SISACT.Entity;
using Claro.SISACT.Web.Base; //PROY-140126 - IDEA 140248 

namespace Claro.SISACT.Web.Paginas.consultas
{
    public partial class sisact_pop_equipo_3play : Sisact_Webbase //PROY-140126 - IDEA 140248  
    {
        int intNroItem = 0;

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

            if (!Page.IsPostBack)
            {
                string strEquipoHFC = Funciones.CheckStr(Request.QueryString["strEquipoHFC"]);
                CargarGrilla(strEquipoHFC, Funciones.CheckInt64(Request.QueryString["nroSEC"]));
            }
        }

        //PROY-24740
        public void CargarGrilla(string strEquipoHFC, Int64 nroSEC)
        {
            ArrayList oLista = new ArrayList();

            if (strEquipoHFC.Length > 0)
            {
                oLista.AddRange(strEquipoHFC.Split('|').Where(w => w.Length > 1).Select(s => s.Split(';'))
                                .Select(c => new BEPlanDetalleHFC()
                {
                                    Servicio = Funciones.CheckStr(c[2]).Replace("(*)", "")
                                }).ToList());
            }

            dgCabListaEquipo.DataSource = "";
            dgCabListaEquipo.DataBind();
            this.dgListaEquipo.DataSource = oLista;
            this.dgListaEquipo.DataBind();
        }

        protected void dgListaEquipo_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem | e.Item.ItemType == ListItemType.Item)
            {
                intNroItem += 1;
                e.Item.Cells[0].Text = intNroItem.ToString();
            }
        }
    }
}