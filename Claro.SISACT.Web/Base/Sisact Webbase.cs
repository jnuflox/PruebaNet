using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Claro.SISACT.Common;
using Claro.SISACT.Entity;
using Claro.SISACT.WS;
using System.Runtime.InteropServices;
using System.Web.Services; //PROY-31636
using Claro.SISACT.Web.Comun; //PROY-140126 - IDEA 140248 

namespace Claro.SISACT.Web.Base
{
    public class Sisact_Webbase : System.Web.UI.Page
    {
        private Control _ctlForm;
        private string _focusedControl;
        private StringBuilder scriptPrincipal = new StringBuilder();
        private string dir = "paginas_evaluacion_cons_";
        private static string dirStatic = "paginas_evaluacion_cons_";

        public Sisact_Webbase()
        {
            _ctlForm = this;
        }
        //PROY-140126 - IDEA 140248  INICIO
        protected override void OnInit(EventArgs e)
        {
            WebComunes.CargarAppSettings();
        } 
        //PROY-140126 - IDEA 140248  FIN

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Control PageForm
        {
            get { return _ctlForm; }
            set
            {
                if ((value is System.Web.UI.HtmlControls.HtmlForm || value is System.Web.UI.Page))
                {
                    _ctlForm = value;
                }
                else
                {
                    throw new ArgumentException("PageForm must be set to an HtmlForm or Page object");
                }
            }
        }

        [Browsable(false)]
        public string CurrentUser
        {
            get
            {
                string strDomainUser = Request.ServerVariables["LOGON_USER"];

                string strUser = strDomainUser.Substring(strDomainUser.IndexOf("\\", System.StringComparison.Ordinal) + 1);

                return strUser.ToUpper();
            }
        }

        public static string CurrentUsers
        {
            get
            {
                string strDomainUser = HttpContext.Current.Request.ServerVariables["LOGON_USER"];

                string strUser = strDomainUser.Substring(strDomainUser.IndexOf("\\", System.StringComparison.Ordinal) + 1);

                return  strUser.ToUpper();
            }
        }

        public BEUsuarioSession CurrentUserSession
        {
            get
            {
                return (BEUsuarioSession)Session["Usuario"];
            }
        }

        //PROY-24740
        public static string CurrentTerminal
        {
            get
            {
                string ip_cliente = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(ip_cliente))
                {
                    ip_cliente = HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
                }
                return ip_cliente;
            }
        }
        //INI PROY-SMS PORTABILIDAD
        public static string CurrentServer
        {
            get
            {
                String nombreHost = System.Net.Dns.GetHostName();
                String nombreServer = System.Net.Dns.GetHostName();
                String ipServer = System.Net.Dns.GetHostAddresses(nombreServer)[0].ToString();
                return ipServer;
            }
        }
        //FIN PROY-SMS PORTABILIDAD
        public void SetEnabledAll(bool enabled, Control ctlPageForm)
        {
            Control form = null;
            string controlType = null;

            if (ctlPageForm == null)
                ctlPageForm = this.PageForm;
            controlType = ctlPageForm.ToString();

            if ((ctlPageForm is System.Web.UI.HtmlControls.HtmlForm || ctlPageForm is System.Web.UI.WebControls.Panel || controlType.IndexOf("MultiPage") != -1 || controlType.IndexOf("PageView") != -1))
            {
                form = ctlPageForm;
            }
            else
            {
                if (ctlPageForm is System.Web.UI.Page && (!object.ReferenceEquals(ctlPageForm, PageForm)))
                {
                    form = Sisact_Webbase.FindPageForm((Page)ctlPageForm);
                }
            }

            if ((form == null))
            {
                return;
            }

            foreach (Control ctl in form.Controls)
            {
                if ((ctl is System.Web.UI.WebControls.TextBox || ctl is System.Web.UI.WebControls.DropDownList || ctl is System.Web.UI.WebControls.ListBox || ctl is System.Web.UI.WebControls.CheckBox || ctl is System.Web.UI.WebControls.CheckBoxList || ctl is System.Web.UI.WebControls.RadioButton || ctl is System.Web.UI.WebControls.RadioButtonList))
                {
                    ((WebControl)ctl).Enabled = enabled;
                }
                else
                {
                    controlType = ctl.ToString();
                    if ((ctl is System.Web.UI.WebControls.Panel || controlType.IndexOf("MultiPage") != -1 || controlType.IndexOf("PageView") != -1))
                    {
                        this.SetEnabledAll(enabled, ctl);
                    }
                }
            }
        }

