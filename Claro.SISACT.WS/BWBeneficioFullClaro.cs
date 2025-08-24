using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity;
using Claro.SISACT.Common;
using System.Web;
using System.Configuration;

namespace Claro.SISACT.WS
{
    public class BWBeneficioFullClaro
    {
        private static string nameLogFC = "LogBeneficioFullClaro";
        string strArchivo = "BWBeneficioFullClaro";
        string codResExito = "0";
        string strUserEncrypted = Funciones.CheckStr(ConfigurationManager.AppSettings["usuarioEncriptadoBeneficioFC"]);
        string strPassEncrypted = Funciones.CheckStr(ConfigurationManager.AppSettings["passEncriptadoBeneficioFC"]);
        string strDomainUser = HttpContext.Current.Request.ServerVariables["LOGON_USER"];
        string strDispositivo = "";
        string strUserId = "";

        public Boolean EvaluarAplicaDescuento(string estado, string codCampana, string planPvu, string tmCode_poid, string plataForma, ref string bono, ref string meses )
        {
            Boolean resp = false;
            strUserId = HttpContext.Current.Request.ServerVariables["LOGON_USER"].Substring(strDomainUser.IndexOf("\\", System.StringComparison.Ordinal) + 1).ToUpper();
            strDispositivo = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(strDispositivo))
            {
                strDispositivo = HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
            }
            string codResultado = string.Empty;
            string msjResultado = string.Empty;
            Entity.BeneficioFullClaro.validarAplicaDsctoCargoFijo.Request.ValidarAplicaDsctoCFRequest objValidarAplicaDsctoCFRequest = new Entity.BeneficioFullClaro.validarAplicaDsctoCargoFijo.Request.ValidarAplicaDsctoCFRequest();
            Entity.DataPowerRest.HeaderRequest objHeaderRequest = new Entity.DataPowerRest.HeaderRequest();
            Entity.BeneficioFullClaro.validarAplicaDsctoCargoFijo.Request.BodyRequestValidarAplicaDsctoCF objBodyRequest = new Entity.BeneficioFullClaro.validarAplicaDsctoCargoFijo.Request.BodyRequestValidarAplicaDsctoCF();
            Entity.BeneficioFullClaro.validarAplicaDsctoCargoFijo.Response.ValidarAplicaDsctoCFResponse objValidarAplicaDsctoCFResponse = new Entity.BeneficioFullClaro.validarAplicaDsctoCargoFijo.Response.ValidarAplicaDsctoCFResponse();
            RestReferences.RestBeneficioFullClaro objRestFC = new RestReferences.RestBeneficioFullClaro();
            BEAuditoriaRequest objBEAuditoriaREST = new BEAuditoriaRequest();
            BEUsuarioSession oUsuario = (BEUsuarioSession)HttpContext.Current.Session["oUsuario"];

            try
            {
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}", "[INICIO][INICIATIVA-805][EvaluarAplicaDescuento]"));

