using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using Claro.SISACT.Business;
using Claro.SISACT.Common;
using Claro.SISACT.Entity;
using Claro.SISACT.WS;
using System.Data;
using System.Configuration;
using System.Text;
//PROY-32439 INI MAS
using System.Web;
using Claro.SISACT.WS.WSValidacionDeudaRules;
using Claro.SISACT.WS.WSConsultaDatosOAC;
using Claro.SISACT.Entity.claro_int_consultacliente.consultarDatos; //INICIATIVA-219
using Claro.SISACT.Entity.claro_inte_cuparticipante.consultarParticipante; //INICIATIVA-219
using Claro.SISACT.Entity.DataPowerRest; //INICIATIVA-219
using Claro.SISACT.WS.BWServicesCBIO; //INICIATIVA-219
using Claro.SISACT.WS.RestReferences; // PROY - 140743
using Claro.SISACT.Entity.VentasCuotas.ValidarCuoAccPendXLinea.Response;
using Claro.SISACT.Entity.VentasCuotas.ValidarCuoAccPendXLinea.Request;
using Claro.SISACT.Entity.VentasCuotas.ObtenerVariablesBRMS.Response;
using Claro.SISACT.Entity.VentasCuotas.ObtenerDatosPedidoAccCuotas.Response;
using System.Text.Json;

//PROY-140743 FIN

namespace Claro.SISACT.WS
{
    public class BLDatosCliente
    {
        #region [Declaracion de Constantes - Config]

        String constCodTipoDocRUC = ConfigurationManager.AppSettings["TipoDocumentoRUC"].ToString();
        String constCodTipoDocDNI = ConfigurationManager.AppSettings["constCodTipoDocumentoDNI"].ToString();
        String constCodUmbralDeuda = ConfigurationManager.AppSettings["constUmbralDeuda"].ToString();
        String constCodTiempoPermanenciaRUC = ConfigurationManager.AppSettings["TiempoPermanenciaRUC"].ToString();
        String constCodTiempoPermanenciaDNI = ConfigurationManager.AppSettings["TiempoPermanenciaDNI"].ToString();
        String constCodDiasLineasBSCS = ConfigurationManager.AppSettings["constDiasLineasBSCS"].ToString();
        String constCodBloqueoRobo = ConfigurationManager.AppSettings["constCodBloqueoRobo"].ToString();
        String constCodBloqueoPerdida = ConfigurationManager.AppSettings["constCodBloqueoPerdida"].ToString();

        GeneradorLog _objLog = null;

        #endregion [Declaracion de Constantes - Config]

