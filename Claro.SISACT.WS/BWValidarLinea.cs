//DIL::INI::20170910
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Claro.SISACT.Entity;
using Claro.SISACT.WS.ValidarLineasClienteWS;
using Claro.SISACT.Common;
using System.Net;
using Claro.SISACT.Entity.claro_vent_ventascontingencia;//PROY-140715

namespace Claro.SISACT.WS
{
    public class BWValidarLinea
    {
        public Boolean contarLineas(BEValidarLineaAuditoria objAuditoria, String strDocumentoNumero, String strTipoDocumento, out Int32 intRespuestaCodigo, out String strRespuestaMensaje, out BEValidarLinea objClienteLineas)//[PROY-140600] 
        {
            string nameArchivo = "contarLineas";//PROY-140715
            HelperLog.EscribirLog("", nameArchivo, "" + "[ -- contarLineas -- ]" + " - CONSTRUYENDO REQUEST  -", false);//PROY-140715
            var blnRespuesta = false;
            intRespuestaCodigo = -1000;
            strRespuestaMensaje = String.Empty;
            objClienteLineas = null;

            //PROY-140715 INI
            BWConsultarVentasContigencia objConsultarContigencia = new BWConsultarVentasContigencia();
            bool valido = true;
            //PROY-140715 FIN

            try
            {
                //REQUEST::INI
                var wsValidarLineas = new ValidarLineasClienteWS.ebsValidarLineasClienteService()
                {
                    Url = ConfigurationManager.AppSettings["keyValidarLineas_RutaWS"],
                    Timeout = Funciones.CheckInt(ConfigurationManager.AppSettings["keyValidarLineas_TimeOutWS"].ToString()),
                    Credentials = CredentialCache.DefaultCredentials
                };

                var objWS_AuditoriaRequest = new ValidarLineasClienteWS.AuditRequest()
                {
                    idTransaccion = objAuditoria.strIdTransaccion,
                    ipAplicacion = objAuditoria.strIpAplicacion,
                    nombreAplicacion = objAuditoria.strNombreAplicacion,
                    usuarioAplicacion = objAuditoria.strUsuarioAplicacion
                };

                //[PROY-140600] - INICIO
                ListaCamposAdicionalesTypeCampoAdicional[] listaCamposAdicionales = new ListaCamposAdicionalesTypeCampoAdicional[1];
                listaCamposAdicionales[0] = new ListaCamposAdicionalesTypeCampoAdicional();

                if (!string.IsNullOrEmpty(strTipoDocumento))//JMGF
                {
                    listaCamposAdicionales[0].nombreCampo = "COD_TIPODOC";
                    listaCamposAdicionales[0].valor = strTipoDocumento;
                }
                else
                {
                    listaCamposAdicionales[0].nombreCampo = string.Empty;
                    listaCamposAdicionales[0].valor = string.Empty;
                }
                //[PROY-140600] - FIN

                var objWS_ContarLineasRequest = new ValidarLineasClienteWS.contarLineasRequest()
                {
                    auditRequest = objWS_AuditoriaRequest,
                    numeroDocumento = strDocumentoNumero,
                    listaCamposAdicionales = listaCamposAdicionales//[PROY-140600]
                };
                //REQUEST::FIN

                //RESPONSE::INI
                var objWS_ContarLineasResponse = wsValidarLineas.contarLineas(objWS_ContarLineasRequest);

                //PROY-140715 INI
                BEVenta objBEVenta = new BEVenta()
                {
                    tipoDoc = string.Empty,
                    numDoc = Funciones.CheckStr(strDocumentoNumero),
                    numSec = string.Empty,
                    numPedido = string.Empty,
                    fechaInicio = string.Empty,
                    fechaFin = string.Empty,
                    estadoValidacion = "2",
                    codCanal = string.Empty,
                    codPdv = string.Empty,
                    tipoContingencia = "2",
                    estadoPago = "1"
                };

                var objListaVentaContigencia = objConsultarContigencia.consultarVentasContigencia(objBEVenta, Funciones.CheckStr(objAuditoria.strUsuarioAplicacion));
                HelperLog.EscribirLog(" ---[objListaVentaContigencia] - ", nameArchivo, String.Format("{0} : {1}", "ListaVenta: ", objListaVentaContigencia.Count()), false);
                //PROY-140715 FIN

                if (objWS_ContarLineasResponse != null)
                {
                    List<BEValidarLineaDetalle> lstDetalle = new List<BEValidarLineaDetalle>();
                    
                    objClienteLineas = new BEValidarLinea();

                    int cantidad = Funciones.CheckInt(objWS_ContarLineasResponse.cantidadLineasActivas); //PROY-140715

                    foreach (var item in objWS_ContarLineasResponse.listaLineasConsolidadasType)
                    {
                        BEValidarLineaDetalle obj = new BEValidarLineaDetalle();
                        obj.strLinea = ((item.msisdn).Remove(0, 2)) == "null" ? string.Empty : (item.msisdn).Remove(0, 2);// SUPRIMIR 51 
                        obj.strSegmento = item.segmento;

                        if(!string.IsNullOrEmpty(obj.strLinea))        //[PROY-140600]           
                        {
                            //PROY-140715 INI
                            valido = true;
                            foreach (var listaVenta in objListaVentaContigencia)
                            {
                                if (Funciones.CheckStr(listaVenta.estadoActivacion) == "2")
                                {
                                    string linea = Funciones.CheckStr(obj.strLinea);
                                    if (Funciones.CheckStr(listaVenta.linea) == Funciones.CheckStr(linea))
                                    {
                                        valido = false;
                                        cantidad = cantidad - 1;
                                    }
                                }
                            }

                            if (valido)
                            {
                        lstDetalle.Add(obj);
                    }
                        }
                        //PROY-140715 FIN
                    }

                    intRespuestaCodigo = Funciones.CheckInt(objWS_ContarLineasResponse.auditResponse.codRespuesta);
                    strRespuestaMensaje = objWS_ContarLineasResponse.auditResponse.msjRespuesta;
                    objClienteLineas.intCantidadLineasActivas = cantidad; //PROY-140715
                    objClienteLineas.lstConsolidadoLineas = lstDetalle;

                    //0 = Operación Exitosa
                    //2 = No existen líneas asociadas al documento ingresado
                    if (intRespuestaCodigo.Equals(0) || intRespuestaCodigo.Equals(2))
                        blnRespuesta = true;
                }
                //RESPONSE::FIN
            }
            catch (Exception e)
            {
                blnRespuesta = false;
                intRespuestaCodigo = -100;
                strRespuestaMensaje = "ErrorWS[" + e.Message + "] - Trace[" + e.StackTrace + "]";
                objClienteLineas = null;
            }

            return blnRespuesta;
        }
    }
}
//DIL::FIN::20170910