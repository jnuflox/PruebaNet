//PROY-32439 MAS INI CLASE NUEVA
using System;
using System.Configuration;
using Claro.SISACT.Entity;
using Claro.SISACT.Common;
using Claro.SISACT.WS.WSValidacionDeudaRules;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Web.Services.Protocols;
namespace Claro.SISACT.WS
{
    public class BWNuevaReglasCreditica
    {
        ValidacionDeudaRulesDecisionService _objTransaccion = new ValidacionDeudaRulesDecisionService();

        public BWNuevaReglasCreditica()
        {
            _objTransaccion.Url = ConfigurationManager.AppSettings["constURLNuevaReglasCrediticiaBRMS"].ToString();
            _objTransaccion.Credentials = System.Net.CredentialCache.DefaultCredentials;
            _objTransaccion.Timeout = Funciones.CheckInt(ConfigurationManager.AppSettings["TimeOut_NuevaReglasCrediticia"].ToString());
            _objTransaccion.RequestEncoding = Encoding.UTF8;
        }

        public ValidacionDeudaRulesResponse ConsultaReglaCrediticiaNVOBRMS(ValidacionDeudaBRMSrequest objBRMSrequest, out String strRespuestaCodigo, out String strRespuestaMensaje)
        {
            ValidacionDeudaRulesResponse objResponse = new ValidacionDeudaRulesResponse();
            strRespuestaCodigo = String.Empty;
            strRespuestaMensaje = String.Empty;
            GeneradorLog logError = null;
            
            try
            {
                #region ValidacionDeudaRules|Request

                #region ValidacionDeudaRules|RequestCliente
                bloqueo[] arrBloqueos = null;
                if (!Object.Equals(objBRMSrequest.cliente.bloqueos, null))
                {
                    bloqueo objBloqueo = new bloqueo();
                    List<bloqueo> lstBloqueos = null;
                    foreach (var item in objBRMSrequest.cliente.bloqueos)
                    {
                        if (Object.Equals(lstBloqueos, null))
                        {
                            lstBloqueos = new List<bloqueo>();
                        }
                        objBloqueo.tipo = item.tipo;
                        objBloqueo.tipoLinea = item.tipoLinea;
                        lstBloqueos.Add(objBloqueo);
                    }
                    arrBloqueos = lstBloqueos.ToArray();
                }

                disputa objDisputa = new disputa();
                objDisputa.antiguedad = objBRMSrequest.cliente.disputa.antiguedad;
                objDisputa.antiguedadSpecified = true;
                objDisputa.cantidad = objBRMSrequest.cliente.disputa.cantidad;
                objDisputa.cantidadSpecified = true;
                objDisputa.monto = objBRMSrequest.cliente.disputa.monto;
                objDisputa.montoSpecified = true;

                documento objDocumento = new documento();
                objDocumento.tipo = objBRMSrequest.cliente.documento.tipo;
                objDocumento.numero = objBRMSrequest.cliente.documento.numero;

                suspension[] arrSuspensiones = null;
                if (!Object.Equals(objBRMSrequest.cliente.suspensiones, null))
                {
                    suspension objSuspension = new suspension();
                    List<suspension> lstSuspensiones = null;
                    foreach (var item in objBRMSrequest.cliente.suspensiones)
                    {
                        if (Object.Equals(lstSuspensiones, null))
                        {
                            lstSuspensiones = new List<suspension>();
                        }
                        objSuspension.tipo = item.tipo;
                        objSuspension.tipoLinea = item.tipoLinea;
                        lstSuspensiones.Add(objSuspension);
                    }
                    arrSuspensiones = lstSuspensiones.ToArray();
                }

                string[] arrFraudes = null;
                if (!Object.Equals(objBRMSrequest.cliente.tiposFraude, null))
                {
                    arrFraudes = new String[objBRMSrequest.cliente.tiposFraude.Count - 1];
                    arrFraudes = objBRMSrequest.cliente.tiposFraude.ToArray();
                }

                cliente objCliente = new cliente();
                objCliente.antiguedadDeuda = objBRMSrequest.cliente.antiguedadDeuda;
                objCliente.antiguedadDeudaSpecified = true;
                objCliente.bloqueos = arrBloqueos;
                objCliente.cantidadDocumentosDeuda = objBRMSrequest.cliente.cantidadDocumentosDeuda;
                objCliente.cantidadDocumentosDeudaSpecified = true;
                objCliente.comportamientoPago = objBRMSrequest.cliente.comportamientoPago;
                objCliente.disputa = objDisputa;
                objCliente.documento = objDocumento;
                if (objBRMSrequest.cliente.flagBloqueos == ValidacionDeudaBRMSrequest.tipoSiNo.SI)
                {
                    objCliente.flagBloqueos = tipoSiNo.SI;
                }
                else
                {
                    objCliente.flagBloqueos = tipoSiNo.NO;
                }
                objCliente.flagBloqueosSpecified = true;
                if (objBRMSrequest.cliente.flagSuspensiones == ValidacionDeudaBRMSrequest.tipoSiNo.SI)
                {
                    objCliente.flagSuspensiones = tipoSiNo.SI;
                }
                else
                {
                    objCliente.flagSuspensiones = tipoSiNo.NO;
                }
                objCliente.flagSuspensionesSpecified = true;
                objCliente.montoDeuda = objBRMSrequest.cliente.montoDeuda;
                objCliente.montoDeudaSpecified = true;
                objCliente.montoDeudaCastigada = objBRMSrequest.cliente.montoDeudaCastigada;
                objCliente.montoDeudaCastigadaSpecified = true;
                objCliente.montoDeudaVencida = objBRMSrequest.cliente.montoDeudaVencida;
                objCliente.montoDeudaVencidaSpecified = true;
                objCliente.montoTotalPago = objBRMSrequest.cliente.montoTotalPago;
                objCliente.montoTotalPagoSpecified = true;
                objCliente.promedioFacturadoSoles = objBRMSrequest.cliente.promedioFacturadoSoles;
                objCliente.promedioFacturadoSolesSpecified = true;
                objCliente.segmento = objBRMSrequest.cliente.segmento;
                objCliente.suspensiones = arrSuspensiones;
                objCliente.tiempoPermanencia = objBRMSrequest.cliente.tiempoPermanencia;
                objCliente.tiempoPermanenciaSpecified = true;
                objCliente.tiposFraude = arrFraudes;
                #endregion

                #region ValidacionDeudaRules|RequestLineaARenovar
                lineaARenovar objLineaARenovar = null;
                if (!Object.Equals(objBRMSrequest.lineaARenovar, null))
                {
                    objLineaARenovar = new lineaARenovar();
                    objLineaARenovar.antiguedad = objBRMSrequest.lineaARenovar.antiguedad;
                    objLineaARenovar.antiguedadSpecified = true;
                    objLineaARenovar.fechaActivacion = objBRMSrequest.lineaARenovar.fechaActivacion;
                    objLineaARenovar.fechaActivacionSpecified = true;
                }
                #endregion

                #region ValidacionDeudaRulesRequest|PuntoDeVenta
                vendedor objVendedor = new vendedor();
                objVendedor.codigo = objBRMSrequest.puntoDeVenta.vendedor.codigo;
                objVendedor.nombre = objBRMSrequest.puntoDeVenta.vendedor.nombre;

                puntoDeVenta objPuntoDeVenta = new puntoDeVenta();
                objPuntoDeVenta.canal = objBRMSrequest.puntoDeVenta.canal;
                objPuntoDeVenta.codigo = objBRMSrequest.puntoDeVenta.codigo;
                objPuntoDeVenta.departamento = objBRMSrequest.puntoDeVenta.departamento;
                objPuntoDeVenta.nombre = objBRMSrequest.puntoDeVenta.nombre;
                objPuntoDeVenta.region = objBRMSrequest.puntoDeVenta.region;
                objPuntoDeVenta.segmento = objBRMSrequest.puntoDeVenta.segmento;
                objPuntoDeVenta.vendedor = objVendedor;
                #endregion

                solicitud1 objSolicitud1 = new solicitud1();
                objSolicitud1.cliente = objCliente;
                objSolicitud1.lineaARenovar = objLineaARenovar;
                objSolicitud1.puntoDeVenta = objPuntoDeVenta;
                objSolicitud1.sistemaEvaluacion = objBRMSrequest.sistemaEvaluacion;
                objSolicitud1.tipoOperacion = objBRMSrequest.tipoOperacion;

                solicitud objSolicitud = new solicitud();
                objSolicitud.solicitud1 = objSolicitud1;

                ValidacionDeudaRulesRequest objRequest = new ValidacionDeudaRulesRequest();
                objRequest.DecisionID = objBRMSrequest.decisionID;
                objRequest.solicitud = objSolicitud;
                #endregion

                #region RequestLog
                //DIL :: INI
                GeneradorLog log = null;
                log = new GeneradorLog(null, "BRMS", null, "BRMS");
                log.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][NUEVO-BRMS][REQUEST][INICIO]", ""), null);
                XmlSerializer xmlRequest = new XmlSerializer(typeof(ValidacionDeudaRulesRequest));
                StringBuilder builderRequest = new StringBuilder();
                TextWriter writerRequest = new StringWriter(builderRequest);
                xmlRequest.Serialize(writerRequest, objRequest);
                writerRequest.Close();
                log.CrearArchivolog(null, builderRequest.ToString(), null);
                log.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][NUEVO-BRMS][REQUEST][FIN]", ""), null);
                //DIL :: FIN
                #endregion