        String nroDocumento = null;
        String tipoDocumento = null;
        BETipoDocumento objDocumento = null;
        BEItemMensaje objMensaje = null;
        //gaa20170511
        //public BEClienteCuenta ConsultarDatosCliente(string tipoDocumento, string nroDocumento)
        public BEClienteCuenta ConsultarDatosCliente(string CurrentTerminal, string currentUser, string tipoDocumento, string nroDocumento, int intComportamientoPago, int consAntiguedadDeuda, string consFlagFlexibilidad, BEUsuarioSession objSession, Boolean strPagina)//PROY-29121// PROY-26963 - GPRD - PROMFACT //PROY-32439
        {//fin gaa20170511
            BEClienteCuenta objCliente = new BEClienteCuenta();
            BLConsumer objConsumer = new BLConsumer();
            DataTable dtResumen = null;
            DataTable dtDetalle = null;
            DataTable dtListaFraude = null;
            List<BEPlanBilletera> objListaMontoFactura = null;
            List<BEPlanBilletera> objListaPlanesActivos = null;
            string listaTelefono = string.Empty;
            //PROY-32439 INI
            int intFlagBloqueo = 0;
            int intFlagSuspension = 0;
            string strTipoBloqueo = string.Empty;
            string strTipoSuspencion = string.Empty;
            string strTipoFraude = string.Empty;
            string strTMCODE = string.Empty;
            string strConsPlanLineaFijas = string.Empty;
            string strsistemaEvaluacion = string.Empty;
            string strtipoOperacion = string.Empty;
            string strtipodocparam = string.Empty;
            string strconsDatosNulos = string.Empty;
            string strconsBrmsCaido = string.Empty;
            string strMesesDisputa = String.Empty;

            List<ValidacionDeudaBRMSrequest.Cliente.Bloqueo> lstBloqueosBRMS = null;
            List<ValidacionDeudaBRMSrequest.Cliente.Suspension> lstSuspensionesBRMS = null;
            List<String> lstFraudesBRMS = null;
            List<BEParametro> lstBEParametroParamNvoBRMS = new List<BEParametro>();
            string strCodGrupoParamNvoBRMS = Funciones.CheckStr(ConfigurationManager.AppSettings["strCodGrupoParamNvoBRMS"]);
            //PROY-32439 FIN
            string lstTelefonosActivos = string.Empty;// PROY-26963 - GPRD - PROMFACT
            string strFlagPortabilidad = "S";// PROY-26963 - GPRD - PROMFACT

            //PROY-32439 - CAMBIO_LOG INI
            string strValTipoBloqueoBRMS = "";
            string strValTipoLineaBloqueoBRMS = "";

            //string strTipoSusBRMS = "";
            string strValTipoSusBRMS = "";
            //string strTipoLineaSusBRMS = "";
            string strValTipoLineaSusBRMS = "";

            string strTipoFraudeBRMS = "";
            string strValTipoFraudeBRMS = "";
            //PROY-32439 - CAMBIO_LOG FIN

            BLDatosCBIO objBLCbio = new BLDatosCBIO(); //INICIATIVA-219
            HttpContext.Current.Session["DetalleLineaCbio"] = null; //INICIATIVA-219

            try
            {
                _objLog = new GeneradorLog(null, nroDocumento, null, "WEB");

                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][INICIO][strPagina]", strPagina.ToString()), null); // PROY 32439

                #region Documento 
                List<BETipoDocumento> objListaDocumento = (new BLGeneral()).ListarTipoDocumento();
                foreach (BETipoDocumento obj in objListaDocumento)
                {
                    if (obj.ID_SISACT == tipoDocumento)
                    {
                        this.nroDocumento = nroDocumento;
                        this.tipoDocumento = tipoDocumento;
                        objDocumento = new BETipoDocumento();
                        objDocumento = obj;
                        objDocumento.ID_OAC = Funciones.CheckInt(tipoDocumento).ToString();
                        break;
                    }
                }
                #endregion

                //PROY-32439 INI
                string blnFlagPorta = ""; //CAMBIO_PORTA 
                blnFlagPorta = (string)HttpContext.Current.Session["ObjTienePorta"]; //CAMBIO_PORTA
                objCliente.errorBrms = false;
                #region PROY-32439_Parametrica
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][PARAMETRICA][INICIO]", ""), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][PARAMETRICA][IN | strCodGrupoParamNvoBRMS]", strCodGrupoParamNvoBRMS), null);
                lstBEParametroParamNvoBRMS = (new BLGeneral()).ListaParametrosGrupo(Funciones.CheckInt64(strCodGrupoParamNvoBRMS));
                if (!Object.Equals(lstBEParametroParamNvoBRMS, null))
                {
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][PARAMETRICA][OUT | lstBEParametroParamNvoBRMS]", lstBEParametroParamNvoBRMS.Count), null);
                    strConsPlanLineaFijas = lstBEParametroParamNvoBRMS.Where(p => p.Valor1 == "TMCODE").SingleOrDefault().Valor;
                    strsistemaEvaluacion = lstBEParametroParamNvoBRMS.Where(p => p.Valor1 == "APP").SingleOrDefault().Valor;
                    
                    //CAMBIO_PORTA INI
                    if (blnFlagPorta == "S")
                    {
                        strtipoOperacion = lstBEParametroParamNvoBRMS.Where(p => p.Valor1 == "CONSUMER_PORTA").SingleOrDefault().Valor;
                    }
                    else
                    {
                        strtipoOperacion = lstBEParametroParamNvoBRMS.Where(p => p.Valor1 == "CONSUMER").SingleOrDefault().Valor;
                    }
                    //CAMBIO_PORTA FIN

                    strtipodocparam = lstBEParametroParamNvoBRMS.Where(p => p.Valor1 == "DESDOC").SingleOrDefault().Valor;
                    strMesesDisputa = lstBEParametroParamNvoBRMS.Where(p => p.Valor1 == "consMesesDisputa").SingleOrDefault().Valor;
                    strconsDatosNulos = lstBEParametroParamNvoBRMS.Where(p => p.Valor1 == "consDatosNulos").SingleOrDefault().Valor;
                    strconsBrmsCaido = lstBEParametroParamNvoBRMS.Where(p => p.Valor1 == "consBrmsCaido").SingleOrDefault().Valor;
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][PARAMETRICA][OUT | strConsPlanLineaFijas]", strConsPlanLineaFijas), null);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][PARAMETRICA][OUT | strsistemaEvaluacion]", strsistemaEvaluacion), null);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][PARAMETRICA][OUT | strtipoOperacion]", strtipoOperacion), null);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][PARAMETRICA][OUT | strtipodocparam]", strtipodocparam), null);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][PARAMETRICA][OUT | strMesesDisputa]", strMesesDisputa), null);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][PARAMETRICA][OUT | strconsDatosNulos]", strconsDatosNulos), null);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][PARAMETRICA][OUT | strconsBrmsCaido]", strconsBrmsCaido), null);
                }
                else
                {
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][PARAMETRICA][OUT | lstBEParametroParamNvoBRMS]", "vacío"), null);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][PARAMETRICA][ERROR | strconsDatosNulos]", strconsDatosNulos), null);
                    objCliente.errorBrms = true;
                    objCliente.mensajeDeudaBloqueo = "Origen de datos no responde, Porfavor comunicarse con ATU ";
                    return objCliente;
                }
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][PARAMETRICA][FIN]", ""), null);
                #endregion
                //PROY-32439 FIN

                objCliente.idCliente = DateTime.Now.ToString("ddMMyyyyhhmmss");
                objCliente.tipoDoc = tipoDocumento;
                objCliente.tipoDocDes = objDocumento.DESCRIPCION.ToString();
                objCliente.nroDoc = nroDocumento;
                if (tipoDocumento == constCodTipoDocRUC) objCliente.nroDocAsociado = nroDocumento.Substring(2, 8);
                int intMesesPermanencia = 0;
                int intMesesPermanenciaBSCS = 0;
                int intMesesPermanenciaSGA = 0;
                System.DateTime datFechaActivaBscs = Funciones.CheckDate(DBNull.Value);//PROY-140743
                System.DateTime datFechaActivaSGA = Funciones.CheckDate(DBNull.Value);//PROY-140743

                // Lista Parametros Generales
                List<BEItemGenerico> objListaItem = (new BLGeneral()).ListarParametroGeneral("1");

                int intMeses = Funciones.CheckInt(obtenerParametro(objListaItem, "27"));
                double dblPorcentajeDeuda = Funciones.CheckDbl(obtenerParametro(objListaItem, "26"));

                double dblUmbralDeuda = Funciones.CheckDbl(obtenerParametro(objListaItem, constCodUmbralDeuda));
                int intUmbralPermanencia;
                if (tipoDocumento == constCodTipoDocRUC)
                    intUmbralPermanencia = Funciones.CheckInt(obtenerParametro(objListaItem, constCodTiempoPermanenciaRUC));
                else
                    intUmbralPermanencia = Funciones.CheckInt(obtenerParametro(objListaItem, constCodTiempoPermanenciaDNI));

                objCliente.nroRangoDiasBSCS = 90; // Funciones.CheckInt(obtenerParametro(objListaItem, constCodDiasLineasBSCS));


                // INI PROY-32439 NUEVOOAC
                #region ConsultaDatosBRMSOAC

                // Valores de auditoria
                BEItemGenerico objAudit = new BEItemGenerico();
                objAudit.Codigo = nroDocumento + DateTime.Now.ToString("yyyyMMddhhmmss");
                objAudit.Codigo2 = objSession.idCuentaRed;
                objAudit.Codigo3 = ConfigurationManager.AppSettings["CodigoAplicacion"].ToString();
                objAudit.Descripcion = ConfigurationManager.AppSettings["ConstSistemaConsumer"].ToString();

                // valores de OAC
                var intDeudaCantidadDocumentos = 0;
                var dblDeudaCastigadaMonto = 0.0;
                var dblDeudaVencidaMonto = 0.0;
                var dblDeudaTotal = 0.0; // PROY-32439 NUEVOOAC
                var intDisputaAntiguedad = 0;
                var intDisputaCantidad = 0;
                var dblDisputaMonto = 0.0;
                var dblPagoTotalMonto = 0.0;
                var intAntiguedadDeuda = 0; // PROY-32439 NUEVOOAC


                String strCodigoWSOAC = "";
                String strDescripcionWSOAC = "";
                String strStatus = "";
                String strMessage = "";

                BWConsultaDatosBRMSOAC bwConsultaDatosBRMSOAC = new BWConsultaDatosBRMSOAC();

                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][RESQUEST][consultarDeudaCuentaBRMS][IN | objDocumento.ID_OAC]", Funciones.CheckStr(objDocumento.ID_OAC)), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][RESQUEST][consultarDeudaCuentaBRMS][IN | nroDocumento]", Funciones.CheckStr(nroDocumento)), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][RESQUEST][consultarDeudaCuentaBRMS][IN | strMesesDisputa]", Funciones.CheckStr(strMesesDisputa)), null);


                bwConsultaDatosBRMSOAC.consultarDeudaCuentaBRMS(objDocumento.ID_OAC, nroDocumento, strMesesDisputa, objAudit,
                                                                out strCodigoWSOAC, out strDescripcionWSOAC, out intDeudaCantidadDocumentos,
                                                                out dblDeudaCastigadaMonto, out dblDeudaVencidaMonto, out dblDeudaTotal,
                                                                out intDisputaAntiguedad, out intDisputaCantidad, out dblDisputaMonto,
                                                                out dblPagoTotalMonto, out intAntiguedadDeuda, out strStatus, out strMessage);

                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][RESPONSE][consultarDeudaCuentaBRMS][OUT | strCodigoWSOAC]", Funciones.CheckStr(strCodigoWSOAC)), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][RESPONSE][consultarDeudaCuentaBRMS][OUT | strDescripcionWSOAC]", Funciones.CheckStr(strDescripcionWSOAC)), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][RESPONSE][consultarDeudaCuentaBRMS][OUT | Status]", Funciones.CheckStr(strStatus)), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][RESPONSE][consultarDeudaCuentaBRMS][OUT | Message]", Funciones.CheckStr(strMessage)), null);


                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][RESPONSE][consultarDeudaCuentaBRMS][OUT | intDeudaCantidadDocumentos]", Funciones.CheckStr(intDeudaCantidadDocumentos)), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][RESPONSE][consultarDeudaCuentaBRMS][OUT | dblDeudaCastigadaMonto]", Funciones.CheckStr(dblDeudaCastigadaMonto)), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][RESPONSE][consultarDeudaCuentaBRMS][OUT | dblDeudaVencidaMonto]", Funciones.CheckStr(dblDeudaVencidaMonto)), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][RESPONSE][consultarDeudaCuentaBRMS][OUT | dblDeudaTotal]", Funciones.CheckStr(dblDeudaTotal)), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][RESPONSE][consultarDeudaCuentaBRMS][OUT | intDisputaAntiguedad]", Funciones.CheckStr(intDisputaAntiguedad)), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][RESPONSE][consultarDeudaCuentaBRMS][OUT | intDisputaCantidad]", Funciones.CheckStr(intDisputaCantidad)), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][RESPONSE][consultarDeudaCuentaBRMS][OUT | dblDisputaMonto]", Funciones.CheckStr(dblDisputaMonto)), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][RESPONSE][consultarDeudaCuentaBRMS][OUT | dblPagoTotalMonto]", Funciones.CheckStr(dblPagoTotalMonto)), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][RESPONSE][consultarDeudaCuentaBRMS][OUT | intAntiguedadDeuda]", Funciones.CheckStr(intAntiguedadDeuda)), null);

                #endregion
                // FIN PROY-32439 NUEVOOAC

                #region DetalleOAC
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][OAC][INICIO]", ""), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][OAC][IN | tipoDocumento]", Funciones.CheckStr(objDocumento.ID_OAC)), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][OAC][IN | nroDocumento]", Funciones.CheckStr(nroDocumento)), null);
                WS.WSOAC.DetalleClienteType[] objDetalleOAC = (new BWOAC()).detalleClienteOAC(objDocumento.ID_OAC, nroDocumento);

                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000002384777-Despues de la Consulta detalleClienteOAC]", null), null); //INC000002464679
               
            
                if (objDetalleOAC != null && objDetalleOAC[0].xCuentas.Length > 0)
                {
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000002384777-Registra datos objDetalleOAC]", null), null);  //INC000002464679
               
                    objCliente.nombres = Funciones.CheckStr(objDetalleOAC[0].xNombreCliente);
                    objCliente.apellidoPaterno = Funciones.CheckStr(objDetalleOAC[0].xApePatCliente);
                    objCliente.apellidoMaterno = Funciones.CheckStr(objDetalleOAC[0].xApeMatCliente);
                    objCliente.apellidos = Funciones.CheckStr(objCliente.apellidoPaterno + " " + objCliente.apellidoMaterno);
                    objCliente.razonSocial =  Funciones.CheckStr(objDetalleOAC[0].xApePatCliente) + " " + Funciones.CheckStr(objDetalleOAC[0].xApeMatCliente) +" "+ Funciones.CheckStr(objDetalleOAC[0].xNombreCliente);   //INC000002464679
                    objCliente.nroDocAsociado = objDetalleOAC[0].xDniAsociado;
                    objCliente.deudaVencida = dblDeudaVencidaMonto; // ROY-32439 NUEVOOAC
                    objCliente.deudaCastigada = dblDeudaCastigadaMonto; // ROY-32439 NUEVOOAC
                    objCliente.deudaTotal = dblDeudaTotal; // ROY-32439 NUEVOOAC
                    objCliente.nroDiasDeuda = Funciones.CheckInt(objDetalleOAC[0].xAntiguedadDeuda);
                    string apellidos = separarApellidos(objCliente.apellidos);
                    objCliente.apellidoPaterno = apellidos.Split('|')[0].ToString();
                    objCliente.apellidoMaterno = apellidos.Split('|')[1].ToString();
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][OAC][OUT | objDetalleOAC.deudaVencida]", objCliente.deudaVencida), null);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][OAC][OUT | objDetalleOAC.deudaCastigada]", objCliente.deudaCastigada), null);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][OAC][OUT | objDetalleOAC.deudaTotal]", objCliente.deudaTotal), null);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - WS OAC - objCliente.nombres]: ", Funciones.CheckStr(objCliente.nombres)), null);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - WS OAC - objCliente.apellidoPaterno]: ", Funciones.CheckStr(objCliente.apellidoPaterno)), null);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - WS OAC - objCliente.apellidoMaterno]: ", Funciones.CheckStr(objCliente.apellidoMaterno)), null);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - WS OAC - objCliente.razonSocial]: ", Funciones.CheckStr(objCliente.razonSocial)), null);
                }
                else
                {
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][OAC][OUT | objDetalleOAC]", "vacío, se utilizarán valores por defecto"), null);
                }
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][OAC][FIN]", ""), null);
                #endregion

                #region DetalleBSCS - CBIO
                //INI: INICIATIVA-219
                BEDetalleLinea_CBIO objDetalleLineaCBIO = new BEDetalleLinea_CBIO();
                string strFraude = string.Empty;
                bool blDetalleLineaCBIO = false;
                bool blBloqueoRoboPerdida = false;
                objDetalleLineaCBIO = objBLCbio.ListarDetalleLineaCBIO(tipoDocumento, nroDocumento, objSession.OficinaVenta, ref blDetalleLineaCBIO, ref strFraude, ref blBloqueoRoboPerdida);
                //FIN: INICIATIVA-219

                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][BSCS][INICIO]", ""), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][BSCS][IN | tipoDocumento]", Funciones.CheckStr(objDocumento.ID_BSCS)), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][BSCS][IN | nroDocumento]", nroDocumento), null);

                //INI INC000003910770
                DataSet dsListaBSCS = null;
                if (objDocumento.ID_BSCS == 99) //99 = RUC
                {
                    int tipoDocumentoBSCS7 = 0;
                    //objDocumento.ID_BSCS = Funciones.CheckInt(ReadKeySettings.Key_TipoDocumentoBSCS7);
                    tipoDocumentoBSCS7 = Funciones.CheckInt(ReadKeySettings.Key_TipoDocumentoBSCS7);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003910770][BSCS][objDocumento.ID_BSCS: ]", Funciones.CheckStr(objDocumento.ID_BSCS)), null);
                    dsListaBSCS = objConsumer.ListarDetalleLineaBSCS(tipoDocumentoBSCS7, nroDocumento);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003910770][BSCS][dsListaBSCS.Tables[0].Rows.Count: ]", Funciones.CheckStr(dsListaBSCS.Tables[0].Rows.Count)), null);
                }
                else 
                {
                    dsListaBSCS = objConsumer.ListarDetalleLineaBSCS(objDocumento.ID_BSCS, nroDocumento);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003910770][BSCS][dsListaBSCS.Tables[0].Rows.Count: ]", Funciones.CheckStr(dsListaBSCS.Tables[0].Rows.Count)), null);
                }
                //FIN INC000003910770

                _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0}:{1}", "[BLDatosCliente][ConsultarDatosCliente] dsListaBSCS", Funciones.CheckStr(JsonSerializer.Serialize(Funciones.ConvertirDataSetAListaDictionary(dsListaBSCS)))), null);

                //DataSet dsListaBSCS = objConsumer.ListarDetalleLineaBSCS(objDocumento.ID_BSCS, nroDocumento); INC000003910770
                if (dsListaBSCS != null && dsListaBSCS.Tables[0].Rows.Count > 0 || blDetalleLineaCBIO)
                {
                    dtResumen = new DataTable();
                    dtDetalle = new DataTable();

                    //INI: INICIATIVA-219
                if (dsListaBSCS != null && dsListaBSCS.Tables[0].Rows.Count > 0)
                {
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][BSCS][IN | dsListaBSCS]", "no es vacío"), null);
                    dtResumen = dsListaBSCS.Tables[0];
                    dtDetalle = dsListaBSCS.Tables[1];
                        _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[BSCS][IN | dtResumen]", Funciones.CheckInt(dtResumen.Rows.Count)), null);
                        _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[BSCS][IN | dtDetalle]", Funciones.CheckInt(dtDetalle.Rows.Count)), null);
                    }

                    //Integracion de BSCS con CBIO
                    objBLCbio.DatosTableCabecera(objDetalleLineaCBIO, ref dtResumen);
                    objBLCbio.DatosTableDetalle(objDetalleLineaCBIO, ref dtDetalle);
                    //FIN: INICIATIVA-219


                    if (string.IsNullOrEmpty(objCliente.nombres) && string.IsNullOrEmpty(objCliente.razonSocial))
                    {
                        _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000002384777-Registra datos dtResumen]", null), null); //INC000002464679
               
                        objCliente.nombres = Funciones.CheckStr(dtResumen.Rows[0]["NOMBRES"]);
                        objCliente.apellidos = Funciones.CheckStr(dtResumen.Rows[0]["APELLIDOS"]);
                        string apellidos = separarApellidos(objCliente.apellidos);
                        objCliente.apellidoPaterno = apellidos.Split('|')[0].ToString();
                        objCliente.apellidoMaterno = apellidos.Split('|')[1].ToString();
                        objCliente.razonSocial = Funciones.CheckStr(dtResumen.Rows[0]["RAZON_SOCIAL"]);

                        _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - Si objCliente.nombres es nulo a vacio le asigna lo siguiente: ", ""), null);
                        _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - BSCS - objCliente.nombres]: ", Funciones.CheckStr(objCliente.nombres)), null);
                        _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - BSCS - objCliente.apellidoPaterno]: ", Funciones.CheckStr(objCliente.apellidoPaterno)), null);
                        _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - BSCS - objCliente.apellidoMaterno]: ", Funciones.CheckStr(objCliente.apellidoMaterno)), null);
                        _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - BSCS - objCliente.razonSocial]: ", Funciones.CheckStr(objCliente.razonSocial)), null);
                    
                    }
                    //PROY-32439 INI MAS
                    Int32 intAuxBSCS = 1;
                    Int32 intAuxBSCSlongitud = dtDetalle.Rows.Count.ToString().Length;
                    String strNumeroItem = String.Empty;
                    String strTelefonoBSCS = String.Empty;
                    //PROY-32439 INI MAS
                    foreach (DataRow dr in dtDetalle.Rows)
                    {
// INI PROY-32439 - NUEVOOAC
                        var objBloqueoBRMS = new ValidacionDeudaBRMSrequest.Cliente.Bloqueo();
                        var objSuspensionBRMS = new ValidacionDeudaBRMSrequest.Cliente.Suspension();
                        // FIN PROY-32439 - NUEVOOAC
                        if ((Funciones.CheckStr(dr["ESTADO"])).Equals("Activo"))
                        {
                            objCliente.CF_Total += Funciones.CheckDbl(dr["CF_CONTRATO"]);
                            objCliente.nroPlanesActivos++; // JCFM - INC000000681349
                        }

                        //PROY-32439 INI MAS
                        strNumeroItem = intAuxBSCS.ToString().PadLeft(intAuxBSCSlongitud, '0');
                        strTelefonoBSCS = Funciones.CheckStr(dr["NUMERO"]);
                        _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][BSCS][" + strNumeroItem + "][" + strTelefonoBSCS + "]========================================", ""), null);
                        #region PlanLinea
                        strTMCODE = (Funciones.CheckStr(dr["TMCODE"]));
                        string strFijaoMovil = string.Empty;
                        String[] arregloPlanLineaFijas = strConsPlanLineaFijas.Split('|');
                        _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][BSCS][" + strNumeroItem + "][" + strTelefonoBSCS + "][PLANLINEA][strTMCODE]", strTMCODE), null);
                        _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][BSCS][" + strNumeroItem + "][" + strTelefonoBSCS + "][PLANLINEA][strConsPlanLineaFijas]", strConsPlanLineaFijas), null);
                        foreach (String PlanLinea in arregloPlanLineaFijas)
                        {
                            if (PlanLinea == strTMCODE)
                            {
                                strFijaoMovil = "FIJA";
                            }
                            else
                            {
                                strFijaoMovil = "MOVIL";
                            }
                        }
                        _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][BSCS][" + strNumeroItem + "][" + strTelefonoBSCS + "][PLANLINEA][strFijaoMovil]", strFijaoMovil), null);
                        #endregion

                        #region Bloqueos
                        _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][BSCS][" + strNumeroItem + "][" + strTelefonoBSCS + "]----------------------------------------", ""), null);
                        string COD_BLOQ = Funciones.CheckStr(dr["COD_BLOQ"]);
                        if (!COD_BLOQ.Equals(""))
                        {
                            if (Object.Equals(lstBloqueosBRMS, null))
                            {
                                lstBloqueosBRMS = new List<ValidacionDeudaBRMSrequest.Cliente.Bloqueo>();
                            }

                            //PROY-32439 - CAMBIOS_LOG INI
                            strValTipoBloqueoBRMS += Funciones.CheckStr(COD_BLOQ) + "|";
                            strValTipoLineaBloqueoBRMS += strFijaoMovil + "|";
                            //PROY-32439 - CAMBIOS_LOG FIN

                            objBloqueoBRMS.tipo = Funciones.CheckStr(COD_BLOQ);
                            objBloqueoBRMS.tipoLinea = strFijaoMovil;
                            lstBloqueosBRMS.Add(objBloqueoBRMS);

                            _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][BSCS][" + strNumeroItem + "][" + strTelefonoBSCS + "][BLOQUEO][COD_BLOQ]", COD_BLOQ), null);
                            _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][BSCS][" + strNumeroItem + "][" + strTelefonoBSCS + "][BLOQUEO][tipo]", objBloqueoBRMS.tipo), null);
                            _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][BSCS][" + strNumeroItem + "][" + strTelefonoBSCS + "][BLOQUEO][tipoLinea]", objBloqueoBRMS.tipoLinea), null);
                        }
                        else
                        {
                            _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][BSCS][" + strNumeroItem + "][" + strTelefonoBSCS + "][BLOQUEO][NODATA]", "Línea sin bloqueo"), null);
                        }
                        #endregion

                        #region Suspensiones
                        _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][BSCS][" + strNumeroItem + "][" + strTelefonoBSCS + "]----------------------------------------", ""), null);
                        string COD_SUSP = Funciones.CheckStr(dr["COD_SUSP"]);
                        if (!COD_SUSP.Equals(""))
                        {
                            if (Object.Equals(lstSuspensionesBRMS, null))
                            {
                                lstSuspensionesBRMS = new List<ValidacionDeudaBRMSrequest.Cliente.Suspension>();
                            }

                            //PROY-32439 - CAMBIOS_LOG INI
                            strValTipoSusBRMS += Funciones.CheckStr(COD_SUSP) + "|";
                            strValTipoLineaSusBRMS += strFijaoMovil + "|";
                            //PROY-32439 - CAMBIOS_LOG FIN

                            objSuspensionBRMS.tipo = Funciones.CheckStr(COD_SUSP);
                            objSuspensionBRMS.tipoLinea = strFijaoMovil;
                            lstSuspensionesBRMS.Add(objSuspensionBRMS);
                            _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][BSCS][" + strNumeroItem + "][" + strTelefonoBSCS + "][SUSPENSION][COD_SUSP]", COD_SUSP), null);
                            _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][BSCS][" + strNumeroItem + "][" + strTelefonoBSCS + "][SUSPENSION][tipo]", objSuspensionBRMS.tipo), null);
                            _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][BSCS][" + strNumeroItem + "][" + strTelefonoBSCS + "][SUSPENSION][tipoLinea]", objSuspensionBRMS.tipoLinea), null);
                        }
                        else
                        {
                            _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][BSCS][" + strNumeroItem + "][" + strTelefonoBSCS + "][SUSPENSION][NODATA]", "Línea sin suspensión"), null);
                        }
                        #endregion

                        intAuxBSCS++;
                        //PROY-32439 FIN MAS
                        _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][CANTIDAD PLANES ACTIVOS CBIO]", objCliente.nroPlanesActivos), null);//INICIATIVA-219
                    }
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][CANTIDAD PLANES ACTIVOS CBIO][2960]", objCliente.nroPlanesActivos), null);//LOG 2960

                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][objDocumento.ID_SISACT][2960]", objDocumento.ID_SISACT), null);//LOG 2960
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][TipoDocumentoRUC][2960]", ConfigurationManager.AppSettings["TipoDocumentoRUC"].ToString()), null);//LOG 2960
                    //PROY-26963 JCC RF08
                    if (objDocumento.ID_SISACT == ConfigurationManager.AppSettings["TipoDocumentoRUC"].ToString())
                    {
                        objCliente.nroPlanesActivos = Funciones.CheckInt(dtResumen.Rows[0]["PLANES"]);
                    }
                     _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][CANTIDAD PLANES ACTIVOS CBIO][2960]", objCliente.nroPlanesActivos), null);//LOG 2960
                    //PROY-26963 JCC RF08 
 
                    objCliente.nroBloqueo = Funciones.CheckInt(dtResumen.Rows[0]["BLOQ"]);
                    objCliente.nroSuspension = Funciones.CheckInt(dtResumen.Rows[0]["SUSP"]);
                    objCliente.nroLineasBSCS = Funciones.CheckInt(dtResumen.Rows[0]["PLANES"]);
                    objCliente.nroLineaMenor7Dia = Funciones.CheckInt(dtResumen.Rows[0]["NRO_7"]);
                    objCliente.nroLineaMenor30Dia = Funciones.CheckInt(dtResumen.Rows[0]["NRO_30"]);
                    objCliente.nroLineaMenor90Dia = Funciones.CheckInt(dtResumen.Rows[0]["NRO_90"]);
                    objCliente.nroLineaMenor180Dia = Funciones.CheckInt(dtResumen.Rows[0]["NRO_180"]);
                    objCliente.nroLineaMayor90Dia = Funciones.CheckInt(dtResumen.Rows[0]["NRO_90_MAS"]);
                    objCliente.nroLineaMayor180Dia = Funciones.CheckInt(dtResumen.Rows[0]["NRO_180_MAS"]);
                    objCliente.lineaBSCS = dtDetalle;
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][DetalleBSCS][ProcesarDetalleBSCS()] dtDetalle.Rows.Count >", " ProcesarDetalleBSCS()"), null);
                    ProcesarDetalleBSCS(dtDetalle, ref objListaMontoFactura, ref objListaPlanesActivos, ref intMesesPermanenciaBSCS, ref datFechaActivaBscs);
                    
                    #region Fraude
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][BSCS]----------------------------------------", ""), null);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][BSCS][FRAUDE][INICIO]", ""), null);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][BSCS][FRAUDE][IN | strFlagBuscar]", "1"), null);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][BSCS][FRAUDE][IN | tipoDocumento]", Funciones.CheckStr(objDocumento.ID_BSCS)), null);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][BSCS][FRAUDE][IN | nroDocumento]", nroDocumento), null);
                    dtListaFraude = objConsumer.ListarDetalleLineaFraude("1", objDocumento.ID_BSCS, nroDocumento);

                    //INI: INICIATIVA-219
                    objBLCbio.ListarDetalleLineaFraudeCBIO(strFraude, ref dtListaFraude);
                    int cantFraude = 0;
                    //FIN: INICIATIVA-219

                    //PROY-32439 MAS INI
                    if (!Object.Equals(dtListaFraude, null))
                    {
                        foreach (DataRow dr in dtListaFraude.Rows)
                        {
                            strTipoFraude = Funciones.CheckStr(dr["TIPO_ESTADO"]);
                            if (strTipoFraude.Length > 0)
                            {
                                if (Object.Equals(lstFraudesBRMS, null))
                                {
                                    lstFraudesBRMS = new List<String>();
                                }

                                cantFraude++; //INICIATIVA-219

                                //PROY-32439 - CAMBIOS_LOG INI
                                strValTipoFraudeBRMS = strTipoFraude;
                                //PROY-32439 - CAMBIOS_LOG FIN

                                lstFraudesBRMS.Add(strTipoFraude);
                                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][BSCS][FRAUDE][OUT | strTipoFraude]", strTipoFraude), null);
                            }

                            //PROY-32439 - CAMBIOS_LOG INI
                            if (strValTipoFraudeBRMS != "")
                            {
                                strTipoFraudeBRMS += strValTipoFraudeBRMS + "|";
                            }
                            //PROY-32439 - CAMBIOS_LOG FIN

                        }

                        _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INICIATIVA-219][BSCS][FRAUDE][Cantidad de Fraudes]", cantFraude), null);//INICIATIVA-219

                        if (Object.Equals(lstFraudesBRMS, null) || (lstFraudesBRMS.Count == 0))
                        {
                            _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][BSCS][FRAUDE][OUT | strTipoFraude]", "vacío, no cuenta con fraude"), null);
                        }
                    }
                    else
                    {
                        _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][BSCS][FRAUDE][OUT | dtListaFraude]", "vacío, se utilizarán valores por defecto"), null);
                    }
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][BSCS][FRAUDE][FIN]", ""), null);
                    //PROY-32439 MAS FIN
                    #endregion

// INI PROY-32439 - NUEVOOAC
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][VALIDACION BLOQUEO LDI - FRAUDE]", ""), null);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][VALIDACION BLOQUEO LDI - FRAUDE][constCodTiempoPermanenciaDNI]", constCodTiempoPermanenciaDNI), null);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][VALIDACION BLOQUEO LDI - FRAUDE][objCliente.deudaTotal]", objCliente.deudaTotal), null);
// FIN PROY-32439 - NUEVOOAC

                    if (objCliente.tipoDoc == constCodTiempoPermanenciaDNI && objCliente.deudaTotal > 0)
                    {
                        objCliente.deudaFraude = ProcesarDetalleFraude(dtListaFraude, dtDetalle, objDetalleOAC[0].xCuentas);
                        objCliente.deudaTotal = objCliente.deudaTotal - objCliente.deudaFraude;
                        // INI PROY-32439 - NUEVOOAC
_objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][VALIDACION BLOQUEO LDI - FRAUDE][objCliente.deudaFraude]", objCliente.deudaFraude), null);
                        _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][VALIDACION BLOQUEO LDI - FRAUDE][objCliente.deudaTotal]", objCliente.deudaTotal), null);
