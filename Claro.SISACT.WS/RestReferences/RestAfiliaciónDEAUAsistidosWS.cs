using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity;
using Claro.SISACT.Common;
using System.Collections;
using System.Configuration;
using Claro.SISACT.WS.RestServices;
using Claro.SISACT.Entity.AfiliaciónDEAUAsistidosWSRest.EnviaLink;
using Claro.SISACT.Entity.AfiliaciónDEAUAsistidosWSRest.ConsultarAfiliacion;
using Claro.SISACT.Entity.AfiliaciónDEAUAsistidosWSRest.RegistraEnvio;
//PROY-140657
namespace Claro.SISACT.WS.RestReferences
{
    public class RestAfiliaciónDEAUAsistidosWS
    {
        public  ResponseEnviaLinkDEAU EnviaLink( RequestEnviaLinkDEAU objRequestEnviaLink, BEAuditoriaRequest objBEAuditoriaRequest)
        {
            GeneradorLog _objLog = null;
            _objLog = new GeneradorLog(objBEAuditoriaRequest.usuarioAplicacion, "[RestAfiliaciónDEAUAsistidosWS][EnviaLink]", objBEAuditoriaRequest.idTransaccion, null);
            try
            {
                Hashtable paramHeader = new Hashtable();
                paramHeader.Add("timestamp", objBEAuditoriaRequest.timestamp);
                paramHeader.Add("userId", objBEAuditoriaRequest.userId);
                paramHeader.Add("idTransaccion", objBEAuditoriaRequest.idTransaccion);

                string strWSIP = Funciones.CheckStr(ConfigurationManager.AppSettings["AfiliacionDEAUAsistidosWS_wsip"]);
                string strUserEncrypted = Funciones.CheckStr(ConfigurationManager.AppSettings["AfiliacionDEAUAsistidosWS_User"]);
                string strPassEncrypted = Funciones.CheckStr(ConfigurationManager.AppSettings["AfiliacionDEAUAsistidosWS_Pass"]);
                string strTimeout = Funciones.CheckStr(ConfigurationManager.AppSettings["AfiliacionDEAUAsistidosWS_TimeOut"]);
                string strnombreProy = "[PROY-140657]";

                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliaciónDEAUAsistidosWS][EnviaLink]strnombreProy", strnombreProy), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliaciónDEAUAsistidosWS][EnviaLink]timestamp", objBEAuditoriaRequest.timestamp), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliaciónDEAUAsistidosWS][EnviaLink]userId", objBEAuditoriaRequest.userId), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliaciónDEAUAsistidosWS][EnviaLink]idTransaccion", objBEAuditoriaRequest.idTransaccion), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliaciónDEAUAsistidosWS][EnviaLink]accept", objBEAuditoriaRequest.accept), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliaciónDEAUAsistidosWS][EnviaLink]ipApplication", objBEAuditoriaRequest.ipApplication), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliaciónDEAUAsistidosWS][EnviaLink]strWSIP", strWSIP), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliaciónDEAUAsistidosWS][EnviaLink]strUserEncrypted", strUserEncrypted), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliaciónDEAUAsistidosWS][EnviaLink]strPassEncrypted", strPassEncrypted), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliaciónDEAUAsistidosWS][EnviaLink]strTimeout", strTimeout), null, null);

                return PostInvoqueDP.PostInvoque<ResponseEnviaLinkDEAU>("url_EnviarLink", paramHeader, objRequestEnviaLink, objBEAuditoriaRequest.userId, objBEAuditoriaRequest.ipApplication, strnombreProy, strWSIP, strUserEncrypted, strPassEncrypted, strTimeout);
            }
            catch (Exception ex)
            {
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliaciónDEAUAsistidosWS][EnviaLink]Excepcion", objBEAuditoriaRequest.applicationCodeWS), null, ex);
                throw;
            }

        }

        public ResponseConsultarAfiliacionDEAU ConsultarAfiliacion(RequestConsultarAfiliacionDEAU objRequestConsultarAfil, BEAuditoriaRequest objBEAuditoriaRequest)
        {
            GeneradorLog _objLog = null;
            _objLog = new GeneradorLog(objBEAuditoriaRequest.usuarioAplicacion, "[RestAfiliaciónDEAUAsistidosWS][ConsultarAfiliacion]", objBEAuditoriaRequest.idTransaccion, null);
            try
            {
                StringBuilder sbParametros = new StringBuilder();
                Hashtable paramHeader = new Hashtable();
                paramHeader.Add("timestamp", objBEAuditoriaRequest.timestamp);
                paramHeader.Add("userId", objBEAuditoriaRequest.userId);
                paramHeader.Add("idTransaccion", objBEAuditoriaRequest.idTransaccion);

                BEItemGetInvoque objGetInvoque= new BEItemGetInvoque();
                objGetInvoque.name_Url = "url_ConsultarAfiliacionPre";
                objGetInvoque.strWSIP= Funciones.CheckStr(ConfigurationManager.AppSettings["AfiliacionDEAUAsistidosWS_wsip"]);
                objGetInvoque.UserEncrypted = Funciones.CheckStr(ConfigurationManager.AppSettings["AfiliacionDEAUAsistidosWS_User"]);
                objGetInvoque.PassEncrypted = Funciones.CheckStr(ConfigurationManager.AppSettings["AfiliacionDEAUAsistidosWS_Pass"]);
                objGetInvoque.TimeoutUrl = Funciones.CheckStr(ConfigurationManager.AppSettings["AfiliacionDEAUAsistidosWS_TimeOut"]);
                objGetInvoque.idProy = "[PROY-140657]";
                objGetInvoque.ipServidor=objBEAuditoriaRequest.ipApplication;
                objGetInvoque.usuario=objBEAuditoriaRequest.userId;
                sbParametros.Append("?");
                sbParametros.AppendFormat("tipoConsulta={0}&", objRequestConsultarAfil.MessageRequest.body.tipoConsulta);
                sbParametros.AppendFormat("tipoDocumento={0}&", objRequestConsultarAfil.MessageRequest.body.tipoDocumento);
                sbParametros.AppendFormat("nroDocumento={0}&", objRequestConsultarAfil.MessageRequest.body.nroDocumento);
                objGetInvoque.parametroUrl = Funciones.CheckStr(sbParametros);
               /* objGetInvoque.parametroUrl = string.Format("?tipoDocumento={0}&numDocumento={1}", 
                                                            objRequestConsultarAfil.MessageRequest.body.tipoDocumento,
                                                            objRequestConsultarAfil.MessageRequest.body.nroDocumento);*/

                _objLog.CrearArchivolog("---- Parametros de entrada GetInvoque : ----", null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliaciónDEAUAsistidosWS][ConsultarAfiliacion]timestamp", objBEAuditoriaRequest.timestamp), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliaciónDEAUAsistidosWS][ConsultarAfiliacion]userId", objBEAuditoriaRequest.userId), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliaciónDEAUAsistidosWS][ConsultarAfiliacion]idTransaccion", objBEAuditoriaRequest.idTransaccion), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliaciónDEAUAsistidosWS][ConsultarAfiliacion]accept", objBEAuditoriaRequest.accept), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliaciónDEAUAsistidosWS][ConsultarAfiliacion]ipApplication", objBEAuditoriaRequest.ipApplication), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliaciónDEAUAsistidosWS][ConsultarAfiliacion]objGetInvoque.name_Url", Funciones.CheckStr(objGetInvoque.name_Url)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliaciónDEAUAsistidosWS][ConsultarAfiliacion]objGetInvoque.strWSIP", Funciones.CheckStr(objGetInvoque.strWSIP)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliaciónDEAUAsistidosWS][ConsultarAfiliacion]objGetInvoque.UserEncrypted", Funciones.CheckStr(objGetInvoque.UserEncrypted)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliaciónDEAUAsistidosWS][ConsultarAfiliacion]objGetInvoque.PassEncrypted", Funciones.CheckStr(objGetInvoque.PassEncrypted)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliaciónDEAUAsistidosWS][ConsultarAfiliacion]objGetInvoque.TimeoutUrl", Funciones.CheckStr(objGetInvoque.TimeoutUrl)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliaciónDEAUAsistidosWS][ConsultarAfiliacion]objGetInvoque.idProy", Funciones.CheckStr(objGetInvoque.idProy)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliaciónDEAUAsistidosWS][ConsultarAfiliacion]objGetInvoque.ipServidor", Funciones.CheckStr(objGetInvoque.ipServidor)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliaciónDEAUAsistidosWS][ConsultarAfiliacion]objGetInvoque.usuario", Funciones.CheckStr(objGetInvoque.usuario)), null, null);

                return GetInvoqueDP.GetInvoque<ResponseConsultarAfiliacionDEAU>(objGetInvoque, paramHeader);
                                                                                    
            }
            catch (Exception ex)
            {
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliaciónDEAUAsistidosWS][ConsultarAfiliacion]Excepcion", objBEAuditoriaRequest.applicationCodeWS), null, ex);
                throw;
            }

        }
        
        public ResponseRegistaEnvioDEAU RegistrarAfiliacion(RequestRegistaEnvioDEAU objRequestRegistraEnvio, BEAuditoriaRequest objBEAuditoriaRequest)
        {
            GeneradorLog _objLog = null;
            _objLog = new GeneradorLog(objBEAuditoriaRequest.usuarioAplicacion, "[RestAfiliaciónDEAUAsistidosWS][RegistraEnvio]", objBEAuditoriaRequest.idTransaccion, null);
            try
            {
                Hashtable paramHeader = new Hashtable();
                paramHeader.Add("timestamp", objBEAuditoriaRequest.timestamp);
                paramHeader.Add("userId", objBEAuditoriaRequest.userId);
                paramHeader.Add("idTransaccion", objBEAuditoriaRequest.idTransaccion);

                string strWSIP = Funciones.CheckStr(ConfigurationManager.AppSettings["AfiliacionDEAUAsistidosWS_wsip"]);
                string strUserEncrypted = Funciones.CheckStr(ConfigurationManager.AppSettings["AfiliacionDEAUAsistidosWS_User"]);
                string strPassEncrypted = Funciones.CheckStr(ConfigurationManager.AppSettings["AfiliacionDEAUAsistidosWS_Pass"]);
                string strTimeout = Funciones.CheckStr(ConfigurationManager.AppSettings["AfiliacionDEAUAsistidosWS_TimeOut"]);
                string strnombreProy = "Adecuaciones_SIOP";
                
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliaciónDEAUAsistidosWS][RegistrarAfiliacion]strnombreProy: ", strnombreProy), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliaciónDEAUAsistidosWS][RegistrarAfiliacion]timestamp: ", objBEAuditoriaRequest.timestamp), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliaciónDEAUAsistidosWS][RegistrarAfiliacion]userId: ", objBEAuditoriaRequest.userId), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliaciónDEAUAsistidosWS][RegistrarAfiliacion]idTransaccion: ", objBEAuditoriaRequest.idTransaccion), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliaciónDEAUAsistidosWS][RegistrarAfiliacion]accept: ", objBEAuditoriaRequest.accept), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliaciónDEAUAsistidosWS][RegistrarAfiliacion]ipApplication: ", objBEAuditoriaRequest.ipApplication), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliaciónDEAUAsistidosWS][RegistrarAfiliacion]strWSIP: ", strWSIP), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliaciónDEAUAsistidosWS][RegistrarAfiliacion]strUserEncrypted: ", strUserEncrypted), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliaciónDEAUAsistidosWS][RegistrarAfiliacion]strPassEncrypted: ", strPassEncrypted), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliaciónDEAUAsistidosWS][RegistrarAfiliacion]strTimeout: ", strTimeout), null, null);

                return PostInvoqueDP.PostInvoque<ResponseRegistaEnvioDEAU>("url_RegistrarAfiliacionPre", paramHeader, objRequestRegistraEnvio, objBEAuditoriaRequest.userId, objBEAuditoriaRequest.ipApplication, strnombreProy, strWSIP, strUserEncrypted, strPassEncrypted, strTimeout);
            }
            catch (Exception ex)
            {
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliaciónDEAUAsistidosWS][RegistrarAfiliacion]Excepcion: ", objBEAuditoriaRequest.applicationCodeWS), null, ex);
                throw;
            }
        }        
    }
}
