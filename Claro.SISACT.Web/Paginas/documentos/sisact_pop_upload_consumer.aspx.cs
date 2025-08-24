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
using Claro.SISACT.Web.Base; //PROY-140126 - IDEA 140248 

namespace Claro.SISACT.Web.Paginas.documentos
{
    public partial class sisact_pop_upload_consumer : Sisact_Webbase //PROY-140126 - IDEA 140248  
    {
        #region [Declaracion de Constantes - Config]

        string rutaDestino = ConfigurationManager.AppSettings["constRutaUploadFileConsumer"].ToString();

        #endregion [Declaracion de Constantes - Config]

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            if (Page.IsPostBack)
            {
                UploadArchivo();
            }
        }

        private void UploadArchivo()
        {
            string mensaje = string.Empty;
            string rutaCompletaArchivo = string.Empty;
            try
            {
                if (txtFile.PostedFile == null || txtFile.PostedFile.ContentLength == 0)
                {
                    mensaje = "El Archivo debe contener información";
                    WebMsgBox.Show(mensaje);
                }
                else
                {
                    if (txtFile.PostedFile.ContentLength > 512001)
                    {
                        mensaje = "El archivo sobrepasa el tamaño máximo permitido";
                        WebMsgBox.Show(mensaje);
                    }
                    else
                    {
                        string id = DateTime.Now.ToString("ddMMyyyyhhmmss");
                        string nombreArchivo = Path.GetFileName(txtFile.PostedFile.FileName);
                        rutaCompletaArchivo = rutaDestino + id + nombreArchivo;

                        txtFile.SaveAs(rutaCompletaArchivo);

                        hidArchivo.Value = string.Format("{0};{1};{2}", id, nombreArchivo, rutaCompletaArchivo);

                        ClientScript.RegisterStartupScript(Page.GetType(), "script", "<script>" + "actualizarGrillaArchivo();" + ";</script>");
                    }
                }
            }
            catch (Exception ex)
            {
                if (File.Exists(rutaCompletaArchivo))
                {
                    File.Delete(rutaCompletaArchivo);
                }
                mensaje = ConfigurationManager.AppSettings["consMsjErrorAdjuntarArchivo"].ToString() + "-" + ex.Message;
                WebMsgBox.Show(mensaje);
            }
        }
    }
}