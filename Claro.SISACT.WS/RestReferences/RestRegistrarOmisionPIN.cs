using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity.RegistrarOmisionPINRest;
using Claro.SISACT.Entity;
using Claro.SISACT.Common;
using System.Collections;
using System.Configuration;
using Claro.SISACT.WS.RestServices;

namespace Claro.SISACT.WS.RestReferences
{
    public class RestRegistrarOmisionPIN
    {
        public ResponseRegistrarOmisionPIN registrarOmisionPIN(RequestRegistrarOmisionPIN objRequestRegistrarOmisionPIN, BEAuditoriaRequest objBEAuditoriaRequest)
        {
            GeneradorLog _objLog = null;
            _objLog = new GeneradorLog(objBEAuditoriaRequest.usuarioAplicacion, "Metodo registrarOmisionPIN", objBEAuditoriaRequest.idTransaccion, null);
            try
            {
                Hashtable paramHeader = new Hashtable();
                paramHeader.Add("timestamp", objBEAuditoriaRequest.timestamp);
                paramHeader.Add("userId", objBEAuditoriaRequest.userId);
                paramHeader.Add("idTransaccion", objBEAuditoriaRequest.idTransaccion);

                string strWSIP = Funciones.CheckStr(ConfigurationManager.AppSettings["OmisionPINService_wsip"]);
                string strUserEncrypted = Funciones.CheckStr(ConfigurationManager.AppSettings["OmisionPINService_User"]);
                string strPassEncrypted = Funciones.CheckStr(ConfigurationManager.AppSettings["OmisionPINService_Pass"]);
                string strTimeout = Funciones.CheckStr(ConfigurationManager.AppSettings["OmisionPINService_TimeOut"]);
                string strnombreProy = "RegistraOmisionPin";

                _objLog.CrearArchivolog("---- Parametros de entrada Inicio : ----", null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}","strnombreProy",strnombreProy), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}","timestamp",objBEAuditoriaRequest.timestamp), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}","userId",objBEAuditoriaRequest.userId), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}","idTransaccion",objBEAuditoriaRequest.idTransaccion), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}","accept",objBEAuditoriaRequest.accept), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}","ipApplication",objBEAuditoriaRequest.ipApplication), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}","strWSIP",strWSIP), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}","strUserEncrypted",strUserEncrypted), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}","strPassEncrypted",strPassEncrypted), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "strTimeout", strTimeout), null, null);
                
                return PostInvoqueDP.PostInvoque<ResponseRegistrarOmisionPIN>("urlRegistrarOmisionPIN", paramHeader, objRequestRegistrarOmisionPIN, objBEAuditoriaRequest.userId, objBEAuditoriaRequest.ipApplication, strnombreProy, strWSIP, strUserEncrypted, strPassEncrypted, strTimeout);
            }
            catch (Exception ex)
            {
                _objLog.CrearArchivolog(String.Format("{0} : {1}","Excepcion metodo registrarOmisionPIN",objBEAuditoriaRequest.applicationCodeWS), null, ex);
                throw;
            }

        }
    }
}
