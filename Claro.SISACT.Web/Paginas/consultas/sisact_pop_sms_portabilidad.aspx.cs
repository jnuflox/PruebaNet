using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Claro.SISACT.Entity.RegistrarSMSPortabilidadRest;
using Claro.SISACT.Entity.ValidarSMSPortabilidadRest;
using Claro.SISACT.Entity.DataPowerRest;
using Claro.SISACT.WS.RestReferences;
using Claro.SISACT.Entity;
using System.Text;
using Claro.SISACT.Common;
using Claro.SISACT.Business;
using System.Net;
using System.IO;
using Claro.SISACT.Entity.Intico;
using Claro.SISACT.WS; //INC-SMS_PORTA

//INI PROY-SMS PORTABILIDAD

namespace Claro.SISACT.Web.Paginas.consultas
{
    public partial class sisact_pop_sms_portabilidad : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        [System.Web.Services.WebMethod()]
        public static BEResponseWebMethod EnviarSMSPortabilidades(string tipoDocumento, string nroDocumento, string nroPortabilidad, string codPortabilidad) //INC-SMS_PORTA
        {
            GeneradorLog objLog = new GeneradorLog(null, "POPUP_SMSPortabilidad", null, "log_PopUp_SMSPortabilidad");
            BEResponseWebMethod respGen = new BEResponseWebMethod();
            try
            {
                //INC-SMS_PORTA_INI
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[PROY-SMSPORTABILIDAD][Page_Load][INICIO]", string.Empty), null, null);
                BEResponseWebMethod resp = new BEResponseWebMethod();
                bool rptaValidacion = false;
                string[] nrotelefonos = ValidarTrazabilidadLineas(tipoDocumento, nroDocumento, codPortabilidad, nroPortabilidad, ref rptaValidacion).Split(';');

                if (rptaValidacion)
                {
                string codigoResp = String.Empty;
                string mensajeResp = String.Empty;
                string cadenaResp = String.Empty;
                string strTimer = String.Empty;
                int intTimer = 0;
                string codigoEnvioSMS = String.Empty;
                int cantNroTelefono = nrotelefonos.Length;

                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[PROY-SMSPORTABILIDAD][Page_Load][tipoDocumento]", tipoDocumento), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[PROY-SMSPORTABILIDAD][Page_Load][nroDocumento]", nroDocumento), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[PROY-SMSPORTABILIDAD][Page_Load][nroPortabilidad]", nrotelefonos), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[PROY-SMSPORTABILIDAD][Page_Load][cantNroTelefono]", cantNroTelefono), null, null);

