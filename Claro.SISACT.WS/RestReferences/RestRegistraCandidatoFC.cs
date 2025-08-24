using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity.RegistraCandidatoFullClaroRest;
using Claro.SISACT.Entity;
using Claro.SISACT.Common;
using System.Collections;
using Claro.SISACT.WS.RestServices;

namespace Claro.SISACT.WS.RestReferences
{
    public class RestRegistraCandidatoFC
    {
        public RegistraCandidatosFullClaroResponse registraCandidatoFC(RegistraCandidatosFullClaroRequest objRegistraCandidatosFullClaroRequest, BEAuditoriaRequest objBEAuditoriaRequest)
        {
            GeneradorLog _objLog = null;
            _objLog = new GeneradorLog(objBEAuditoriaRequest.usuarioAplicacion, "Metodo registraCandidatoFC", objBEAuditoriaRequest.idTransaccion, null);
            try
            {
                Hashtable paramHeader = new Hashtable();
                paramHeader.Add("timestamp", objBEAuditoriaRequest.timestamp);
                paramHeader.Add("userId", objBEAuditoriaRequest.userId);
                paramHeader.Add("idTransaccion", objBEAuditoriaRequest.idTransaccion);
                _objLog.CrearArchivolog("---- Parametros de entrada Inicio : ----", null, null);
                _objLog.CrearArchivolog(" timestamp : " + objBEAuditoriaRequest.timestamp, null, null);
                _objLog.CrearArchivolog(" userId : " + objBEAuditoriaRequest.userId, null, null);
                _objLog.CrearArchivolog(" idTransaccion : " + objBEAuditoriaRequest.idTransaccion, null, null);
                _objLog.CrearArchivolog(" accept : " + objBEAuditoriaRequest.accept, null, null);
                _objLog.CrearArchivolog(" ipApplication) : " + objBEAuditoriaRequest.ipApplication, null, null);
                return RestServiceRegistraCandidatoFC.PostInvoque<RegistraCandidatosFullClaroResponse>("urlregistraCandidatoFC", paramHeader, objRegistraCandidatosFullClaroRequest, objBEAuditoriaRequest.userId, objBEAuditoriaRequest.ipApplication);
            }
            catch (Exception ex)
            {
               _objLog.CrearArchivolog("Excepcion metodo registraCandidatoFC : " + objBEAuditoriaRequest.applicationCodeWS, null, ex);
                throw;
            }

        }
    }
}
