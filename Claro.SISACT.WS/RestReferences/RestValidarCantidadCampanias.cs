using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity;
using Claro.SISACT.Entity.ConsultarCantidadCampaniaRest;
using Claro.SISACT.Entity.DataPowerRest;
using System.Collections;
using Claro.SISACT.Common;
using Claro.SISACT.WS.RestServices;


namespace Claro.SISACT.WS.RestReferences
{
    //proy-140245 
       public class RestValidarCantidadCampanias
    {
        public ValidarCantidadCampaniasResponse ValidarCantidadCampanias(ValidarCantidadCampaniasRequest objValidarCantCampaniasRequest, BEAuditoriaRequest objBEAuditoriaREST)
        {
            GeneradorLog _objLog = null;
            try
            {
                _objLog = new GeneradorLog(objBEAuditoriaREST.usuarioAplicacion, "Metodo ValidarCantidadCampanias", objBEAuditoriaREST.idTransaccion, "log_ProyOfertaColabMovil");
                Hashtable paramHeader = new Hashtable();
                paramHeader.Add("idTransaccion", objBEAuditoriaREST.idTransaccion);
                paramHeader.Add("ipApplication", objBEAuditoriaREST.ipApplication);
                paramHeader.Add("nombreAplicacion", objBEAuditoriaREST.usuarioAplicacion);
                paramHeader.Add("usuarioAplicacion", objBEAuditoriaREST.usuarioAplicacion);
                paramHeader.Add("nameRegEdit", objBEAuditoriaREST.nameRegEdit);
                paramHeader.Add("userId", objBEAuditoriaREST.userId);
                paramHeader.Add("applicationCode", objBEAuditoriaREST.applicationCode);
                paramHeader.Add("idTransaccionNegocio", objBEAuditoriaREST.idTransaccionNegocio);
                paramHeader.Add("applicationCodeWS", objBEAuditoriaREST.applicationCodeWS);
                
                _objLog.CrearArchivolog("---- Parametros de entrada Inicio : ----", null, null);
                _objLog.CrearArchivolog(" idTransaccion : " + objBEAuditoriaREST.idTransaccion, null, null);
                _objLog.CrearArchivolog(" ipApplication : " + objBEAuditoriaREST.ipApplication, null, null);
                _objLog.CrearArchivolog(" nombreAplicacion : " + objBEAuditoriaREST.usuarioAplicacion, null, null);
                _objLog.CrearArchivolog(" usuarioAplicacion : " + objBEAuditoriaREST.usuarioAplicacion, null, null);
                _objLog.CrearArchivolog(" nameRegEdit : " + objBEAuditoriaREST.nameRegEdit, null, null);
                _objLog.CrearArchivolog(" userId : " + objBEAuditoriaREST.userId, null, null);
                _objLog.CrearArchivolog(" applicationCode : " + objBEAuditoriaREST.applicationCode, null, null);
                _objLog.CrearArchivolog(" idTransaccionNegocio : " + objBEAuditoriaREST.idTransaccionNegocio, null, null);
                _objLog.CrearArchivolog(" applicationCodeWS : " + objBEAuditoriaREST.applicationCodeWS, null, null);
                return RestServiceDP.PostInvoque<ValidarCantidadCampaniasResponse>("strConsultarCampaniasREST", paramHeader, objValidarCantCampaniasRequest);
                  
            }
            catch (Exception ex)
            {
                 _objLog.CrearArchivolog(" Excepcion metodo ValidarCantidadCampanias : " + objBEAuditoriaREST.applicationCodeWS, null, ex);
                throw ex;
               
            }
        }
    }
}
