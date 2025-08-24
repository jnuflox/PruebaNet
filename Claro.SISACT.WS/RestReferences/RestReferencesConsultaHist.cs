//PROY-140546
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest.DataPower.ConsultaHistorico.Request;
using Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest.DataPower.ConsultaHistorico.Response;
using System.Configuration;
using Claro.SISACT.Common;
using System.Collections;
using Claro.SISACT.WS.RestServices;

namespace Claro.SISACT.WS.RestReferences
{
    public class RestReferencesConsultaHist
    {
        public bool consultarHistorico(ConsultaHistGenericRequest pRequest, string host, ref ConsultaHistGenericResponse outResponse)
        {
            string strArchivo = "Log_consultarHistorico";
            string timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");
            string userId = ConfigurationManager.AppSettings["system_ConsultaClave"];
            string idTransaccion = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string msgid = DateTime.Now.ToString("yyyyMMddHHmmss");
            string aplicacion = ConfigurationManager.AppSettings["constUsuarioAplicacion"];

            GeneradorLog _objLog = null;
            _objLog = new GeneradorLog(strArchivo, "Metodo consultarHistorico ", idTransaccion, null);
            _objLog.CrearArchivolog("PROY-140546|RestReferencesConsultaHist|consultarHistorico|-- INICIO --", null, null);

            try
            {
                Hashtable paramHeader = new Hashtable();
                paramHeader.Add("timestamp", timestamp);
                paramHeader.Add("userId", userId);
                paramHeader.Add("idTransaccion", idTransaccion);
                paramHeader.Add("msgid", msgid);
                paramHeader.Add("aplicacion", aplicacion);

                _objLog.CrearArchivolog(" timestamp : " + timestamp, null, null);
                _objLog.CrearArchivolog(" userId : " + userId, null, null);
                _objLog.CrearArchivolog(" idTransaccion : " + idTransaccion, null, null);
                _objLog.CrearArchivolog(" msgid : " + msgid, null, null);
                _objLog.CrearArchivolog(" aplicacion : " + aplicacion, null, null);

                _objLog.CrearArchivolog("PROY-140546|RestReferencesConsultaHist|consultarHistorico|-- FIN --", null, null);

                outResponse = RestServiceConsultaHist.PostInvoque<ConsultaHistGenericResponse>("constUrlConsultaHistorico", paramHeader, pRequest, host);
                return true;
            }
            catch (Exception ex)
            {
                _objLog.CrearArchivolog(String.Format("{0} : {1} | {2}", "Excepcion metodo consultarHistorico", Funciones.CheckStr(ex.Message), Funciones.CheckStr(ex.StackTrace)), null, null);
                return false;
            }
        }
    }
}
