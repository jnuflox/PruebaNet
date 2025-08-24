using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using Claro.SISACT.Entity;
using Claro.SISACT.Business;
using System.IO;
using TestWebMsgApp;
using Claro.SISACT.Web.Base;
using System.Collections;
using Claro.SISACT.Common;

namespace Claro.SISACT.Web.Paginas.frames
{
    public partial class sisact_ifr_portabilidad : Sisact_Webbase
    {
        string rutaDestino = ConfigurationManager.AppSettings["constLeerFileServerSPPortINTemp"].ToString();

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
                Inicio();
            }
        }

        private void Inicio()
        {
            CargarOperadorCedente();
            CargarModalidad();

            // PROY-26358 -Inicio - sisact_ifr_portabilidad(flagPortaAdjuntarFormato) - Evalenzs 
            String TipoOficina = Request.QueryString["strTipoOficina"];// PROY-26358 - Evalenzs -TipoOficina
          Int64 ConsPortaAdjFormt = Funciones.CheckInt(ConfigurationManager.AppSettings["ConsPortaAdjuntarFormatparangrup"]);

            GeneradorLog objLog = new GeneradorLog(CurrentUser, "Portabilidad", null, "WEB");

            objLog.CrearArchivolog("[Inicio][ListaParametrosGrupo]" + ConsPortaAdjFormt, null, null);
            objLog.CrearArchivolog("[Inicio][ListaParametrosGrupo][SP]" + "SISACT_PKG_CONSULTA_GENERAL.SECSS_CON_PARAMETRO_GP", null, null);
          List<BEParametro> arrParametro = (new BLGeneral()).ListaParametrosGrupo(ConsPortaAdjFormt);
            objLog.CrearArchivolog("[Fin][ListaParametrosGrupo]     Cantidad de elementos=" + arrParametro.Count, null, null);

          String parangrupo=String.Empty;
           foreach (BEParametro Item in arrParametro)
           {
               parangrupo = Item.Valor;
               if (Item.Valor1.Equals(TipoOficina))//01 - CAC 02 - DAC 02 - CAD
               { 
                    objLog.CrearArchivolog("[TipoOficina]" + Item.Valor1, null, null);

                 if (parangrupo.Equals("0"))
                    {
                        objLog.CrearArchivolog("[Se visualiza la seccion Adjuntar Archivo]", null, null);
                       trAdjuntarArchivo.Style.Add("display", "");
                    }
                   else
                    {
                       trAdjuntarArchivo.Style.Add("display", "none");
                        objLog.CrearArchivolog("[No se visualiza la seccion Adjuntar Archivo]", null, null);
               }
           }
            }
            objLog.CrearArchivolog("[Fin][ListaParametrosGrupo]" + ConsPortaAdjFormt, null, null);
            // PROY-26358 - Fin - Evalenzs
        }

        private void CargarGrilla()
        {
            if (!string.IsNullOrEmpty(hidListaArchivos.Value))
            {
                BEArchivo objArchivo;
                List<BEArchivo> objLista = new List<BEArchivo>();
                string[] arrListaArchivo = hidListaArchivos.Value.Split('|');
                foreach (string strItem in arrListaArchivo)
                {
                    if (!string.IsNullOrEmpty(strItem))
                    {
                        string[] arrArchivo = strItem.Split(';');
                        objArchivo = new BEArchivo();

                        objArchivo.ARCH_CODIGO = arrArchivo[0];
                        objArchivo.ARCH_NOMBRE = arrArchivo[1];
                        objArchivo.ARCH_TIPO = arrArchivo[2];
                        objArchivo.ARCH_DESCRIPCION = arrArchivo[3];
                        objArchivo.ARCH_RUTA = arrArchivo[4].Replace("\\", "\\\\");

                        objLista.Add(objArchivo);
                    }
                }
                gvArchivo.DataSource = objLista;
                gvArchivo.DataBind();
                gvArchivo.Visible = true;
            }
            else
            {
                gvArchivo.DataSource = null;
                gvArchivo.DataBind();
                gvArchivo.Visible = false;
            }
        }

        protected void btnCargarArchivo_Click(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        protected void gvArchivo_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string nombreArchivo = gvArchivo.DataKeys[e.RowIndex].Values[0].ToString();
                string rutaCompletaArchivo = rutaDestino + nombreArchivo;

                using (System.Security.Principal.WindowsImpersonationContext ctx = System.Security.Principal.WindowsIdentity.Impersonate(IntraTimToken()))
                {
                    if (File.Exists(rutaCompletaArchivo))
                    {
                        File.Delete(rutaCompletaArchivo);
                    }
                    ctx.Undo();
                }

                string listaArhivo = string.Empty;
                string[] arrListaArchivo = hidListaArchivos.Value.Split('|');
                foreach (string strItem in arrListaArchivo)
                {
                    if (!string.IsNullOrEmpty(strItem))
                    {
                        if (nombreArchivo != strItem.Split(';')[0])
                        {
                            listaArhivo += "|" + strItem;
                        }
                    }
                }
                hidListaArchivos.Value = listaArhivo;

                CargarGrilla();
            }
            catch { }
        }

        private void CargarOperadorCedente()
        {
            ddlOperadorCedente.DataSource = (new BLPortabilidad()).ListarParametroPortabilidad("CO", "", "1", "", "", 1);
            ddlOperadorCedente.DataValueField = "PK_PARAT_PARAC_COD";
            ddlOperadorCedente.DataTextField = "DESCRIPCION";
            ddlOperadorCedente.DataBind();
            ddlOperadorCedente.Items.Insert(0, new ListItem("SELECCIONE...", String.Empty));
        }

        private void CargarModalidad()
        {
            ddlModalidadPorta.DataSource = (new BLPortabilidad()).ListarParametroPortabilidad("MO", "", "", "", "", 1);
            ddlModalidadPorta.DataValueField = "PK_PARAT_PARAC_COD";
            ddlModalidadPorta.DataTextField = "DESCRIPCION";
            ddlModalidadPorta.DataBind();
            ddlModalidadPorta.Items.Insert(0, new ListItem("SELECCIONE...", String.Empty));
        }

        [System.Web.Services.WebMethod()]
        public static BEResponseWebMethod CargarOperadorCedente(string strTipoPortabilidad)
        {
            BEResponseWebMethod objResponse = new BEResponseWebMethod();
            List<BEParametroPortabilidad> objLista = (new BLPortabilidad()).ListarParametroPortabilidad("CO", "", "1", strTipoPortabilidad, "", 1);
            foreach (BEParametroPortabilidad obj in objLista)
            {
                objResponse.Cadena = string.Format("{0}|{1};{2}", objResponse.Cadena, obj.PK_PARAT_PARAC_COD, obj.DESCRIPCION);
            }

            return objResponse;
        }
    }
}