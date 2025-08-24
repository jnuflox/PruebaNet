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
using System.Data;
using System.IO;

namespace Claro.SISACT.Web.Paginas.documentos
{
    public partial class sisact_pop_sustento_ingreso : Sisact_Webbase
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

            if (!Page.IsPostBack)
                Inicio();
            else
            {
                string accion = hidProceso.Value;
                hidProceso.Value = string.Empty;
                switch (accion)
                {
                    case "R":
                        rechazar();
                        break;
                    case "A":
                        aprobar();
                        break;
                }
            }
        }

        private void Inicio()
        {
            Int64 nroSEC = Funciones.CheckInt64(Request.QueryString["nroSEC"]);
            lblNroSEC.Text = nroSEC.ToString();
            txtCodSolicitud.Text = lblNroSEC.Text;

            DataRow dr = new BLSolicitud().ObtenerSolicitudPersona(nroSEC).Rows[0];

            lblTipoDoc.Text = dr["TDOCV_DESCRIPCION"].ToString();
            lblNroDoc.Text = Funciones.FormatoNroDocumento(dr["TDOCC_CODIGO"].ToString(), dr["CLIEC_NUM_DOC"].ToString());
            lblRazon.Text = dr["CLIEV_NOMBRE"].ToString() + ' ' + dr["CLIEV_APE_PAT"].ToString() + ' ' + dr["CLIEV_APE_MAT"].ToString();
        }

        //PROY-24740
        private void CargarGrilla()
        {
            List<BEItemGenerico> lstArchivo = new List<BEItemGenerico>();

            lstArchivo.AddRange(hidListaArchivos.Value.Split('|').Where(w => !string.IsNullOrEmpty(w))
                .Select(s => s.Split(';'))
                .Select(c => new BEItemGenerico(c[0], c[1])).ToList()
                );

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
            string rutaCompletaArchivo = rutaDestino + id + nombreArchivo;

            File.Delete(rutaCompletaArchivo);

            string listaArhivo = hidListaArchivos.Value.Split('|').Where(w => !string.IsNullOrEmpty(w)).Where(q => q.Split(';')[0] != id).Aggregate((a, b) => string.Format("{0}|{1}", a, b));

            hidListaArchivos.Value = string.IsNullOrEmpty(listaArhivo) ? string.Empty : string.Format("|{0}", listaArhivo);

            CargarGrilla();
        }

        private void aprobar()
        {
            BLSolicitud objSolicitud = new BLSolicitud();
            Int64 nroSEC = Funciones.CheckInt64(lblNroSEC.Text);

            GrabarArchivo(nroSEC);
            objSolicitud.AprobarSustentoIngreso(nroSEC, CurrentUser, 0);
        }

        private void rechazar()
        {
            BLSolicitud objSolicitud = new BLSolicitud();
            Int64 nroSEC = Funciones.CheckInt64(lblNroSEC.Text);
            string strEstado = ConfigurationManager.AppSettings["constcodEstadoRECHAZADOXNOADJSUSTENTO"].ToString();

            objSolicitud.RechazarSEC(nroSEC, string.Empty, string.Empty, CurrentUser, string.Empty);
            objSolicitud.GrabarLogHistorico(nroSEC, strEstado, CurrentUser);
        }

        private void GrabarArchivo(Int64 nroSEC)
        {
            string rutaDestino = ConfigurationManager.AppSettings["constRutaUploadFileConsumer"].ToString();
            string listaArchivo = hidListaArchivos.Value;

            if (!string.IsNullOrEmpty(listaArchivo))
            {
                string[] objArchivo = listaArchivo.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in objArchivo)
                {
                    string nombreArchivo = item.Split(';')[1];
                    string rutaCompletaArchivo = item.Split(';')[2];
                    (new BLSolicitud()).GrabarArchivo(nroSEC, nombreArchivo, rutaCompletaArchivo, CurrentUser);
                }
            }
        }
    }
}