// FIN PROY-32439 - NUEVOOAC
                    }
                    // Detalle Lineas con Bloqueo
                    if (objCliente.nroBloqueo > 0)
                    {
                        dtListaFraude = null;
                        dtListaFraude = objConsumer.ListarDetalleLineaBloqueo("1", objDocumento.ID_BSCS, nroDocumento);

                        foreach (DataRow dr in dtListaFraude.Rows)
                        {
                            string strCodBloqueo = Funciones.CheckStr(dr["CODIGO_BLOQUEO"].ToString());

                            if (strCodBloqueo == constCodBloqueoRobo || strCodBloqueo == constCodBloqueoPerdida)
                            {
                                objCliente.soloBloqueoRoboPerdida = true;
                            }
                            else
                            {
                                objCliente.soloBloqueoRoboPerdida = false;
                                break;
                            }
                        }
                        //INI: INICIATIVA-219
                        if (!objCliente.soloBloqueoRoboPerdida)
                        {
                            objCliente.soloBloqueoRoboPerdida = blBloqueoRoboPerdida;
                        }
                        //FIN: INICIATIVA-219
                    }

                    //INICIO SD1035101 - Se extrayo de la condicional tipoDocumento != constCodTipoDocRUC
                    foreach (DataRow dr in dtDetalle.Rows)
                    {
                        listaTelefono = String.Format("{0}|{1}", listaTelefono, dr["NUMERO"].ToString());
                        // PROY-26963 - GPRD - PROMFACT
                        if ((Funciones.CheckStr(dr["ESTADO"])).Equals("Activo"))
                        {
                            lstTelefonosActivos = String.Format("{0}|{1}", lstTelefonosActivos, dr["NUMERO"].ToString());
                        }
                        // PROY-26963 - GPRD - PROMFACT
                    }
                    //FIN SD1035101
                    if (tipoDocumento != constCodTipoDocRUC)
                    {
                        //INI: INICIATIVA-219
                        double CF_MenorCbio = 0;
			double CF_MayorCbio = 0;
                        double CF_MenorBSCS = 0;
                        double CF_MayorBSCS = 0;
                        // CF Mayor/ Menor a N meses BSCS9(CBIO)
                        _objLog.CrearArchivolog(null, string.Format("{0} {1}", nroDocumento, "[INICIO][CalcularCF]"), null);
                        CF_MenorCbio = Funciones.CheckDbl(objBLCbio.CalcularCF_N_MesesCBIO(nroDocumento, intMeses, 1), 2);
                        CF_MayorCbio = Funciones.CheckDbl(objBLCbio.CalcularCF_N_MesesCBIO(nroDocumento, intMeses, 2), 2);
                        _objLog.CrearArchivolog(null, string.Format("{0} {1}-->{2}", nroDocumento, "[CalcularCF][CF_MenorCbio]", Funciones.CheckStr(CF_MenorCbio)), null);
                        _objLog.CrearArchivolog(null, string.Format("{0} {1}-->{2}", nroDocumento, "[CalcularCF][CF_MayorCbio]", Funciones.CheckStr(CF_MayorCbio)), null);
                        //FIN: INICIATIVA-219

                        objCliente.oPlanesActivosCorporativo = (new BLConsumer()).ListarDetallePlanesCorporativo(objDocumento.ID_BSCS, nroDocumento);
                        // CF Mayor/ Menor a N meses BSCS7
                        CF_MenorBSCS = Funciones.CheckDbl(CalcularCF(nroDocumento, intMeses, 1), 2);
                        CF_MayorBSCS = Funciones.CheckDbl(CalcularCF(nroDocumento, intMeses, 2), 2);
                        _objLog.CrearArchivolog(null, string.Format("{0} {1}-->{2}", nroDocumento, "[CalcularCF][CF_MenorBSCS]", Funciones.CheckStr(CF_MenorBSCS)), null);
                        _objLog.CrearArchivolog(null, string.Format("{0} {1}-->{2}", nroDocumento, "[CalcularCF][CF_MayorBSCS]", Funciones.CheckStr(CF_MayorBSCS)), null);
                        
                        objCliente.CF_Menor = CF_MenorBSCS + CF_MenorCbio;
                        objCliente.CF_Mayor = CF_MayorBSCS + CF_MayorCbio;
                        _objLog.CrearArchivolog(null, string.Format("{0} {1}-->{2}", nroDocumento, "[CalcularCF][Total CF_Menor de BSCS y CBIO][objCliente.CF_Menor]", Funciones.CheckStr(objCliente.CF_Menor)), null);
                        _objLog.CrearArchivolog(null, string.Format("{0} {1}-->{2}", nroDocumento, "[CalcularCF][Total CF_Mayor de BSCS y CBIO][objCliente.CF_Mayor]", Funciones.CheckStr(objCliente.CF_Mayor)), null);
                        _objLog.CrearArchivolog(null, string.Format("{0} {1}", nroDocumento, "[FIN][CalcularCF]"), null);
                    }
                }
                else
                {
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][BSCS][OUT | dsListaBSCS]", "vacío, se utlizarán valores por defecto"), null);

                }
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][BSCS][FIN]", ""), null);
                #endregion

                #region DetalleSGA
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][SGA][INICIO]", ""), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][SGA][IN | tipoDocumento]", Funciones.CheckStr(objDocumento.ID_SGA)), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][SGA][IN | nroDocumento]", nroDocumento), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][SGA][IN | intCantMeses]", intMeses), null);
                DataSet dsListaSGA = objConsumer.ListarDetalleLineaSGA(objDocumento.ID_SGA, nroDocumento, intMeses);
                if (dsListaSGA != null && dsListaSGA.Tables[0].Rows.Count > 0)
                {
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][SGA][OUT | dsListaSGA]", "no es vacío"), null);
                    dtResumen = new DataTable();
                    dtResumen = dsListaSGA.Tables[0];
                    dtDetalle = new DataTable();
                    dtDetalle = dsListaSGA.Tables[1];

                    if (string.IsNullOrEmpty(objCliente.nombres) && string.IsNullOrEmpty(objCliente.razonSocial))
                    {
                        objCliente.nombres = Funciones.CheckStr(dtResumen.Rows[0]["NOMCLI"]);
                        objCliente.apellidoPaterno = Funciones.CheckStr(dtResumen.Rows[0]["APEPAT"]);
                        objCliente.apellidoMaterno = Funciones.CheckStr(dtResumen.Rows[0]["APEMAT"]);
                        objCliente.apellidos = Funciones.CheckStr(objCliente.apellidoPaterno + " " + objCliente.apellidoMaterno);
                        objCliente.razonSocial = Funciones.CheckStr(dtResumen.Rows[0]["RAZON_SOCIAL"]);

                        _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - Si objCliente.nombres es nulo a vacio le asigna lo siguiente: ", ""), null);
                        _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - SGA - objCliente.nombres]: ", Funciones.CheckStr(objCliente.nombres)), null);
                        _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - SGA - objCliente.apellidoPaterno]: ", Funciones.CheckStr(objCliente.apellidoPaterno)), null);
                        _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - SGA - objCliente.apellidoMaterno]: ", Funciones.CheckStr(objCliente.apellidoMaterno)), null);
                        _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - SGA - objCliente.razonSocial]: ", Funciones.CheckStr(objCliente.razonSocial)), null);

                    }

                    foreach (DataRow dr in dtDetalle.Rows)
                    {
                        if ((Funciones.CheckStr(dr["ESTADO"])).Equals("Activo"))
                        {
                            objCliente.CF_Total += Funciones.CheckDbl(dr["MONTO_CR"]);
                            objCliente.nroPlanesActivos += Funciones.CheckInt(dr["CANT_SRV"]);
                        }
                    }

                    _objLog.CrearArchivolog("[INC000004091065][ConsultarDatos] linea 678", string.Format("{0}:{1}", "objCliente.nroPlanesActivos", Funciones.CheckStr(objCliente.nroPlanesActivos)), null);

                    objCliente.nroBloqueo += Funciones.CheckInt(dtResumen.Rows[0]["NUM_BLOQ"]);
                    objCliente.nroSuspension += Funciones.CheckInt(dtResumen.Rows[0]["NUM_SUSP"]);
                    objCliente.nroLineasSGA = Funciones.CheckInt(dtResumen.Rows[0]["NUM_PLAN"]);
                    objCliente.lineaSGA = dtDetalle;
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][ConsultarDatosCliente][DetalleSGA]> ", "ProcesarDetalleSGA()"), null);
                    ProcesarDetalleSGA(dtDetalle, ref objListaMontoFactura, ref objListaPlanesActivos, ref intMesesPermanenciaSGA, ref datFechaActivaSGA);//PROY-140743
                }
                else
                {
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][SGA][OUT | dsListaSGA]", "vacío, se utlizarán valores por defecto"), null);
                }
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][SGA][2960]", objCliente.nroPlanesActivos), null);//LOG 2960
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][SGA][FIN]", ""), null);
                #endregion

                #region FlagBloqueo
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][FLAGBLOQUEO][INICIO]", ""), null);
                int intNroBloqueo = Funciones.CheckInt(objCliente.nroBloqueo);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][FLAGBLOQUEO][intNroBloqueo]", intNroBloqueo), null);
                if (intNroBloqueo > 0)
                {
                    intFlagBloqueo = 1;
                }
                else
                {
                    intFlagBloqueo = 0;
                }
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][FLAGBLOQUEO][intFlagBloqueo]", intFlagBloqueo), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][FLAGBLOQUEO][FIN]", ""), null);
                #endregion

                #region FlagSuspension
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][FLAGSUSPENSION][INICIO]", ""), null);
                int intNroSuspension = Funciones.CheckInt(objCliente.nroSuspension);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][FLAGSUSPENSION][intNroSuspension]", intNroSuspension), null);
                if (intNroSuspension > 0)
                {
                    intFlagSuspension = 1;
                }
                else
                {
                    intFlagSuspension = 0;
                }
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][FLAGSUSPENSION][intFlagSuspension]", intFlagBloqueo), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][FLAGSUSPENSION][FIN]", ""), null);
                #endregion

                #region tiempoPermanencia
                objCliente.fechaActivacion = (datFechaActivaBscs > datFechaActivaSGA) ? datFechaActivaBscs : datFechaActivaSGA; //PROY-140743
                // Cliente Nuevo/Cliente Claro
                intMesesPermanencia = (intMesesPermanenciaBSCS > intMesesPermanenciaSGA) ? intMesesPermanenciaBSCS : intMesesPermanenciaSGA;
                objCliente.tiempoPermanencia = intMesesPermanencia;
                objCliente.esClienteClaro = (intMesesPermanencia >= intUmbralPermanencia);
                #endregion
                // PROY-32439 INI
                #region ComportamientoDePago
                //gaa20170511
                //objCliente.comportamientoPago = objConsumer.ObtenerComportamientoPago(objDocumento.ID_BSCS, nroDocumento, ref objMensaje);
                int intCPnulo = Convert.ToInt32(ConfigurationManager.AppSettings["consCPnulo"]);
                int intCPValorEspecial = Convert.ToInt32(ConfigurationManager.AppSettings["consCPValorEspecial"]);
                if (intComportamientoPago != intCPnulo)
                    objCliente.comportamientoPago = intComportamientoPago;
                else
                    objCliente.comportamientoPago = objBLCbio.ObtenerComportamientoPago(tipoDocumento, nroDocumento); //INICIATIVA-219
                //fin gaa20170511
                #endregion
                // PROY-32439 FIN
                #region DetalleSISACT
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][ConsultarDatosCliente][DetalleSISACT] objDocumento.ID_SISACT: ", Funciones.CheckStr(objDocumento.ID_SISACT)), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][ConsultarDatosCliente][DetalleSISACT] nroDocumento: ", Funciones.CheckStr(nroDocumento)), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][ConsultarDatosCliente][DetalleSISACT] listaTelefono: ", Funciones.CheckStr(listaTelefono)), null);



                // INC000004140507 ----- INI
                _objLog.CrearArchivolog(null, string.Format("{0} ---> {1}", DateTime.Now.ToString(), "[INC000004140507 - INI]"), null);

                DataSet dsListaSISACT = new DataSet();

                Boolean s = false;
                int maxSPVarchar2Limit = 4000; // LIMITE DE CAPACIDAD VARCHAR2
                _objLog.CrearArchivolog(null, string.Format("FECHA: {0}, INCIDENCIA: {1}, METODO:{2}, maxSPVarchar2Limit: {3}   ", DateTime.Now.ToString(), "[INC000004140507]", "ConsultarDatosCliente()", maxSPVarchar2Limit.ToString()), null);
                int lastVBarPosition = 0;
                String subListaTelefono = "";
                _objLog.CrearArchivolog(null, string.Format("FECHA: {0}, INCIDENCIA: {1}, METODO:{2}, : listaTelefono.Length > maxSPVarchar2Limit: {3}   ", DateTime.Now.ToString(), "[INC000004140507]", "ConsultarDatosCliente()", Funciones.CheckStr(listaTelefono.Length > maxSPVarchar2Limit)), null);
                if (listaTelefono.Length > maxSPVarchar2Limit)
                {
                    _objLog.CrearArchivolog(null, string.Format("FECHA: {0}, INCIDENCIA: {1}, METODO:{2}, : listaTelefono.Length > 0: {3}   ", DateTime.Now.ToString(), "[INC000004140507]", "ConsultarDatosCliente()", Funciones.CheckStr(listaTelefono.Length > 0)), null);
                    while (listaTelefono.Length > 0)
                    {
                        _objLog.CrearArchivolog(null, string.Format("FECHA: {0}, INCIDENCIA: {1}, METODO:{2}, : listaTelefono.Length > 4000: {3}   ", DateTime.Now.ToString(), "[INC000004140507]", "ConsultarDatosCliente()", Funciones.CheckStr(listaTelefono.Length > 4000)), null);
                        if (listaTelefono.Length > 4000)
                        {
                            lastVBarPosition = listaTelefono.LastIndexOf(('|'), maxSPVarchar2Limit - 1, 10);
                            _objLog.CrearArchivolog(null, string.Format("FECHA: {0}, INCIDENCIA: {1}, METODO:{2}, : lastVBarPosition: {3}   ", DateTime.Now.ToString(), "[INC000004140507]", "ConsultarDatosCliente()", Funciones.CheckStr(lastVBarPosition)), null);
                            subListaTelefono = listaTelefono.Substring(0, lastVBarPosition);
                            listaTelefono = listaTelefono.Substring(lastVBarPosition);
                            }
                        else
                        {
                            subListaTelefono = listaTelefono;
                            listaTelefono = "";

                        }
                        _objLog.CrearArchivolog(null, string.Format("FECHA: {0}, INCIDENCIA: {1}, METODO:{2}, : subListaTelefono: {3}   ", DateTime.Now.ToString(), "[INC000004140507]", "ConsultarDatosCliente()", Funciones.CheckStr(subListaTelefono)), null);
                        _objLog.CrearArchivolog(null, string.Format("FECHA: {0}, INCIDENCIA: {1}, METODO:{2}, : listaTelefono: {3}   ", DateTime.Now.ToString(), "[INC000004140507]", "ConsultarDatosCliente()", Funciones.CheckStr(listaTelefono)), null);

                        if (subListaTelefono.Length > 0)
                        {

                            _objLog.CrearArchivolog(null, string.Format("FECHA: {0}, INCIDENCIA: {1}, METODO:{2}, objDocumento.ID_SISACT: {3}   ", DateTime.Now.ToString(), "[INC000004140507]", "ConsultarDatosCliente()", Funciones.CheckStr(objDocumento.ID_SISACT)), null);
                            _objLog.CrearArchivolog(null, string.Format("FECHA: {0}, INCIDENCIA: {1}, METODO:{2}, nroDocumento: {3}   ", DateTime.Now.ToString(), "[INC000004140507]", "ConsultarDatosCliente()", Funciones.CheckStr(nroDocumento)), null);

                            DataSet subDataSet = objConsumer.ListarDetalleLineaSISACT(objDocumento.ID_SISACT, nroDocumento, subListaTelefono);

                            if (s == false)
                            {
                                dsListaSISACT = subDataSet.Clone();
                                s = true;
                            }
                            
                            if (subDataSet != null)
                            {
                                DataTable dtRes = subDataSet.Tables[0];
                                DataTable dtDet = subDataSet.Tables[1];

                                _objLog.CrearArchivolog(null, string.Format("FECHA: {0}, INCIDENCIA: {1}, METODO:{2}, dsListaSISACT data Cabecera count: {3}   ", DateTime.Now.ToString(), "[INC000004140507]", "ConsultarDatosCliente()", Funciones.CheckStr(dtRes.Rows.Count)), null);
                                _objLog.CrearArchivolog(null, string.Format("FECHA: {0}, INCIDENCIA: {1}, METODO:{2}, dsListaSISACT data Detalle count: {3}   ", DateTime.Now.ToString(), "[INC000004140507]", "ConsultarDatosCliente()", Funciones.CheckStr(dtDet.Rows.Count)), null);

                                foreach (DataRow dtrow in dtRes.Rows)
                                {
                                    dsListaSISACT.Tables[0].Rows.Add(dtrow.ItemArray);
                                }
                                foreach (DataRow dtrow1 in dtDet.Rows)
                                {
                                    dsListaSISACT.Tables[1].Rows.Add(dtrow1.ItemArray);
                                }
                            }

                        }

                    }
                    _objLog.CrearArchivolog(null, string.Format("FECHA: {0}, INCIDENCIA: {1}, METODO:{2}, dsListaSISACT data Cabecera count: {3}   ", DateTime.Now.ToString(), "[INC000004140507]", "ConsultarDatosCliente()", Funciones.CheckStr(dsListaSISACT.Tables[0].Rows.Count)), null);
                    _objLog.CrearArchivolog(null, string.Format("FECHA: {0}, INCIDENCIA: {1}, METODO:{2}, dsListaSISACT data Detalle count: {3}   ", DateTime.Now.ToString(), "[INC000004140507]", "ConsultarDatosCliente()", Funciones.CheckStr(dsListaSISACT.Tables[1].Rows.Count)), null);

                }
                else
                {
                     dsListaSISACT = objConsumer.ListarDetalleLineaSISACT(objDocumento.ID_SISACT, nroDocumento, listaTelefono);
                }
                // INC000004140507 ----- FIN
                             
              

                if (dsListaSISACT != null && dsListaSISACT.Tables[0].Rows.Count > 0)
                {
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][ConsultarDatosCliente][DetalleSISACT] dsListaSISACT.Tables[0].Rows.Count: ", Funciones.CheckStr(dsListaSISACT.Tables[0].Rows.Count)), null);
                    dtResumen = new DataTable();
                    dtResumen = dsListaSISACT.Tables[0];
                    dtDetalle = new DataTable();
                    dtDetalle = dsListaSISACT.Tables[1];

                    if (string.IsNullOrEmpty(objCliente.nombres) && string.IsNullOrEmpty(objCliente.razonSocial))
                    {
                        objCliente.nombres = Funciones.CheckStr(dtResumen.Rows[0]["NOMBRE"]);
                        objCliente.apellidoPaterno = Funciones.CheckStr(dtResumen.Rows[0]["APELLIDO_PAT"]);
                        objCliente.apellidoMaterno = Funciones.CheckStr(dtResumen.Rows[0]["APELLIDO_MAT"]);
                        objCliente.apellidos = Funciones.CheckStr(objCliente.apellidoPaterno + " " + objCliente.apellidoMaterno);
                        objCliente.razonSocial = Funciones.CheckStr(dtResumen.Rows[0]["RAZON_SOCIAL"]);
                        objCliente.nacionalidad = Funciones.CheckStr(dtResumen.Rows[0]["NACIONALIDAD"]); //PROY-31636

                        _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - Si objCliente.nombres es nulo a vacio le asigna lo siguiente", ""), null);
                        _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - ListarDetalleLineaSISACT - objCliente.nombres]: ", Funciones.CheckStr(objCliente.nombres)), null);
                        _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - ListarDetalleLineaSISACT - objCliente.apellidoPaterno]: ", Funciones.CheckStr(objCliente.apellidoPaterno)), null);
                        _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - ListarDetalleLineaSISACT - objCliente.apellidoMaterno]: ", Funciones.CheckStr(objCliente.apellidoMaterno)), null);
                        _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - ListarDetalleLineaSISACT - objCliente.razonSocial]: ", Funciones.CheckStr(objCliente.razonSocial)), null);
                        _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - ListarDetalleLineaSISACT - objCliente.nacionalidad]: ", Funciones.CheckStr(objCliente.nacionalidad)), null);
                    }

                    objCliente.lineaSISACT = dtDetalle;
                    // INI PROY-26963 - GPRD - PROMFACT
                    objCliente.lineaSISACT.Columns.Add("TIPO_EVAL", typeof(String));
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][ConsultarDatosCliente][DetalleSISACT] objCliente.lineaSISACT.Rows.Count: ", Funciones.CheckStr(objCliente.lineaSISACT.Rows.Count)), null);
                    foreach (DataRow dr in objCliente.lineaSISACT.Rows)
                    {
                        dr["TIPO_EVAL"] = "V";
                        
                    }
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][ConsultarDatosCliente][DetalleSISACT] dtDetalle.Rows.Count > ", "ProcesarDetalleSISACT()"), null);
                    ProcesarDetalleSISACT(dtDetalle, ref objListaMontoFactura, ref objListaPlanesActivos);

                    foreach (DataRow dr in dtDetalle.Rows)
                    {
                        objCliente.nroPlanesActivos += 1;
                        objCliente.CF_Total += Funciones.CheckDbl(dr["CARGO_FIJO"]);
                        objCliente.CF_Menor += Funciones.CheckDbl(dr["CARGO_FIJO"]);
                        objCliente.nroLineasBSCS += 1;
                        objCliente.nroLineaMenor7Dia += 1;
                        objCliente.nroLineaMenor30Dia += 1;
                        objCliente.nroLineaMenor90Dia += 1;
                        objCliente.nroLineaMenor180Dia += 1;
                        objCliente.nroLineasSISACT += 1;
                    }
                }
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][DETALLE SISACT][2960]", objCliente.nroPlanesActivos), null);//LOG 2960
                _objLog.CrearArchivolog("[Lista de Telefono BSCS][Activos y Desactivos]", string.Format("[{0}][{1}][{2}][{3}]", strFlagPortabilidad, objDocumento.ID_SISACT, nroDocumento, listaTelefono), null);
                _objLog.CrearArchivolog("[Lista de Telefono BSCS][Activos]", string.Format("[{0}][{1}][{2}][{3}]", strFlagPortabilidad, objDocumento.ID_SISACT, nroDocumento, lstTelefonosActivos), null);

                //Se quita flag de portabilidad
                string strCodResp = string.Empty;
                string strMsgResp = string.Empty;

                //SEC Pendientes de Activacion
                _objLog.CrearArchivolog("[Inicio][BWConsultaSEC().ObtenerPlanesSinVentaSISACT]", string.Format("[{0}][{1}][{2}][{3}]", strFlagPortabilidad, objDocumento.ID_SISACT, nroDocumento, lstTelefonosActivos), null);
                DataSet dsPlanesSinVentaSISACT = new BWConsultaSEC().ObtenerPlanesSinVentaSISACT(CurrentTerminal, currentUser, objDocumento.ID_SISACT, nroDocumento, lstTelefonosActivos, ref strCodResp, ref strMsgResp);

                if (strCodResp == "0")
                {
                    if (dsPlanesSinVentaSISACT != null && dsPlanesSinVentaSISACT.Tables.Count > 0)
                    {

                        dtResumen = new DataTable();
                        dtResumen = dsPlanesSinVentaSISACT.Tables[0];
                        dtDetalle = new DataTable();
                        dtDetalle = dsPlanesSinVentaSISACT.Tables[1];


                        if (string.IsNullOrEmpty(objCliente.nombres) && string.IsNullOrEmpty(objCliente.razonSocial) && dsPlanesSinVentaSISACT.Tables[0].Rows.Count > 0)
                        {
                            objCliente.nombres = Funciones.CheckStr(dtResumen.Rows[0]["NOMBRE"]);
                            objCliente.apellidoPaterno = Funciones.CheckStr(dtResumen.Rows[0]["APELLIDO_PAT"]);
                            objCliente.apellidoMaterno = Funciones.CheckStr(dtResumen.Rows[0]["APELLIDO_MAT"]);
                            objCliente.apellidos = Funciones.CheckStr(objCliente.apellidoPaterno + " " + objCliente.apellidoMaterno);
                            objCliente.razonSocial = Funciones.CheckStr(dtResumen.Rows[0]["RAZON_SOCIAL"]);

                            _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - Si objCliente.nombres es nulo a vacio le asigna lo siguiente", ""), null);
                            _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - WS ObtenerPlanesSinVentaSISACT - objCliente.nombres]: ", Funciones.CheckStr(objCliente.nombres)), null);
                            _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - WS ObtenerPlanesSinVentaSISACT - objCliente.apellidoPaterno]: ", Funciones.CheckStr(objCliente.apellidoPaterno)), null);
                            _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - WS ObtenerPlanesSinVentaSISACT - objCliente.apellidoMaterno]: ", Funciones.CheckStr(objCliente.apellidoMaterno)), null);
                            _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - WS ObtenerPlanesSinVentaSISACT - objCliente.razonSocial]: ", Funciones.CheckStr(objCliente.razonSocial)), null);

                        }

                        if (objCliente.lineaSISACT == null || objCliente.lineaSISACT.Rows.Count == 0)
                        {
                            objCliente.lineaSISACT = dtDetalle;
                        }
                        else
                        {
                            objCliente.lineaSISACT.Merge(dtDetalle);
                        }

                        _objLog.CrearArchivolog("[-][BWConsultSEC().ObtenerPlanesSinVentaSISACT][lineaSISACT]", string.Format("[{0}][{1}]", currentUser, objCliente.lineaSISACT.Rows.Count), null);
                        ProcesarDetalleSISACT(dtDetalle, ref objListaMontoFactura, ref objListaPlanesActivos);
                        foreach (DataRow dr in dtDetalle.Rows)
                        {
                            objCliente.nroPlanesActivos += 1;
                            objCliente.CF_Total += Funciones.CheckDbl(dr["CARGO_FIJO"]);
                            objCliente.CF_Menor += Funciones.CheckDbl(dr["CARGO_FIJO"]);
                            objCliente.nroLineasBSCS += 1;
                            objCliente.nroLineaMenor7Dia += 1;
                            objCliente.nroLineaMenor30Dia += 1;
                            objCliente.nroLineaMenor90Dia += 1;
                            objCliente.nroLineaMenor180Dia += 1;
                            objCliente.nroLineasSISACT += 1;
                        }
                    }
                }
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ObtenerPlanesSinVentaSISACT][2960]", objCliente.nroPlanesActivos), null);//LOG 2960
                _objLog.CrearArchivolog("[Fin][BWConsultSEC().ObtenerPlanesSinVentaSISACT]", string.Format("[{0}][{1}][{2}][{3}][{4}][{5}][{6}][{7}]", CurrentTerminal, currentUser, strFlagPortabilidad, objDocumento.ID_SISACT, nroDocumento, listaTelefono, strCodResp, strMsgResp), null);

                string strCodRespVenta = string.Empty;
                string strMsgRespVenta = string.Empty;
                //SEC Pendientes de Venta
                _objLog.CrearArchivolog("[Inicio][BWConsultSEC().ObtenerPlanesConVentaSISACT]", string.Format("[{0}][{1}][{2}][{3}][{4}][{5}]", strFlagPortabilidad, currentUser, CurrentTerminal, objDocumento.ID_SISACT, nroDocumento, lstTelefonosActivos), null);
                DataSet dsPlanesConVentaSISACT = new BWConsultaSECPlanPorta().ObtenerPlanesConVentaSISACT(CurrentTerminal, currentUser, objDocumento.ID_SISACT, nroDocumento, lstTelefonosActivos, ref strCodRespVenta, ref strMsgRespVenta);
                _objLog.CrearArchivolog("[Fin][BWConsultSEC().ObtenerPlanesConVentaSISACT]", string.Format("[{0}][{1}][{2}][{3}][{4}][{5}][{6}][{7}]", strFlagPortabilidad, currentUser, CurrentTerminal, objDocumento.ID_SISACT, nroDocumento, lstTelefonosActivos, strCodRespVenta, strMsgRespVenta), null);
                if (strCodRespVenta == "0")
                {
                    if (dsPlanesConVentaSISACT != null && dsPlanesConVentaSISACT.Tables.Count > 0)
                    {

                        dtResumen = new DataTable();
                        dtResumen = dsPlanesConVentaSISACT.Tables[0];
                        dtDetalle = new DataTable();
                        dtDetalle = dsPlanesConVentaSISACT.Tables[1];
                        _objLog.CrearArchivolog("[-][BWConsultSEC().ObtenerPlanesConVentaSISACT][dsPlanesConVentaSISACT_0]", string.Format("[{0}][{1}]", currentUser, dtResumen.Rows.Count), null);
                        _objLog.CrearArchivolog("[-][BWConsultSEC().ObtenerPlanesConVentaSISACT][dsPlanesConVentaSISACT_1]", string.Format("[{0}][{1}]", CurrentTerminal, dtDetalle.Rows.Count), null);

                        if (string.IsNullOrEmpty(objCliente.nombres) && string.IsNullOrEmpty(objCliente.razonSocial) && dsPlanesConVentaSISACT.Tables[0].Rows.Count > 0)
                        {
                            objCliente.nombres = Funciones.CheckStr(dtResumen.Rows[0]["NOMBRE"]);
                            objCliente.apellidoPaterno = Funciones.CheckStr(dtResumen.Rows[0]["APELLIDO_PAT"]);
                            objCliente.apellidoMaterno = Funciones.CheckStr(dtResumen.Rows[0]["APELLIDO_MAT"]);
                            objCliente.apellidos = Funciones.CheckStr(objCliente.apellidoPaterno + " " + objCliente.apellidoMaterno);
                            objCliente.razonSocial = Funciones.CheckStr(dtResumen.Rows[0]["RAZON_SOCIAL"]);

                            _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - Si objCliente.nombres es nulo a vacio le asigna lo siguiente", ""), null);
                            _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - WS ObtenerPlanesConVentaSISACT - objCliente.nombres]: ", Funciones.CheckStr(objCliente.nombres)), null);
                            _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - WS ObtenerPlanesConVentaSISACT - objCliente.apellidoPaterno]: ", Funciones.CheckStr(objCliente.apellidoPaterno)), null);
                            _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - WS ObtenerPlanesConVentaSISACT - objCliente.apellidoMaterno]: ", Funciones.CheckStr(objCliente.apellidoMaterno)), null);
                            _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - WS ObtenerPlanesConVentaSISACT - objCliente.razonSocial]: ", Funciones.CheckStr(objCliente.razonSocial)), null);

                        }

                        if (objCliente.lineaSISACT == null || objCliente.lineaSISACT.Rows.Count == 0)
                        {
                            objCliente.lineaSISACT = dtDetalle;
                        }
                        else
                        {
                            objCliente.lineaSISACT.Merge(dtDetalle);
                        }

                        _objLog.CrearArchivolog("[-][BWConsultPorta().ObtenerPlanesConVentaSISACT][lineaSISACT]", string.Format("[{0}][{1}]", currentUser, objCliente.lineaSISACT.Rows.Count), null);
                    ProcesarDetalleSISACT(dtDetalle, ref objListaMontoFactura, ref objListaPlanesActivos);

                    foreach (DataRow dr in dtDetalle.Rows)
                    {
                        objCliente.nroPlanesActivos += 1;
                        objCliente.CF_Total += Funciones.CheckDbl(dr["CARGO_FIJO"]);
                        objCliente.CF_Menor += Funciones.CheckDbl(dr["CARGO_FIJO"]);
                        objCliente.nroLineasBSCS += 1;
                        objCliente.nroLineaMenor7Dia += 1;
                        objCliente.nroLineaMenor30Dia += 1;
                        objCliente.nroLineaMenor90Dia += 1;
                        objCliente.nroLineaMenor180Dia += 1;
                        objCliente.nroLineasSISACT += 1;
                    }
                }
                }
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ObtenerPlanesConVentaSISACT][2960]", objCliente.nroPlanesActivos), null);//LOG 2960
                _objLog.CrearArchivolog("[Fin][BWConsultSEC().ObtenerPlanesConVentaSISACT]", string.Format("[{0}][{1}][{2}][{3}][{4}][{5}][{6}][{7}]", CurrentTerminal, currentUser, strFlagPortabilidad, objDocumento.ID_SISACT, nroDocumento, listaTelefono, strCodRespVenta, strMsgRespVenta), null);

                //}// se quita flag porta

                objCliente.ListaMontoFactura = objListaMontoFactura;
                objCliente.ListaPlanesActivos = objListaPlanesActivos;

                // FIN PROY-26963 - GPRD - PROMFACT
                #endregion

                //INI: INICIATIVA-219
                objCliente.clienteCBIO = blDetalleLineaCBIO;

                if (objCliente.clienteCBIO)
                {
                    Participante objParticipante = new Participante();
                    string strIdentificador = "Detalle de Linea - Datos del Cliente";
                    objParticipante = objBLCbio.ObtenerDatosParticipanteMostrar(tipoDocumento, nroDocumento, strIdentificador);
                    objCliente.nombres = objParticipante.nombre;
                    objCliente.apellidoPaterno = objParticipante.apellidoPaterno;
                    objCliente.apellidoMaterno = objParticipante.apellidoMaterno;
                    objCliente.apellidos = string.Format("{0} {1}", objCliente.apellidoPaterno, objCliente.apellidoMaterno);
                    objCliente.razonSocial = objParticipante.razonSocial;

                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - objCliente.clienteCBIO = TRUE le asigna lo siguinte", ""), null);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - DataPowerCBIO - objCliente.nombres]: ", Funciones.CheckStr(objCliente.nombres)), null);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - DataPowerCBIO - objCliente.apellidoPaterno]: ", Funciones.CheckStr(objCliente.apellidoPaterno)), null);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - DataPowerCBIO - objCliente.apellidoMaterno]: ", Funciones.CheckStr(objCliente.apellidoMaterno)), null);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - DataPowerCBIO - objCliente.razonSocial]: ", Funciones.CheckStr(objCliente.razonSocial)), null);
                }
                //FIN: INICIATIVA-219

                // Detalle Lineas Prepago
                if (objDocumento.ID_SISACT == constCodTipoDocDNI && ConfigurationManager.AppSettings["consultarListaLineasVenta"].ToString() == "1")
                    objCliente.lineaPrepago = ProcesarDetallePrepago(objDocumento.DESCRIPCION, nroDocumento);

                // Calcular Monto Facturado, NO Facturado x Producto y Total Facturado
                if (objListaMontoFactura != null)
                    ObtenerMontoFactxBilletera(objListaMontoFactura, ref objCliente);

                // Validar BlackList de Créditos / WhiteList de Flexibilización
                bool isBlackList = true, isWhiteList = true, isTop = true;
                objConsumer.ConsultaTopBlackWhiteList(tipoDocumento, nroDocumento, objCliente.nroDiasDeuda, objCliente.deudaTotal, objCliente.nroPlanesActivos, objCliente.nroBloqueo,
                                                      ref isBlackList, ref isWhiteList, ref isTop);

                // Validación Deuda
                bool blnDeudaEvaluacion = false;

