using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity.RegistrarEvaluacionMejPortaRest;
using Claro.SISACT.Entity;
using Claro.SISACT.Common;
using System.Collections;
using Claro.SISACT.WS.RestServices;
//PROY-140618
namespace Claro.SISACT.WS.RestReferences
{
    public class RestRegistrarEvaMejoraPortabilidad
    {
        public RegistrarEvaluacionMejoraPortaResponse RegistrarEvaluacionMejPor(RegistrarEvaluacionMejoraPortaRequest objRegistrarEvaluacionMejPorRequest, BEAuditoriaRequest objBEAuditoriaRequest)
        {
            GeneradorLog _objLog = null;
            _objLog = new GeneradorLog(objBEAuditoriaRequest.usuarioAplicacion, "Metodo RegistrarEvauacionMejPor", objBEAuditoriaRequest.idTransaccion, null);
            try
            {
                
                Hashtable paramHeader = new Hashtable();
                paramHeader.Add("idTransaccion", objBEAuditoriaRequest.idTransaccion);
                paramHeader.Add("aplicacion", objBEAuditoriaRequest.idAplicacion);
                paramHeader.Add("msgId", objBEAuditoriaRequest.msgid);
                paramHeader.Add("userId", objBEAuditoriaRequest.userId);
                paramHeader.Add("timestamp", objBEAuditoriaRequest.timestamp);
                _objLog.CrearArchivolog("---- Parametros de entrada Inicio : ----", null, null);
                _objLog.CrearArchivolog(" idTransaccion : " + objBEAuditoriaRequest.idTransaccion, null, null);
                _objLog.CrearArchivolog(" aplicacion : " + objBEAuditoriaRequest.idAplicacion, null, null);
                _objLog.CrearArchivolog(" msgId : " + objBEAuditoriaRequest.msgid, null, null);
                _objLog.CrearArchivolog(" userId : " + objBEAuditoriaRequest.userId, null, null);
                _objLog.CrearArchivolog(" timestamp : " + objBEAuditoriaRequest.timestamp, null, null);
                
                return RestServiceRegistrarEvaluacion.PostInvoque<RegistrarEvaluacionMejoraPortaResponse>("ConsURL_RegistrarEvaMejoraPorta", paramHeader, objRegistrarEvaluacionMejPorRequest, objBEAuditoriaRequest.userId, objBEAuditoriaRequest.ipApplication);
            }
            catch (Exception ex)
            {
                _objLog.CrearArchivolog("Excepcion metodo RegistrarEvaluacionMejPor : " + objBEAuditoriaRequest.applicationCodeWS, null, ex);
                throw;
            }

        }
    }
}