                #region ValidacionDeudaRules|Response
                objResponse = _objTransaccion.ValidacionDeudaRules(objRequest);
                if (!Object.Equals(objResponse, null))
                {
                    strRespuestaCodigo = "1";
                    strRespuestaMensaje = "ValidacionDeudaRules ejecutado exitosamente";
                }
                #endregion

                #region ResponseLog
                //DIL :: INI
                log.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][NUEVO-BRMS][RESPONSE][INICIO]", ""), null);
                XmlSerializer xmlResponse = new XmlSerializer(typeof(ValidacionDeudaRulesResponse));
                StringBuilder builderResponse = new StringBuilder();
                TextWriter writerResponse = new StringWriter(builderResponse);
                xmlResponse.Serialize(writerResponse, objResponse);
                writerRequest.Close();
                log.CrearArchivolog(null, builderResponse.ToString(), null);
                log.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][NUEVO-BRMS][RESPONSE][FIN]", ""), null);
                //DIL :: FIN
                #endregion
            }
            catch(SoapException se)
            {
                //log
                
                logError = new GeneradorLog(null, "ErrorWS", null, "ErrorWS");
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaReglaCrediticiaNVOBRMS][WS | Error :: INI]", ""),null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaReglaCrediticiaNVOBRMS][WS | Error Actor]", se.Actor),null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaReglaCrediticiaNVOBRMS][WS | Error Code]", se.Code),null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaReglaCrediticiaNVOBRMS][WS | Error Data]", se.Data),null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaReglaCrediticiaNVOBRMS][WS | Error Detail]", se.Detail),null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaReglaCrediticiaNVOBRMS][WS | Error HelpLink]", se.HelpLink),null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaReglaCrediticiaNVOBRMS][WS | Error InnerException]", se.InnerException),null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaReglaCrediticiaNVOBRMS][WS | Error Lang]", se.Lang),null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaReglaCrediticiaNVOBRMS][WS | Error Message]", se.Message),null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaReglaCrediticiaNVOBRMS][WS | Error Node]", se.Node),null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaReglaCrediticiaNVOBRMS][WS | Error Role]", se.Role),null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaReglaCrediticiaNVOBRMS][WS | Error Source]", se.Source),null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaReglaCrediticiaNVOBRMS][WS | Error StackTrace]", se.StackTrace),null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaReglaCrediticiaNVOBRMS][WS | Error SubCode]", se.SubCode),null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaReglaCrediticiaNVOBRMS][WS | Error TargetSite]", se.TargetSite),null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaReglaCrediticiaNVOBRMS][WS | Error :: FIN]", ""), null);
            }
            catch (Exception e)
            {
                objResponse = null;
                strRespuestaCodigo = "-1";
                strRespuestaMensaje = "ErrorWS_ValidacionDeudaRules[" + e.Message + "]";
                logError = new GeneradorLog(null, "ErrorWS", null, "ErrorWS");
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaReglaCrediticiaNVOBRMS][APP | Error :: INI]", ""), null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaReglaCrediticiaNVOBRMS][APP | Error Data]", e.Data), null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaReglaCrediticiaNVOBRMS][APP | Error HelpLink]", e.HelpLink), null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaReglaCrediticiaNVOBRMS][APP | Error InnerException]", e.InnerException), null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaReglaCrediticiaNVOBRMS][APP | Error Message]", e.Message), null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaReglaCrediticiaNVOBRMS][APP | Error Source]", e.Source), null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaReglaCrediticiaNVOBRMS][APP | Error StackTrace]", e.StackTrace), null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaReglaCrediticiaNVOBRMS][APP | Error StackTrace]", e.TargetSite), null);
                logError.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ConsultaReglaCrediticiaNVOBRMS][APP | Error :: FIN]", ""), null);

            }
            finally
            {
                _objTransaccion.Dispose();
            }

            return objResponse;
        }
    }
}
//PROY-32439 MAS FIN CLASE NUEVA