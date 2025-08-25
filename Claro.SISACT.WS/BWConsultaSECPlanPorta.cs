using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Common;
using Claro.SISACT.WS.WSConsultarPlanPorta;
using System.Configuration;
using System.Data;
namespace Claro.SISACT.WS
{
    class BWConsultaSECPlanPorta
    {
        WSConsultarPlanPorta.BSS_ConsultasSEC_v1 _objTransaccionPlan = null;//segund variable para el metood plan porta

        GeneradorLog _objLog = null;
        private string nameLog = "Log_BWConsultaSECPlanPorta";
        public BWConsultaSECPlanPorta()
        {
            _objTransaccionPlan = new BSS_ConsultasSEC_v1();
            _objTransaccionPlan.Url = Funciones.CheckStr(ConfigurationManager.AppSettings["RutaWS_ConsultaSECPlan"]);// +"consultarPlan";
            _objTransaccionPlan.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["DAT_TimeOut_ConsultaSEC"].ToString());
            _objTransaccionPlan.Credentials = System.Net.CredentialCache.DefaultCredentials;
        }


        public DataSet ObtenerPlanesConVentaSISACT(string CurrentTerminal, string CurrentUser, string strTipoDoc, string strNroDoc, string strLineasIN, ref string outCodigo, ref string outMensaje)
        {
            DataSet dsConsulta = new DataSet();
            string evalVenta = "V";
            GeneradorLog.EscribirLog(nameLog, strNroDoc, "---- Inicio método [ObtenerPlanesConVentaSISACT] ----");
            BWConsultaClaves objConsultaClave = new BWConsultaClaves();
            try
            {
                string codigoResultado = string.Empty;
                string mensajeResultado = string.Empty;
                string usuarioAplicacion = string.Empty;
                string clave = string.Empty;
                string wsIp = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_ConsultaSEC_wsIp"]);
                string ipTransaccion = CurrentTerminal;
                string usrAplicacion = CurrentUser;
                string codigoAplicacion = Funciones.CheckStr(ConfigurationManager.AppSettings["CodigoAplicacion"]);
                string idAplicacion = Funciones.CheckStr(ConfigurationManager.AppSettings["system_ConsultaClave"]);
                string usuarioAplicacionEncriptado = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_User_ConsultaSEC"]);
                string claveEncriptada = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_Password_ConsultaSEC"]);

                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesConVentaSISACT][wsIp]",wsIp));
                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesConVentaSISACT][ipTransaccion]", ipTransaccion));
                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesConVentaSISACT][codigoAplicacion]", codigoAplicacion));
                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesConVentaSISACT][idAplicacion]", idAplicacion));
                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesConVentaSISACT][usuarioAplicacionEncriptado]", usuarioAplicacionEncriptado));
                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesConVentaSISACT][claveEncriptada]", claveEncriptada));

                codigoResultado = objConsultaClave.ConsultaClavesWS(DateTime.Now.ToString("YYYYMMDDHHMISSMS"),
                                                                     wsIp,
                                                                     ipTransaccion,
                                                                     usrAplicacion,
                                                                     codigoAplicacion,
                                                                     idAplicacion,
                                                                     usuarioAplicacionEncriptado,
                                                                     claveEncriptada,
                                                                     out mensajeResultado,
                                                                     out usuarioAplicacion,
                                                                     out clave);

                //usuarioAplicacion = "usrPortUSisactInt";
                //clave = "Q@ve123456";

                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesConVentaSISACT][mensajeResultado]", mensajeResultado));
                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesConVentaSISACT][usuarioAplicacion]", usuarioAplicacion));
                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesConVentaSISACT][clave]", clave));
                //wsse:Security  
                usernameToken = new UsernameToken(usuarioAplicacion, clave, PasswordOption.SendPlainText);
                _objTransaccionPlan.RequestSoapContext.Security.Tokens.Add(usernameToken);

                //DataPower
                //DataPower
                 ArgType[] arg=new ArgType[1];
                 arg[0] = new ArgType
                 {
                     key = String.Empty,
                     value = String.Empty
                 };

                _objTransaccionPlan.HeaderRequest = new HeaderRequestType()
                {
                    country = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_ConsultaNacionalidad_country"]),
                    language = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_ConsultaNacionalidad_language"]),
                    consumer = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_ConsultaNacionalidad_consumer"]),
                    system = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_ConsultaNacionalidad_system"]),
                    modulo = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_ConsultaNacionalidad_modulo"]),
                    pid = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                    userId = CurrentUser,
                    dispositivo = CurrentTerminal,
                    wsIp =wsIp,
                    operation = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_ConsultaSECPlan_operation"]),
                    timestamp = Convert.ToDateTime(string.Format("{0:u}", DateTime.UtcNow)),
                    msgType = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_ConsultaNacionalidad_msgType"]),
                    VarArg = arg
                };

              
                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesConVentaSISACT][HeaderRequest][country]", _objTransaccionPlan.HeaderRequest.country));
                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesConVentaSISACT][HeaderRequest][language]", _objTransaccionPlan.HeaderRequest.language));
                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesConVentaSISACT][HeaderRequest][consumer]", _objTransaccionPlan.HeaderRequest.consumer));
                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesConVentaSISACT][HeaderRequest][system]", _objTransaccionPlan.HeaderRequest.system));
                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesConVentaSISACT][HeaderRequest][modulo]", _objTransaccionPlan.HeaderRequest.modulo));
                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesConVentaSISACT][HeaderRequest][operation]", _objTransaccionPlan.HeaderRequest.operation));
                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesConVentaSISACT][HeaderRequest][msgType]", _objTransaccionPlan.HeaderRequest.msgType));
                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesConVentaSISACT][HeaderRequest][timestamp]", Funciones.CheckStr(_objTransaccionPlan.HeaderRequest.timestamp)));
            
             
                _objTransaccionPlan.headerRequest = new HeaderRequest()
                {
                    channel = string.Empty,
                    idApplication = CurrentTerminal,
                    userApplication = Funciones.CheckStr(ConfigurationManager.AppSettings["usrSisactPrograma"]),
                    userSession = CurrentUser,
                    idESBTransaction = string.Empty,
                    idBusinessTransaction = string.Empty,
                    startDate = Convert.ToDateTime(string.Format("{0:u}", DateTime.UtcNow)),
                    additionalNode = string.Empty,
                };


                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesConVentaSISACT][headerRequest][idApplication]", _objTransaccionPlan.headerRequest.idApplication));
                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesConVentaSISACT][headerRequest][userApplication]", _objTransaccionPlan.headerRequest.userApplication));
                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesConVentaSISACT][headerRequest][userSession]", _objTransaccionPlan.headerRequest.userSession));
                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesConVentaSISACT][headerRequest][startDate]", Funciones.CheckStr(_objTransaccionPlan.headerRequest.startDate)));


                RequestOpcionalTypeRequestOpcional[] listaResquestOpcional = new RequestOpcionalTypeRequestOpcional[1];
                listaResquestOpcional[0] = new RequestOpcionalTypeRequestOpcional
                {
                    campo = String.Empty,
                    valor = String.Empty
                };

                ConsultarPlanRequest objRequest = new ConsultarPlanRequest()
                {
                    tDoccCodigo = strTipoDoc,
                    cliecNumDoc = Funciones.FormatoNroDocumentoBD(strTipoDoc, strNroDoc),
                    listaTelefono = strLineasIN,
                     listaResquestOpcional = listaResquestOpcional
                };

                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesConVentaSISACT][tDoccCodigo][tDoccCodigo]", strTipoDoc));
                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesConVentaSISACT][tDoccCodigo][cliecNumDoc]", Funciones.FormatoNroDocumentoBD(strTipoDoc, strNroDoc)));
                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesConVentaSISACT][tDoccCodigo][listaTelefono]", strLineasIN));

                ConsultarPlanResponse objResponse = _objTransaccionPlan.consultarPlan(objRequest);

                if (objResponse != null && objResponse.responseStatus != null)
                {
                    GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}|{2}", "[ObtenerPlanesConVentaSISACT][consultaPlan][headerRequest][codeResponse]", _objTransaccionPlan.headerRequest.idESBTransaction, objResponse.responseStatus.codeResponse.ToString()));
                    GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}|{2}", "[ObtenerPlanesConVentaSISACT][consultaPlan][headerRequest][descriptionResponse]", _objTransaccionPlan.headerRequest.idESBTransaction, objResponse.responseStatus.descriptionResponse.ToString()));

                    outCodigo = objResponse.responseStatus.codeResponse;
                    outMensaje = objResponse.responseStatus.descriptionResponse;
                    if (outCodigo == "0")
                    {
                        CursorCabTypeCursorCabType[] cursorCab = objResponse.responseData.cursorCab;
                        CursorDetTypeCursorDetType[] cursorDet = objResponse.responseData.cursorDet;

                        GeneradorLog.EscribirLog(nameLog,strNroDoc, string.Format("{0}|{1}|{2}","[ObtenerPlanesConVentaSISACT][consultaPlan][cursorDet]", _objTransaccionPlan.headerRequest.idESBTransaction, cursorCab.Count().ToString()));
                        GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}|{2}", "[ObtenerPlanesConVentaSISACT][consultaPlan][cursorDet]", _objTransaccionPlan.headerRequest.idESBTransaction, cursorDet.Count().ToString()));

                        DataTable dtCab = new DataTable();
                        dtCab.Columns.Add("CUSTOMER_ID", typeof(String));
                        dtCab.Columns.Add("ESTADO", typeof(String));
                        dtCab.Columns.Add("BLOQUEO", typeof(String));
                        dtCab.Columns.Add("SUSPENSION", typeof(String));
                        dtCab.Columns.Add("DEUDA_VENCIDA", typeof(String));
                        dtCab.Columns.Add("DEUDA_CASTIGA", typeof(String));
                        dtCab.Columns.Add("RAZON_SOCIAL", typeof(String));
                        dtCab.Columns.Add("NOMBRE", typeof(String));
                        dtCab.Columns.Add("APELLIDO_PAT", typeof(String));
                        dtCab.Columns.Add("APELLIDO_MAT", typeof(String));
                        dtCab.Columns.Add("DIAS_DEUDA", typeof(String));
                        dtCab.Columns.Add("PROM_FACT", typeof(String));
                        dtCab.Columns.Add("CANT_SERVICIO", typeof(String));

                        DataTable dtDet = new DataTable();
                        dtDet.Columns.Add("CUENTA", typeof(String));
                        dtDet.Columns.Add("PLAN", typeof(String));
                        dtDet.Columns.Add("PLAN_SISACT", typeof(String));
                        dtDet.Columns.Add("TELEFONO", typeof(String));
                        dtDet.Columns.Add("SERVICIO", typeof(String));
                        dtDet.Columns.Add("CARGO_FIJO", typeof(Double));
                        dtDet.Columns.Add("SOLIN_SUM_CAR_CON", typeof(Single));
                        dtDet.Columns.Add("FECHA_ACTIVACION", typeof(String));
                        dtDet.Columns.Add("FECHA_ESTADO", typeof(String));
                        dtDet.Columns.Add("ESTADO", typeof(String));
                        dtDet.Columns.Add("MOTIVO_BLOQUEO", typeof(String));
                        dtDet.Columns.Add("MOTIVO_SUSPENSION", typeof(String));
                        dtDet.Columns.Add("CAMPANA", typeof(String));
                        dtDet.Columns.Add("PLAN_BSCS", typeof(Decimal));
                        dtDet.Columns.Add("TIPO_EVAL", typeof(String));
                        var dtCabFill = cursorCab.Select(p => dtCab.Rows.Add(p.customerId, p.estado, p.bloqueo, p.suspension, p.deudaVencida, p.deudaCastiga,
                                                                             p.razonSocial, p.nombre, p.apellidoPat, p.apellidoMat, p.diasDeuda, p.promFact,
                                                                             p.cantServicio)).ToArray();

                        dsConsulta.Tables.Add(dtCab);

                        var dtDetFill = cursorDet.Select(p => dtDet.Rows.Add(p.cuenta, p.plan, p.planSisact, p.telefono, p.servicio, p.cargoFijo,
                                                                            p.solinSumCarCon, p.fechaActivacion, p.fechaEstado,
                                                                            p.estado, p.motivoBloqueo, p.motivoSuspension, p.campana, p.planBscs,evalVenta)).ToArray();
                        dsConsulta.Tables.Add(dtDet);
                    }



                }
            }
            catch (Exception ex)
            {
                dsConsulta = null;
                 
                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesConVentaSISACT][consultaPlan][Error]", ex.ToString()));

                outCodigo = "-99";
                outMensaje = "El servicio no se encuentra disponible";
            }
            finally
            {
                _objTransaccionPlan.Dispose();
            }

            return dsConsulta;
        }

    }
}