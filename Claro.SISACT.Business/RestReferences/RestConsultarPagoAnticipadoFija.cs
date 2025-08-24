//PROY-140546
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest;
using Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest.DataPower;
using Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest.DataPower.RegistroPA;
using Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest.DataPower.ActualizaPA;
using Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest.DataPower.RegistraHistorial;
using Claro.SISACT.Business.RestServices;
using System.Configuration;
using System.Collections;
using Claro.SISACT.Entity;
//using System.Collections.Generic;
using Claro.SISACT.Common;
using Claro.SISACT.Entity.DataPowerRest;

namespace Claro.SISACT.Business.RestReferences
{
    public class RestConsultarPagoAnticipadoFija
    {
        public List<PagoAnticipado> ConsultarPagosAnticipados(ConsultaPAGenericRequest oRequest)
        {
            GeneradorLog objLog = null;
            List<PagoAnticipado> oListaPagos = new List<PagoAnticipado>();
            try
            {
                objLog = new GeneradorLog(ReadKeySettings.ConsCurrentUser, "Metodo ConsultarPagosAnticipados", "", "log_PagoAnticipadoFija");
                String nameURL = "cons_UrlCobroAnticipadoConsultaRes";
                
                Hashtable paramHeader = new Hashtable();
                paramHeader.Add("idTransaccion", DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                paramHeader.Add("msgid", "");
                paramHeader.Add("timestamp", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
                paramHeader.Add("userId", ConfigurationManager.AppSettings["system_ConsultaClave"]);
                paramHeader.Add("ipServidor", ReadKeySettings.ConsCurrentIP);//Para ConsultaClave
                paramHeader.Add("usuario", ConfigurationManager.AppSettings["system_ConsultaClave"]);//Para ConsultaClave
                paramHeader.Add("aplicacion", ConfigurationManager.AppSettings["constUsuarioAplicacion"]);

                CredencialesDPRest oCredenciales = new CredencialesDPRest();
                oCredenciales.UsuarioEncriptado = ConfigurationManager.AppSettings["cons_CobroAnticipadoUser"];
                oCredenciales.ClaveEncriptada = ConfigurationManager.AppSettings["cons_CobroAnticipadoPass"];

                ConsultaPAGenericResponse oResponse = RestService.PostInvoque<ConsultaPAGenericResponse>(nameURL, paramHeader, oRequest, oCredenciales);
                
                objLog.CrearArchivolog(String.Format("{0} : {1}", " codigoRespuesta", oResponse.MessageResponse.Body.consultaPAResponseType.responseStatus.codigoRespuesta), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", " mensajeRespuesta", oResponse.MessageResponse.Body.consultaPAResponseType.responseStatus.mensajeRespuesta), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", " idTransaccion", oResponse.MessageResponse.Body.consultaPAResponseType.responseStatus.idTransaccion), null, null);

                oListaPagos = oResponse.MessageResponse.Body.consultaPAResponseType.responseData.pagoAnticipado;
            }
            catch (Exception ex)
            {
                oListaPagos = new List<PagoAnticipado>();
                objLog.CrearArchivolog(String.Format("{0} : {1}", " Excepcion metodo ConsultarPagosAnticipados", ""), null, ex); //INC-SMS_PORTA
            }
            
            return oListaPagos;
        }

        public bool RegistrarPagoAnticipado(RegistroPAGenericRequest oRequest)
        {
            bool bRespuesta = false;
            GeneradorLog objLog = null;
            try
            {
                objLog = new GeneradorLog(ReadKeySettings.ConsCurrentUser, "Metodo RegistrarPagoAnticipado", "", "log_PagoAnticipadoFija");
           
                String nameURL = "cons_UrlCobroAnticipadoRegistroRes";
                
                Hashtable paramHeader = new Hashtable();
                paramHeader.Add("idTransaccion", DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                paramHeader.Add("msgid", "");
                paramHeader.Add("timestamp", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
                paramHeader.Add("userId", ConfigurationManager.AppSettings["system_ConsultaClave"]);
                paramHeader.Add("ipServidor", ReadKeySettings.ConsCurrentIP);//Para ConsultaClave
                paramHeader.Add("usuario", ConfigurationManager.AppSettings["system_ConsultaClave"]);//Para ConsultaClave
                paramHeader.Add("aplicacion", ConfigurationManager.AppSettings["constUsuarioAplicacion"]);
                
                CredencialesDPRest oCredenciales = new CredencialesDPRest();
                oCredenciales.UsuarioEncriptado = ConfigurationManager.AppSettings["cons_CobroAnticipadoUser"];
                oCredenciales.ClaveEncriptada = ConfigurationManager.AppSettings["cons_CobroAnticipadoPass"];

                objLog.CrearArchivolog("--------- Parametros Entrada Inicio Rest (RegistrarPagoAnticipado) : -----------", null, null);

                RegistroPAGenericResponse oResponse = RestService.PostInvoque<RegistroPAGenericResponse>(nameURL, paramHeader, oRequest, oCredenciales);

                objLog.CrearArchivolog(String.Format("{0} : {1}", " codigoRespuesta", oResponse.MessageResponse.Body.registroPAResponseType.responseStatus.codigoRespuesta), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", " mensajeRespuesta", oResponse.MessageResponse.Body.registroPAResponseType.responseStatus.mensajeRespuesta), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", " idTransaccion", oResponse.MessageResponse.Body.registroPAResponseType.responseStatus.idTransaccion), null, null);

                objLog.CrearArchivolog(String.Format("{0} : {1}", " Consumer", oResponse.MessageResponse.Header.HeaderResponse.Consumer), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", " Pid", oResponse.MessageResponse.Header.HeaderResponse.Pid), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", " Status", oResponse.MessageResponse.Header.HeaderResponse.Status), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", " Timestamp", oResponse.MessageResponse.Header.HeaderResponse.Timestamp), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", " VarArg", oResponse.MessageResponse.Header.HeaderResponse.VarArg), null, null);

                objLog.CrearArchivolog("--------- Parametros Salida Fin Rest : -----------", null, null);
                
                if (oResponse.MessageResponse.Body.registroPAResponseType.responseStatus.codigoRespuesta == "0")
                {
                    bRespuesta = true;
                }
                else
	            {
                    bRespuesta = false;
	            }
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog(String.Format("{0} : {1}", " Excepcion metodo RegistrarPagoAnticipado", ""), null, ex); //INC-SMS_PORTA
                //throw;
            }

            return bRespuesta;
        }

