using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Claro.SISACT.Business;
using Claro.SISACT.Common;
using Claro.SISACT.Entity;
using Claro.SISACT.WS;
using Claro.SISACT.Web.Base;
using Claro.SISACT.Web.Comun;

namespace Claro.SISACT.Web.Paginas.Evaluacion_cons
{
    public partial class sisact_pool_evaluador_consumer : Sisact_Webbase
    {
        #region [Declaracion de Constantes - Config]

        string constCodEstadoSEC = ConfigurationManager.AppSettings["constcodEstadoSECCONADJARCHIVOS"].ToString();
        string constDiasAntiguedadPool = ConfigurationManager.AppSettings["constCodDiasAntiguedadPool"].ToString();
        string constCodOpcionVerEvaluarSEC = ConfigurationManager.AppSettings["constCodOpcionVerEvaluarSEC"].ToString();

        #endregion [Declaracion de Constantes - Config]

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write("<script language='javascript' src='../../Scripts/funciones_sec.js'></script>");

            if (Funciones.CheckStr(HttpContext.Current.Request.QueryString["cu"]).Length == 0)
            {
                Response.Write("<script language=javascript>validarUrl();</script>");
            }
            else
            {
                Response.Write("<script language='javascript'>restringirEventos();</script>");
            }
            GeneradorLog objLogINC = new GeneradorLog(null, "[INC000003427525]", null, " SEC USUARIO NO AUTORIZADO ");
            if (Session["Usuario"] == null)
            {
                objLogINC.CrearArchivolog("[INC000003427525] - Page_Load() -Session[Usuario] == null  ", null, null);
                string codUsuarioExt = Request.QueryString["cu"];
                if (!AccesoUsuario.ValidarAcceso(codUsuarioExt, CurrentUser))
                {
                    string strRutaSite = ConfigurationManager.AppSettings["RutaSite"].ToString();
                    Response.Redirect(strRutaSite);
                    return;
                }
            }
            else
            {
                objLogINC.CrearArchivolog("[INC000003427525] - Page_Load() Pool -Session[Usuario] != null  ", null, null);
            }

            //INC000003427525 - INI
            
            BEUsuarioSession objUser = null;
            objUser = (BEUsuarioSession)Session["Usuario"];
            string cuSession = Funciones.CheckStr(objUser.idUsuario);
            string cuQueryStr = Funciones.CheckStr(Request.QueryString["cu"]) == string.Empty ? cuSession : Funciones.CheckStr(Request.QueryString["cu"]);

            objLogINC.CrearArchivolog("[INC000003427525] - Page_Load() - Request.QueryString[cu] ==> " + Funciones.CheckStr(Request.QueryString["cu"]), null, null);
            objLogINC.CrearArchivolog("[INC000003427525] - Page_Load() -objUser.idUsuario (cu) ==> " + cuSession, null, null);
            objLogINC.CrearArchivolog("[INC000003427525] - Page_Load() - cuQueryStr [cu] ==> " + cuQueryStr, null, null);

            if (cuSession == cuQueryStr)
            {
                if (!validaPermisos(Funciones.CheckStr(cuSession)))
                {
                    objLogINC.CrearArchivolog("[INC000003427525] - Page_Load() - ERROR ==> Redireccionando al inicio", null, null);
                    string strRutaSite = Funciones.CheckStr(ConfigurationManager.AppSettings["RutaSite"].ToString());
                    Response.Redirect(strRutaSite);
                    return;
                }
            }
            else
            {
                objLogINC.CrearArchivolog("[INC000003427525] - Page_Load() -CUs no coinciden ==> Redireccionando al inicio " , null, null);
                string strRutaSite = Funciones.CheckStr(ConfigurationManager.AppSettings["RutaSite"].ToString());
                Response.Redirect(strRutaSite);
                return;
            }
            //INC000003427525 - FIN

            if (!Page.IsPostBack)
            {
                Inicio();
            }
        }

        private void Inicio()
        {
            BEUsuarioSession objUsuarioSession = (BEUsuarioSession)Session["Usuario"];
            List<BEItemGenerico> objListaItem = (new BLGeneral()).ListarParametroGeneral("1");

            int intDiasAntiguedad = Funciones.CheckInt(WebComunes.obtenerParametro(objListaItem, constDiasAntiguedadPool));
            hidFlgVerEvaluarSEC.Value = WebComunes.tienePerfil(objUsuarioSession.CadenaOpcionesPagina, constCodOpcionVerEvaluarSEC);

            DateTime datFechaFin = DateTime.Now;
            DateTime datFechaInicio = DateTime.Now.AddDays(intDiasAntiguedad * -1);
            txtFechaInicio.Text = string.Format("{0:dd/MM/yyyy}", datFechaInicio);
            txtFechaFin.Text = string.Format("{0:dd/MM/yyyy}", datFechaFin);

            hidFechaInicioMaximo.Value = txtFechaInicio.Text;
            hidRangoFecha.Value = intDiasAntiguedad.ToString();
            hidUsuarioRed.Value = CurrentUser;

            Consultar();
        }

