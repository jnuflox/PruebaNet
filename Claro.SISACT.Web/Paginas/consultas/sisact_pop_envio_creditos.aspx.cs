using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;
using Claro.SISACT.Entity;
using TestWebMsgApp;
using Claro.SISACT.Web.Base; //PROY-140126 - IDEA 140248 

namespace Claro.SISACT.Web.Paginas.consultas
{
    public partial class sisact_pop_envio_creditos : Sisact_Webbase //PROY-140126 - IDEA 140248 
    {
        #region [Declaracion de Constantes - Config]

        string rutaDestino = ConfigurationManager.AppSettings["constRutaUploadFileConsumer"].ToString();

        #endregion [Declaracion de Constantes - Config]

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
        }

        private void CargarGrilla()
        {
            List<BEItemGenerico> lstArchivo = new List<BEItemGenerico>();
            string[] listaArchivo = hidListaArchivos.Value.Split('|');
            foreach (string item in listaArchivo)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    BEItemGenerico obj = new BEItemGenerico(item.Split(';')[0], item.Split(';')[1]);
                    lstArchivo.Add(obj);
                }
            }
            gvArchivo.DataSource = lstArchivo;
            gvArchivo.DataBind();
        }

        protected void btnCargarArchivo_Click(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        //PROY-24740
        protected void gvArchivo_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string id = gvArchivo.DataKeys[e.RowIndex].Values[0].ToString();
            string nombreArchivo = gvArchivo.DataKeys[e.RowIndex].Values[1].ToString();
            string rutaCompletaArchivo = string.Format("{0}{1}{2}", rutaDestino, id, nombreArchivo);

            File.Delete(rutaCompletaArchivo);

            string listaArhivo = hidListaArchivos.Value.Split('|').Where(w => !string.IsNullOrEmpty(w)).Where(q => q.Split(';')[0] != id).Aggregate((a, b) => string.Format("{0}|{1}", a, b));

            hidListaArchivos.Value = string.IsNullOrEmpty(listaArhivo) ? string.Empty : string.Format("|{0}", listaArhivo);

            CargarGrilla();
        }
    }
}