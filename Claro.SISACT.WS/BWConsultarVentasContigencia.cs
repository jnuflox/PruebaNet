using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity.claro_vent_ventascontingencia;
using Claro.SISACT.Entity;
using Claro.SISACT.WS.RestReferences;
using Claro.SISACT.Common;
using System.Configuration;

namespace Claro.SISACT.WS
{
    public class BWConsultarVentasContigencia
    {
        static string strArchivo = "BWConsultarVentasContigencia";

        public List<BEVentasContingencia> consultarVentasContigencia(BEVenta objBEVenta, string strUsuario)
        {
            string idLogDatos = "consultarVentasContigencia";
            GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("{0}{1}{0}", "*********************", "consultarVentasContigencia INICIO"));

            MessageResponseVentasContingencia objMessageResponseVentasContingencia = new MessageResponseVentasContingencia();
            List<BEVentasContingencia> listaVentasContingencia = new List<BEVentasContingencia>();
            string codigoRespuestaServidor = string.Empty;
            string mensajeRespuestaServidor = string.Empty;
            try
            {
                BEAuditoriaRequest objBEAuditoriaRequest = new BEAuditoriaRequest();
                RestConsultarVentasContingencia objRestConsultarVentasContingencia = new RestConsultarVentasContingencia();

                #region Auditoria
                objBEAuditoriaRequest.idTransaccion = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                objBEAuditoriaRequest.timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
                objBEAuditoriaRequest.userId = strUsuario;
                objBEAuditoriaRequest.msgid = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                objBEAuditoriaRequest.accept = "application/json";

                GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("objBEAuditoriaRequest.idTransaccion => {0}", Funciones.CheckStr(objBEAuditoriaRequest.idTransaccion)));
                GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("objBEAuditoriaRequest.timestamp => {0}", Funciones.CheckStr(objBEAuditoriaRequest.timestamp)));
                GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("objBEAuditoriaRequest.userId => {0}", Funciones.CheckStr(objBEAuditoriaRequest.userId)));
                GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("objBEAuditoriaRequest.msgid => {0}", Funciones.CheckStr(objBEAuditoriaRequest.msgid)));
                GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("objBEAuditoriaRequest.accept => {0}", Funciones.CheckStr(objBEAuditoriaRequest.accept)));
                #endregion

                objMessageResponseVentasContingencia = objRestConsultarVentasContingencia.ConsultarVentasContingencia(objBEVenta, objBEAuditoriaRequest);

                codigoRespuestaServidor = Funciones.CheckStr(objMessageResponseVentasContingencia.MessageResponse.Body.consultarVentasContingenciaResponse.auditResponse.codigoRespuesta);
                mensajeRespuestaServidor = Funciones.CheckStr(objMessageResponseVentasContingencia.MessageResponse.Body.consultarVentasContingenciaResponse.auditResponse.mensajeRespuesta);

                GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("codigoRespuestaServidor => {0}", codigoRespuestaServidor));
                GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("mensajeRespuestaServidor => {0}", mensajeRespuestaServidor));

                if (codigoRespuestaServidor == "0")
                {
                    foreach (var list in objMessageResponseVentasContingencia.MessageResponse.Body.consultarVentasContingenciaResponse.ventasContingencia)
                    {
                        listaVentasContingencia.Add(list);
                    }
                }
                GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("Cantidad en lista => {0}", listaVentasContingencia.Count()));
            }

            catch (Exception ex)
            {
                codigoRespuestaServidor = "-1";
                mensajeRespuestaServidor = Funciones.CheckStr(ex.Message);

                GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("codigoRespuestaServidor => {0}", codigoRespuestaServidor));
                GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("mensajeRespuestaServidor => {0}", mensajeRespuestaServidor));
                GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("Cantidad en lista => {0}", listaVentasContingencia.Count()));

                GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("Error => {0}", ex.Message));
                GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("Error => {0}", ex.StackTrace));
            }

            GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("{0}{1}{0}", "*********************", "consultarVentasContigencia FIN"));

            return listaVentasContingencia;
        }
        //PROY-140715 FIN
    }
}
