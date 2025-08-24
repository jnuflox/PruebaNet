using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity.IteracionCliente.Response;
using Claro.SISACT.Common;
using Claro.SISACT.Entity.DataPowerRest.Generic;
using System.Collections;
using Claro.SISACT.WS.RestServices;
using System.Net;
using Claro.SISACT.Entity;

namespace Claro.SISACT.WS.RestReferences
{
    public class RestIteracionCliente : RestServiceGeneric
    {
        public RestIteracionCliente()
        {
            this.SetLogFileName("RestIteracionCliente");
        }

        public ConsultarEvaluacionResponse restConsultarEvaluacion(Dictionary<string, string> parametros, BEAuditoriaRequest objBeAuditRequest)
        {
            string logFileName = string.Format("{0} - {1}", GetLogFileName(), "restConsultarEvaluacion");
            this.SetLogFileName(logFileName);
            GeneradorLog.EscribirLog(logFileName, null, string.Format("{0} {1} {0}", "********************", "Inicio método restConsultarEvaluacion"));
            try
            {
                BEAuditoriaRequest objAuditoria = GetAuditoria(objBeAuditRequest);
                RestServiceDPGeneral.strArchivo = logFileName;

                return RestServiceDPGeneral.HttpCallInvoque<ConsultarEvaluacionResponse>(WebRequestMethods.Http.Get, "ConsultarEvaluacionService_Url", parametros, null, null, objAuditoria);
            }
            catch (Exception ex)
            {
                GeneradorLog.EscribirLog(logFileName, null, string.Format("ERROR. Ha ocurrido un error en el método restConsultarEvaluacion - Message. {0}", ex.Message), null, ex);
                GeneradorLog.EscribirLog(logFileName, null, string.Format("ERROR. Ha ocurrido un error en el método restConsultarEvaluacion - StackTrace. {0}", ex.StackTrace), null, ex);
                throw;
            }
            finally
            {
                GeneradorLog.EscribirLog(logFileName, null, string.Format("{0} {1} {0}", "********************", "Fin método restConsultarEvaluacion"));
            }

        }
    }
}
