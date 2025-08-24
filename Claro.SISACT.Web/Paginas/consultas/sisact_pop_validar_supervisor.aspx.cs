using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.DirectoryServices;
using Claro.SISACT.Common;
using System.Configuration;
using Claro.SISACT.Entity;
using Claro.SISACT.Business;
using Claro.SISACT.Web.Comun;
using Claro.SISACT.WS;
using System.Collections;
using System.Text;

namespace Claro.SISACT.Web.Paginas.consultas
{
    public partial class sisact_pop_validar_supervisor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           CargarCausalOmision();
        }

        [System.Web.Services.WebMethod()]
        public static BEResponseWebMethod ValidarSupervisor(string vUsuario, string vClave, string causalOmision)
        {
            GeneradorLog objLog = new GeneradorLog(null, "POPUP_ValidaSupervisor", null, "log_PopUp_ValidaSupervisor");
            BEResponseWebMethod objResponse = new BEResponseWebMethod();
            bool bolCodPerfil = false;
            string strMensaje = string.Empty;
            HttpContext.Current.Session["ValidacionSupervisor"] = false;
            HttpContext.Current.Session["UsuarioSupervisor"] = vUsuario;
            HttpContext.Current.Session["CausalOmision"] = string.Empty;
            objLog.CrearArchivolog(string.Format("{0} --> {1}", "[PROY-140419 Autorizar Portabilidad sin PIN][ValidarSupervisor][INICIO]", ""), null, null);
            objLog.CrearArchivolog(string.Format("{0} --> {1}", "[PROY-140419 Autorizar Portabilidad sin PIN][PARAMETRICA][OUT | strUsuarioSuperv]", ReadKeySettings.key_SmsPortaSupervPerfil), null, null);
            objLog.CrearArchivolog("[PROY-140419 Autorizar Portabilidad sin PIN][PARAMETRICA][FIN]", null, null);
            try
            {
                bolCodPerfil = ConsultarPerfiles(vUsuario, vClave, ref strMensaje);
                if (bolCodPerfil)
                {
                    objResponse.CodigoError = "0";
                    HttpContext.Current.Session["ValidacionSupervisor"] = true;
                    HttpContext.Current.Session["CausalOmision"] = causalOmision;
                    objLog.CrearArchivolog(string.Format("{0} --> {1}", "[PROY-140419 Autorizar Portabilidad sin PIN][ValidarSupervisor][Validacion de usuario correcta] -> CodSupervisor : ", vUsuario), null, null);
                }
                else
                {
                    objResponse.CodigoError = "1";
                    objLog.CrearArchivolog(string.Format("{0} --> {1}", "[PROY-140419 Autorizar Portabilidad sin PIN][ValidarSupervisor][Validacion de usuario incorrecta] -> CodSupervisor : ", vUsuario), null, null);
                }
                objLog.CrearArchivolog(string.Format("{0} --> {1}", "[PROY-140419 Autorizar Portabilidad sin PIN][ValidarSupervisor] -> Mensaje : ", strMensaje), null, null);
                objResponse.Mensaje = strMensaje;
            }
            catch (Exception ex)
            {
                objResponse.CodigoError = "-1";
                objResponse.Mensaje = String.Format("{0} --> {1} | {2}", "[ERROR][ValidarSupervisor]", Funciones.CheckStr(ex.Message), Funciones.CheckStr(ex.StackTrace));
                objLog.CrearArchivolog("[ERROR][ValidarSupervisor]", null, ex);
            }
            objLog.CrearArchivolog(string.Format("{0} --> {1}", "[PROY-140419 Autorizar Portabilidad sin PIN][ValidarSupervisor][Usuario]", (bool)HttpContext.Current.Session["ValidacionSupervisor"]), null, null);
            objLog.CrearArchivolog(string.Format("{0} --> {1}", "[PROY-140419 Autorizar Portabilidad sin PIN][ValidarSupervisor][FIN]", ""), null, null);
            objResponse.Boleano = bolCodPerfil;
            return objResponse;
        }

        private static bool isAuthenticated(string vUsuario, string vClave)
        {
            GeneradorLog objLog = new GeneradorLog(null, "POPUP_ValidaSupervisor", null, "log_PopUp_ValidaSupervisor");
            objLog.CrearArchivolog(string.Format("{0} --> {1}", "[PROY-140419 Autorizar Portabilidad sin PIN][isAuthenticated][INICIO]", ""), null, null);
            objLog.CrearArchivolog(string.Format("{0} --> {1}", "[PROY-140419 Autorizar Portabilidad sin PIN][isAuthenticated] -> UsuarioSupervisor : ", vUsuario), null, null);
            string strDominio = Funciones.CheckStr(ConfigurationManager.AppSettings["DominioLDAP"]);
            objLog.CrearArchivolog(string.Format("{0} --> {1}", "[PROY-140419 Autorizar Portabilidad sin PIN][isAuthenticated] -> strDominio : ", strDominio), null, null);

            try
            {
                DirectoryEntry entry = new DirectoryEntry(strDominio, vUsuario, vClave);
                object obj = entry.NativeObject;
                objLog.CrearArchivolog(string.Format("{0} --> {1}", "[PROY-140419 Autorizar Portabilidad sin PIN][isAuthenticated] -> obj : ", Funciones.CheckStr(obj)), null, null);
                DirectorySearcher search = new DirectorySearcher(entry);
                objLog.CrearArchivolog(string.Format("{0} --> {1}", "[PROY-140419 Autorizar Portabilidad sin PIN][isAuthenticated] -> search : ", Funciones.CheckStr(search)), null, null);
                search.Filter = "(SAMAccountName=" + vUsuario + ")";
                search.PropertiesToLoad.Add("cn");
                SearchResult result;
                result = search.FindOne();
                if (result == null)
                {
                    objLog.CrearArchivolog("[PROY-140419 Autorizar Portabilidad sin PIN][isAuthenticated] -> Result: NUll ", null, null);
                    objLog.CrearArchivolog(string.Format("{0} --> {1}", "[PROY-140419 Autorizar Portabilidad sin PIN][isAuthenticated][FIN]", ""), null, null);
                    search.Dispose();
                    entry.Dispose();
                    return false;
                }
                else
                {
                    objLog.CrearArchivolog(string.Format("{0} --> {1}", "[PROY-140419 Autorizar Portabilidad sin PIN][isAuthenticated] -> Result : ", Funciones.CheckStr(result)), null, null);
                    objLog.CrearArchivolog(string.Format("{0} --> {1}", "[PROY-140419 Autorizar Portabilidad sin PIN][isAuthenticated][FIN]", ""), null, null);
                    search.Dispose();
                    entry.Dispose();
                    return true;
                }
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog(string.Format("{0} --> {1}", "[PROY-140419 Autorizar Portabilidad sin PIN][isAuthenticated] -> Exception: ", ex), null, null);
                objLog.CrearArchivolog(string.Format("{0} --> {1}", "[PROY-140419 Autorizar Portabilidad sin PIN][isAuthenticated][FIN]", ""), null, null);
                return false;
            }
        }

        private static bool ConsultarPerfiles(string pUsuario, string pClave, ref string strMensaje)
        {
            bool bolPerfilAutorizado = false;
            GeneradorLog objLog = new GeneradorLog(null, "POPUP_ValidaSupervisor", null, "log_PopUp_ValidaSupervisor");

            objLog.CrearArchivolog(string.Format("{0} --> {1}", "[PROY-140419 Autorizar Portabilidad sin PIN][ConsultarPerfiles][INICIO]", ""), null, null);
            objLog.CrearArchivolog(string.Format("{0} --> {1}", "[PROY-140419 Autorizar Portabilidad sin PIN][ConsultarPerfiles][IN | Usuario]", pUsuario), null, null);
            objLog.CrearArchivolog(string.Format("{0} --> {1}", "[PROY-140419 Autorizar Portabilidad sin PIN][ConsultarPerfiles][IN | Perfiles]", ReadKeySettings.key_SmsPortaSupervPerfil), null, null);

            try
            {
                if (isAuthenticated(pUsuario, pClave))
                {
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[PROY-140419 Autorizar Portabilidad sin PIN][ConsultarPerfiles][Inicio ConsultaSeguridad]", ""), null, null);
                    string idTrans = Funciones.CheckStr(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                    Int64 codApp = Funciones.CheckInt64(ConfigurationManager.AppSettings["CodigoAplicacion"]);
                    string ipApp = Funciones.CheckStr(Claro.SISACT.Web.Base.Sisact_Webbase.CurrentServer);
                    string nomApp = Funciones.CheckStr(ConfigurationManager.AppSettings["constNombreAplicacion"]);
                    string errorMsg = string.Empty;
                    string errorCod = string.Empty;
                    BWConsultaSeguridad cs = new BWConsultaSeguridad();
                    List<BEConsultaSeguridad> lstPerfiles = new List<BEConsultaSeguridad>();
                    string[] lstPerfParametros = null;

                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[PROY-140419 Autorizar Portabilidad sin PIN][ConsultarPerfiles][Inicio verificaUsuario]", ""), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[PROY-140419 Autorizar Portabilidad sin PIN][ConsultarPerfiles][verificaUsuario][IN|idTrans]", idTrans), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[PROY-140419 Autorizar Portabilidad sin PIN][ConsultarPerfiles][verificaUsuario][IN|ipApp]", ipApp), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[PROY-140419 Autorizar Portabilidad sin PIN][ConsultarPerfiles][verificaUsuario][IN|nomApp]", nomApp), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[PROY-140419 Autorizar Portabilidad sin PIN][ConsultarPerfiles][verificaUsuario][IN|pUsuario]", pUsuario), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[PROY-140419 Autorizar Portabilidad sin PIN][ConsultarPerfiles][verificaUsuario][IN|codApp]", codApp), null, null);

                    lstPerfiles = cs.verificaUsuario(ref idTrans, ipApp, nomApp, pUsuario, codApp, ref errorMsg, ref errorCod);

                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[PROY-140419 Autorizar Portabilidad sin PIN][ConsultarPerfiles][verificaUsuario][IN|lstPerfiles]", lstPerfiles.Count()), null, null);

                    if (lstPerfiles.Count > 0)
                    {
                        objLog.CrearArchivolog(string.Format("{0}-->{1}", "[PROY-140419 Autorizar Portabilidad sin PIN][ConsultarPerfiles][Si cuenta con perfiles]", codApp), null, null);

                        lstPerfParametros = ReadKeySettings.key_SmsPortaSupervPerfil.Split('|');

                        foreach (var item in lstPerfiles)
                        {
                            for (int x = 0; x < lstPerfParametros.Length; x++)
                            {
                                if (lstPerfParametros[x] == item.PERFCCOD)
                                {
                                    bolPerfilAutorizado = true;
                                    break;
                                }
                            }
                        }

                        if (bolPerfilAutorizado)
                        {
                            strMensaje = Funciones.CheckStr(ReadKeySettings.key_MsjExito);
                        }
                        else
                        {
                            strMensaje = Funciones.CheckStr(ReadKeySettings.key_MsjPerfilSinPermisos);
                        }
                        objLog.CrearArchivolog(string.Format("{0}-->{1}", "[PROY-140419 Autorizar Portabilidad sin PIN][ConsultarPerfiles][verificaUsuario][OUT|errorMsg]", errorMsg), null, null);
                        objLog.CrearArchivolog(string.Format("{0}-->{1}", "[PROY-140419 Autorizar Portabilidad sin PIN][ConsultarPerfiles][verificaUsuario][OUT|errorCod]", errorCod), null, null);
                        objLog.CrearArchivolog(string.Format("{0}-->{1}", "[PROY-140419 Autorizar Portabilidad sin PIN][ConsultarPerfiles][verificaUsuario][OUT|lstPerfiles]", lstPerfiles), null, null);
                    }
                    else
                    {
                        strMensaje = Funciones.CheckStr(ReadKeySettings.key_MsjPerfilSinPermisos);
                    }
                }
                else
                {
                    strMensaje = Funciones.CheckStr(ReadKeySettings.key_MsjPerfilIncorrecto);
                }
            }
            catch (Exception ex)
            {
                strMensaje = ConfigurationManager.AppSettings["consMsjErrorGeneral"];
                objLog.CrearArchivolog("[PROY-140419 Autorizar Portabilidad sin PIN][ERROR][ConsultarPerfiles]", null, ex);
            }
            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[PROY-140419 Autorizar Portabilidad sin PIN][ConsultarPerfiles][verificaUsuario][OUT|bolPerfilAutorizado]", bolPerfilAutorizado), null, null);
            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[PROY-140419 Autorizar Portabilidad sin PIN][ConsultarPerfiles][verificaUsuario][OUT|strMensaje]", strMensaje), null, null);
            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[PROY-140419 Autorizar Portabilidad sin PIN][ConsultarPerfiles][FIN]", ""), null, null);
            return bolPerfilAutorizado;
        }

        public void CargarCausalOmision()
        {
            GeneradorLog objLog = new GeneradorLog(null, "Proceso_Omision_PIN", null, "log_Proceso_Omision_PIN");
            try
            {
                HttpContext.Current.Session["SMSPNCodigoPorta"] = string.Empty; //PROY-140585-FASE2
                HttpContext.Current.Session["flagSMSPortabilidadXPDV"] = "0"; //PROY-140585-FASE2

                string strTitulo = ReadKeySettings.Key_TituloCausalOmision;

                objLog.CrearArchivolog(string.Format("{0} --> {1}", "[PROY-140542 - Mejora en Proceso de Omision de PIN][CargarCausalOmision][INICIO]", string.Empty), null, null);
                objLog.CrearArchivolog(string.Format("{0} --> {1}", "[PROY-140542 - Mejora en Proceso de Omision de PIN][CargarCausalOmision][strTitulo]", strTitulo), null, null);

                List<BEItemGenerico> lstParametro = new List<BEItemGenerico>();
                hidCausal.Value = string.Empty;

                BEItemGenerico objParametro = new BEItemGenerico();
                objParametro.Codigo = "1";
                objParametro.Descripcion = strTitulo;
                lstParametro.Add(objParametro);

                dgdLista.DataSource = lstParametro;
                dgdLista.DataBind();
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog(string.Format("{0} --> {1}", "[PROY-140542 - Mejora en Proceso de Omision de PIN][CargarCausalOmision] -> Exception: ", Funciones.CheckStr(ex.Message)), null, null);
                objLog.CrearArchivolog(string.Format("{0} --> {1}", "[PROY-140542 - Mejora en Proceso de Omision de PIN][CargarCausalOmision] -> Exception: ", Funciones.CheckStr(ex.StackTrace)), null, null);
            }
            finally 
            {
                objLog.CrearArchivolog(string.Format("{0} --> {1}", "[PROY-140542 - Mejora en Proceso de Omision de PIN][CargarCausalOmision][FIN]", string.Empty), null, null);
            }
            
        }

        protected void dgdLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GeneradorLog objLog = new GeneradorLog(null, "Proceso_Omision_PIN", null, "log_Proceso_Omision_PIN");

            try
            {
                objLog.CrearArchivolog(string.Format("{0} --> {1}", "[PROY-140542 - Mejora en Proceso de Omision de PIN][dgdLista_RowDataBound][INICIO]", string.Empty), null, null);

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[0].Text = tableDetalle();
                }
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog(string.Format("{0} --> {1}", "[PROY-140542 - Mejora en Proceso de Omision de PIN][dgdLista_RowDataBound] -> Exception: ", Funciones.CheckStr(ex.Message)), null, null);
                objLog.CrearArchivolog(string.Format("{0} --> {1}", "[PROY-140542 - Mejora en Proceso de Omision de PIN][dgdLista_RowDataBound] -> Exception: ", Funciones.CheckStr(ex.StackTrace)), null, null);
            }
            finally
            {
                objLog.CrearArchivolog(string.Format("{0} --> {1}", "[PROY-140542 - Mejora en Proceso de Omision de PIN][dgdLista_RowDataBound][FIN]", string.Empty), null, null);
            }
            
        }

        public string tableDetalle()
        {
            GeneradorLog objLog = new GeneradorLog(null, "Proceso_Omision_PIN", null, "log_Proceso_Omision_PIN");
            StringBuilder strTableHTML = new StringBuilder();
            string strTableFormat = string.Empty;

            try
            {
                objLog.CrearArchivolog(string.Format("{0} --> {1}", "[PROY-140542 - Mejora en Proceso de Omision de PIN][tableDetalle][INICIO]", string.Empty), null, null);

                StringBuilder strTableContentID = new StringBuilder();

                List<BEItemGenerico> lstParametro = new List<BEItemGenerico>();
                string[] lstCausales = null;
                string strCausales = string.Empty;

                strTableHTML.Append("<tr style='display:block' id='" + "1" + "'>");
                strTableHTML.Append("<td id='hola' colspan='100%' style='border-collapse:collapse;Z-INDEX: 0;border:0'>");
                strTableHTML.Append("<table align='center' width='100%' cellspacing='0' cellpadding='0' rules='all' border='0' style='border-collapse:collapse;Z-INDEX: 0'>");

                strCausales = ReadKeySettings.Key_CausalesOmisionPIN;

                lstCausales = strCausales.Split('|');

                foreach (string item in lstCausales)
                {
                    BEItemGenerico objParametro = new BEItemGenerico();
                    objParametro.Codigo = item.Split(';')[0];
                    objParametro.Descripcion = item.Split(';')[1];

                    objLog.CrearArchivolog(string.Format("{0} --> {1}", "[PROY-140542 - Mejora en Proceso de Omision de PIN][tableDetalle][objParametro.Codigo]", objParametro.Codigo), null, null);
                    objLog.CrearArchivolog(string.Format("{0} --> {1}", "[PROY-140542 - Mejora en Proceso de Omision de PIN][tableDetalle][objParametro.Descripcion]", objParametro.Descripcion), null, null);

                    strTableContentID.Append("<tr class='Arial10B' align='Left'>");
                    strTableContentID.Append("<td id='hola2' width='10px'><input type='radio' name='group_" + "1" + "' value='" + objParametro.Codigo + "_" + objParametro.Descripcion + "' onclick='javascript:f_Respuesta(this.value)'><br/><br/></td>");
                    strTableContentID.Append("<td id='hola3'>" + objParametro.Descripcion + "<br/><br/></td>");
                    strTableContentID.Append("</tr>");
                }

                strTableHTML.Append(strTableContentID);
                strTableHTML.Append("</table>");
                strTableHTML.Append("</td>");
                strTableHTML.Append("</tr>");

                strTableFormat = strTableHTML.ToString();

            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog(string.Format("{0} --> {1}", "[PROY-140542 - Mejora en Proceso de Omision de PIN][tableDetalle] -> Exception: ", Funciones.CheckStr(ex.Message)), null, null);
                objLog.CrearArchivolog(string.Format("{0} --> {1}", "[PROY-140542 - Mejora en Proceso de Omision de PIN][tableDetalle] -> Exception: ", Funciones.CheckStr(ex.StackTrace)), null, null);
            }
            finally
            {
                objLog.CrearArchivolog(string.Format("{0} --> {1}", "[PROY-140542 - Mejora en Proceso de Omision de PIN][tableDetalle][FIN]", string.Empty), null, null);
            }

            return strTableFormat;
        }
    }
}