        public void Consultar()
        {
            string strFechaInicio = (!Page.IsPostBack) ? txtFechaInicio.Text : Request.Form[txtFechaInicio.UniqueID];
            string strFechaFin = (!Page.IsPostBack) ? txtFechaFin.Text : Request.Form[txtFechaFin.UniqueID];

            DateTime datFechaInicio = Funciones.CheckDate(strFechaInicio);
            DateTime datFechaFin = Funciones.CheckDate(strFechaFin);

            DataTable dtPool = (new BLSolicitud()).ObtenerPoolEvaluadorPersona(constCodEstadoSEC, datFechaInicio, datFechaFin);

            if (dtPool.Rows.Count == 0)
                lblMensaje.Text = "No Existen Registros";
            else
                lblMensaje.Text = dtPool.Rows.Count.ToString() + " registro(s) encontrado(s)";

            gvPool.DataSource = dtPool;
            gvPool.DataBind();
        }

        protected void gvPool_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Image imgEvaluar = (Image)e.Row.FindControl("imgEvaluar");
                //if (hidFlgVerEvaluarSEC.Value == "S")
                //{
                imgEvaluar.Visible = true;
                //}
                Label lblNroDocumento = (Label)e.Row.FindControl("lblNroDocumento");
                string tipoDocumento = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["TDOCC_CODIGO"]);
                string nroDocumento = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["CLIEC_NUM_DOC"]);
                lblNroDocumento.Text = Funciones.FormatoNroDocumento(tipoDocumento, nroDocumento);
            }
        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            Consultar();
            ScriptManager1.RegisterDataItem(lblMensaje, lblMensaje.Text);
        }

        #region [WebMethod]

        [System.Web.Services.WebMethod()]
        public static BEResponseWebMethod verificarUsuario(string nroSEC, string usuarioActual)
        {
            BEResponseWebMethod objResponse = new BEResponseWebMethod();
            try
            {
                string strUsuario = string.Empty;
                string strCodUsuario = string.Empty;

                new BLSolicitud().ObtenerUsuarioAsignadoSEC(Funciones.CheckInt64(nroSEC), "E", ref strCodUsuario, ref strUsuario);

                if (!string.IsNullOrEmpty(strCodUsuario) && usuarioActual != strCodUsuario)
                {
                    objResponse.Error = true;
                    objResponse.Mensaje = string.Format(ConfigurationManager.AppSettings["consMsjErrorPoolEvaluadorPersona"].ToString(), strUsuario, strCodUsuario);
                }
            }
            catch (Exception ex)
            {
                objResponse.Error = true;
                objResponse.DescripcionError = ex.Message;
                objResponse.Mensaje = ConfigurationManager.AppSettings["consMsjErrorEvaluadorPersona"].ToString();
            }
            return objResponse;
        }

        [System.Web.Services.WebMethod()]
        public static BEResponseWebMethod asignarSECAutomatica(string usuario)
        {
            BEResponseWebMethod objResponse = new BEResponseWebMethod();
            try
            {
                Int64 nroSEC = new BLSolicitud().AsignarSECAutomatica(usuario);
                if (nroSEC == 0)
                {
                    objResponse.Error = true;
                    objResponse.Mensaje = ConfigurationManager.AppSettings["consMsjPoolAsignacionAutomatica"].ToString();
                }
                else
                    objResponse.Cadena = nroSEC.ToString();
            }
            catch (Exception ex)
            {
                objResponse.Error = true;
                objResponse.DescripcionError = ex.Message;
                objResponse.Mensaje = ConfigurationManager.AppSettings["consMsjPoolAsignacionAutomatica"].ToString();
            }
            return objResponse;
        }

        #endregion [WebMethod]

        //INC000003427525 - INI
        public static bool validaPermisos(string strCuentaRed)
        {
            bool respuesta = true;
            string strAbreviaturaPaginasN2 = string.Empty;
            GeneradorLog objLogINC = new GeneradorLog(null, "[INC000003427525]", null, " SEC USUARIO NO AUTORIZADO ");

            try
            {
               

                objLogINC.CrearArchivolog("************************** INICIO METODO VALIDAPERMISOS *************" + "", null, null);

                objLogINC.CrearArchivolog("[INC000003427525] - validaPermisos() -strCuentaRed ==> " + Funciones.CheckStr(strCuentaRed), null, null);

                if (!AccesoUsuario.LeerOpcionesPorUsuario(strCuentaRed))
                {
                    respuesta = false;  
                } 

                strAbreviaturaPaginasN2 = Funciones.CheckStr(HttpContext.Current.Session["AbreviaturaPaginasN2"]);

                objLogINC.CrearArchivolog("[INC000003427525] - validaPermisos() - strAbreviaturaPaginasN2 ==> " + Funciones.CheckStr(strAbreviaturaPaginasN2), null, null);

                if (strAbreviaturaPaginasN2.IndexOf(Funciones.CheckStr(AppSettings.Key_PaginaSec_AntiFraude)) == -1)
                {
                    respuesta = false;
                }

            }
            catch (Exception ex)
            {
                respuesta = false;
                objLogINC.CrearArchivolog("[INC000003427525] - validaPermisos() - ex.Message ==> " + Funciones.CheckStr(ex.Message), null, null);
                objLogINC.CrearArchivolog("[INC000003427525] - validaPermisos() - ex.StackTrace ==> " + Funciones.CheckStr(ex.StackTrace), null, null);
            }

            objLogINC.CrearArchivolog("[INC000003427525] - validaPermisos() -strCuentaRed ==> " + Funciones.CheckStr(respuesta), null, null);
            objLogINC.CrearArchivolog("************************** FIN METODO VALIDAPERMISOS *************" + "", null, null);

            return respuesta;
        }
        //INC000003427525 - FIN

    }
}