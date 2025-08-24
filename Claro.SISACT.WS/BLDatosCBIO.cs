using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections;
using System.Data;
using System.Web;

using Claro.SISACT.Common;
using Claro.SISACT.Entity;
using Claro.SISACT.WS.RestReferences;
using Claro.SISACT.WS.RestServices;
using Claro.SISACT.WS.BWServicesCBIO;
using Claro.SISACT.Business;
using Claro.SISACT.Entity.DataPowerRest;
using Claro.SISACT.Entity.DatosLineaRest;
using Claro.SISACT.Entity.claro_int_consultacliente.consultarDatos;
using Claro.SISACT.Entity.claro_ope_comportamientoCliente.consultarComportamientoPago;
using Claro.SISACT.Entity.claro_inte_cuparticipante.consultarParticipante;

namespace Claro.SISACT.WS
{
    [Serializable]
    public class BLDatosCBIO
    {
        #region INICIATIVA-219 | Legados 2.0 | Andre Chumbes Lizarraga
        public BEDetalleLinea_CBIO ListarDetalleLineaCBIO(string strTipoDocumento, string strNumeroDocumento, string strPdv, ref bool blDetalleLineaCBIO, ref string strFraude, ref bool blBloqueoRoboPerdida)
        {
            GeneradorLog objLog = new GeneradorLog(null, string.Format("{0}{1}", "[INICIO][INICIATIVA-219][ListarDetalleLineaCBIO]", string.Empty), null, null);
            MessageResponseConsultarCliente objResponseCliente = new MessageResponseConsultarCliente();
            BEDetalleLinea_CBIO objDetalleLinea = new BEDetalleLinea_CBIO();

            try
            {
                string strTipoDocumentoCBIO = string.Empty;
                string strFlagCBIO = string.Empty;
                string codigoRucCBIO = ConfigurationManager.AppSettings["codTipoDocumentoRUC_CBIO"];
                bool blClienteCBIO = false;

                #region Flag - WhiteList CBIO
                strFlagCBIO = Funciones.CheckStr(HttpContext.Current.Session["flagCBIO"]);

                if (string.IsNullOrEmpty(strFlagCBIO))
                {
                    //Si las sessiones son nulas, se volverán a consultar(Aplica para la pagina "Consulta de SEC").
                    Int64 codParanGrupoCBIO = Funciones.CheckInt64(ConfigurationManager.AppSettings["key_ParamGrupoCBIO"]);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-219][ListarDetalleLineaCBIO][codParanGrupoCBIO]", codParanGrupoCBIO), null, null);
                    List<BEParametro> lstParamCBIO = new BLGeneral().ListaParametrosGrupo(codParanGrupoCBIO);

                    strFlagCBIO = lstParamCBIO.Where(w => w.Valor1 == "key_flagCBIO").ToList().Count > 0 ?
                    Funciones.CheckStr(lstParamCBIO.Where(w => w.Valor1 == "key_flagCBIO").ToList()[0].Valor) : string.Empty;
                    HttpContext.Current.Session["flagCBIO"] = Funciones.CheckStr(strFlagCBIO);
                }
                objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-219][ListarDetalleLineaCBIO][strFlagCBIO]", strFlagCBIO), null, null);
                #endregion

