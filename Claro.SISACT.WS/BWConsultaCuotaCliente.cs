//PROY-31948 INI
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using Claro.SISACT.Entity;
using Claro.SISACT.Common;
using Claro.SISACT.WS.WSOAC;
using Claro.SISACT.WS.WSConsultaCuotaCliente;


namespace Claro.SISACT.WS
{
    public class BWConsultaCuotaCliente
    {

        ConsultaCuotaCliente _objTransaccion = new ConsultaCuotaCliente();

        public BWConsultaCuotaCliente()
        {
            _objTransaccion.Url = Funciones.CheckStr(ConfigurationManager.AppSettings["WSConsultaCuotaCliente_URL"]);
            _objTransaccion.Credentials = System.Net.CredentialCache.DefaultCredentials;
            _objTransaccion.Timeout = Convert.ToInt32(Funciones.CheckStr(ConfigurationManager.AppSettings["TimeOut_ConsultaCuotaCliente"]));
        }

        public BECuota consultarCuotaCliente(string tipoDocumento, string nroDocumento, string lineaCliente, ref string strCodRespuestaOAC, ref string strMsjRespuestaOAC)
        {
            BECuota objCuota = new BECuota();

            ListaResponseOpcional[] lResponseOpcional = null;
            AuditTypeRequest objRequestAudit = new AuditTypeRequest();
            AuditTypeResponse objResponseAudit = new AuditTypeResponse();
            objRequestAudit.idTransaccion = Funciones.CheckStr(ConfigurationManager.AppSettings["CodigoAplicacion"]);
            objRequestAudit.ipAplicacion = Funciones.CheckStr(ConfigurationManager.AppSettings["constNombreAplicacion"]);
            objRequestAudit.aplicacion = DateTime.Now.ToString("hhmmssfff");
            objRequestAudit.usrAplicacion = string.Empty;

            try
            {
                string montoPendienteCuotasSistema = string.Empty;
                string cantidadPlanesCuotasPendientesSistema = string.Empty;
                string cantidadMaximaCuotasPendientesSistema = string.Empty;
                string cantidadCuotasPendientes = string.Empty;
                string montoPendienteCuotas = string.Empty;
               string estado = string.Empty, mensaje = string.Empty;

                objResponseAudit = _objTransaccion.consultarCuotaCliente(objRequestAudit,
                                                                    tipoDocumento,
                                                                    nroDocumento,
                                                                    lineaCliente,
                                                                    null,
                                                                    out montoPendienteCuotasSistema,
                                                                    out cantidadPlanesCuotasPendientesSistema,
                                                                    out cantidadMaximaCuotasPendientesSistema,
                                                                    out cantidadCuotasPendientes,
                                                                    out montoPendienteCuotas,
                                                                    out estado, out mensaje,
                                                                    out lResponseOpcional);


                objCuota.montoPendienteCuotasSistema = Funciones.CheckDbl(montoPendienteCuotasSistema);
                objCuota.cantidadPlanesCuotasPendientesSistema = Funciones.CheckInt(cantidadPlanesCuotasPendientesSistema);
                objCuota.cantidadMaximaCuotasPendientesSistema = Funciones.CheckInt(cantidadMaximaCuotasPendientesSistema);
                objCuota.cantidadCuotasPendientes = Funciones.CheckInt(cantidadCuotasPendientes);
                objCuota.montoPendienteCuotas = Funciones.CheckDbl(montoPendienteCuotas);
                strCodRespuestaOAC = objResponseAudit.codigoRespuesta;
                strMsjRespuestaOAC = objResponseAudit.mensajeRespuesta;

            }
            catch (Exception ex)
            {
                strCodRespuestaOAC = Funciones.CheckStr(ex.Source);
                strMsjRespuestaOAC = Funciones.CheckStr(ex.Message);
            }
            finally
            {
                _objTransaccion.Dispose();
            }
            return objCuota;
        }
    }
}
//PROY-31948 FIN
