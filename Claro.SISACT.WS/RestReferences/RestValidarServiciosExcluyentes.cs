using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity;
using Claro.SISACT.Entity.DataPowerRest;
using Claro.SISACT.Entity.ValidarServExcluyentesRest;
using Claro.SISACT.Entity.RegistrarServExcluyentesRest;
using System.Collections;
using Claro.SISACT.Common;
using Claro.SISACT.WS.RestServices;

namespace Claro.SISACT.WS.RestReferences
{
    public class RestValidarServiciosExcluyentes
    {
        public ValidarServiciosExcluyentesResponse ValidarServiciosExcluyentes(ValidarServiciosExcluyentesRequest objValidarServExcluyentesRequest, BEAuditoriaRequest objBEAuditoriaRest, string strUsuarioValidEncrip, string strContraValidEncrip)
        {
            GeneradorLog objLog = null;
            try
            {
                objLog = new GeneradorLog(objBEAuditoriaRest.usuarioAplicacion, "Metodo ValidarServiciosExcluyentes", objBEAuditoriaRest.idTransaccion, "log_ValidarServExclu");

                Hashtable paramHeader = new Hashtable();
                paramHeader.Add("idTransaccion", objBEAuditoriaRest.idTransaccion);
                paramHeader.Add("msgId", objBEAuditoriaRest.msgid);
                paramHeader.Add("userId", objBEAuditoriaRest.userId);
                paramHeader.Add("timestamp", objBEAuditoriaRest.timestamp);

                objLog.CrearArchivolog("--------- Parametros Entrada Inicio Rest : -----------", null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", " timestamp", objBEAuditoriaRest.timestamp), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", " userId", objBEAuditoriaRest.userId), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", " idTransaccion", objBEAuditoriaRest.idTransaccion), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", " ipApplication)", objBEAuditoriaRest.ipApplication), null, null);

                return RestServiceValidServExcluyentes.PostInvoque<ValidarServiciosExcluyentesResponse>("ConsURLValidarServExcluyentes", paramHeader, objValidarServExcluyentesRequest, objBEAuditoriaRest.userId, objBEAuditoriaRest.ipApplication, strUsuarioValidEncrip, strContraValidEncrip);
            }
            catch(Exception ex)
            {
                objLog.CrearArchivolog(String.Format("{0} : {1}", " Excepcion metodo ValidarServiciosExcluyentes", objBEAuditoriaRest.applicationCodeWS), null, ex); //INC-SMS_PORTA
                throw ex;
            }
        }

        public RegistrarServiciosExcluyentesResponse RegistrarServiciosExcluyentes(RegistrarServiciosExcluyentesRequest objValidarServExcluyentesRequest, BEAuditoriaRequest objBEAuditoriaRest, string strUsuarioInserEncrip, string strContraInserEncrip)
        {
            GeneradorLog objLog = null;
            try
            {
                objLog = new GeneradorLog(objBEAuditoriaRest.usuarioAplicacion, "Metodo ValidarServiciosExcluyentes", objBEAuditoriaRest.idTransaccion, "log_ValidarServExclu");

                Hashtable paramHeader = new Hashtable();
                paramHeader.Add("idTransaccion", objBEAuditoriaRest.idTransaccion);
                paramHeader.Add("msgId", objBEAuditoriaRest.msgid);
                paramHeader.Add("userId", objBEAuditoriaRest.userId);
                paramHeader.Add("timestamp", objBEAuditoriaRest.timestamp);

                objLog.CrearArchivolog("--------- Parametros Entrada Inicio Rest : -----------", null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", " timestamp", objBEAuditoriaRest.timestamp), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", " userId", objBEAuditoriaRest.userId), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", " idTransaccion", objBEAuditoriaRest.idTransaccion), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", " ipApplication)", objBEAuditoriaRest.ipApplication), null, null);

                return RestServiceValidServExcluyentes.PostInvoque<RegistrarServiciosExcluyentesResponse>("ConsUrl_InsertServExcluyentes", paramHeader, objValidarServExcluyentesRequest, objBEAuditoriaRest.userId, objBEAuditoriaRest.ipApplication, strUsuarioInserEncrip, strContraInserEncrip);
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog(String.Format("{0} : {1}", " Excepcion metodo ValidarServiciosExcluyentes", objBEAuditoriaRest.applicationCodeWS), null, ex); //INC-SMS_PORTA
                throw ex;
            }
        }

    }
}
