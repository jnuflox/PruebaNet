//PROY-32439 MAS INI CLASE NUEVA
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using Claro.SISACT.Entity;
using Claro.SISACT.Common;
using Claro.SISACT.WS.WSConsultaDatosOAC;
using System.Web.Services.Protocols;
namespace Claro.SISACT.WS
{
    public class BWConsultaDatosOACWS
    {
        ConsultaDatosOACWS _objTransaccion = new ConsultaDatosOACWS();

        public BWConsultaDatosOACWS()
        {
            _objTransaccion.Url = ConfigurationManager.AppSettings["RutaWS_ConsultaDatosOACWS"].ToString();
            _objTransaccion.Credentials = System.Net.CredentialCache.DefaultCredentials;
            _objTransaccion.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["TimeOut_OAC"].ToString());
        }

        public consultarDeudaCuentaResponseType ConsultaDatosOAC(String strTipoDoc, String strNroDoc, String strMesesDisputa , out String strRespuestaCodigo, out String strRespuestaMensaje)
        {
            var objResponse = new consultarDeudaCuentaResponseType();
            strRespuestaCodigo = String.Empty;
            strRespuestaMensaje = String.Empty;
            GeneradorLog logError = null;
            try
            {
                #region ConsultarDeudaCuenta|RequestHeader
                HeaderRequestType objRequestHeader = new HeaderRequestType();
                objRequestHeader.canal = "";
                objRequestHeader.idAplicacion = Funciones.CheckStr(ConfigurationManager.AppSettings["CodigoAplicacion"]);
                objRequestHeader.usuarioAplicacion = Funciones.CheckStr(ConfigurationManager.AppSettings["constNombreAplicacion"]);
                objRequestHeader.usuarioSesion = "";
                objRequestHeader.idTransaccionESB = DateTime.Now.ToString("hhmmssfff");
                objRequestHeader.idTransaccionNegocio = "";
                objRequestHeader.fechaInicio = DateTime.Now;
                objRequestHeader.nodoAdicional = "";
                _objTransaccion.headerRequest = objRequestHeader;
                #endregion

                #region ConsultarDeudaCuenta|RequestBody
                consultarDeudaCuentaRequestType objRequest = new consultarDeudaCuentaRequestType();
                objRequest.tipoDocumento = strTipoDoc; 
                objRequest.numDocumento = strNroDoc;
                objRequest.nroMesesDisputa = strMesesDisputa;
                objRequest.listaAdicionalRequest = null;
                #endregion

                #region ConsultarDeudaCuenta|Response
                objResponse = _objTransaccion.consultarDeudaCuenta(objRequest);
                if (!Object.Equals(objResponse, null))
                {
                    strRespuestaCodigo = objResponse.responseStatus.codigoRespuesta;
                    strRespuestaMensaje = objResponse.responseStatus.descripcionRespuesta;
                }
                #endregion
            }
            catch (SoapException se)
            {
                logError = new GeneradorLog(null, "ErrorWS", null, "ErrorWS");
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaDatosOAC][WS | Error :: INI]", ""), null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaDatosOAC][WS | Error Actor]", se.Actor), null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaDatosOAC][WS | Error Code]", se.Code), null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaDatosOAC][WS | Error Data]", se.Data), null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaDatosOAC][WS | Error Detail]", se.Detail), null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaDatosOAC][WS | Error HelpLink]", se.HelpLink), null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaDatosOAC][WS | Error InnerException]", se.InnerException), null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaDatosOAC][WS | Error Lang]", se.Lang), null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaDatosOAC][WS | Error Message]", se.Message), null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaDatosOAC][WS | Error Node]", se.Node), null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaDatosOAC][WS | Error Role]", se.Role), null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaDatosOAC][WS | Error Source]", se.Source), null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaDatosOAC][WS | Error StackTrace]", se.StackTrace), null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaDatosOAC][WS | Error SubCode]", se.SubCode), null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaDatosOAC][WS | Error TargetSite]", se.TargetSite), null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaDatosOAC][WS | Error :: FIN]", ""), null);
            }
            catch (Exception e)
            {
                objResponse = null;
                strRespuestaCodigo = "-1";
                strRespuestaMensaje = "ErrorWS_consultarDeudaCuenta[" + e.Message + "]";
                logError = new GeneradorLog(null, "ErrorWS", null, "ErrorWS");
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaDatosOAC][APP | Error :: INI]", ""), null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaDatosOAC][APP | Error Data]", e.Data), null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaDatosOAC][APP | Error HelpLink]", e.HelpLink), null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaDatosOAC][APP | Error InnerException]", e.InnerException), null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaDatosOAC][APP | Error Message]", e.Message), null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaDatosOAC][APP | Error Source]", e.Source), null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaDatosOAC][APP | Error StackTrace]", e.StackTrace), null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaDatosOAC][APP | Error StackTrace]", e.TargetSite), null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaDatosOAC][APP | Error :: FIN]", ""), null);

            }
            finally
            {
                _objTransaccion.Dispose();
            }

            return objResponse;
        }
    }
}
//PROY-32439 MAS FIN CLASE NUEVA