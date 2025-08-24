using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity;
using Claro.SISACT.Entity.ValidarCampanaRest.Request;
using Claro.SISACT.Entity.ValidarCampanaRest.Response;
using Claro.SISACT.Common;
using Claro.SISACT.Entity.DataPowerRest;
using System.Configuration;
using Claro.SISACT.WS.RestReferences;

namespace Claro.SISACT.WS
{
    public class BWValidarCampana
    {
        public bool ConsultarValidacionCampana(string tipoDocumento, string desTipoDocumento, string numeroDocumento, ref string codRespuesta, ref string mensajeRespuesta)
        {
            bool respuestaValidarCampana = false;
            string _idTransaccion = Convert.ToString(DateTime.Now.Year + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second);
            GeneradorLog _objLog = new GeneradorLog("[IDEA-142010][ConsultarValidacionCampana]", numeroDocumento, _idTransaccion, "WEB");
            _objLog.CrearArchivolog(string.Format("{0} => {1}", "[IDEA-142010][ConsultarValidacionCampana]", "[INICIO]"), null, null);
            _objLog.CrearArchivolog("- INICIO: PARAMETROS DE ENTRADA" + null, null, null);

            try
            {
                RestValidarCampana objValidarCampana = new RestValidarCampana();
                ValidarCampanaDtpRequest request = new ValidarCampanaDtpRequest();
                ValidarCampanaDtpResponse response = new ValidarCampanaDtpResponse();
                HeaderRequest objHeaderRequest = new HeaderRequest();

                //REGION HEADER - INI
                objHeaderRequest.consumer = ConfigurationManager.AppSettings["strPostTipClienteConsumer"];
                objHeaderRequest.country = ConfigurationManager.AppSettings["DAT_ConsultaNacionalidad_country"];
                objHeaderRequest.dispositivo = ConfigurationManager.AppSettings["constRBRMS"];//
                objHeaderRequest.language = ConfigurationManager.AppSettings["DAT_ConsultaNacionalidad_language"];
                objHeaderRequest.modulo = ConfigurationManager.AppSettings["DAT_ConsultaNacionalidad_modulo"];
                objHeaderRequest.msgType = ConfigurationManager.AppSettings["DAT_ConsultaNacionalidad_msgType"];
                objHeaderRequest.operation = ConfigurationManager.AppSettings["strOperacionValidarCampana"];
                objHeaderRequest.pid = Convert.ToString(DateTime.Now.Year + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second);
                objHeaderRequest.system = ConfigurationManager.AppSettings["DAT_ConsultaNacionalidad_system"];
                objHeaderRequest.timestamp = Funciones.CheckStr(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ"));
                objHeaderRequest.userId = ConfigurationManager.AppSettings["system_ConsultaClave"];
                objHeaderRequest.wsIp = ConfigurationManager.AppSettings["ValidacionCampanaWsip"];
                objHeaderRequest.VarArg = ConfigurationManager.AppSettings["system_ConsultaClave"];
                _objLog.CrearArchivolog(string.Format("{0} => {1}", "[IDEA-142010][ConsultarValidacionCampana][HeaderRequest Ip]", objHeaderRequest.wsIp), null, null);

                request.MessageRequest.header.HeaderRequest = objHeaderRequest;

                ValidarCampanaRequest objDatosRequest = new ValidarCampanaRequest();
                objDatosRequest.flagLinea = ConfigurationManager.AppSettings["strFlagLineaValidarCampana"];
                objDatosRequest.tipoDocumento = tipoDocumento + "|" + desTipoDocumento;
                objDatosRequest.numeroDocumento = numeroDocumento;

                request.MessageRequest.body.validarCampanaRequest = objDatosRequest;
                response = objValidarCampana.validarCampanasActivas(request);
                _objLog.CrearArchivolog(string.Format("{0} => {1}", "[IDEA-142010][ConsultarValidacionCampana()][Mnesaje mensaje respuesta Dtp]", response.MessageResponse.body.validarCampanaResponse.responseStatus.mensajeRespuesta ), null, null);
                _objLog.CrearArchivolog(string.Format("{0} => {1}", "[IDEA-142010][ConsultarValidacionCampana()][Mnesaje codigo respuesta Dtp]", response.MessageResponse.body.validarCampanaResponse.responseStatus.codigoRespuesta ), null, null);

                if (response.MessageResponse.body.validarCampanaResponse.responseData.campana != null)
                {
                    int numeroCampanasActivas = Funciones.CheckInt(response.MessageResponse.body.validarCampanaResponse.responseData.campana);
                    if (numeroCampanasActivas > 0)
                    {
                        respuestaValidarCampana = true;
                        codRespuesta = "1";
                        mensajeRespuesta = "El cliente si tiene campañas configuradas.";
                    }
                    else {
                        codRespuesta = "2";
                        mensajeRespuesta = "El cliente no tiene campañas configuradas.";
                    }                    
                }
            }
            catch (Exception ex)
            {
                respuestaValidarCampana = false;
                codRespuesta = "-1";
                mensajeRespuesta = Funciones.CheckStr(ex.Message);
                _objLog.CrearArchivolog(string.Format("{0} => {1}", "[IDEA-142010][ConsultarValidacionCampana][Error]", ex.Message), null, null);
            }
            return respuestaValidarCampana;
        }
    }
}
