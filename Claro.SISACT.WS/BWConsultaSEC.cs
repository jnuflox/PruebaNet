using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Common;
using Claro.SISACT.WS.WSConsultaPlanAprobPorta;
using System.Configuration;
using System.Data;
namespace Claro.SISACT.WS
{
    class BWConsultaSEC
    {
        WSConsultaPlanAprobPorta.BSS_ConsultasSEC_v1 _objTransaccionAprob = null;//primera variable para el metodo planaprob
       
        GeneradorLog _objLog = null;
        string username = null;
        string password = null;
        private string nameLog = "Log_BWConsultaSEC";
        public BWConsultaSEC()
        {
            _objTransaccionAprob = new BSS_ConsultasSEC_v1();
            _objTransaccionAprob.Url = ConfigurationManager.AppSettings["RutaWS_ConsultaSECPlanAprobado"].ToString(); //+ "consultarPlanAprobado";
            _objTransaccionAprob.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["DAT_TimeOut_ConsultaSEC"].ToString());
            _objTransaccionAprob.Credentials = System.Net.CredentialCache.DefaultCredentials;
        }


        public DataSet ObtenerPlanesSinVentaSISACT( string CurrentTerminal, string CurrentUser, string strTipoDoc, string strNroDoc, string strLineasIN, ref string outCodigo, ref string outMensaje)
        {
            DataSet dsConsulta = new DataSet();
            string evalPendiente = "P";
            GeneradorLog.EscribirLog(nameLog, strNroDoc, "---- Inicio método [ObtenerPlanesSinVentaSISACT] ----");
            BWConsultaClaves objConsultaClave = new BWConsultaClaves();
            try
            {
                //172.17.26.51
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

                 GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesSinVentaSISACT][wsIp]", wsIp));
                 GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesSinVentaSISACT][ipTransaccion]", ipTransaccion));
                 GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesSinVentaSISACT][codigoAplicacion]", codigoAplicacion));
                 GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesSinVentaSISACT][idAplicacion]", idAplicacion));
                 GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesSinVentaSISACT][usuarioAplicacionEncriptado]", usuarioAplicacionEncriptado));
                 GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesSinVentaSISACT][claveEncriptada]", claveEncriptada));

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

                //wsse:Security  
                //usuarioAplicacion = "usrPortUSisactInt";
                //clave = "Q@ve123456";
                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesSinVentaSISACT][mensajeResultado]", mensajeResultado));
                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesSinVentaSISACT][usuarioAplicacion]", usuarioAplicacion));
                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesSinVentaSISACT][clave]", clave));
                username = usuarioAplicacion;
                password = clave;
                // Modern WCF client configuration would typically be done via binding configuration
                // For now, we'll store the credentials but not apply them directly as the client implementation has changed

                //DataPower
                ArgType[] arg = new ArgType[1];
                arg[0] = new ArgType
                {
                    key = String.Empty,
                    value = String.Empty
                };
                _objTransaccionAprob.HeaderRequest = new HeaderRequestType()
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
                    operation = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_ConsultaSECPlanAprobado_operation"]),
                    timestamp = Convert.ToDateTime(string.Format("{0:u}", DateTime.UtcNow)),
                    msgType = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_ConsultaNacionalidad_msgType"]),
                    VarArg=arg
                };

                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesSinVentaSISACT][HeaderRequest][country]", _objTransaccionAprob.HeaderRequest.country));
                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesSinVentaSISACT][HeaderRequest][language]", _objTransaccionAprob.HeaderRequest.language));
                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesSinVentaSISACT][HeaderRequest][consumer]", _objTransaccionAprob.HeaderRequest.consumer));
                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesSinVentaSISACT][HeaderRequest][system]", _objTransaccionAprob.HeaderRequest.system));
                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesSinVentaSISACT][HeaderRequest][modulo]", _objTransaccionAprob.HeaderRequest.modulo));
                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesSinVentaSISACT][HeaderRequest][operation]", _objTransaccionAprob.HeaderRequest.operation));
                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesSinVentaSISACT][HeaderRequest][msgType]", _objTransaccionAprob.HeaderRequest.msgType));
                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesSinVentaSISACT][HeaderRequest][timestamp]", _objTransaccionAprob.HeaderRequest.timestamp));
            
                _objTransaccionAprob.headerRequest = new HeaderRequest()
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

                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesSinVentaSISACT][headerRequest][idApplication]", _objTransaccionAprob.headerRequest.idApplication));
                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesSinVentaSISACT][headerRequest][userApplication]", _objTransaccionAprob.headerRequest.userApplication));
                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesSinVentaSISACT][headerRequest][userSession]", _objTransaccionAprob.headerRequest.userSession));
                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesSinVentaSISACT][headerRequest][startDate]", Funciones.CheckStr(_objTransaccionAprob.headerRequest.startDate)));

                RequestOpcionalTypeRequestOpcional[] listaRequestOpcional = new RequestOpcionalTypeRequestOpcional[1];
                listaRequestOpcional[0] = new RequestOpcionalTypeRequestOpcional
                {
                    campo = String.Empty,
                    valor = String.Empty
                };

                ConsultarPlanAprobadoRequest objRequest = new ConsultarPlanAprobadoRequest()
                {
                    tDoccCodigo = strTipoDoc,
                    cliecNumDoc = Funciones.FormatoNroDocumentoBD(strTipoDoc, strNroDoc),
                    listaTelefono = strLineasIN,
                    listaResquestOpcional = listaRequestOpcional
                };
                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesSinVentaSISACT][tDoccCodigo][tDoccCodigo]", strTipoDoc));
                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesSinVentaSISACT][tDoccCodigo][cliecNumDoc]", Funciones.FormatoNroDocumentoBD(strTipoDoc, strNroDoc)));
                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesSinVentaSISACT][tDoccCodigo][listaTelefono]", strLineasIN));
                
              //  _objLog.CrearArchivolog("[ObtenerPlanesSinVentaSISACT][ConsultarPlanAprobadoResponse]", string.Format("{0}|{1}|{2}|{3}", _objTransaccionAprob.headerRequest.idESBTransaction, objRequest.tDoccCodigo, objRequest.cliecNumDoc, objRequest.listaTelefono), null);
                ConsultarPlanAprobadoResponse objResponse = _objTransaccionAprob.consultarPlanAprobado(objRequest);

                if (objResponse != null && objResponse.responseStatus != null)
                {
                    GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}|{2}", "[ObtenerPlanesSinVentaSISACT][consultaPlanAprob][headerRequest][codeResponse]", _objTransaccionAprob.headerRequest.idESBTransaction, objResponse.responseStatus.codeResponse.ToString()));
                    GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}|{2}", "[ObtenerPlanesSinVentaSISACT][consultaPlanAprob][headerRequest][descriptionResponse]", _objTransaccionAprob.headerRequest.idESBTransaction, objResponse.responseStatus.descriptionResponse.ToString()));
                    outCodigo = objResponse.responseStatus.codeResponse;
                    outMensaje = objResponse.responseStatus.descriptionResponse;
                    if (outCodigo=="0")
                    {
                        CursorCabTypeCursorCabType[] cursorCab = objResponse.responseData.cursorCab;
                        CursorDetTypeCursorDetType[] cursorDet = objResponse.responseData.cursorDet;

                        GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}|{2}", "[ObtenerPlanesSinVentaSISACT][consultaPlanAprob][cursorCab]", _objTransaccionAprob.headerRequest.idESBTransaction, cursorCab.Count().ToString()));
                        GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}|{2}", "[ObtenerPlanesSinVentaSISACT][consultaPlanAprob][cursorDet]", _objTransaccionAprob.headerRequest.idESBTransaction, cursorDet.Count().ToString()));

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
                                                                            p.estado, p.motivoBloqueo, p.motivoSuspension, p.campana, p.planBscs, evalPendiente)).ToArray();
                        dsConsulta.Tables.Add(dtDet);
                    }
                    

                    
                }
            }
            catch (Exception ex)
            {
                dsConsulta = null;
                GeneradorLog.EscribirLog(nameLog, strNroDoc, string.Format("{0}|{1}", "[ObtenerPlanesSinVentaSISACT][consultaPlanAprob][Error]", ex.ToString()));
                outCodigo = "-99";
                outMensaje = "El servicio no se encuentra disponible";
            }
            finally
            {
                _objTransaccionAprob.Dispose();
            }

            return dsConsulta;
        }


   
    
    }
}