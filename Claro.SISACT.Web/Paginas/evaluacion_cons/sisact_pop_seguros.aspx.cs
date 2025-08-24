using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Claro.SISACT.WS;
using Claro.SISACT.WS.ConsultarPuntosWS;
using Claro.SISACT.Entity;
using System.Configuration;
using Claro.SISACT.Web.Base;
using Claro.SISACT.Common;
using System.Data;
using System.Collections;
using System.Text;
using Claro.SISACT.Business;

namespace Claro.SISACT.Web.Paginas.Venta
{
    public partial class sisact_pop_seguros : Sisact_Webbase //PROY-24724-IDEA-28174 - NUEVA CLASE
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
            string strConcatSeguros = Request.QueryString["concatSeguros"];
            strConcatSeguros = strConcatSeguros.Replace('=', 'ñ');
            strConcatSeguros = strConcatSeguros.Replace('_', ' ');
            DataTable dtSeguros = new DataTable();

            for (int i = 0; i < gvSeguros.Columns.Count; i++)
                dtSeguros.Columns.Add(gvSeguros.Columns[i].HeaderText.ToString());
                
            foreach(string fila in strConcatSeguros.Split('|'))
            {
                DataRow drSeguro = dtSeguros.NewRow();
                string[] astrFila = fila.Split(';');
                drSeguro.ItemArray = astrFila;
                dtSeguros.Rows.Add(drSeguro);
            }
            gvSeguros.DataSource = dtSeguros;
            gvSeguros.DataBind();            
        }
    }
}