using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.WS.FlexibilizaDeudaWS;
using System.Configuration;
using Claro.SISACT.Entity;
using Claro.SISACT.Common;

namespace Claro.SISACT.WS
{
    public class BWFlexibilizacionDeudaWS
    {
        FlexibilizaDeudaWS.BSS_FlexibilizaDeudaSOAP11BindingQSService _objTransaccion = new FlexibilizaDeudaWS.BSS_FlexibilizaDeudaSOAP11BindingQSService();
        
        GeneradorLog _objLog = null;
        string _canal = null;
        string _idAplicacion = null;
        string _usuAplicacion = null;
        string _usuarioSesion = null;
        string _idTransaccionESB = null;
        string _idTransaccion = null;
        string _fechaInicio = null;
        string _nodoAdicional = null;


        public BWFlexibilizacionDeudaWS()
        {
            _objTransaccion.Url = ConfigurationManager.AppSettings["urlWS_FlexibilizacionDeuda"].ToString();
            _objTransaccion.Credentials = System.Net.CredentialCache.DefaultCredentials;
            _objTransaccion.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["TimeOut_FlexibilizacionDeuda"].ToString());

            String nombreServer = System.Net.Dns.GetHostName();

            _canal = ConfigurationManager.AppSettings["constNombreAplicacion"].ToString();
            _idAplicacion = ConfigurationManager.AppSettings["CodigoAplicacion"].ToString();
            _usuAplicacion = ConfigurationManager.AppSettings["constNombreAplicacion"].ToString();
            _idTransaccionESB = DateTime.Now.ToString("hhmmssfff");
            _idTransaccion = DateTime.Now.ToString("hhmmssfff");
            _fechaInicio = DateTime.Now.ToString("yyyy/MM/dd");
                
        } 

        public BEItemMensaje ConsultaDeuda(BEClienteCuenta objRequest, int consAntiguedadDeuda, BEUsuarioSession objUsuario)
        {
            _objLog = new GeneradorLog(null, objRequest.nroDoc, _idTransaccion, "BWFlexibilizacionDeudaWS.cs");
            BEItemMensaje objMensaje = new BEItemMensaje(false);
            try

            {
                _objLog.CrearArchivologWS("INICIO WS Flexibilizacion", _objTransaccion.Url, null, null);
                FlexibilizaDeudaWS.ConsultaDeudaRequestType flexibilizacionRequest = new ConsultaDeudaRequestType();
                FlexibilizaDeudaWS.ConsultaDeudaResponseType flexibilizacionResponse = new ConsultaDeudaResponseType();

                //Auditoria-INI
                FlexibilizaDeudaWS.HeaderRequestType HeaderRequest = new HeaderRequestType();

                HeaderRequest.canal = _canal;
                HeaderRequest.idAplicacion = Funciones.CheckInt(_idAplicacion);
                HeaderRequest.usuarioAplicacion = objUsuario.idCuentaRed;
                HeaderRequest.usuarioSesion = objUsuario.idCuentaRed;
                HeaderRequest.idTransaccionESB = _idTransaccion;
                HeaderRequest.idTransaccionNegocio = _idTransaccion;
                HeaderRequest.fechaInicio = Funciones.CheckDate(_fechaInicio);
                HeaderRequest.nodoAdicional = objUsuario.Terminal;

                _objLog.CrearArchivologWS("AUDITORIA", null, "Parametros canal:" + HeaderRequest.canal + ", idAplicacion:" + HeaderRequest.idAplicacion
                    + ",  usuarioAplicacion:" + HeaderRequest.usuarioAplicacion + ",  usuarioSesion:" + HeaderRequest.usuarioSesion
                    + ",  idTransaccionESB:" + HeaderRequest.idTransaccionESB + ",  idTransaccionNegocio:" + HeaderRequest.idTransaccionNegocio
                    + ",  fechaInicio:" + HeaderRequest.fechaInicio + ",  nodoAdicional:" + HeaderRequest.nodoAdicional, null);
                            
                _objTransaccion.headerRequest = HeaderRequest;
                //Auditoria-FIN
                flexibilizacionRequest.numDocumento = objRequest.nroDoc;
                flexibilizacionRequest.tipoDocumento = objRequest.tipoDoc.Replace("0", "");
                flexibilizacionRequest.tipoOper = "";
                flexibilizacionRequest.cantAnios = consAntiguedadDeuda.ToString();
                _objLog.CrearArchivologWS("INPUT", null, "Parametros Flexibilizacion- Numero Documento:" + flexibilizacionRequest.numDocumento + ", Tipo Documento:" + flexibilizacionRequest.tipoDocumento + ", Tipo Operacion:" + flexibilizacionRequest.tipoOper
                    + ", fecha: " + flexibilizacionRequest.fecActivacion + ", Cantidad Años:" + flexibilizacionRequest.cantAnios.ToString(), null);
                flexibilizacionResponse = _objTransaccion.consultaDeuda(flexibilizacionRequest);

                objMensaje.codigo = flexibilizacionResponse.responseStatus.estado;
                objMensaje.descripcion = flexibilizacionResponse.responseStatus.descripcionRespuesta;
                objMensaje.cadenaValoresOut = flexibilizacionResponse.responseDataConsultaDeuda.resultado;

                if (objMensaje.codigo == "0") objMensaje.exito = true;
                _objLog.CrearArchivologWS("OUTPUT", null, "Resultado Flexibilizacion: " + objMensaje.cadenaValoresOut + " descripcionRespuesta: " + objMensaje.descripcion, null);
                _objLog.CrearArchivologWS(null, null, "FIN WS Flexibilizacion", null);
                
            }
            catch (Exception ex)
            {
                objMensaje.codigo = "-99";
                objMensaje.mensajeCliente= "Se genero un error al ejecutar el servicio de FlexibilizacionDeuda.";
                objMensaje.mensajeSistema = ex.Message;
                objMensaje.exito = false;
                _objLog.CrearArchivologWS("ERROR WS Flexibilizacion", _objTransaccion.Url, "Error en: " + ex.StackTrace, ex);
            }
            return objMensaje;
        }
    }
}
