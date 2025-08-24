using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Claro.SISACT.Common;
using Claro.SISACT.Entity.claroventacobertura.validarcobertura;
using Claro.SISACT.Entity;
using System.Collections;

namespace Claro.SISACT.WS.RestReferences
{
    public class RestValidarCobertura
    {
        public ResponseValidarCobertura validarCobertura(RequestValidarCobertura objRequestValidarCobertura, BEAuditoriaRequest objBEAuditoriaRequest)
        {
            GeneradorLog _objLog = null;
            _objLog = new GeneradorLog(objBEAuditoriaRequest.usuarioAplicacion, "Metodo validarCobertura", objBEAuditoriaRequest.idTransaccion, null);
            try
            {
                Hashtable paramHeader = new Hashtable();
                paramHeader.Add("idAplicacion", objBEAuditoriaRequest.idAplicacion);
                paramHeader.Add("canal", objBEAuditoriaRequest.canal);
                paramHeader.Add("usuarioAplicacion", objBEAuditoriaRequest.usuarioAplicacion);
                paramHeader.Add("usuarioSesion", objBEAuditoriaRequest.usuarioSesion);
                paramHeader.Add("idTransaccionESB", objBEAuditoriaRequest.idTransaccionESB);
                paramHeader.Add("idTransaccionNegocio", objBEAuditoriaRequest.idTransaccionNegocio);
                paramHeader.Add("fechaInicio", objBEAuditoriaRequest.fechaInicio);
                paramHeader.Add("nodoAdicional", objBEAuditoriaRequest.nodoAdicional);

                string strWSIP = Funciones.CheckStr(ConfigurationManager.AppSettings["Service_Wsip_Generico"]);
                string strUserEncrypted = Funciones.CheckStr(ConfigurationManager.AppSettings["Service_User_Generico"]);
                string strPassEncrypted = Funciones.CheckStr(ConfigurationManager.AppSettings["Service_Pass_Generico"]);
                string strTimeout = Funciones.CheckStr(ConfigurationManager.AppSettings["Service_TimeOut_Generico"]);
                string strnombreProy = "validarCobertura";

                _objLog.CrearArchivolog("---- Parametros de entrada Inicio : ----", null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "strnombreProy", strnombreProy), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "timestamp", objBEAuditoriaRequest.timestamp), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "userId", objBEAuditoriaRequest.userId), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "idTransaccion", objBEAuditoriaRequest.idTransaccion), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "accept", objBEAuditoriaRequest.accept), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "ipApplication", objBEAuditoriaRequest.ipApplication), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "strWSIP", strWSIP), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "strUserEncrypted", strUserEncrypted), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "strPassEncrypted", strPassEncrypted), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "strTimeout", strTimeout), null, null);

                return PostInvoqueDP.PostInvoque<ResponseValidarCobertura>("urlValidarCobertura", paramHeader, objRequestValidarCobertura, objBEAuditoriaRequest.userId, objBEAuditoriaRequest.ipApplication, strnombreProy, strWSIP, strUserEncrypted, strPassEncrypted, strTimeout);
            }
            catch (Exception ex)
            {
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "Excepcion metodo validarCobertura", objBEAuditoriaRequest.applicationCodeWS), null, ex);
                throw;
            }

        }
    }
}