// INI PROY-32439 - NUEVOOAC
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][VALIDACION BLOQUEO X DEUDA]", ""), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][VALIDACION BLOQUEO X DEUDA][objCliente.deudaTotal]", objCliente.deudaTotal), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][VALIDACION BLOQUEO X DEUDA][dblUmbralDeuda]", dblUmbralDeuda), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][VALIDACION BLOQUEO X DEUDA][dblPorcentajeDeuda]", dblPorcentajeDeuda), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][VALIDACION BLOQUEO X DEUDA][objCliente.montoFacturadoTotal]", objCliente.montoFacturadoTotal), null);
// FIN PROY-32439 - NUEVOOAC

                if ((objCliente.deudaTotal > dblUmbralDeuda) && (objCliente.deudaTotal > (dblPorcentajeDeuda * objCliente.montoFacturadoTotal / 100)))
                {
                    blnDeudaEvaluacion = true;
                }

                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][VALIDACION BLOQUEO X DEUDA][blnDeudaEvaluacion]", blnDeudaEvaluacion), null); // PROY-32439 - NUEVOOAC

                //PROY-32439 INI MAS Captura datos Nuevo BRMS entrada
                #region ValidacionDeudaBRMS
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][INICIO]", ""), null);
                var strValidacionDeudaBRMS_RespuestaCodigo = String.Empty;
                var strValidacionDeudaBRMS_RespuestaMensaje = String.Empty;

                #region ConsultarRegionDepartamentoporOficina
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][REQUEST][PVUDB][ConsultarRegionDepartamentoporOficina][INICIO]", ""), null);
                var strDepartamentoDescripcion = String.Empty;
                var strRegionDescripcion = String.Empty;
                var strCanalDescripcion = String.Empty;
                var strNombreDescripcion = String.Empty;
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][REQUEST][PVUDB][ConsultarRegionDepartamentoporOficina][IN | Oficina]", Funciones.CheckStr(objSession.OficinaVenta)), null);
                var objDepartamentoRegionBRMS = (new BLSolicitud()).ConsultarRegionDepartamentoporOficina(objSession.OficinaVenta, out strValidacionDeudaBRMS_RespuestaCodigo, out strValidacionDeudaBRMS_RespuestaMensaje);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][REQUEST][PVUDB][ConsultarRegionDepartamentoporOficina][OUT | RespuestaCodigo]", strValidacionDeudaBRMS_RespuestaCodigo), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][REQUEST][PVUDB][ConsultarRegionDepartamentoporOficina][OUT | RespuestaMensaje]", strValidacionDeudaBRMS_RespuestaMensaje), null);
                if (!Object.Equals(objDepartamentoRegionBRMS, null))
                {
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][REQUEST][PVUDB][ConsultarRegionDepartamentoporOficina][OUT | objDepartamentoRegionBRMS]", "no es vacío"), null);
                    strDepartamentoDescripcion = Funciones.CheckStr(objDepartamentoRegionBRMS.DepavDescripcion);
                    strRegionDescripcion = Funciones.CheckStr(objDepartamentoRegionBRMS.RegionDescripcion);
                    strNombreDescripcion = Funciones.CheckStr(objDepartamentoRegionBRMS.OvenvDescripcion);
                    strCanalDescripcion = Funciones.CheckStr(objDepartamentoRegionBRMS.ToficCodigo);
                }
                else
                    {
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][REQUEST][PVUDB][ConsultarRegionDepartamentoporOficina][OUT | objDepartamentoRegionBRMS]", "vacío"), null);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][REQUEST][PVUDB][ConsultarRegionDepartamentoporOficina][OUT | strconsDatosNulos]", strconsDatosNulos), null);
                    objCliente.errorBrms = true;
                    objCliente.mensajeDeudaBloqueo = strconsDatosNulos;
                    return objCliente;
                }
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][REQUEST][PVUDB][ConsultarRegionDepartamentoporOficina][FIN]", ""), null);
                #endregion

                #region Disputa
                var objDisputaBRMS = new ValidacionDeudaBRMSrequest.Cliente.Disputa();
                objDisputaBRMS.antiguedad = intDisputaAntiguedad;
                objDisputaBRMS.cantidad = intDisputaCantidad;
                objDisputaBRMS.monto = dblDisputaMonto;
                #endregion
                        
                #region Documento
                string strDocumentoTipo = string.Empty;
                String[] DocumentosTipo = strtipodocparam.Split('|');
                foreach (String DocumentoTipo in DocumentosTipo)
                        {
                    foreach (String tipo in DocumentoTipo.Split(','))
                        {
                        if (tipo == tipoDocumento)
                            {
                            strDocumentoTipo = DocumentoTipo.Split(',')[1];
                            break;
                        }
                                }
                            }
                var objDocumentoBRMS = new ValidacionDeudaBRMSrequest.Cliente.Documento();
                objDocumentoBRMS.tipo = strDocumentoTipo;
                objDocumentoBRMS.numero = nroDocumento;
                #endregion

                #region Cliente
                var objClienteBRMS = new ValidacionDeudaBRMSrequest.Cliente();
                //objClienteBRMS.antiguedadDeuda = intDeudaAntiguedad;
                objClienteBRMS.bloqueos = lstBloqueosBRMS;
                objClienteBRMS.cantidadDocumentosDeuda = intDeudaCantidadDocumentos;
                objClienteBRMS.comportamientoPago = Funciones.CheckStr(objCliente.comportamientoPago);
                objClienteBRMS.disputa = objDisputaBRMS;
                objClienteBRMS.documento = objDocumentoBRMS;

                if (intFlagBloqueo == 1)
                                {
                    objClienteBRMS.flagBloqueos = ValidacionDeudaBRMSrequest.tipoSiNo.SI;
                                }
                else
                                {
                    objClienteBRMS.flagBloqueos = ValidacionDeudaBRMSrequest.tipoSiNo.NO;
                            }
                                   
                if (intFlagSuspension == 1)
                            {
                    objClienteBRMS.flagSuspensiones = ValidacionDeudaBRMSrequest.tipoSiNo.SI;
                            }
                else
                {
                    objClienteBRMS.flagSuspensiones = ValidacionDeudaBRMSrequest.tipoSiNo.NO;
                }

                objClienteBRMS.montoDeudaCastigada = dblDeudaCastigadaMonto;
                objClienteBRMS.montoDeudaVencida = dblDeudaVencidaMonto;
                objClienteBRMS.antiguedadDeuda = intAntiguedadDeuda;
                objClienteBRMS.montoDeuda = dblDeudaTotal; // PROY-32439 NUEVOOAC

                

                objClienteBRMS.montoTotalPago = dblPagoTotalMonto;
                objClienteBRMS.promedioFacturadoSoles = objCliente.montoFacturadoTotal + objCliente.montoNoFacturadoTotal;
                objClienteBRMS.segmento = String.Empty;
                objClienteBRMS.suspensiones = lstSuspensionesBRMS;
                objClienteBRMS.tiempoPermanencia = objCliente.tiempoPermanencia;
                objClienteBRMS.tiposFraude = lstFraudesBRMS;

                #region [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil]
                objClienteBRMS = ObtenerVariablesBRMSVtaCuotas(objClienteBRMS, nroDocumento, tipoDocumento, currentUser);
                #endregion

                #endregion

                #region LineaARenovar
                ValidacionDeudaBRMSrequest.LineaARenovar objLineaARenovarBRMS = null;
                #endregion
                      
                #region Vendedor
                var objVendedorBRMS = new ValidacionDeudaBRMSrequest.PuntoDeVenta.Vendedor();
                objVendedorBRMS.codigo = objSession.idCuentaRed;
                objVendedorBRMS.nombre = objSession.NombreCompleto;
                #endregion

                #region PuntoDeVenta
                var objPuntoDeVentaBRMS = new ValidacionDeudaBRMSrequest.PuntoDeVenta()
                {
                    canal = strCanalDescripcion,
                    codigo = objSession.OficinaVenta,
                    departamento = strDepartamentoDescripcion,
                    nombre = strNombreDescripcion,
                    region = strRegionDescripcion,
                    segmento = String.Empty,
                    vendedor = objVendedorBRMS
                };
                #endregion

                #region Request
                var objValidacionDeudaBRMSrequest = new ValidacionDeudaBRMSrequest();
                objValidacionDeudaBRMSrequest.decisionID = String.Empty;
                objValidacionDeudaBRMSrequest.cliente = objClienteBRMS;
                objValidacionDeudaBRMSrequest.lineaARenovar = objLineaARenovarBRMS;
                objValidacionDeudaBRMSrequest.puntoDeVenta = objPuntoDeVentaBRMS;
                objValidacionDeudaBRMSrequest.sistemaEvaluacion = strsistemaEvaluacion;
                objValidacionDeudaBRMSrequest.tipoOperacion = strtipoOperacion;
                #endregion

                #region Response
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][INICIO]", ""), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][IN | objValidacionDeudaBRMSrequest.decisionID]", objValidacionDeudaBRMSrequest.decisionID), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][IN | objValidacionDeudaBRMSrequest.cliente.antiguedadDeuda]", objValidacionDeudaBRMSrequest.cliente.antiguedadDeuda), null);

                if (!Object.Equals(objValidacionDeudaBRMSrequest.cliente.bloqueos, null))
                {
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][IN | objValidacionDeudaBRMSrequest.cliente.bloqueos]", objValidacionDeudaBRMSrequest.cliente.bloqueos.Count), null);
                }
                else
                {
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][IN | objValidacionDeudaBRMSrequest.cliente.bloqueos]", "vacío"), null);
                            }                        

                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][IN | objValidacionDeudaBRMSrequest.cliente.cantidadDocumentosDeuda]", objValidacionDeudaBRMSrequest.cliente.cantidadDocumentosDeuda), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][IN | objValidacionDeudaBRMSrequest.cliente.comportamientoPago]", objValidacionDeudaBRMSrequest.cliente.comportamientoPago), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][IN | objValidacionDeudaBRMSrequest.cliente.disputa.antiguedad]", objValidacionDeudaBRMSrequest.cliente.disputa.antiguedad), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][IN | objValidacionDeudaBRMSrequest.cliente.disputa.cantidad]", objValidacionDeudaBRMSrequest.cliente.disputa.cantidad), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][IN | objValidacionDeudaBRMSrequest.cliente.disputa.monto]", objValidacionDeudaBRMSrequest.cliente.disputa.monto), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][IN | objValidacionDeudaBRMSrequest.cliente.documento.numero]", objValidacionDeudaBRMSrequest.cliente.documento.numero), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][IN | objValidacionDeudaBRMSrequest.cliente.documento.tipo]", objValidacionDeudaBRMSrequest.cliente.documento.tipo), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][IN | objValidacionDeudaBRMSrequest.cliente.flagBloqueos]", objValidacionDeudaBRMSrequest.cliente.flagBloqueos), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][IN | objValidacionDeudaBRMSrequest.cliente.flagSuspensiones]", objValidacionDeudaBRMSrequest.cliente.flagSuspensiones), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][IN | objValidacionDeudaBRMSrequest.cliente.montoDeuda]", objValidacionDeudaBRMSrequest.cliente.montoDeuda), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][IN | objValidacionDeudaBRMSrequest.cliente.montoDeudaCastigada]", objValidacionDeudaBRMSrequest.cliente.montoDeudaCastigada), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][IN | objValidacionDeudaBRMSrequest.cliente.montoDeudaVencida]", objValidacionDeudaBRMSrequest.cliente.montoDeudaVencida), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][IN | objValidacionDeudaBRMSrequest.cliente.montoTotalPago]", objValidacionDeudaBRMSrequest.cliente.montoTotalPago), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][IN | objValidacionDeudaBRMSrequest.cliente.promedioFacturadoSoles]", objValidacionDeudaBRMSrequest.cliente.promedioFacturadoSoles), null);

                if (!Object.Equals(objValidacionDeudaBRMSrequest.cliente.suspensiones, null))
                {
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][IN | objValidacionDeudaBRMSrequest.cliente.suspensiones]", objValidacionDeudaBRMSrequest.cliente.suspensiones.Count), null);
                }
                else
                {
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][IN | objValidacionDeudaBRMSrequest.cliente.suspensiones]", "vacío"), null);
                    }

                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][IN | objValidacionDeudaBRMSrequest.cliente.tiempoPermanencia]", objValidacionDeudaBRMSrequest.cliente.tiempoPermanencia), null);

                if (!Object.Equals(objValidacionDeudaBRMSrequest.cliente.tiposFraude, null))
                {
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][IN | objValidacionDeudaBRMSrequest.cliente.tiposFraude]", objValidacionDeudaBRMSrequest.cliente.tiposFraude.Count), null);
                }
                else
                {
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][IN | objValidacionDeudaBRMSrequest.cliente.tiposFraude]", "vacío"), null);
                }

                if (!Object.Equals(objValidacionDeudaBRMSrequest.lineaARenovar, null))
                {
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][IN | objValidacionDeudaBRMSrequest.lineaARenovar.antiguedad]", objValidacionDeudaBRMSrequest.lineaARenovar.antiguedad), null);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][IN | objValidacionDeudaBRMSrequest.lineaARenovar.fechaActivacion]", objValidacionDeudaBRMSrequest.lineaARenovar.fechaActivacion), null);
                }
                else
                {
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][IN | objValidacionDeudaBRMSrequest.lineaARenovar.antiguedad]", "vacío"), null);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][IN | objValidacionDeudaBRMSrequest.lineaARenovar.fechaActivacion]", "vacío"), null);
                }
               
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][IN | objValidacionDeudaBRMSrequest.puntoDeVenta.canal]", objValidacionDeudaBRMSrequest.puntoDeVenta.canal), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][IN | objValidacionDeudaBRMSrequest.puntoDeVenta.codigo]", objValidacionDeudaBRMSrequest.puntoDeVenta.codigo), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][IN | objValidacionDeudaBRMSrequest.puntoDeVenta.departamento]", objValidacionDeudaBRMSrequest.puntoDeVenta.departamento), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][IN | objValidacionDeudaBRMSrequest.puntoDeVenta.nombre]", objValidacionDeudaBRMSrequest.puntoDeVenta.nombre), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][IN | objValidacionDeudaBRMSrequest.puntoDeVenta.region]", objValidacionDeudaBRMSrequest.puntoDeVenta.region), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][IN | objValidacionDeudaBRMSrequest.puntoDeVenta.vendedor.codigo]", objValidacionDeudaBRMSrequest.puntoDeVenta.vendedor.codigo), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][IN | objValidacionDeudaBRMSrequest.puntoDeVenta.vendedor.nombre]", objValidacionDeudaBRMSrequest.puntoDeVenta.vendedor.nombre), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][IN | objValidacionDeudaBRMSrequest.sistemaEvaluacion]", objValidacionDeudaBRMSrequest.sistemaEvaluacion), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][IN | objValidacionDeudaBRMSrequest.tipoOperacion]", objValidacionDeudaBRMSrequest.tipoOperacion), null);
                var objResponse = (new BWNuevaReglasCreditica()).ConsultaReglaCrediticiaNVOBRMS(objValidacionDeudaBRMSrequest, out strValidacionDeudaBRMS_RespuestaCodigo, out strValidacionDeudaBRMS_RespuestaMensaje);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][OUT | RespuestaCodigo]", strValidacionDeudaBRMS_RespuestaCodigo), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][OUT | RespuestaMensaje]", strValidacionDeudaBRMS_RespuestaMensaje), null);
                var objValidacionDeudaBRMSresponse = new ValidacionDeudaBRMSresponse();

                //PROY-32439 - CAMBIOS_LOG INI
                string strMensajeErrorBRMS = "";
                Int64 intFlagErrorBRMS = 0;
                string strResProComercial = "";
                string strResProducto = "";
                string strResTipoOperacion = "";
                BLReglaNegocio objNuevoBRMS = new BLReglaNegocio();
                Int64 strFlagWhilist = 0;
                Int64 strFlagTieneDeuda = 0;
                //PROY-32439 - CAMBIOS_LOG FIN

                if (!Object.Equals(objResponse, null))
                {
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][OUT | objResponse]", "no es vacío"), null);

                    //decisionID
                    objValidacionDeudaBRMSresponse.decisionID = objResponse.DecisionID;

                    //validacionCliente
                    if (objResponse.respuesta.respuesta1.validacionCliente == tipoSiNo.SI)
                    {
                        objValidacionDeudaBRMSresponse.validacionCliente = ValidacionDeudaBRMSresponse.tipoSiNo.SI;
                    }
                    else
                    {
                        objValidacionDeudaBRMSresponse.validacionCliente = ValidacionDeudaBRMSresponse.tipoSiNo.NO;
                    }

                    objValidacionDeudaBRMSresponse.mensajeValidacionCliente = objResponse.respuesta.respuesta1.mensajeValidacionCliente;

                    //restriccionProductoComercial
                    if (!Object.Equals(objResponse.respuesta.respuesta1.restriccionTipoOperacion, null))
                {
                        objValidacionDeudaBRMSresponse.restriccionProductoComercial = objResponse.respuesta.respuesta1.restriccionTipoOperacion.ToList();
                        //PROY-32439 - CAMBIOS_LOG INI
                        foreach (string word1 in objValidacionDeudaBRMSresponse.restriccionProductoComercial)
                        {
                            strResProComercial += "|" + word1;
                        }
                        //PROY-32439 - CAMBIOS_LOG FIN
                    }
                    else
                    {
                        objValidacionDeudaBRMSresponse.restriccionProductoComercial = null;
                        //PROY-32439 - CAMBIOS_LOG INI
                        strResProComercial = "";
                        //PROY-32439 - CAMBIOS_LOG FIN
                }

                    //restriccionProducto
                    if (!Object.Equals(objResponse.respuesta.respuesta1.restriccionProductoComercial, null))
                    {
                        objValidacionDeudaBRMSresponse.restriccionProducto = objResponse.respuesta.respuesta1.restriccionProductoComercial.ToList();
                        //PROY-32439 - CAMBIOS_LOG INI
                        foreach (string word2 in objValidacionDeudaBRMSresponse.restriccionProducto)
                        {
                            strResProducto += "|" + word2;
                        }
                        //PROY-32439 - CAMBIOS_LOG FIN
                    }
                    else
                    {
                        objValidacionDeudaBRMSresponse.restriccionProducto = null;
                         //PROY-32439 - CAMBIOS_LOG INI
                        strResProducto = "";
                        //PROY-32439 - CAMBIOS_LOG FIN
                    }

                    //restriccionTipoOperacion
                    if (!Object.Equals(objResponse.respuesta.respuesta1.restriccionProducto, null))
                    {
                        objValidacionDeudaBRMSresponse.restriccionTipoOperacion = objResponse.respuesta.respuesta1.restriccionProducto.ToList();
                        //PROY-32439 - CAMBIOS_LOG INI
                        foreach (string word3 in objValidacionDeudaBRMSresponse.restriccionTipoOperacion)
                        {
                            strResTipoOperacion += "|" + word3;
                        }
                        //PROY-32439 - CAMBIOS_LOG FIN

                    }
                    else
                    {
                        objValidacionDeudaBRMSresponse.restriccionTipoOperacion = null;
                        //PROY-32439 - CAMBIOS_LOG INI
                        strResTipoOperacion = "";
                        //PROY-32439 - CAMBIOS_LOG FIN
                    }

                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][OUT | DecisionID]", objValidacionDeudaBRMSresponse.decisionID), null);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][OUT | validacionCliente]", objValidacionDeudaBRMSresponse.validacionCliente), null);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][OUT | mensajeValidacionCliente]", Funciones.CheckStr(objValidacionDeudaBRMSresponse.mensajeValidacionCliente)), null);
                    if (!Object.Equals(objValidacionDeudaBRMSresponse.restriccionTipoOperacion, null))
                    {
                        _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][OUT | restriccionTipoOperacion]", objValidacionDeudaBRMSresponse.restriccionTipoOperacion.Count), null);
                    }
                    else
                    {
                        _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][OUT | restriccionTipoOperacion]", "VACIO"), null);
                    }
                    if (!Object.Equals(objValidacionDeudaBRMSresponse.restriccionProductoComercial, null))
                    {
                        _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][OUT | restriccionProductoComercial]", objValidacionDeudaBRMSresponse.restriccionProductoComercial.Count), null);
                    }
                    else
                    {
                        _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][OUT | restriccionProductoComercial]", "VACIO"), null);
                    }
                    if (!Object.Equals(objValidacionDeudaBRMSresponse.restriccionProducto, null))
                    {
                        _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][OUT | restriccionProducto]", objValidacionDeudaBRMSresponse.restriccionProducto.Count), null);
                    }
                    else
                    {
                        _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][OUT | restriccionProducto]", "VACIO"), null);
                    }


                    //PROY-32439 - CAMBIOS_LOG INI
                    if (isWhiteList == true)
                    {
                        strFlagWhilist = 1;
                    }
                    if (blnDeudaEvaluacion == true)
                    {
                        strFlagTieneDeuda = 1;
                    }

                    if (Equals(strPagina,true))
                    {
                        string strMensajeNuevo = Funciones.CheckStr(HttpContext.Current.Session["strMensajeError"]) == string.Empty ? "SI" : string.Empty; //PROY-140743
                        objNuevoBRMS.InsertarLogNuevoBRMS(objValidacionDeudaBRMSrequest, strValTipoBloqueoBRMS, strValTipoLineaBloqueoBRMS, strValTipoSusBRMS, strValTipoLineaSusBRMS, strTipoFraudeBRMS, objValidacionDeudaBRMSresponse, strFlagWhilist, strFlagTieneDeuda, objSession.idCuentaRed, intFlagErrorBRMS, strMensajeErrorBRMS, strResProComercial, strResProducto, strResTipoOperacion, strMensajeNuevo); //PROY-140743
                    }
                    //PROY-32439 - CAMBIOS_LOG FIN

                }
                else
                {
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][OUT | objResponse]", "vacío"), null);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][OUT | strconsBrmsCaido]", strconsBrmsCaido), null);
                    objCliente.errorBrms = true;
                    objCliente.mensajeDeudaBloqueo = strconsBrmsCaido;

                    //PROY-32439 - CAMBIOS_LOG INI
                    intFlagErrorBRMS = 1;
                    strMensajeErrorBRMS = strconsBrmsCaido;
                    if (isWhiteList == true)
                {
                    strFlagWhilist = 1;
                }
                if (blnDeudaEvaluacion == true)
                {
                    strFlagTieneDeuda = 1;
                }

                if (Equals(strPagina, true))
                {
                        string strMensajeNuevo = Funciones.CheckStr(HttpContext.Current.Session["strMensajeError"]) == string.Empty ? "SI" : string.Empty; //PROY-140743
                        objNuevoBRMS.InsertarLogNuevoBRMS(objValidacionDeudaBRMSrequest, strValTipoBloqueoBRMS, strValTipoLineaBloqueoBRMS, strValTipoSusBRMS, strValTipoLineaSusBRMS, strTipoFraudeBRMS, objValidacionDeudaBRMSresponse, strFlagWhilist, strFlagTieneDeuda, objSession.idCuentaRed, intFlagErrorBRMS, strMensajeErrorBRMS, strResProComercial, strResProducto, strResTipoOperacion, strMensajeNuevo); //PROY-140743
                }
                //PROY-32439 - CAMBIOS_LOG FIN

                    return objCliente;
                }
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][ConsultaReglaCrediticiaNVOBRMS][FIN]", ""), null);
                #endregion

                #region Session
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][SESSION][INICIO]", ""), null);
                Boolean blnRRLL = false;
                if (!Object.Equals(HttpContext.Current.Session["intFlagRRLL"], null))
                {
                    blnRRLL = (Boolean)HttpContext.Current.Session["intFlagRRLL"];
                }
                var objValidacionDeudaBRMS = new ValidacionDeudaBRMS();
                if (blnRRLL)
                {
                    ValidacionDeudaBRMS objRUC = new ValidacionDeudaBRMS();
                    objRUC = (ValidacionDeudaBRMS)HttpContext.Current.Session["ObjNvoBRMS"];
                    objValidacionDeudaBRMS.request = objValidacionDeudaBRMSrequest;
                    objValidacionDeudaBRMS.response = objValidacionDeudaBRMSresponse;
                    objRUC.clientes.Add(objValidacionDeudaBRMS);
                    HttpContext.Current.Session["ObjNvoBRMS"] = objRUC;
                }
                else
                {
                    objValidacionDeudaBRMS.request = objValidacionDeudaBRMSrequest;
                    objValidacionDeudaBRMS.response = objValidacionDeudaBRMSresponse;
                    objValidacionDeudaBRMS.clientes = new List<ValidacionDeudaBRMS>();
                    HttpContext.Current.Session["ObjNvoBRMS"] = objValidacionDeudaBRMS;
                }
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][SESSION][FIN]", ""), null);
                #endregion

                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][ValidacionDeudaBRMS][FIN]", ""), null);
                #endregion
                //PROY-32439 FIN MAS Captura datos Nuevo BRMS entrada

                //PROY-32439 INI MAS Validacion de Planes
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][RESTRICCION][INICIO]", ""), null);
                ValidacionDeudaBRMSresponse.tipoSiNo ValidacionDeudaBRMS = objValidacionDeudaBRMSresponse.validacionCliente;
                string strSoloEvaluarFijo = string.Empty;
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][RESTRICCION][ValidacionDeudaBRMS]", ValidacionDeudaBRMS), null);

                #region Ruc y RRLL
                if (blnRRLL)
                {
                    Boolean blnRRLLBRMS = false;
                    if (ValidacionDeudaBRMS == ValidacionDeudaBRMSresponse.tipoSiNo.SI)
                    {
                        blnRRLLBRMS = true;
                    }
                    HttpContext.Current.Session["blnRRLLBRMS"] = blnRRLLBRMS;
                    Boolean blnRRWhiteList = false;
                    if (isWhiteList)
                    {
                        blnRRWhiteList = true;
                    }

                    HttpContext.Current.Session["blnRRWhiteList"] = blnRRWhiteList;
                    objCliente.deudaCliente = "";
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][RESTRICCION][Temporal][blnRRLLBRMS]", blnRRLLBRMS.ToString()), null);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][RESTRICCION][Temporal][blnRRWhiteList]", blnRRWhiteList.ToString()), null);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][RESTRICCION][FIN]", ""), null);
                }
                else
                {
                    if (ValidacionDeudaBRMS == ValidacionDeudaBRMSresponse.tipoSiNo.NO)
                    {
                        // Validar si SOLO se ofrece producto Fijo
                        if (!isWhiteList)
                        {
                            if (blnDeudaEvaluacion)
                            {
                                strSoloEvaluarFijo = "T";
                            }
                            else
                            {
                                strSoloEvaluarFijo = "P";
                            }
                        }
                    }
                    HttpContext.Current.Session["strSoloEvaluarFijo"] = strSoloEvaluarFijo;
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][RESTRICCION][isWhiteList]", isWhiteList.ToString()), null);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][RESTRICCION][strSoloEvaluarFijo]", strSoloEvaluarFijo), null);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][RESTRICCION][FIN]", ""), null);
                    //PROY-32439 FIN MAS Validacion de Planes
                }
                //PROY-29121-INI
                if (blnDeudaEvaluacion)
                {
                    objCliente.deudaCliente = "SI";
                }
                else
                {
                    objCliente.deudaCliente = "NO";
                }
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][DEUDA_CLIENTE][INICIO]", ""), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][DEUDA_CLIENTE][deudaCliente]", Funciones.CheckStr(objCliente.deudaCliente)), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][DEUDA_CLIENTE][FIN]", ""), null);
                #endregion

                //PROY-29121-FIN

                string strCategoriaCliente = objCliente.esClienteClaro ? "CLIENTE CLARO" : "CLIENTE NUEVO";
                strCategoriaCliente += isTop ? " TOP" : "";

                objCliente.isBlackList = isBlackList;
                objCliente.isWhiteList = isWhiteList;
                objCliente.isTop = isTop;
                objCliente.tipoCliente = strCategoriaCliente;
                objCliente.soloEvaluarFijo = strSoloEvaluarFijo;
                objCliente.oPlanesActivosxBilletera = objListaPlanesActivos;
                //PROY-32439 INI MAS Mensaje por parte de Nvo BRMS
                objCliente.cumpleReglaA = "";
                objCliente.mensajeDeudaBloqueo = Funciones.CheckStr(objValidacionDeudaBRMSresponse.mensajeValidacionCliente);
                //PROY-32439 FIN MAS Mensaje por parte de Nvo BRMS
                if (objDetalleOAC != null && objDetalleOAC[0].xCuentas.Length > 0)
                {
                    objCliente.oOAC = new ArrayList();
                    objCliente.oOAC.Add(objDetalleOAC[0].xCuentas);
                }

                //INC000003443673 - INI
                _objLog.CrearArchivolog(null, string.Format("{0}{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - Cambio del valor de Apellido Paterno/Materno a punto INI", ""), null);
                _objLog.CrearArchivolog(null, string.Format("{0}{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - ReadKeySettings.Key_ValorSinApellidoPaternoOMaterno: ", Funciones.CheckStr(ReadKeySettings.Key_ValorSinApellidoPaternoOMaterno)), null);
                _objLog.CrearArchivolog(null, string.Format("{0}{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - objCliente.nombres: ", Funciones.CheckStr(objCliente.nombres)), null);
                _objLog.CrearArchivolog(null, string.Format("{0}{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - objCliente.apellidoPaterno: ", Funciones.CheckStr(objCliente.apellidoPaterno)), null);
                _objLog.CrearArchivolog(null, string.Format("{0}{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - objCliente.apellidoMaterno: ", Funciones.CheckStr(objCliente.apellidoMaterno)), null);
                _objLog.CrearArchivolog(null, string.Format("{0}{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - objCliente.razonSocial: ", Funciones.CheckStr(objCliente.razonSocial)), null);

                if (objCliente.apellidoPaterno != string.Empty || objCliente.apellidoMaterno != string.Empty)
                {
                    _objLog.CrearArchivolog(null, string.Format("{0}{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - Comienza la validacion de Apellido Paterno", ""), null);
                    if (Funciones.CheckStr(ReadKeySettings.Key_ValorSinApellidoPaternoOMaterno).IndexOf(Funciones.CheckStr(objCliente.apellidoPaterno)) > -1)
                    {
                        _objLog.CrearArchivolog(null, string.Format("{0}{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - Cliente sin apellido paterno", ""), null);
                        objCliente.apellidoPaterno = ".";
                        _objLog.CrearArchivolog(null, string.Format("{0}{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - objCliente.apellidoPaterno: ", Funciones.CheckStr(objCliente.apellidoPaterno)), null);
                    }
                    _objLog.CrearArchivolog(null, string.Format("{0}{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - Comienza la validacion de Apellido Materno", ""), null);
                    if (Funciones.CheckStr(ReadKeySettings.Key_ValorSinApellidoPaternoOMaterno).IndexOf(Funciones.CheckStr(objCliente.apellidoMaterno)) > -1)
                    {
                        _objLog.CrearArchivolog(null, string.Format("{0}{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - Cliente sin apellido materno", ""), null);
                        objCliente.apellidoMaterno = ".";
                        _objLog.CrearArchivolog(null, string.Format("{0}{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - objCliente.apellidoMaterno: ", Funciones.CheckStr(objCliente.apellidoMaterno)), null);
                    }
                    objCliente.apellidos = Funciones.CheckStr(objCliente.apellidoPaterno + " " + objCliente.apellidoMaterno);
                    _objLog.CrearArchivolog(null, string.Format("{0}{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - objCliente.apellidos: ", Funciones.CheckStr(objCliente.apellidos)), null);
                }
                _objLog.CrearArchivolog(null, string.Format("{0}{1}", "[INC000003443673 - BLDatosCliente - ConsultarDatosCliente() - Cambio del valor de Apellido Paterno/Materno a punto FIN", ""), null);
                //INC000003443673 - FIN

                // PROY-140743 IDEA141192 NUEVAS VARIABLES BRMS VTACUO VICTOR CANCHICA INI  
                List<BEObtenerDatosPedidoAccCuotas> DatosPedidoAccCuotas = new List<BEObtenerDatosPedidoAccCuotas>();
                DatosPedidoAccCuotas = BWReglasCreditica.ObtenerDatosPedidoAccCuotas("", "", nroDocumento, "", objSession.Login, objSession.idCuentaRed);
                objCliente.DatosPedidoAccCuotas = DatosPedidoAccCuotas;
                // PROY-140743 IDEA141192 NUEVAS VARIABLES BRMS VTACUO VICTOR CANCHICA FIN

            }
            catch (Exception ex)
            {
                _objLog.CrearArchivolog("ERROR DATOS CLIENTE", null, ex);
                throw new Exception(ex.Message);
            }
            return objCliente;
        }

        private void ProcesarDetalleBSCS(DataTable dtDetalleBSCS, ref List<BEPlanBilletera> objListaMontoFactura, ref List<BEPlanBilletera> objListaPlanActivos, ref int intMesesPermanencia, ref System.DateTime datFechaActiva)//PROY-140743
        {
            int intMeses = 0;
            intMesesPermanencia = 0;
            _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][DetalleBSCS][ProcesarDetalleBSCS] ", "INICIO"), null);
            if (objListaMontoFactura == null) objListaMontoFactura = new List<BEPlanBilletera>();
            List<String> objListaCuenta = new List<String>();
            BEPlanBilletera objBilletera;
            String strCuenta;

            foreach (DataRow dr in dtDetalleBSCS.Rows)
            {
                strCuenta = dr["CUENTA"].ToString();
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][DetalleBSCS][ProcesarDetalleBSCS]strCuenta: ", Funciones.CheckStr(strCuenta)), null);
                if (!objListaCuenta.Contains(strCuenta))
                {
					if (dr["ESTADO"].ToString().ToUpper() == "ACTIVO")
					{
                    objBilletera = new BEPlanBilletera();
                    objBilletera.cuenta = strCuenta;
                    objBilletera.montoFacturado = Funciones.CheckDbl(dr["PROM_FACT"], 2);
                    objBilletera.montoNoFacturado = Funciones.CheckDbl(dr["MONTO_NO_FACT"], 2);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][DetalleBSCS][ProcesarDetalleBSCS]objBilletera.cuenta: ", Funciones.CheckStr(objBilletera.cuenta)), null);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][DetalleBSCS][ProcesarDetalleBSCS]objBilletera.montoFacturado: ", Funciones.CheckStr(objBilletera.montoFacturado)), null);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][DetalleBSCS][ProcesarDetalleBSCS]objBilletera.montoNoFacturado: ", Funciones.CheckStr(objBilletera.montoNoFacturado)), null);

                    int intCodBilletera = 0;
                    if (Funciones.CheckInt(dr["NRO_MOVIL"]) > 0) intCodBilletera += (int)BEBilletera.TIPO_BILLETERA.MOVIL;
                    if (Funciones.CheckInt(dr["NRO_INTERNET_FIJO"]) > 0) intCodBilletera += (int)BEBilletera.TIPO_BILLETERA.INTERNET;
                    if (Funciones.CheckInt(dr["NRO_CLARO_TV"]) > 0) intCodBilletera += (int)BEBilletera.TIPO_BILLETERA.CLAROTV;
                    if (Funciones.CheckInt(dr["NRO_TELEF_FIJA"]) > 0) intCodBilletera += (int)BEBilletera.TIPO_BILLETERA.TELEFONIA;
                    if (Funciones.CheckInt(dr["NRO_BAM"]) > 0) intCodBilletera += (int)BEBilletera.TIPO_BILLETERA.BAM;
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][DetalleBSCS][ProcesarDetalleBSCS]intCodBilletera: ", Funciones.CheckStr(intCodBilletera)), null);
                    objBilletera.idBilletera = intCodBilletera;
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][DetalleBSCS][ProcesarDetalleBSCS]objBilletera.idBilletera: ", Funciones.CheckStr(objBilletera.idBilletera)), null);
                    objListaMontoFactura.Add(objBilletera);
                    objListaCuenta.Add(strCuenta);
                }
                }

                if (tipoDocumento != constCodTipoDocRUC)
                {
                    //Consulta Apadece Vigente SIGA PENDIENTE

                    if (dr["ESTADO"].ToString().ToUpper() == "ACTIVO" && dr["FECHA_ACTIVACION"].ToString() != String.Empty)
                    {
                        datFechaActiva = DateTime.Parse(dr["FECHA_ACTIVACION"].ToString());
                        intMeses = (DateTime.Now.Subtract(datFechaActiva).Days / 30);
                        if (intMesesPermanencia < intMeses) intMesesPermanencia = intMeses;
                    }
                }
                else
                {
                    if (dr["FECHA_ACTIVACION"].ToString() != String.Empty)
                    {
                        datFechaActiva = DateTime.Parse(dr["FECHA_ACTIVACION"].ToString());
                        intMeses = (DateTime.Now.Subtract(datFechaActiva).Days / 30);
                        if (intMesesPermanencia < intMeses) intMesesPermanencia = intMeses;
                    }
                }
            }

            // Detalle Cantidad de Planes x Billetera
            _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][DetalleBSCS][ProcesarDetalleBSCS]objDocumento.ID_BSCS: ", Funciones.CheckStr(objDocumento.ID_BSCS)), null);
            _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][DetalleBSCS][ProcesarDetalleBSCS]nroDocumento: ", Funciones.CheckStr(nroDocumento)), null);
            DataTable dtListaCantPlanBSCS = (new BLConsumer()).ListarCantPlanxBilleteraBSCS(objDocumento.ID_BSCS, nroDocumento);

            //INI: INICIATIVA-219
            BLDatosCBIO objBLCbio = new BLDatosCBIO();
            objBLCbio.ListarCantPlanxBilleteraCBIO(ref dtListaCantPlanBSCS);
            //FIN: INICIATIVA-219

            objListaPlanActivos = ProcesarCantPlanBSCS(dtListaCantPlanBSCS);
        }

        private void ProcesarDetalleSGA(DataTable dtDetalleSGA, ref List<BEPlanBilletera> objListaMontoFactura, ref List<BEPlanBilletera> objListaPlanActivos, ref int intMesesPermanencia, ref System.DateTime datFechaActiva)//PROY-140743
        {
            int intMeses = 0;
            intMesesPermanencia = 0;
            BEPlanBilletera objPlan;
            int intSistemaSGA = (int)BEPlanBilletera.TIPO_SISTEMA.SGA;

            _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][DetalleSGA][ProcesarDetalleSGA][INICIO] intSistemaSGA: ", Funciones.CheckStr(intSistemaSGA)), null);

            List<BEPlanBilletera> objListaPlan = (new BLEvaluacion()).ObtenerPlanesxBilletera(intSistemaSGA);
            String strCuenta; //PROY-26963 GPRD - PROMFACT
            List<String> objListaCuenta = new List<String>();//PROY-26963 GPRD - PROMFACT

            if (objListaMontoFactura == null) objListaMontoFactura = new List<BEPlanBilletera>();
            if (objListaPlanActivos == null) objListaPlanActivos = new List<BEPlanBilletera>();

            foreach (DataRow dr in dtDetalleSGA.Rows)
            {
                strCuenta = dr["CODCLI"].ToString();//PROY-26963-GPRD - PROMFACT
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][DetalleSGA][ProcesarDetalleSGA] strCuenta: ", Funciones.CheckStr(strCuenta)), null);
                if (!objListaCuenta.Contains(strCuenta))//PROY-26963-GPRD - PROMFACT
                {//PROY-26963-GPRD - PROMFACT
                if (dr["ESTADO"].ToString().ToUpper() == "ACTIVO")
                {
                    objPlan = new BEPlanBilletera();
                    objPlan.plan = dr["IDPAQ"].ToString();
                    objPlan.montoFacturado = Funciones.CheckDbl(dr["PROM_FAC"], 2);
                    objPlan.montoNoFacturado = Funciones.CheckDbl(dr["MONTO_NO_FAC"], 2);

                    objPlan.tipoFacturador = BEPlanBilletera.TIPO_FACTURADOR.SGA;
                    objPlan.nroPlanes = 1;

                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][DetalleSGA][ProcesarDetalleSGA] objPlan.plan: ", Funciones.CheckStr(objPlan.plan)), null);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][DetalleSGA][ProcesarDetalleSGA] objPlan.montoFacturado: ", Funciones.CheckStr(objPlan.montoFacturado)), null);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][DetalleSGA][ProcesarDetalleSGA] objPlan.montoNoFacturado: ", Funciones.CheckStr(objPlan.montoNoFacturado)), null);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][DetalleSGA][ProcesarDetalleSGA] objPlan.tipoFacturador: ", Funciones.CheckStr(objPlan.tipoFacturador)), null);


                    List<BEBilletera> objListaBilletera = null;
                    foreach (BEPlanBilletera obj in objListaPlan)
                    {
                        _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][DetalleSGA][ProcesarDetalleSGA] objPlan.plan: ", Funciones.CheckStr(objPlan.plan)), null);
                        _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][DetalleSGA][ProcesarDetalleSGA] obj.plan: ", Funciones.CheckStr(obj.plan)), null);
                        if (obj.plan == objPlan.plan)
                        {
                            objListaBilletera = obj.oBilletera;
                            break;
                        }
                    }

                    if (objListaBilletera != null)
                    {
                        _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][DetalleSGA][ProcesarDetalleSGA] objListaBilletera != null ", ""), null);
                        objPlan.oBilletera = new List<BEBilletera>();
                        foreach (BEBilletera obj in objListaBilletera)
                        {
                            _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][DetalleSGA][ProcesarDetalleSGA] obj.idBilletera: ", Funciones.CheckStr(obj.idBilletera)), null);
                            _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][DetalleSGA][ProcesarDetalleSGA] objPlan.idBilletera: ", Funciones.CheckStr(objPlan.idBilletera)), null);
                            objPlan.idBilletera += obj.idBilletera;
                            objPlan.oBilletera.Add(new BEBilletera(obj.idBilletera, 1));
                            _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][DetalleSGA][ProcesarDetalleSGA]objPlan.idBilletera += obj.idBilletera: ", Funciones.CheckStr(objPlan.idBilletera)), null);
                        }
                    }
                    else
                    {
                        objPlan.idBilletera = (int)BEBilletera.TIPO_BILLETERA.TRIPLEPLAY;
                        _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][DetalleSGA][ProcesarDetalleSGA] ", Funciones.CheckStr(objPlan.idBilletera)), null);
                        objPlan.oBilletera = new List<BEBilletera>();
                        objPlan.oBilletera.Add(new BEBilletera((int)(BEBilletera.TIPO_BILLETERA.INTERNET), 1));
                        objPlan.oBilletera.Add(new BEBilletera((int)(BEBilletera.TIPO_BILLETERA.CLAROTV), 1));
                        objPlan.oBilletera.Add(new BEBilletera((int)(BEBilletera.TIPO_BILLETERA.TELEFONIA), 1));
                    }

                    objListaPlanActivos.Add(objPlan);
                    objListaMontoFactura.Add(objPlan);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][ProcesarDetalleSGA][objListaMontoFactura]: ", "objListaMontoFactura.Add(objPlan)"), null);
                        objListaCuenta.Add(strCuenta); //PROY-26963-GPRD - PROMFACT

                    if (dr["FECHA_ACTIVACION"].ToString() != String.Empty)
                    {
                        datFechaActiva = DateTime.Parse(dr["FECHA_ACTIVACION"].ToString());
                        intMeses = (DateTime.Now.Subtract(datFechaActiva).Days / 30);
                        if (intMesesPermanencia < intMeses) intMesesPermanencia = intMeses;
                    }
                }
                }////PROY-26963-GPRD - PROMFACT
            }
        }

        private void ProcesarDetalleSISACT(DataTable dtDetalleSISACT, ref List<BEPlanBilletera> objListaMontoFactura, ref List<BEPlanBilletera> objListaPlanActivos)
        {
            _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][ProcesarDetalleSISACT][INICIO] dtDetalleSISACT.Rows.Count: ", Funciones.CheckStr(dtDetalleSISACT.Rows.Count)), null);
            
            BEPlanBilletera objPlan;
            int intSistemaSISACT = (int)BEPlanBilletera.TIPO_SISTEMA.SISACT;
            _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][ProcesarDetalleSISACT] intSistemaSISACT: ", Funciones.CheckStr(intSistemaSISACT)), null);
            List<BEPlanBilletera> objListaPlan = (new BLEvaluacion()).ObtenerPlanesxBilletera(intSistemaSISACT);
            _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][ProcesarDetalleSISACT] objListaPlan.Count.ToString(): ", Funciones.CheckStr(objListaPlan.Count.ToString())), null);
            

            if (objListaMontoFactura == null) objListaMontoFactura = new List<BEPlanBilletera>();
            if (objListaPlanActivos == null) objListaPlanActivos = new List<BEPlanBilletera>();

            foreach (DataRow dr in dtDetalleSISACT.Rows)
            {
                objPlan = new BEPlanBilletera();
                //PROY-26963_F3 - GPRD - PROMFACT
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][ProcesarDetalleSISACT] dtDetalleSISACT.Columns[0].ColumnName: ", Funciones.CheckStr(dtDetalleSISACT.Columns[0].ColumnName)), null);
                if (dtDetalleSISACT.Columns[0].ColumnName == "CUENTA")
                {
                    objPlan.nroSEC = dr["CUENTA"].ToString();
                }

                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][ProcesarDetalleSISACT][INICIO] objPlan.nroSEC: ", Funciones.CheckStr(objPlan.nroSEC)), null);
                //PROY-26963_F3 - GPRD - PROMFACT
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][ProcesarDetalleSISACT][INICIO] dr[PLAN_SISACT].ToString(): ", Funciones.CheckStr(dr["PLAN_SISACT"].ToString())), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][ProcesarDetalleSISACT][INICIO] dr[CARGO_FIJO].ToString(): ", Funciones.CheckStr(dr["CARGO_FIJO"].ToString())), null);
                objPlan.plan = dr["PLAN_SISACT"].ToString();
                objPlan.montoFacturado = 0;
                objPlan.montoNoFacturado = Funciones.CheckDbl(dr["CARGO_FIJO"], 2);

                objPlan.tipoPlan = BEPlanBilletera.TIPO_PLAN.MOVIL;
                objPlan.tipoFacturador = BEPlanBilletera.TIPO_FACTURADOR.BSCS;
                objPlan.nroPlanes = 1;

                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][ProcesarDetalleSISACT][INICIO] objPlan.plan: ", Funciones.CheckStr(objPlan.plan)), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][ProcesarDetalleSISACT][INICIO] objPlan.montoNoFacturado: ", Funciones.CheckStr(objPlan.montoNoFacturado)), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][ProcesarDetalleSISACT][INICIO] objPlan.tipoPlan: ", Funciones.CheckStr(objPlan.tipoPlan)), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][ProcesarDetalleSISACT][INICIO] objPlan.tipoFacturador: ", Funciones.CheckStr(objPlan.tipoFacturador)), null);

                List<BEBilletera> objListaBilletera = null;
                foreach (BEPlanBilletera obj in objListaPlan)
                {
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][ProcesarDetalleSISACT][BEPlanBilletera obj in objListaPlan] obj.plan: ", Funciones.CheckStr(obj.plan)), null);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][ProcesarDetalleSISACT][BEPlanBilletera obj in objListaPlan] objPlan.plan: ", Funciones.CheckStr(objPlan.plan)), null);
                    if (obj.plan == objPlan.plan)
                    {
                        objListaBilletera = obj.oBilletera;
                        break;
                    }
                }

                if (objListaBilletera != null)
                {
                    objPlan.oBilletera = new List<BEBilletera>();
                    foreach (BEBilletera obj in objListaBilletera)
                    {
                        _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][ProcesarDetalleSISACT][BEBilletera obj in objListaBilletera] objPlan.idBilletera: ", Funciones.CheckStr(objPlan.idBilletera)), null);
                        _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][ProcesarDetalleSISACT]BEBilletera obj in objListaBilletera] obj.idBilletera: ", Funciones.CheckStr(obj.idBilletera)), null);
                        objPlan.idBilletera = obj.idBilletera;//INC000003018790
                        _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][ProcesarDetalleSISACT][BEBilletera obj in objListaBilletera]SUMADO objPlan.idBilletera: ", Funciones.CheckStr(objPlan.idBilletera)), null);
                        objPlan.oBilletera.Add(new BEBilletera(obj.idBilletera, 1));

                        if (obj.idBilletera == (int)BEBilletera.TIPO_BILLETERA.BAM)
                        {
                            objPlan.tipoPlan = BEPlanBilletera.TIPO_PLAN.DATOS;
                        }
                    }
                }
                else
                {
                    objPlan.idBilletera = (int)BEBilletera.TIPO_BILLETERA.MOVIL;
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][ProcesarDetalleSISACT][BEPlanBilletera obj in objListaPlan] objPlan.idBilletera: ", Funciones.CheckStr(objPlan.idBilletera)), null);
                    objPlan.oBilletera = new List<BEBilletera>();
                    objPlan.oBilletera.Add(new BEBilletera((int)(BEBilletera.TIPO_BILLETERA.MOVIL), 1));
                }

                objListaPlanActivos.Add(objPlan);
                objListaMontoFactura.Add(objPlan);
            }
        }

        private List<BEPlanBilletera> ProcesarCantPlanBSCS(DataTable dtDetalle)
        {
            BEPlanBilletera objPlan;
            List<BEPlanBilletera> objLista = new List<BEPlanBilletera>();
            foreach (DataRow dr in dtDetalle.Rows)
            {
                objPlan = new BEPlanBilletera();
                objPlan.plan = dr["TMCODE"].ToString();
                objPlan.tipoFacturador =  BEPlanBilletera.TIPO_FACTURADOR.BSCS;
                int nroPlanes = 0;

                objPlan.oBilletera = new List<BEBilletera>();
                if (Funciones.CheckInt(dr["NRO_MOVIL"]) > 0)
                {
                    nroPlanes = Funciones.CheckInt(dr["NRO_MOVIL"]);
                    objPlan.idBilletera += (int)(BEBilletera.TIPO_BILLETERA.MOVIL);
                    objPlan.oBilletera.Add(new BEBilletera((int)(BEBilletera.TIPO_BILLETERA.MOVIL), nroPlanes));
                }
                if (Funciones.CheckInt(dr["NRO_INTERNET_FIJO"]) > 0)
                {
                    nroPlanes = Funciones.CheckInt(dr["NRO_INTERNET_FIJO"]);
                    objPlan.idBilletera += (int)(BEBilletera.TIPO_BILLETERA.INTERNET);
                    objPlan.oBilletera.Add(new BEBilletera((int)(BEBilletera.TIPO_BILLETERA.INTERNET), nroPlanes));
                }
                if (Funciones.CheckInt(dr["NRO_CLARO_TV"]) > 0)
                {
                    nroPlanes = Funciones.CheckInt(dr["NRO_CLARO_TV"]);
                    objPlan.idBilletera += (int)(BEBilletera.TIPO_BILLETERA.CLAROTV);
                    objPlan.oBilletera.Add(new BEBilletera((int)(BEBilletera.TIPO_BILLETERA.CLAROTV), nroPlanes));
                }
                if (Funciones.CheckInt(dr["NRO_TELEF_FIJA"]) > 0)
                {
                    nroPlanes = Funciones.CheckInt(dr["NRO_TELEF_FIJA"]);
                    objPlan.idBilletera += (int)(BEBilletera.TIPO_BILLETERA.TELEFONIA);
                    objPlan.oBilletera.Add(new BEBilletera((int)(BEBilletera.TIPO_BILLETERA.TELEFONIA), nroPlanes));
                }
                if (Funciones.CheckInt(dr["NRO_BAM"]) > 0)
                {
                    nroPlanes = Funciones.CheckInt(dr["NRO_BAM"]);
                    objPlan.idBilletera += (int)(BEBilletera.TIPO_BILLETERA.BAM);
                    objPlan.oBilletera.Add(new BEBilletera((int)(BEBilletera.TIPO_BILLETERA.BAM), nroPlanes));
                }

                objPlan.nroPlanes = nroPlanes;
                objPlan.tipoPlan = BEPlanBilletera.TIPO_PLAN.MOVIL;
                if (Funciones.CheckInt(dr["NRO_BAM"]) > 0) objPlan.tipoPlan = BEPlanBilletera.TIPO_PLAN.DATOS;

                objLista.Add(objPlan);
            }
            return objLista;
        }

        private double ProcesarDetalleFraude(DataTable dtDetalle, DataTable dtDetalleBSCS, WS.WSOAC.DetalleCuentaType[] objCuentaOAC)
        {
            string strBloqueosFraude = ConfigurationManager.AppSettings["ConstCodBloqueoFraude"].ToString();
            string strCuenta = string.Empty;
            List<string> objListaCuenta = new List<string>();
            double dblDeuda = 0.0;

            foreach (DataRow dr in dtDetalle.Rows)
            {
                if (strBloqueosFraude.IndexOf(strCuenta) > -1)
                {
                    foreach (DataRow dr1 in dtDetalleBSCS.Rows)
                    {
                        if (dr["NRO_CUENTA"] == dr1["CUSTOMER_ID"])
                        {
                            strCuenta = dr1["CUENTA"].ToString();
                            if (!objListaCuenta.Contains(strCuenta))
                            {
                                objListaCuenta.Add(strCuenta);
                                break;
                            }
                        }
                    }
                }
            }

            if (objCuentaOAC != null)
            {
                foreach (string cuenta in objListaCuenta)
                {
                    foreach (WS.WSOAC.DetalleCuentaType OAC in objCuentaOAC)
                    {
                        if (OAC.xTipoServicio == "MOVIL" && OAC.xCodCuenta == cuenta)
                        {
                            dblDeuda = dblDeuda + Funciones.CheckDbl(OAC.xDeudaVencida, 2) + Funciones.CheckDbl(OAC.xDeudaCastigada, 2);
                            break;
                        }
                    }
                }
            }
            return dblDeuda;
        }

        private void ObtenerMontoFactxBilletera(List<BEPlanBilletera> objListaMonto, ref BEClienteCuenta objCliente)
        {
            string strMontoFacturado = string.Empty;
            string strMontoNoFacturado = string.Empty;
            BLEvaluacion objEvaluacion = new BLEvaluacion();
            string strNroDocumento = objCliente.nroDoc;

            _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][ObtenerMontoFactxBilletera][INICIO] objCliente.nroDoc: ", Funciones.CheckStr(objCliente.nroDoc)), null);

            int cont = 0;//INC000003018790
            foreach (BEPlanBilletera obj in objListaMonto)
            {
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][ObtenerMontoFactxBilletera] cont: ", Funciones.CheckStr(cont)), null);

                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][ObtenerMontoFactxBilletera] strMontoFacturado: ", strMontoFacturado), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][ObtenerMontoFactxBilletera] strMontoFacturado| obj.idBilletera: ", Funciones.CheckStr(obj.idBilletera)), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][ObtenerMontoFactxBilletera] strMontoFacturado| obj.montoFacturado: ", Funciones.CheckStr(obj.montoFacturado)), null);
                if (obj.montoFacturado > 0)
                {                    
                    strMontoFacturado = String.Format("{0}|{1};{2}", strMontoFacturado, obj.idBilletera, obj.montoFacturado);

                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][ObtenerMontoFactxBilletera] strMontoFacturado: ", strMontoFacturado), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][ObtenerMontoFactxBilletera] strMontoFacturado| obj.idBilletera: ", Funciones.CheckStr(obj.idBilletera)), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][ObtenerMontoFactxBilletera] strMontoFacturado| obj.montoFacturado: ", Funciones.CheckStr(obj.montoFacturado)), null);
                }
                ////INC000002510108 INI
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000002510108][BLDatosCliente][ObtenerMontoFactxBilletera] strMontoNoFacturado: ", strMontoNoFacturado), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000002510108][BLDatosCliente][ObtenerMontoFactxBilletera] strMontoNoFacturado|obj.idBilletera: ", Funciones.CheckStr(obj.idBilletera)), null);
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000002510108][BLDatosCliente][ObtenerMontoFactxBilletera] strMontoNoFacturado|obj.montoNoFacturado: ", Funciones.CheckStr(obj.montoNoFacturado)), null);
                if (obj.montoNoFacturado > 0)
                {
                    strMontoNoFacturado = String.Format("{0}|{1};{2}", strMontoNoFacturado, obj.idBilletera, obj.montoNoFacturado); //INC000003018790

                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000002510108][BLDatosCliente][ObtenerMontoFactxBilletera] strMontoNoFacturado: ", strMontoNoFacturado), null);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000002510108][BLDatosCliente][ObtenerMontoFactxBilletera] strMontoNoFacturado|obj.idBilletera: ", Funciones.CheckStr(obj.idBilletera)), null);
                    _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000002510108][BLDatosCliente][ObtenerMontoFactxBilletera] strMontoNoFacturado|obj.montoNoFacturado: ", Funciones.CheckStr(obj.montoNoFacturado)), null);
                }                           

                ////INC000002510108 FIN
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][ObtenerMontoFactxBilletera] obj.montoFacturado: ", Funciones.CheckStr(obj.montoFacturado)), null);
                objCliente.montoFacturadoTotal += obj.montoFacturado;
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][ObtenerMontoFactxBilletera] objCliente.montoFacturadoTotal: ", Funciones.CheckStr(objCliente.montoFacturadoTotal)), null);

                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][ObtenerMontoFactxBilletera] obj.montoNoFacturado: ", Funciones.CheckStr(obj.montoNoFacturado)), null);
                objCliente.montoNoFacturadoTotal += obj.montoNoFacturado;
                _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][ObtenerMontoFactxBilletera] objCliente.montoNoFacturadoTotal: ", Funciones.CheckStr(objCliente.montoNoFacturadoTotal)), null);

                cont++;//INC000003018790
            }

            _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][ObtenerMontoFactxBilletera] strNroDocumento: ", Funciones.CheckStr(strNroDocumento)), null);
            _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][ObtenerMontoFactxBilletera] strMontoFacturado: ", Funciones.CheckStr(strMontoFacturado)), null);
            objCliente.oMontoFacturadoxBilletera = objEvaluacion.ObtenerMontoFactxBilletera(strNroDocumento, strMontoFacturado);
            _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][ObtenerMontoFactxBilletera] strNroDocumento: ", Funciones.CheckStr(strNroDocumento)), null);
            _objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[INC000003018790][BLDatosCliente][ObtenerMontoFactxBilletera] strMontoNoFacturado: ", Funciones.CheckStr(strMontoNoFacturado)), null);
            objCliente.oMontoNoFacturadoxBilletera = objEvaluacion.ObtenerMontoFactxBilletera(strNroDocumento, strMontoNoFacturado);
            
        }

        private double CalcularCF(string strNroDoc, int intMesesRecurrente, int tipo)
        {
            double dblCF = 0;
            List<BEPlan> objLista = (new BLConsumer()).ListarDetallePlanCF(strNroDoc, intMesesRecurrente, tipo);
            if (objLista != null)
            {
                foreach (BEPlan obj in objLista)
                {
                    dblCF += Funciones.CheckDbl(obj.PLANN_CAR_FIJ * obj.CANTIDAD);
                }
            }
            return dblCF;
        }

        private DataTable ProcesarDetallePrepago(string strTipoDocumento, string strNroDocumento)
        {
            DataTable dtTable = new DataTable();
            dtTable.Columns.Add(new DataColumn("LINEA_PREPAGO", System.Type.GetType("System.String")));
            dtTable.Columns.Add(new DataColumn("PLAN", System.Type.GetType("System.String")));
            dtTable.Columns.Add(new DataColumn("ESTADO", System.Type.GetType("System.String")));
            dtTable.Columns.Add(new DataColumn("TIPO_BLOQUEO", System.Type.GetType("System.String")));

            try
            {
                string validaBloqueo = String.Empty;
                string[] aBloqueo;
                bool bBloqueo = false;
                bool bTipoBloqueo = false;
                string sTipoBloqueo = String.Empty;
                string numero_telefono = String.Empty;
            
                string sListaLineaTipoBloqueo = Funciones.CheckStr(ConfigurationManager.AppSettings["ListaLineaTipoBloqueo"]);
                int aMaxRegistros = Funciones.CheckInt(ConfigurationManager.AppSettings["consMaxListaLineasCliente"]);
                int maskNumeroDigitos = Funciones.CheckInt(ConfigurationManager.AppSettings["consMaskNumeroDigitosListaLinea"]);

                string[] aTipoBloqueos = sListaLineaTipoBloqueo.Split(',');
                string sMaskNumeroTelefono = String.Empty;

                // Consultar Lineas
                List<BELineaAbonado> objLista = (new BLPrepago()).ListarLineasAbonado(strTipoDocumento, strNroDocumento);

                foreach (BELineaAbonado oItem in objLista)
                {
                    if ((oItem.nro_telefono != "") && (dtTable.Rows.Count < aMaxRegistros))
                    {
                        numero_telefono = "";
                        bBloqueo = false;
                        bTipoBloqueo = false;

                        if (oItem.nro_telefono.Length > 2)
                        {
                            numero_telefono = oItem.nro_telefono.Substring(2, (oItem.nro_telefono.Length - 2));
                            validaBloqueo = (new BLPrepago()).ValidarBloqueoLinea(strNroDocumento, numero_telefono);

                            aBloqueo = validaBloqueo.Split('|');

                            if (aBloqueo.Length == 3)
                            {
                                if ((aBloqueo[1] == "BLOQUEADO") || (aBloqueo[1] == "DESBLOQUEADO") || (aBloqueo[1] == ""))
                                {
                                    bBloqueo = true;
                                }
                                if (aBloqueo[1] == "DESBLOQUEADO")
                                {
                                    bTipoBloqueo = true;
                                }
                                else if (Array.IndexOf(aTipoBloqueos, aBloqueo[0]) >= 0)
                                {
                                    bTipoBloqueo = true;
                                }
                            }
                            else if (aBloqueo[0] == "")
                            {
                                bBloqueo = true;
                                bTipoBloqueo = true;
                                sTipoBloqueo = "DESBLOQUEADO";
                            }
                            // AGREGAR
                            if (bBloqueo && bTipoBloqueo)
                            {
                                if (aBloqueo.Length == 3)
                                {
                                    if ((aBloqueo[1] == "DESBLOQUEADO") || (aBloqueo[1] == ""))
                                    {
                                        sTipoBloqueo = "DESBLOQUEADO";
                                    }
                                    else
                                    {
                                        switch (aBloqueo[0])
                                        {
                                            case "BLOQ_ROB":
                                                sTipoBloqueo = "ROBO";
                                                break;
                                            case "BLOQ_PER":
                                                sTipoBloqueo = "PERDIDA";
                                                break;
                                            case "BLOQ_HUR":
                                                sTipoBloqueo = "PERDIDA";
                                                break;
                                            default:
                                                sTipoBloqueo = "BLOQUEADO";
                                                break;
                                        }
                                    }
                                }
                                sMaskNumeroTelefono = (numero_telefono.Substring(0, (numero_telefono.Length - maskNumeroDigitos)) + "".PadRight(maskNumeroDigitos, char.Parse("*")));
                                
                                DataRow dr = dtTable.NewRow();
                                dr["LINEA_PREPAGO"] = sMaskNumeroTelefono;
                                dr["PLAN"] = oItem.segmento;
                                dr["ESTADO"] = oItem.estado;
                                dr["TIPO_BLOQUEO"] = sTipoBloqueo;
                                dtTable.Rows.Add(dr);
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                dtTable = null;
                _objLog = new GeneradorLog(null, strNroDocumento, null, "WEB");
                _objLog.CrearArchivolog("ERROR CONSULTAR DETALLE PREPAGO", null, ex); ;
            }
            return dtTable;
        }

        private string separarApellidos(string apellido)
        {
            int contador = 0;
            string ap_paterno = "";
            string ap_materno = "";
            string tok = "";

            try
            {
                string special_tokens = new BLGeneral().ListaPrefijosApellidoCompuesto().ToUpper();

                string[] tokens = apellido.Split(new string[] { " " }, StringSplitOptions.None);
                string[] apellido_ordenado = new string[tokens.Length];

                for (var i = tokens.Length - 1; i >= 0; i--)
                {
                    tok = tokens[i];
                    if (special_tokens.IndexOf(tok.ToUpper()) >= 0)
                    {
                        if (apellido_ordenado.Length > 0)
                        {
                            apellido_ordenado[contador - 1] = tok + " " + apellido_ordenado[contador - 1];
                        }
                    }
                    else
                    {
                        contador += 1;
                        apellido_ordenado[contador - 1] = tok;
                    }
                }

                if (apellido_ordenado.Length > 0)
                {
                    if (apellido_ordenado.Length == 1)
                    {
                        ap_paterno = apellido_ordenado[0].ToString();
                        ap_materno = "";
                    }
                    else if (apellido_ordenado.Length == 2)
                    {
                        ap_paterno = apellido_ordenado[1].ToString();
                        ap_materno = apellido_ordenado[0].ToString();
                    }
                    else
                    {
                        for (var j = 1; j < apellido_ordenado.Length; j++)
                        {
                            if (j == 1)
                                ap_paterno = apellido_ordenado[j].ToString();
                            else
                                ap_paterno = apellido_ordenado[j] + " " + ap_paterno;

                        }
                        ap_materno = apellido_ordenado[0].ToString();
                    }
                }
            }
            catch { }
            string apellidos = Funciones.CheckStr(ap_paterno) + '|' + Funciones.CheckStr(ap_materno);

            return apellidos;
        }	

        private string obtenerParametro(List<BEItemGenerico> objListaItem, string idParametro)
        {
            string valor = string.Empty;
            if (objListaItem != null)
            {
                foreach (BEItemGenerico objItem in objListaItem)
                {
                    if (objItem.Codigo == idParametro)
                    {
                        valor = Funciones.CheckStr(objItem.Valor);
                        break;
                    }
                }
            }
            return valor;
        }

        #region [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil]
        public static string ObtenerTipoDocumento(string strTipoDoc, string strNroDoc)
        {
            if (strTipoDoc == Funciones.CheckStr(ConfigurationManager.AppSettings["constCodTipoDocumentoDNI"]))
                return Funciones.CheckStr(ReadKeySettings.KEY_CONSTDNIBRMS);// KEY_CONSTDNIBRMS   -- DNI

            if (strTipoDoc == Funciones.CheckStr(ConfigurationManager.AppSettings["constCodTipoDocumentoCEX"]))
                return Funciones.CheckStr(ReadKeySettings.KEY_CONSTCEBRMS);// KEY_CONSTCEBRMS   -- CARNET

            if (strTipoDoc == Funciones.CheckStr(ConfigurationManager.AppSettings["constCodTipoDocumentoRUC"]))
            {
                if (strNroDoc.Substring(0, 1) == "1") return Funciones.CheckStr(ReadKeySettings.KEY_CONSTRUC10BRMS); //KEY_CONSTRUC10BRMS    -- RUC10
                else return Funciones.CheckStr(ReadKeySettings.KEY_CONSTRUC20BRMS);//KEY_CONSTRUC20BRMS    -- RUC20
            }

            if (strTipoDoc == ReadKeySettings.Key_codigoDocPasaporte07)
                return Funciones.CheckStr(ReadKeySettings.KEY_CONSTPASSBRMS);//KEY_CONSTPASSBRMS    -- PASAPORTE
            if (strTipoDoc == ReadKeySettings.Key_codigoDocCIRE)
                return Funciones.CheckStr(ReadKeySettings.KEY_CONSTCIREBRMS); //KEY_CONSTCIREBRMS    -- CIRE
            if (strTipoDoc == ReadKeySettings.Key_codigoDocCIE)
                return Funciones.CheckStr(ReadKeySettings.KEY_CONSTCIEBRMS);//KEY_CONSTCIEBRMS    -- CIE
            if (strTipoDoc == ReadKeySettings.Key_codigoDocCPP)
                return Funciones.CheckStr(ReadKeySettings.KEY_CONSTCPPBRMS);//KEY_CONSTCPPBRMS    -- CPP
            if (strTipoDoc == ReadKeySettings.Key_codigoDocCTM)
                return Funciones.CheckStr(ReadKeySettings.KEY_CONSTCTMBRMS);//KEY_CONSTCTMBRMS    -- CTM

            return Funciones.CheckStr(ReadKeySettings.KEY_CONSTDNIBRMS);// KEY_CONSTDNIBRMS   -- DNI
        }       

        static string CurrentServer
        {
            get
            {
                String nombreHost = System.Net.Dns.GetHostName();
                String nombreServer = System.Net.Dns.GetHostName();
                String ipServer = System.Net.Dns.GetHostAddresses(nombreServer)[0].ToString();
                return ipServer;
            }
        }


        public ValidacionDeudaBRMSrequest.Cliente ObtenerVariablesBRMSVtaCuotas(ValidacionDeudaBRMSrequest.Cliente objRespuesta, string nroDocumento, string TipoDocumento, string currentUser)
        {
            RestVentasCuotas rService = new RestVentasCuotas();
            ObtenerVariablesBRMSResponse objResponse = new ObtenerVariablesBRMSResponse();
            Dictionary<string, string> dcParameters = new Dictionary<string, string>();

            try
            {
                _objLog.CrearArchivologWS("PROY-140743 - IDEA-141192 - Venta Cuotas: ", string.Format("{0}{1}{0}", "**********************************************", " INICIO - ObtenerVariablesBRMSVtaCuotas "), null, null);

                dcParameters.Add("tipoDoc", TipoDocumento);
                dcParameters.Add("numeroDoc", nroDocumento);
                dcParameters.Add("linea", string.Empty);
                dcParameters.Add("rangoHoras", ReadKeySettings.Key_RangoHoras);
                HttpContext.Current.Session["strMensajeError"] = string.Empty;

                #region Datos Auditoria
                BEAuditoriaRequest objBeAuditoriaRequest = new BEAuditoriaRequest();
                objBeAuditoriaRequest.idTransaccion = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                objBeAuditoriaRequest.timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
                objBeAuditoriaRequest.userId = Funciones.CheckStr(currentUser);
                objBeAuditoriaRequest.msgid = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                objBeAuditoriaRequest.dataPower = true;
                objBeAuditoriaRequest.accept = "application/json";
                objBeAuditoriaRequest.urlTimeOut_Rest = Funciones.CheckStr(ConfigurationManager.AppSettings["ConsMejorasPorta_TimeOut"]);
                objBeAuditoriaRequest.wsIp = Funciones.CheckStr(ConfigurationManager.AppSettings["conswsipPedidosTienda"]);
                objBeAuditoriaRequest.ipTransaccion = Funciones.CheckStr(HttpContext.Current.Session["CurrentTerminal"]);
                objBeAuditoriaRequest.usuarioAplicacion = Funciones.CheckStr(currentUser);
                objBeAuditoriaRequest.urlRest = "urlObtVarBRMS";
                objBeAuditoriaRequest.ipApplication = CurrentServer;

                _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0} ===> {1}", " [ObtenerVariablesBRMSVtaCuotas] [idTransaccion] ", objBeAuditoriaRequest.idTransaccion), null, null);
                _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0} ===> {1}", " [ObtenerVariablesBRMSVtaCuotas] [timestamp] ", objBeAuditoriaRequest.timestamp), null, null);
                _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0} ===> {1}", " [ObtenerVariablesBRMSVtaCuotas] [userId] ", objBeAuditoriaRequest.userId), null, null);
                _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0} ===> {1}", " [ObtenerVariablesBRMSVtaCuotas] [msgid] ", objBeAuditoriaRequest.msgid), null, null);
                _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0} ===> {1}", " [ObtenerVariablesBRMSVtaCuotas] [dataPower] ", Funciones.CheckStr(objBeAuditoriaRequest.dataPower)), null, null);
                _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0} ===> {1}", " [ObtenerVariablesBRMSVtaCuotas] [accept] ", objBeAuditoriaRequest.accept), null, null);
                _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0} ===> {1}", " [ObtenerVariablesBRMSVtaCuotas] [urlTimeOut_Rest] ", objBeAuditoriaRequest.urlTimeOut_Rest), null, null);
                _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0} ===> {1}", " [ObtenerVariablesBRMSVtaCuotas] [wsIp] ", objBeAuditoriaRequest.wsIp), null, null);
                _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0} ===> {1}", " [ObtenerVariablesBRMSVtaCuotas] [ipTransaccion] ", objBeAuditoriaRequest.ipTransaccion), null, null);
                _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0} ===> {1}", " [ObtenerVariablesBRMSVtaCuotas] [usuarioAplicacion] ", objBeAuditoriaRequest.usuarioAplicacion), null, null);
                _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0} ===> {1}", " [ObtenerVariablesBRMSVtaCuotas] [urlRest] ", objBeAuditoriaRequest.urlRest), null, null);
                _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0} ===> {1}", " [ObtenerVariablesBRMSVtaCuotas] [ipApplication] ", objBeAuditoriaRequest.ipApplication), null, null);

                #endregion

                #region response servicio
                objResponse = rService.ObtenerVariablesBRMS(dcParameters, objBeAuditoriaRequest);

                string strCodRespuesta = Funciones.CheckStr(objResponse.MessageResponse.Body.variablesBRMSResponse.responseStatus.codigoRespuesta);
                string strMsjRespuesta = Funciones.CheckStr(objResponse.MessageResponse.Body.variablesBRMSResponse.responseStatus.mensajeRespuesta);

                _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0} ===> {1}", " [ObtenerVariablesBRMSVtaCuotas] [codigoRespuesta] ", strCodRespuesta), null, null);
                _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0} ===> {1}", " [ObtenerVariablesBRMSVtaCuotas] [mensajeRespuesta] ", strMsjRespuesta), null, null);

                if (strCodRespuesta.Equals("0"))
                {
                    objRespuesta.montoCuotasPendientesAcc = Funciones.CheckDbl(objResponse.MessageResponse.Body.variablesBRMSResponse.responseData.montoPendCuotas);
                    objRespuesta.cantidadLineaCuotasPendientesAcc = Funciones.CheckInt64(objResponse.MessageResponse.Body.variablesBRMSResponse.responseData.cantPendCuotas);
                    objRespuesta.cantidadMaximaCuotasPendientesAcc = Funciones.CheckInt64(objResponse.MessageResponse.Body.variablesBRMSResponse.responseData.cantMaxPendCuotas);
                    objRespuesta.montoCuotasPendientesAccUltiVenta = Funciones.CheckDbl(objResponse.MessageResponse.Body.variablesBRMSResponse.responseData.montoPendUltVentas);
                    objRespuesta.cantidadLineaCuotasPendientesAccUltiVenta = Funciones.CheckInt64(objResponse.MessageResponse.Body.variablesBRMSResponse.responseData.cantPendUltVentas);
                    objRespuesta.cantidadMaximaCuotasPendientesAccUltiVenta = Funciones.CheckInt64(objResponse.MessageResponse.Body.variablesBRMSResponse.responseData.cantMaxPendUltVentas);
                }
                else
                {
                    HttpContext.Current.Session["strMensajeError"] = Funciones.CheckStr(ReadKeySettings.Key_MsjNoVariablesBRMS);
                }
                #endregion
            }
            catch (Exception ex)
            {
                HttpContext.Current.Session["strMensajeError"] = Funciones.CheckStr(ReadKeySettings.Key_MsjNoVariablesBRMS);
                _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0}{1}{0}", "**********************************************", " ERROR - ObtenerVariablesBRMSVtaCuotas "), null, null);
                _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0} ===> {1}", " [ObtenerVariablesBRMSVtaCuotas] [Message] ", ex.Message), null, null);
                _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0} ===> {1}", " [ObtenerVariablesBRMSVtaCuotas] [StackTrace] ", ex.StackTrace), null, null);
            }

            _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0}{1}{0}", "**********************************************", " FIN - ObtenerVariablesBRMSVtaCuotas "), null, null);

            return objRespuesta;
        }
        #endregion
    }
}
