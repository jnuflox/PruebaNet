using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity.ValidarCampanaRest.Request;
using Claro.SISACT.Entity.ValidarCampanaRest.Response;
using Claro.SISACT.Entity;
using Claro.SISACT.Common;
using System.Collections;
using Claro.SISACT.WS.RestServices;
using System.Configuration;

namespace Claro.SISACT.WS.RestReferences
{
    public class RestValidarCampana
    {
        public ValidarCampanaDtpResponse validarCampanasActivas(ValidarCampanaDtpRequest request)
        {
            string strArchivo = "Log_validarCampanasActivas";
            string strIdTransaccion = Convert.ToString(DateTime.Now.Year + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second);
            string strUserId = Funciones.CheckStr(ConfigurationManager.AppSettings["strUserId"]);

            GeneradorLog _objLog = null;
            _objLog = new GeneradorLog(strArchivo, "Metodo validarCampanasActivas ", strIdTransaccion, null);
            try
            {
                Hashtable paramHeader = new Hashtable();
                paramHeader.Add("idTransaccion", strIdTransaccion);
                paramHeader.Add("msgid", strUserId);
                paramHeader.Add("aplicacion", strUserId);
                paramHeader.Add("userId", strUserId);
                paramHeader.Add("timestamp", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ"));

                _objLog.CrearArchivolog("---- Parametros de entrada Inicio : ----", null, null);
                _objLog.CrearArchivolog(" idTransaccion : " + strIdTransaccion, null, null);
                _objLog.CrearArchivolog(" msgid : " + strUserId, null, null);
                _objLog.CrearArchivolog(" aplicacion : " + strUserId, null, null);
                _objLog.CrearArchivolog(" userId : " + strUserId, null, null);
                _objLog.CrearArchivolog(" timestamp) : " + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ"), null, null);
                return RestServiceValidarCampana.PostInvoque<ValidarCampanaDtpResponse>("urlValidarCampana", paramHeader, request);
            }
            catch (Exception ex)
            {
                _objLog.CrearArchivolog("Excepcion metodo validarCampanasActivas : " + "--- Log_validarCampanasActivas ---", null, ex);
                throw;
            }

        }
    }
}
