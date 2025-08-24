using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity.ConsultarClienteFullClaroRest;
using Claro.SISACT.Common;
using Claro.SISACT.Entity;
using System.Collections;
using Claro.SISACT.WS.RestServices;

namespace Claro.SISACT.WS.RestReferences
{
    public class RestConsultarClientesFullClaro
    {
        public ConsultarClientesFullClaroResponse ConsultarClienteFullClaro(ConsultarClientesFullClaroRequest objConsultarClientesFullClaroRequest, BEAuditoriaRequest objBEAuditoriaREST) 
        {
            GeneradorLog _objLog = null;
            _objLog = new GeneradorLog(objBEAuditoriaREST.usuarioAplicacion, "Metodo ConsultarClienteFullClaro", objBEAuditoriaREST.idTransaccion, null);
            try
            {
                Hashtable paramHeader = new Hashtable();
                paramHeader.Add("timestamp", objBEAuditoriaREST.timestamp);
                paramHeader.Add("userId", objBEAuditoriaREST.userId);
                paramHeader.Add("idTransaccion", objBEAuditoriaREST.idTransaccion);
                _objLog.CrearArchivolog("---- Parametros de entrada Inicio : ----", null, null);
                _objLog.CrearArchivolog(" timestamp : " + objBEAuditoriaREST.timestamp, null, null);
                _objLog.CrearArchivolog(" userId : " + objBEAuditoriaREST.userId, null, null);
                _objLog.CrearArchivolog(" idTransaccion : " + objBEAuditoriaREST.idTransaccion, null, null);
                _objLog.CrearArchivolog(" ipApplication) : " + objBEAuditoriaREST.ipApplication, null, null);
                return RestServiceConsultaFC.PostInvoque<ConsultarClientesFullClaroResponse>("urlconsultaractivosFC", paramHeader, objConsultarClientesFullClaroRequest, objBEAuditoriaREST.userId, objBEAuditoriaREST.ipApplication);
            }
            catch (Exception ex)
            {
                _objLog.CrearArchivolog("Excepcion metodo ConsultarClienteFullClaro : " + objBEAuditoriaREST.applicationCodeWS, null, ex);
                throw;
            }
        }
    }
}
