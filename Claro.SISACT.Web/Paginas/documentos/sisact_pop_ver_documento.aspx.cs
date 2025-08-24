using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;
using Claro.SISACT.Common;
using TestWebMsgApp;
using Claro.SISACT.Web.Base;

namespace Claro.SISACT.Web.Paginas.documentos
{
    public partial class verDocumento : Sisact_Webbase
    {
        string rutaDestino = ConfigurationManager.AppSettings["constLeerFileServerSPPortINTemp"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            VisualizaDocumento();
        }

        private void VisualizaDocumento()
        {
            string archivo = Request.QueryString["archivo"];
            try
            {
                string sTipoArchivo = Path.GetExtension(archivo);
                FileInfo fiFichero;

                using (System.Security.Principal.WindowsImpersonationContext ctx = System.Security.Principal.WindowsIdentity.Impersonate(IntraTimToken()))
                {
                    fiFichero = new FileInfo(archivo);
                    
                    Response.Clear();

                    string strTipoContenido = F_ObtieneContentType(sTipoArchivo);
                    string strExtencionesImagenRotacion = ConfigurationManager.AppSettings["Const_TipoExtencionImagenRotacion"];

                    if (strExtencionesImagenRotacion.IndexOf(strTipoContenido) > -1)
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + fiFichero.Name.Trim().Replace(" ", "_"));
                    else
                        Response.AddHeader("Content-Disposition", "inline; filename=" + fiFichero.Name.Trim().Replace(" ", "_"));

                    Response.AddHeader("Content-Length", fiFichero.Length.ToString());
                    Response.ContentType = Funciones.ObtenerContentType(sTipoArchivo);
                    Response.WriteFile(fiFichero.FullName);
                    Response.Flush();
                    Response.Close();

                    ctx.Undo();
                }
            }
            catch (Exception ex)
            {
                GeneradorLog _objLog = CrearLog(null);
                _objLog.CrearArchivolog("[ERROR][rutaCompletaArchivo]", archivo, null);
                _objLog.CrearArchivolog("[ERROR][VisualizaDocumento]", null, ex);
                string mensaje = ConfigurationManager.AppSettings["consMsjErrorVerArchivo"].ToString() + "-" + ex.Message;
                WebMsgBox.Show(mensaje);
            }
        }

        protected string F_ObtieneContentType(string pExtArchivo)
        {

            pExtArchivo = pExtArchivo.ToLower();

            if (pExtArchivo == ".htm" | pExtArchivo == ".html")
            {
                return "text/html";
            }
            else if (pExtArchivo == ".xls")
            {
                return "application/vnd.ms-excel";
            }
            else if (pExtArchivo == ".txt")
            {
                return "text/plain";
            }
            else if (pExtArchivo == ".pdf")
            {
                return "application/pdf";
            }
            else if (pExtArchivo == ".xml")
            {
                return "text/xml";
            }
            else if (pExtArchivo == ".doc" || pExtArchivo == ".docx")
            {
                return "application/msword";
            }
            else if (pExtArchivo == ".rtf")
            {
                return "application/rtf";
            }
            else if (pExtArchivo == ".odt")
            {
                return "application/vnd.oasis.opendocument.text";
            }
            else if (pExtArchivo == ".ods")
            {
                return "application/vnd.oasis.opendocument.spreadsheet";
            }
            else if (pExtArchivo == ".png")
            {
                return "image/png";
            }
            else if (pExtArchivo == ".jpg" | pExtArchivo == ".jpeg")
            {
                return "image/jpeg";
            }
            else if (pExtArchivo == ".gif")
            {
                return "image/gif";
            }
            else if (pExtArchivo == ".bmp")
            {
                return "image/bmp";
            }
            else if (pExtArchivo == ".tif" | pExtArchivo == ".tiff")
            {
                return "image/tiff";
            }
            else if (pExtArchivo == ".zip")
            {
                return "application/zip";
            }
            else if (pExtArchivo == ".rar")
            {
                return "application/x-rar-compressed";
            }
            else if (pExtArchivo == ".ppt")
            {
                return "application/mspowerpoint";
            }
            else if (pExtArchivo == ".swf")
            {
                return "application/x-shockwave-flash";
            }
            else
            {
                return "application/octet-stream";
            }
        }
    }
}