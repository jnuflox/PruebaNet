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
using Claro.SISACT.Web.Base;

namespace Claro.SISACT.Web.Paginas.consultas
{
    public partial class sisact_pop_comentario : Sisact_Webbase
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

            if (!Page.IsPostBack)
                hidNroSEC.Value = Request.QueryString["nroSEC"];
            else
                Grabar();
        }

        private void Grabar()
        {
            BEComentario obj = new BEComentario();
            obj.COMEC_ESTADO = "1";
            obj.COMEC_FLA_COM = "01";
            obj.COMEC_USU_REG = CurrentUser;
            obj.SOLIN_CODIGO = Funciones.CheckInt64(hidNroSEC.Value);
            obj.COMEV_COMENTARIO = Funciones.CheckStr(txtComentario.Value).ToUpper();

            bool blnOK = new BLSolicitud().GrabarComentario(obj);
            if (blnOK) hidProceso.Value = "OK";

            string script = "inicio();";
            ClientScript.RegisterStartupScript(Page.GetType(), "script", "<script>" + script + ";</script>");
        }
    }
}