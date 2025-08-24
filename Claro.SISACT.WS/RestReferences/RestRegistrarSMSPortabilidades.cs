using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity;
using Claro.SISACT.Entity.DataPowerRest;
using Claro.SISACT.Entity.RegistrarSMSPortabilidadRest;
using System.Collections;
using Claro.SISACT.Common;
using Claro.SISACT.WS.RestServices;

//PROY-SMS PORTABILIDAD
namespace Claro.SISACT.WS.RestReferences
{
    public class RestRegistrarSMSPortabilidades
    {
        public RegistrarSMSPortabilidadesResponse RegistrarSMSPortabilidades(RegistrarSMSPortabilidadesRequest objRegistrarSMSPortabilidadesRequest, BEAuditoriaRequest objBEAuditoriaREST)
        {
            GeneradorLog objLog = null;
            try
            {
                objLog = new GeneradorLog(objBEAuditoriaREST.usuarioAplicacion, "Metodo RegistrarSMSPortabilidades", objBEAuditoriaREST.idTransaccion, "log_SMSPortabilidad_Registro");
                Hashtable paramHeader = new Hashtable();
                paramHeader.Add("timestamp", objBEAuditoriaREST.timestamp);
                paramHeader.Add("userId", objBEAuditoriaREST.userId);
                paramHeader.Add("idTransaccion", objBEAuditoriaREST.idTransaccion);
                paramHeader.Add("access_token", objRegistrarSMSPortabilidadesRequest.MessageRequest.header.HeaderRequest.access_token); // JRM 
                paramHeader.Add("token_intico", objRegistrarSMSPortabilidadesRequest.MessageRequest.header.HeaderRequest.access_token); // JRM 

                objLog.CrearArchivolog("---- Parametros de entrada Inicio : ----", null, null);
                //INC-SMS_PORTA-INI
                objLog.CrearArchivolog(String.Format("{0} : {1}"," timestamp",objBEAuditoriaREST.timestamp), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}"," userId",objBEAuditoriaREST.userId), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}"," idTransaccion",objBEAuditoriaREST.idTransaccion), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}"," ipApplication)",objBEAuditoriaREST.ipApplication), null, null);
                //INC-SMS_PORTA-FIN
                return RestServiceSMSPorta.PostInvoque<RegistrarSMSPortabilidadesResponse>("urlRegistrarSMSPortaREST", paramHeader, objRegistrarSMSPortabilidadesRequest, objBEAuditoriaREST.userId, objBEAuditoriaREST.ipApplication);
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog(String.Format("{0} : {1}"," Excepcion metodo RegistrarSMSPortabilidades",objBEAuditoriaREST.applicationCodeWS), null, ex); //INC-SMS_PORTA
                throw ex;
            }
        }
    }
}
