using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity.AfiliacionDEAUDebito.ConsultarAfiliacion;
using Claro.SISACT.Common;
using System.Collections;
using Claro.SISACT.Entity;
using System.Configuration;

namespace Claro.SISACT.WS.RestReferences
{
    public class RestAfiliacionDEAUDebito
    {
        public ResponseConsultarAfiliacionDEAUDebito ConsultarAfiliacionesDEAUDebito(BodyRequestConsultarAfiliacionDEAUDebito objRequestConsultarAfil, BEAuditoriaRequest objBEAuditoriaRequest)
        {
            GeneradorLog _objLog = null;
            //objBEAuditoriaRequest.usuarioAplicacion = "HARDCODEADO";
            _objLog = new GeneradorLog(objBEAuditoriaRequest.usuarioAplicacion, "[RestAfiliacionDEAUDebito][ConsultarAfiliacionesDEAUDebito]", objBEAuditoriaRequest.idTransaccion, null);
            try
            {
                StringBuilder sbParametros = new StringBuilder();
                Hashtable paramHeader = new Hashtable();
                paramHeader.Add("timestamp", objBEAuditoriaRequest.timestamp);
                paramHeader.Add("userId", objBEAuditoriaRequest.userId);
                paramHeader.Add("idTransaccion", objBEAuditoriaRequest.idTransaccion);

                BEItemGetInvoque objGetInvoque = new BEItemGetInvoque();
                objGetInvoque.name_Url = "url_ConsultarAfiliacion";
                objGetInvoque.strWSIP = Funciones.CheckStr(ConfigurationManager.AppSettings["AfiliacionDEAUAsistidosWS_wsip"]);
                objGetInvoque.UserEncrypted = Funciones.CheckStr(ConfigurationManager.AppSettings["AfiliacionDEAUAsistidosWS_User"]);
                objGetInvoque.PassEncrypted = Funciones.CheckStr(ConfigurationManager.AppSettings["AfiliacionDEAUAsistidosWS_Pass"]);
                objGetInvoque.TimeoutUrl = Funciones.CheckStr(ConfigurationManager.AppSettings["AfiliacionDEAUAsistidosWS_TimeOut"]);
                objGetInvoque.idProy = "[PROY-142525]";
                objGetInvoque.ipServidor = objBEAuditoriaRequest.ipApplication;
                objGetInvoque.usuario = objBEAuditoriaRequest.userId;
                sbParametros.Append("?");
                sbParametros.AppendFormat("tipo={0}&", objRequestConsultarAfil.tipo);
                sbParametros.AppendFormat("tipoConsulta={0}&", objRequestConsultarAfil.tipoConsulta);
                objGetInvoque.parametroUrl = Funciones.CheckStr(sbParametros);


                _objLog.CrearArchivolog("---- Parametros de entrada GetInvoque : ----", null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliacionDEAUDebito][ConsultarAfiliacionesDEAUDebito]timestamp", objBEAuditoriaRequest.timestamp), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliacionDEAUDebito][ConsultarAfiliacionesDEAUDebito]userId", objBEAuditoriaRequest.userId), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliacionDEAUDebito][ConsultarAfiliacionesDEAUDebito]idTransaccion", objBEAuditoriaRequest.idTransaccion), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliacionDEAUDebito][ConsultarAfiliacionesDEAUDebito]accept", objBEAuditoriaRequest.accept), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliacionDEAUDebito][ConsultarAfiliacionesDEAUDebito]ipApplication", objBEAuditoriaRequest.ipApplication), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliacionDEAUDebito][ConsultarAfiliacionesDEAUDebito]objGetInvoque.name_Url", Funciones.CheckStr(objGetInvoque.name_Url)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliacionDEAUDebito][ConsultarAfiliacionesDEAUDebito]objGetInvoque.strWSIP", Funciones.CheckStr(objGetInvoque.strWSIP)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliacionDEAUDebito][ConsultarAfiliacionesDEAUDebito]objGetInvoque.UserEncrypted", Funciones.CheckStr(objGetInvoque.UserEncrypted)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliacionDEAUDebito][ConsultarAfiliacionesDEAUDebito]objGetInvoque.PassEncrypted", Funciones.CheckStr(objGetInvoque.PassEncrypted)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliacionDEAUDebito][ConsultarAfiliacionesDEAUDebito]objGetInvoque.TimeoutUrl", Funciones.CheckStr(objGetInvoque.TimeoutUrl)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliacionDEAUDebito][ConsultarAfiliacionesDEAUDebito]objGetInvoque.idProy", Funciones.CheckStr(objGetInvoque.idProy)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliacionDEAUDebito][ConsultarAfiliacionesDEAUDebito]objGetInvoque.ipServidor", Funciones.CheckStr(objGetInvoque.ipServidor)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliacionDEAUDebito][ConsultarAfiliacionesDEAUDebito]objGetInvoque.usuario", Funciones.CheckStr(objGetInvoque.usuario)), null, null);

                return GetInvoqueDP.GetInvoque<ResponseConsultarAfiliacionDEAUDebito>(objGetInvoque, paramHeader);

            }
            catch (Exception ex)
            {
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[RestAfiliacionDEAUDebito][ConsultarAfiliacionesDEAUDebito]Excepcion", objBEAuditoriaRequest.applicationCodeWS), null, ex);
                throw;
            }

        }

    }
}