                    if (cantNroTelefono > 0)
                    {
                        objLog.CrearArchivolog("[PROY-SMSPORTABILIDAD][Page_Load][Existen numero de telefonos]", null, null);

                for (int i = 0; i < cantNroTelefono; i++)
                {
                            resp = EnviarSMSPortabilidad(nroDocumento, Funciones.CheckStr(nrotelefonos[i]));

                    if (resp != null)
                    {
                                codigoResp = Funciones.CheckStr(resp.CodigoError);
                                cadenaResp = Funciones.CheckStr(resp.Mensaje);
                        if (codigoResp.Equals("0"))
                        {
                            string[] dataResp = resp.Cadena.Split(';');
                                    codigoEnvioSMS += string.Format("{0}{1}", Funciones.CheckStr(dataResp[3]), ";");
                                    intTimer += Convert.ToInt32(Funciones.CheckStr(dataResp[2]));
                        }
                        else
                        {
                                    respGen.CodigoError = codigoResp;
                                    respGen.Mensaje = cadenaResp;
                                    objLog.CrearArchivolog(string.Format("{0}-->ERROR[{1}]", "[PROY-SMSPORTABILIDAD][Page_Load]", respGen.Mensaje), null, null);
                            return respGen;
                        }
                    }
                    else
                    {
                        respGen.CodigoError = "-1";
                        respGen.Mensaje = ConfigurationManager.AppSettings["consMsjErrorGeneral"];
                                objLog.CrearArchivolog(string.Format("{0}-->ERROR[{1}]", "[PROY-SMSPORTABILIDAD][Page_Load]", respGen.Mensaje), null, null);
                        return respGen;
                    }
                }
                    }
                    else
                    {
                        objLog.CrearArchivolog("[PROY-SMSPORTABILIDAD][Page_Load][No existen numero de telefonos]", null, null);
                    }
                respGen.CodigoError = "0";
                    respGen.Cadena = string.Format("{0}|{1}", Funciones.CheckStr(intTimer), codigoEnvioSMS);

                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[PROY-SMSPORTABILIDAD][Page_Load][hidTimer]", Funciones.CheckStr(intTimer)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[PROY-SMSPORTABILIDAD][Page_Load][hidCodSMS]", codigoEnvioSMS), null, null);
                }
                else
                {
                    respGen.CodigoError = "-1";
                    respGen.Mensaje = ReadKeySettings.key_MsjNoCumpleValidacion;
                    objLog.CrearArchivolog(string.Format("{0}-->ERROR[{1}]", "[PROY-SMSPORTABILIDAD][Page_Load]", respGen.Mensaje), null, null);
                }
            }
            catch (Exception ex)
            {
                respGen.CodigoError = "-1";
                respGen.Mensaje = ConfigurationManager.AppSettings["consMsjErrorGeneral"];
                objLog.CrearArchivolog(string.Format("{0}-->ERROR[{1}|{2}]", "[PROY-SMSPORTABILIDAD][Page_Load]", Funciones.CheckStr(ex.Message), Funciones.CheckStr(ex.StackTrace)), null, null);
            }
            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[PROY-SMSPORTABILIDAD][Page_Load][FIN]", string.Empty), null, null);
            return respGen;
            //INC-SMS_PORTA_FIN
        }

        //INC-SMS_PORTA_INI
        public static string ValidarTrazabilidadLineas(string strTipoDocumento, string strNumeroDocumento, string codPortabilidad, string nroPortabilidad, ref bool rptaValidacion)
        {
            GeneradorLog objLog = new GeneradorLog(strNumeroDocumento, string.Empty, null, "log_SMSPortabilidad");
            string strNumeroLinea = string.Empty;
            string strCodRpta = string.Empty;
            string strMsjRpta = string.Empty;
            rptaValidacion = false;
            try
            {
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[INICIO][PROY-SMSPORTABILIDAD][ValidarTrazabilidadLineas]", string.Empty), null, null);
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-SMSPORTABILIDAD][ValidarTrazabilidadLineas][strTipoDocumento]", strTipoDocumento), null, null);
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-SMSPORTABILIDAD][ValidarTrazabilidadLineas][strNumeroDocumento]", strNumeroDocumento), null, null);
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-SMSPORTABILIDAD][ValidarTrazabilidadLineas][codPortabilidad - encriptado]", codPortabilidad), null, null);
                codPortabilidad = Funciones.Desencriptar(codPortabilidad);
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-SMSPORTABILIDAD][ValidarTrazabilidadLineas][codPortabilidad]", codPortabilidad), null, null);
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[INICIO][PROY-SMSPORTABILIDAD][ObtenerTrazabilidadPinSMSPorta]", string.Empty), null, null);

                strNumeroLinea = BLEvaluacion.ObtenerTrazabilidadPinSMSPorta(strTipoDocumento, strNumeroDocumento, Funciones.CheckInt64(codPortabilidad), ref strCodRpta, ref strMsjRpta);

                objLog.CrearArchivolog(String.Format("{0} => {1}", "[PROY-SMSPORTABILIDAD][ObtenerTrazabilidadPinSMSPorta][Codigo Respuesta]", strCodRpta), null, null);
                objLog.CrearArchivolog(String.Format("{0} => {1}", "[PROY-SMSPORTABILIDAD][ObtenerTrazabilidadPinSMSPorta][Mensaje Respuesta]", strMsjRpta), null, null);
                objLog.CrearArchivolog(String.Format("{0} => {1}", "[PROY-SMSPORTABILIDAD][ObtenerTrazabilidadPinSMSPorta][Lineas]", strNumeroLinea), null, null);
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[FIN][PROY-SMSPORTABILIDAD][ObtenerTrazabilidadPinSMSPorta]", string.Empty), null, null);

                if (strNumeroLinea == nroPortabilidad)
                {
                    rptaValidacion = true;
                    objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-SMSPORTABILIDAD][ValidarCoindicenciadeLineas]", "Validación exitosa - Las lineas coinciden"), null, null);
                    objLog.CrearArchivolog(string.Format("{0} => {1}", "[FIN][PROY-SMSPORTABILIDAD][ValidarCoindicenciadeLineas]", string.Empty), null, null);
                }
                else
                {
                    objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-SMSPORTABILIDAD][ValidarCoindicenciadeLineas]", "Error en validación - Las lineas NO coinciden"), null, null);
                    objLog.CrearArchivolog(string.Format("{0} => {1}", "[FIN][PROY-SMSPORTABILIDAD][ValidarCoindicenciadeLineas]", string.Empty), null, null);
                }
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[PROY-SMSPORTABILIDAD][ValidarTrazabilidadLineas][Ocurrio un error en el proceso de ValidarTrazabilidadLineas]", null, null);
                objLog.CrearArchivolog(string.Format("{0} => {1} | {2}", "[PROY-SMSPORTABILIDAD][ValidarTrazabilidadLineas][ERROR]", Funciones.CheckStr(ex.Message), Funciones.CheckStr(ex.StackTrace)), null, null);
            }
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[FIN][PROY-SMSPORTABILIDAD][ValidarTrazabilidadLineas]", string.Empty), null, null);
            return strNumeroLinea;
        }
        //INC-SMS_PORTA_FIN

        [System.Web.Services.WebMethod()]
        public static BEResponseWebMethod ValidarCodigoSMSPortabilidades(string strLineas)
        {
            BEResponseWebMethod objResponse = new BEResponseWebMethod();
            string[] arrayNros = strLineas.Split('|');
            StringBuilder sbLineas = new StringBuilder();
            GeneradorLog objLog = new GeneradorLog(null, "ValidarCodigoSMSPortabilidad", null, "log_ValidarCodigoSMSPortabilidad");
            //INC-SMS_PORTA_INI
            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[PROY-SMSPORTABILIDAD][ValidarCodigoSMSPortabilidades][INICIO]", string.Empty), null, null);
            try
            {
                for (int i = 0; i < arrayNros.Length; i++)
                {
                    if (!string.IsNullOrEmpty(Funciones.CheckStr(arrayNros[i])))
                    {
                        string codSms = String.Empty;
                        string pin = String.Empty;
                        string codigoResp = String.Empty;
                        string mensajeResp = String.Empty;
                        string estado = String.Empty;

                        codSms = Funciones.CheckStr(arrayNros[i].Split(';')[0]);
                        pin = Funciones.CheckStr(arrayNros[i].Split(';')[1]);
                        estado = Funciones.CheckStr(arrayNros[i].Split(';')[2]);

                        objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][ValidarCodigoSMSPortabilidades] -> codSms", codSms), null, null);
                        objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][ValidarCodigoSMSPortabilidades] -> pin", pin), null, null);
                        objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][ValidarCodigoSMSPortabilidades] -> estado", estado), null, null);

                        if (estado != "0")
                        {
                            BEResponseWebMethod objResp = new BEResponseWebMethod();
                            objResp = ValidarCodigoSMSPortabilidad(codSms, pin);

                            if (objResp != null)
                            {
                                codigoResp = Funciones.CheckStr(objResp.CodigoError);
                                mensajeResp = Funciones.CheckStr(objResp.Mensaje);

                                sbLineas.Append(i);
                                sbLineas.Append(";");
                                sbLineas.Append(codSms);
                                sbLineas.Append(";");
                                sbLineas.Append(codigoResp);
                                sbLineas.Append(";");
                                sbLineas.Append(mensajeResp);
                                sbLineas.Append("|");

                                objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][ValidarCodigoSMSPortabilidades] -> codigoResp", codigoResp), null, null);
                                objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][ValidarCodigoSMSPortabilidades] -> mensajeResp", mensajeResp), null, null);

                                //INICIO JRM
                                BLIntico objIntico = new BLIntico();
                                RemueveTokenResponse objReponseGeneraToken = objIntico.RemueveTokem(HttpContext.Current.Session["TokenIntico"].ToString());
                                //FIN JRM

                            }
                            else
                            {
                                objResponse.CodigoError = "-1";
                                objResponse.Mensaje = ConfigurationManager.AppSettings["consMsjErrorGeneral"];
                                break;
                            }
                        }
                    }
                }
                objResponse.CodigoError = "0";
                objResponse.Mensaje = "OK";
                objResponse.Cadena = sbLineas.ToString();
                objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][ValidarCodigoSMSPortabilidades] -> objResponse.Cadena", Funciones.CheckStr(objResponse.Cadena)), null, null);
            }
            catch (Exception e)
            {
                objResponse.CodigoError = "-1";
                objResponse.Mensaje = ConfigurationManager.AppSettings["consMsjErrorGeneral"];
                objLog.CrearArchivolog(string.Format("{0}-->ERROR[{1}|{2}]", "[PROY-SMSPORTABILIDAD][ValidarCodigoSMSPortabilidades]", Funciones.CheckStr(e.Message), Funciones.CheckStr(e.StackTrace)), null, null);
            }
            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[PROY-SMSPORTABILIDAD][ValidarCodigoSMSPortabilidades][FIN]", string.Empty), null, null);
            return objResponse;
            //INC-SMS_PORTA_FIN
        }

        public static BEResponseWebMethod EnviarSMSPortabilidad(string strNroDoc, string strLinea)
        {
            StringBuilder cadenaResultado;
            GeneradorLog objLog = new GeneradorLog(null, "EnviarSMSPortabilidad", null, "log_EnviarSMSPortabilidad");
            BEResponseWebMethod objResponse = new BEResponseWebMethod();
            //INC-SMS_PORTA_INI
            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[PROY-SMSPORTABILIDAD][EnviarSMSPortabilidad][INICIO]", string.Empty), null, null);
            RegistrarSMSPortabilidadesRequest objRegistrarSMSPortabilidadesRequest = null;
            RegistrarSMSPortabilidadesResponse objRegistrarSMSPortabilidadesResponse = new RegistrarSMSPortabilidadesResponse();
            string codigoRespuestaServidor = string.Empty;
            string mensajeRespuestaServidor = string.Empty;
            string strCurrentUser = string.Empty;
            string strCurrentServer = string.Empty;
            string strCurrentTerminal = string.Empty;

            try
            {
                //INICIO JRM
                BLIntico objIntico = new BLIntico();
                GeneraTokenResponse objReponseGeneraToken = objIntico.GeneraTokem();
                HttpContext.Current.Session["TokenIntico"] = objReponseGeneraToken.access_token;
                string flagIntico = Funciones.CheckStr(ConfigurationManager.AppSettings["FlagIntico"]);
                //FIN JRM

                objRegistrarSMSPortabilidadesRequest = new RegistrarSMSPortabilidadesRequest();
                cadenaResultado = new StringBuilder();
                BEAuditoriaRequest objBEAuditoriaRequest = new BEAuditoriaRequest();
                Claro.SISACT.Entity.DataPowerRest.HeaderRequest objHeaderRequest = new Claro.SISACT.Entity.DataPowerRest.HeaderRequest();
                RegistrarSMSPortabilidadRequest objRegistrarSMSPortabilidadRequest = new RegistrarSMSPortabilidadRequest();
                RestRegistrarSMSPortabilidades objRestRegistrarSMSPortabilidades = new RestRegistrarSMSPortabilidades();
                DataRegResponse data = new DataRegResponse();

                strCurrentUser = Funciones.CheckStr(Claro.SISACT.Web.Base.Sisact_Webbase.CurrentUsers);
                strCurrentServer = Funciones.CheckStr(Claro.SISACT.Web.Base.Sisact_Webbase.CurrentServer);
                strCurrentTerminal = Funciones.CheckStr(Claro.SISACT.Web.Base.Sisact_Webbase.CurrentTerminal);

                #region Header
                objHeaderRequest.consumer = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_ConsultaNacionalidad_consumer"]);
                objHeaderRequest.country = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_ConsultaNacionalidad_country"]);
                objHeaderRequest.dispositivo = strCurrentTerminal;
                objHeaderRequest.language = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_ConsultaNacionalidad_language"]);
                objHeaderRequest.modulo = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_SMSPorta_modulo"]);
                objHeaderRequest.msgType = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_ConsultaNacionalidad_msgType"]);
                objHeaderRequest.operation = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_SMSPorta_operation"]);
                objHeaderRequest.pid = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                objHeaderRequest.system = Funciones.CheckStr(ConfigurationManager.AppSettings["system_ConsultaClave"]);
                objHeaderRequest.timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
                objHeaderRequest.userId = strCurrentUser;
                objHeaderRequest.wsIp = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_SMSPorta_wsIp"]);
                objHeaderRequest.access_token = objReponseGeneraToken.access_token;//JRM

                objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][EnviarSMSPortabilidad] -> objHeaderRequest.consumer", objHeaderRequest.consumer), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][EnviarSMSPortabilidad] -> objHeaderRequest.country", objHeaderRequest.country), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][EnviarSMSPortabilidad] -> objHeaderRequest.dispositivo", objHeaderRequest.dispositivo), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][EnviarSMSPortabilidad] -> objHeaderRequest.language", objHeaderRequest.language), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][EnviarSMSPortabilidad] -> objHeaderRequest.modulo", objHeaderRequest.modulo), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][EnviarSMSPortabilidad] -> objHeaderRequest.msgType", objHeaderRequest.msgType), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][EnviarSMSPortabilidad] -> objHeaderRequest.operation", objHeaderRequest.operation), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][EnviarSMSPortabilidad] -> objHeaderRequest.pid", objHeaderRequest.pid), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][EnviarSMSPortabilidad] -> objHeaderRequest.system", objHeaderRequest.system), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][EnviarSMSPortabilidad] -> objHeaderRequest.timestamp", objHeaderRequest.timestamp), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][EnviarSMSPortabilidad] -> objHeaderRequest.userId", objHeaderRequest.userId), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][EnviarSMSPortabilidad] -> objHeaderRequest.wsIp", objHeaderRequest.wsIp), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][EnviarSMSPortabilidad] -> objHeaderRequest.wsIp", objHeaderRequest.access_token), null, null); //JRM
                objRegistrarSMSPortabilidadesRequest.MessageRequest.header.HeaderRequest = objHeaderRequest;
                #endregion

                #region Body
                objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][EnviarSMSPortabilidad] -> body.linea", strLinea), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][EnviarSMSPortabilidad] -> body.numeroDocumento", strNroDoc), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][EnviarSMSPortabilidad] -> body.flagEnvioSMS", "1"), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][EnviarSMSPortabilidad] -> body.flagSMSIndico", flagIntico), null, null);

                objRegistrarSMSPortabilidadesRequest.MessageRequest.body = objRegistrarSMSPortabilidadRequest;
                objRegistrarSMSPortabilidadesRequest.MessageRequest.body.linea = strLinea;
                objRegistrarSMSPortabilidadesRequest.MessageRequest.body.numeroDocumento = strNroDoc;
                objRegistrarSMSPortabilidadesRequest.MessageRequest.body.flagEnvioSMS = "1";
                objRegistrarSMSPortabilidadesRequest.MessageRequest.body.flagSMSIndico = flagIntico; //JRM - INTICO
                #endregion

                #region Auditoria
                objBEAuditoriaRequest.idTransaccion = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                objBEAuditoriaRequest.timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
                objBEAuditoriaRequest.userId = Funciones.CheckStr(ConfigurationManager.AppSettings["system_ConsultaClave"]); // INC000002368760
                objBEAuditoriaRequest.ipApplication = strCurrentServer;
                #endregion

                #region Response
                objRegistrarSMSPortabilidadesResponse = objRestRegistrarSMSPortabilidades.RegistrarSMSPortabilidades(objRegistrarSMSPortabilidadesRequest, objBEAuditoriaRequest);

                codigoRespuestaServidor = objRegistrarSMSPortabilidadesResponse.MessageResponse.body.codigoRespuesta;
                mensajeRespuestaServidor = objRegistrarSMSPortabilidadesResponse.MessageResponse.body.mensajeRespuesta;
                data = objRegistrarSMSPortabilidadesResponse.MessageResponse.body.Data;
                cadenaResultado.Append(data.numeroIntentos);
                cadenaResultado.Append(";");
                cadenaResultado.Append(data.numeroReintentos);
                cadenaResultado.Append(";");
                cadenaResultado.Append(data.tiempoVentanaModal);
                cadenaResultado.Append(";");
                cadenaResultado.Append(data.envioSms);
                objResponse.CodigoError = codigoRespuestaServidor;
                objResponse.Mensaje = mensajeRespuestaServidor;
                objResponse.Cadena = cadenaResultado.ToString();

                objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][EnviarSMSPortabilidad] -> Resultado.codigoRespuestaServidor", codigoRespuestaServidor), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][EnviarSMSPortabilidad] -> Resultado.mensajeRespuestaServidor", mensajeRespuestaServidor), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][EnviarSMSPortabilidad] -> Resultado.data.numeroIntentos", data.numeroIntentos), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][EnviarSMSPortabilidad] -> Resultado.data.numeroReintentos", data.numeroReintentos), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][EnviarSMSPortabilidad] -> Resultado.data.tiempoVentanaModal", data.tiempoVentanaModal), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][EnviarSMSPortabilidad] -> Resultado.data.envioSms", data.envioSms), null, null);
                #endregion
            }
            catch (Exception e)
            {
                objResponse.CodigoError = "-1";
                objResponse.Mensaje = ConfigurationManager.AppSettings["consMsjErrorGeneral"];
                objLog.CrearArchivolog(string.Format("{0}-->ERROR[{1}|{2}]", "[PROY-SMSPORTABILIDAD][EnviarSMSPortabilidad]", Funciones.CheckStr(e.Message), Funciones.CheckStr(e.StackTrace)), null, null);
            }
            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[PROY-SMSPORTABILIDAD][EnviarSMSPortabilidad][FIN]", string.Empty), null, null);
            return objResponse;
            //INC-SMS_PORTA_FIN
        }

        public static BEResponseWebMethod ValidarCodigoSMSPortabilidad(string strCodigo, string strPin)
        {
            GeneradorLog objLog = new GeneradorLog(null, "ValidarCodigoSMSPortabilidad", null, "log_ValidarCodigoSMSPortabilidad");
            BEResponseWebMethod objResponse = new BEResponseWebMethod();
            //INC-SMS_PORTA_INI
            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[PROY-SMSPORTABILIDAD][ValidarCodigoSMSPortabilidad][INICIO]", string.Empty), null, null);
            ValidarSMSPortabilidadesRequest objValidarSMSPortabilidadesRequest = null;
            ValidarSMSPortabilidadesResponse objValidarSMSPortabilidadesResponse = new ValidarSMSPortabilidadesResponse();
            string codigoRespuestaServidor = string.Empty;
            string mensajeRespuestaServidor = string.Empty;
            string strCurrentUser = string.Empty;
            string strCurrentTerminal = string.Empty;

            try
            {
                objValidarSMSPortabilidadesRequest = new ValidarSMSPortabilidadesRequest();
                BEAuditoriaRequest objBEAuditoriaRequest = new BEAuditoriaRequest();
                Claro.SISACT.Entity.DataPowerRest.HeaderRequest objHeaderRequest = new Claro.SISACT.Entity.DataPowerRest.HeaderRequest();
                ValidarSMSPortabilidadRequest objRegistrarSMSPortabilidadRequest = new ValidarSMSPortabilidadRequest();
                RestValidarSMSPortabilidades objRestValidarSMSPortabilidades = new RestValidarSMSPortabilidades();

                strCurrentUser = Funciones.CheckStr(Claro.SISACT.Web.Base.Sisact_Webbase.CurrentUsers);
                strCurrentTerminal = Funciones.CheckStr(Claro.SISACT.Web.Base.Sisact_Webbase.CurrentTerminal);

                #region Header
                objHeaderRequest.consumer = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_ConsultaNacionalidad_consumer"]);
                objHeaderRequest.country = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_ConsultaNacionalidad_country"]);
                objHeaderRequest.dispositivo = strCurrentTerminal;
                objHeaderRequest.language = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_ConsultaNacionalidad_language"]);
                objHeaderRequest.modulo = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_SMSPorta_modulo"]);
                objHeaderRequest.msgType = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_ConsultaNacionalidad_msgType"]);
                objHeaderRequest.operation = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_SMSPorta_operation"]);
                objHeaderRequest.pid = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                objHeaderRequest.system = Funciones.CheckStr(ConfigurationManager.AppSettings["system_ConsultaClave"]);
                objHeaderRequest.timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
                objHeaderRequest.userId = strCurrentUser;
                objHeaderRequest.wsIp = Funciones.CheckStr(ConfigurationManager.AppSettings["DAT_SMSPorta_wsIp"]);

                objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][ValidarCodigoSMSPortabilidad] -> objHeaderRequest.consumer", objHeaderRequest.consumer), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][ValidarCodigoSMSPortabilidad] -> objHeaderRequest.country", objHeaderRequest.country), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][ValidarCodigoSMSPortabilidad] -> objHeaderRequest.dispositivo", objHeaderRequest.dispositivo), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][ValidarCodigoSMSPortabilidad] -> objHeaderRequest.language", objHeaderRequest.language), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][ValidarCodigoSMSPortabilidad] -> objHeaderRequest.modulo", objHeaderRequest.modulo), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][ValidarCodigoSMSPortabilidad] -> objHeaderRequest.msgType", objHeaderRequest.msgType), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][ValidarCodigoSMSPortabilidad] -> objHeaderRequest.operation", objHeaderRequest.operation), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][ValidarCodigoSMSPortabilidad] -> objHeaderRequest.pid", objHeaderRequest.pid), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][ValidarCodigoSMSPortabilidad] -> objHeaderRequest.system", objHeaderRequest.system), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][ValidarCodigoSMSPortabilidad] -> objHeaderRequest.timestamp", objHeaderRequest.timestamp), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][ValidarCodigoSMSPortabilidad] -> objHeaderRequest.userId", objHeaderRequest.userId), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][ValidarCodigoSMSPortabilidad] -> objHeaderRequest.wsIp", objHeaderRequest.wsIp), null, null);
                objValidarSMSPortabilidadesRequest.MessageRequest.header.HeaderRequest = objHeaderRequest;
                #endregion

                #region Body
                objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][ValidarCodigoSMSPortabilidad] -> body.envioSms", strCodigo), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][ValidarCodigoSMSPortabilidad] -> body.codigo", strPin), null, null);

                objValidarSMSPortabilidadesRequest.MessageRequest.body = objRegistrarSMSPortabilidadRequest;
                objValidarSMSPortabilidadesRequest.MessageRequest.body.envioSms = strCodigo;
                objValidarSMSPortabilidadesRequest.MessageRequest.body.codigo = strPin;
                #endregion

                #region Auditoria
                objBEAuditoriaRequest.idTransaccion = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                objBEAuditoriaRequest.timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
                objBEAuditoriaRequest.userId = Funciones.CheckStr(ConfigurationManager.AppSettings["system_ConsultaClave"]); // INC000002368760
                #endregion

                #region Response
                objValidarSMSPortabilidadesResponse = objRestValidarSMSPortabilidades.ValidarSMSPortabilidades(objValidarSMSPortabilidadesRequest, objBEAuditoriaRequest);

                codigoRespuestaServidor = objValidarSMSPortabilidadesResponse.MessageResponse.body.codigoRespuesta;
                mensajeRespuestaServidor = objValidarSMSPortabilidadesResponse.MessageResponse.body.mensajeRespuesta;
                objResponse.CodigoError = codigoRespuestaServidor;
                objResponse.Mensaje = mensajeRespuestaServidor;

                objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][ValidarCodigoSMSPortabilidad] -> Resultado.codigoRespuestaServidor", codigoRespuestaServidor), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-SMSPORTABILIDAD][ValidarCodigoSMSPortabilidad] -> Resultado.mensajeRespuestaServidor", mensajeRespuestaServidor), null, null);
                #endregion
            }
            catch (Exception e)
            {
                objResponse.CodigoError = "-1";
                objResponse.Mensaje = ConfigurationManager.AppSettings["consMsjErrorGeneral"];
                objLog.CrearArchivolog(String.Format("{0}-->ERROR[{1}|{2}]", "[PROY-SMSPORTABILIDAD][ValidarCodigoSMSPortabilidad]", Funciones.CheckStr(e.Message), Funciones.CheckStr(e.StackTrace)), null, null);
            }
            objLog.CrearArchivolog(String.Format("{0}-->{1}", "[PROY-SMSPORTABILIDAD][ValidarCodigoSMSPortabilidad][FIN]", string.Empty), null, null);
            return objResponse;
            //INC-SMS_PORTA_FIN
        }

        //INC-SMS_PORTA_INI
        [System.Web.Services.WebMethod()]
        public static BEResponseWebMethod ActualizarTrazabilidadLineas(string strTipoDocumento, string strNumeroDocumento, string codPortabilidad, string msg)
        {
            GeneradorLog objLog = new GeneradorLog(strNumeroDocumento, string.Empty, null, "log_SMSPortabilidad");
            BEResponseWebMethod objResponse = new BEResponseWebMethod();
            string strflagSMS = string.Empty;
            string strcanales = String.Empty;
            string strNodoSisact = string.Empty;
            string strCodRpta = string.Empty;
            string strMsjRpta = string.Empty;
            Boolean salida = false;
            Int64 codigoValidador = 0;
            string strCodigoValidador = string.Empty;
            objResponse.CodigoError = "1";
            objResponse.DescripcionError = ReadKeySettings.key_MsjErrorValidacionPIN;
            objResponse.Cadena = string.Empty;
            string nroPortabilidad = string.Empty;

            try
            {
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[INICIO][PROY-SMSPORTABILIDAD][ActualizarTrazabilidadLineas]", string.Empty), null, null);
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-SMSPORTABILIDAD][ActualizarTrazabilidadLineas][tipoDocumento]", strTipoDocumento), null, null);
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-SMSPORTABILIDAD][ActualizarTrazabilidadLineas][numeroDoc]", strNumeroDocumento), null, null);
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-SMSPORTABILIDAD][ActualizarTrazabilidadLineas][codPortabilidad - encriptado]", codPortabilidad), null, null);

                codPortabilidad = Funciones.Desencriptar(codPortabilidad);

                codigoValidador = Funciones.CheckInt64(codPortabilidad) + 1;
                strCodigoValidador = Funciones.Encriptar(Funciones.CheckStr(codigoValidador));

                objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-SMSPORTABILIDAD][ActualizarTrazabilidadLineas][codPortabilidad]", codPortabilidad), null, null);
                nroPortabilidad = BLEvaluacion.ObtenerTrazabilidadPinSMSPorta(strTipoDocumento, strNumeroDocumento, Funciones.CheckInt64(codPortabilidad), ref strCodRpta, ref strMsjRpta);

                objLog.CrearArchivolog(string.Format("{0} =>{1}", "[PROY-SMSPORTABILIDAD][ActualizarTrazabilidadLineas][nroPortabilidad]", Funciones.CheckStr(nroPortabilidad)), null, null);
                objLog.CrearArchivolog(String.Format("{0} => {1}", "[PROY-SMSPORTABILIDAD][ActualizarTrazabilidadLineas][Solicitar PIN de Portabilidad a los numeros]", Funciones.CheckStr(nroPortabilidad)), null, null);

                /*Actualizar trazabilidad de Pin SMS*/
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[INICIO][PROY-SMSPORTABILIDAD][ActualizarTrazabilidadPinSMSPorta]", string.Empty), null, null);

                if (!String.IsNullOrEmpty(nroPortabilidad))
                {
                    salida = BLEvaluacion.ActualizarTrazabilidadPinSMSPorta(strTipoDocumento, strNumeroDocumento, Funciones.CheckStr(nroPortabilidad), Funciones.CheckInt64(codPortabilidad), ref strCodRpta, ref strMsjRpta);

                    if (salida)
                    {
                        objLog.CrearArchivolog(String.Format("{0} => {1}", "[PROY-SMSPORTABILIDAD][ActualizarTrazabilidadPinSMSPorta][Exito al Registrar/Actualizar las lineas : Codigo Respuesta]", strCodRpta), null, null);
                        objLog.CrearArchivolog(String.Format("{0} => {1}", "[PROY-SMSPORTABILIDAD][ActualizarTrazabilidadPinSMSPorta][Exito al Registrar/Actualizar las lineas: Mensaje Respuesta]", strMsjRpta), null, null);
                        objLog.CrearArchivolog("[PROY-SMSPORTABILIDAD][ActualizarTrazabilidadPinSMSPorta][Las lineas se han actualizado correctamente]", null, null);
                        objResponse.CodigoError = "0";
                        objResponse.DescripcionError = string.Empty;
                        objResponse.Cadena = strCodigoValidador;
                        //PROY-140585 F2 - INICIO                        
                        objResponse.Cadena = objResponse.Cadena +"|"+codPortabilidad;
                        HttpContext.Current.Session["SMSPNCodigoPorta"] = codPortabilidad;
                        objLog.CrearArchivolog(string.Format("{0}:{1}", "[PROY-140585 F2][ActualizarTrazabilidadPinSMSPorta][objResponse.Cadena]", Funciones.CheckStr(objResponse.Cadena)), null, null);
                        objLog.CrearArchivolog(string.Format("{0}:{1}", "[PROY-140585 F2][ActualizarTrazabilidadPinSMSPorta][Session(SMSPNCodigoPorta)]", Funciones.CheckStr(HttpContext.Current.Session["SMSPNCodigoPorta"])), null, null);
                        //PROY-140585 F2 - FIN
        }
                    else
                    {
                        objLog.CrearArchivolog(String.Format("{0} => {1}", "[PROY-SMSPORTABILIDAD][ActualizarTrazabilidadPinSMSPorta][Error al Registrar/Actualizar las líneas : Codigo Respuesta]", strCodRpta), null, null);
                        objLog.CrearArchivolog(String.Format("{0} => {1}", "[PROY-SMSPORTABILIDAD][ActualizarTrazabilidadPinSMSPorta][Error al Registrar/Actualizar las líneas : Mensaje Respuesta]", strMsjRpta), null, null);
                    }
                }
                else
                {
                    objLog.CrearArchivolog("[PROY-SMSPORTABILIDAD][ActualizarTrazabilidadPinSMSPorta][No se ha enviado ninguna línea a actualizar]", null, null);
                }
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[FIN][PROY-SMSPORTABILIDAD][ActualizarTrazabilidadPinSMSPorta]", string.Empty), null, null);
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[PROY-SMSPORTABILIDAD][ActualizarTrazabilidadPinSMSPorta][Ocurrio un error al ActualizarTrazabilidadPinSMSPorta]", null, null);
                objLog.CrearArchivolog(string.Format("{0} {1} | {2}", "[PROY-SMSPORTABILIDAD][ActualizarTrazabilidadPinSMSPorta][ERROR]", Funciones.CheckStr(ex.Message), Funciones.CheckStr(ex.StackTrace)), null, null);
            }
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[FIN][PROY-SMSPORTABILIDAD][ActualizarTrazabilidadLineas]", string.Empty), null, null);
            return objResponse;
        }
        //INC-SMS_PORTA_FIN
    }
}