        private static Control FindPageForm(Page cpage)
        {
            Control form = null;

            foreach (Control ctlItem in cpage.Controls)
            {
                if ((ctlItem is System.Web.UI.HtmlControls.HtmlForm))
                {
                    form = ctlItem;
                    break;
                }
            }
            return form;
        }

        public void SetFocusExtended(WebControl ctl)
        {
            if ((ctl != null))
            {
                _focusedControl = ctl.ClientID;
            }
            else
            {
                _focusedControl = null;
            }
        }

        public void SetFocusExtended(string clientId)
        {
            _focusedControl = clientId;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            RegistrarScript(scriptPrincipal.ToString());
        }

        public string GetParam(string pstrNombreParam)
        {
            string strParam = Request[pstrNombreParam];
            if ((strParam == null))
                strParam = "";

            return strParam.Trim();
        }

        public void ManejaError(Exception obExcepcion)
        {
            int iPos = obExcepcion.Message.IndexOf("NO_DATA_FOUND", System.StringComparison.Ordinal);
            if (iPos >= 0)
            {
                scriptPrincipal.AppendLine("alert('No existen datos');");
            }
            else
            {
                Response.Write("<font Color='Red' size='1' face='Verdana'><b>" + obExcepcion.Message + "</b></font>");
            }
        }

        public void Alert(string mensaje)
        {
            mensaje = mensaje.Replace("\n", "\\r");

            scriptPrincipal.AppendLine("alert(" + "\"" + mensaje + "\"" + ");");
        }

        public void AlertYCierra(string mensaje)
        {
            mensaje = mensaje.Replace("\n", "\\r");

            scriptPrincipal.AppendLine("alert(" + "\"" + mensaje + "\"" + ");window.close();");
        }

        public void Script(string script)
        {
            scriptPrincipal.AppendLine(script);
        }

        public void Script(StringBuilder script)
        {
            scriptPrincipal.AppendLine(script.ToString());
        }

        private void RegistrarScript(string scriptSource)
        {
            string scriptHeader = "$(document).ready(function () { " + (char)13;
            string scriptFooter = "});" + (char)13;
            string script = "";

            if (scriptSource != "")
            {
                script = scriptHeader + scriptSource + scriptFooter;

                if (HttpContext.Current.CurrentHandler is Page)
                {
                    Page p = (Page)HttpContext.Current.CurrentHandler;

                    if (ScriptManager.GetCurrent(p) != null)
                    {
                        ScriptManager.RegisterStartupScript(p, Page.GetType(), "script_principal", script, true);
                    }
                    else
                    {
                        p.ClientScript.RegisterStartupScript(Page.GetType(), "script_principal", script, true);
                    }
                }
            }
        }

        public static void redirigir()
        {
            HttpContext.Current.Response.Redirect("http://localhost:1597/inicio.htm");
        }

        [Browsable(false)]
        public static GeneradorLog CrearLogStatic(string idIdentificador)
        {
            var pag = (Page)HttpContext.Current.CurrentHandler;
            GeneradorLog _objlog = new GeneradorLog(null, idIdentificador, null, pag.ToString().Substring(4).Replace(dirStatic, ""));
            return _objlog;
        }