        public bool ActualizarPagoAnticipado(ActualizaPAGenericRequest oRequest)
        {
            bool bRespuesta = false;
            GeneradorLog objLog = null;
            try
            {
                objLog = new GeneradorLog(ReadKeySettings.ConsCurrentUser, "Metodo ActualizarPagoAnticipado", "", "log_PagoAnticipadoFija");
                
                String nameURL = "cons_UrlCobroAnticipadoActualizaRes";

                Hashtable paramHeader = new Hashtable();
                paramHeader.Add("idTransaccion", DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                paramHeader.Add("msgid", "");
                paramHeader.Add("timestamp", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
                paramHeader.Add("userId", ConfigurationManager.AppSettings["system_ConsultaClave"]);
                paramHeader.Add("ipServidor", ReadKeySettings.ConsCurrentIP);//Para ConsultaClave
                paramHeader.Add("usuario", ConfigurationManager.AppSettings["system_ConsultaClave"]);//Para ConsultaClave
                paramHeader.Add("aplicacion", ConfigurationManager.AppSettings["constUsuarioAplicacion"]);

                CredencialesDPRest oCredenciales = new CredencialesDPRest();
                oCredenciales.UsuarioEncriptado = ConfigurationManager.AppSettings["cons_CobroAnticipadoUser"];
                oCredenciales.ClaveEncriptada = ConfigurationManager.AppSettings["cons_CobroAnticipadoPass"];

                objLog.CrearArchivolog("--------- Parametros Entrada Inicio Rest (ActualizarPagoAnticipado): -----------", null, null);

                ActualizaPAGenericResponse oResponse = RestService.PostInvoque<ActualizaPAGenericResponse>(nameURL, paramHeader, oRequest, oCredenciales);

                objLog.CrearArchivolog(String.Format("{0} : {1}", " codigoRespuesta", oResponse.MessageResponse.Body.actualizaPAResponseType.responseStatus.codigoRespuesta), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", " mensajeRespuesta", oResponse.MessageResponse.Body.actualizaPAResponseType.responseStatus.mensajeRespuesta), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", " idTransaccion", oResponse.MessageResponse.Body.actualizaPAResponseType.responseStatus.idTransaccion), null, null);

                objLog.CrearArchivolog(String.Format("{0} : {1}", " Consumer", oResponse.MessageResponse.Header.HeaderResponse.Consumer), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", " Pid", oResponse.MessageResponse.Header.HeaderResponse.Pid), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", " Status", oResponse.MessageResponse.Header.HeaderResponse.Status), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", " Timestamp", oResponse.MessageResponse.Header.HeaderResponse.Timestamp), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", " VarArg", oResponse.MessageResponse.Header.HeaderResponse.VarArg), null, null);

                if (oResponse.MessageResponse.Body.actualizaPAResponseType.responseStatus.codigoRespuesta == "0")
                {
                    bRespuesta = true;
                }
                else
                {
                    bRespuesta = false;
                }
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog(String.Format("{0} : {1}", " Excepcion metodo ActualizarPagoAnticipado", ""), null, ex); //INC-SMS_PORTA
            }

