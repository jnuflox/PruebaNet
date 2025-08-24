using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using Claro.SISACT.Entity;
using Claro.SISACT.Common;
using Claro.SISACT.WS.RegistraPortaWS;
using System.Web.Services.Protocols;

namespace Claro.SISACT.WS
{
    public class BWRegistrarPorta
    {
        RegistraPortaWSService _objTransaccion = new RegistraPortaWSService();

        public BWRegistrarPorta(){
            _objTransaccion.Url = ConfigurationManager.AppSettings["WSRegistrarPortaWS_Url"].ToString();
            _objTransaccion.Credentials = System.Net.CredentialCache.DefaultCredentials;
            _objTransaccion.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["WSRegistrarPortaWS_timeout"].ToString());
        }


        public BEItemMensaje RealizarConsultaPrevia( BeConsultaPrevia objConsultaPrevia)
        {
            BEItemMensaje objMensaje = new BEItemMensaje(false);
            try
            {
                realizarConsultaPreviaResponse objResponse = new realizarConsultaPreviaResponse();
                realizarConsultaPreviaRequest objRequest = new realizarConsultaPreviaRequest();

                parametrosTypeObjetoOpcional[] listaRequestOpcional = new parametrosTypeObjetoOpcional[1];

                auditRequestType objRequestAudit = new auditRequestType();
                objRequestAudit.idTransaccion = objConsultaPrevia.auditoria.Codigo;
                objRequestAudit.ipAplicacion = objConsultaPrevia.auditoria.Descripcion2;
                objRequestAudit.nombreAplicacion = objConsultaPrevia.auditoria.Descripcion;
                objRequestAudit.usuarioAplicacion = objConsultaPrevia.auditoria.Codigo2;
                objRequest.auditRequest = objRequestAudit;

                objRequest.codigoCedente = objConsultaPrevia.codigoCedente;
                objRequest.modalidad = objConsultaPrevia.modalidad;
                objRequest.msisdn = objConsultaPrevia.msisdn;
                objRequest.tipoDocumento = objConsultaPrevia.tipoDocumento;
                objRequest.numeroDocumento = objConsultaPrevia.numeroDocumento;
                objRequest.observaciones = objConsultaPrevia.observaciones;
                objRequest.tipoPorta = objConsultaPrevia.tipoPorta;
                objRequest.modoEnvio = objConsultaPrevia.modoEnvio;
                objRequest.tipoServicio = objConsultaPrevia.tipoServicio;                

                objResponse = _objTransaccion.realizarConsultaPrevia(objRequest);
                
                objMensaje.codigo = objResponse.auditResponse.codigoRespuesta.ToString();
                objMensaje.descripcion = objResponse.auditResponse.mensajeRespuesta.ToString();
                objMensaje.cadenaValoresOut = Funciones.CheckStr(objResponse.identificadorProceso).Trim(); //Numero de secuencia
                if (objMensaje.codigo == "0") objMensaje.exito = true;

                /*
                 * 0 Operacion Exito
                 * 1 No se realizo la consulta previa
                 * 2 Se realizo rollback exitosamente
                 * -1 Disponibilidad de la bd
                 * -2 Timeout de la bd
                 * -3 Timeout del servicio
                 * -4 Disponibilidad del servicio
                */
            }
            catch (SoapException se)
            {
               objMensaje.codigo = "-4";
               objMensaje.mensajeCliente = "Se genero un error al ejecutar el servicio registrar consulta previa.";
               objMensaje.mensajeSistema = "SoapException -> Fault: " + Funciones.CheckStr(se.Code.Namespace) + " - " +
                                           "faultcode: " + Funciones.CheckStr(se.Code.Name) + " - " +
                                           "faultstring: " + Funciones.CheckStr(se.Message);
            }
            catch (Exception ex)
            {
                objMensaje.codigo = "-4";
                objMensaje.mensajeCliente= "Se genero un error al ejecutar el servicio registrar consulta previa.";
                objMensaje.mensajeSistema = ex.Message;
            }
            finally
            {
                _objTransaccion.Dispose();
            }
            return objMensaje;
        }


        public BEItemMensaje ActualizarConsultaPrevia(BeConsultaPrevia objConsultaPrevia)
        {
            BEItemMensaje objMensaje = new BEItemMensaje(false);
            try
            {
                actualizarSolicitudCPResponse objResponse = new actualizarSolicitudCPResponse();
                actualizarSolicitudCPRequest objRequest = new actualizarSolicitudCPRequest();

                parametrosTypeObjetoOpcional[] listaRequestOpcional = new parametrosTypeObjetoOpcional[1];

                auditRequestType objRequestAudit = new auditRequestType();
                objRequestAudit.idTransaccion = objConsultaPrevia.auditoria.Codigo;
                objRequestAudit.ipAplicacion = objConsultaPrevia.auditoria.Descripcion2;
                objRequestAudit.nombreAplicacion = objConsultaPrevia.auditoria.Descripcion;
                objRequestAudit.usuarioAplicacion = objConsultaPrevia.auditoria.Codigo2;
                objRequest.auditRequest = objRequestAudit;

                objRequest.solInCodigo = objConsultaPrevia.numeroSEC.ToString();
                objRequest.numeroSecuencial = objConsultaPrevia.numeroSecuencial;
                objRequest.observaciones = objConsultaPrevia.observaciones;
                objRequest.nombreRasoAbonado = objConsultaPrevia.nombreRSAbonado;

                objResponse = _objTransaccion.actualizarSolicitudCP(objRequest);


                objMensaje.codigo = objResponse.auditResponse.codigoRespuesta.ToString();
                objMensaje.descripcion = objResponse.auditResponse.mensajeRespuesta.ToString();
                if (objMensaje.codigo == "0") objMensaje.exito = true;
            }
            catch (Exception ex)
            {
                objMensaje.codigo = "-99";
                objMensaje.mensajeCliente = "Se genero un error al ejecutar el servicio de actualizacion de consulta previa.";
                objMensaje.mensajeSistema = ex.Message;
            }
            finally
            {
                _objTransaccion.Dispose();
            }
            return objMensaje;
        }
    }
}
