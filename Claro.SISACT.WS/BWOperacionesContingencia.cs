using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Common;
using Claro.SISACT.Entity;
using Claro.SISACT.WS.RestReferences;
using Claro.SISACT.Entity.claro_vent_operacionescontingencia.desactivarContingencia;
using Claro.SISACT.Entity.claro_vent_operacionescontingencia.consultarconfiguracion;

namespace Claro.SISACT.WS
{
    public class BWOperacionesContingencia
    {
        public string DesactivarContingencia(string usuario)
        {
            GeneradorLog objlog = new GeneradorLog(string.Format("[{0}][{1}]", "PROY-140715", "DesactivarContingencia"),null,null,null);
            objlog.CrearArchivolog(string.Format("{0}{1}{0}", "*********************", "INICIO MÉTODO DesactivarContingencia"),null,null);
            string validacion = string.Empty;
            string codigoRespuestaServidor = string.Empty;
            string mensajeRespuestaServidor = string.Empty;
            try
            {
                MessageResponseDesactivarContingencia objMessageResponseDesactivarContingencia = new MessageResponseDesactivarContingencia();
                BEAuditoriaRequest objBEAuditoriaRequest = new BEAuditoriaRequest();
                RestOperacionesContingencia objRestOperacionesContingencia = new RestOperacionesContingencia();
                BodyResponseDesactivarContingencia objBodyResponseDesactivarContingencia = new BodyResponseDesactivarContingencia();

                #region Auditoria
                objBEAuditoriaRequest.idTransaccion = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                objBEAuditoriaRequest.timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
                objBEAuditoriaRequest.userId = usuario;
                objBEAuditoriaRequest.msgid = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                objBEAuditoriaRequest.accept = "application/json";

                objlog.CrearArchivolog(string.Format("objBEAuditoriaRequest.idTransaccion => {0}", Funciones.CheckStr(objBEAuditoriaRequest.idTransaccion)), null, null);
                objlog.CrearArchivolog(string.Format("objBEAuditoriaRequest.timestamp => {0}", Funciones.CheckStr(objBEAuditoriaRequest.timestamp)), null, null);
                objlog.CrearArchivolog(string.Format("objBEAuditoriaRequest.userId => {0}", Funciones.CheckStr(objBEAuditoriaRequest.userId)), null, null);
                objlog.CrearArchivolog(string.Format("objBEAuditoriaRequest.msgid => {0}", Funciones.CheckStr(objBEAuditoriaRequest.msgid)), null, null);
                objlog.CrearArchivolog(string.Format("objBEAuditoriaRequest.accept => {0}", Funciones.CheckStr(objBEAuditoriaRequest.accept)), null, null);
                #endregion

                objMessageResponseDesactivarContingencia = objRestOperacionesContingencia.DesactivarContingencia(objBEAuditoriaRequest);

                codigoRespuestaServidor = Funciones.CheckStr(objMessageResponseDesactivarContingencia.MessageResponse.Body.desactivarContingenciaResponse.auditResponse.codigoRespuesta);
                mensajeRespuestaServidor = Funciones.CheckStr(objMessageResponseDesactivarContingencia.MessageResponse.Body.desactivarContingenciaResponse.auditResponse.mensajeRespuesta);

                objlog.CrearArchivolog(string.Format("codigoRespuestaServidor => {0}", codigoRespuestaServidor), null, null);
                objlog.CrearArchivolog(string.Format("mensajeRespuestaServidor => {0}", mensajeRespuestaServidor), null, null);

                if (codigoRespuestaServidor == "0")
                {
                    objBodyResponseDesactivarContingencia = objMessageResponseDesactivarContingencia.MessageResponse.Body;
                    validacion = Funciones.CheckStr(objBodyResponseDesactivarContingencia.desactivarContingenciaResponse.desactivarContingencia.validacion);
                }
            }
            catch (Exception ex)
            {
                objlog.CrearArchivolog(string.Format("Error => {0}", ex.Message), null, null);
                objlog.CrearArchivolog(string.Format("Error => {0}", ex.StackTrace), null, null);
            }
            objlog.CrearArchivolog(string.Format("Validacion => {0}", validacion), null, null);
            objlog.CrearArchivolog(string.Format("{0}{1}{0}", "*********************", "FIN MÉTODO DesactivarContingencia"), null, null);
            return validacion;
        }



