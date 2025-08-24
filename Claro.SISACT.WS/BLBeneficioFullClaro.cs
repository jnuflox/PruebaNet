using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity.ConsultarClienteFullClaroRest;
using Claro.SISACT.Entity;
using Claro.SISACT.WS.RestReferences;
using System.Configuration;
using Claro.SISACT.Common;
using Claro.SISACT.Business;
using System.Web;
using System.Collections;
using Claro.SISACT.Entity.claro_vent_fullclaroCBIO;
using Claro.SISACT.WS.RestServices;
using Claro.SISACT.Entity.ValidarProductosFCRest;
using Claro.SISACT.WS.BWServicesCBIO;
using Claro.SISACT.Entity.claro_vent_fullclaroCBIO.consultarActivosCBIO;
using Claro.SISACT.Entity.claro_vent_fullclaroCBIO.consultarPendientesActivasCBIO;

namespace Claro.SISACT.WS
{
    public class BLBeneficioFullClaro
    {
        private static string nameLogCBIO = "LogCBIO-BLBeneficioFullClaro";

        public ConsultarClientesFullClaroResponse ConsultarClienteFullClaro(ConsultarClientesFullClaroRequest objConsultarClientesFullClaroRequest, BEAuditoriaRequest objBEAuditoriaRequest, String tipoDocumentoONE)
        {
            RestConsultarClientesFullClaro objRestBSCS = new RestConsultarClientesFullClaro();
            ConsultarClientesFullClaroResponse objValidarCandidatoFCResponse = new ConsultarClientesFullClaroResponse();
            ConsultarClientesFullClaroResponse objValidarCandidatoFCResponseBSCS = new ConsultarClientesFullClaroResponse();
            ConsultarClientesFullClaroResponse objValidarCandidatoFCResponseCBIO = new ConsultarClientesFullClaroResponse();

            //ASIS - BSCS7
            HelperLog.EscribirLog(string.Empty, nameLogCBIO, string.Format("{0}-->{1}", "[INICIATIVA-710][ConsultarClienteFullClaro][INICIO]", "OBTENER DATOS DE ASIS - BSCS7"), false);
            HelperLog.EscribirLog(string.Empty, nameLogCBIO, string.Format("{0} --> {1}", "[FULL_CLARO - Servicio Consulta Cliente Fullclaro en BSCS7][tipoDocumento][ASIS]", Funciones.CheckStr(objConsultarClientesFullClaroRequest.MessageRequest.body.tipoDocumento)), false);
            HelperLog.EscribirLog(string.Empty, nameLogCBIO, string.Format("{0} --> {1}", "[FULL_CLARO - Servicio Consulta Cliente Fullclaro en BSCS7][nroDocumento]", Funciones.CheckStr(objConsultarClientesFullClaroRequest.MessageRequest.body.nroDocumento)), false);
            objValidarCandidatoFCResponseBSCS = objRestBSCS.ConsultarClienteFullClaro(objConsultarClientesFullClaroRequest, objBEAuditoriaRequest);

            //TOBE - BSCS9(CBIO)
            HelperLog.EscribirLog(string.Empty, nameLogCBIO, string.Format("{0}-->{1}", "[INICIATIVA-710][ConsultarClienteFullClaro][INICIO]", "OBTENER DATOS DE TOBE - BSCS9"), false);
            HelperLog.EscribirLog(string.Empty, nameLogCBIO, string.Format("{0} --> {1}", "[FULL_CLARO - Servicio Consulta Cliente Fullclaro en BSCS9][tipoDocumento][ONE]", Funciones.CheckStr(tipoDocumentoONE)), false);
            HelperLog.EscribirLog(string.Empty, nameLogCBIO, string.Format("{0} --> {1}", "[FULL_CLARO - Servicio Consulta Cliente Fullclaro en BSCS9][nroDocumento]", Funciones.CheckStr(objConsultarClientesFullClaroRequest.MessageRequest.body.nroDocumento)), false);
            objValidarCandidatoFCResponseCBIO = ConsultarClienteFC_CBIO(objConsultarClientesFullClaroRequest, tipoDocumentoONE);

            objValidarCandidatoFCResponse = ValidarClienteFC(objValidarCandidatoFCResponseBSCS, objValidarCandidatoFCResponseCBIO);

            return objValidarCandidatoFCResponse;
        }

