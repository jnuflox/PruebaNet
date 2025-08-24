using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Claro.SISACT.Business;
using Claro.SISACT.Common;
using Claro.SISACT.Entity;
using TestWebMsgApp;
using Claro.SISACT.Web.Base;

namespace Claro.SISACT.Web.Paginas.documentos
{
    public partial class sisact_pop_upload_portabilidad : Sisact_Webbase
    {
        #region [Declaracion de Constantes - Config]

        string rutaDestino = ConfigurationManager.AppSettings["constLeerFileServerSPPortINTemp"].ToString();
        int tamanioMinimo = Convert.ToInt32(ConfigurationManager.AppSettings["TamanioMinimoArchivoSP"].ToString());
        int tamanioMaximo = Convert.ToInt32(ConfigurationManager.AppSettings["TamanioMaximoArchivoSP"].ToString());

        #endregion [Declaracion de Constantes - Config]

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            if (!Page.IsPostBack)
            {
                CargarTipoArchivo();
            }
            else
            {
                UploadArchivo();
            }
        }

        private void UploadArchivo()
        {
            string mensaje = string.Empty;
            string rutaCompletaArchivo = string.Empty;
            GeneradorLog _objLog = CrearLog(null);
            try
            {
                if (txtFile.PostedFile == null || txtFile.PostedFile.ContentLength == 0)
                {
                    mensaje = "El Archivo debe contener información";
                    WebMsgBox.Show(mensaje);
                }
                else
                {
                    int intTamanioFile = txtFile.PostedFile.ContentLength;

                    if (intTamanioFile < tamanioMinimo)
                    {
                        mensaje = string.Format("El documento debe ser mayor a {0} Kb.", Funciones.CalculodeKb(tamanioMinimo));
                    }

                    if (intTamanioFile > tamanioMaximo)
                    {
                        mensaje = string.Format("El documento debe tener como máximo {0} Mb.", Funciones.CalculodeMb(tamanioMaximo));
                    }

                    if (string.IsNullOrEmpty(mensaje))
                    {
                        string id = DateTime.Now.ToString("ddMMyyyyhhmmss");
                        string prefijo = "$__TEMP__";
                        string fileExtension = Path.GetExtension(txtFile.PostedFile.FileName);
                        string nombreArchivo = string.Format("{0}-{1}{2}", id, ddlTipoArchivo.SelectedValue, fileExtension);

                        rutaCompletaArchivo = rutaDestino + prefijo + nombreArchivo;

                        using (System.Security.Principal.WindowsImpersonationContext ctx = System.Security.Principal.WindowsIdentity.Impersonate(IntraTimToken()))
                        {
                            txtFile.SaveAs(rutaCompletaArchivo);
                            ctx.Undo();
                        }

                        string idTipoArchivo = ddlTipoArchivo.SelectedValue;
                        string tipoArchivo = ddlTipoArchivo.Items[ddlTipoArchivo.SelectedIndex].Text;

                        hidArchivo.Value = string.Format("{0};{1};{2};{3};{4}", prefijo + nombreArchivo, nombreArchivo, idTipoArchivo, tipoArchivo, rutaCompletaArchivo);

                        ClientScript.RegisterStartupScript(Page.GetType(), "script", "<script>" + "actualizarGrillaArchivo();" + ";</script>");
                    }
                    else
                    {
                        WebMsgBox.Show(mensaje);
                    }
                }
            }
            catch (Exception ex)
            {
                _objLog.CrearArchivolog("[ERROR][UploadArchivo]", rutaCompletaArchivo, ex);
                mensaje = ConfigurationManager.AppSettings["consMsjErrorAdjuntarArchivo"].ToString() + ex.Message;
                WebMsgBox.Show(mensaje);
            }
        }

        private void CargarTipoArchivo()
        {
            ddlTipoArchivo.DataSource = (new BLPortabilidad()).ListarParametroPortabilidad("AS", "", "", "PORTIN", "", 1);
            ddlTipoArchivo.DataValueField = "PK_PARAT_PARAC_COD";
            ddlTipoArchivo.DataTextField = "DESCRIPCION";
            ddlTipoArchivo.DataBind();
            ddlTipoArchivo.Items.Insert(0, new ListItem("SELECCIONE...", String.Empty));

            ddlTipoArchivo.SelectedValue = "T";
            ddlTipoArchivo.Enabled = false;
        }
    }
}