            return bRespuesta;
        }

        public bool RegistrarHistorialPagoAnticipado(RegistraHistorialGenericRequest oRequest)
        {
            bool bRespuesta = false;
            GeneradorLog objLog = null;
            try
            {
                objLog = new GeneradorLog(ReadKeySettings.ConsCurrentUser, "Metodo RegistrarHistorialPagoAnticipado", "", "log_PagoAnticipadoFija");

                String nameURL = "cons_UrlCobroAnticipadoRegistraHistorialRes";

                Hashtable paramHeader = new Hashtable();
                paramHeader.Add("idTransaccion", DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                paramHeader.Add("msgid", "");
                paramHeader.Add("timestamp", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
                paramHeader.Add("userId", ConfigurationManager.AppSettings["system_ConsultaClave"]);
                paramHeader.Add("ipServidor", ReadKeySettings.ConsCurrentIP);//Para ConsultaClave
                paramHeader.Add("usuario", ConfigurationManager.AppSettings["system_ConsultaClave"]);//Para ConsultaClave
                paramHeader.Add("aplicacion", ConfigurationManager.AppSettings["constUsuarioAplicacion"]);

                CredencialesDPRest oCredenciales = new CredencialesDPRest();
                oCredenciales.UsuarioEncriptado = ConfigurationManager.AppSettings["cons_CobroAnticipadoUser"];
                oCredenciales.ClaveEncriptada = ConfigurationManager.AppSettings["cons_CobroAnticipadoPass"];

                objLog.CrearArchivolog("--------- Parametros Entrada Inicio Rest (RegistrarHistorialPagoAnticipado) : -----------", null, null);

                RegistraHistorialGenericResponse oResponse = RestService.PostInvoque<RegistraHistorialGenericResponse>(nameURL, paramHeader, oRequest, oCredenciales);

                objLog.CrearArchivolog(String.Format("{0} : {1}", " codigoRespuesta", oResponse.MessageResponse.Body.registraHistorialResponseType.responseStatus.codigoRespuesta), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", " mensajeRespuesta", oResponse.MessageResponse.Body.registraHistorialResponseType.responseStatus.mensajeRespuesta), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", " idTransaccion", oResponse.MessageResponse.Body.registraHistorialResponseType.responseStatus.idTransaccion), null, null);

                objLog.CrearArchivolog(String.Format("{0} : {1}", " Consumer", oResponse.MessageResponse.Header.HeaderResponse.Consumer), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", " Pid", oResponse.MessageResponse.Header.HeaderResponse.Pid), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", " Status", oResponse.MessageResponse.Header.HeaderResponse.Status), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", " Timestamp", oResponse.MessageResponse.Header.HeaderResponse.Timestamp), null, null);
                objLog.CrearArchivolog(String.Format("{0} : {1}", " VarArg", oResponse.MessageResponse.Header.HeaderResponse.VarArg), null, null);

                objLog.CrearArchivolog("--------- Parametros Salida Fin Rest : -----------", null, null);

                if (oResponse.MessageResponse.Body.registraHistorialResponseType.responseStatus.codigoRespuesta == "0")
                {
                    bRespuesta = true;
                }
                else
                {
                    bRespuesta = false;
                }
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog(String.Format("{0} : {1}", " Excepcion metodo RegistrarHistorialPagoAnticipado", ""), null, ex); //INC-SMS_PORTA
                //throw;
            }

            return bRespuesta;
        }
    }
}
