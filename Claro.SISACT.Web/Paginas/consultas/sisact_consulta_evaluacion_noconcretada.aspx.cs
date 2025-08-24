using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Claro.SISACT.Web.Base;
using System.Configuration;
using Claro.SISACT.Entity;
using Claro.SISACT.Business;
using Claro.SISACT.Entity.IteracionCliente.Response;
using Claro.SISACT.WS.RestReferences;
using Claro.SISACT.Web.Comun;
using Claro.SISACT.Common;
using System.Web.Services;
//PROY-140618
namespace Claro.SISACT.Web.Paginas.consultas
{
    public partial class sisact_consulta_evaluacion_noconcretada : Sisact_Webbase
    {
        public object CurrentPage
        {

            get
            {
                return ViewState["CurrentPage"];
            }
            set
            {
                ViewState["CurrentPage"] = value;
            }
        }
        GeneradorLog objLog_ = null;

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

            HttpContext.Current.Session["CurrentTerminal"] = null;
            HttpContext.Current.Session["CurrentUser"] = null;

            string codUsuarioExt = Request.QueryString["cu"];

            HttpContext.Current.Session["CurrentTerminal"] = CurrentTerminal;
            HttpContext.Current.Session["CurrentUser"] = CurrentUser;

            if (!Base.AccesoUsuario.ValidarAcceso(codUsuarioExt, CurrentUser))
            {
                string strRutaSite = ConfigurationManager.AppSettings["RutaSite"];
                Response.Redirect(strRutaSite);
                return;
            }
            objLog_ = CrearLog(CurrentUsers);
            if (!Page.IsPostBack)
            {
                HttpContext.Current.Session["oUsuario"] = (BEUsuarioSession)Session["Usuario"];
                Inicio();
            }            
        }

