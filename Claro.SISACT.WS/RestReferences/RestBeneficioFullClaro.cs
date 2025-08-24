using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Common;
using System.Configuration;
using Claro.SISACT.WS.RestServices;
using System.Collections;
using Claro.SISACT.Entity;
using Claro.SISACT.Entity.BeneficioFullClaro.validarAplicaDsctoCargoFijo.Response;
using Claro.SISACT.Entity.BeneficioFullClaro.registrarClienteFCDsctoCargoFijo.Response;

namespace Claro.SISACT.WS.RestReferences
{
    public class RestBeneficioFullClaro
    {
        public object ProcesarBeneficioFCResponse(string urlServicio, object objProcesarBeneficioFCRequest, BEAuditoriaRequest objBEAuditoriaREST, string op, string strUserEncrypted, string strPassEncrypted)
        {
            string strArchivo = Funciones.CheckStr(ConfigurationManager.AppSettings["strNombreLogSISACT"]);
            try
            {
                GeneradorLog.EscribirLog(strArchivo, String.Format("{0}", "[RestBeneficioFullClaro] [INICIO ProcesarBeneficioFCResponse]"));
                Hashtable paramHeader = new Hashtable();
                paramHeader.Add("timestamp", objBEAuditoriaREST.timestamp);
                paramHeader.Add("userId", objBEAuditoriaREST.userId);
                paramHeader.Add("idTransaccion", objBEAuditoriaREST.idTransaccion);
                paramHeader.Add("msgid", objBEAuditoriaREST.msgid);

                GeneradorLog.EscribirLog(strArchivo, String.Format("{0}", "[ProcesarBeneficioFCResponse]---- Parametros de entrada Inicio : ----"));
                foreach(DictionaryEntry ph in paramHeader) {
                    GeneradorLog.EscribirLog(strArchivo, String.Format("{0} --> {1}", Funciones.CheckStr(ph.Key), Funciones.CheckStr(ph.Value)));
                }

                switch (op)
                {
                    case "0":
                        return RestServiceBeneficioFullClaro.PostInvoque<ValidarAplicaDsctoCFResponse>(urlServicio, paramHeader, objProcesarBeneficioFCRequest, strUserEncrypted, strPassEncrypted);
                    default:
                        return RestServiceBeneficioFullClaro.PostInvoque<RegistrarClienteFCDsctCFResponse>(urlServicio, paramHeader, objProcesarBeneficioFCRequest, strUserEncrypted, strPassEncrypted);
                }
            }
            catch (Exception ex)
            {
                GeneradorLog.EscribirLog(strArchivo, String.Format("{0} : {1}", "[ProcesarBeneficioFCResponse] ERROR", objBEAuditoriaREST.applicationCodeWS));
                GeneradorLog.EscribirLog(strArchivo, String.Format("{0} : {1}", "[ProcesarBeneficioFCResponse] EXCEPCION", Funciones.CheckStr(ex)));
                throw ex;
            }
        }
    }
}
