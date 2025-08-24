using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Web.UI.WebControls;
using Claro.SISACT.Common;
using Claro.SISACT.Entity;
using Claro.SISACT.WS; //PROY-31636
using Claro.SISACT.Web.Base; //PROY-140126 - IDEA 140248 

namespace Claro.SISACT.Web.Paginas.frames
{
    public partial class sisact_ifr_representante_legal : Sisact_Webbase //PROY-140126 - IDEA 140248  
    {
        private static List<BEItemGenerico> lstNacionalidad; //PROY-31636

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
            BEEmpresaExperto objDC = (BEEmpresaExperto)HttpContext.Current.Session["objClienteDC" + nroDocumento];
            //PROY-29121-INI
            hidSituacionOK.Value = ConfigurationManager.AppSettings["constMensajeEstatusRRLLOK"];
            hidSituacionNOOK.Value = ConfigurationManager.AppSettings["constMensajeEstatusRRLLNOOK"];
            //PROY-29121-FIN
            dgRepresentanteLegal.DataSource = objDC.oRepresentanteLegal;
            dgRepresentanteLegal.DataBind();
        }

        //INI PROY-31636
        public List<BEItemGenerico> CargarNacionalidades()
        {
            if (lstNacionalidad == null)
            {
                lstNacionalidad = (List<BEItemGenerico>)HttpContext.Current.Session["listNacionalidad"];
                lstNacionalidad.Insert(0, new BEItemGenerico { Codigo = String.Empty, Descripcion = "--SELECCIONE--" });
            }
            return lstNacionalidad;
        }
        //FIN PROY-31636
    }
}