        public ConsultarClientesFullClaroResponse ConsultarClienteFC_CBIO(ConsultarClientesFullClaroRequest objConsultarClientesFullClaroRequest, string tipoDocumentoONE)
        {
            ConsultarClientesFullClaroResponse objValidarCandidatoFCResponse = new ConsultarClientesFullClaroResponse();
            List<BEDatosClienteFC> lstClienteFC = new List<BEDatosClienteFC>();
            ConsultarActivosCBIOResponse objResponse = new ConsultarActivosCBIOResponse();
            Request objRequest = new Request();
            BWFullClaroCBIO objWSFullClaro = new BWFullClaroCBIO();

            objRequest.consultarActivosCBIORequest.tipoDocumento = Funciones.CheckStr(tipoDocumentoONE);
            objRequest.consultarActivosCBIORequest.numeroDocumento = Funciones.CheckStr(objConsultarClientesFullClaroRequest.MessageRequest.body.nroDocumento);

            objResponse = objWSFullClaro.ValidarCandidatoFullClaroWSCBIO(objRequest);

            if ((objResponse != null && objResponse.responseData != null))
            {
                if (objResponse.responseData.cursorClienteFC != null && objResponse.responseData.cursorClienteFC.Count > 0)
                {
                    //Lista de lineas activas del cliente que puedan aplicar el Beneficio FC
                    foreach (var objOnHold in objResponse.responseData.cursorClienteFC)
                    {
                        BEDatosClienteFC objClienteFC = new BEDatosClienteFC();
                        objClienteFC.coId = objOnHold.coId;
                        objClienteFC.linea = objOnHold.linea;
                        objClienteFC.tipoServicio = objOnHold.tipoServicio;
                        objClienteFC.tmCode = objOnHold.codProducto;
                        objClienteFC.desTmcode = objOnHold.beneficio;
                        objClienteFC.customerId = objOnHold.customerId;

                        lstClienteFC.Add(objClienteFC);
                    }
                }
            }
            objValidarCandidatoFCResponse.MessageResponse.body.dataRespuesta = lstClienteFC;
            objValidarCandidatoFCResponse.MessageResponse.body.codigoRespuesta = objResponse.codigoRespuesta;
            objValidarCandidatoFCResponse.MessageResponse.body.mensajeRespuesta = objResponse.mensajeRespuesta;

            return objValidarCandidatoFCResponse;
        }