                #region Request
                #region Header
                objHeaderRequest.consumer = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_FullClaro_consumer"]);
                objHeaderRequest.country = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_FullClaro_country"]);
                objHeaderRequest.dispositivo = strDispositivo;
                objHeaderRequest.language = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_FullClaro_language"]);
                objHeaderRequest.modulo = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_FullClaro_modulo"]);
                objHeaderRequest.msgType = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_FullClaro_msgType"]);
                objHeaderRequest.operation = Funciones.CheckStr("EvaluarAplicaDescuento");
                objHeaderRequest.pid = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                objHeaderRequest.system = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_FullClaro_codSystem"]);
                objHeaderRequest.timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
                objHeaderRequest.userId = strUserId;
                objHeaderRequest.wsIp = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_FullClaro_wsIp"]);

                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[EvaluarAplicaDescuento]objHeaderRequest.consumer: ", Funciones.CheckStr(objHeaderRequest.consumer)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[EvaluarAplicaDescuento]objHeaderRequest.country: ", Funciones.CheckStr(objHeaderRequest.country)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[EvaluarAplicaDescuento]objHeaderRequest.dispositivo: ", Funciones.CheckStr(objHeaderRequest.dispositivo)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[EvaluarAplicaDescuento]objHeaderRequest.language: ", Funciones.CheckStr(objHeaderRequest.language)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[EvaluarAplicaDescuento]objHeaderRequest.modulo: ", Funciones.CheckStr(objHeaderRequest.modulo)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[EvaluarAplicaDescuento]objHeaderRequest.msgType: ", Funciones.CheckStr(objHeaderRequest.msgType)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[EvaluarAplicaDescuento]objHeaderRequest.operation: ", Funciones.CheckStr(objHeaderRequest.operation)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[EvaluarAplicaDescuento]objHeaderRequest.pid: ", Funciones.CheckStr(objHeaderRequest.pid)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[EvaluarAplicaDescuento]objHeaderRequest.system: ", Funciones.CheckStr(objHeaderRequest.system)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[EvaluarAplicaDescuento]objHeaderRequest.timestamp: ", Funciones.CheckStr(objHeaderRequest.timestamp)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[EvaluarAplicaDescuento]objHeaderRequest.userId: ", Funciones.CheckStr(objHeaderRequest.userId)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[EvaluarAplicaDescuento]objHeaderRequest.wsIp: ", Funciones.CheckStr(objHeaderRequest.wsIp)));
                #endregion

                #region Body
                objBodyRequest.PI_ESTADO = Funciones.CheckStr(estado);
                objBodyRequest.PI_COD_CAMPANA = Funciones.CheckStr(codCampana);
                objBodyRequest.PI_PLAN_PVU = Funciones.CheckStr(planPvu);
                objBodyRequest.PI_TMCODE_POID = Funciones.CheckStr(tmCode_poid);

                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[EvaluarAplicaDescuento]objBodyRequest.PI_ESTADO: ", Funciones.CheckStr(objBodyRequest.PI_ESTADO)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[EvaluarAplicaDescuento]objBodyRequest.PI_COD_CAMPANA: ", Funciones.CheckStr(objBodyRequest.PI_COD_CAMPANA)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[EvaluarAplicaDescuento]objBodyRequest.PI_PLAN_PVU: ", Funciones.CheckStr(objBodyRequest.PI_PLAN_PVU)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[EvaluarAplicaDescuento]objBodyRequest.PI_TMCODE_POID: ", Funciones.CheckStr(objBodyRequest.PI_TMCODE_POID)));
                #endregion

                objValidarAplicaDsctoCFRequest.MessageRequest.Header.HeaderRequest = objHeaderRequest;
                objValidarAplicaDsctoCFRequest.MessageRequest.Body = objBodyRequest;

                #endregion

                #region objAuditoria
                objBEAuditoriaREST.idTransaccion = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
                objBEAuditoriaREST.timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
                objBEAuditoriaREST.msgid = System.DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                objBEAuditoriaREST.userId = oUsuario.idVendedorSap;
                #endregion

                #region Response
                objValidarAplicaDsctoCFResponse = (Entity.BeneficioFullClaro.validarAplicaDsctoCargoFijo.Response.ValidarAplicaDsctoCFResponse)objRestFC.ProcesarBeneficioFCResponse("urlValidarAplicaDsctoFC", objValidarAplicaDsctoCFRequest, objBEAuditoriaREST, "0", strUserEncrypted, strPassEncrypted);

                codResultado = objValidarAplicaDsctoCFResponse.MessageResponse.Body.PO_COD_RESULTADO;
                msjResultado = objValidarAplicaDsctoCFResponse.MessageResponse.Body.PO_MSJ_RESULTADO;

                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[EvaluarAplicaDescuento]MessageResponse.Body.PO_COD_RESULTADO: ", Funciones.CheckStr(codResultado)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[EvaluarAplicaDescuento]MessageResponse.Body.PO_MSJ_RESULTADO: ", Funciones.CheckStr(msjResultado)));

                if (codResultado.Equals(codResExito))
                {
                    resp = true;
                    if (objValidarAplicaDsctoCFResponse.MessageResponse.Body.PO_CURSOR.PO_CURSOR_Row.Count > 0)
                    {
                        for (int i = 0; i < objValidarAplicaDsctoCFResponse.MessageResponse.Body.PO_CURSOR.PO_CURSOR_Row.Count; i++ )
                        {
                            if (objValidarAplicaDsctoCFResponse.MessageResponse.Body.PO_CURSOR.PO_CURSOR_Row[i].CCFCV_PLATAFORMA == plataForma)
                            {
                                bono = objValidarAplicaDsctoCFResponse.MessageResponse.Body.PO_CURSOR.PO_CURSOR_Row[i].CCFCV_BONO;
                                meses = objValidarAplicaDsctoCFResponse.MessageResponse.Body.PO_CURSOR.PO_CURSOR_Row[i].CCFCV_MESES;
                            }
                        }
                    }
                    GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[EvaluarAplicaDescuento]MessageResponse.Body.PO_CURSOR.PO_CURSOR_Row[0].CCFCV_BONO: ", bono));
                    GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[EvaluarAplicaDescuento]MessageResponse.Body.PO_CURSOR.PO_CURSOR_Row[0].CCFCV_MESES: ", Funciones.CheckStr(meses)));
                }
                    

                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[EvaluarAplicaDescuento]resp: ", Funciones.CheckStr(resp)));
                #endregion
            }
            catch (Exception ex)
            {
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0} -> {1}", "[INICIATIVA-805][EvaluarAplicaDescuento]", "ERROR"));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0} -> {1}", "ex.ToString", ex.Message));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0} -> {1}", "ex.StackTrace", ex.StackTrace));
            }
            GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}", "[FIN][INICIATIVA-805][EvaluarAplicaDescuento]"));
            return resp;
        }

        public Boolean RegistroLineaMovil(string tipoDoc, string nroDoc, string linea, string coId, string customerId, string estado, string plataforma, string tipoServicio, string codCampana, string campana, string numSec, string numContrato, 
                                          string codProducto, string plan, string planPvu, string bono, string meses, string fullClaro, string usuario)
        {
            Boolean resp = false;
            strUserId = HttpContext.Current.Request.ServerVariables["LOGON_USER"].Substring(strDomainUser.IndexOf("\\", System.StringComparison.Ordinal) + 1).ToUpper();
            strDispositivo = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(strDispositivo))
            {
                strDispositivo = HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
            }
            string codResultado = string.Empty;
            string msjResultado = string.Empty;
            Entity.BeneficioFullClaro.registrarClienteFCDsctoCargoFijo.Request.RegistrarClienteFCDsctCFRequest objRegistrarClienteCFRequest = new Entity.BeneficioFullClaro.registrarClienteFCDsctoCargoFijo.Request.RegistrarClienteFCDsctCFRequest();
            Entity.DataPowerRest.HeaderRequest objHeaderRequest = new Entity.DataPowerRest.HeaderRequest();
            Entity.BeneficioFullClaro.registrarClienteFCDsctoCargoFijo.Request.BodyRequestRegistrarClienteFCDsctCF objBodyRequest = new Entity.BeneficioFullClaro.registrarClienteFCDsctoCargoFijo.Request.BodyRequestRegistrarClienteFCDsctCF();
            Entity.BeneficioFullClaro.registrarClienteFCDsctoCargoFijo.Response.RegistrarClienteFCDsctCFResponse objRegistrarClienteCFResponse = new Entity.BeneficioFullClaro.registrarClienteFCDsctoCargoFijo.Response.RegistrarClienteFCDsctCFResponse();
            RestReferences.RestBeneficioFullClaro objRestFC = new RestReferences.RestBeneficioFullClaro();
            BEAuditoriaRequest objBEAuditoriaREST = new BEAuditoriaRequest();
            BEUsuarioSession oUsuario = (BEUsuarioSession)HttpContext.Current.Session["oUsuario"];

            try
            {
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}", "[INICIO][INICIATIVA-805][RegistroLineaMovil]"));

                #region Request
                #region Header
                objHeaderRequest.consumer = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_FullClaro_consumer"]);
                objHeaderRequest.country = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_FullClaro_country"]);
                objHeaderRequest.dispositivo = strDispositivo;
                objHeaderRequest.language = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_FullClaro_language"]);
                objHeaderRequest.modulo = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_FullClaro_modulo"]);
                objHeaderRequest.msgType = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_FullClaro_msgType"]);
                objHeaderRequest.operation = Funciones.CheckStr("RegistroLineaMovil");
                objHeaderRequest.pid = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                objHeaderRequest.system = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_FullClaro_codSystem"]);
                objHeaderRequest.timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
                objHeaderRequest.userId = strUserId;
                objHeaderRequest.wsIp = ConfigurationManager.AppSettings["DAT_FullClaro_wsIp"];

                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[RegistroLineaMovil]objHeaderRequest.consumer: ", Funciones.CheckStr(objHeaderRequest.consumer)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[RegistroLineaMovil]objHeaderRequest.country: ", Funciones.CheckStr(objHeaderRequest.country)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[RegistroLineaMovil]objHeaderRequest.dispositivo: ", Funciones.CheckStr(objHeaderRequest.dispositivo)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[RegistroLineaMovil]objHeaderRequest.language: ", Funciones.CheckStr(objHeaderRequest.language)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[RegistroLineaMovil]objHeaderRequest.modulo: ", Funciones.CheckStr(objHeaderRequest.modulo)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[RegistroLineaMovil]objHeaderRequest.msgType: ", Funciones.CheckStr(objHeaderRequest.msgType)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[RegistroLineaMovil]objHeaderRequest.operation: ", Funciones.CheckStr(objHeaderRequest.operation)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[RegistroLineaMovil]objHeaderRequest.pid: ", Funciones.CheckStr(objHeaderRequest.pid)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[RegistroLineaMovil]objHeaderRequest.system: ", Funciones.CheckStr(objHeaderRequest.system)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[RegistroLineaMovil]objHeaderRequest.timestamp: ", Funciones.CheckStr(objHeaderRequest.timestamp)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[RegistroLineaMovil]objHeaderRequest.userId: ", Funciones.CheckStr(objHeaderRequest.userId)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[RegistroLineaMovil]objHeaderRequest.wsIp: ", Funciones.CheckStr(objHeaderRequest.wsIp)));
                #endregion

                #region Body
                objBodyRequest.PI_CLBCV_TIPO_DOC = Funciones.CheckStr(tipoDoc);
                objBodyRequest.PI_CLBCV_NRO_DOC = Funciones.CheckStr(nroDoc);
                objBodyRequest.PI_CLBCV_LINEA = Funciones.CheckStr(linea);
                objBodyRequest.PI_CLBCV_COID = Funciones.CheckStr(coId);
                objBodyRequest.PI_CLBCV_CUSTOMERID = Funciones.CheckStr(customerId);
                objBodyRequest.PI_CLBCV_ESTADO = Funciones.CheckStr(estado);
                objBodyRequest.PI_CLBCV_PLATAFORMA = Funciones.CheckStr(plataforma);
                objBodyRequest.PI_CLBCV_TIPO_SERVICIO = Funciones.CheckStr(tipoServicio);
                objBodyRequest.PI_CLBCV_COD_CAMPANA = Funciones.CheckStr(codCampana);
                objBodyRequest.PI_CLBCV_CAMPANA = Funciones.CheckStr(campana);
                objBodyRequest.PI_CLBCN_NUM_SEC = Funciones.CheckStr(numSec);
                objBodyRequest.PI_CLBCV_NUM_CONTRATO = Funciones.CheckStr(numContrato);
                objBodyRequest.PI_CLBCV_COD_PRODUCTO = Funciones.CheckStr(codProducto);
                objBodyRequest.PI_CLBCV_PLAN = Funciones.CheckStr(plan);
                objBodyRequest.PI_CLBCV_PLAN_PVU = Funciones.CheckStr(planPvu);
                objBodyRequest.PI_CLBCV_BONO = bono;
                objBodyRequest.PI_CLBCV_MESES = Funciones.CheckStr(meses);
                objBodyRequest.PI_CLBCV_FULL_CLARO = Funciones.CheckStr(fullClaro);
                objBodyRequest.PI_CLBCV_USU_REG = Funciones.CheckStr(usuario);

                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[RegistroLineaMovil]objBodyRequest.PI_CLBCV_TIPO_DOC: ", Funciones.CheckStr(objBodyRequest.PI_CLBCV_TIPO_DOC)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[RegistroLineaMovil]objBodyRequest.PI_CLBCV_NRO_DOC: ", Funciones.CheckStr(objBodyRequest.PI_CLBCV_NRO_DOC)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[RegistroLineaMovil]objBodyRequest.PI_CLBCV_LINEA: ", Funciones.CheckStr(objBodyRequest.PI_CLBCV_LINEA)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[RegistroLineaMovil]objBodyRequest.PI_CLBCV_COID: ", Funciones.CheckStr(objBodyRequest.PI_CLBCV_COID)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[RegistroLineaMovil]objBodyRequest.PI_CLBCV_CUSTOMERID: ", Funciones.CheckStr(objBodyRequest.PI_CLBCV_CUSTOMERID)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[RegistroLineaMovil]objBodyRequest.PI_CLBCV_ESTADO: ", Funciones.CheckStr(objBodyRequest.PI_CLBCV_ESTADO)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[RegistroLineaMovil]objBodyRequest.PI_CLBCV_PLATAFORMA: ", Funciones.CheckStr(objBodyRequest.PI_CLBCV_PLATAFORMA)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[RegistroLineaMovil]objBodyRequest.PI_CLBCV_TIPO_SERVICIO: ", Funciones.CheckStr(objBodyRequest.PI_CLBCV_TIPO_SERVICIO)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[RegistroLineaMovil]objBodyRequest.PI_CLBCV_COD_CAMPANA: ", Funciones.CheckStr(objBodyRequest.PI_CLBCV_COD_CAMPANA)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[RegistroLineaMovil]objBodyRequest.PI_CLBCV_CAMPANA: ", Funciones.CheckStr(objBodyRequest.PI_CLBCV_CAMPANA)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[RegistroLineaMovil]objBodyRequest.PI_CLBCN_NUM_SEC: ", Funciones.CheckStr(objBodyRequest.PI_CLBCN_NUM_SEC)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[RegistroLineaMovil]objBodyRequest.PI_CLBCV_NUM_CONTRATO: ", Funciones.CheckStr(objBodyRequest.PI_CLBCV_NUM_CONTRATO)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[RegistroLineaMovil]objBodyRequest.PI_CLBCV_COD_PRODUCTO: ", Funciones.CheckStr(objBodyRequest.PI_CLBCV_COD_PRODUCTO)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[RegistroLineaMovil]objBodyRequest.PI_CLBCV_PLAN: ", Funciones.CheckStr(objBodyRequest.PI_CLBCV_PLAN)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[RegistroLineaMovil]objBodyRequest.PI_CLBCV_PLAN_PVU: ", Funciones.CheckStr(objBodyRequest.PI_CLBCV_PLAN_PVU)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[RegistroLineaMovil]objBodyRequest.PI_CLBCV_BONO: ", objBodyRequest.PI_CLBCV_BONO));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[RegistroLineaMovil]objBodyRequest.PI_CLBCV_MESES: ", Funciones.CheckStr(objBodyRequest.PI_CLBCV_MESES)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[RegistroLineaMovil]objBodyRequest.PI_CLBCV_FULL_CLARO: ", Funciones.CheckStr(objBodyRequest.PI_CLBCV_FULL_CLARO)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[RegistroLineaMovil]objBodyRequest.PI_CLBCV_USU_REG: ", Funciones.CheckStr(objBodyRequest.PI_CLBCV_USU_REG)));
                #endregion

                objRegistrarClienteCFRequest.MessageRequest.Header.HeaderRequest = objHeaderRequest;
                objRegistrarClienteCFRequest.MessageRequest.Body = objBodyRequest;

                #endregion

                #region objAuditoria
                objBEAuditoriaREST.idTransaccion = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
                objBEAuditoriaREST.timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
                objBEAuditoriaREST.msgid = System.DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                objBEAuditoriaREST.userId = oUsuario.idVendedorSap;
                #endregion

                #region Response
                objRegistrarClienteCFResponse = (Entity.BeneficioFullClaro.registrarClienteFCDsctoCargoFijo.Response.RegistrarClienteFCDsctCFResponse)objRestFC.ProcesarBeneficioFCResponse("urlRegistrarClienteFC", objRegistrarClienteCFRequest, objBEAuditoriaREST, "1", strUserEncrypted, strPassEncrypted);

                codResultado = objRegistrarClienteCFResponse.MessageResponse.Body.PO_COD_RESULTADO;
                msjResultado = objRegistrarClienteCFResponse.MessageResponse.Body.PO_MSJ_RESULTADO;

                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[RegistroLineaMovil]MessageResponse.Body.PO_COD_RESULTADO: ", Funciones.CheckStr(codResultado)));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[RegistroLineaMovil]MessageResponse.Body.PO_MSJ_RESULTADO: ", Funciones.CheckStr(msjResultado)));

                if (codResultado.Equals(codResExito))
                    resp = true;

                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}{1}", "[RegistroLineaMovil]resp: ", Funciones.CheckStr(resp)));
                #endregion
            }
            catch (Exception ex)
            {
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0} -> {1}", "[INICIATIVA-805][RegistroLineaMovil]", "ERROR"));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0} -> {1}", "ex.ToString", ex.Message));
                GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0} -> {1}", "ex.StackTrace", ex.StackTrace));
            }
            GeneradorLog.EscribirLog(strArchivo, nameLogFC, string.Format("{0}", "[FIN][INICIATIVA-805][RegistroLineaMovil]"));
            return resp;
        }

    }
}
