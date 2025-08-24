using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity.DataPowerRest.Generic;
using Claro.SISACT.Common;
using System.Configuration;
using System.Collections;
using Claro.SISACT.Entity;

namespace Claro.SISACT.WS.RestReferences
{
    public class RestServiceGeneric
    {
        private string logFileName = string.Empty;

        public HeaderRequest GetHeader()
        {
            HeaderRequest objHeaderRequest = new HeaderRequest();
            objHeaderRequest.consumer = Funciones.CheckStr(ConfigurationManager.AppSettings["consumer"]);
            objHeaderRequest.country = Funciones.CheckStr(ConfigurationManager.AppSettings["country"]);
            objHeaderRequest.language = Funciones.CheckStr(ConfigurationManager.AppSettings["languaje"]);
            objHeaderRequest.modulo = Funciones.CheckStr(ConfigurationManager.AppSettings["modulo"]);
            objHeaderRequest.msgType = Funciones.CheckStr(ConfigurationManager.AppSettings["msgType"]);
            objHeaderRequest.pid = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            objHeaderRequest.system = Funciones.CheckStr(ConfigurationManager.AppSettings["constAplicacion"]);
            objHeaderRequest.timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            objHeaderRequest.userId = Funciones.CheckStr(ConfigurationManager.AppSettings["strCodAPlicativoClaveWeb"]);
            return objHeaderRequest;
        }
//INICIATIVA 803 INI
        public HeaderRequest GetHeader_v2()
        {
            HeaderRequest objHeaderRequest = new HeaderRequest();
            objHeaderRequest.consumer = Funciones.CheckStr(ConfigurationManager.AppSettings["OmisionPINService_consumer"]);
            objHeaderRequest.country = Funciones.CheckStr(ConfigurationManager.AppSettings["OmisionPINService_country"]);
            objHeaderRequest.language = Funciones.CheckStr(ConfigurationManager.AppSettings["OmisionPINService_language"]);
            objHeaderRequest.modulo = string.Empty;
            objHeaderRequest.msgType = Funciones.CheckStr(ConfigurationManager.AppSettings["OmisionPINService_msgtype"]);
            objHeaderRequest.dispositivo = Funciones.CheckStr(ConfigurationManager.AppSettings["DP_consDispositivo_Generico"]);
            objHeaderRequest.pid = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            objHeaderRequest.operation = string.Empty;
            objHeaderRequest.system = Funciones.CheckStr(ConfigurationManager.AppSettings["OmisionPINService_system"]);
            objHeaderRequest.timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
            objHeaderRequest.userId = Funciones.CheckStr(ConfigurationManager.AppSettings["system_ConsultaClave"]);
            return objHeaderRequest;
        }
//INICIATIVA 803 FIN
        public Hashtable GetParamHeader(HeaderRequest objHeaderRequest)
        {
            Hashtable paramHeader = new Hashtable();
            paramHeader.Add("idTransaccion", DateTime.Now.ToString("yyyyMMddHHmmssfff"));
            paramHeader.Add("msgId", DateTime.Now.ToString("yyyyMMddHHmmssfff"));
            paramHeader.Add("consumer", Funciones.CheckStr(objHeaderRequest.consumer));
            paramHeader.Add("country", Funciones.CheckStr(objHeaderRequest.country));
            paramHeader.Add("dispositivo", Funciones.CheckStr(objHeaderRequest.dispositivo));
            paramHeader.Add("language", Funciones.CheckStr(objHeaderRequest.language));
            paramHeader.Add("modulo", Funciones.CheckStr(objHeaderRequest.modulo));
            paramHeader.Add("msgType", Funciones.CheckStr(objHeaderRequest.msgType));
            paramHeader.Add("operation", Funciones.CheckStr(objHeaderRequest.operation));
            paramHeader.Add("pid", Funciones.CheckStr(objHeaderRequest.pid));
            paramHeader.Add("system", Funciones.CheckStr(objHeaderRequest.system));
            paramHeader.Add("timestamp", Funciones.CheckStr(objHeaderRequest.timestamp));
            paramHeader.Add("userId", Funciones.CheckStr(objHeaderRequest.userId));
            paramHeader.Add("wsIp", Funciones.CheckStr(objHeaderRequest.wsIp));
            return paramHeader;
        }

        public BEAuditoriaRequest GetAuditoria(BEAuditoriaRequest objAuditoria)
        {
            BEAuditoriaRequest objAudit = new BEAuditoriaRequest();
            Hashtable paramHeader = new Hashtable();
            //headers
            paramHeader.Add("timestamp", objAuditoria.timestamp);
            paramHeader.Add("userId", objAuditoria.userId);
            paramHeader.Add("idTransaccion", objAuditoria.idTransaccion);
            paramHeader.Add("msgid", objAuditoria.msgid);
            paramHeader.Add("aplicacion", Funciones.CheckStr(ConfigurationManager.AppSettings["constUsuarioAplicacion"]));

            //credenciales
            objAudit.usuarioAplicacionEncriptado = Funciones.CheckStr(ConfigurationManager.AppSettings["User_ConsultaNacionalidad"]); ;
            objAudit.claveEncriptada = Funciones.CheckStr(ConfigurationManager.AppSettings["Password_ConsultaNacionalidad"]); ;

            //header consulta claves
            objAudit.wsIp = objAuditoria.wsIp;
            objAudit.ipApplication = objAuditoria.ipTransaccion;
            objAudit.usuarioAplicacion = objAuditoria.usuarioAplicacion;
            objAudit.applicationCode = Funciones.CheckStr(ConfigurationManager.AppSettings["constUsuarioAplicacion"]);
            objAudit.idAplicacion = Funciones.CheckStr(ConfigurationManager.AppSettings["CodigoAplicacion"]);

            //flag DP
            objAudit.dataPower = objAuditoria.dataPower;

            //timeout
            objAudit.urlTimeOut_Rest = objAuditoria.urlTimeOut_Rest;
            objAudit.table = paramHeader;
            return objAudit;
        }
        public string GetLogFileName()
        {
            return this.logFileName;
        }

        public void SetLogFileName(string logFileName)
        {
            this.logFileName = logFileName;
        }
    }
}