        private void Inicio()
        {
            objLog_.CrearArchivolog("PROY-140618  IDEA-142181","INI metodo Inicio()",null);
            //llenar tipo documento
            String docsExtPermitidos = ReadKeySettings.Key_tipoDocPermitidoPostCAC;
            List<BETipoDocumento> lstDocumento = (new BLGeneral()).ListarTipoDocumento();
            if (lstDocumento.Count > 0)
            {
                lstDocumento.RemoveAll(x => !docsExtPermitidos.Contains(x.ID_SISACT) && !ReadKeySettings.Key_docsExistEvaluacionPost.Contains(x.ID_SISACT));
                ddlTipoDocumentoEvaCli.DataSource = lstDocumento;
                ddlTipoDocumentoEvaCli.DataValueField = "ID_SISACT";
                ddlTipoDocumentoEvaCli.DataTextField = "DESCRIPCION";
                ddlTipoDocumentoEvaCli.DataBind();
            }
            

            ddlTipoDocumentoEvaCli.SelectedValue = Funciones.CheckStr(ConfigurationManager.AppSettings["constCodTipoDocumentoDNI"]);
            objLog_.CrearArchivolog("PROY-140618  IDEA-142181", "FIN metodo Inicio()", null);
            txtNroDocumento.Focus();
            hidMostrar.Value = "0";

            objLog_.CrearArchivolog("PROY-140618  IDEA-142181", hidMostrar.Value, null);
        }
        protected void btnLimpiarEvaclie_Click(object sender, EventArgs e)
        {
            txtApellidoEva.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtNacionalidad.Text = string.Empty;
            txtNombresEva.Text = string.Empty;
            txtNroDocumento.Text = string.Empty;
            txtTipoDocumentoEva.Text = string.Empty;
            txtNroDocumentoEva.Text = string.Empty;
            ddlTipoDocumentoEvaCli.SelectedValue = Funciones.CheckStr(ConfigurationManager.AppSettings["constCodTipoDocumentoDNI"]);
            txtNroDocumento.Focus();
            dgEvaluacion.DataSource = null;
            dgEvaluacion.DataBind();
            hidMostrar.Value = "0";
        }
        protected void btnConsultarEvaclie_Click(object sender, EventArgs e)
        {
            objLog_.CrearArchivolog("PROY-140618  IDEA-142181", " ******************************************* INI btnConsultarEvaclie_Click *******************************************", null);
            hidMostrar.Value = "0";
            try
            {
                RestIteracionCliente objRestReference = new RestIteracionCliente();
                Dictionary<string, string> dcParameters = new Dictionary<string, string>();

                dcParameters.Add("tipoDoc", Funciones.CheckStr(ddlTipoDocumentoEvaCli.SelectedValue));
                dcParameters.Add("nroDoc", Funciones.CheckStr(txtNroDocumento.Text));
                dcParameters.Add("codOficina", string.Empty);
                dcParameters.Add("diasConsulta", Funciones.CheckStr(AppSettings.Key_DiasAntiguedad));
                dcParameters.Add("cantRegistros", Funciones.CheckStr(AppSettings.Key_CantidadRegistros));

                objLog_.CrearArchivolog(string.Format("PROY-140618  IDEA-142181 | tipoDoc : {0} ", ddlTipoDocumentoEvaCli.SelectedValue), null, null);
                objLog_.CrearArchivolog(string.Format("PROY-140618  IDEA-142181 | nroDoc : {0} ", txtNroDocumento.Text), null, null);
                objLog_.CrearArchivolog(string.Format("PROY-140618  IDEA-142181 | codOficina : {0} ", string.Empty), null, null);
                objLog_.CrearArchivolog(string.Format("PROY-140618  IDEA-142181 | diasConsulta : {0} ", Funciones.CheckStr(AppSettings.Key_DiasAntiguedad)), null, null);
                objLog_.CrearArchivolog(string.Format("PROY-140618  IDEA-142181 | cantRegistros : {0} ", Funciones.CheckStr(AppSettings.Key_CantidadRegistros)), null, null);

                BEAuditoriaRequest objBeAuditoriaRequest = new BEAuditoriaRequest();

                objBeAuditoriaRequest.idTransaccion = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                objBeAuditoriaRequest.timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
                objBeAuditoriaRequest.userId = CurrentUsers;
                objBeAuditoriaRequest.msgid = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                objBeAuditoriaRequest.dataPower = true;
                objBeAuditoriaRequest.urlTimeOut_Rest = Funciones.CheckStr(ConfigurationManager.AppSettings["ConsMejorasPorta_TimeOut"]);
                
                objLog_.CrearArchivolog(string.Format("PROY-140618  IDEA-142181 | objBeAuditoriaRequest.idTransaccion : {0} ", objBeAuditoriaRequest.idTransaccion), null, null);
                objLog_.CrearArchivolog(string.Format("PROY-140618  IDEA-142181 | objBeAuditoriaRequest.timestamp : {0} ", objBeAuditoriaRequest.timestamp), null, null);
                objLog_.CrearArchivolog(string.Format("PROY-140618  IDEA-142181 | objBeAuditoriaRequest.userId : {0} ", objBeAuditoriaRequest.userId), null, null);
                objLog_.CrearArchivolog(string.Format("PROY-140618  IDEA-142181 | objBeAuditoriaRequest.msgid : {0} ", objBeAuditoriaRequest.msgid), null, null);
                objLog_.CrearArchivolog(string.Format("PROY-140618  IDEA-142181 | objBeAuditoriaRequest.dataPower : {0} ", Funciones.CheckStr(objBeAuditoriaRequest.dataPower)), null, null);
                objLog_.CrearArchivolog(string.Format("PROY-140618  IDEA-142181 | objBeAuditoriaRequest.urlTimeOut_Rest : {0} ", Funciones.CheckStr(objBeAuditoriaRequest.urlTimeOut_Rest)), null, null);

                //consulta claves
                objBeAuditoriaRequest.wsIp = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_ConsultaNacionalidad_wsIp"]);
                objBeAuditoriaRequest.ipTransaccion = Funciones.CheckStr(HttpContext.Current.Session["CurrentTerminal"]);
                objBeAuditoriaRequest.usuarioAplicacion = Funciones.CheckStr(HttpContext.Current.Session["CurrentUser"]);

                objLog_.CrearArchivolog(string.Format("PROY-140618  IDEA-142181 | objBeAuditoriaRequest.wsIp  : {0} ", objBeAuditoriaRequest.wsIp), null, null);
                objLog_.CrearArchivolog(string.Format("PROY-140618  IDEA-142181 | objBeAuditoriaRequest.ipTransaccion : {0} ", Funciones.CheckStr(objBeAuditoriaRequest.ipTransaccion)), null, null);
                objLog_.CrearArchivolog(string.Format("PROY-140618  IDEA-142181 | objBeAuditoriaRequest.usuarioAplicacion : {0} ", Funciones.CheckStr(objBeAuditoriaRequest.usuarioAplicacion)), null, null);

                objLog_.CrearArchivolog("PROY-140618  IDEA-142181", "*****************************************restConsultarEvaluacion*****************************************", null);
                ConsultarEvaluacionResponse obj = objRestReference.restConsultarEvaluacion(dcParameters, objBeAuditoriaRequest);
                objLog_.CrearArchivolog("PROY-140618  IDEA-142181", "*****************************************restConsultarEvaluacion*****************************************", null);


                ConsultarEvaluacionResponseBody objResponseBody = (ConsultarEvaluacionResponseBody)obj.getMessageResponse().getBody();
                BEDatosCabecera objcabecera = new BEDatosCabecera();
                objLog_.CrearArchivolog(string.Format("PROY-140618  IDEA-142181 | objResponseBody.consultarEvaluacionResponse.codigoRespuesta : {0} ", Funciones.CheckStr(objResponseBody.consultarEvaluacionResponse.codigoRespuesta)), null, null);
                objLog_.CrearArchivolog(string.Format("PROY-140618  IDEA-142181 | objResponseBody.consultarEvaluacionResponse.mensajeRespuesta : {0} ", Funciones.CheckStr(objResponseBody.consultarEvaluacionResponse.mensajeRespuesta)), null, null);
                objLog_.CrearArchivolog(string.Format("PROY-140618  IDEA-142181 | objResponseBody.consultarEvaluacionResponse.mensajeError : {0} ", Funciones.CheckStr(objResponseBody.consultarEvaluacionResponse.mensajeError)), null, null);

                if (objResponseBody.consultarEvaluacionResponse.codigoRespuesta == "0" && objResponseBody.consultarEvaluacionResponse.listaCursor != null && objResponseBody.consultarEvaluacionResponse.listaDetalle != null)
                {
                    objLog_.CrearArchivolog(string.Format("PROY-140618  IDEA-142181 | objBeAuditoriaRequest.usuarioAplicacion : {0} ", Funciones.CheckStr(objBeAuditoriaRequest.usuarioAplicacion)), null, null);
                    Int64 countList = objResponseBody.consultarEvaluacionResponse.listaDetalle.Count();
                    string[] arrImporteRA = new string[countList - 1];
                    double importeRA = 0;
                    double lcDisponible = 0;
                    double cargoFijo = 0;
                    for (int j = 0; j < countList; j++)
                    {
                        importeRA = Funciones.CheckDbl(objResponseBody.consultarEvaluacionResponse.listaDetalle[j].importeRa);
                        lcDisponible = Funciones.CheckDbl(objResponseBody.consultarEvaluacionResponse.listaDetalle[j].lcDisponible);
                        cargoFijo = Funciones.CheckDbl(objResponseBody.consultarEvaluacionResponse.listaDetalle[j].cargoFijo);
                        objResponseBody.consultarEvaluacionResponse.listaDetalle[j].importeRa = importeRA == 0 ? "0.00" : string.Format("{0:n2}", (Math.Truncate(importeRA * 100) / 100));
                        objResponseBody.consultarEvaluacionResponse.listaDetalle[j].lcDisponible = lcDisponible == 0 ? "0.00" : string.Format("{0:n2}", (Math.Truncate(lcDisponible * 100) / 100));
                        objResponseBody.consultarEvaluacionResponse.listaDetalle[j].cargoFijo = cargoFijo == 0 ? "0.00" : string.Format("{0:n2}", (Math.Truncate(cargoFijo * 100) / 100));
                    }

                    objcabecera = objResponseBody.consultarEvaluacionResponse.listaCursor[0];
                    //cabecera
                    if (objcabecera.tipoDoc == ConfigurationManager.AppSettings["constDesTipoDocumentoRUC"] && objcabecera.nroDoc.Substring(0, 2) == "20")
                    {
                        txtRazonSocial.Text = Funciones.CheckStr(string.Format("{0}", objcabecera.nombre)).ToUpper();
                        objLog_.CrearArchivolog(string.Format("PROY-140618  IDEA-142181 | txtRazonSocial.Text : {0} ", Funciones.CheckStr(txtRazonSocial.Text)), null, null);
                    }
                    else
                    {
                    txtNombresEva.Text = Funciones.CheckStr(objcabecera.nombre).ToUpper();
                    objLog_.CrearArchivolog(string.Format("PROY-140618  IDEA-142181 | txtNombresEva.Text : {0} ", Funciones.CheckStr(txtNombresEva.Text)), null, null);
                    txtApellidoEva.Text = Funciones.CheckStr(string.Format("{0} {1}", objcabecera.apePaterno, objcabecera.apeMaterno)).ToUpper();
                    objLog_.CrearArchivolog(string.Format("PROY-140618  IDEA-142181 | txtApellidoEva.Text : {0} ", Funciones.CheckStr(txtApellidoEva.Text)), null, null);
                    }

                    txtTipoDocumentoEva.Text = objcabecera.tipoDoc;
                    objLog_.CrearArchivolog(string.Format("PROY-140618  IDEA-142181 | txtTipoDocumentoEva.Text : {0} ", Funciones.CheckStr(txtTipoDocumentoEva.Text)), null, null);
                    txtNroDocumentoEva.Text = objcabecera.nroDoc;
                    objLog_.CrearArchivolog(string.Format("PROY-140618  IDEA-142181 | txtNroDocumentoEva.Text : {0} ", Funciones.CheckStr(txtNroDocumentoEva.Text)), null, null);
                    txtEmail.Text = Funciones.CheckStr(objcabecera.email).ToUpper();
                    objLog_.CrearArchivolog(string.Format("PROY-140618  IDEA-142181 | txtEmail.Text : {0} ", Funciones.CheckStr(txtEmail.Text)), null, null);
                    txtNacionalidad.Text = Funciones.CheckStr(objcabecera.nacionalidad).ToUpper();
                    objLog_.CrearArchivolog(string.Format("PROY-140618  IDEA-142181 | txtNacionalidad.Text : {0} ", Funciones.CheckStr(txtNacionalidad.Text)), null, null);

                    dgEvaluacion.DataSource = objResponseBody.consultarEvaluacionResponse.listaDetalle;
                    dgEvaluacion.DataBind();
                    CurrentPage = objResponseBody.consultarEvaluacionResponse.listaDetalle;
                    HttpContext.Current.Session["listaDetalle"] = objResponseBody.consultarEvaluacionResponse.listaDetalle;
                    hidMostrar.Value = "1";
                }
                else
                {
                    HttpContext.Current.Session["listaDetalle"] = null;
                    dgEvaluacion.DataSource = null;
                    dgEvaluacion.DataBind();
                    objLog_.CrearArchivolog(string.Format("PROY-140618  IDEA-142181 | btnConsultarEvaclie_Click : codigoRespuesta {0}", objResponseBody.consultarEvaluacionResponse.codigoRespuesta), null, null);
                    objLog_.CrearArchivolog(string.Format("PROY-140618  IDEA-142181 | btnConsultarEvaclie_Click : mensajeRespuesta {0}", objResponseBody.consultarEvaluacionResponse.mensajeRespuesta), null, null);
                    Response.Write("<script language=javascript>alert('" + objResponseBody.consultarEvaluacionResponse.mensajeRespuesta + "');</script>");
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Session["listaDetalle"] = null;
                objLog_.CrearArchivolog("PROY-140618  IDEA-142181", "ERROR btnConsultarEvaclie_Click", ex);
                Response.Write("<script language=javascript>alert('A ocurrido un error en la busqueda " + ex.Message + "');</script>");
            }
            objLog_.CrearArchivolog("PROY-140618  IDEA-142181", " ******************************************* FIN btnConsultarEvaclie_Click *******************************************", null);
        }
        protected void dgEvaluacion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
        }

        protected void dgEvaluacion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            objLog_.CrearArchivolog("PROY-140618  IDEA-142181", " INI dgEvaluacion_PageIndexChanging", null);
            objLog_.CrearArchivolog("PROY-140618  IDEA-142181", " INI dgEvaluacion_PageIndexChanging : e.NewPageIndex " + e.NewPageIndex, null);
            dgEvaluacion.PageIndex = e.NewPageIndex;
            dgEvaluacion.DataSource = CurrentPage;
            dgEvaluacion.DataBind();
            objLog_.CrearArchivolog("PROY-140618  IDEA-142181", " FIN dgEvaluacion_PageIndexChanging", null);
        }
        [WebMethod()]
        public static string consultarServicios(string idEvaluacion)
        {
            List<BEDatosDetalle> detalle = (List<BEDatosDetalle>)HttpContext.Current.Session["listaDetalle"];

            return (detalle).Where(z => z.idEval == idEvaluacion).FirstOrDefault().servAdicional;
        }
    }
}