using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Collections;
using Claro.SISACT.Common;
using System.Configuration;

namespace Claro.SISACT.WS.RestReferences
{
    public class GetHeadersDP
    {
        public static WebHeaderCollection GetHeaders(Hashtable table, string strusuario, string stripServidor, string strnombreProy, string strWSIP, string strUserEncrypted, string strPassEncrypted)
        {
            GeneradorLog _objLog = null;
            _objLog = new GeneradorLog("Metodo GetHeaders", null, null, strnombreProy);

            WebHeaderCollection Headers = new WebHeaderCollection();
            string codigoResultado = string.Empty;
            string mensajeResultado = string.Empty;
            string usuarioAplicacion = string.Empty;
            string clave = string.Empty;

            foreach (DictionaryEntry entry in table)
            {
                Headers.Add(entry.Key.ToString(), entry.Value != null ? entry.Value.ToString() : null);
                _objLog.CrearArchivolog(String.Format("{0}  --> {1} = {2}","Headers",entry.Key.ToString(),entry.Value.ToString()), null, null);
            }

            _objLog.CrearArchivolog("INICIO SERVICIO DESENCRIPTACION  --> BWConsultaClaves.ConsultaClaveWS", null, null);

            string ipTransaccion = DateTime.Now.ToString("YYYYMMDDHHMISSMS");
            string wsIp = string.IsNullOrEmpty(strWSIP) ? stripServidor : strWSIP;
            string usrAplicacion = strusuario;
            string codigoAplicacion = ConfigurationManager.AppSettings["CodigoAplicacion"];
            string idAplicacion = ConfigurationManager.AppSettings["system_ConsultaClave"];
            string usuarioAplicacionEncriptado = strUserEncrypted;
            string claveEncriptada = strPassEncrypted;

            _objLog.CrearArchivolog(String.Format("{0}  --> {1}","wsIp",wsIp), null, null);
            _objLog.CrearArchivolog(String.Format("{0}  --> {1}","ipServidor",stripServidor), null, null);
            _objLog.CrearArchivolog(String.Format("{0}  --> {1}","usrAplicacion",usrAplicacion), null, null);
            _objLog.CrearArchivolog(String.Format("{0}  --> {1}","codigoAplicacion",codigoAplicacion), null, null);
            _objLog.CrearArchivolog(String.Format("{0}  --> {1}","idAplicacion",idAplicacion), null, null);
            _objLog.CrearArchivolog(String.Format("{0}  --> {1}","ipTransaccion",ipTransaccion), null, null);
            _objLog.CrearArchivolog(String.Format("{0}  --> {1}","usuarioAplicacionEncriptado",usuarioAplicacionEncriptado), null, null);
            _objLog.CrearArchivolog(String.Format("{0}  --> {1}", "claveEncriptada", claveEncriptada), null, null);

            codigoResultado = new BWConsultaClaves().ConsultaClavesWS(ipTransaccion,
                                                                 stripServidor,
                                                                 ipTransaccion,
                                                                 usrAplicacion,
                                                                 codigoAplicacion,
                                                                 idAplicacion,
                                                                 usuarioAplicacionEncriptado,
                                                                 claveEncriptada,
                                                                 out mensajeResultado,
                                                                 out usuarioAplicacion,
                                                                 out clave
                                                                 );

            _objLog.CrearArchivolog(String.Format("{0}  --> {1}","Codigo Resultado",codigoResultado), null, null);
            _objLog.CrearArchivolog(String.Format("{0}  --> {1}","Mensaje Resultado ConsultaClaves",mensajeResultado), null, null);
            _objLog.CrearArchivolog(String.Format("{0}  --> {1}","Usuario",usuarioAplicacion), null, null);
            _objLog.CrearArchivolog(String.Format("{0}  --> {1}", "Clave", clave), null, null);

            string strEncryptedBase64;

            if (codigoResultado == "0")
            {

                strEncryptedBase64 = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", usuarioAplicacion, clave)));

                if (strEncryptedBase64 != "1" && strEncryptedBase64 != "-1" && strEncryptedBase64 != "-2" && strEncryptedBase64 != "-3")
                {
                    Headers.Add("Authorization", String.Format("{0} {1}","Basic",strEncryptedBase64));
                }
            }

            return Headers;
        }
    }
}
