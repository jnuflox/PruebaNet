using Claro.SISACT.Common;
using Claro.SISACT.Entity;
using Claro.SISACT.Entity.MotorPromociones.GetBonos;
using Claro.SISACT.Entity.MotorPromociones.SimulacionMultilineas;
using Claro.SISACT.Entity.MotorPromociones.GetMultilineaInfo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Claro.SISACT.WS.RestReferences
{
    public class RestMotorPromociones
    {
        public GetBonosResponse getBonos(GetBonosRequest objGetBonosRequest, BEAuditoriaRequest objBEAuditoriaRequest)
        {
            GeneradorLog _objLog = null;
            _objLog = new GeneradorLog(objBEAuditoriaRequest.usuarioAplicacion, "Metodo getBonos", objBEAuditoriaRequest.idTransaccion, null);
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
                string strnombreProy = "getBonos";

                _objLog.CrearArchivolog("---- Parametros de entrada Inicio : ----", null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "strnombreProy", strnombreProy), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "strWSIP", strWSIP), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "strUserEncrypted", strUserEncrypted), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "strPassEncrypted", strPassEncrypted), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "strTimeout", strTimeout), null, null);

                return PostInvoqueDP.PostInvoque<GetBonosResponse>("urlGetBonos", paramHeader, objGetBonosRequest, objBEAuditoriaRequest.userId, objBEAuditoriaRequest.ipApplication, strnombreProy, strWSIP, strUserEncrypted, strPassEncrypted, strTimeout);
            }
            catch (Exception ex)
            {
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "Excepcion metodo getBonos", objBEAuditoriaRequest.applicationCodeWS), null, ex);
                throw;
            }

        }

        public SimulacionMultilineasResponse simulacionMultilineas(SimulacionMultilineasRequest objSimulacionMultilineasRequest, BEAuditoriaRequest objBEAuditoriaRequest)
        {
            GeneradorLog _objLog = null;
            _objLog = new GeneradorLog(objBEAuditoriaRequest.usuarioAplicacion, "Metodo simulacionMultilineas", objBEAuditoriaRequest.idTransaccion, null);
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
                string strnombreProy = "simulacionMultilineas";

                _objLog.CrearArchivolog("---- Parametros de entrada Inicio : ----", null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "strnombreProy", strnombreProy), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "strWSIP", strWSIP), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "strUserEncrypted", strUserEncrypted), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "strPassEncrypted", strPassEncrypted), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "strTimeout", strTimeout), null, null);

                return PostInvoqueDP.PostInvoque<SimulacionMultilineasResponse>("urlSimulacionMultilineas", paramHeader, objSimulacionMultilineasRequest, objBEAuditoriaRequest.userId, objBEAuditoriaRequest.ipApplication, strnombreProy, strWSIP, strUserEncrypted, strPassEncrypted, strTimeout);
            }
            catch (Exception ex)
            {
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "Excepcion metodo simulacionMultilineas", objBEAuditoriaRequest.applicationCodeWS), null, ex);
                throw;
            }

        }
        public GetMultilineaInfoResponse getMultilineaInfo(GetMultilineaInfoRequest objGetMultilineaInfoRequest, BEAuditoriaRequest objBEAuditoriaRequest)
        {
            GeneradorLog _objLog = null;
            _objLog = new GeneradorLog(objBEAuditoriaRequest.usuarioAplicacion, "Metodo getMultilineaInfo", objBEAuditoriaRequest.idTransaccion, null);
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
                string strnombreProy = "getMultilineaInfo";

                _objLog.CrearArchivolog("---- Parametros de entrada Inicio : ----", null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "strnombreProy", strnombreProy), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "strWSIP", strWSIP), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "strUserEncrypted", strUserEncrypted), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "strPassEncrypted", strPassEncrypted), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "strTimeout", strTimeout), null, null);

                return PostInvoqueDP.PostInvoque<GetMultilineaInfoResponse>("urlGetMultilineaInfo", paramHeader, objGetMultilineaInfoRequest, objBEAuditoriaRequest.userId, objBEAuditoriaRequest.ipApplication, strnombreProy, strWSIP, strUserEncrypted, strPassEncrypted, strTimeout);
            }
            catch (Exception ex)
            {
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "Excepcion metodo getMultilineaInfo", objBEAuditoriaRequest.applicationCodeWS), null, ex);
                throw;
            }

        }
    }
}
