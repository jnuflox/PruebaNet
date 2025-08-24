using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Claro.SISACT.Business;
using Claro.SISACT.Common;
using Claro.SISACT.Entity;
using Claro.SISACT.WS;
using System.Configuration;

namespace Claro.SISACT.Web.Comun
{   
//INI PROY-31948_Migracion
    public class sisact_SMS
    {
        GeneradorLog objLog = new GeneradorLog("****LOG_ENVIO_SMS****", null, null, "ENVIO_SMS_LOG");

        public void EnvioSMS(Int64 cod_solicitud, Int64 cod_consultor, Int64 cod_autorizador, string nro_documento, string razon_social,
            string apellido_paterno, string apellido_materno, string nombres, Int64 idEstadoSol, List<BEParametro> pParamMensajes)
        {            
            objLog.CrearArchivolog("INICIO METODO DE [EnvioSMS]", null, null);
           
            BEUsuario oUsuario = new BEUsuario();
            BWServicesNegocio oService = new BWServicesNegocio();              
            
            string strMensaje = string.Empty;
            string strNombreCompleto = string.Empty;

            string zip = Funciones.CheckStr(ConfigurationManager.AppSettings["constZIP"].ToString()); //51
            string strEmisorSMS = Funciones.CheckStr(ConfigurationManager.AppSettings["constSMSFrom"].ToString()); //SISACT
            string strCodigoAplicacionSMS = Funciones.CheckStr(ConfigurationManager.AppSettings["constSMSCodAplicacion"].ToString()); //PVU
            
            objLog.CrearArchivolog(string.Format("{0}{1}", "[EnvioSMS][cod_solicitud -> ]", cod_solicitud), null, null);
            objLog.CrearArchivolog(string.Format("{0}{1}", "[EnvioSMS][cod_consultor -> ]", cod_consultor), null, null);
            objLog.CrearArchivolog(string.Format("{0}{1}", "[EnvioSMS][cod_autorizador -> ]", cod_autorizador), null, null);
            objLog.CrearArchivolog(string.Format("{0}{1}", "[EnvioSMS][nro_documento -> ]", nro_documento), null, null);
            objLog.CrearArchivolog(string.Format("{0}{1}", "[EnvioSMS][razon_social -> ]", razon_social), null, null);
            objLog.CrearArchivolog(string.Format("{0}{1}", "[EnvioSMS][apellido_paterno -> ]", apellido_paterno), null, null);
            objLog.CrearArchivolog(string.Format("{0}{1}", "[EnvioSMS][apellido_materno -> ]", apellido_materno), null, null);
            objLog.CrearArchivolog(string.Format("{0}{1}", "[EnvioSMS][nombres -> ]", nombres), null, null);
            objLog.CrearArchivolog(string.Format("{0}{1}", "[EnvioSMS][idEstadoSol -> ]", idEstadoSol), null, null);
            objLog.CrearArchivolog(string.Format("{0}{1}", "[EnvioSMS][pParamMensajes -> ]", pParamMensajes.Count), null, null);
            objLog.CrearArchivolog(string.Format("{0}{1}", "[EnvioSMS][strEmisorSMS -> ]", strEmisorSMS), null, null);
            objLog.CrearArchivolog(string.Format("{0}{1}", "[EnvioSMS][strCodigoAplicacionSMS -> ]", strCodigoAplicacionSMS), null, null);

            try
            {
                if (cod_consultor > 0 && !string.IsNullOrEmpty(cod_consultor.ToString()))
                {
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[Se prepara envio de SMS a consultor][cod_consultor -> ]", cod_consultor), null, null);
                    oUsuario = BLMaestro.ObtenerUsuario(cod_consultor);

                    if (string.IsNullOrEmpty(razon_social))
                    {
                        strNombreCompleto = string.Format("{0}{1}{2}{3}{4}", nombres, string.Empty, apellido_paterno, " ", apellido_materno);
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[EnvioSMS][strNombreCompleto -> ]", strNombreCompleto), null, null);
                    }
                    else
                    {
                        strNombreCompleto = razon_social;
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[EnvioSMS][strNombreCompleto -> ]", strNombreCompleto), null, null);
                    }

                    if (oUsuario != null)
                    {
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[EnvioSMS][strTelefono -> ]", oUsuario.Telefono), null, null);
                        if (!string.IsNullOrEmpty(oUsuario.Telefono))
                        {
                            string strTelefono = oUsuario.Telefono;
                            if (strTelefono.Substring(0, 1) == "0") strTelefono.Substring(1);

                            strTelefono = zip + strTelefono;
                            objLog.CrearArchivolog(string.Format("{0}{1}", "[EnvioSMS][zip + strTelefono -> ]", strTelefono), null, null);
                            strMensaje = ExtraeMensaje(idEstadoSol, Funciones.CheckStr(cod_solicitud), pParamMensajes);
                            objLog.CrearArchivolog(string.Format("{0}{1}", "[EnvioSMS][strMensaje -> ]", strMensaje), null, null);

                            if (!string.IsNullOrEmpty(strMensaje))
                            {
                                objLog.CrearArchivolog(string.Format("{0}", "[EnvioSMS][Enviando SMS a consultor...]"), null, null);
                                strMensaje = string.Format("{0}{1}{2}{3}{4}", strMensaje, ". Cliente: ", nro_documento, " - ", strNombreCompleto);
                                objLog.CrearArchivolog(string.Format("{0}{1}", "[EnvioSMS][strMensaje Final ->]", strMensaje), null, null);

                                oService.EnvioSMS(strTelefono, strEmisorSMS, strMensaje, strCodigoAplicacionSMS);
                            }
                            else
                            {
                                objLog.CrearArchivolog(string.Format("{0}", "[EnvioSMS][No se envia el SMS - Mensaje vacio]"), null, null);
                            }
                        }
                        else
                        {
                            objLog.CrearArchivolog(string.Format("{0}", "[EnvioSMS][No se envia el SMS - Falta numero de telefono]"), null, null);
                        }
                    }
                    oUsuario = null;
                }
                if (cod_autorizador > 0 && !string.IsNullOrEmpty(cod_autorizador.ToString()))
                {                    
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[EnvioSMS][NSe prepara envio de sms a autorizador cod_autorizador]", cod_autorizador), null, null);
                    oUsuario = BLMaestro.ObtenerUsuario(cod_autorizador);

                    if (string.IsNullOrEmpty(razon_social))
                    {
                        strNombreCompleto = string.Format("{0}{1}{2}{3}{4}", nombres, " ", apellido_paterno, " ", apellido_materno);
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[EnvioSMS][strNombreCompleto -> ]", strNombreCompleto), null, null);
                    }
                    else
                    {
                        strNombreCompleto = razon_social;
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[EnvioSMS][strNombreCompleto -> ]", strNombreCompleto), null, null);
                    }

                    if (oUsuario != null)
                    {
                        if (!string.IsNullOrEmpty(oUsuario.Telefono))
                        {
                            string strTelefono = oUsuario.Telefono;
                            if (strTelefono.Substring(0, 1) == "0") strTelefono.Substring(1);
                            strTelefono = zip + strTelefono;
                            objLog.CrearArchivolog(string.Format("{0}{1}", "[EnvioSMS][zip + strTelefono -> ]", strTelefono), null, null);
                            strMensaje = ExtraeMensaje(idEstadoSol, Funciones.CheckStr(cod_solicitud), pParamMensajes);
                            objLog.CrearArchivolog(string.Format("{0}{1}", "[EnvioSMS][strMensaje -> ]", strMensaje), null, null);

                            if (!string.IsNullOrEmpty(strMensaje))
                            {
                                objLog.CrearArchivolog(string.Format("{0}", "[EnvioSMS][Enviando SMS a autorizador...]"), null, null);
                                strMensaje = string.Format("{0}{1}{2}{3}{4}", strMensaje, ". Cliente: ", nro_documento, " - ", strNombreCompleto);
                                objLog.CrearArchivolog(string.Format("{0}{1}", "[EnvioSMS][strMensaje Final ->]", strMensaje), null, null);

                                oService.EnvioSMS(strTelefono, strEmisorSMS, strMensaje, strCodigoAplicacionSMS);
                            }
                            else
                            {
                                objLog.CrearArchivolog(string.Format("{0}", "[EnvioSMS][No se envia el SMS - Mensaje vacio]"), null, null);
                            }
                        }
                        else
                        {
                            objLog.CrearArchivolog(string.Format("{0}", "[EnvioSMS][No se envia el SMS - Falta numero de telefono]"), null, null);
                        }
                    }
                }
            }
            catch(Exception e)
            {                
                objLog.CrearArchivolog(string.Format("{0}{1}", "[EnvioSMS - ERROR][Mensaje de error]",e.Message.ToString()), null, null);
            }
            objLog.CrearArchivolog("FIN METODO DE [EnvioSMS]", null, null);
        }

        private static string ExtraeMensaje(Int64 idValorMensaje, string cod_sol, List<BEParametro> pParamMensajes)
        {
            string cadena = string.Empty;

            foreach(BEParametro param in pParamMensajes)
            {
                if (Funciones.CheckInt(param.Valor) == idValorMensaje)
                {
                    cadena = param.Descripcion;
                    break;
                }
            }
            if (!string.IsNullOrEmpty(cadena))
            {
                cadena = cadena.Replace("##", cod_sol);
            }            
            return cadena;
        }


    }//FIN PROY-31948_Migracion
}