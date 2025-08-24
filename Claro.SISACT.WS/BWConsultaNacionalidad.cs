using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Common;
using Claro.SISACT.WS.ConsultaNacionalidadWS;
using Microsoft.Web.Services2;
using Microsoft.Web.Services2.Security;
using Microsoft.Web.Services2.Security.Tokens;
using System.Configuration;

namespace Claro.SISACT.WS
{
    public class BWConsultaNacionalidad
    {
        static string strArchivo = "BWConsultaNacionalidad";

        ConsultaNacionalidadWS.DAT_ConsultaNacionalidad_v1 objTransaction = null;
        UsernameToken usernameToken = null;

        public BWConsultaNacionalidad()
        {
            objTransaction = new DAT_ConsultaNacionalidad_v1();
            objTransaction.Url = ConfigurationManager.AppSettings["RutaWS_ConsultaNacionalidad"].ToString();
            objTransaction.Credentials = System.Net.CredentialCache.DefaultCredentials;
            objTransaction.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["TimeOut_ConsultaNacionalidad"].ToString());
        }

        public Result[] ConsultarNacionalidad(string strUsuario, string strPassword, string CurrentUser, string CurrentTerminal, string WsIp, ref string msgRespuesta, ref string codRespuesta)
        {

            ConsultaNacionalidadWS.consultarNacionalidadRequest consultarNacionalidadRequestWS = new ConsultaNacionalidadWS.consultarNacionalidadRequest();
            ConsultaNacionalidadWS.consultarNacionalidadResponse consultarNacionalidadResponseWS = new ConsultaNacionalidadWS.consultarNacionalidadResponse();
            string idLog = CurrentUser;

            GeneradorLog.EscribirLog(strArchivo, idLog, "---- Inicio método [ConsultarNacionalidad] ----");

            try
            {
                //wsse:Security  
                usernameToken = new UsernameToken(strUsuario, strPassword, PasswordOption.SendPlainText);
                objTransaction.RequestSoapContext.Security.Tokens.Add(usernameToken);

                //Auditoria OSB
                objTransaction.headerRequest = new HeaderRequest()
                {
                    channel = string.Empty,
                    idApplication = CurrentTerminal,
                    userApplication = "USRSISACT",
                    userSession = CurrentUser,
                    idESBTransaction = string.Empty,
                    idBusinessTransaction = string.Empty,
                    startDate = Convert.ToDateTime(string.Format("{0:u}", DateTime.UtcNow)),
                    additionalNode = string.Empty,
                };

                GeneradorLog.EscribirLog(strArchivo, idLog, String.Format("channel => {0}", objTransaction.headerRequest.channel));
                GeneradorLog.EscribirLog(strArchivo, idLog, String.Format("idApplication => {0}", objTransaction.headerRequest.idApplication));
                GeneradorLog.EscribirLog(strArchivo, idLog, String.Format("userApplication => {0}", objTransaction.headerRequest.userApplication));
                GeneradorLog.EscribirLog(strArchivo, idLog, String.Format("userSession => {0}", objTransaction.headerRequest.userSession));
                GeneradorLog.EscribirLog(strArchivo, idLog, String.Format("idESBTransaction => {0}", objTransaction.headerRequest.idESBTransaction));
                GeneradorLog.EscribirLog(strArchivo, idLog, String.Format("idBusinessTransaction => {0}", objTransaction.headerRequest.idBusinessTransaction));
                GeneradorLog.EscribirLog(strArchivo, idLog, String.Format("startDate => {0}", objTransaction.headerRequest.startDate));
                GeneradorLog.EscribirLog(strArchivo, idLog, String.Format("additionalNode => {0}", objTransaction.headerRequest.additionalNode));

                //DataPower
                objTransaction.HeaderRequest = new HeaderRequestType()
                {
                    country = ConfigurationManager.AppSettings["DAT_ConsultaNacionalidad_country"],
                    language = ConfigurationManager.AppSettings["DAT_ConsultaNacionalidad_language"],
                    consumer = ConfigurationManager.AppSettings["DAT_ConsultaNacionalidad_consumer"],
                    system = ConfigurationManager.AppSettings["DAT_ConsultaNacionalidad_system"],
                    modulo = ConfigurationManager.AppSettings["DAT_ConsultaNacionalidad_modulo"],
                    pid = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                    userId = CurrentUser,
                    dispositivo = CurrentTerminal,
                    wsIp = WsIp,
                    operation = ConfigurationManager.AppSettings["DAT_ConsultaNacionalidad_operation"],
                    timestamp = Convert.ToDateTime(string.Format("{0:u}", DateTime.UtcNow)),
                    msgType = ConfigurationManager.AppSettings["DAT_ConsultaNacionalidad_msgType"]
                };

                GeneradorLog.EscribirLog(strArchivo, idLog, String.Format("country => {0}", objTransaction.HeaderRequest.country));
                GeneradorLog.EscribirLog(strArchivo, idLog, String.Format("language => {0}", objTransaction.HeaderRequest.language));
                GeneradorLog.EscribirLog(strArchivo, idLog, String.Format("consumer => {0}", objTransaction.HeaderRequest.consumer));
                GeneradorLog.EscribirLog(strArchivo, idLog, String.Format("system => {0}", objTransaction.HeaderRequest.system));
                GeneradorLog.EscribirLog(strArchivo, idLog, String.Format("modulo => {0}", objTransaction.HeaderRequest.modulo));
                GeneradorLog.EscribirLog(strArchivo, idLog, String.Format("pid => {0}", objTransaction.HeaderRequest.pid));
                GeneradorLog.EscribirLog(strArchivo, idLog, String.Format("userId => {0}", objTransaction.HeaderRequest.userId));
                GeneradorLog.EscribirLog(strArchivo, idLog, String.Format("dispositivo => {0}", objTransaction.HeaderRequest.dispositivo));
                GeneradorLog.EscribirLog(strArchivo, idLog, String.Format("wsIp => {0}", objTransaction.HeaderRequest.wsIp));
                GeneradorLog.EscribirLog(strArchivo, idLog, String.Format("operation => {0}", objTransaction.HeaderRequest.operation));
                GeneradorLog.EscribirLog(strArchivo, idLog, String.Format("timestamp => {0}", objTransaction.HeaderRequest.timestamp));
                GeneradorLog.EscribirLog(strArchivo, idLog, String.Format("msgType => {0}", objTransaction.HeaderRequest.msgType));


                RequestOpcionalTypeRequestOpcional[] listaResquestOpcional = new RequestOpcionalTypeRequestOpcional[1];

                listaResquestOpcional[0] = new RequestOpcionalTypeRequestOpcional()
                {
                    campo = string.Empty,
                    valor = string.Empty

                };

                consultarNacionalidadRequestWS.listaResquestOpcional = listaResquestOpcional;

                consultarNacionalidadResponseWS = objTransaction.consultarNacionalidad(consultarNacionalidadRequestWS);

                codRespuesta = consultarNacionalidadResponseWS.responseStatus.codigoRespuesta;
                msgRespuesta = consultarNacionalidadResponseWS.responseStatus.descripcionRespuesta;

                GeneradorLog.EscribirLog(strArchivo, idLog, String.Format("codRespuesta => {0}", codRespuesta));
                GeneradorLog.EscribirLog(strArchivo, idLog, String.Format("msgRespuesta => {0}", msgRespuesta));

                GeneradorLog.EscribirLog(strArchivo, idLog, String.Format("Se obtuvieron {0} nacionalidades", Funciones.CheckStr(consultarNacionalidadResponseWS.responseData.result.Count())));

                return consultarNacionalidadResponseWS.responseData.result;
            }
            catch (Exception ex)
            {
                msgRespuesta = ex.Message;
                return null;
            }
            finally
            {
                objTransaction.Dispose();
                GeneradorLog.EscribirLog(strArchivo, idLog, "---- Fin método [ConsultarNacionalidad] ----");
            }

        }
    }
}