                if (strFlagCBIO == "1")
                {
                    strTipoDocumentoCBIO = ObtenerTipoDocumentoCBIO(strTipoDocumento);
                    bool blIsRuc = codigoRucCBIO.IndexOf(strTipoDocumentoCBIO) > -1 ? true : false;
                    blClienteCBIO = ConsultarClienteCBIO(strTipoDocumentoCBIO, strNumeroDocumento, ref objResponseCliente);

                    if (blClienteCBIO)
                    {
                        objDetalleLinea = ConsultarDetalleLineaCBIO(objResponseCliente, strTipoDocumento, strNumeroDocumento, blIsRuc, ref strFraude, ref blBloqueoRoboPerdida);

                        if (objDetalleLinea != null && objDetalleLinea.objListaDetalle != null && objDetalleLinea.objListaDetalle.Count > 0)
                        {
                            blDetalleLineaCBIO = true;
                            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[ListarDetalleLineaCBIO][objDetalleLinea.objListaDetalle.Count]", Funciones.CheckStr(objDetalleLinea.objListaDetalle.Count)), null, null);

                            foreach (var objLista in objDetalleLinea.objListaDetalle)
                            {
                                objLog.CrearArchivolog("[INICIATIVA-219][ListarDetalleLineaCBIO][CBIO]**************************************************************************************", null, null);
                                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ListarDetalleLineaCBIO][CBIO][CUSTOMER_ID]", Funciones.CheckStr(objLista.D_CUSTOMER_ID)), null, null);
                                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ListarDetalleLineaCBIO][CBIO][CUENTA]", Funciones.CheckStr(objLista.CUENTA)), null, null);
                                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ListarDetalleLineaCBIO][CBIO][CO_ID]", Funciones.CheckDecimal(objLista.CO_ID)), null, null);
                                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ListarDetalleLineaCBIO][CBIO][TMCODE]", Funciones.CheckDecimal(objLista.TM_CODE)), null, null);
                                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ListarDetalleLineaCBIO][CBIO][PLAN]", Funciones.CheckStr(objLista.PLAN)), null, null);
                                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ListarDetalleLineaCBIO][CBIO][PRODUCTO/SERVICIO]", Funciones.CheckStr(objLista.PRODUCTO_SERVICIO)), null, null);
                                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ListarDetalleLineaCBIO][CBIO][NUMERO]", Funciones.CheckStr(objLista.NUMERO)), null, null);
                                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ListarDetalleLineaCBIO][CBIO][CF_CONTRATO]", Funciones.CheckDecimal(objLista.CF_CONTRATO)), null, null);
                                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ListarDetalleLineaCBIO][CBIO][PROM_FACT]", Funciones.CheckDecimal(objLista.D_PROM_FACT)), null, null);
                                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ListarDetalleLineaCBIO][CBIO][FECHA_ACTIVACION]", Funciones.CheckStr(objLista.FECHA_ACTIVACION)), null, null);
                                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ListarDetalleLineaCBIO][CBIO][ESTADO]", Funciones.CheckStr(objLista.ESTADO)), null, null);
                                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ListarDetalleLineaCBIO][CBIO][COD_BLOQ]", Funciones.CheckStr(objLista.COD_BLOQ)), null, null);
                                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ListarDetalleLineaCBIO][CBIO][COD_SUSP]", Funciones.CheckStr(objLista.COD_SUSP)), null, null);
                                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ListarDetalleLineaCBIO][CBIO][MOT_BLOQ]", Funciones.CheckStr(objLista.MOT_BLOQ)), null, null);
                                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ListarDetalleLineaCBIO][CBIO][MOT_SUSP]", Funciones.CheckStr(objLista.MOT_SUSP)), null, null);
                                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ListarDetalleLineaCBIO][CBIO][FECHA_ESTADO]", Funciones.CheckStr(objLista.FECHA_ESTADO)), null, null);
                                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ListarDetalleLineaCBIO][CBIO][MONTO_VENCIDO]", Funciones.CheckDecimal(objLista.MONTO_VENCIDO)), null, null);
                                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ListarDetalleLineaCBIO][CBIO][MONTO_CASTIGO]", Funciones.CheckDecimal(objLista.MONTO_CASTIGADO)), null, null);
                                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ListarDetalleLineaCBIO][CBIO][CAMPANA]", Funciones.CheckStr(objLista.CAMPANA)), null, null);
                                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ListarDetalleLineaCBIO][CBIO][MONTO_NO_FACT]", Funciones.CheckDecimal(objLista.D_MONTO_NO_FACT)), null, null);
                                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ListarDetalleLineaCBIO][CBIO][NRO_MOVIL]", Funciones.CheckInt(objLista.NRO_MOVIL)), null, null);
                                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ListarDetalleLineaCBIO][CBIO][NRO_INTERNET_FIJO]", Funciones.CheckInt(objLista.NRO_INTERNET_FIJO)), null, null);
                                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ListarDetalleLineaCBIO][CBIO][NRO_CLARO_TV]", Funciones.CheckInt(objLista.NRO_CLARO_TV)), null, null);
                                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ListarDetalleLineaCBIO][CBIO][NRO_TELEF_FIJA]", Funciones.CheckInt(objLista.NRO_TELEF_FIJA)), null, null);
                                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ListarDetalleLineaCBIO][CBIO][NRO_BAM]", Funciones.CheckInt(objLista.NRO_BAM)), null, null);
                                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ListarDetalleLineaCBIO][CBIO][NRO_BLOQ]", Funciones.CheckInt(objLista.NRO_BLOQ)), null, null);
                                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ListarDetalleLineaCBIO][CBIO][NRO_SUSP]", Funciones.CheckInt(objLista.NRO_SUSP)), null, null);
                                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ListarDetalleLineaCBIO][CBIO][DEUDA_REINT_EQUIPO]", Funciones.CheckDecimal(objLista.DEUDA_REINT_EQUIPO)), null, null);
                                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ListarDetalleLineaCBIO][CBIO][NRO_PLANES]", Funciones.CheckInt(objLista.NRO_PLANES)), null, null);
                                objLog.CrearArchivolog("[INICIATIVA-219][ListarDetalleLineaCBIO][CBIO]**************************************************************************************", null, null);
                            }
                        }
                    }
                    else
                    {
                        objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ListarDetalleLineaCBIO]", "No se encontraron Datos en CBIO"), null, null);
                    }
                }

            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[INICIATIVA-219][ListarDetalleLineaCBIO][Ocurrio un error al listar el detalle de linea de CBIO]", null, null);
                objLog.CrearArchivolog(string.Format("{0} {1} | {2}", "[INICIATIVA-219][ListarDetalleLineaCBIO][ERROR]", Funciones.CheckStr(ex.Message), Funciones.CheckStr(ex.StackTrace)), null, null);
                throw ex;
            }
            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[ListarDetalleLineaCBIO][blDetalleLineaCBIO]", Funciones.CheckStr(blDetalleLineaCBIO)), null, null);
            return objDetalleLinea;
        }

        public string ConsultarWhiteListCBIO(string strTipoDocumento, string strNumeroDocumento, string strPDV)
        {
            GeneradorLog objLog = new GeneradorLog(null, string.Format("{0}{1}", "[INICIO][INICIATIVA-219][ConsultarWhiteListCBIO]", string.Empty), null, null);
            BLGeneral_II objCBIO = new BLGeneral_II();
            string strCodigoRespuesta = string.Empty;
            string strMensajeRespuesta = string.Empty;

            try
            {
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarWhiteListCBIO][INPUT][PI_SOLIN_CODIGO]", string.Empty), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarWhiteListCBIO][INPUT][PI_ID_CONTRATO]", string.Empty), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarWhiteListCBIO][INPUT][PI_PDV]", Funciones.CheckStr(strPDV)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarWhiteListCBIO][INPUT][PI_TIPO_DOCUMENTO]", Funciones.CheckStr(strTipoDocumento)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarWhiteListCBIO][INPUT][PI_NRO_DOCUMENTO]", Funciones.CheckStr(strNumeroDocumento)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarWhiteListCBIO][INPUT][PI_FLUJO]", Funciones.CheckStr("1")), null, null);

                string[] arrParamConsultaCBIO = new string[6];
                arrParamConsultaCBIO[0] = string.Empty; // Numero de Sec: no se enviara para este flujo(Evaluación).
                arrParamConsultaCBIO[1] = string.Empty; // Id Contrato : no se enviara para este flujo(Evaluación).
                arrParamConsultaCBIO[2] = strPDV; // Punto de Venta del usuario
                arrParamConsultaCBIO[3] = strTipoDocumento; // Tipo de documento del cliente
                arrParamConsultaCBIO[4] = strNumeroDocumento; // Numero de documento del cliente
                arrParamConsultaCBIO[5] = "1"; // Tipo de Flujo : 1 --> Evaluación

                objCBIO.ConsultarFlagCBIO(arrParamConsultaCBIO, ref strCodigoRespuesta, ref strMensajeRespuesta);

                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarWhiteListCBIO][OUTPUT][PO_CODIGO_RESPUESTA]", strCodigoRespuesta), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarWhiteListCBIO][OUTPUT][PO_MENSAJE_RESPUESTA]", strTipoDocumento), null, null);
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[INICIATIVA-219][ConsultarWhiteListCBIO][Ocurrio un error al consultar al cliente en el WhiteList de CBIO]", null, null);
                objLog.CrearArchivolog(string.Format("{0} {1} | {2}", "[INICIATIVA-219][ConsultarWhiteListCBIO][ERROR]", Funciones.CheckStr(ex.Message), Funciones.CheckStr(ex.StackTrace)), null, null);
            }

            return strCodigoRespuesta;
        }

        public bool ConsultarClienteCBIO(string strTipoDocumentoCBIO, string strNumeroDocumento, ref MessageResponseConsultarCliente objResponse)
        {
            GeneradorLog objLog = new GeneradorLog(null, string.Format("{0}{1}", "[INICIO][INICIATIVA-219][ConsultarClienteCBIO]", string.Empty), null, null);
            BWConsultaclienteCBIO objClienteWS = new BWConsultaclienteCBIO();
            MessageRequestConsultarCliente objRequest = new MessageRequestConsultarCliente();
            bool blResultado = false;

            objRequest.Body.tipoDocumento = strTipoDocumentoCBIO;
            objRequest.Body.numeroDocumento = strNumeroDocumento;

            objResponse = objClienteWS.ConsultarClienteWSCBIO(objRequest);

            if (objResponse != null && objResponse.consultarDatosResponse != null && objResponse.consultarDatosResponse.responseAudit != null
                && objResponse.consultarDatosResponse.responseAudit.codigoRespuesta == "0")
            {
                if (objResponse.consultarDatosResponse.responseData != null)
                {
                    blResultado = true;
                }
            }

            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarClienteCBIO][blResultado]", blResultado), null, null);

            objLog.CrearArchivolog(string.Format("{0}{1}", "[FIN][INICIATIVA-219][ConsultarClienteCBIO]", string.Empty), null, null);

            return blResultado;
        }

        public int ObtenerComportamientoPago(string strTipoDocumento, string strNumeroDocumento)
        {
            GeneradorLog objLog = new GeneradorLog(null, string.Format("{0}{1}", "[INICIO][INICIATIVA-219][ObtenerComportamientoPago]", string.Empty), null, null);
            BWComportamientoClienteCBIO objComportamientoWS = new BWComportamientoClienteCBIO();
            BodyResponseComportamientoCliente objResponse = new BodyResponseComportamientoCliente();
            BodyRequestComportamientoCliente objRequest = new BodyRequestComportamientoCliente();
            int comportamientoPago = 0;

            try
            {
                objRequest.consultarComportamientPago.tipoDocumento = Funciones.CheckInt(strTipoDocumento);
                objRequest.consultarComportamientPago.numeroDocumento = strNumeroDocumento;

                objResponse = objComportamientoWS.ObtenerComportamientoPagoWSCBIO(objRequest);

                if ((objResponse != null && objResponse.consultarComportamientoPagoResponse != null && objResponse.consultarComportamientoPagoResponse.responseAudit != null
                    && objResponse.consultarComportamientoPagoResponse.responseAudit.codigoRespuesta == "0")) //Audit
                {
                    if (objResponse.consultarComportamientoPagoResponse.responseData != null) //Data
                    {
                        comportamientoPago = Funciones.CheckInt(objResponse.consultarComportamientoPagoResponse.responseData.comportamientoPago);
                        objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ObtenerComportamientoPago][OUTPUT][comportamientoPago]", Funciones.CheckStr(comportamientoPago)), null, null);
                    }
                    else
                    {
                        objLog.CrearArchivolog("[INICIATIVA-219][ObtenerComportamientoPago][Ocurrio un error, se le asignará 0 por defecto al comportamiento de pago del cliente]", null, null);
                    }
                }
                else
                {
                    objLog.CrearArchivolog("[INICIATIVA-219][ObtenerComportamientoPago][Ocurrio un error, se le asignará 0 por defecto al comportamiento de pago del cliente]", null, null);
                }
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[INICIATIVA-219][ObtenerComportamientoPago][Ocurrio un error al obtener el comportamiento de pago del cliente]", null, null);
                objLog.CrearArchivolog(string.Format("{0} {1} | {2}", "[INICIATIVA-219][ObtenerComportamientoPago][ERROR]", Funciones.CheckStr(ex.Message), Funciones.CheckStr(ex.StackTrace)), null, null);
            }

            objLog.CrearArchivolog(string.Format("{0}{1}", "[FIN][INICIATIVA-219][ObtenerComportamientoPago]", string.Empty), null, null);

            return Funciones.CheckInt(comportamientoPago);
        }

        public bool ObtenerPromedioFacturado(string strTipoDocumento, string strNumeroDocumento, ref BodyResponseComportamientoCliente objResponse)
        {
            GeneradorLog objLog = new GeneradorLog(null, string.Format("{0}{1}", "[INICIO][INICIATIVA-219][ObtenerPromedioFacturado]", string.Empty), null, null);
            BWComportamientoClienteCBIO objPromedioFacturadoWS = new BWComportamientoClienteCBIO();
            BodyRequestComportamientoCliente objRequest = new BodyRequestComportamientoCliente();
            objResponse = new BodyResponseComportamientoCliente();
            bool blResultado = false;

            objRequest.consultarPromedioFacturado.tipoDocumento = Funciones.CheckInt(strTipoDocumento);
            objRequest.consultarPromedioFacturado.numeroDocumento = strNumeroDocumento;

            objResponse = objPromedioFacturadoWS.ObtenerPromedioFacturadoWSCBIO(objRequest);

            if ((objResponse != null && objResponse.consultarPromedioFacturadoResponse != null && objResponse.consultarPromedioFacturadoResponse.responseAudit != null
                && objResponse.consultarPromedioFacturadoResponse.responseAudit.codigoRespuesta == "0")) //Audit
            {
                if (objResponse.consultarPromedioFacturadoResponse.responseData != null && objResponse.consultarPromedioFacturadoResponse.responseData.listPromedioFacturado != null
                    && objResponse.consultarPromedioFacturadoResponse.responseData.listPromedioFacturado.Count > 0) //Data
                {
                    blResultado = true;
                }
            }

            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ObtenerPromedioFacturado][blResultado]", blResultado), null, null);
            objLog.CrearArchivolog(string.Format("{0}{1}", "[FIN][INICIATIVA-219][ObtenerPromedioFacturado]", string.Empty), null, null);

            return blResultado;
        }

        public string ObtenerTipoDocumentoCBIO(string strTipoDocumento)
        {
            string strTipoDocumentoCBIO = string.Empty;
            List<BETipoDocumento> objListaDocumento = (new BLGeneral()).ListarTipoDocumento();
            strTipoDocumentoCBIO = Funciones.CheckStr(objListaDocumento.Where(x => x.ID_SISACT == strTipoDocumento).FirstOrDefault().ID_BSCS_IX);

            return strTipoDocumentoCBIO;
        }

        public BEDetalleLinea_CBIO ConsultarDetalleLineaCBIO(MessageResponseConsultarCliente objConsultarCliente, string strTipoDocumento, string strNumeroDocumento, bool blIsRuc, ref string strFraude, ref bool blBloqueoRoboPerdida)
        {
            GeneradorLog objLog = new GeneradorLog(null, string.Format("{0}{1}", "[INICIO][INICIATIVA-219][ConsultarDetalleLineaCBIO]", string.Empty), null, null);
            List<BEDetalleLinea_CBIO> objListaDetalleLinea = new List<BEDetalleLinea_CBIO>();
            BEDetalleLinea_CBIO objDetalleLinea = new BEDetalleLinea_CBIO();
            List<BEDetalleLinea_CBIO_D> lstDetalleLineaCbio = new List<BEDetalleLinea_CBIO_D>();
            BodyResponseComportamientoCliente objPromedioFacturado = new BodyResponseComportamientoCliente();
            BLGeneral_II objSPTicklers = new BLGeneral_II();
            string strProducto = string.Empty;
            string strTMCODE = string.Empty;
            string strDescripcionTickler = string.Empty;
            string strTipoTickler = string.Empty;
            string strMsjRespuesta = string.Empty;
            string strCodBloq = "B";
            string strCodSusp = "S";
            string strDescripcionPlan = string.Empty;
            string strPlanBasico = string.Empty;
            string strPlanesAdic = string.Empty;
            string strCodCuenta = string.Empty;
            string lstDetalleRuc = string.Empty;
            string strCodigoTicklerBSCS = string.Empty;
            string strCodFraudeBscs = ConfigurationManager.AppSettings["codigoFraudeBSCS"];
            string codigoRoboPerdidaBSCS = ConfigurationManager.AppSettings["codigoRoboPerdidaBSCS"];
            bool blPromedioFacturado = false;
            double dblPromedioFacturado = 0;
            HttpContext.Current.Session["DetalleLineaCbio"] = null;

            blPromedioFacturado = ObtenerPromedioFacturado(strTipoDocumento, strNumeroDocumento, ref objPromedioFacturado);

            #region DatosLineaCabecera
            var objListaCustomer = objConsultarCliente.consultarDatosResponse.responseData.customer;
            objDetalleLinea.C_CUSTOMER_ID = Funciones.CheckStr(strNumeroDocumento);
            objDetalleLinea.RAZON_SOCIAL = Funciones.CheckStr(objConsultarCliente.consultarDatosResponse.responseData.razonSocial);
            objDetalleLinea.NOMBRES = Funciones.CheckStr(objConsultarCliente.consultarDatosResponse.responseData.nombres);
            objDetalleLinea.CF = Funciones.CheckDbl(objConsultarCliente.consultarDatosResponse.responseData.cargoFijo);
            objDetalleLinea.APELLIDOS = Funciones.CheckStr(objConsultarCliente.consultarDatosResponse.responseData.apellidos);
            objDetalleLinea.BLOQ = Funciones.CheckInt(objConsultarCliente.consultarDatosResponse.responseData.cantPlanesBloquedos);
            objDetalleLinea.SUSP = Funciones.CheckInt(objConsultarCliente.consultarDatosResponse.responseData.cantPlanesSuspendidos);
            objDetalleLinea.PLANES = Funciones.CheckInt(objConsultarCliente.consultarDatosResponse.responseData.cantPlanes);
            objDetalleLinea.NRO_7 = Funciones.CheckInt(objConsultarCliente.consultarDatosResponse.responseData.nro7);
            objDetalleLinea.NRO_30 = Funciones.CheckInt(objConsultarCliente.consultarDatosResponse.responseData.nro30);
            objDetalleLinea.NRO_90 = Funciones.CheckInt(objConsultarCliente.consultarDatosResponse.responseData.nro90);
            objDetalleLinea.NRO_180 = Funciones.CheckInt(objConsultarCliente.consultarDatosResponse.responseData.nro180);
            objDetalleLinea.NRO_90_MAS = Funciones.CheckInt(objConsultarCliente.consultarDatosResponse.responseData.nro90);
            objDetalleLinea.NRO_180_MAS = Funciones.CheckInt(objConsultarCliente.consultarDatosResponse.responseData.nro180mas);

            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][Cabecera][CUSTOMER_ID]", objDetalleLinea.C_CUSTOMER_ID), null, null);
            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][Cabecera][BLOQ]", objDetalleLinea.BLOQ), null, null);
            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][Cabecera][SUSP]", objDetalleLinea.SUSP), null, null);
            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][Cabecera][PLANES]", objDetalleLinea.PLANES), null, null);
            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][Cabecera][NRO_7]", objDetalleLinea.NRO_7), null, null);
            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][Cabecera][NRO_30]", objDetalleLinea.NRO_30), null, null);
            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][Cabecera][NRO_90]", objDetalleLinea.NRO_90), null, null);
            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][Cabecera][NRO_180]", objDetalleLinea.NRO_180), null, null);
            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][Cabecera][NRO_90_MAS]", objDetalleLinea.NRO_90_MAS), null, null);
            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][Cabecera][NRO_180_MAS]", objDetalleLinea.NRO_180_MAS), null, null);
            #endregion

            #region DatosLineaDetalle
            foreach (var objCustomer in objListaCustomer)
            {
                if (objCustomer.contratos != null && objCustomer.contratos.Count > 0)
                {
                    foreach (var objContrato in objCustomer.contratos)
                    {
                        BEDetalleLinea_CBIO_D objDetalleLineaCbio = new BEDetalleLinea_CBIO_D();

                        if (!string.IsNullOrEmpty(objContrato.coStatus))
                        {
                            switch (objContrato.coStatus)
                            {
                                case "a":
                                    objDetalleLineaCbio.ESTADO = "Activo";
                                    break;
                                case "s":
                                    objDetalleLineaCbio.ESTADO = "Suspendido";
                                    break;
                                case "d":
                                    objDetalleLineaCbio.ESTADO = "Desactivo";
                                    break;
                            }
                        }

                        objContrato.coActivated = Funciones.CheckStr(objContrato.coActivated) == string.Empty ? string.Empty : Funciones.CheckStr(Convert.ToDateTime((objContrato.coActivated)).ToString("dd/MM/yyyy"));
                        objContrato.coLastStatusChangeDate = Funciones.CheckStr(objContrato.coLastStatusChangeDate) == string.Empty ? string.Empty : Funciones.CheckStr(Convert.ToDateTime((objContrato.coLastStatusChangeDate)).ToString("dd/MM/yyyy"));

                        objDetalleLineaCbio.CO_ID = Funciones.CheckStr(objContrato.coId);
                        objDetalleLineaCbio.NUMERO = Funciones.CheckStr(objContrato.dirNum).Substring(2, objContrato.dirNum.Length - 2);
                        objDetalleLineaCbio.FECHA_ACTIVACION = objContrato.coActivated;
                        objDetalleLineaCbio.FECHA_ESTADO = objContrato.coLastStatusChangeDate;
                        objDetalleLineaCbio.D_CUSTOMER_ID = Funciones.CheckStr(objCustomer.customerId);
                        objDetalleLineaCbio.CUENTA = Funciones.CheckStr(objCustomer.custCode);
                        objDetalleLineaCbio.CF_CONTRATO = Funciones.CheckDbl(objContrato.cargoFijo);
                        objDetalleLineaCbio.CICLO_FACTURACION = Funciones.CheckStr(objCustomer.billingCycle);

                        objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][Detalle][CO_ID]", objDetalleLineaCbio.CO_ID), null, null);
                        objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][Detalle][NUMERO]", objDetalleLineaCbio.NUMERO), null, null);
                        objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][Detalle][FECHA_ACTIVACION]", objDetalleLineaCbio.FECHA_ACTIVACION), null, null);
                        objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][Detalle][FECHA_ESTADO]", objDetalleLineaCbio.FECHA_ESTADO), null, null);
                        objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][Detalle][CUSTOMER_ID]", objDetalleLineaCbio.D_CUSTOMER_ID), null, null);
                        objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][Detalle][CUSTOMER_ID_PUB]", objCustomer.customerIdPub), null, null);
                        objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][Detalle][CUENTA]", objDetalleLineaCbio.CUENTA), null, null);
                        objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][Detalle][CF_CONTRATO]", objDetalleLineaCbio.CF_CONTRATO), null, null);
                        objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][Detalle][CICLO_FACTURACION]", objDetalleLineaCbio.CICLO_FACTURACION), null, null);
                        objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][Detalle][ESTADO]", Funciones.CheckStr(objDetalleLineaCbio.ESTADO)), null, null);

                        #region Bloqueo - Suspension - Fraude
                        int nroBloq = 0;
                        int nroSusp = 0;

                        if (objDetalleLineaCbio.ESTADO == "Suspendido")
                        {
                            if (objContrato.ticklers != null && objContrato.ticklers.Count > 0)
                            {
                                foreach (var objTicklers in objContrato.ticklers)
                                {
                                    if (!string.IsNullOrEmpty(objTicklers.tickLdes) && objTicklers.tickStatus == "OPEN")
                                    {
                                        objTicklers.tickLdes = objTicklers.tickLdes.Replace("|", "");
                                        objSPTicklers.ObtenerDescripcionTicklers(objTicklers.tickLdes, ref strDescripcionTickler, ref strTipoTickler, ref strCodigoTicklerBSCS, ref strMsjRespuesta);
                                        if (strTipoTickler == strCodBloq) // "B"
                                        {
                                            objDetalleLineaCbio.COD_BLOQ = strCodigoTicklerBSCS;
                                            objDetalleLineaCbio.MOT_BLOQ = strDescripcionTickler;
                                            nroBloq++;
                                            objDetalleLineaCbio.NRO_BLOQ = nroBloq;
                                            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][Detalle][COD_BLOQ de CBIO]", objDetalleLineaCbio.COD_BLOQ), null, null);
                                            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][Detalle][MOT_BLOQ de CBIO]", objDetalleLineaCbio.MOT_BLOQ), null, null);

                                            if (!string.IsNullOrEmpty(strCodigoTicklerBSCS) && strCodFraudeBscs.IndexOf(strCodigoTicklerBSCS) > -1)
                                            {
                                                strFraude = string.Format("{0}{1},{2}|", strFraude, objDetalleLineaCbio.D_CUSTOMER_ID, strCodigoTicklerBSCS);
                                            }
                                            else if (codigoRoboPerdidaBSCS.Contains(strCodigoTicklerBSCS))
                                            {
                                                blBloqueoRoboPerdida = true;
                                            }
                                        }
                                        else if (strTipoTickler == strCodSusp) // "S"
                                        {
                                            objDetalleLineaCbio.COD_SUSP = strCodigoTicklerBSCS;
                                            objDetalleLineaCbio.MOT_SUSP = strDescripcionTickler;
                                            nroSusp++;
                                            objDetalleLineaCbio.NRO_SUSP = nroSusp;
                                            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][Detalle][COD_SUSP de CBIO]", objDetalleLineaCbio.COD_SUSP), null, null);
                                            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][Detalle][MOT_SUSP de CBIO]", objDetalleLineaCbio.MOT_SUSP), null, null);

                                            if (!string.IsNullOrEmpty(strCodigoTicklerBSCS) && strCodFraudeBscs.IndexOf(strCodigoTicklerBSCS) > -1)
                                            {
                                                strFraude = string.Format("{0}{1},{2}|", strFraude, objDetalleLineaCbio.D_CUSTOMER_ID, strCodigoTicklerBSCS);
                                            }
                                        }
                                    }
                                }

                                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][Detalle][NRO_BLOQ]", objDetalleLineaCbio.NRO_BLOQ), null, null);
                                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][Detalle][NRO_SUSP]", objDetalleLineaCbio.NRO_SUSP), null, null);
                            }
                        }
                        #endregion

                        #region Plan - Servicios Adicionales
                        if (objContrato.productos != null && objContrato.productos.Count > 0)
                        {
                            var objProductos = objContrato.productos;

                            if (objProductos.Where(w => w.productOfferingType == "B").Count() > 0)
                            {
                                strPlanBasico = objContrato.productos.Where(w => w.productOfferingType == "B").First().productOfferingId;
                            }

                            if (objProductos.Where(w => w.productOfferingType != "B").Where(x => x.productStatus == "a").Count() > 0)
                            {
                                strPlanesAdic = objContrato.productos.Where(w => w.productOfferingType != "B").Where(x => x.productStatus == "a").Select(s => s.productOfferingId).Aggregate((a, b) => a + "," + b);
                            }

                            if (!string.IsNullOrEmpty(strPlanBasico))
                            {
                                objDetalleLineaCbio = CalcularBilleteraXPlanCBIO(objDetalleLineaCbio, strPlanBasico, ref strTMCODE, ref strDescripcionPlan);
                                objDetalleLineaCbio.TM_CODE = strTMCODE;
                                objDetalleLineaCbio.PLAN = strDescripcionPlan;
                                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][Detalle][PRODUCT_ID]", strPlanBasico), null, null);
                                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][Detalle][TM_CODE]", objDetalleLineaCbio.TM_CODE), null, null);
                                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][Detalle][PLAN]", objDetalleLineaCbio.PLAN), null, null);
                            }

                            if (!string.IsNullOrEmpty(strPlanesAdic))
                            {
                                List<string> lstPlanesAdic = strPlanesAdic.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                ObtenerDescripcionServiciosAdicionales(lstPlanesAdic, ref strProducto);
                                objDetalleLineaCbio.PRODUCTO_SERVICIO = Funciones.CheckStr(strProducto);
                                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][Detalle][PRODUCTO_SERVICIO]", objDetalleLineaCbio.PRODUCTO_SERVICIO), null, null);
                            }
                        }
                        #endregion

                        #region PromedioFacturado
                        objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][INICIO REGION PROMEDIO FACTURADO]", string.Empty), null, null);
                        objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][blPromedioFacturado]", Funciones.CheckStr(blPromedioFacturado)), null, null);
                        if (blPromedioFacturado)
                        {
                            var cargoFijo = objDetalleLineaCbio.CF_CONTRATO;
                            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][cargoFijo]", Funciones.CheckStr(cargoFijo)), null, null);
                            var ListaPromedioFacturado = objPromedioFacturado.consultarPromedioFacturadoResponse.responseData.listPromedioFacturado;

                            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][objCustomer.customerIdPub]", Funciones.CheckStr(objCustomer.customerIdPub)), null, null);

                            if (ListaPromedioFacturado.Where(x => x.customerId == objCustomer.customerIdPub).Count() > 0)
                            {
                                dblPromedioFacturado = ListaPromedioFacturado.Where(x => x.customerId == objCustomer.customerIdPub).FirstOrDefault().dblPromedio;
                                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][dblPromedioFacturado]", Funciones.CheckStr(dblPromedioFacturado)), null, null);

                                objDetalleLineaCbio.D_PROM_FACT = dblPromedioFacturado > 0 ? dblPromedioFacturado : 0;
                                objDetalleLineaCbio.D_MONTO_NO_FACT = objDetalleLineaCbio.D_PROM_FACT > 0 ? 0 : cargoFijo;

                                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][Detalle][PROM_FACT]", objDetalleLineaCbio.D_PROM_FACT), null, null);
                                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][Detalle][MONTO_NO_FACT]", objDetalleLineaCbio.D_MONTO_NO_FACT), null, null);
                            }
                            else
                            {
                                objLog.CrearArchivolog(string.Format("{0} : {1} Existe en CBIO pero no en OAC", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][customerIdPub]", objCustomer.customerIdPub), null, null);
                            }
                        }
                        objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][FIN REGION PROMEDIO FACTURADO]", string.Empty), null, null);
                        #endregion

                        objDetalleLineaCbio.NRO_PLANES = objCustomer.contratos.Count();
                        lstDetalleLineaCbio.Add(objDetalleLineaCbio);
                        objDetalleLinea.objListaDetalle = lstDetalleLineaCbio;
                    }
                }
            }
            foreach (var objLista in objDetalleLinea.objListaDetalle)
            {
                objLog.CrearArchivolog("[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO]**************************************************************************************", null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][CUSTOMER_ID]", Funciones.CheckStr(objLista.D_CUSTOMER_ID)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][CUENTA]", Funciones.CheckStr(objLista.CUENTA)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][CO_ID]", Funciones.CheckDecimal(objLista.CO_ID)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][TMCODE]", Funciones.CheckDecimal(objLista.TM_CODE)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][PLAN]", Funciones.CheckStr(objLista.PLAN)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][PRODUCTO/SERVICIO]", Funciones.CheckStr(objLista.PRODUCTO_SERVICIO)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][NUMERO]", Funciones.CheckStr(objLista.NUMERO)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][CF_CONTRATO]", Funciones.CheckDecimal(objLista.CF_CONTRATO)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][PROM_FACT]", Funciones.CheckDecimal(objLista.D_PROM_FACT)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][FECHA_ACTIVACION]", Funciones.CheckStr(objLista.FECHA_ACTIVACION)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][ESTADO]", Funciones.CheckStr(objLista.ESTADO)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][COD_BLOQ]", Funciones.CheckStr(objLista.COD_BLOQ)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][COD_SUSP]", Funciones.CheckStr(objLista.COD_SUSP)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][MOT_BLOQ]", Funciones.CheckStr(objLista.MOT_BLOQ)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][MOT_SUSP]", Funciones.CheckStr(objLista.MOT_SUSP)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][FECHA_ESTADO]", Funciones.CheckStr(objLista.FECHA_ESTADO)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][MONTO_VENCIDO]", Funciones.CheckDecimal(objLista.MONTO_VENCIDO)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][MONTO_CASTIGO]", Funciones.CheckDecimal(objLista.MONTO_CASTIGADO)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][CAMPANA]", Funciones.CheckStr(objLista.CAMPANA)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][MONTO_NO_FACT]", Funciones.CheckDecimal(objLista.D_MONTO_NO_FACT)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][NRO_MOVIL]", Funciones.CheckInt(objLista.NRO_MOVIL)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][NRO_INTERNET_FIJO]", Funciones.CheckInt(objLista.NRO_INTERNET_FIJO)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][NRO_CLARO_TV]", Funciones.CheckInt(objLista.NRO_CLARO_TV)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][NRO_TELEF_FIJA]", Funciones.CheckInt(objLista.NRO_TELEF_FIJA)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][NRO_BAM]", Funciones.CheckInt(objLista.NRO_BAM)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][NRO_BLOQ]", Funciones.CheckInt(objLista.NRO_BLOQ)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][NRO_SUSP]", Funciones.CheckInt(objLista.NRO_SUSP)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][DEUDA_REINT_EQUIPO]", Funciones.CheckDecimal(objLista.DEUDA_REINT_EQUIPO)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][NRO_PLANES]", Funciones.CheckInt(objLista.NRO_PLANES)), null, null);
                objLog.CrearArchivolog("[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO]**************************************************************************************", null, null);
            }

            objLog.CrearArchivolog(string.Format("{0} --> {1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][blIsRuc]", Funciones.CheckStr(blIsRuc)), null, null);
            #region Ruc
            if (blIsRuc)
            {
                List<BEDetalleLinea_CBIO_D> lstDetalleLineaRucCbio = new List<BEDetalleLinea_CBIO_D>();
                BEDetalleLinea_CBIO_D objDetalleLineaRucCbio = new BEDetalleLinea_CBIO_D();
                foreach (var objRuc in objDetalleLinea.objListaDetalle)
                {
                    objLog.CrearArchivolog(string.Format("{0} --> {1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][strCodCuenta]", Funciones.CheckStr(strCodCuenta)), null, null);
                    objLog.CrearArchivolog(string.Format("{0} --> {1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][objRuc.CUENTA]", Funciones.CheckStr(objRuc.CUENTA)), null, null);
                    if (strCodCuenta != objRuc.CUENTA)
                    {
                        strCodCuenta = objRuc.CUENTA;
                        int TotalNroBam = objDetalleLinea.objListaDetalle.Where(x => x.CUENTA == strCodCuenta).Sum(a => a.NRO_BAM);
                        int TotalNroClaroTV = objDetalleLinea.objListaDetalle.Where(x => x.CUENTA == strCodCuenta).Sum(a => a.NRO_CLARO_TV);
                        int TotalNroInternetFijo = objDetalleLinea.objListaDetalle.Where(x => x.CUENTA == strCodCuenta).Sum(a => a.NRO_INTERNET_FIJO);
                        int TotalNroMovil = objDetalleLinea.objListaDetalle.Where(x => x.CUENTA == strCodCuenta).Sum(a => a.NRO_MOVIL);
                        int TotalNroTeleFija = objDetalleLinea.objListaDetalle.Where(x => x.CUENTA == strCodCuenta).Sum(a => a.NRO_TELEF_FIJA);
                        double TotalCargoFijo = objDetalleLinea.objListaDetalle.Where(x => x.CUENTA == strCodCuenta).Sum(a => a.CF_CONTRATO);
                        int TotalNroBloqueos = objDetalleLinea.objListaDetalle.Where(x => x.CUENTA == strCodCuenta).Sum(a => a.NRO_BLOQ);
                        int TotalNroSusp = objDetalleLinea.objListaDetalle.Where(x => x.CUENTA == strCodCuenta).Sum(a => a.NRO_SUSP);

                        objDetalleLineaRucCbio.NRO_BAM = Funciones.CheckInt(TotalNroBam);
                        objDetalleLineaRucCbio.NRO_CLARO_TV = Funciones.CheckInt(TotalNroClaroTV);
                        objDetalleLineaRucCbio.NRO_INTERNET_FIJO = Funciones.CheckInt(TotalNroInternetFijo);
                        objDetalleLineaRucCbio.NRO_MOVIL = Funciones.CheckInt(TotalNroMovil);
                        objDetalleLineaRucCbio.NRO_TELEF_FIJA = Funciones.CheckInt(TotalNroTeleFija);
                        objDetalleLineaRucCbio.CUENTA = Funciones.CheckStr(objRuc.CUENTA);
                        objDetalleLineaRucCbio.D_CUSTOMER_ID = Funciones.CheckStr(objRuc.D_CUSTOMER_ID);
                        objDetalleLineaRucCbio.FECHA_ACTIVACION = Funciones.CheckStr(objRuc.FECHA_ACTIVACION);
                        objDetalleLineaRucCbio.ESTADO = Funciones.CheckStr(objRuc.ESTADO);
                        objDetalleLineaRucCbio.CF_CONTRATO = Funciones.CheckDbl(TotalCargoFijo);
                        objDetalleLineaRucCbio.NRO_BLOQ = Funciones.CheckInt(TotalNroBloqueos);
                        objDetalleLineaRucCbio.NRO_SUSP = Funciones.CheckInt(TotalNroSusp);
                        objDetalleLineaRucCbio.NRO_PLANES = Funciones.CheckInt(objRuc.NRO_PLANES);
                        objLog.CrearArchivolog("[INICIATIVA-219][ConsultarDetalleLineaCBIO][Se agrega nuevo registro]", null, null);
                        lstDetalleLineaRucCbio.Add(objDetalleLineaRucCbio);
                    }
                }
                objDetalleLinea.objListaDetalle = lstDetalleLineaRucCbio;

                foreach (var objLista in objDetalleLinea.objListaDetalle)
                {
                    objLog.CrearArchivolog("[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO]**************************************************************************************", null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][CUSTOMER_ID]", Funciones.CheckStr(objLista.D_CUSTOMER_ID)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][CUENTA]", Funciones.CheckStr(objLista.CUENTA)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][CO_ID]", Funciones.CheckDecimal(objLista.CO_ID)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][TMCODE]", Funciones.CheckDecimal(objLista.TM_CODE)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][PLAN]", Funciones.CheckStr(objLista.PLAN)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][PRODUCTO/SERVICIO]", Funciones.CheckStr(objLista.PRODUCTO_SERVICIO)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][NUMERO]", Funciones.CheckStr(objLista.NUMERO)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][CF_CONTRATO]", Funciones.CheckDecimal(objLista.CF_CONTRATO)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][PROM_FACT]", Funciones.CheckDecimal(objLista.D_PROM_FACT)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][FECHA_ACTIVACION]", Funciones.CheckStr(objLista.FECHA_ACTIVACION)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][ESTADO]", Funciones.CheckStr(objLista.ESTADO)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][COD_BLOQ]", Funciones.CheckStr(objLista.COD_BLOQ)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][COD_SUSP]", Funciones.CheckStr(objLista.COD_SUSP)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][MOT_BLOQ]", Funciones.CheckStr(objLista.MOT_BLOQ)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][MOT_SUSP]", Funciones.CheckStr(objLista.MOT_SUSP)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][FECHA_ESTADO]", Funciones.CheckStr(objLista.FECHA_ESTADO)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][MONTO_VENCIDO]", Funciones.CheckDecimal(objLista.MONTO_VENCIDO)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][MONTO_CASTIGO]", Funciones.CheckDecimal(objLista.MONTO_CASTIGADO)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][CAMPANA]", Funciones.CheckStr(objLista.CAMPANA)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][MONTO_NO_FACT]", Funciones.CheckDecimal(objLista.D_MONTO_NO_FACT)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][NRO_MOVIL]", Funciones.CheckInt(objLista.NRO_MOVIL)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][NRO_INTERNET_FIJO]", Funciones.CheckInt(objLista.NRO_INTERNET_FIJO)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][NRO_CLARO_TV]", Funciones.CheckInt(objLista.NRO_CLARO_TV)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][NRO_TELEF_FIJA]", Funciones.CheckInt(objLista.NRO_TELEF_FIJA)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][NRO_BAM]", Funciones.CheckInt(objLista.NRO_BAM)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][NRO_BLOQ]", Funciones.CheckInt(objLista.NRO_BLOQ)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][NRO_SUSP]", Funciones.CheckInt(objLista.NRO_SUSP)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][DEUDA_REINT_EQUIPO]", Funciones.CheckDecimal(objLista.DEUDA_REINT_EQUIPO)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO][NRO_PLANES]", Funciones.CheckInt(objLista.NRO_PLANES)), null, null);
                    objLog.CrearArchivolog("[INICIATIVA-219][ConsultarDetalleLineaCBIO][CBIO]**************************************************************************************", null, null);
                }
            }

            HttpContext.Current.Session["DetalleLineaCbio"] = objDetalleLinea;

            #endregion
            #endregion

            objLog.CrearArchivolog(string.Format("{0}{1}", "[FIN][INICIATIVA-219][ConsultarDetalleLineaCBIO]", string.Empty), null, null);

            return objDetalleLinea;
        }

        #region CalcularBilleteraXPlanCBIO
        public BEDetalleLinea_CBIO_D CalcularBilleteraXPlanCBIO(BEDetalleLinea_CBIO_D objDetalleLineaCbio, string strPlanPoId, ref string strTMCODE, ref string strDescripcionPlan)
        {
            //Obtener las billeteras por el plan basico
            GeneradorLog objLog = new GeneradorLog(null, string.Format("{0}{1}", "[INICIO][INICIATIVA-219][CalcularBilleteraXPlanCBIO]", string.Empty), null, null);
            BEPlan_CBIO objPlanes = new BEPlan_CBIO();
            BLGeneral_II objGeneral = new BLGeneral_II();
            StringBuilder sbLog = new StringBuilder();
            string strLog = string.Empty;
            string strDescripcionServicio = string.Empty;
            string strCodigoRespuesta = string.Empty;
            string strMsjRespuesta = string.Empty;

            //INI: Modificacion SP Catalogo
            objPlanes = objGeneral.ListarPlanesCBIO_Catalogo(strPlanPoId); // Se le envia el Plan basico

            if (objDetalleLineaCbio.ESTADO == "Activo" || objDetalleLineaCbio.ESTADO == "Suspendido")
            {
                if (objPlanes.lstBilletera != null && objPlanes.lstBilletera.Count > 0)
                {
                    foreach (var objLista in objPlanes.lstBilletera)
                    {
                        objDetalleLineaCbio.NRO_MOVIL = objLista.PRCLN_CODIGO == 2 ? 1 : 0;
                        objDetalleLineaCbio.NRO_INTERNET_FIJO = objLista.PRCLN_CODIGO == 4 ? 1 : 0;
                        objDetalleLineaCbio.NRO_CLARO_TV = objLista.PRCLN_CODIGO == 8 ? 1 : 0;
                        objDetalleLineaCbio.NRO_TELEF_FIJA = objLista.PRCLN_CODIGO == 16 ? 1 : 0;
                        objDetalleLineaCbio.NRO_BAM = objLista.PRCLN_CODIGO == 32 ? 1 : 0;
                    }

                    sbLog.AppendFormat("[PRODUCT_ID : {0}]", strPlanPoId);
                    sbLog.AppendFormat("[NRO_MOVIL : {0}]", objDetalleLineaCbio.NRO_MOVIL);
                    sbLog.AppendFormat("[NRO_INTERNET_FIJO : {0}]", objDetalleLineaCbio.NRO_INTERNET_FIJO);
                    sbLog.AppendFormat("[NRO_CLARO_TV : {0}]", objDetalleLineaCbio.NRO_CLARO_TV);
                    sbLog.AppendFormat("[NRO_TELEF_FIJA : {0}]", objDetalleLineaCbio.NRO_TELEF_FIJA);
                    sbLog.AppendFormat("[NRO_BAM : {0}]", objDetalleLineaCbio.NRO_BAM);

                    strLog = sbLog.ToString();

                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][CalcularBilleteraXPlanCBIO][Resultado]", Funciones.CheckStr(strLog)), null, null);
                }
            }
            else
            {
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][CalcularBilleteraXPlanCBIO][No se calcularan las billetera por el plan debido a que el estado de la linea no es Activo o Suspendido][Estado]", objDetalleLineaCbio.ESTADO), null, null);
            }

            strDescripcionPlan = objPlanes.PLANV_DESCRIPCION; //Se adecuara para consultar el sp de Catalogo
            //FIN: Modificacion SP Catalogo
            strTMCODE = Funciones.CheckStr(ObtenerDatosPlanCBIO(strPlanPoId)); //Codigo BSCS del Plan

            objLog.CrearArchivolog(string.Format("{0}{1}", "[FIN][INICIATIVA-219][CalcularBilleteraXPlanCBIO]", string.Empty), null, null);

            return objDetalleLineaCbio;
        }
        #endregion

        private string ObtenerDatosPlanCBIO(string strPlanPoId)
        {
            string resultado = string.Empty;
            BLGeneral_II objGeneral = new BLGeneral_II();
            GeneradorLog objLog = new GeneradorLog(null, "[INICIO][INICIATIVA-219][ObtenerDatosPlanCBIO]", null, null);
            BEPlan_CBIO objPlan = new BEPlan_CBIO();
            try
            {
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ObtenerDatosPlanCBIO][SISACT_PKG_GENERAL_CBIO.SISACTSS_OBTENER_DATOS_PLAN][PI_PO_ID]-->", Funciones.CheckStr(strPlanPoId)), null, null);
                objPlan = objGeneral.ListarDatosPlanesCBIO(strPlanPoId, "", "CONSUMER");
                resultado = Funciones.CheckStr(objPlan.CODIGO_BSCS);
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ObtenerDatosPlanCBIO][ERROR]", Funciones.CheckStr(ex.Message)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ObtenerDatosPlanCBIO][ERROR]", Funciones.CheckStr(ex.StackTrace)), null, null);
                resultado = string.Empty;
            }
            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ObtenerDatosPlanCBIO][Resultado]", Funciones.CheckStr(resultado)), null, null);
            objLog.CrearArchivolog( "[FIN][INICIATIVA-219][ObtenerDatosPlanCBIO]", null, null);
            return resultado;
        }

        #region ObtenerDescripcionServiciosAdicionales
        private void ObtenerDescripcionServiciosAdicionales(List<string> lstServiciosAdicionales, ref string strProducto)
        {
            GeneradorLog objLog = new GeneradorLog(null, string.Format("{0}{1}", "[INICIO][INICIATIVA-219][ObtenerDescripcionServiciosAdicionales]", string.Empty), null, null);
            string strDescripcionServicio = string.Empty;
            string strCodigoRespuesta = string.Empty;
            string strMsjRespuesta = string.Empty;

            BLGeneral_II objGeneral = new BLGeneral_II();

            if (lstServiciosAdicionales != null && lstServiciosAdicionales.Count > 0)
            {
                foreach (var strServicio in lstServiciosAdicionales)
                {
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ObtenerDescripcionServiciosAdicionales][INPUT][PI_PO_ID]", Funciones.CheckStr(strServicio)), null, null);

                    objGeneral.ObtenerDescripcionServiciosAdic(strServicio, ref strDescripcionServicio, ref strCodigoRespuesta, ref strMsjRespuesta);

                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ObtenerDescripcionServiciosAdicionales][INPUT][PO_PLAN_DESCRIPCION]", strDescripcionServicio), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ObtenerDescripcionServiciosAdicionales][INPUT][PO_CODIGO_RESPUESTA]", strCodigoRespuesta), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ObtenerDescripcionServiciosAdicionales][INPUT][PO_MENSAJE_RESPUESTA]", strMsjRespuesta), null, null);

                    if (!string.IsNullOrEmpty(strDescripcionServicio))
                    {
                        strProducto = string.Format("{0}{1}{2}", strProducto, "+", strDescripcionServicio);
                    }
                }
            }
            else
            {
                objLog.CrearArchivolog("[INICIATIVA-219][ObtenerDescripcionServiciosAdicionales][No tiene servicios adicionales]", null, null);
            }
        }
        #endregion

        #region Integracion cabecera de BSCS con CBIO
        public void DatosTableCabecera(BEDetalleLinea_CBIO objDetalleLinea, ref DataTable dtResumen)
        {
            if (dtResumen == null || dtResumen.Rows.Count == 0)
            {
                dtResumen = new DataTable();

                if (objDetalleLinea != null)
                {
                    dtResumen = EstructuraTableCabeceraBSCS();

                    DataRow rowC = dtResumen.NewRow();
                    rowC["CUSTOMER_ID"] = Funciones.CheckStr(objDetalleLinea.C_CUSTOMER_ID);
                    rowC["PLANES"] = Funciones.CheckInt(objDetalleLinea.PLANES);
                    rowC["CF"] = Funciones.CheckDbl(objDetalleLinea.CF); // No se usa
                    rowC["BLOQ"] = Funciones.CheckInt(objDetalleLinea.BLOQ);
                    rowC["SUSP"] = Funciones.CheckInt(objDetalleLinea.SUSP);
                    rowC["DEUV"] = Funciones.CheckInt(objDetalleLinea.DEUV); // No se usa
                    rowC["DEUC"] = Funciones.CheckInt(objDetalleLinea.DEUC); // No se usa
                    rowC["RAZON_SOCIAL"] = Funciones.CheckStr(objDetalleLinea.RAZON_SOCIAL);
                    rowC["NOMBRES"] = Funciones.CheckStr(objDetalleLinea.NOMBRES);
                    rowC["APELLIDOS"] = Funciones.CheckStr(objDetalleLinea.APELLIDOS);
                    rowC["DIAS_DEUDA"] = Funciones.CheckInt(objDetalleLinea.DIAS_DEUDA); // No se usa
                    rowC["PROM_FACT"] = Funciones.CheckDbl(objDetalleLinea.C_PROM_FACT);
                    rowC["MONTO_NO_FACT"] = Funciones.CheckDbl(objDetalleLinea.C_MONTO_NO_FACT);
                    rowC["NRO_7"] = Funciones.CheckInt(objDetalleLinea.NRO_7);
                    rowC["NRO_30"] = Funciones.CheckInt(objDetalleLinea.NRO_30);
                    rowC["NRO_90"] = Funciones.CheckInt(objDetalleLinea.NRO_90);
                    rowC["NRO_90_MAS"] = Funciones.CheckInt(objDetalleLinea.NRO_90_MAS);
                    rowC["NRO_180"] = Funciones.CheckInt(objDetalleLinea.NRO_180);
                    rowC["NRO_180_MAS"] = Funciones.CheckInt(objDetalleLinea.NRO_180_MAS);
                    rowC["AFIL_RECIBO_X_CORREO"] = string.Empty; // No se usa
                    rowC["EMAIL"] = string.Empty; // No se usa
                    dtResumen.Rows.Add(rowC);
                }
            }
            else
            {
                if (objDetalleLinea != null && (objDetalleLinea.objListaDetalle != null && objDetalleLinea.objListaDetalle.Count > 0))
                {
                    dtResumen.Rows[0]["PROM_FACT"] = Funciones.CheckDbl(dtResumen.Rows[0]["PROM_FACT"]) + Funciones.CheckDbl(objDetalleLinea.C_PROM_FACT);
                    dtResumen.Rows[0]["MONTO_NO_FACT"] = Funciones.CheckDbl(dtResumen.Rows[0]["MONTO_NO_FACT"]) + Funciones.CheckDbl(objDetalleLinea.C_MONTO_NO_FACT);
                    dtResumen.Rows[0]["PLANES"] = Funciones.CheckInt(dtResumen.Rows[0]["PLANES"]) + Funciones.CheckInt(objDetalleLinea.PLANES);
                    dtResumen.Rows[0]["BLOQ"] = Funciones.CheckInt(dtResumen.Rows[0]["BLOQ"]) + Funciones.CheckInt(objDetalleLinea.BLOQ);
                    dtResumen.Rows[0]["SUSP"] = Funciones.CheckInt(dtResumen.Rows[0]["SUSP"]) + Funciones.CheckInt(objDetalleLinea.SUSP);
                    dtResumen.Rows[0]["NRO_7"] = Funciones.CheckInt(dtResumen.Rows[0]["NRO_7"]) + Funciones.CheckInt(objDetalleLinea.NRO_7);
                    dtResumen.Rows[0]["NRO_30"] = Funciones.CheckInt(dtResumen.Rows[0]["NRO_30"]) + Funciones.CheckInt(objDetalleLinea.NRO_30);
                    dtResumen.Rows[0]["NRO_90"] = Funciones.CheckInt(dtResumen.Rows[0]["NRO_90"]) + Funciones.CheckInt(objDetalleLinea.NRO_90);
                    dtResumen.Rows[0]["NRO_90_MAS"] = Funciones.CheckInt(dtResumen.Rows[0]["NRO_90_MAS"]) + Funciones.CheckInt(objDetalleLinea.NRO_90_MAS);
                    dtResumen.Rows[0]["NRO_180"] = Funciones.CheckInt(dtResumen.Rows[0]["NRO_180"]) + Funciones.CheckInt(objDetalleLinea.NRO_180);
                    dtResumen.Rows[0]["NRO_180_MAS"] = Funciones.CheckInt(dtResumen.Rows[0]["NRO_180_MAS"]) + Funciones.CheckInt(objDetalleLinea.NRO_180_MAS);
                }
            }
        }
        #endregion

        #region Integracion detalle de BSCS con CBIO
        public void DatosTableDetalle(BEDetalleLinea_CBIO objDetalleLinea, ref DataTable dtDetalleBSCS)
        {
            GeneradorLog objLog = new GeneradorLog(null, string.Format("{0}{1}", "[INICIO][INICIATIVA-219][DatosTableDetalle]", string.Empty), null, null);
            bool blExisteCBIO = false;
            DataTable dtDetalleCBIO = new DataTable();

            if (objDetalleLinea != null && (objDetalleLinea.objListaDetalle != null && objDetalleLinea.objListaDetalle.Count > 0))
            {
                dtDetalleCBIO = EstructuraTableDetalleBSCS();

                foreach (var objLista in objDetalleLinea.objListaDetalle)
                {
                    DataRow rowD = dtDetalleCBIO.NewRow();
                    rowD["CUSTOMER_ID"] = Funciones.CheckStr(objLista.D_CUSTOMER_ID);
                    rowD["CUENTA"] = Funciones.CheckStr(objLista.CUENTA);
                    rowD["CO_ID"] = Funciones.CheckDecimal(objLista.CO_ID);
                    rowD["TMCODE"] = Funciones.CheckDecimal(objLista.TM_CODE);
                    rowD["PLAN"] = Funciones.CheckStr(objLista.PLAN);
                    rowD["PRODUCTO/SERVICIO"] = Funciones.CheckStr(objLista.PRODUCTO_SERVICIO);
                    rowD["NUMERO"] = Funciones.CheckStr(objLista.NUMERO);
                    rowD["CF_CONTRATO"] = Funciones.CheckDecimal(objLista.CF_CONTRATO);
                    rowD["PROM_FACT"] = Funciones.CheckDecimal(objLista.D_PROM_FACT);
                    rowD["FECHA_ACTIVACION"] = Funciones.CheckStr(objLista.FECHA_ACTIVACION);
                    rowD["ESTADO"] = Funciones.CheckStr(objLista.ESTADO);
                    rowD["COD_BLOQ"] = Funciones.CheckStr(objLista.COD_BLOQ);
                    rowD["COD_SUSP"] = Funciones.CheckStr(objLista.COD_SUSP);
                    rowD["MOT_BLOQ"] = Funciones.CheckStr(objLista.MOT_BLOQ);
                    rowD["MOT_SUSP"] = Funciones.CheckStr(objLista.MOT_SUSP);
                    rowD["FECHA_ESTADO"] = Funciones.CheckStr(objLista.FECHA_ESTADO);
                    rowD["MONTO_VENCIDO"] = Funciones.CheckDecimal(objLista.MONTO_VENCIDO);
                    rowD["MONTO_CASTIGO"] = Funciones.CheckDecimal(objLista.MONTO_CASTIGADO);
                    rowD["CAMPANA"] = Funciones.CheckStr(objLista.CAMPANA);
                    rowD["MONTO_NO_FACT"] = Funciones.CheckDecimal(objLista.D_MONTO_NO_FACT);
                    rowD["NRO_MOVIL"] = Funciones.CheckInt(objLista.NRO_MOVIL);
                    rowD["NRO_INTERNET_FIJO"] = Funciones.CheckInt(objLista.NRO_INTERNET_FIJO);
                    rowD["NRO_CLARO_TV"] = Funciones.CheckInt(objLista.NRO_CLARO_TV);
                    rowD["NRO_TELEF_FIJA"] = Funciones.CheckInt(objLista.NRO_TELEF_FIJA);
                    rowD["NRO_BAM"] = Funciones.CheckInt(objLista.NRO_BAM);
                    rowD["NRO_BLOQ"] = Funciones.CheckInt(objLista.NRO_BLOQ);
                    rowD["NRO_SUSP"] = Funciones.CheckInt(objLista.NRO_SUSP);
                    rowD["DEUDA_REINT_EQUIPO"] = Funciones.CheckDecimal(objLista.DEUDA_REINT_EQUIPO);
                    rowD["NRO_PLANES"] = Funciones.CheckInt(objLista.NRO_PLANES);
                    dtDetalleCBIO.Rows.Add(rowD);

                    blExisteCBIO = true;
                    objLog.CrearArchivolog("[INICIATIVA-219][DatosTableDetalle][CBIO]**************************************************************************************", null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][DatosTableDetalle][CBIO][CUSTOMER_ID]", Funciones.CheckStr(objLista.D_CUSTOMER_ID)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][DatosTableDetalle][CBIO][CUENTA]", Funciones.CheckStr(objLista.CUENTA)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][DatosTableDetalle][CBIO][CO_ID]", Funciones.CheckDecimal(objLista.CO_ID)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][DatosTableDetalle][CBIO][TMCODE]", Funciones.CheckDecimal(objLista.TM_CODE)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][DatosTableDetalle][CBIO][PLAN]", Funciones.CheckStr(objLista.PLAN)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][DatosTableDetalle][CBIO][PRODUCTO/SERVICIO]", Funciones.CheckStr(objLista.PRODUCTO_SERVICIO)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][DatosTableDetalle][CBIO][NUMERO]", Funciones.CheckStr(objLista.NUMERO)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][DatosTableDetalle][CBIO][CF_CONTRATO]", Funciones.CheckDecimal(objLista.CF_CONTRATO)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][DatosTableDetalle][CBIO][PROM_FACT]", Funciones.CheckDecimal(objLista.D_PROM_FACT)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][DatosTableDetalle][CBIO][FECHA_ACTIVACION]", Funciones.CheckStr(objLista.FECHA_ACTIVACION)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][DatosTableDetalle][CBIO][ESTADO]", Funciones.CheckStr(objLista.ESTADO)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][DatosTableDetalle][CBIO][COD_BLOQ]", Funciones.CheckStr(objLista.COD_BLOQ)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][DatosTableDetalle][CBIO][COD_SUSP]", Funciones.CheckStr(objLista.COD_SUSP)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][DatosTableDetalle][CBIO][MOT_BLOQ]", Funciones.CheckStr(objLista.MOT_BLOQ)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][DatosTableDetalle][CBIO][MOT_SUSP]", Funciones.CheckStr(objLista.MOT_SUSP)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][DatosTableDetalle][CBIO][FECHA_ESTADO]", Funciones.CheckStr(objLista.FECHA_ESTADO)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][DatosTableDetalle][CBIO][MONTO_VENCIDO]", Funciones.CheckDecimal(objLista.MONTO_VENCIDO)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][DatosTableDetalle][CBIO][MONTO_CASTIGO]", Funciones.CheckDecimal(objLista.MONTO_CASTIGADO)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][DatosTableDetalle][CBIO][CAMPANA]", Funciones.CheckStr(objLista.CAMPANA)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][DatosTableDetalle][CBIO][MONTO_NO_FACT]", Funciones.CheckDecimal(objLista.D_MONTO_NO_FACT)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][DatosTableDetalle][CBIO][NRO_MOVIL]", Funciones.CheckInt(objLista.NRO_MOVIL)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][DatosTableDetalle][CBIO][NRO_INTERNET_FIJO]", Funciones.CheckInt(objLista.NRO_INTERNET_FIJO)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][DatosTableDetalle][CBIO][NRO_CLARO_TV]", Funciones.CheckInt(objLista.NRO_CLARO_TV)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][DatosTableDetalle][CBIO][NRO_TELEF_FIJA]", Funciones.CheckInt(objLista.NRO_TELEF_FIJA)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][DatosTableDetalle][CBIO][NRO_BAM]", Funciones.CheckInt(objLista.NRO_BAM)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][DatosTableDetalle][CBIO][NRO_BLOQ]", Funciones.CheckInt(objLista.NRO_BLOQ)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][DatosTableDetalle][CBIO][NRO_SUSP]", Funciones.CheckInt(objLista.NRO_SUSP)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][DatosTableDetalle][CBIO][DEUDA_REINT_EQUIPO]", Funciones.CheckDecimal(objLista.DEUDA_REINT_EQUIPO)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][DatosTableDetalle][CBIO][NRO_PLANES]", Funciones.CheckInt(objLista.NRO_PLANES)), null, null);
                    objLog.CrearArchivolog("[INICIATIVA-219][DatosTableDetalle][CBIO]**************************************************************************************", null, null);
                }
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][objDetalleLineaCBIO.Count]", Funciones.CheckStr(objDetalleLinea.objListaDetalle.Count)), null, null);

                objLog.CrearArchivolog("[INICIATIVA-219][DatosTableDetalle][CBIO][dtDetalleCBIO no es vacio]", null, null);
            }
            if (dtDetalleBSCS == null || dtDetalleBSCS.Rows.Count == 0)
            {
                objLog.CrearArchivolog("[INICIATIVA-219][DatosTableDetalle][BSCS][dtDetalleBSCS es vacio]", null, null);
                dtDetalleBSCS = dtDetalleCBIO;
            }
            else if (blExisteCBIO)
            {
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][dtDetalleBSCS.Count]", Funciones.CheckStr(dtDetalleBSCS.Rows.Count)), null, null);
                dtDetalleBSCS.Merge(dtDetalleCBIO, true, MissingSchemaAction.Ignore);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][DatosTableDetalle][CBIO][blResultado]", blExisteCBIO), null, null);
                objLog.CrearArchivolog("[INICIATIVA-219][DatosTableDetalle][BSCS][CBIO][Se unifico el dtDetalle de BSCS con CBIO]", null, null);
            }
            else
            {
                objLog.CrearArchivolog("[INICIATIVA-219][DatosTableDetalle][BSCS][dtDetalleBSCS es vacio]", null, null);
                objLog.CrearArchivolog("[INICIATIVA-219][DatosTableDetalle][CBIO][dtDetalleCBIO es vacio]", null, null);
            }
            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][FINAL][DatosTableDetalle.Count]", Funciones.CheckStr(dtDetalleBSCS.Rows.Count)), null, null);
            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[FIN][INICIATIVA-219][DatosTableDetalle]", string.Empty), null, null);
        }
        #endregion

        #region ListarDetalleLineaFraudeCBIO
        public void ListarDetalleLineaFraudeCBIO(string strFraude, ref DataTable dtListaFraude)
        {
            bool blExisteCBIO = false;
            DataTable dtListaFraudeCBIO = new DataTable();

            if (!string.IsNullOrEmpty(strFraude))
            {
                dtListaFraudeCBIO = EstructuraTableFraudeBSCS();
                string[] arrFraude = strFraude.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var objFraude in arrFraude)
                {
                    var arr = objFraude.Split(',');
                    DataRow row = dtListaFraudeCBIO.NewRow();
                    row["NRO_CUENTA"] = Funciones.CheckStr(arr[0]);
                    row["NRO_CONTRATO"] = 0.0; // No se usa
                    row["DEUDA_VENCIDA_CUENTA"] = 0.0; // No se usa
                    row["NRO_LINEA"] = string.Empty; // No se usa
                    row["FECHA_ACTIVACION"] = DateTime.Now; // No se usa
                    row["ESTADO_LINEA"] = string.Empty; ; // No se usa
                    row["MOTIVO_ESTADO"] = string.Empty; // No se usa
                    row["TIPO_ESTADO"] = Funciones.CheckStr(arr[1]); // Se usa
                    row["FECHA_ESTADO"] = DateTime.Now; // No se usa
                    row["BLOQUEO"] = string.Empty; ; // No se usa
                    row["CODIGO_BLOQUEO"] = string.Empty; // No se usa
                    row["FECHA_BLOQUEO"] = string.Empty; // No se usa
                    dtListaFraudeCBIO.Rows.Add(row);
                }
            }
            if (dtListaFraude == null || dtListaFraude.Rows.Count == 0)
            {
                dtListaFraude = dtListaFraudeCBIO;
            }
            else if (blExisteCBIO)
            {
                dtListaFraude.Merge(dtListaFraudeCBIO, true, MissingSchemaAction.Ignore);
            }
        }
        #endregion

        #region ListarCantPlanxBilleteraCBIO
        public void ListarCantPlanxBilleteraCBIO(ref DataTable dtListaCantPlanBSCS)
        {
            GeneradorLog objLog = new GeneradorLog(null, string.Format("{0}{1}", "[INICIO][INICIATIVA-219][ListarCantPlanxBilleteraCBIO]", string.Empty), null, null);
            DataTable dtListaCantPlanCBIO = new DataTable();
            BEDetalleLinea_CBIO objDetalleLineaCBIO = (BEDetalleLinea_CBIO)HttpContext.Current.Session["DetalleLineaCbio"];
            bool blExisteCBIO = false;

            if (objDetalleLineaCBIO != null)
            {
                if (objDetalleLineaCBIO.objListaDetalle != null && objDetalleLineaCBIO.objListaDetalle.Count > 0)
            {
                dtListaCantPlanCBIO = EstructuraTablePlanesXBilletera();

                    foreach (var objDetalle in objDetalleLineaCBIO.objListaDetalle)
                {
                    DataRow row = dtListaCantPlanCBIO.NewRow();
                    row["TMCODE"] = Funciones.CheckInt(objDetalle.TM_CODE);
                    row["NRO_MOVIL"] = Funciones.CheckInt(objDetalle.NRO_MOVIL);
                    row["NRO_INTERNET_FIJO"] = Funciones.CheckInt(objDetalle.NRO_INTERNET_FIJO);
                    row["NRO_CLARO_TV"] = Funciones.CheckInt(objDetalle.NRO_CLARO_TV);
                    row["NRO_TELEF_FIJA"] = Funciones.CheckInt(objDetalle.NRO_TELEF_FIJA);
                    row["NRO_BAM"] = Funciones.CheckInt(objDetalle.NRO_BAM);
                    dtListaCantPlanCBIO.Rows.Add(row);

                    blExisteCBIO = true;
                }
                }

                objLog.CrearArchivolog("[INICIATIVA-219][ListarCantPlanxBilleteraCBIO][CBIO][dtListaCantPlanCBIO no es vacio]", null, null);
            }
            if (dtListaCantPlanBSCS == null || dtListaCantPlanBSCS.Rows.Count == 0)
            {
                objLog.CrearArchivolog("[INICIATIVA-219][ListarCantPlanxBilleteraCBIO][BSCS][dtListaCantPlanBSCS es vacio]", null, null);
                dtListaCantPlanBSCS = dtListaCantPlanCBIO;
            }
            else if (blExisteCBIO)
            {
                dtListaCantPlanBSCS.Merge(dtListaCantPlanCBIO, true, MissingSchemaAction.Ignore);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-219][ListarCantPlanxBilleteraCBIO][CBIO][blResultado]", blExisteCBIO), null, null);
                objLog.CrearArchivolog("[INICIATIVA-219][ListarCantPlanxBilleteraCBIO][BSCS][CBIO][Se unifico las billeteras por el plan de BSCS con CBIO]", null, null);
            }
            else
            {
                objLog.CrearArchivolog("[INICIATIVA-219][ListarCantPlanxBilleteraCBIO][BSCS][dtListaCantPlanBSCS es vacio]", null, null);
                objLog.CrearArchivolog("[INICIATIVA-219][ListarCantPlanxBilleteraCBIO][CBIO][dtListaCantPlanCBIO es vacio]", null, null);
            }

            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[FIN][INICIATIVA-219][ListarCantPlanxBilleteraCBIO]", string.Empty), null, null);
        }
        #endregion

        #region EstructuraTableCabeceraBSCS
        public DataTable EstructuraTableCabeceraBSCS()
        {
            DataTable dtResumen = new DataTable();

            dtResumen.Columns.Add(new DataColumn("CUSTOMER_ID", System.Type.GetType("System.String")));
            dtResumen.Columns.Add(new DataColumn("PLANES", System.Type.GetType("System.String")));
            dtResumen.Columns.Add(new DataColumn("CF", System.Type.GetType("System.String")));
            dtResumen.Columns.Add(new DataColumn("BLOQ", System.Type.GetType("System.String")));
            dtResumen.Columns.Add(new DataColumn("SUSP", System.Type.GetType("System.String")));
            dtResumen.Columns.Add(new DataColumn("DEUV", System.Type.GetType("System.String")));
            dtResumen.Columns.Add(new DataColumn("DEUC", System.Type.GetType("System.String")));
            dtResumen.Columns.Add(new DataColumn("RAZON_SOCIAL", System.Type.GetType("System.String")));
            dtResumen.Columns.Add(new DataColumn("NOMBRES", System.Type.GetType("System.String")));
            dtResumen.Columns.Add(new DataColumn("APELLIDOS", System.Type.GetType("System.String")));
            dtResumen.Columns.Add(new DataColumn("DIAS_DEUDA", System.Type.GetType("System.String")));
            dtResumen.Columns.Add(new DataColumn("PROM_FACT", System.Type.GetType("System.String")));
            dtResumen.Columns.Add(new DataColumn("MONTO_NO_FACT", System.Type.GetType("System.String")));
            dtResumen.Columns.Add(new DataColumn("NRO_7", System.Type.GetType("System.String")));
            dtResumen.Columns.Add(new DataColumn("NRO_30", System.Type.GetType("System.String")));
            dtResumen.Columns.Add(new DataColumn("NRO_90", System.Type.GetType("System.String")));
            dtResumen.Columns.Add(new DataColumn("NRO_90_MAS", System.Type.GetType("System.String")));
            dtResumen.Columns.Add(new DataColumn("NRO_180", System.Type.GetType("System.String")));
            dtResumen.Columns.Add(new DataColumn("NRO_180_MAS", System.Type.GetType("System.String")));
            dtResumen.Columns.Add(new DataColumn("AFIL_RECIBO_X_CORREO", System.Type.GetType("System.String")));
            dtResumen.Columns.Add(new DataColumn("EMAIL", System.Type.GetType("System.String")));

            return dtResumen;
        }
        #endregion

        #region EstructuraTableDetalleBSCS
        public DataTable EstructuraTableDetalleBSCS()
        {
            DataTable dtDetalle = new DataTable();

            dtDetalle.Columns.Add(new DataColumn("CUSTOMER_ID", System.Type.GetType("System.String")));
            dtDetalle.Columns.Add(new DataColumn("CUENTA", System.Type.GetType("System.String")));
            dtDetalle.Columns.Add(new DataColumn("CO_ID", System.Type.GetType("System.String")));
            dtDetalle.Columns.Add(new DataColumn("TMCODE", System.Type.GetType("System.String")));
            dtDetalle.Columns.Add(new DataColumn("PLAN", System.Type.GetType("System.String")));
            dtDetalle.Columns.Add(new DataColumn("PRODUCTO/SERVICIO", System.Type.GetType("System.String")));
            dtDetalle.Columns.Add(new DataColumn("NUMERO", System.Type.GetType("System.String")));
            dtDetalle.Columns.Add(new DataColumn("CF_CONTRATO", System.Type.GetType("System.String")));
            dtDetalle.Columns.Add(new DataColumn("PROM_FACT", System.Type.GetType("System.String")));
            dtDetalle.Columns.Add(new DataColumn("FECHA_ACTIVACION", System.Type.GetType("System.String")));
            dtDetalle.Columns.Add(new DataColumn("ESTADO", System.Type.GetType("System.String")));
            dtDetalle.Columns.Add(new DataColumn("COD_BLOQ", System.Type.GetType("System.String")));
            dtDetalle.Columns.Add(new DataColumn("COD_SUSP", System.Type.GetType("System.String")));
            dtDetalle.Columns.Add(new DataColumn("MOT_BLOQ", System.Type.GetType("System.String")));
            dtDetalle.Columns.Add(new DataColumn("MOT_SUSP", System.Type.GetType("System.String")));
            dtDetalle.Columns.Add(new DataColumn("FECHA_ESTADO", System.Type.GetType("System.String")));
            dtDetalle.Columns.Add(new DataColumn("MONTO_VENCIDO", System.Type.GetType("System.String")));
            dtDetalle.Columns.Add(new DataColumn("MONTO_CASTIGO", System.Type.GetType("System.String")));
            dtDetalle.Columns.Add(new DataColumn("CAMPANA", System.Type.GetType("System.String")));
            dtDetalle.Columns.Add(new DataColumn("MONTO_NO_FACT", System.Type.GetType("System.String")));
            dtDetalle.Columns.Add(new DataColumn("NRO_MOVIL", System.Type.GetType("System.String")));
            dtDetalle.Columns.Add(new DataColumn("NRO_INTERNET_FIJO", System.Type.GetType("System.String")));
            dtDetalle.Columns.Add(new DataColumn("NRO_CLARO_TV", System.Type.GetType("System.String")));
            dtDetalle.Columns.Add(new DataColumn("NRO_TELEF_FIJA", System.Type.GetType("System.String")));
            dtDetalle.Columns.Add(new DataColumn("NRO_BAM", System.Type.GetType("System.String")));
            dtDetalle.Columns.Add(new DataColumn("NRO_BLOQ", System.Type.GetType("System.String")));
            dtDetalle.Columns.Add(new DataColumn("NRO_SUSP", System.Type.GetType("System.String")));
            dtDetalle.Columns.Add(new DataColumn("DEUDA_REINT_EQUIPO", System.Type.GetType("System.String")));
            dtDetalle.Columns.Add(new DataColumn("NRO_PLANES", System.Type.GetType("System.String")));

            return dtDetalle;
        }
        #endregion

        #region EstructuraTableFraudeBSCS
        public DataTable EstructuraTableFraudeBSCS()
        {
            DataTable dtFraude = new DataTable();

            dtFraude.Columns.Add(new DataColumn("NRO_CUENTA", System.Type.GetType("System.String")));
            dtFraude.Columns.Add(new DataColumn("NRO_CONTRATO", System.Type.GetType("System.String")));
            dtFraude.Columns.Add(new DataColumn("DEUDA_VENCIDA_CUENTA", System.Type.GetType("System.String")));
            dtFraude.Columns.Add(new DataColumn("NRO_LINEA", System.Type.GetType("System.String")));
            dtFraude.Columns.Add(new DataColumn("FECHA_ACTIVACION", System.Type.GetType("System.String")));
            dtFraude.Columns.Add(new DataColumn("ESTADO_LINEA", System.Type.GetType("System.String")));
            dtFraude.Columns.Add(new DataColumn("MOTIVO_ESTADO", System.Type.GetType("System.String")));
            dtFraude.Columns.Add(new DataColumn("TIPO_ESTADO", System.Type.GetType("System.String")));
            dtFraude.Columns.Add(new DataColumn("FECHA_ESTADO", System.Type.GetType("System.String")));
            dtFraude.Columns.Add(new DataColumn("BLOQUEO", System.Type.GetType("System.String")));
            dtFraude.Columns.Add(new DataColumn("CODIGO_BLOQUEO", System.Type.GetType("System.String")));
            dtFraude.Columns.Add(new DataColumn("FECHA_BLOQUEO", System.Type.GetType("System.String")));

            return dtFraude;
        }
        #endregion

        #region EstructuraTablePlanesXBilletera
        public DataTable EstructuraTablePlanesXBilletera()
        {
            DataTable dtPlanesXBilletera = new DataTable();

            dtPlanesXBilletera.Columns.Add(new DataColumn("TMCODE", System.Type.GetType("System.String")));
            dtPlanesXBilletera.Columns.Add(new DataColumn("NRO_MOVIL", System.Type.GetType("System.String")));
            dtPlanesXBilletera.Columns.Add(new DataColumn("NRO_INTERNET_FIJO", System.Type.GetType("System.String")));
            dtPlanesXBilletera.Columns.Add(new DataColumn("NRO_CLARO_TV", System.Type.GetType("System.String")));
            dtPlanesXBilletera.Columns.Add(new DataColumn("NRO_TELEF_FIJA", System.Type.GetType("System.String")));
            dtPlanesXBilletera.Columns.Add(new DataColumn("NRO_BAM", System.Type.GetType("System.String")));

            return dtPlanesXBilletera;
        }
        #endregion

        #region ObtenerDatosParticipanteMostrar
        public Participante ObtenerDatosParticipanteMostrar(string strTipoDocumento, string strNumeroDocumento, string strIdentificador)
        {
            GeneradorLog objLog = new GeneradorLog(null, string.Format("{0}{1}", "[INICIO][INICIATIVA-219][ObtenerDatosParticipanteMostrar]", string.Empty), null, null);
            BuscarCuParticipanteRequest objRequest = new BuscarCuParticipanteRequest();
            BuscarCuParticipanteResponse objResponse = new BuscarCuParticipanteResponse();
            BWCuParticipanteCBIO objWSParticipante = new BWCuParticipanteCBIO();
            Participante objParticipanteResponse = new Participante();
            
            try
            {
                string strTipoDocCBIO = string.Empty;
                strTipoDocCBIO = ObtenerTipoDocumentoCBIO(strTipoDocumento);

                objRequest.tipoDocumento = strTipoDocCBIO;
                objRequest.numeroDocumento = strNumeroDocumento;

                objResponse = objWSParticipante.ConsultarParticipanteWSCBIO(objRequest);

                if (objResponse != null && (objResponse.participante != null && objResponse.participante.Length > 0))
                {
                    objParticipanteResponse.nombre = Funciones.CheckStr(objResponse.participante.FirstOrDefault().nombre);
                    objParticipanteResponse.apellidoPaterno = Funciones.CheckStr(objResponse.participante.FirstOrDefault().apellidoPaterno);
                    objParticipanteResponse.apellidoMaterno = Funciones.CheckStr(objResponse.participante.FirstOrDefault().apellidoMaterno);
                    objParticipanteResponse.razonSocial = Funciones.CheckStr(objResponse.participante.FirstOrDefault().razonSocial);

                    objLog.CrearArchivolog(string.Format("{0}[{1}]{2}-->{3}", "[INICIATIVA-219][ObtenerDatosParticipanteMostrar]", strIdentificador, "[nombre]", objParticipanteResponse.nombre), null, null);
                    objLog.CrearArchivolog(string.Format("{0}[{1}]{2}-->{3}", "[INICIATIVA-219][ObtenerDatosParticipanteMostrar]", strIdentificador, "[apellidoPaterno]", objParticipanteResponse.apellidoPaterno), null, null);
                    objLog.CrearArchivolog(string.Format("{0}[{1}]{2}-->{3}", "[INICIATIVA-219][ObtenerDatosParticipanteMostrar]", strIdentificador, "[apellidoMaterno]", objParticipanteResponse.apellidoMaterno), null, null);
                    objLog.CrearArchivolog(string.Format("{0}[{1}]{2}-->{3}", "[INICIATIVA-219][ObtenerDatosParticipanteMostrar]", strIdentificador, "[razonSocial]", objParticipanteResponse.razonSocial), null, null);

                }
                else
                {
                    objLog.CrearArchivolog(string.Format("{0}[{1}]-->{2}", "[INICIATIVA-219][ObtenerDatosParticipanteMostrar]", Funciones.CheckStr(strIdentificador), "No se encontraron datos al consultar el participante en CBIO"), null, null);

                }
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog(string.Format("{0}[{1}]-->{2}", "[INICIATIVA-219][ObtenerDatosParticipanteMostrar]", Funciones.CheckStr(strIdentificador), "Ocurrio un error al consultar datos del participante para mostrar"), null, null);
                objLog.CrearArchivolog(string.Format("{0} {1} | {2}", "[INICIATIVA-219][ObtenerDatosParticipanteMostrar][ERROR]", Funciones.CheckStr(ex.Message), Funciones.CheckStr(ex.StackTrace)), null, null);
            }

            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[FIN][INICIATIVA-219][ObtenerDatosParticipanteMostrar]", string.Empty), null, null);

            return objParticipanteResponse;
        }
        #endregion

        public double CalcularCF_N_MesesCBIO(string strNumeroDocumento, int intConsMesesRecurrente, int tipo)
        {
            double dblCF = 0;

            try
            {
                double dblCargoFijo = 0;

                BEDetalleLinea_CBIO objDetalleLineaCBIO = (BEDetalleLinea_CBIO)HttpContext.Current.Session["DetalleLineaCbio"];

                if (objDetalleLineaCBIO != null)
                {
                    if (objDetalleLineaCBIO.C_CUSTOMER_ID == strNumeroDocumento)
                    {
                        if (objDetalleLineaCBIO.objListaDetalle != null && objDetalleLineaCBIO.objListaDetalle.Count > 0)
                        {
                            foreach (BEDetalleLinea_CBIO_D objDetalle in objDetalleLineaCBIO.objListaDetalle)
                            {
                                DateTime dtFechaActivacion = Funciones.CheckDate(objDetalle.FECHA_ACTIVACION);
                                DateTime dtMonthsBefore = DateTime.Now.AddMonths(-intConsMesesRecurrente);
                                dblCargoFijo = Funciones.CheckDbl(objDetalle.CF_CONTRATO, 2);

                                if (objDetalle.ESTADO == "Activo" && dtFechaActivacion > dtMonthsBefore && tipo == 1)
                                {
                                    dblCF = dblCF + Funciones.CheckDbl(dblCargoFijo);
                                }
                                else if (objDetalle.ESTADO == "Activo" && dtFechaActivacion < dtMonthsBefore && tipo == 2)
                                {
                                    dblCF = dblCF + Funciones.CheckDbl(dblCargoFijo);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                dblCF = 0;
            }

            return dblCF;
        }
        #endregion

        public BEItemGenerico consultaDatosLinea(BEAuditoriaRequest oAuditoria, string msisdn, ref string mensajeError)
        {
            BodyRequestDatosLinea requestDatosLinea = new BodyRequestDatosLinea();
            BEItemGenerico itemDatos = null;

            requestDatosLinea.datosLineaRequest.msisdn = msisdn;
            requestDatosLinea.datosLineaRequest.listaOpcional = null;

            try
            {
                BodyResponseDatosLinea responseDatosLinea = new BodyResponseDatosLinea();
                responseDatosLinea = RestServiceDPGeneral.PostInvoque<BodyResponseDatosLinea>(requestDatosLinea, oAuditoria);

                if (!Object.Equals(responseDatosLinea, null))
                {
                    if (responseDatosLinea.datosLineaResponse.responseAudit.codigoRespuesta.Trim() == "0")
                    {
                        itemDatos = new BEItemGenerico();
                        itemDatos.Descripcion = responseDatosLinea.datosLineaResponse.responseData.cliente.desplanTarifario;
                        itemDatos.Descripcion2 = responseDatosLinea.datosLineaResponse.responseData.cliente.descEstado;
                        mensajeError = "PostPago";
                    }
                    else
                    {
                        mensajeError = "No PostPago";
                    }
                }
            }
            catch (Exception ex)
            {
                mensajeError = ex.Message.ToString();
            }

            return itemDatos;
        }
    }
}