        public BEconsultarConfiguracion ConsultarConfiguracion(string usuario, string sistema, string codCtg, string codCanal, string codOficina, string descOficina)
        {
            GeneradorLog objlog = new GeneradorLog(string.Format("[{0}][{1}]", "PROY-140715", "ConsultarConfiguracion"), null, null, null);
            objlog.CrearArchivolog(string.Format("{0}{1}{0}", "*********************", "INICIO MÉTODO ConsultarConfiguracion"), null, null);
            string validacion = string.Empty;
            string codigoRespuestaServidor = string.Empty;
            string mensajeRespuestaServidor = string.Empty;
            BEconsultarConfiguracion consultarconfig = new BEconsultarConfiguracion();
            try
            {
                MessageResponseConsultarConfiguracion objMessageResponseConsultarConfiguracion = new MessageResponseConsultarConfiguracion();
                BEAuditoriaRequest objBEAuditoriaRequest = new BEAuditoriaRequest();
                RestOperacionesContingencia objRestConsultarConfiguracion = new RestOperacionesContingencia();
                BodyResponseConsultarConfiguracion objBodyResponseConsultarConfiguracion = new BodyResponseConsultarConfiguracion();

                #region Auditoria
                objBEAuditoriaRequest.idTransaccion = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                objBEAuditoriaRequest.timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
                objBEAuditoriaRequest.userId = usuario;
                objBEAuditoriaRequest.msgid = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                objBEAuditoriaRequest.accept = "application/json";

                objlog.CrearArchivolog(string.Format("objBEAuditoriaRequest.idTransaccion => {0}", Funciones.CheckStr(objBEAuditoriaRequest.idTransaccion)), null, null);
                objlog.CrearArchivolog(string.Format("objBEAuditoriaRequest.timestamp => {0}", Funciones.CheckStr(objBEAuditoriaRequest.timestamp)), null, null);
                objlog.CrearArchivolog(string.Format("objBEAuditoriaRequest.userId => {0}", Funciones.CheckStr(objBEAuditoriaRequest.userId)), null, null);
                objlog.CrearArchivolog(string.Format("objBEAuditoriaRequest.msgid => {0}", Funciones.CheckStr(objBEAuditoriaRequest.msgid)), null, null);
                objlog.CrearArchivolog(string.Format("objBEAuditoriaRequest.accept => {0}", Funciones.CheckStr(objBEAuditoriaRequest.accept)), null, null);
                #endregion

                objMessageResponseConsultarConfiguracion = objRestConsultarConfiguracion.ConsultarConfiguracion(objBEAuditoriaRequest, sistema, codCtg, codCanal, codOficina, descOficina);

                codigoRespuestaServidor = Funciones.CheckStr(objMessageResponseConsultarConfiguracion.MessageResponse.Body.consultarconfiguracionResponse.auditResponse.codigoRespuesta);
                mensajeRespuestaServidor = Funciones.CheckStr(objMessageResponseConsultarConfiguracion.MessageResponse.Body.consultarconfiguracionResponse.auditResponse.mensajeRespuesta);

                objlog.CrearArchivolog(string.Format("codigoRespuestaServidor => {0}", codigoRespuestaServidor), null, null);
                objlog.CrearArchivolog(string.Format("mensajeRespuestaServidor => {0}", mensajeRespuestaServidor), null, null);

                if (codigoRespuestaServidor == "0")
                {
                    objBodyResponseConsultarConfiguracion = objMessageResponseConsultarConfiguracion.MessageResponse.Body;
                    consultarconfig = objBodyResponseConsultarConfiguracion.consultarconfiguracionResponse.consultarConfiguracion;
                    objlog.CrearArchivolog(string.Format("Cantidad config canal => {0}", consultarconfig.configuracionCanal.Count()), null, null);
                }
            }
            catch (Exception ex)
            {
                objlog.CrearArchivolog(string.Format("Error => {0}", ex.Message), null, null);
                objlog.CrearArchivolog(string.Format("Error => {0}", ex.StackTrace), null, null);
            }
            objlog.CrearArchivolog(string.Format("{0}{1}{0}", "*********************", "FIN MÉTODO ConsultarConfiguracion"), null, null);

            return consultarconfig;
        }


    }
}