        public ConsultarClientesFullClaroResponse ValidarClienteFC(ConsultarClientesFullClaroResponse objValidarCandidatoFCResponseBSCS, ConsultarClientesFullClaroResponse objValidarCandidatoFCResponseCBIO)
        {
            GeneradorLog objLog = new GeneradorLog(null, string.Format("{0}{1}", "[INICIO][INICIATIVA-710][ValidarClienteFC]", string.Empty), null, null);
            ConsultarClientesFullClaroResponse objResponse = new ConsultarClientesFullClaroResponse();
            List<BEDatosClienteFC> lstClienteFCBSCS = new List<BEDatosClienteFC>();
            List<BEDatosClienteFC> lstClienteFCCBIO = new List<BEDatosClienteFC>();
            string strCodigoRespuestaBSCS = string.Empty;
            string strCodigoRespuestaCBIO = string.Empty;
            string strMensajeRespuestaBSCS = string.Empty;
            string strMensajeRespuestaCBIO = string.Empty;
            string strCodigoFinal = string.Empty;
            string strMensajeFinal = string.Empty;
            string tipoServicioFCBSCS = string.Empty;
            string tipoServicioFCCBIO = string.Empty;

            try
            {
                strCodigoRespuestaBSCS = Funciones.CheckStr(objValidarCandidatoFCResponseBSCS.MessageResponse.body.codigoRespuesta);
                strCodigoRespuestaCBIO = Funciones.CheckStr(objValidarCandidatoFCResponseCBIO.MessageResponse.body.codigoRespuesta);

                strMensajeRespuestaBSCS = Funciones.CheckStr(objValidarCandidatoFCResponseBSCS.MessageResponse.body.mensajeRespuesta);
                strMensajeRespuestaCBIO = Funciones.CheckStr(objValidarCandidatoFCResponseCBIO.MessageResponse.body.mensajeRespuesta);

                lstClienteFCBSCS = objValidarCandidatoFCResponseBSCS.MessageResponse.body.dataRespuesta;
                lstClienteFCCBIO = objValidarCandidatoFCResponseCBIO.MessageResponse.body.dataRespuesta;

                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-710][ValidarClienteFC][CodigoRespuestaBSCS]", strCodigoRespuestaBSCS), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-710][ValidarClienteFC][CodigoRespuestaCBIO]", strCodigoRespuestaCBIO), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-710][ValidarClienteFC][MensajeRespuestaBSCS]", strMensajeRespuestaBSCS), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-710][ValidarClienteFC][MensajeRespuestaCBIO]", strMensajeRespuestaCBIO), null, null);

                if (strCodigoRespuestaBSCS == "0") //Ya no puede escoger ya que ya cuenta con bono ASIS.
                {
                    strCodigoFinal = strCodigoRespuestaBSCS;
                    strMensajeFinal = strMensajeRespuestaBSCS;
                }
                else if (strCodigoRespuestaCBIO == "0") //Ya no puede escoger ya que ya cuenta con bono ONE.
                {
                    strCodigoFinal = strCodigoRespuestaCBIO;
                    strMensajeFinal = strMensajeRespuestaCBIO;
                }
                else if (strCodigoRespuestaBSCS == "1") //Le falta elegir a donde va su bono.
                {
                    strCodigoFinal = strCodigoRespuestaBSCS;
                    strMensajeFinal = strMensajeRespuestaBSCS;
                    if (lstClienteFCCBIO != null && lstClienteFCCBIO.Count > 0) //Hacer Merge de Listas.
                    {
                        lstClienteFCBSCS = Merge_BSCS_CBIO_FC(lstClienteFCBSCS, lstClienteFCCBIO);
                    }
                }
                else if (strCodigoRespuestaBSCS == "2") //Cliente Nuevo.
                {
                    //Si cantidad de lineas tobe es mayor a 0, codigo respuesta es igual a 3, caso contrario es igual a 2.
                    strCodigoFinal = strCodigoRespuestaBSCS;
                    strMensajeFinal = strMensajeRespuestaBSCS;
                    if (lstClienteFCCBIO != null && (strCodigoRespuestaCBIO == "3" || strCodigoRespuestaCBIO == "6"))
                    {
                        strCodigoFinal = "3";
                        strMensajeFinal = strMensajeRespuestaCBIO;
                        lstClienteFCBSCS = lstClienteFCCBIO;
                    }
                }
                else if (strCodigoRespuestaBSCS == "3") //Tiene un tipo de servicio activa ya sea fija o movil.
                {
                    //Si cantidad de lineas moviles tobe es mayor a 0 y la cantidad de lineas fijas ASIS es mayor a 0 el codigo de
                    //respuesta es igual a 1(Le falta elegir a donde va su bono), caso contrario igual a 3(Tiene un tipo de servicio activa ya sea fija o movil)
                    strCodigoFinal = strCodigoRespuestaBSCS;
                    strMensajeFinal = strMensajeRespuestaBSCS;
                    if (lstClienteFCCBIO != null && lstClienteFCCBIO.Count > 0)
                    {
                        //Descomentar cuando el servicio del TOBE funcione igual que el ASIS
                        if (lstClienteFCBSCS != null && (strCodigoRespuestaCBIO == "3" || strCodigoRespuestaCBIO == "6"))
                        {
                            bool blServicioFijaBscs = false;
                            foreach (BEDatosClienteFC objClienteFCBSCS in lstClienteFCBSCS)
                            {
                                if (objClienteFCBSCS.tipoServicio == "FIJA")
                                {
                                    strCodigoFinal = "1";
                                    strMensajeFinal = "Cliente cuenta con bono pendiente";
                                    blServicioFijaBscs = true;
                                    break;
                                }
                            }
                            if (blServicioFijaBscs)
                            {
                                lstClienteFCBSCS = Merge_BSCS_CBIO_FC(lstClienteFCBSCS, lstClienteFCCBIO);
                            }
                        }
                    }
                    //Descomentar cuando el servicio del TOBE funcione igual que el ASIS
                    else if (strCodigoRespuestaCBIO == "3" || strCodigoRespuestaCBIO == "6")
                    {
                        if (lstClienteFCCBIO != null && lstClienteFCCBIO.Count > 0)
                        {
                            lstClienteFCBSCS = Merge_BSCS_CBIO_FC(lstClienteFCBSCS, lstClienteFCCBIO);
                        }
                    }
                }
                else if (strCodigoRespuestaBSCS == "7") //Bono en proceso por parte del ASIS
                {
                    strCodigoFinal = strCodigoRespuestaBSCS;
                    strMensajeFinal = strMensajeRespuestaBSCS;
                    if (lstClienteFCCBIO != null && lstClienteFCCBIO.Count > 0) //Hacer Merge de Listas.
                    {
                        lstClienteFCBSCS = Merge_BSCS_CBIO_FC(lstClienteFCBSCS, lstClienteFCCBIO);
                    }
                }
                else
                {
                    strCodigoFinal = strCodigoRespuestaCBIO;
                    strMensajeFinal = strMensajeRespuestaCBIO;
                    lstClienteFCBSCS = lstClienteFCCBIO;
                }

                objResponse.MessageResponse.body.codigoRespuesta = strCodigoFinal;
                objResponse.MessageResponse.body.mensajeRespuesta = strMensajeFinal;
                objResponse.MessageResponse.body.dataRespuesta = lstClienteFCBSCS;

                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-710][ValidarClienteFC][CodigoFinal]", Funciones.CheckStr(strCodigoFinal)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-710][ValidarClienteFC][MensajeFinal]", Funciones.CheckStr(strMensajeFinal)), null, null);
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[INICIATIVA-710][ValidarClienteFC][Ocurrio un error al validar el Cliente Full Claro]", null, null);
                objLog.CrearArchivolog(string.Format("{0} {1} | {2}", "[INICIATIVA-710][ValidarClienteFC][ERROR]", Funciones.CheckStr(ex.Message), Funciones.CheckStr(ex.StackTrace)), null, null);
            }

            objLog.CrearArchivolog(string.Format("{0}{1}", "[FIN][INICIATIVA-710][ValidarClienteFC]", string.Empty), null, null);

            return objResponse;
        }

        public ValidarProductosFCResponse ValidarProductosFC(ValidarProductosFCRequest objValidarProductosFCRequest, BEAuditoriaRequest objBeAuditoriaRequest)
        {
            RestValidarProductosFC objRestBSCS = new RestValidarProductosFC();
            ValidarProductosFCResponse objValidarProductosFCResponse = new ValidarProductosFCResponse();
            ValidarProductosFCResponse objValidarProductosFCResponseBSCS = new ValidarProductosFCResponse();
            ValidarProductosFCResponse objValidarProductosFCResponseCBIO = new ValidarProductosFCResponse();
            List<BEDatosClienteFC> lstOnHoldClienteFCBSCS = new List<BEDatosClienteFC>();
            List<BENuevosProductosFC> lstPlanesEvaluadosFCBSCS = new List<BENuevosProductosFC>();
            string strTipoProducto = string.Empty;

            //ASIS - BSCS7
            objValidarProductosFCResponseBSCS = objRestBSCS.ValidarProductosFC(objValidarProductosFCRequest, objBeAuditoriaRequest);

            //TOBE - BSCS9(CBIO)
            objValidarProductosFCResponseCBIO = ValidarProductosFC_CBIO(objValidarProductosFCRequest, ref strTipoProducto);

            objValidarProductosFCResponse = ValidarOnHoldFC(objValidarProductosFCResponseBSCS, objValidarProductosFCResponseCBIO, strTipoProducto);

            return objValidarProductosFCResponse;
        }

        public ValidarProductosFCResponse ValidarProductosFC_CBIO(ValidarProductosFCRequest objValidarProductosFCRequest, ref string strTipoProducto)
        {
            GeneradorLog objLog = new GeneradorLog(null, string.Format("{0}{1}", "[INICIO][INICIATIVA-710][ValidarProductosFC_CBIO]", string.Empty), null, null);
            ValidarProductosFCResponse objValidarProductosFCResponse = new ValidarProductosFCResponse();
            List<BEDatosClienteFC> lstOnHoldClienteFC = new List<BEDatosClienteFC>();
            List<BENuevosProductosFC> lstPlanesEvaluadosFC = new List<BENuevosProductosFC>();
            ConsultarPendientesActivasCBIOResponse objResponse = new ConsultarPendientesActivasCBIOResponse();
            RequestConsultarPendientesActivasCBIO objRequest = new RequestConsultarPendientesActivasCBIO();
            BLDatosCBIO objBLCbio = new BLDatosCBIO();
            BWFullClaroCBIO objWSFullClaro = new BWFullClaroCBIO();
            string strTipoDocumentoPvu = string.Empty;
            string strPlanesServicios = string.Empty;
            bool blPlanesEvaluados = false;
            string strFlagCBIO = string.Empty;
            string strWhiteListFlagCBIO = string.Empty;

            List<BETipoDocumento> objListaDocumento = (new BLGeneral()).ListarTipoDocumento();
            strTipoDocumentoPvu = Funciones.CheckStr(objListaDocumento.Where(x => x.ID_BSCS == Funciones.CheckInt(objValidarProductosFCRequest.MessageRequest.body.strTipoDocumento)).FirstOrDefault().ID_SISACT);
            strTipoProducto = objValidarProductosFCRequest.MessageRequest.body.strTipoProducto;

            objRequest.consultarPendientesActivasCBIORequest.tipoDocumento = objValidarProductosFCRequest.MessageRequest.body.strTipoDocumento;
            objRequest.consultarPendientesActivasCBIORequest.nroDocumento = objValidarProductosFCRequest.MessageRequest.body.strNroDocumento;

            objResponse = objWSFullClaro.ValidarProductosFullClaroWSCBIO(objRequest);

            #region ListaOnHoldClienteFC
            if (objResponse != null && objResponse.responseData != null)
            {
                if (objResponse.responseData.listaOnHold != null && objResponse.responseData.listaOnHold.Count > 0)
                {
                    //Lista de lineas del cliente que estan pendientes de activacion que puedan aplicar el Beneficio FC
                    foreach (var objOnHold in objResponse.responseData.listaOnHold)
                    {
                        BEDatosClienteFC objOnHoldClienteFC = new BEDatosClienteFC();
                        objOnHoldClienteFC.coId = objOnHold.coId;
                        objOnHoldClienteFC.linea = objOnHold.linea;
                        objOnHoldClienteFC.tipoServicio = objOnHold.tipoServicio;
                        objOnHoldClienteFC.tmCode = objOnHold.tmcode;
                        objOnHoldClienteFC.desTmcode = objOnHold.desTmcode;
                        objOnHoldClienteFC.customerId = objOnHold.customerId;

                        lstOnHoldClienteFC.Add(objOnHoldClienteFC);
                    }

                    objValidarProductosFCResponse.MessageResponse.body.datosOnHoldClienteFC = lstOnHoldClienteFC;
                    objLog.CrearArchivolog("[INICIATIVA-710][El cliente tiene lineas On Hold en CBIO]", null, null);
                }
            }
            #endregion

            #region ListaPlanesEvaluadosFC
            strFlagCBIO = (string)HttpContext.Current.Session["flagCBIO"];
            strWhiteListFlagCBIO = (string)HttpContext.Current.Session["WhiteListFlagCBIO"];
            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-710][ValidarProductosFC_CBIO][flagCBIO]", Funciones.CheckStr(strFlagCBIO)), null, null);
            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-710][ValidarProductosFC_CBIO][WhiteListFlagCBIO]", Funciones.CheckStr(strWhiteListFlagCBIO)), null, null);

            if (strFlagCBIO == "1" && strWhiteListFlagCBIO == "1")
            {
                strPlanesServicios = objValidarProductosFCRequest.MessageRequest.body.strPlanes;
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-710][ValidarProductosFC_CBIO][strTipoProducto]", Funciones.CheckStr(strTipoProducto)), null, null);

                if (strTipoProducto == "M") //MOVIL
                {
                    lstPlanesEvaluadosFC = EvaluarNuevoPlanFC(strTipoProducto, strPlanesServicios, ref blPlanesEvaluados);

                    if (blPlanesEvaluados)
                    {
                        //Lista de los Planes que se estan evaluando en el instante que puedan aplicar el Beneficio FC
                        objValidarProductosFCResponse.MessageResponse.body.nuevosProductosFC = lstPlanesEvaluadosFC;
                        objLog.CrearArchivolog("[INICIATIVA-710][ValidarProductosFC_CBIO][El cliente tiene nuevos Planes evaluados que aplican a Full Claro]", null, null);
                    }
                }
            }
            #endregion

            objValidarProductosFCResponse.MessageResponse.body.codigoRespuesta = objResponse.codigoRespuesta;
            objValidarProductosFCResponse.MessageResponse.body.mensajeRespuesta = objResponse.mensajeRespuesta;

            objLog.CrearArchivolog(string.Format("{0}{1}", "[FIN][INICIATIVA-710][ValidarProductosFC_CBIO]", string.Empty), null, null);

            return objValidarProductosFCResponse;
        }

        public List<BENuevosProductosFC> EvaluarNuevoPlanFC(string strTipoServicio, string strCodigoPlanes, ref bool blPlaneEvaluados)
        {
            GeneradorLog objLog = new GeneradorLog(null, string.Format("{0}{1}", "[INICIO][INICIATIVA-710][EvaluarNuevoPlanFC]", string.Empty), null, null);
            List<BENuevosProductosFC> planesEvaluadosFC = null;
            string strCodigoRespuesta = string.Empty;
            string strMensajeRespuesta = string.Empty;
            string strTMCode = string.Empty;
            string strPoId = string.Empty;
            bool blAplicaFC = false;
            string strCodPlanResp = string.Empty;
            blPlaneEvaluados = false;

            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-710][EvaluarNuevoPlanFC][strTipoServicio]", Funciones.CheckStr(strTipoServicio)), null, null);
            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-710][EvaluarNuevoPlanFC][strCodigoPlanes]", Funciones.CheckStr(strCodigoPlanes)), null, null);

            if (strTipoServicio == "M")
            {
                string[] arrCodigoPlan = strCodigoPlanes.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                planesEvaluadosFC = new List<BENuevosProductosFC>();

                foreach (var strPlan in arrCodigoPlan)
                {
                    objLog.CrearArchivolog(string.Format("[INICIATIVA-710][EvaluarNuevoPlanFC][Validar Plan: {0}]", Funciones.CheckStr(strPlan)), null, null);
                    //Obtener TMCODE del Plan
                    BEPlan objPlan = new BLGeneral_II().ObtenerPlanBSCS(strTipoServicio, strPlan, ref strCodigoRespuesta, ref strMensajeRespuesta);
                    strTMCode = objPlan.CODIGO_BSCS;

                    //Obtener PO_ID del Plan
                    BEItemGenerico objCBIO = new BLGeneral_II().ListarTopeAutomaticoCBIO(strTMCode, 0);
                    strPoId = objCBIO.Valor1;

                    //Validar si el Nuevo Plan aplica a Full Claro
                    string strMensajeAplicaFC = string.Empty;
                    blAplicaFC = new BLGeneral_II().ValidaPlanFullClaro(strPoId, ref strCodPlanResp, ref strCodigoRespuesta, ref strMensajeRespuesta);
                    strMensajeAplicaFC = strCodPlanResp == "N" ? string.Format("{0} {1} {2}", "El Plan", strPlan, "NO aplica a Full Claro") : string.Format("{0} {1} {2}", "El Plan", strPlan, "SI aplica a Full Claro");
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-710][EvaluarNuevoPlanFC]", Funciones.CheckStr(strMensajeAplicaFC)), null, null);

                    if (blAplicaFC)
                    {
                        BENuevosProductosFC objNuevoProducto = new BENuevosProductosFC();
                        objNuevoProducto.planPvu = strPlan;
                        objNuevoProducto.tipo = "MOVIL";
                        objNuevoProducto.tmcode = strTMCode;
                        objNuevoProducto.po_id = strPoId; //INICIATIVA-805 Campana Descuento Cargo Fijo 

                        planesEvaluadosFC.Add(objNuevoProducto);
                    }

                    blPlaneEvaluados = true;
                }
            }

            objLog.CrearArchivolog(string.Format("{0}{1}", "[FIN][INICIATIVA-710][EvaluarNuevoPlanFC]", string.Empty), null, null);

            return planesEvaluadosFC;
        }

        public ValidarProductosFCResponse ValidarOnHoldFC(ValidarProductosFCResponse objValidarCandidatoFCResponseBSCS, ValidarProductosFCResponse objValidarCandidatoFCResponseCBIO, string strTipoProducto)
        {
            GeneradorLog objLog = new GeneradorLog(null, string.Format("{0}{1}", "[INICIO][INICIATIVA-710][ValidarOnHoldFC]", string.Empty), null, null);
            ValidarProductosFCResponse objResponse = new ValidarProductosFCResponse();
            List<BEDatosClienteFC> lstClienteFCBSCS = new List<BEDatosClienteFC>();
            List<BEDatosClienteFC> lstClienteFCCBIO = new List<BEDatosClienteFC>();
            List<BENuevosProductosFC> lstNuevosProductosFCBSCS = new List<BENuevosProductosFC>();
            List<BENuevosProductosFC> lstNuevosProductosFCCBIO = new List<BENuevosProductosFC>();
            string strCodigoRespuestaBSCS = string.Empty;
            string strCodigoRespuestaCBIO = string.Empty;
            string strMensajeRespuestaBSCS = string.Empty;
            string strMensajeRespuestaCBIO = string.Empty;
            string strCodigoFinal = string.Empty;
            string strMensajeFinal = string.Empty;
            string strFlagCBIO = string.Empty;
            string strWhiteListFlagCBIO = string.Empty;

            try
            {
                strCodigoRespuestaCBIO = Funciones.CheckStr(objValidarCandidatoFCResponseCBIO.MessageResponse.body.codigoRespuesta);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-710][ValidarOnHoldFC][CodigoRespuestaCBIO]", strCodigoRespuestaCBIO), null, null);

                strMensajeRespuestaCBIO = Funciones.CheckStr(objValidarCandidatoFCResponseCBIO.MessageResponse.body.mensajeRespuesta);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-710][ValidarOnHoldFC][MensajeRespuestaCBIO]", strMensajeRespuestaCBIO), null, null);

                lstNuevosProductosFCCBIO = objValidarCandidatoFCResponseCBIO.MessageResponse.body.nuevosProductosFC;
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-710][ValidarOnHoldFC][lstNuevosProductosFCCBIO]", string.Empty), null, null);

                if (objValidarCandidatoFCResponseBSCS == null || objValidarCandidatoFCResponseBSCS.MessageResponse == null || objValidarCandidatoFCResponseBSCS.MessageResponse.body == null)
                {
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-710][ValidarOnHoldFC][Datos nulos en BSCS]", string.Empty), null, null);
                    strCodigoFinal = strCodigoRespuestaCBIO;
                    strMensajeFinal = strMensajeRespuestaCBIO;
                    lstClienteFCBSCS = lstClienteFCCBIO;
                }
                else
                {
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-710][ValidarOnHoldFC][Datos no son nulos en BSCS]", string.Empty), null, null);
                strCodigoRespuestaBSCS = Funciones.CheckStr(objValidarCandidatoFCResponseBSCS.MessageResponse.body.codigoRespuesta);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-710][ValidarOnHoldFC][CodigoRespuestaBSCS]", strCodigoRespuestaBSCS), null, null);

                strMensajeRespuestaBSCS = Funciones.CheckStr(objValidarCandidatoFCResponseBSCS.MessageResponse.body.mensajeRespuesta);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-710][ValidarOnHoldFC][MensajeRespuestaBSCS]", strMensajeRespuestaBSCS), null, null);

                lstClienteFCBSCS = objValidarCandidatoFCResponseBSCS.MessageResponse.body.datosOnHoldClienteFC;
                    lstNuevosProductosFCBSCS = objValidarCandidatoFCResponseBSCS.MessageResponse.body.nuevosProductosFC;
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-710][ValidarOnHoldFC][lstClienteFCBSCS]", string.Empty), null, null);
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-710][ValidarOnHoldFC][lstNuevosProductosFCBSCS]", string.Empty), null, null);

                strFlagCBIO = (string)HttpContext.Current.Session["flagCBIO"];
                strWhiteListFlagCBIO = (string)HttpContext.Current.Session["WhiteListFlagCBIO"];

                if (strCodigoRespuestaBSCS == "0")
                {
                    strCodigoFinal = strCodigoRespuestaBSCS;
                    strMensajeFinal = strMensajeRespuestaBSCS;
                    if (strCodigoRespuestaCBIO == "0")
                    {
                        //Hacer Merge con listas OnHold de CBIO
                        if (lstClienteFCCBIO != null && lstClienteFCCBIO.Count > 0)
                        {
                            lstClienteFCBSCS.AddRange(lstClienteFCCBIO);
                        }
                    }
                }
                else if (strCodigoRespuestaBSCS == "1")
                {
                    strCodigoFinal = strCodigoRespuestaBSCS;
                    strMensajeFinal = strMensajeRespuestaBSCS;

                    if (strCodigoRespuestaCBIO == "0")
                    {
                        strCodigoFinal = strCodigoRespuestaCBIO;
                        strMensajeFinal = strMensajeRespuestaCBIO;
                        lstClienteFCBSCS = lstClienteFCCBIO; //Devolver lista de lineas OnHold de CBIO
                    }
                }
                else
                {
                    strCodigoFinal = strCodigoRespuestaCBIO;
                    strMensajeFinal = strMensajeRespuestaCBIO;
                    lstClienteFCBSCS = lstClienteFCCBIO;
                }
                }
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-710][ValidarOnHoldFC][CodigoFinal]", Funciones.CheckStr(strCodigoFinal)), null, null);
                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[INICIATIVA-710][ValidarOnHoldFC][MensajeFinal]", Funciones.CheckStr(strMensajeFinal)), null, null);

                objResponse.MessageResponse.body.codigoRespuesta = strCodigoFinal;
                objResponse.MessageResponse.body.mensajeRespuesta = strMensajeFinal;
                objResponse.MessageResponse.body.datosOnHoldClienteFC = lstClienteFCBSCS;
                objResponse.MessageResponse.body.nuevosProductosFC = (strFlagCBIO == "1" && strWhiteListFlagCBIO == "1" && strTipoProducto == "M") ? lstNuevosProductosFCCBIO : lstNuevosProductosFCBSCS;
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[INICIATIVA-710][ValidarOnHoldFC][Ocurrio un error al validar el Cliente OnHold Full Claro]", null, null);
                objLog.CrearArchivolog(string.Format("{0} {1} | {2}", "[INICIATIVA-710][ValidarOnHoldFC][ERROR]", Funciones.CheckStr(ex.Message), Funciones.CheckStr(ex.StackTrace)), null, null);
            }

            objLog.CrearArchivolog(string.Format("{0}{1}", "[FIN][INICIATIVA-710][ValidarOnHoldFC]", string.Empty), null, null);

            return objResponse;
        }

        public List<BEDatosClienteFC> Merge_BSCS_CBIO_FC(List<BEDatosClienteFC> lstClienteFCBSCS, List<BEDatosClienteFC> lstClienteFCCBIO)
        {
            HelperLog.EscribirLog(string.Empty, nameLogCBIO, string.Format("{0}{1}", "[INICIO][INICIATIVA-710][Merge_BSCS_CBIO_FC]", string.Empty), false);
            List<BEDatosClienteFC> lstClienteFCUnificado = new List<BEDatosClienteFC>();
            int cantBSCS = lstClienteFCBSCS.Count;
            int cantCBIO = lstClienteFCCBIO.Count;
            //lstClienteFCUnificado = new BEDatosClienteFC[cantBSCS + cantCBIO];

            int x = 0; //lstClienteFCUnificado.Length;

            if (lstClienteFCBSCS != null && lstClienteFCBSCS.Count > 0)
            {
                HelperLog.EscribirLog(string.Empty, nameLogCBIO, "[INICIATIVA-710][Merge_BSCS_CBIO_FC][BSCS][lstClienteFCBSCS tiene datos]", false);
                foreach (BEDatosClienteFC objClienteFCBSCS in lstClienteFCBSCS)
                {
                    BEDatosClienteFC objlstClienteFCUnificado = new BEDatosClienteFC();
                    objlstClienteFCUnificado.coId = objClienteFCBSCS.coId;
                    objlstClienteFCUnificado.linea = objClienteFCBSCS.linea;
                    objlstClienteFCUnificado.tipoServicio = objClienteFCBSCS.tipoServicio;
                    objlstClienteFCUnificado.tmCode = objClienteFCBSCS.tmCode;
                    objlstClienteFCUnificado.desTmcode = objClienteFCBSCS.desTmcode;
                    objlstClienteFCUnificado.customerId = objClienteFCBSCS.customerId;

                    lstClienteFCUnificado.Add(objlstClienteFCUnificado);
                }
                HelperLog.EscribirLog(string.Empty, nameLogCBIO, "[INICIATIVA-710][Merge_BSCS_CBIO_FC][BSCS][lstClienteFCBSCS tiene datos : OK]", false);
            }

            if (lstClienteFCCBIO != null && lstClienteFCCBIO.Count > 0)
            {
                HelperLog.EscribirLog(string.Empty, nameLogCBIO, "[INICIATIVA-710][Merge_BSCS_CBIO_FC][CBIO][lstClienteFCCBIO tiene datos]", false);
                foreach (BEDatosClienteFC objClienteFCCBIO in lstClienteFCCBIO)
                {
                    BEDatosClienteFC objlstClienteFCUnificado = new BEDatosClienteFC();
                    objlstClienteFCUnificado.coId = objClienteFCCBIO.coId;
                    objlstClienteFCUnificado.linea = objClienteFCCBIO.linea;
                    objlstClienteFCUnificado.tipoServicio = objClienteFCCBIO.tipoServicio;
                    objlstClienteFCUnificado.tmCode = objClienteFCCBIO.tmCode;
                    objlstClienteFCUnificado.desTmcode = objClienteFCCBIO.desTmcode;
                    objlstClienteFCUnificado.customerId = objClienteFCCBIO.customerId;

                    lstClienteFCUnificado.Add(objlstClienteFCUnificado);
                }
                HelperLog.EscribirLog(string.Empty, nameLogCBIO, "[INICIATIVA-710][Merge_BSCS_CBIO_FC][CBIO][lstClienteFCCBIO tiene datos : OK]", false);
            }

            HelperLog.EscribirLog(string.Empty, nameLogCBIO, string.Format("{0}-->{1}", "[FIN][INICIATIVA-710][Merge_BSCS_CBIO_FC]", string.Empty), false);

            return lstClienteFCUnificado;
        }
    }
}