        [Browsable(false)]
        public GeneradorLog CrearLog(string idIdentificador)
        {
            var pag = (Page)HttpContext.Current.CurrentHandler;
            GeneradorLog _objlog = new GeneradorLog(CurrentUser, idIdentificador, null, pag.ToString().Substring(4).Replace(dir, ""));
            return _objlog;
        }

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword,
            int dwLogonType, int dwLogonProvider, out IntPtr phToken);

        public IntPtr IntraTimToken()
        {
            //PROY-24740
            string aplicacion = ConfigurationManager.AppSettings["CONEXION_USRINTRATIM"];
            Claro.SISACT.Configuracion.ConfigConexionFTP oConfigConexionFTP = Claro.SISACT.Configuracion.ConfigConexionFTP.GetInstance(aplicacion);

            IntPtr admin_token = IntPtr.Zero;

            CrearLog("INICIO IntraTimToken()");
            try
            {
                if (LogonUser(oConfigConexionFTP.Parametros.Usuario, oConfigConexionFTP.Parametros.BaseDatos, oConfigConexionFTP.Parametros.Password, 9, 0, out admin_token))
                {
                    CrearLog("OK");
                }
            }
            catch (Exception ex)
            {
                CrearLog("EXCEPTION->" + ex.Message);
            }
            finally
            {
                CrearLog("FIN IntraTimToken()");
            }

            return admin_token;
        }


        #region PROY-31636 | RENTESEG | Bryan Chumbes Lizarraga

        [WebMethod(EnableSession = true)]
        public static JSReadKeySettings getParametros()
        {
            JSReadKeySettings readKeySettings = new JSReadKeySettings();

            readKeySettings.Key_codigoDocMigratorios = ReadKeySettings.Key_codigoDocMigratorios;
            readKeySettings.Key_codigoDocMigraYPasaporte = ReadKeySettings.Key_codigoDocMigraYPasaporte;
            readKeySettings.Key_codDocMigra_Pasaporte_CE = ReadKeySettings.Key_codDocMigra_Pasaporte_CE;
            readKeySettings.Key_codigoDocCIRE = ReadKeySettings.Key_codigoDocCIRE;
            readKeySettings.Key_codigoDocCIE = ReadKeySettings.Key_codigoDocCIE;
            readKeySettings.Key_codigoDocCPP = ReadKeySettings.Key_codigoDocCPP;
            readKeySettings.Key_codigoDocCTM = ReadKeySettings.Key_codigoDocCTM;
            readKeySettings.Key_minLengthDocMigratorios = ReadKeySettings.Key_minLengthDocMigratorios;
            readKeySettings.Key_maxLengthDocMigratorios = ReadKeySettings.Key_maxLengthDocMigratorios;

            //INC000003430042 - INICIO
            readKeySettings.Key_minLengthDocPass = ReadKeySettings.Key_minLengthDocPass;
            //INC000003430042 - FIN

            readKeySettings.Key_codigoDocPasaporte07 = ReadKeySettings.Key_codigoDocPasaporte07;
            readKeySettings.Key_flagPermitirProductosLTE = ReadKeySettings.Key_flagPermitirProductosLTE;
            readKeySettings.Key_docsExistEvaluacionPost = ReadKeySettings.Key_docsExistEvaluacionPost;
            readKeySettings.Key_tipoDocPermitidoPostCAC = ReadKeySettings.Key_tipoDocPermitidoPostCAC;
            readKeySettings.Key_tipoDocPermitidoPostDAC = ReadKeySettings.Key_tipoDocPermitidoPostDAC;
            readKeySettings.Key_tipoDocPermitidoPostCAD = ReadKeySettings.Key_tipoDocPermitidoPostCAD;
            //INI PROY-CAMPANA LG
            readKeySettings.keyComboLG_CampanasAutorizadas = ReadKeySettings.keyComboLG_CampanasAutorizadas;
            readKeySettings.keyComboLG_CampanasReglas = ReadKeySettings.keyComboLG_CampanasReglas;
            readKeySettings.keyComboLG_CampanaMensaje_ClienteConExistencia = ReadKeySettings.keyComboLG_CampanaMensaje_ClienteConExistencia;
            readKeySettings.keyComboLG_CampanaMensaje_ClienteNoAplica = ReadKeySettings.keyComboLG_CampanaMensaje_ClienteNoAplica;
           //INC000002396378
            readKeySettings.Key_EstadoSecRechazado = ReadKeySettings.Key_EstadoSecRechazado;
            //INC000002396378

            //INC000003443673
            readKeySettings.Key_ValorSinApellidoPaternoOMaterno = ReadKeySettings.Key_ValorSinApellidoPaternoOMaterno;
            //INC000003443673

            //FIN PROY-CAMPANA LG

            //PROY-FULLCLARO.V2-INI
            readKeySettings.key_msjBeneficioFija = ReadKeySettings.key_msjBeneficioFija;
            readKeySettings.key_msjBeneficioMovil = ReadKeySettings.key_msjBeneficioMovil;
            //PROY-FULLCLARO.V2-FIN

            //PROY-140457-DEBITO AUTOMATICO-INI
            readKeySettings.Key_flagDebitoAuto = ReadKeySettings.Key_flagDebitoAuto;
            //PROY-140457-DEBITO AUTOMATICO-FIN

            //INC000002977281-INI
            readKeySettings.Key_codFlagBFClaroFijaApagado = ReadKeySettings.Key_codFlagBFClaroFijaApagado;
            //INC000002977281-FIN

            readKeySettings.Key_ReintentosRegistroRRLL = ReadKeySettings.Key_ReintentosRegistroRRLL; //INC000003013199

            /*INICIO PROY-140585- IDEA142064 Mejora en los sistemas de venta*/
            readKeySettings.Key_FlagGeneralOfertaDefault = ReadKeySettings.Key_FlagGeneralOfertaDefault;
            readKeySettings.Key_CanalPermitidoOfertaDefault = ReadKeySettings.Key_CanalPermitidoOfertaDefault;
            readKeySettings.Key_OperacionPermitidaOfertaDefault = ReadKeySettings.Key_OperacionPermitidaOfertaDefault;
            readKeySettings.Key_DocumentosPermitidosOfertaDefault = ReadKeySettings.Key_DocumentosPermitidosOfertaDefault;
            readKeySettings.Key_CodigoOfertaDefault = ReadKeySettings.Key_CodigoOfertaDefault;
            readKeySettings.Key_IsPortabilidadOfertaDefault = ReadKeySettings.Key_IsPortabilidadOfertaDefault;
            /*FIN PROY-140585- IDEA142064 Mejora en los sistemas de venta*/

            //PROY-140618-INI
            readKeySettings.Key_TipoOperPermitida = ReadKeySettings.Key_TipoOperPermitida;
            readKeySettings.Key_TipoProdPermitido = ReadKeySettings.Key_TipoProdPermitido;
            //PROY-140618-FIN

            /*INICIO PROY-140585- IDEA142064 Mejora en los sistemas de venta Fase2*/
            readKeySettings.key_CanalPermMsjeBRMSCamp = ReadKeySettings.key_CanalPermMsjeBRMSCamp;
            readKeySettings.key_OperaPermMsjeBRMSCamp = ReadKeySettings.key_OperaPermMsjeBRMSCamp;
            readKeySettings.key_MsjAplicaCuotas = ReadKeySettings.key_MsjAplicaCuotas;
            /*FIN PROY-140585- IDEA142064 Mejora en los sistemas de venta Fase2*/

            //INICIATIVA - 733 - INI - C21
            readKeySettings.Key_CodEquipoIPTV = ReadKeySettings.Key_CodEquipoIPTV;
            readKeySettings.Key_CodClaroVideoIPTV = ReadKeySettings.Key_CodClaroVideoIPTV;
            readKeySettings.Key_strMensajeClaroVideoIPTV = ReadKeySettings.Key_strMensajeClaroVideoIPTV;
            readKeySettings.Key_CodGrupoServicio = ReadKeySettings.Key_CodGrupoServicio;
            //INICIATIVA - 733 - FIN - C21

            //INICIATIVA - 803 - INI
            readKeySettings.Key_FlagAprobPrecio = ReadKeySettings.Key_FlagAprobPrecio;
            readKeySettings.Key_MsjFactorSubsidio = ReadKeySettings.Key_MsjFactorSubsidio;
            readKeySettings.Key_MsjIngresarIdPedido = ReadKeySettings.Key_MsjIngresarIdPedido;
            readKeySettings.Key_PorcentSubsidio = ReadKeySettings.Key_PorcentSubsidio;
            readKeySettings.Key_MsjErrorExcepPrec = ReadKeySettings.Key_MsjErrorExcepPrec;
            readKeySettings.Key_FlagApagadoExcepcionPrecio = ReadKeySettings.Key_FlagApagadoExcepcionPrecio;
            readKeySettings.Key_FlagApagadoValidacionSubsidio = ReadKeySettings.Key_FlagApagadoValidacionSubsidio;
            //INICIATIVA - 803 - FIN

            //PROY-140736 INI
            readKeySettings.Key_CodCampaniaBuyBack = ReadKeySettings.Key_CodCampaniaBuyBack;
            readKeySettings.Key_Max_Length_Cupon = ReadKeySettings.Key_Max_Length_Cupon;
            readKeySettings.Key_Min_Length_IMEI = ReadKeySettings.Key_Min_Length_IMEI;
            readKeySettings.Key_Max_Length_IMEI = ReadKeySettings.Key_Max_Length_IMEI;
            readKeySettings.Key_Msj_Error_CBO_BuyBack = ReadKeySettings.Key_Msj_Error_CBO_BuyBack;
            readKeySettings.Key_Msj_Error_Cupon_BuyBack = ReadKeySettings.Key_Msj_Error_Cupon_BuyBack;
            readKeySettings.Key_Msj_Error_Fila_BuyBack = ReadKeySettings.Key_Msj_Error_Fila_BuyBack;
            readKeySettings.Key_Msj_Error_Igual_Cupon=ReadKeySettings.Key_Msj_Error_Igual_Cupon;
            readKeySettings.Key_Msj_Error_Igual_Imei = ReadKeySettings.Key_Msj_Error_Igual_Imei;
            readKeySettings.Key_Ingr_Cupon_Buyback = ReadKeySettings.Key_Ingr_Cupon_Buyback;
            readKeySettings.Key_Ingr_IMEI_Buyback = ReadKeySettings.Key_Ingr_IMEI_Buyback;
            readKeySettings.Key_Sel_Equipo_Buyback = ReadKeySettings.Key_Sel_Equipo_Buyback;
            //PROY-140736 FIN

            //INI PROY-140739 Formulario Leads
            readKeySettings.KeyLeadsCanal=ReadKeySettings.KeyLeadsCanal;
            readKeySettings.KeyLeadsTopenPostPago=ReadKeySettings.KeyLeadsTopenPostPago;
            readKeySettings.KeyLeadsPDV=ReadKeySettings.KeyLeadsPDV;
            readKeySettings.KeyLeadsFlag=ReadKeySettings.KeyLeadsFlag;
            readKeySettings.KeyLeadsMensajeObligatorio = ReadKeySettings.KeyLeadsMensajeObligatorio;
            readKeySettings.KeyLeadsMaxLength = ReadKeySettings.KeyLeadsMaxLength;
            readKeySettings.KeyLeadsProductosPermitidosPostpago = ReadKeySettings.KeyLeadsProductosPermitidosPostpago;
            //FIN PROY-140739 Formulario Leads

			//INICIO PROY-140546
            readKeySettings.Key_CadValorFormaPago = ReadKeySettings.ConsCadValorFormaPago;
            readKeySettings.Key_CanalesPermitidosCAI = ReadKeySettings.Key_CanalesPermitidosCAI;
            readKeySettings.Key_MontoDescuentoPorFullClaroCAI = ReadKeySettings.Key_MontoDescuentoPorFullClaroCAI;
			//FIN PROY-140546

            readKeySettings.Key_MensajeMaiMayor = ReadKeySettings.Key_MensajeMaiMayor; //FALLAS PROY-140546
            readKeySettings.Key_FlagGeneralCobertura = ReadKeySettings.Key_FlagGeneralCobertura; //INICIATIVA-932

            readKeySettings.Key_MsgCoberturaIFI = ReadKeySettings.Key_MsgCoberturaIFI; //INICIATIVA-992

            #region [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] | [Keys de parametros]
            readKeySettings.Key_OpcionSinPromocion = ReadKeySettings.Key_OpcionSinPromocion;
            readKeySettings.Key_TipoMatVentaVarias = ReadKeySettings.Key_TipoMatVentaVarias;
            readKeySettings.Key_FlagGeneralVtaCuotas = ReadKeySettings.Key_FlagGeneralVtaCuotas;
            readKeySettings.Key_MsjAccNoCuotas = ReadKeySettings.Key_MsjAccNoCuotas;
            readKeySettings.Key_MsjClienteNoAplicaCuotas = ReadKeySettings.Key_MsjClienteNoAplicaCuotas;
            readKeySettings.Key_MsjNoAplicaCuotas = ReadKeySettings.Key_MsjNoAplicaCuotas;
            readKeySettings.Key_OpcionLineaCuenta = ReadKeySettings.Key_OpcionLineaCuenta;
            readKeySettings.Key_MsjSelecAcc = ReadKeySettings.Key_MsjSelecAcc;
            readKeySettings.Key_MsjSelecLineaCuenta = ReadKeySettings.Key_MsjSelecLineaCuenta;
            readKeySettings.Key_MsjVntaCuotasBRMS = ReadKeySettings.Key_MsjVntaCuotasBRMS;
            #endregion

            return readKeySettings;
        }

        #endregion

        //PROY-26963-F3 - GPRD - PROMFACT
        public static string CurrentNodo
        {
            get
            {
                string nodo_Servidor = Environment.MachineName;
                if (!string.IsNullOrEmpty(nodo_Servidor) && nodo_Servidor.Length>2)
                {
                    return nodo_Servidor.Substring(nodo_Servidor.Length - 2);
                }
                return string.Empty;
            }
        }
      //PROY-26963-F3 - GPRD - PROMFACT


        //INICIATIVA-803
        public static string CurrentHostName
        {
            get
            {
                string nodo_Servidor = System.Net.Dns.GetHostName(); 
                return nodo_Servidor;
                 
            }
        }
        //INICIATIVA-803
    }
}
