using System;
using System.Collections.Generic;
using System.Configuration;
using System.Collections;
using System.Data;
using System.Linq;
using System.Text;
using Claro.SISACT.Business;
using Claro.SISACT.Common;
using Claro.SISACT.Entity;
using Claro.SISACT.WS;
//PROY-140230-MAS-INI
using Claro.SISACT.WS.ConsultarPuntosWS;
using System.Web;
//PROY-140230-MAS-FIN
//PROY-32439 INI MAS
using Claro.SISACT.WS.WSValidacionDeudaRules;
using Claro.SISACT.WS.WSConsultaDatosOAC;
using Claro.SISACT.Entity.VentasCuotas.ObtenerVariablesBRMS.Response;
using Claro.SISACT.WS.RestReferences; //INC000004091065
using System.Text.Json;


namespace Claro.SISACT.WS
{
    public class BLReglaCrediticia
    {
        #region [Declaracion de Constantes - Config]

        string consTipoProductoDTH = ConfigurationManager.AppSettings["consTipoProductoDTH"].ToString();
        string consTipoProductoHFC = ConfigurationManager.AppSettings["consTipoProducto3Play"].ToString();
        string consTipoDocRUC = ConfigurationManager.AppSettings["TipoDocumentoRUC"].ToString();
        string consTipoDocDNI = ConfigurationManager.AppSettings["constCodTipoDocumentoDNI"].ToString();
        string consPaginaEvaluacion = ConfigurationManager.AppSettings["constPaginaEvaluacion"].ToString();
        string consFormaPagoContado = ConfigurationManager.AppSettings["constFormaPagoContado"].ToString();
        string consFormaPagoCuota = ConfigurationManager.AppSettings["constFormaPagoCuota"].ToString();
        //INICIATIVA 920 INI
        string consFormaPagoChip = ConfigurationManager.AppSettings["constFormaPagoChip"].ToString();
        string consFormaPagoContratoCode = ConfigurationManager.AppSettings["constFormaPagoContratoCode"].ToString();

        //INICIATIVA 920 FIN
        //PROY-140335 RF1 INI
        string consTipoProductoMovil = ConfigurationManager.AppSettings["constTipoProductoMovil"].ToString();
        string consTipoProductoBAM = ConfigurationManager.AppSettings["constTipoProductoBAM"].ToString();
        //PROY-140335 RF1 INI

        GeneradorLog _objLog = null;

        #endregion [Declaracion de Constantes - Config]

        string tipoDocumento = string.Empty;
        string nroDocumento = string.Empty;
        BEItemMensaje objMensaje;

        //PROY-30166-INI
        double dblCuotaInicialComercial;
        double dblMontoCuotaComercial;
        int intResultado;
        string strMensaje;
        //PROY-30166-FIN

        public void CalcularLCDisponible(BEClienteCuenta objCliente, string strCodRiesgo, string strEsSaludSunat, string strClienteNuevo, double dblLC,
                                         ref List<BEBilletera> objLCxProducto, ref List<BEBilletera> objLCDisponiblexProducto)
        {
            // Calcular LC Buro x Producto
            objLCxProducto = (new BLEvaluacion()).ObtenerLCxBilletera(strCodRiesgo, objCliente.tipoDoc, objCliente.nroDoc, strEsSaludSunat, strClienteNuevo, dblLC);
            // Calcular Monto Facturado x Producto
            List<BEBilletera> oMontoFacturadoxProducto = objCliente.oMontoFacturadoxBilletera;
            // Calcular Monto NO Facturado x Producto
            List<BEBilletera> oMontoNoFacturadoxProducto = objCliente.oMontoNoFacturadoxBilletera;
            // Calcular LC Disponible x Producto
            objLCDisponiblexProducto = new List<BEBilletera>();

            double dblLCxBilletera;
            foreach (BEBilletera objBilletera in objLCxProducto)
            {
                dblLCxBilletera = 0.0;
                if (oMontoFacturadoxProducto != null)
                {
                    foreach (BEBilletera objMonto in oMontoFacturadoxProducto)
                    {
                        if (objBilletera.idBilletera == objMonto.idBilletera)
                        {
                            dblLCxBilletera = objMonto.monto;
                            break;
                        }
                    }
                }
                if (oMontoNoFacturadoxProducto != null)
                {
                    foreach (BEBilletera objMonto in oMontoNoFacturadoxProducto)
                    {
                        if (objBilletera.idBilletera == objMonto.idBilletera)
                        {
                            dblLCxBilletera += objMonto.monto;
                            break;
                        }
                    }
                }
                if (objBilletera.monto >= dblLCxBilletera)
                    objBilletera.monto = objBilletera.monto - dblLCxBilletera;
                else
                    objBilletera.monto = 0.0;

                objBilletera.monto = Funciones.CheckDbl(objBilletera.monto, 2);
                objLCDisponiblexProducto.Add(objBilletera);
            }
        }

        public BEVistaEvaluacion Evaluar(BEClienteCuenta objCliente, List<BEDireccionCliente> objDireccion, string strDatosGeneral, string strPlanesDetalle, string strServiciosDetalle, string strEquiposDetalle, string strTieneProteccionMovil,BECuota objCuotaOAC,BECuota objCuotaPVU,string prodFacturar,  ref WS.WSReglasCrediticia.ClaroEvalClientesReglasRequest oRequestReglasCrediticia) //PROY-24724-IDEA-28174 , PROY-30748//PROY-31948 //PROY-140743
        {
            _objLog = new GeneradorLog(null, Funciones.CheckStr(objCliente.nroDoc), null, "WEB");
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0}:{1}", "[BLReglaCrediticia][Evaluar]", Funciones.CheckStr("Entro")), null);

            BEVistaEvaluacion objVistaEvaluacion = null;

            
                


            _objLog.CrearArchivolog(string.Format("[{0}]", "====> INICIO Parametro Evaluar <===="), null, null);
            //INC000004091065 - Ini objCliente

            string logDesc = "[BLReglaCrediticia.cs][Evaluar()]";

            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.idCliente", Funciones.CheckStr(objCliente.idCliente)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.nroDoc", Funciones.CheckStr(objCliente.nroDoc)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.tipoDoc", Funciones.CheckStr(objCliente.tipoDoc)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.tipoDocDes", Funciones.CheckStr(objCliente.tipoDocDes)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.nroDocAsociado", Funciones.CheckStr(objCliente.nroDocAsociado)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.nombres", Funciones.CheckStr(objCliente.nombres)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.apellidos", Funciones.CheckStr(objCliente.apellidos)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.apellidoPaterno", Funciones.CheckStr(objCliente.apellidoPaterno)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.apellidoMaterno", Funciones.CheckStr(objCliente.apellidoMaterno)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.razonSocial", Funciones.CheckStr(objCliente.razonSocial)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.nacionalidad", Funciones.CheckStr(objCliente.nacionalidad)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.CF_Total", Funciones.CheckStr(objCliente.CF_Total)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.CF_Menor", Funciones.CheckStr(objCliente.CF_Menor)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.CF_Mayor", Funciones.CheckStr(objCliente.CF_Mayor)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.deudaVencida", Funciones.CheckStr(objCliente.deudaVencida)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.deudaCastigada", Funciones.CheckStr(objCliente.deudaCastigada)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.deudaFraude", Funciones.CheckStr(objCliente.deudaFraude)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.deudaTotal", Funciones.CheckStr(objCliente.deudaTotal)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.nroDiasDeuda", Funciones.CheckStr(objCliente.nroDiasDeuda)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.nroBloqueo", Funciones.CheckStr(objCliente.nroBloqueo)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.nroSuspension", Funciones.CheckStr(objCliente.nroSuspension)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.soloEvaluarFijo", Funciones.CheckStr(objCliente.soloEvaluarFijo)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.esClienteClaro", objCliente.esClienteClaro), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.isBlackList", objCliente.isBlackList), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.isWhiteList", objCliente.isWhiteList), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.isTop", objCliente.isTop), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.soloBloqueoRoboPerdida", objCliente.soloBloqueoRoboPerdida), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.mensajeDeudaBloqueo", Funciones.CheckStr(objCliente.mensajeDeudaBloqueo)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.lineaSGA", Funciones.CheckStr(JsonSerializer.Serialize(Funciones.ConvertirDataTableAListaDictionary(objCliente.lineaSGA)))), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.lineaBSCS", Funciones.CheckStr(JsonSerializer.Serialize(Funciones.ConvertirDataTableAListaDictionary(objCliente.lineaBSCS)))), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.lineaSISACT", Funciones.CheckStr(JsonSerializer.Serialize(Funciones.ConvertirDataTableAListaDictionary(objCliente.lineaSISACT)))), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.lineaPrepago", Funciones.CheckStr(JsonSerializer.Serialize(Funciones.ConvertirDataTableAListaDictionary(objCliente.lineaPrepago)))), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.lineaOAC", Funciones.CheckStr(JsonSerializer.Serialize(objCliente.lineaOAC))), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.nroRangoDiasBSCS", Funciones.CheckStr(objCliente.nroRangoDiasBSCS)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.nroPlanesActivos", Funciones.CheckStr(objCliente.nroPlanesActivos)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.nroLineasBSCS", Funciones.CheckStr(objCliente.nroLineasBSCS)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.nroLineasSGA", Funciones.CheckStr(objCliente.nroLineasSGA)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.nroLineasSISACT", Funciones.CheckStr(objCliente.nroLineasSISACT)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.nroLineaMenor7Dia", Funciones.CheckStr(objCliente.nroLineaMenor7Dia)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.nroLineaMenor30Dia", Funciones.CheckStr(objCliente.nroLineaMenor30Dia)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.nroLineaMenor90Dia", Funciones.CheckStr(objCliente.nroLineaMenor90Dia)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.nroLineaMenor180Dia", Funciones.CheckStr(objCliente.nroLineaMenor180Dia)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.nroLineaMayor90Dia", Funciones.CheckStr(objCliente.nroLineaMayor90Dia)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.nroLineaMayor180Dia", Funciones.CheckStr(objCliente.nroLineaMayor180Dia)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.tipoCliente", Funciones.CheckStr(objCliente.tipoCliente)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.tiempoPermanencia", Funciones.CheckStr(objCliente.tiempoPermanencia)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.comportamientoPago", Funciones.CheckStr(objCliente.comportamientoPago)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.montoFacturadoTotal", Funciones.CheckStr(objCliente.montoFacturadoTotal)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.montoNoFacturadoTotal", Funciones.CheckStr(objCliente.montoNoFacturadoTotal)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.oficina", Funciones.CheckStr(objCliente.oficina)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.nroOperacionBuro", Funciones.CheckStr(objCliente.nroOperacionBuro)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.oPlanesActivosxBilletera", Funciones.CheckStr(JsonSerializer.Serialize(objCliente.oPlanesActivosxBilletera))), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.oPlanesActivosCorporativo", Funciones.CheckStr(JsonSerializer.Serialize(objCliente.oPlanesActivosCorporativo))), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.oLCBuroxBilletera", Funciones.CheckStr(JsonSerializer.Serialize(objCliente.oLCBuroxBilletera))), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.oMontoFacturadoxBilletera", Funciones.CheckStr(JsonSerializer.Serialize(objCliente.oMontoFacturadoxBilletera))), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.oMontoNoFacturadoxBilletera", Funciones.CheckStr(JsonSerializer.Serialize(objCliente.oMontoNoFacturadoxBilletera))), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.oLCDisponiblexBilletera", Funciones.CheckStr(JsonSerializer.Serialize(objCliente.oLCDisponiblexBilletera))), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.oGarantiaxProducto", Funciones.CheckStr(JsonSerializer.Serialize(objCliente.oGarantiaxProducto))), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.oOAC", Funciones.CheckStr(JsonSerializer.Serialize(objCliente.oOAC))), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.oVistaEvaluacion", Funciones.CheckStr(JsonSerializer.Serialize(objCliente.oVistaEvaluacion))), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.buroConsultado", Funciones.CheckStr(objCliente.buroConsultado)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.deudaCliente", Funciones.CheckStr(objCliente.deudaCliente)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.cumpleReglaA", Funciones.CheckStr(objCliente.cumpleReglaA)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.totalplanes", Funciones.CheckStr(objCliente.totalplanes)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.Deuda", Funciones.CheckStr(objCliente.Deuda)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.ListaMontoFactura", Funciones.CheckStr(JsonSerializer.Serialize(objCliente.ListaMontoFactura))), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.ListaPlanesActivos", Funciones.CheckStr(JsonSerializer.Serialize(objCliente.ListaPlanesActivos))), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.errorBrms", Funciones.CheckStr(objCliente.errorBrms)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.oRepresentanteLegal", Funciones.CheckStr(JsonSerializer.Serialize(objCliente.oRepresentanteLegal))), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.TipContribuyente", Funciones.CheckStr(objCliente.TipContribuyente)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.NomComercial", Funciones.CheckStr(objCliente.NomComercial)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.FecIniActividades", Funciones.CheckStr(objCliente.FecIniActividades)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.EstContribuyente", Funciones.CheckStr(objCliente.EstContribuyente)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.CondContribuyente", Funciones.CheckStr(objCliente.CondContribuyente)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.CiiuContribuyente", Funciones.CheckStr(objCliente.CiiuContribuyente)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.CantTrabajadores", Funciones.CheckStr(objCliente.CantTrabajadores)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.EmisionComp", Funciones.CheckStr(objCliente.EmisionComp)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.SistEmielectronica", Funciones.CheckStr(objCliente.SistEmielectronica)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.CantMesIniActividades", Funciones.CheckStr(objCliente.CantMesIniActividades)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCliente.clienteCBIO", objCliente.clienteCBIO), null);
                
            //INC000004091065 - Fin objCliente
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objDireccion", Funciones.CheckStr(JsonSerializer.Serialize(objDireccion))), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "strDatosGeneral", Funciones.CheckStr(strDatosGeneral)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "strPlanesDetalle", Funciones.CheckStr(strPlanesDetalle)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "strServiciosDetalle", Funciones.CheckStr(strServiciosDetalle)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "strEquiposDetalle", Funciones.CheckStr(strEquiposDetalle)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "strTieneProteccionMovil", Funciones.CheckStr(strTieneProteccionMovil)), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCuotaOAC", Funciones.CheckStr(JsonSerializer.Serialize(objCuotaOAC))), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "objCuotaPVU", Funciones.CheckStr(JsonSerializer.Serialize(objCuotaPVU))), null);
            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0} {1}:{2}", logDesc, "oRequestReglasCrediticia", Funciones.CheckStr(JsonSerializer.Serialize(oRequestReglasCrediticia))), null);
            _objLog.CrearArchivolog(string.Format("[{0}]", "====> FIN Parametro Evaluar <===="), null, null);

             

                
            string strTipoDoc = objCliente.tipoDoc;
            string nroDocumento = objCliente.nroDoc;

            BLEvaluacion objEvaluacion = new BLEvaluacion();
            List<BEBilletera> objListaBilleteraEvaluado = new List<BEBilletera>();
            List<BEOfrecimiento> objListaOfrecimiento = new List<BEOfrecimiento>();

                
            // Consultar Reglas Creditos
            ConsultarReglasCreditos(objCliente, objDireccion, strDatosGeneral, strPlanesDetalle, strServiciosDetalle, strEquiposDetalle, "N", ref objListaOfrecimiento, ref objListaBilleteraEvaluado, strTieneProteccionMovil, objCuotaOAC, objCuotaPVU,Funciones.CheckStr(prodFacturar), ref oRequestReglasCrediticia); //PROY-24724-IDEA-28174 //PROY-30748//PROY-31948//PROY-140743

                
                _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0}:{1}", "[Out BLReglaCrediticia.ConsultarReglasCreditos() objListaOfrecimiento]", Funciones.CheckStr(JsonSerializer.Serialize(objListaOfrecimiento))), null);
                _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0}:{1}", "[Out BLReglaCrediticia.ConsultarReglasCreditos() objListaBilleteraEvaluado]", Funciones.CheckStr(JsonSerializer.Serialize(objListaBilleteraEvaluado))), null);
                _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0}:{1}", "[Out BLReglaCrediticia.ConsultarReglasCreditos() oRequestReglasCrediticia]", Funciones.CheckStr(JsonSerializer.Serialize(oRequestReglasCrediticia))), null);

            objVistaEvaluacion = ResultadoOfrecimiento(objListaOfrecimiento);

            // Calcular LC Disponible Evaluado
            if (strTipoDoc != consTipoDocRUC) strTipoDoc = consTipoDocDNI;

            double dblLCEvaluado = CalcularMontoxProducto(objCliente.oLCDisponiblexBilletera, objListaBilleteraEvaluado);
            objVistaEvaluacion.LCDisponible = dblLCEvaluado;
            objVistaEvaluacion.rangoLCDisponible = objEvaluacion.ConsultarTextoRangoLC(strTipoDoc, nroDocumento, dblLCEvaluado);

            return objVistaEvaluacion;
        }

        //PROY-29123 Venta en Cuotas INICIO
        public BEOfrecimiento EvaluarCliente(BEClienteCuenta objCliente, List<BEDireccionCliente> objDireccion, string strDatos, BECuota objCuotaOAC, BECuota objCuotaPVU, string prodFacturar)//PROY-31948//PROY-140743
        {
            BEOfrecimiento objOfrecimiento = null;

            List<BEOfrecimiento> objListaOfrecimiento = new List<BEOfrecimiento>();

            // Consultar Reglas Creditos
            ConsultarReglasCliente(objCliente, objDireccion, "S", strDatos,objCuotaOAC, objCuotaPVU, Funciones.CheckStr(prodFacturar), ref objListaOfrecimiento);//PROY-31948//PROY-140743

            if (objListaOfrecimiento.Count > 0)
                objOfrecimiento = objListaOfrecimiento.FirstOrDefault();

            return objOfrecimiento;
        }
        //PROY-29123 Venta en Cuotas FIN

        public List<BECuota> EvaluarCuota(BEClienteCuenta objCliente, List<BEDireccionCliente> objDireccion, string strDatosGeneral, string strPlanesDetalle, string strEquiposDetalle, string strTieneProteccionMovil,BECuota objCuotaOAC, BECuota objCuotaPVU, string prodFacturar, ref List<BEOfrecimiento> objListOfrecimiento) //PROY-24724-IDEA-28174 //PROY-29123-IDEA-36703//PROY-31948//PROY-140743
        {
            List<BECuota> objListaCuota = new List<BECuota>();
            List<BEBilletera> objListaBilleteraEvaluado = new List<BEBilletera>();
            List<BEOfrecimiento> objListaOfrecimiento = new List<BEOfrecimiento>();

            WS.WSReglasCrediticia.ClaroEvalClientesReglasRequest oRequestReglasCreditcia = new WS.WSReglasCrediticia.ClaroEvalClientesReglasRequest(); //PROY-30748
            // Consultar Reglas Creditos
            ConsultarReglasCreditos(objCliente, objDireccion, strDatosGeneral, strPlanesDetalle, string.Empty, strEquiposDetalle, "S", ref objListaOfrecimiento, ref objListaBilleteraEvaluado, strTieneProteccionMovil, objCuotaOAC, objCuotaPVU, Funciones.CheckStr(prodFacturar), ref oRequestReglasCreditcia); //PROY-24724-IDEA-28174 //PROY-30748//PROY-31948//PROY-140743

            List<BECuota> objLista = new BLGeneral().ListarTipoCuota();
            foreach (BEOfrecimiento obj in objListaOfrecimiento)
            {
                if (obj.ListaCuotas != null && obj.ListaCuotas.Count > 0)
                {
                    foreach (BECuota objCuota in obj.ListaCuotas)
                    {
                        foreach (BECuota objItem in objLista)
                        {
                            if (objCuota.nroCuota == objItem.nroCuota)
                            {
                                objCuota.idFila = obj.IdFila.ToString();
                                objCuota.idCuota = objItem.idCuota;
                                objCuota.cuota = objItem.cuota;

                                objListaCuota.Add(objCuota);
                            }
                        }
                    }
                }
                else if (obj.MostrarMensaje != "" && obj.MostrarMensaje != null){
                        objListOfrecimiento = objListaOfrecimiento;
                }

                
            }

            return objListaCuota;
        }

        public void ConsultarReglasCreditos(BEClienteCuenta objCliente, List<BEDireccionCliente> objDireccion, string strDatosGeneral, string strPlanesDetalle, string strServiciosDetalle,
                                            string strEquiposDetalle, string strFlgCuota, ref List<BEOfrecimiento> objListaOfrecimiento, ref List<BEBilletera> objListaBilleteraEvaluado, string strTieneProteccionMovil
           , BECuota objCuotaOAC, BECuota objCuotaPVU,string prodFacturar, ref WS.WSReglasCrediticia.ClaroEvalClientesReglasRequest oEvaluacionCliente) //PROY-24724-IDEA-28174 //PROY-30748 //PROY-31948//PROY-140743
        {
            try
            {
                string strTipoDoc = objCliente.tipoDoc;
                string nroDocumento = objCliente.nroDoc;
                this.tipoDocumento = strTipoDoc;
                this.nroDocumento = nroDocumento;

                string[] arrDatosGeneral = strDatosGeneral.Split('|');
                string strTipoOperacion = arrDatosGeneral[0];
                string strOferta = arrDatosGeneral[1];
                string strCasoEspecial = arrDatosGeneral[2];
                string strTipoModalidad = arrDatosGeneral[3].ToUpper();
                string strOperadorCedente = arrDatosGeneral[4].ToUpper();
                string strModalidadVenta = arrDatosGeneral[5].ToUpper();
                string strCombo = arrDatosGeneral[6];
                string strComboTexto = arrDatosGeneral[7].ToUpper();

                // Log
                _objLog = new GeneradorLog(null, nroDocumento, null, consPaginaEvaluacion);

                //PROY-30166-INI
                if (strModalidadVenta.Trim() == "CUOTAS")
                {
                _objLog.CrearArchivolog("OBTENER PARAMETROS CUOTA COMERCIAL - INICIO ", null, null);
                string[] arrEquipoDetalle = strEquiposDetalle.Split(';');
                string strCodigoMaterial = arrEquipoDetalle[2];
                string[] arrListaPrecio = strEquiposDetalle.Split('_');
                string strCodListaPrecio = arrListaPrecio[1];
                string[] arrPlazoDetalle = strPlanesDetalle.Split(';');
                string strCodPlazo = arrPlazoDetalle[2];
                _objLog.CrearArchivolog("PARAMETROS : CodMaterial = " + strCodigoMaterial,null, null);
                _objLog.CrearArchivolog("PARAMETROS : CodListaPrecio = " + strCodListaPrecio,null, null);
                _objLog.CrearArchivolog("PARAMETROS : CodPlazo = " + strCodPlazo,null, null);
                _objLog.CrearArchivolog("OBTENER PARAMETROS CUOTA COMERCIAL - FIN ",null, null);

               
                _objLog.CrearArchivolog(" OBTENER CUOTA COMERCIAL- CLARO UP - INICIO ", null, null);
                BLEvaluacion objEval = new BLEvaluacion();
                string strCuotaIniComer = objEval.ObtenerCuotaIniCom(strCodListaPrecio, strCodigoMaterial, strCodPlazo);

                if (strCuotaIniComer != "")
                {

                    string[] arrCuotaIniComer = strCuotaIniComer.Split(';');
                    dblCuotaInicialComercial = Funciones.CheckDbl(arrCuotaIniComer[0]);
                    HttpContext.Current.Session["dblCuotaInicialComercial"] = dblCuotaInicialComercial; //INICIATIVA- 803
                    dblMontoCuotaComercial = Funciones.CheckDbl(arrCuotaIniComer[1]);
                    intResultado = Funciones.CheckInt(arrCuotaIniComer[2]);
                    strMensaje = arrCuotaIniComer[3];
                    _objLog.CrearArchivolog("RESULTADO CUOTA INICIAL COMERCIAL : " + dblCuotaInicialComercial,null, null);
                    _objLog.CrearArchivolog("RESULTADO MONTO CUOTA COMERCIAL :  " + dblMontoCuotaComercial,null, null);
                }

                _objLog.CrearArchivolog(" OBTENER CUOTA COMERCIAL - FIN ", null, null);
                }

                //PROY-30166-FIN

                //INICIATIVA 920
                if (strModalidadVenta.Trim() == "CUOTAS / SIN CODE")// INICIATIVA 920
                {
                    _objLog.CrearArchivolog("OBTENER PARAMETROS CUOTA COMERCIAL - INICIO ", null, null);
                    string[] arrEquipoDetalle = strEquiposDetalle.Split(';');
                    string strCodigoMaterial = arrEquipoDetalle[2];
                    string[] arrListaPrecio = strEquiposDetalle.Split('_');
                    string strCodListaPrecio = arrListaPrecio[1];
                    //el numero de cuotas reemplaza al plazo
                    string strCodPlazoVigencia = arrEquipoDetalle[6];//porque el plazo siempre es 0

                    string strCodPlazo = "0";

                    string[] arrCodigosPlazo = ConfigurationManager.AppSettings["Key_codigoPlazos"].ToString().Split('|');
                    _objLog.CrearArchivolog("PARAMETROS : CodMaterial = " + strCodigoMaterial, null, null);
                    _objLog.CrearArchivolog("PARAMETROS : CodListaPrecio = " + strCodListaPrecio, null, null);
                    _objLog.CrearArchivolog("strCodPlazoVigencia = " + strCodPlazoVigencia, null, null);
                    _objLog.CrearArchivolog("OBTENER PARAMETROS CUOTA COMERCIAL - FIN ", null, null);


                    _objLog.CrearArchivolog(" OBTENER CUOTA COMERCIAL- CLARO UP - INICIO ", null, null);

                    foreach (string codigoPlazo in arrCodigosPlazo) { 
                        string[] cod = codigoPlazo.Split(';');
                        if (strCodPlazoVigencia == cod[0])
                        {
                            strCodPlazo = cod[1]; break;
                        }
                    }

                    _objLog.CrearArchivolog("PARAMETROS : strCodPlazo = " + strCodPlazo, null, null);
                    BLEvaluacion objEval = new BLEvaluacion();
                    string strCuotaIniComer = objEval.ObtenerCuotaIniCom(strCodListaPrecio, strCodigoMaterial, strCodPlazo);

                    if (strCuotaIniComer != "")
                    {

                        string[] arrCuotaIniComer = strCuotaIniComer.Split(';');
                        dblCuotaInicialComercial = Funciones.CheckDbl(arrCuotaIniComer[0]);
                        HttpContext.Current.Session["dblCuotaInicialComercial"] = dblCuotaInicialComercial; //INICIATIVA- 803
                        dblMontoCuotaComercial = Funciones.CheckDbl(arrCuotaIniComer[1]);
                        intResultado = Funciones.CheckInt(arrCuotaIniComer[2]);
                        strMensaje = arrCuotaIniComer[3];
                        _objLog.CrearArchivolog("RESULTADO CUOTA INICIAL COMERCIAL : " + dblCuotaInicialComercial, null, null);
                        _objLog.CrearArchivolog("RESULTADO MONTO CUOTA COMERCIAL :  " + dblMontoCuotaComercial, null, null);
                    }

                    _objLog.CrearArchivolog(" OBTENER CUOTA COMERCIAL - FIN ", null, null);
                }

                //INICIATIVA 920

                // Detalle Planes Evaluados
                List<BEOfertaBRMS> objListaOferta = ObtenerDetalleEvaluacion(strPlanesDetalle, strServiciosDetalle, strEquiposDetalle);

                // Productos de Planes Evaluados
                List<BEPlanBilletera> objProductoPlanesEval = ObtenerProductosPlanesEval(objListaOferta);

                _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0}:{1}", "[BLReglaCrediticia][ConsultarReglasCreditos] objProductoPlanesEval", Funciones.CheckStr(JsonSerializer.Serialize(objProductoPlanesEval))), null);

                List<BEPlanBilletera> objBilleteraPlanesActivo = null;
                BLEvaluacion objEvaluacion = new BLEvaluacion();
                List<BEOfertaBRMS> objListaOfertaEvaluado = new List<BEOfertaBRMS>();
                BEOfrecimiento objOfrecimiento = null;
                objListaOfrecimiento = new List<BEOfrecimiento>();
                objListaBilleteraEvaluado = new List<BEBilletera>();

                // Nro Planes Activos
                if (objCliente.oPlanesActivosxBilletera != null)
                {
                    objBilleteraPlanesActivo = new List<BEPlanBilletera>(objCliente.oPlanesActivosxBilletera);
                    if (objBilleteraPlanesActivo != null && !string.IsNullOrEmpty(strCasoEspecial))
                    {
                        List<BEItemGenerico> objListaPlanes = objEvaluacion.ObtenerPlanesBSCSxCE(strCasoEspecial);
                        if (objListaPlanes != null)
                        {
                            objBilleteraPlanesActivo.Clear();
                            foreach (BEItemGenerico obj in objListaPlanes)
                            {
                                foreach (BEPlanBilletera objPlan in objCliente.oPlanesActivosxBilletera)
                                {
                                    if (objPlan.tipoFacturador == BEPlanBilletera.TIPO_FACTURADOR.BSCS && objPlan.plan == obj.Codigo)
                                        objBilleteraPlanesActivo.Add(objPlan);
                                }
                            }
                        }
                    }
                }

                // Descuento x Producto
                double dblDescuentoCF = 0.0;
                List<BEDescuento> lstDescuentoProducto = null;
                if (!string.IsNullOrEmpty(strCombo))
                    lstDescuentoProducto = new BLGeneral_II().ListarComboDescuento(strCombo);

                // Consulta Datos Evaluaci�n BRMS
                //WS.WSReglasCrediticia.ClaroEvalClientesReglasRequest oEvaluacionCliente = datosGeneralEvaluacion(objCliente);
                oEvaluacionCliente = datosGeneralEvaluacion(objCliente); //PROY-30748

                _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0}:{1}", "[BLReglaCrediticia][ConsultarReglasCreditos] oEvaluacionCliente", Funciones.CheckStr(JsonSerializer.Serialize(oEvaluacionCliente))), null);

                WS.WSReglasCrediticia.oferta oOferta;
                WS.WSReglasCrediticia.solicitud1 oSolicitud1;

                oSolicitud1 = oEvaluacionCliente.solicitud.solicitud1;
                oSolicitud1.tipoDeOperacion = strTipoOperacion;

                //PROY-140335 - INICIO EJRC
                if (strTipoOperacion == ConfigurationManager.AppSettings["consDescPortabilidad"].ToString() && ReadKeySettings.Key_ConsCPPuntoVenta.IndexOf(oSolicitud1.puntodeVenta.codigo) > -1)
                {
                    oSolicitud1.puntodeVenta.calidadDeVendedor = ConfigurationManager.AppSettings["consCalidadDeVendedor"].ToString(); 
                }
                //PROY-140335 - FIN EJRC

                oSolicitud1.totalPlanes = objCliente.totalplanes; //PROY-30748
                // Flujo Reglas Evaluaci�n / Reglas Cuotas
                oSolicitud1.transaccion = ConfigurationManager.AppSettings["constTrxEvaluacion"].ToString();
                if (strFlgCuota == "S")
                    oSolicitud1.transaccion = ConfigurationManager.AppSettings["constTrxVentaCuotas"].ToString();

                // Modalidad de Venta
                //INCIATIVA 920
               if (strModalidadVenta == consFormaPagoChip || strModalidadVenta == consFormaPagoContratoCode)
                    strModalidadVenta = consFormaPagoContado;


                foreach (BEOfertaBRMS objOferta in objListaOferta)
                {
                    // Plan
                    string idPlan = objOferta.idPlan;

                    oEvaluacionCliente.DecisionID = objOferta.idFila.ToString();
                    oOferta = new WS.WSReglasCrediticia.oferta();
                    oOferta.campana = new WS.WSReglasCrediticia.campana();

                    //[INC000002442213]INC FALLA CARGO FIJO INI
                    _objLog.CrearArchivolog("[INC000002442213][=================INC FALLA CARGO FIJO INI =============]", null, null);
                    _objLog.CrearArchivolog("[INC000002442213][VALIDACION FRAUDE][strModalidadVenta]" + strModalidadVenta, null, null);

                        _objLog.CrearArchivolog("[INC000002442213][VALIDACION FRAUDE][Funciones.CheckStr(objOferta.idFila)]" + Funciones.CheckStr(objOferta.idFila), null, null);
                        List<BEPlan> objPLan = null;

                        if (HttpContext.Current.Session["objplan" + Funciones.CheckStr(objOferta.idFila)] == null)
                        {
                            objPLan = null;
                            _objLog.CrearArchivolog("[INC000002442213][=================INC FALLA CARGO FIJO INI null]", null, null);
                        }
                        else
                        {
                            objPLan = (List<BEPlan>)HttpContext.Current.Session["objplan" + Funciones.CheckStr(objOferta.idFila)];
                        }

                        
                        if (objPLan != null)
                        {


                            foreach (BEPlan item in objPLan)
                            {
                                _objLog.CrearArchivolog("[INC000002442213][VALIDACION FRAUDE][session item.PLANC_CODIGO]" + item.PLANC_CODIGO, null, null);
                                _objLog.CrearArchivolog("[INC000002442213][VALIDACION FRAUDE][idPlan]" + idPlan, null, null);
                                _objLog.CrearArchivolog("[INC000002442213][VALIDACION FRAUDE][session item.PLANN_CAR_FIJ]" + Funciones.CheckDbl(item.PLANN_CAR_FIJ), null, null); //INC000002464679
                                _objLog.CrearArchivolog("[INC000002442213][VALIDACION FRAUDE][objOferta.cargoFijo]" + Funciones.CheckDbl(objOferta.cargoFijo), null, null); //INC000002464679
                                _objLog.CrearArchivolog("[INC000002442213][VALIDACION FRAUDE][item.PRDC_CODIGO]" + Funciones.CheckStr(item.PRDC_CODIGO), null, null);

                            Double dblMontoEquipoCuota = 0;
                            if (objOferta.oEquipo != null)
                            {
                                if (objOferta.oEquipo.Count > 0) {

                                    dblMontoEquipoCuota = objOferta.oEquipo[0].montoDeCuota;
                                }

                            }
                            _objLog.CrearArchivolog("[INC000002442213][VALIDACION FRAUDE][objOferta.oEquipo[0].montoDeCuota]" + Funciones.CheckStr(dblMontoEquipoCuota), null, null);


                                if (item.PRDC_CODIGO.Trim().Length > 0)
                                {
//INC000002464679 inicio
                                    Double dblCFSession = Math.Round(Funciones.CheckDbl(item.PLANN_CAR_FIJ), 2);
                                    Double dblCFHidden = Math.Round(Funciones.CheckDbl(objOferta.cargoFijo), 2);

                                    if (item.PLANC_CODIGO == idPlan && "09|08|05|03|06".IndexOf(Funciones.CheckStr(item.PRDC_CODIGO)) <= -1 && dblCFSession != dblCFHidden) //INC000002464679
                                    {

                                        _objLog.CrearArchivolog("[INC000002442213][ENTRO][VALIDACION FRAUDE][PASO 3]" + idPlan, null, null);
                                        _objLog.CrearArchivolog("[INC000002442213][ENTRO][VALIDACION FRAUDE][PASO 3]" + "09|08|05|03|06".IndexOf(Funciones.CheckStr(item.PRDC_CODIGO)), null, null);
                                        _objLog.CrearArchivolog("[INC000002442213][ENTRO][VALIDACION FRAUDE][PASO 3]" + dblCFSession, null, null);
                                        _objLog.CrearArchivolog("[INC000002442213][ENTRO][VALIDACION FRAUDE][PASO 3]" + dblCFHidden, null, null);

                                        double resta = dblCFSession - dblCFHidden;

                                        _objLog.CrearArchivolog("[INC000002442213][ENTRO][VALIDACION FRAUDE][PASO 3]" + resta, null, null);
//INC000002464679 fin
                                    if (!strModalidadVenta.Trim().Contains("CUOTAS")) // INICIATIVA 920
                                    {
                                        _objLog.CrearArchivolog("[INC000002442213][VALIDACION][Error, cargo fijo no coincide con la carga inicial 3.1]" + resta, null, null);
                                        throw new Exception("Error, cargo fijo no coincide con la carga inicial 3.1");
                                    }
                                    else
                                    {

                                        Double dblCFHiddenCouota = Math.Round(Funciones.CheckDbl(objOferta.cargoFijo) - dblMontoEquipoCuota, 2);
                                        _objLog.CrearArchivolog("[INC000002442213][VALIDACION FRAUDE][dblCFHiddenCouota]" + Funciones.CheckStr(dblCFHiddenCouota), null, null);



                                        if (item.PLANC_CODIGO == idPlan && "09|08|05|03|06".IndexOf(Funciones.CheckStr(item.PRDC_CODIGO)) <= -1 && dblCFSession != dblCFHiddenCouota) //INC000002464679
                                        {
                                            _objLog.CrearArchivolog("[INC000002442213][VALIDACION][Error, cargo fijo no coincide con la carga inicial 3.2]" + resta, null, null);
                                            throw new Exception("Error, cargo fijo no coincide con la carga inicial 3.2");
                            }

                                        //_objLog.CrearArchivolog("[INC000002442213][CONSULSTAREGLAS][VALIDACION FRAUDE][objOferta.cargoFijo]" + dblCFSession, null, null); //INC000002464679
                                        //HttpContext.Current.Session["cargoFijoCuota" + Funciones.CheckStr(objOferta.idFila)] = dblCFSession;
                        }
                    }
                    else
                    {
                                    _objLog.CrearArchivolog("[INC000002442213][VALIDACION FRAUDE][PASO 3]", null, null);
                                }

                    }
                        }
                    }
                    
                    _objLog.CrearArchivolog("[INC000002442213][=================INC FALLA CARGO FIJO FIN =============]", null, null);
                    //[INC000002442213]INC FALLA CARGO FIJO FIN

                    // Modalidad / Operador Cedente
                    oOferta.modalidadCedente = strTipoModalidad;
                    oOferta.operadorCedente = strOperadorCedente;

                    oOferta.campana.tipo = objOferta.campana;
                    oOferta.casoEpecial = (string.IsNullOrEmpty(strCasoEspecial)) ? "REGULAR" : strCasoEspecial;
                    oOferta.controlDeConsumo = objOferta.topeConsumo;
                    oOferta.kitDeInstalacion = String.Empty;

                    // LISTA EQUIPOS
                    List<BEEquipoBRMS> objListaEquipo = objOferta.oEquipo;
                    if (objListaEquipo != null)
                    {
                        int idx = 0;
                        if (objOferta.idProducto == consTipoProductoDTH)
                        {
                            BEEquipoBRMS oEquipoBRMS = (BEEquipoBRMS)objListaEquipo[0];
                            // Consulta Detalle Decos asociados al Equipo
                            List<BEEquipoBRMS> objListaDeco = objEvaluacion.ConsultarDetalleDecoxKIT(oEquipoBRMS.idEquipo);
                            WS.WSReglasCrediticia.equipo[] oListaEquipo = new WS.WSReglasCrediticia.equipo[objListaDeco.Count];

                            foreach (BEEquipoBRMS oPlanEquipo in objListaDeco)
                            {
                                oListaEquipo[idx] = new WS.WSReglasCrediticia.equipo();
                                oListaEquipo[idx].costo = oPlanEquipo.costo;
                                oListaEquipo[idx].cuotas = oEquipoBRMS.cantidadDeCuotas;
                                oListaEquipo[idx].formaDePago = oEquipoBRMS.formaDePago;
                                oListaEquipo[idx].gama = oEquipoBRMS.gama;
                                oListaEquipo[idx].modelo = oPlanEquipo.modelo;
                                oListaEquipo[idx].montoDeCuota = oEquipoBRMS.montoDeCuota;
                                oListaEquipo[idx].porcentajecuotaInicial = oEquipoBRMS.porcentajeCuotaInicial;
                                oListaEquipo[idx].precioDeVenta = oEquipoBRMS.precioDeVenta;
                                oListaEquipo[idx].tipoDeDeco = oPlanEquipo.tipoDeDeco;
                                oListaEquipo[idx].tipoOperacionKit = oEquipoBRMS.tipoDeOperacionKit;
                                idx++;
                            }
                            oSolicitud1.equipo = oListaEquipo;
                            oOferta.kitDeInstalacion = oEquipoBRMS.modelo;
                        }
                        else
                        {
                            WS.WSReglasCrediticia.equipo[] oListaEquipo = new WS.WSReglasCrediticia.equipo[objListaEquipo.Count];
                            foreach (BEEquipoBRMS oPlanEquipo in objListaEquipo)
                            {
                                oListaEquipo[idx] = new WS.WSReglasCrediticia.equipo();
                                oListaEquipo[idx].costo = oPlanEquipo.costo;
                                oListaEquipo[idx].cuotas = oPlanEquipo.cantidadDeCuotas;
                                oListaEquipo[idx].formaDePago = strModalidadVenta;
                                oListaEquipo[idx].gama = oPlanEquipo.gama;
                                oListaEquipo[idx].modelo = oPlanEquipo.modelo;
                                oListaEquipo[idx].montoDeCuota = oPlanEquipo.montoDeCuota;
                                oListaEquipo[idx].porcentajecuotaInicial = oPlanEquipo.porcentajeCuotaInicial;
                                oListaEquipo[idx].precioDeVenta = oPlanEquipo.precioDeVenta;
                                oListaEquipo[idx].tipoDeDeco = oPlanEquipo.tipoDeDeco;
                                oListaEquipo[idx].tipoOperacionKit = oPlanEquipo.tipoDeOperacionKit;
                                oListaEquipo[idx].montoDeCuotaComercial = oPlanEquipo.montoDeCuotaComercial; //PROY-30166-INI
                                oListaEquipo[idx].montoDeCuotaInicialComercial = oPlanEquipo.montoDeCuotaInicialComercial; //PROY-30166-FIN
                                
                                idx++;
                            }
                            oSolicitud1.equipo = oListaEquipo;
                        }
                    }

                    // DIRECCION CLIENTE
                    if (objDireccion != null)
                    {
                        foreach (BEDireccionCliente oDirCliente in objDireccion)
                        {
                            if (oDirCliente.IdFila == objOferta.idFila)
                            {
                                string departamento = null, provincia = null, distrito = null;
                                (new BLGeneral()).ConsultarDatosDireccion(oDirCliente.IdDepartamento, oDirCliente.IdProvincia, oDirCliente.IdDistrito,
                                                                          ref departamento, ref provincia, ref distrito);

                                oSolicitud1.cliente.direccion = new WS.WSReglasCrediticia.direccion();
                                oSolicitud1.cliente.direccion.codigoDePlano = oDirCliente.IdPlano;
                                oSolicitud1.cliente.direccion.departamento = departamento;
                                oSolicitud1.cliente.direccion.provincia = provincia;
                                oSolicitud1.cliente.direccion.distrito = distrito;
                                oSolicitud1.cliente.direccion.region = String.Empty;

                                break;
                            }
                        }
                    }

                    oOferta.planSolicitado = new WS.WSReglasCrediticia.planSolicitado();
                    oOferta.planSolicitado.cargoFijo = objOferta.cargoFijo;
                    oOferta.planSolicitado.descripcion = objOferta.plan;
                    oOferta.planSolicitado.paquete = objOferta.paquete;

                    // LISTA SERVICIOS
                    List<BEItemGenerico> objListaServicio = objOferta.oServicio;
                    if (objListaServicio != null && objListaServicio.Count > 0)
                    {
                        int idx = 0;
                        WS.WSReglasCrediticia.servicio[] oListaServicio = new WS.WSReglasCrediticia.servicio[objListaServicio.Count];

                        foreach (BEItemGenerico oPlanServicio in objListaServicio)
                        {
                            oListaServicio[idx] = new WS.WSReglasCrediticia.servicio();
                            oListaServicio[idx].nombre = oPlanServicio.Descripcion;
                            idx++;
                        }
                        oOferta.planSolicitado.servicio = oListaServicio;
                    }

                    // DTH + BAM [CF PROMOCIONAL]
                    if (objOferta.idProducto == consTipoProductoDTH)
                    {
                        objOferta.cargoFijo += objEvaluacion.ObtenerCFPromocional(objOferta.idCampana);
                    }

                    // COMBO [DESCUENTO CF]
                    if (lstDescuentoProducto != null)
                    {
                        lstDescuentoProducto = lstDescuentoProducto.Where(item => objOferta.idProducto == item.idProducto).ToList();
                        if (lstDescuentoProducto.Count > 0)
                        {
                            dblDescuentoCF = lstDescuentoProducto.First().montoDescuento;
                            objOferta.cargoFijo -= dblDescuentoCF;
                        }
                    }

                    oOferta.plazoDeAcuerdo = objOferta.plazo;
                    oOferta.productoComercial = objOferta.producto;
                    oOferta.proteccionMovil = strTieneProteccionMovil.Equals("SI") ? WS.WSReglasCrediticia.tipoSiNo.SI : WS.WSReglasCrediticia.tipoSiNo.NO; //PROY-24724-IDEA-28174
                    oOferta.proteccionMovilSpecified = true; // INC000004321592
                    oOferta.segmentoDeOferta = strOferta;
                    oOferta.tipoDeOperacionEmpresa = String.Empty;
                    oOferta.combo = strComboTexto;
                    oOferta.mesOperadorCedente = objOferta.cantidadMesesOperadorCedente;
                    //INI: PROY-140335 RF1
                    if (strTipoOperacion == ConfigurationManager.AppSettings["consDescPortabilidad"].ToString() && oSolicitud1.transaccion == ConfigurationManager.AppSettings["constTrxEvaluacion"].ToString())
                    {
                        if (objOferta.flagConsultaPrevia != null)
                        {
                        oOferta.flagConsultaPrevia = objOferta.flagConsultaPrevia.Equals("SI") ? WS.WSReglasCrediticia.tipoSiNo.SI : WS.WSReglasCrediticia.tipoSiNo.NO;
                        oOferta.flagConsultaPreviaSpecified = true;
                        _objLog.CrearArchivolog("[PROY-140335 - INICIO: PARAMETRO ENTRADA BRMS]", null, null);
                        _objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-140335 - Linea]", objOferta.numeroLinea), null, null);
                        _objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-140335 - oOferta.flagConsultaPrevia]", oOferta.flagConsultaPrevia), null, null);
                    }
                                                
                    }
                    //FIN: PROY-140335 RF1

                    //gaa20170215
                    oOferta.cantidadLineasSEC = objOferta.cantidadLineasSEC;
                    oOferta.montoCFSEC = objOferta.montoCFSEC;
                    //fin gaa20170215
                    // Productos Plan [Tipos de Productos que componen al Plan]
                    List<BEBilletera> objBilleteraPlan = objProductoPlanesEval.Find(delegate(BEPlanBilletera obj) { return obj.plan == idPlan; }).oBilletera;

                    // Descripci�n Producto [Texto de Tipos de Productos que componen al Plan]
                    foreach (BEBilletera obj in objBilleteraPlan)
                    {
                        if (oOferta.tipoDeProducto == null) oOferta.tipoDeProducto = obj.billetera;
                        else oOferta.tipoDeProducto += " + " + obj.billetera;
                    }

                    // Nro Planes = Sumatoria Planes [Productos que componen al Plan]
                    int intPlanesActivo = CalcularNroPlanesActivoxProducto(objBilleteraPlanesActivo, objBilleteraPlan);
                    int intPlanesEvaluado = CalcularNroPlanesxProducto(objListaOfertaEvaluado, objBilleteraPlan);
                    int intPlanesTotal = intPlanesActivo + intPlanesEvaluado;

                    _objLog.CrearArchivolog("[xml][intPlanesActivo]" + intPlanesActivo.ToString(), null, null); //PROY - 30748
                    _objLog.CrearArchivolog("[xml][intPlanesEvaluado]" + intPlanesEvaluado.ToString(), null, null); //PROY - 30748
                    _objLog.CrearArchivolog("[xml][intPlanesTotal]" + intPlanesTotal.ToString(), null, null); //PROY - 30748


                    // LC Disponible Plan = Sumatoria [LC - CF] [Productos que componen al Plan]
                    double dblLCDisponible = CalcularMontoxProducto(objCliente.oLCDisponiblexBilletera, objBilleteraPlan);
                    double dblCFEvaluado = CalcularCFEvaluado(objListaOfertaEvaluado, objBilleteraPlan);
                    double dblCF = dblCFEvaluado + objOferta.cargoFijo;

                    // Monto Facturado [Productos que componen al Plan]
                    double dblMontoFacturado = CalcularMontoxProducto(objCliente.oMontoFacturadoxBilletera, objBilleteraPlan);
                    double dblMontoNoFacturado = CalcularMontoxProducto(objCliente.oMontoNoFacturadoxBilletera, objBilleteraPlan);

                    _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0}:{1}", "[BLReglaCrediticia][ConsultarReglasCreditos] objBilleteraPlanesActivo", Funciones.CheckStr(JsonSerializer.Serialize(objBilleteraPlanesActivo))), null);
                    _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0}:{1}", "[BLReglaCrediticia][ConsultarReglasCreditos] objBilleteraPlan", Funciones.CheckStr(JsonSerializer.Serialize(objBilleteraPlan))), null);                    
                    
                    _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0}:{1}", "[BLReglaCrediticia][ConsultarReglasCreditos] dblMontoFacturado", Funciones.CheckStr(dblMontoFacturado)), null);
                    _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0}:{1}", "[BLReglaCrediticia][ConsultarReglasCreditos] dblMontoNoFacturado", Funciones.CheckStr(dblMontoNoFacturado)), null);

                    oSolicitud1.cliente.cantidadDePlanesPorProducto = intPlanesActivo;
                    oSolicitud1.cliente.limiteDeCreditoDisponible = (dblLCDisponible - dblCFEvaluado > 0) ? (dblLCDisponible - dblCFEvaluado) : 0;
                    oSolicitud1.cliente.limiteDeCreditoDisponible = Funciones.CheckDbl(oSolicitud1.cliente.limiteDeCreditoDisponible, 2);
                    oSolicitud1.cliente.facturacionPromedioProducto = Funciones.CheckDbl(dblMontoFacturado + dblMontoNoFacturado, 2);

                    //PROY-32439 MAS INI Agregar 7 parametros para BRMS Modificado
                    ValidacionDeudaBRMS objVariablesEntradaNVoBRMS = new ValidacionDeudaBRMS();
                    objVariablesEntradaNVoBRMS = (ValidacionDeudaBRMS)HttpContext.Current.Session["ObjNvoBRMS"];
                    oSolicitud1.cliente.montoDeudaVencida = objVariablesEntradaNVoBRMS.request.cliente.montoDeudaVencida;
                    oSolicitud1.cliente.montoDeudaCastigada = objVariablesEntradaNVoBRMS.request.cliente.montoDeudaCastigada;
                    oSolicitud1.cliente.montoDisputa = objVariablesEntradaNVoBRMS.request.cliente.disputa.monto;
                    oSolicitud1.cliente.cantidadMontoDisputa = objVariablesEntradaNVoBRMS.request.cliente.disputa.cantidad;
                    oSolicitud1.cliente.antiguedadMontoDisputa = objVariablesEntradaNVoBRMS.request.cliente.disputa.antiguedad;
                    oSolicitud1.cliente.montoTotalDeuda = objVariablesEntradaNVoBRMS.request.cliente.montoDeuda;
                    oSolicitud1.cliente.antiguedadDeuda = objVariablesEntradaNVoBRMS.request.cliente.antiguedadDeuda;
                    //PROY-32439 MAS FIN Agregar 7 parametros para BRMS Modificado

                    //PROY-29121-INI
                    oSolicitud1.cliente.deuda = objCliente.deudaCliente; 
                    _objLog.CrearArchivolog("[Consulta BRMS]", "Parametro BRMS cliente.deuda - " + objCliente.deudaCliente, null);
                    //PROY-29121-FIN

                    oSolicitud1.oferta = oOferta;
                    oSolicitud1.fechaEjecucion = DateTime.Now.Date; //PROY-24724-IDEA-28174
                    oSolicitud1.horaEjecucion = DateTime.Now.Hour; //PROY-24724-IDEA-28174
                    
                    //INICIO PROY-31948

                    _objLog.CrearArchivolog("[PROY-31948 - INICIO consultarCuotaClienteOAC]", null, null);
                    _objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-31948 - montoPendienteCuotasSistema]", objCuotaOAC.montoPendienteCuotasSistema), null, null);
                    _objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-31948 - cantidadPlanesCuotasPendientesSistema]", objCuotaOAC.cantidadPlanesCuotasPendientesSistema), null, null);
                    _objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-31948 - cantidadMaximaCuotasPendientesSistema]", objCuotaOAC.cantidadMaximaCuotasPendientesSistema), null, null);
                    _objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-31948 - cantidadCuotasPendientes]", objCuotaOAC.cantidadCuotasPendientes), null, null);
                    _objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-31948 - montoPendienteCuotas]", objCuotaOAC.montoPendienteCuotas), null, null);
                    _objLog.CrearArchivolog("[PROY-31948 - FIN consultarCuotaClienteOAC]", null, null);

                    oSolicitud1.cliente.montoPendienteCuotasSistema = objCuotaOAC.montoPendienteCuotasSistema;
                    oSolicitud1.cliente.cantidadPlanesCuotasPendientesSistema = objCuotaOAC.cantidadPlanesCuotasPendientesSistema;
                    oSolicitud1.cliente.cantidadMaximaCuotasPendientesSistema = objCuotaOAC.cantidadMaximaCuotasPendientesSistema;

                    /*PROY-140743 - INI*/
                    double montoCuotasPendientesAcc = 0;
                    int cantidadLineaCuotasPendientesAcc = 0;
                    int cantidadMaximaCuotasPendientesAcc = 0;
                    double montoCuotasPendientesAccUltiVenta = 0;
                    int cantidadLineaCuotasPendientesAccUltiVenta = 0;
                    int cantidadMaximaCuotasPendientesAccUltiVenta = 0;

                    bool respuestaBR = ObtenerVariablesBRMS(Funciones.CheckStr(objCliente.nroDoc), strTipoOperacion, ref montoCuotasPendientesAcc, ref cantidadLineaCuotasPendientesAcc, ref cantidadMaximaCuotasPendientesAcc, ref montoCuotasPendientesAccUltiVenta, ref cantidadLineaCuotasPendientesAccUltiVenta, ref cantidadMaximaCuotasPendientesAccUltiVenta);

                    if (respuestaBR)
                    {
                        oSolicitud1.cliente.montoCuotasPendientesAcc = Funciones.CheckDbl(montoCuotasPendientesAcc);
                        oSolicitud1.cliente.cantidadLineaCuotasPendientesAcc = Funciones.CheckInt(cantidadLineaCuotasPendientesAcc);
                        oSolicitud1.cliente.cantidadMaximaCuotasPendientesAcc = Funciones.CheckInt(cantidadMaximaCuotasPendientesAcc);
                        oSolicitud1.cliente.montoCuotasPendientesAccUltiVenta = Funciones.CheckDbl(montoCuotasPendientesAccUltiVenta);
                        oSolicitud1.cliente.cantidadLineaCuotasPendientesAccUltiVenta = Funciones.CheckInt(cantidadLineaCuotasPendientesAccUltiVenta);
                        oSolicitud1.cliente.cantidadMaximaCuotasPendientesAccUltiVenta = Funciones.CheckInt(cantidadMaximaCuotasPendientesAccUltiVenta);
                        oSolicitud1.oferta.promociones = Funciones.CheckStr(HttpContext.Current.Session["strPromocionVV"]);
                        oSolicitud1.oferta.productoCuentaAFacturar = Funciones.CheckStr(prodFacturar);
                    }
                    /*PROY-140743 - FIN*/

                    WS.WSReglasCrediticia.planActual oPlanActual = new WSReglasCrediticia.planActual();
                    oPlanActual.cantidadCuotasPendientes = objCuotaOAC.cantidadCuotasPendientes; //Plan Actual
                    oPlanActual.montoPendienteCuotas = objCuotaOAC.montoPendienteCuotas; //Plan Actual

                    _objLog.CrearArchivolog("[PROY-31948 - INICIO ConsultaCuotasPendientesPVU]", null, null);
                    _objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-31948 - montoPendienteCuotasUltimasVentas]", objCuotaPVU.montoPendienteCuotasUltimasVentas), null, null);
                    _objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-31948 - cantidadPlanesCuotasPendientesUltimasVentas]", objCuotaPVU.cantidadPlanesCuotasPendientesUltimasVentas), null, null);
                    _objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-31948 - cantidadMaximaCuotasPendientesUltimasVentas]", objCuotaPVU.cantidadMaximaCuotasPendientesUltimasVentas), null, null);
                    _objLog.CrearArchivolog("[PROY-31948 - FIN ConsultaCuotasPendientesPVU]", null, null);

                    oSolicitud1.cliente.montoPendienteCuotasUltimasVentas = objCuotaPVU.montoPendienteCuotasUltimasVentas;
                    oSolicitud1.cliente.cantidadPlanesCuotasPendientesUltimasVentas = objCuotaPVU.cantidadPlanesCuotasPendientesUltimasVentas;
                    oSolicitud1.cliente.cantidadMaximaCuotasPendientesUltimasVentas = objCuotaPVU.cantidadMaximaCuotasPendientesUltimasVentas;

                    //PROY-140579 0412 NN INICIO         
                    string SessionIsWhiteList = Funciones.CheckStr((String)HttpContext.Current.Session["SessionIsWhiteList"]);
                    if (SessionIsWhiteList=="SI")                    {

                        oSolicitud1.cliente.flagWhitelist = WSReglasCrediticia.tipoSiNo.SI;
                    }
                    else
                    {
                        oSolicitud1.cliente.flagWhitelist = WSReglasCrediticia.tipoSiNo.NO;
                    }
                    //PROY-140579 0412 NN FIN

                    oOferta.planActual = oPlanActual;
                    oSolicitud1.oferta = oOferta;
                    oEvaluacionCliente.solicitud.solicitud1 = oSolicitud1;
                    //FIN PROY-31948 

                    _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0}:{1}", "[BLReglaCrediticia][ConsultarReglasCreditos] oEvaluacionCliente antes BWReglasCreditica().ConsultaReglaCrediticia", Funciones.CheckStr(JsonSerializer.Serialize(oEvaluacionCliente))), null);

                    // Consulta BRMS
                    objOfrecimiento = (new WS.BWReglasCreditica()).ConsultaReglaCrediticia(nroDocumento, Funciones.CheckStr(prodFacturar), oEvaluacionCliente, ref objMensaje);//PROY-140743

                    _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0}:{1}", "[BLReglaCrediticia][ConsultarReglasCreditos] oEvaluacionCliente despues BWReglasCreditica().ConsultaReglaCrediticia", Funciones.CheckStr(JsonSerializer.Serialize(oEvaluacionCliente))), null);


                    objOfrecimiento.IdFila = objOferta.idFila;
                    objOfrecimiento.IdProducto = objOferta.idProducto;
                    objOfrecimiento.CargoFijo = objOferta.cargoFijo;
                    objOfrecimiento.DescuentoCF = dblDescuentoCF;
                    objOfrecimiento.nroLineaActivoxProducto = intPlanesActivo;
                    objOfrecimiento.nroLineaEvaluadoxProducto = intPlanesEvaluado;
                    objOfrecimiento.montoFacturadoxProducto = dblMontoFacturado;
                    objOfrecimiento.Plan = objOferta.plan;
                    objOfrecimiento.Combo = strComboTexto;
                    objOfrecimiento.creditScore = oEvaluacionCliente.solicitud.solicitud1.cliente.creditScore; //PROY - 30748 - creditScore
                    // AUTONOMIA
                    if (strTipoDoc == consTipoDocRUC)
                        objOfrecimiento.TieneAutonomia = ResultadoAutonomia(objOfrecimiento, strTipoDoc, intPlanesEvaluado, dblCF, dblMontoFacturado);
                    else
                        objOfrecimiento.TieneAutonomia = ResultadoAutonomia(objOfrecimiento, strTipoDoc, intPlanesTotal, dblCF, dblMontoFacturado);

                    _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0}:{1}", "[BLReglaCrediticia][ConsultarReglasCreditos] objOfrecimiento antes de grabarLog", Funciones.CheckStr(JsonSerializer.Serialize(objOfrecimiento))), null);

                    // LOG
                    grabarLog(oEvaluacionCliente, ref objOfrecimiento);

                    // Grabar Log
                    _objLog.CrearArchivolog("[Consulta BRMS]", objOfrecimiento, null);

                    //INI PROY-30748
                    foreach (var equiCuot in oSolicitud1.equipo)
                    {
                        objOfrecimiento.NroCuotaProac = equiCuot.cuotas;
                    }
                    //FIN PROY-30748

                    //INI: PROY-140335 RF1
                    if (strTipoOperacion == ConfigurationManager.AppSettings["consDescPortabilidad"].ToString() && oSolicitud1.transaccion == ConfigurationManager.AppSettings["constTrxEvaluacion"].ToString())
                    {
                    objOfrecimiento.numeroLinea = objOferta.numeroLinea;
                        _objLog.CrearArchivolog("[PROY-140335 RF1 - INI: PARAMETRO SALIDA BRMS - SOLO APLICA PARA PORTABILIDAD]", null, null);
                        _objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-140335 - oSolicitud1.transaccion]", oSolicitud1.transaccion), null, null);
                    _objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-140335 - Linea]", objOferta.numeroLinea), null, null);
                    _objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-140335 - ejecucionConsultaPrevia]", objOfrecimiento.ejecucionConsultaPrevia), null, null);

                    List<BEPorttSolicitud> lstRepositorioPorta = (List<BEPorttSolicitud>)HttpContext.Current.Session["DetalleCPRepositorio"];

                    if (lstRepositorioPorta != null)
                    {
                        BEPorttSolicitud tSolicitud = lstRepositorioPorta.Find(x => x.numeroLinea == objOferta.numeroLinea);

                        if (tSolicitud != null)
                        {
                            tSolicitud.ejecucionConsultaPrevia = objOfrecimiento.ejecucionConsultaPrevia;
                            HttpContext.Current.Session["DetalleCPRepositorio"] = lstRepositorioPorta;
                        }

                        if (lstRepositorioPorta.Exists(x => x.ejecucionConsultaPrevia == "SI"))
                        {
                            objOfrecimiento.ejecucionConsultaPrevia = "SI";
                        }
                    }
                        else
                        {
                            objOfrecimiento.ejecucionConsultaPrevia = string.Empty;
                        }
                    }
                    else
                    {
                        objOfrecimiento.ejecucionConsultaPrevia = string.Empty;
                    }
                   
                    //FIN: PROY-140335 RF1

                    _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0}:{1}", "[BLReglaCrediticia][ConsultarReglasCreditos] objOfrecimiento", Funciones.CheckStr(JsonSerializer.Serialize(objOfrecimiento))), null);

                    objListaOfrecimiento.Add(objOfrecimiento);

                    objOferta.oBilletera = new List<BEBilletera>(objBilleteraPlan);
                    objListaOfertaEvaluado.Add(objOferta);
                    HttpContext.Current.Session["objListaOfertaEvaluadoProa"] = objListaOfertaEvaluado;//PROY-30748 F2 MDE
                    objListaBilleteraEvaluado.AddRange(objBilleteraPlan);
                }
            }
            catch (Exception ex)
            {
                _objLog = new GeneradorLog(null, objCliente.nroDoc, null, "WEB");
                _objLog.CrearArchivolog("[ERROR][ConsultarReglasCreditos]", ex, null);
                throw ex;
            }
        }

        //PROY-29123 Venta en Cuotas INICIO
        public void ConsultarReglasCliente(BEClienteCuenta objCliente, List<BEDireccionCliente> objDireccion, string strFlgCuota, string strDatos, BECuota objCuotaOAC, BECuota objCuotaPVU, string prodFacturar, ref List<BEOfrecimiento> objListaOfrecimiento)//PROY-31948//PROY-140743
        {
            try
            {
                string strTipoDoc = objCliente.tipoDoc;
                string nroDocumentoCli = objCliente.nroDoc;
                this.tipoDocumento = strTipoDoc;
                this.nroDocumento = nroDocumentoCli;

                string[] arrDatosGeneral = strDatos.Split('|');

                string strTipoOperacion = arrDatosGeneral[0];
                string strOferta = arrDatosGeneral[1];
                //PROY-31948 INI
                string strTipoDocumento = strTipoDoc;
                string strNroDocumento = objCliente.nroDoc;
                string strNroLinea = string.Empty;
                //PROY-31948 FIN
                if (strOferta == "SELECCIONE...") strOferta = "";

                // Log
                _objLog = new GeneradorLog(null, nroDocumentoCli, null, consPaginaEvaluacion);

                BEOfrecimiento objOfrecimiento = null;
                objListaOfrecimiento = new List<BEOfrecimiento>();

                // Consulta Datos Evaluaci�n BRMS
                WS.WSReglasCrediticia.ClaroEvalClientesReglasRequest oEvaluacionCliente = datosGeneralEvaluacion(objCliente);
                WS.WSReglasCrediticia.oferta oOferta;
                WS.WSReglasCrediticia.solicitud1 oSolicitud1;
                WS.WSReglasCrediticia.planActual oPlanActual = new WSReglasCrediticia.planActual();//PROY-31948
                
                oSolicitud1 = oEvaluacionCliente.solicitud.solicitud1;
                oSolicitud1.cliente.deuda = objCliente.deudaCliente; //PROY - 29121 
                oSolicitud1.totalPlanes = objCliente.totalplanes;//PROY - 30748
                oSolicitud1.tipoDeOperacion = strTipoOperacion;

                oOferta = new WS.WSReglasCrediticia.oferta();
                oOferta.segmentoDeOferta = strOferta;

                oSolicitud1.oferta = oOferta;
                oSolicitud1.fechaEjecucion = DateTime.Now.Date;//PROY-140579 NN
                oEvaluacionCliente.solicitud.solicitud1 = oSolicitud1;
                // Flujo Reglas Evaluaci�n / Reglas Cuotas
                oSolicitud1.transaccion = ConfigurationManager.AppSettings["constTrxEvaluacion"].ToString();
                if (strFlgCuota == "S")
                    oSolicitud1.transaccion = ConfigurationManager.AppSettings["constTrxVentaCuotas"].ToString();

                //INICIO - PROY-31948

                _objLog.CrearArchivolog("[PROY-31948 - INICIO consultarCuotaClienteOAC]", null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-31948 - montoPendienteCuotasSistema]", objCuotaOAC.montoPendienteCuotasSistema), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-31948 - cantidadPlanesCuotasPendientesSistema]", objCuotaOAC.cantidadPlanesCuotasPendientesSistema), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-31948 - cantidadMaximaCuotasPendientesSistema]", objCuotaOAC.cantidadMaximaCuotasPendientesSistema), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-31948 - cantidadCuotasPendientes]", objCuotaOAC.cantidadCuotasPendientes), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-31948 - montoPendienteCuotas]", objCuotaOAC.montoPendienteCuotas), null, null);
                _objLog.CrearArchivolog("[PROY-31948 - FIN consultarCuotaClienteOAC]", null, null);

                oSolicitud1.cliente.montoPendienteCuotasSistema = objCuotaOAC.montoPendienteCuotasSistema;
                oSolicitud1.cliente.cantidadPlanesCuotasPendientesSistema = objCuotaOAC.cantidadPlanesCuotasPendientesSistema;
                oSolicitud1.cliente.cantidadMaximaCuotasPendientesSistema = objCuotaOAC.cantidadMaximaCuotasPendientesSistema;

                oPlanActual.cantidadCuotasPendientes = objCuotaOAC.cantidadCuotasPendientes;
                oPlanActual.montoPendienteCuotas = objCuotaOAC.montoPendienteCuotas;

                _objLog.CrearArchivolog("[PROY-31948 - INICIO ConsultaCuotasPendientesPVU]", null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-31948 - montoPendienteCuotasUltimasVentas]", objCuotaPVU.montoPendienteCuotasUltimasVentas), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-31948 - cantidadPlanesCuotasPendientesUltimasVentas]", objCuotaPVU.cantidadPlanesCuotasPendientesUltimasVentas), null, null);
                _objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-31948 - cantidadMaximaCuotasPendientesUltimasVentas]", objCuotaPVU.cantidadMaximaCuotasPendientesUltimasVentas), null, null);
                _objLog.CrearArchivolog("[PROY-31948 - FIN ConsultaCuotasPendientesPVU]", null, null);

                oSolicitud1.cliente.montoPendienteCuotasUltimasVentas = objCuotaPVU.montoPendienteCuotasUltimasVentas;
                oSolicitud1.cliente.cantidadPlanesCuotasPendientesUltimasVentas = objCuotaPVU.cantidadPlanesCuotasPendientesUltimasVentas;
                oSolicitud1.cliente.cantidadMaximaCuotasPendientesUltimasVentas = objCuotaPVU.cantidadMaximaCuotasPendientesUltimasVentas;

                ////PROY-140579 RU11 NN INICIO
                string SessionIsWhiteList = Funciones.CheckStr((String)HttpContext.Current.Session["SessionIsWhiteList"]);
                if (SessionIsWhiteList == "SI")
                {

                    oSolicitud1.cliente.flagWhitelist = WSReglasCrediticia.tipoSiNo.SI;
                }
                else
                {
                    oSolicitud1.cliente.flagWhitelist = WSReglasCrediticia.tipoSiNo.NO;
                }
                //PROY-140579 RU11 NN FIN

                /*PROY-140743 - INI*/
                double montoCuotasPendientesAcc = 0;
                int cantidadLineaCuotasPendientesAcc = 0;
                int cantidadMaximaCuotasPendientesAcc = 0;
                double montoCuotasPendientesAccUltiVenta = 0;
                int cantidadLineaCuotasPendientesAccUltiVenta = 0;
                int cantidadMaximaCuotasPendientesAccUltiVenta = 0;

                bool respuestaBR = ObtenerVariablesBRMS(Funciones.CheckStr(objCliente.nroDoc), strTipoOperacion, ref montoCuotasPendientesAcc, ref cantidadLineaCuotasPendientesAcc, ref cantidadMaximaCuotasPendientesAcc, ref montoCuotasPendientesAccUltiVenta, ref cantidadLineaCuotasPendientesAccUltiVenta, ref cantidadMaximaCuotasPendientesAccUltiVenta);

                if (respuestaBR)
                {
                    oSolicitud1.cliente.montoCuotasPendientesAcc = Funciones.CheckDbl(montoCuotasPendientesAcc);
                    oSolicitud1.cliente.cantidadLineaCuotasPendientesAcc = Funciones.CheckInt(cantidadLineaCuotasPendientesAcc);
                    oSolicitud1.cliente.cantidadMaximaCuotasPendientesAcc = Funciones.CheckInt(cantidadMaximaCuotasPendientesAcc);
                    oSolicitud1.cliente.montoCuotasPendientesAccUltiVenta = Funciones.CheckDbl(montoCuotasPendientesAccUltiVenta);
                    oSolicitud1.cliente.cantidadLineaCuotasPendientesAccUltiVenta = Funciones.CheckInt(cantidadLineaCuotasPendientesAccUltiVenta);
                    oSolicitud1.cliente.cantidadMaximaCuotasPendientesAccUltiVenta = Funciones.CheckInt(cantidadMaximaCuotasPendientesAccUltiVenta);
                    oSolicitud1.oferta.promociones = Funciones.CheckStr(HttpContext.Current.Session["strPromocionVV"]);
                    oSolicitud1.oferta.productoCuentaAFacturar = Funciones.CheckStr(prodFacturar);
                }
                /*PROY-140743 - FIN*/

                oOferta.planActual = oPlanActual;
                oEvaluacionCliente.solicitud.solicitud1 = oSolicitud1;

                //FIN - PROY-31948

                // Consulta BRMS
                objOfrecimiento = (new WS.BWReglasCreditica()).ConsultaReglaCrediticia(nroDocumentoCli, Funciones.CheckStr(prodFacturar), oEvaluacionCliente, ref objMensaje);//PROY-140743

                // Grabar Log
                _objLog.CrearArchivolog("[Consulta BRMS]", objOfrecimiento, null);

                objListaOfrecimiento.Add(objOfrecimiento);
                                
            }
            catch (Exception ex)
            {
                _objLog = new GeneradorLog(null, objCliente.nroDoc, null, "WEB");
                _objLog.CrearArchivolog("[ERROR][ConsultarReglasCliente]", ex, null);
                throw ex;
            }
        }
        //PROY-29123 Venta en Cuotas FIN
        public List<BEOfertaBRMS> ObtenerDetalleEvaluacion(string strPlanesDetalle, string strServiciosDetalle, string strEquiposDetalle)
        {
            List<BEOfertaBRMS> objDetalleEval = new List<BEOfertaBRMS>();
            List<BEOfertaBRMS> objPlanDetalle = new List<BEOfertaBRMS>();
            objPlanDetalle = ObtenerDetallePlanesEval(strPlanesDetalle);

            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0}:{1}", "[BLReglaCrediticia][ObtenerDetalleEvaluacion] objPlanDetalle", Funciones.CheckStr(JsonSerializer.Serialize(objPlanDetalle))), null);

            objDetalleEval = ObtenerDetallePlanEquipoEval(objPlanDetalle, strServiciosDetalle, strEquiposDetalle);
            return objDetalleEval;
        }

        private List<BEOfertaBRMS> ObtenerDetallePlanesEval(String strPlanesDetalle)
        {
            List<BEOfertaBRMS> listaPlanDetalle = new List<BEOfertaBRMS>();

            //INI: PROY-140335 RF1
            List<BEPorttSolicitud> lstDatosRepositorio = null;
            if (HttpContext.Current.Session["DetalleCPRepositorio"] != null)
            {
                  lstDatosRepositorio = (List<BEPorttSolicitud>)HttpContext.Current.Session["DetalleCPRepositorio"];
            }
            //FIN: PROY-140335 RF1
            
            string[] arrPlanDetalle = strPlanesDetalle.Split(new char[] {'|'}, StringSplitOptions.RemoveEmptyEntries);
            BEOfertaBRMS objOferta;
            List<BEItemGenerico> objListaPlazo = new BLGeneral().ListarPlazoAcuerdo("");
            List<BEItemGenerico> objListaProducto = new BLGeneral().ListarProducto();
            foreach (string strDetalle in arrPlanDetalle)
            {
                objOferta = new BEOfertaBRMS();
                string[] objPlan = strDetalle.ToUpper().Split(';');
                objOferta.idFila = Funciones.CheckInt(objPlan[0]);
                objOferta.idProducto = objPlan[1];
                // Producto
                foreach (BEItemGenerico producto in objListaProducto)
                {
                    if (producto.Codigo == objOferta.idProducto)
                    {
                        objOferta.producto = producto.Descripcion;
                        break;
                    }
                }
                // Plazo Acuerdo
                objOferta.idPlazo = objPlan[2];
                foreach (BEItemGenerico plazo in objListaPlazo)
                {
                    if (plazo.Codigo == objOferta.idPlazo)
                    {
                        objOferta.plazo = plazo.Descripcion;
                        break;
                    }
                }
                objOferta.idPaquete = objPlan[3].Split('_')[0];
                objOferta.paquete = objPlan[4];
                objOferta.idPlan = objPlan[5].Split('_')[0];
                objOferta.plan = objPlan[6];

                objOferta.topeConsumo = ConfigurationManager.AppSettings["ConstTextSinTopeConsumo"].ToString();
                if (objPlan[7] == ConfigurationManager.AppSettings["constCodTopeCeroServicio"].ToString())
                    objOferta.topeConsumo = "TOPE DE CONSUMO CERO";

                if (objPlan[7] == ConfigurationManager.AppSettings["constCodTopeSinCFServicio"].ToString())
                    objOferta.topeConsumo = "TOPE DE CONSUMO SIN CF";

                if (objPlan[7] == ConfigurationManager.AppSettings["constCodTopeAutomatico"].ToString())
                    objOferta.topeConsumo = "TOPE DE CONSUMO AUTOMATICO";

                if (objOferta.idProducto == consTipoProductoHFC)
                    objOferta.topeConsumo = objPlan[7];

                objOferta.idCampana = objPlan[8];
                objOferta.campana = objPlan[9];
                objOferta.cargoFijo = Funciones.CheckDbl(objPlan[10], 2);
                //gaa20170215
                objOferta.cantidadLineasSEC = Funciones.CheckInt(objPlan[12]);
                objOferta.montoCFSEC = Funciones.CheckDbl(objPlan[11], 2);
                //fin gaa20170215
                objOferta.cantidadMesesOperadorCedente = new BEPorttSolicitud() { fechaActivacionCP = objPlan[13] }.cantidadMesesOperadorCedente;
                //PROY-140335 INI RF1
                if (objOferta.idProducto == consTipoProductoMovil || objOferta.idProducto == consTipoProductoBAM)
                {
                objOferta.flagConsultaPrevia = (lstDatosRepositorio != null && objPlan[16].Trim().Length > 0) ? lstDatosRepositorio.Find(w => w.numeroLinea == objPlan[16]).flagConsultaPrevia : "NO"; 
                _objLog.CrearArchivolog("[PROY-140335 RF1 - ObtenerDetallePlanesEval objOferta.flagConsultaPrevia]-->", objOferta.flagConsultaPrevia, null);
                objOferta.numeroLinea = objPlan[16]; 
                }
                //PROY-140335 FIN RF1
                listaPlanDetalle.Add(objOferta);
            }
            listaPlanDetalle = listaPlanDetalle.OrderBy(o => o.idFila).ToList();

            return listaPlanDetalle;
        }

        private List<BEOfertaBRMS> ObtenerDetallePlanEquipoEval(List<BEOfertaBRMS> objPlanDetalle, string strServiciosDetalle, string strEquiposDetalle)
        {
            HelperLog.EscribirLog("[PROY-140579]", "[PROY-140579 ACUMULADOR EQUIPO ObtenerDetallePlanEquipoEval INICIO]", "", false);
            List<BEOfertaBRMS> listaPlanDetalle = new List<BEOfertaBRMS>();

            string idFila = String.Empty;
            string[] arrPlanEquipo = strEquiposDetalle.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            string[] arrPlanServicio = strServiciosDetalle.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            BEEquipoBRMS objEquipo;
            BEItemGenerico objServicio;
            List<BEEquipoBRMS> objListaEquipo;
            List<BEItemGenerico> objListaServicio;

            foreach (BEOfertaBRMS objOferta in objPlanDetalle)
            {
                objListaEquipo = new List<BEEquipoBRMS>();
                foreach (string strEquipo in arrPlanEquipo)
                {
                    idFila = strEquipo.Split(';')[0];
                    if (objOferta.idFila.ToString() == idFila)
                    {
                        objEquipo = new BEEquipoBRMS();
                        objEquipo.idFila = strEquipo.Split(';')[0];
                        objEquipo.idProducto = strEquipo.Split(';')[1];
                        objEquipo.idEquipo = strEquipo.Split(';')[2];
                        objEquipo.costo = Funciones.CheckDbl(strEquipo.Split(';')[4], 2);
                        objEquipo.modelo = strEquipo.Split(';')[3];
                        objEquipo.montoDeCuota = Funciones.CheckDbl(strEquipo.Split(';')[8], 2);
                        objEquipo.precioDeVenta = Funciones.CheckDbl(strEquipo.Split(';')[5], 2);
                        objEquipo.cantidadDeCuotas = Funciones.CheckInt(strEquipo.Split(';')[6]);
                        objEquipo.porcentajeCuotaInicial = Funciones.CheckDbl(strEquipo.Split(';')[7], 2);
                        objEquipo.montoDeCuotaInicialComercial = dblCuotaInicialComercial; //PROY-30166-INI
                        objEquipo.montoDeCuotaComercial = dblMontoCuotaComercial; //PROY-30166-FIN
                        
                        if (objEquipo.cantidadDeCuotas > 0)
                        {
                            objEquipo.formaDePago = ConfigurationManager.AppSettings["constFormaPagoCuota"].ToString();
                        }
                        if (objEquipo.idProducto == ConfigurationManager.AppSettings["consTipoProductoDTH"].ToString())
                        {
                            objEquipo.tipoDeOperacionKit = ConfigurationManager.AppSettings["constTipoKitDECO"];
                            objEquipo.formaDePago = ConfigurationManager.AppSettings["constFormaPagoComodato"];
                        }

                        //PROY-140579 montoDeCuotaComercial montoDeCuotaInicialComercial INICIO
                        try
                        {
                            HelperLog.EscribirLog("[PROY-140579]", "[PROY-140579 ACUMULADOR EQUIPO INICIO]", "", false);
                            string strEquiposGarantiaProa = Funciones.CheckStr((String)HttpContext.Current.Session["objEquipoGarantiaProa"]);
                            string strEquipoGarantiaProa = objEquipo.modelo + "|" + objEquipo.montoDeCuotaComercial + "|" + objEquipo.montoDeCuotaInicialComercial + "_";
                            if ((Funciones.CheckStr(strEquiposGarantiaProa)).Equals(string.Empty))
                            {
                                HttpContext.Current.Session["objEquipoGarantiaProa"] = strEquipoGarantiaProa;
                                HelperLog.EscribirLog("[PROY-140579]", "PROY-140579 [EQUIPO] - ", strEquipoGarantiaProa, false);
                            }
                            else
                            {
                                string acuEquiposGarantiaProa = Funciones.CheckStr((String)HttpContext.Current.Session["objEquipoGarantiaProa"]);
                                HttpContext.Current.Session["objEquipoGarantiaProa"] = acuEquiposGarantiaProa + strEquipoGarantiaProa;
                                acuEquiposGarantiaProa = Funciones.CheckStr((String)HttpContext.Current.Session["objEquipoGarantiaProa"]);
                                HelperLog.EscribirLog("[PROY-140579]", "PROY-140579 [ACUMULADOR EQUIPO] - ", acuEquiposGarantiaProa, false);
                            }
                            HelperLog.EscribirLog("[PROY-140579]", "[PROY-140579 ACUMULADOR EQUIPO FIN]", "", false);
                           
                        }
                        catch(Exception ex)
                        {
                            HelperLog.EscribirLog("[PROY-140579]", "[PROY-140579 ACUMULADOR EQUIPO FIN] ", ex.Message.ToString(), false);
                        }
                        //PROY-140579 montoDeCuotaComercial montoDeCuotaInicialComercial FIN

                        objListaEquipo.Add(objEquipo);
                    }
                }
                objOferta.oEquipo = objListaEquipo;

                objListaServicio = new List<BEItemGenerico>();
                foreach (string strServicio in arrPlanServicio)
                {
                    idFila = strServicio.Split(';')[0];
                    if (objOferta.idFila.ToString() == idFila)
                    {
                        string[] arrServicio = strServicio.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                        for (int i = 1; i < arrServicio.Length; i++)
                        {
                            objServicio = new BEItemGenerico();
                            objServicio.Descripcion = arrServicio[i];
                            objListaServicio.Add(objServicio);
                        }
                    }
                }
                objOferta.oServicio = objListaServicio;

                listaPlanDetalle.Add(objOferta);
            }
            HelperLog.EscribirLog("", "[PROY-140579 ACUMULADOR EQUIPO ObtenerDetallePlanEquipoEval FIN]", "", false);
            return listaPlanDetalle;
        }

        private List<BEPlanBilletera> ObtenerProductosPlanesEval(List<BEOfertaBRMS> objPlanDetalle)
        {
            string strPlanesEvaluados = string.Empty;
            int intSistemaSISACT = (int)BEPlanBilletera.TIPO_SISTEMA.SISACT;
            List<BEPlanBilletera> objProductoPlanesEval = new List<BEPlanBilletera>();
            foreach (BEOfertaBRMS item in objPlanDetalle)
            {
                strPlanesEvaluados = string.Format("{0}|{1};{2}", strPlanesEvaluados, item.idPlan, intSistemaSISACT.ToString());
            }

            _objLog.CrearArchivolog("[INC000004091065]", string.Format("{0}:{1}", "[BLReglaCrediticia][ObtenerProductosPlanesEval] strPlanesEvaluados", Funciones.CheckStr(strPlanesEvaluados)), null);

            objProductoPlanesEval = (new BLEvaluacion()).ObtenerBilleteraxPlan(strPlanesEvaluados);
            return objProductoPlanesEval;
        }

        private WS.WSReglasCrediticia.ClaroEvalClientesReglasRequest datosGeneralEvaluacion(BEClienteCuenta objCliente)
        {
            string strCodOficina = objCliente.oficina;
            string strCodTipoDoc = objCliente.tipoDoc;
            string strNroDocumento = objCliente.nroDoc;
            string strNroOperacion = objCliente.nroOperacionBuro;

            WS.WSReglasCrediticia.ClaroEvalClientesReglasRequest oEvaluacionCliente = new WS.WSReglasCrediticia.ClaroEvalClientesReglasRequest();
            DataSet dsDatosEvaluacionCliente = (new BLEvaluacion()).ObtenerDatosEvaluacion(strCodOficina, strCodTipoDoc, strNroDocumento, strNroOperacion);

            if (dsDatosEvaluacionCliente != null)
            {
                DataTable dtOficina = dsDatosEvaluacionCliente.Tables[0];
                DataTable dtCliente = dsDatosEvaluacionCliente.Tables[1];
                DataTable dtRepresentante = dsDatosEvaluacionCliente.Tables[2];

                WS.WSReglasCrediticia.solicitud oSolicitud = new WS.WSReglasCrediticia.solicitud();
                WS.WSReglasCrediticia.puntodeVenta oOficina = new WS.WSReglasCrediticia.puntodeVenta();
                WS.WSReglasCrediticia.direccion oDireccionOficina = new WS.WSReglasCrediticia.direccion();

                // Informaci�n Punto de Venta
                oOficina.codigo = Funciones.CheckStr(dtOficina.Rows[0]["CODIGO"]);
                oOficina.descripcion = Funciones.CheckStr(dtOficina.Rows[0]["PDV"]);
                oOficina.calidadDeVendedor = String.Empty;
                oOficina.canal = Funciones.CheckStr(dtOficina.Rows[0]["CANAL"]);

                // Informaci�n Direccion Punto de Venta
                oDireccionOficina.codigoDePlano = String.Empty;
                oDireccionOficina.departamento = Funciones.CheckStr(dtOficina.Rows[0]["DEPARTAMENTO"]);
                oDireccionOficina.distrito = Funciones.CheckStr(dtOficina.Rows[0]["DISTRITO"]);
                oDireccionOficina.provincia = Funciones.CheckStr(dtOficina.Rows[0]["PROVINCIA"]);
                oDireccionOficina.region = Funciones.CheckStr(dtOficina.Rows[0]["REGION"]);
                oOficina.direccion = oDireccionOficina;
                WS.WSReglasCrediticia.cliente oCliente = new WS.WSReglasCrediticia.cliente();
                WS.WSReglasCrediticia.documento oDocumentoCliente = new WS.WSReglasCrediticia.documento();

                // Informaci�n Cliente
                oDocumentoCliente.numero = Funciones.CheckStr(dtCliente.Rows[0]["NRO_DOCUMENTO"]);
                oDocumentoCliente.descripcion = ObtenerTipoDocumento(strCodTipoDoc, strNroDocumento);
                oDocumentoCliente.descripcionSpecified = true;
                oCliente.documento = oDocumentoCliente;
                oCliente.cantidadDeLineasActivas = objCliente.nroPlanesActivos;
                oCliente.comportamientoDePago = objCliente.comportamientoPago;
                oCliente.edad = Funciones.CheckInt(dtCliente.Rows[0]["EDAD"]);
                oCliente.facturacionPromedioClaro = objCliente.montoFacturadoTotal + objCliente.montoNoFacturadoTotal;
                oCliente.creditScore = Funciones.CheckDbl(dtCliente.Rows[0]["PUNTAJE"]);
                oCliente.riesgo = Funciones.CheckStr(dtCliente.Rows[0]["RIESGO"]);
                oCliente.sexo = Funciones.CheckStr(dtCliente.Rows[0]["SEXO"]);
                oCliente.tiempoDePermanencia = objCliente.tiempoPermanencia;
                oCliente.tipo = objCliente.tipoCliente;
                //PROY-140230-MAS-INI
                oCliente.segmento = Funciones.CheckStr(HttpContext.Current.Session["strClienteSegmento"]);
                _objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140230 - SEGMENTO DEL CLIENTE][PARAMETROS DE ENTRADA][INI]", ""), null, null);
                _objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140230 - SEGMENTO DEL CLIENTE][tipoDocumento]", oCliente.segmento), null, null);
                _objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140230 - SEGMENTO DEL CLIENTE][PARAMETROS DE ENTRADA][INI]", ""), null, null);
                //PROY-140230-MAS-FIN

                // Informaci�n Lista de Representantes Legales
                if (strCodTipoDoc == consTipoDocRUC)
                {
                    WS.WSReglasCrediticia.representanteLegal oRRLL;
                    WS.WSReglasCrediticia.representanteLegal[] oListaRRLL = new WS.WSReglasCrediticia.representanteLegal[dtRepresentante.Rows.Count];
                    WS.WSReglasCrediticia.documento oDocumentoRRLL;
                    int idx = 0;
                    /*PROY-32438*/
                    int CantidadMesesRRLL;
                    string cargoRR,fechnomRR;
                    _objLog.CrearArchivolog("- INI: PARAMETROS DE SALIDA PROY-32438" + null, null, null);
                    var flagApagado = HttpContext.Current.Session["FlagApagado32438"] ?? ""; //PROY-32438 "Flag Apagado/Encendido"
                    _objLog.CrearArchivolog("- REGLACREDITICIA - flagApagado -> " + flagApagado, null, null);
                    _objLog.CrearArchivolog("- FIN: PARAMETROS DE SALIDA PROY-32438" + null, null, null);
                    /*PROY-32438*/
                    foreach (DataRow dr in dtRepresentante.Rows)
                    {
                        List<BERepresentanteLegal> beRep = new List<BERepresentanteLegal>();/*PROY-32438*/
                        oRRLL = new WS.WSReglasCrediticia.representanteLegal();
                        oDocumentoRRLL = new WS.WSReglasCrediticia.documento();
                        oDocumentoRRLL.numero = Funciones.CheckStr(dr["NUMERO_DOCUMENTO"]);
                        oDocumentoRRLL.descripcion = ObtenerTipoDocumento(dr["TIPO_DOCUMENTO"].ToString(), dr["NUMERO_DOCUMENTO"].ToString());
                        oDocumentoRRLL.descripcionSpecified = true;
                        oRRLL.documento = oDocumentoRRLL;
                        oRRLL.riesgo = Funciones.CheckStr(dr["RIESGO"]);
                        /*PROY-32438 INI*/
                        beRep = objCliente.oRepresentanteLegal.Where(w => w.APODV_NUM_DOC_REP == oDocumentoRRLL.numero).ToList();//Select(j => Funciones.CheckStr(j.APODI_CANT_MESES_NOMBRAMIENTO)).ToString();
                        CantidadMesesRRLL = Convert.ToInt32(beRep[0].APODI_CANT_MESES_NOMBRAMIENTO);
                        cargoRR = beRep[0].APODV_CAR_REP;
                        fechnomRR = beRep[0].APODD_FECHA_NOMBRAMIENTO;

                        //PROY-32438 "Flag Apagado/Encendido" INI
                        if (flagApagado.ToString().Equals("0"))
                        {
                            oRRLL.cantidadMesesNombramiento = CantidadMesesRRLL;
                            oRRLL.cargo = cargoRR;
                            if (fechnomRR!=null) { oRRLL.fechaNombramiento = DateTime.Parse(fechnomRR); }                           
                        }
                        //PROY-32438 "Flag Apagado/Encendido" FIN

                        /*PROY-32438 FIN*/

                        oListaRRLL[idx] = new WS.WSReglasCrediticia.representanteLegal();
                        oListaRRLL[idx] = oRRLL;
                        idx++;
                    }
                    WS.WSReglasCrediticia.contribuyente objContribuyente=new WSReglasCrediticia.contribuyente();/*PROY-32438*/
                    oCliente.representanteLegal = oListaRRLL;
                    /*PROY-32438 INI*/
                    if (flagApagado.ToString().Equals("0")) //PROY-32438 "Flag Apagado/Encendido" INI
                    {
                        objContribuyente.tipo = objCliente.TipContribuyente;
                        objContribuyente.nombreComercial = objCliente.NomComercial;
                        objContribuyente.fechaInicioActividades = Convert.ToDateTime(objCliente.FecIniActividades);
                        objContribuyente.estado = objCliente.EstContribuyente;
                        objContribuyente.condicion = objCliente.CondContribuyente;
                        objContribuyente.CIU = objCliente.CiiuContribuyente;
                        objContribuyente.cantidadTrabajadores = Convert.ToInt16(Funciones.CheckInt16(objCliente.CantTrabajadores));
                        if (objCliente.EmisionComp != null)
                        {
                            objContribuyente.autorizacionImpresion = objCliente.EmisionComp;
                        } 
                        objContribuyente.sistemaEmisionElectronica = objCliente.SistEmielectronica;
                        objContribuyente.cantidadMesesInicioActividades = objCliente.CantMesIniActividades;
                        oCliente.contribuyente = objContribuyente;
                    } //PROY-32438 "Flag Apagado/Encendido" FIN
                    /*PROY-32438 FIN*/
                }

                // Informaci�n Solicitud
                oSolicitud.solicitud1 = new WS.WSReglasCrediticia.solicitud1();
                oSolicitud.solicitud1.flagDeLicitacion = WS.WSReglasCrediticia.tipoSiNo.NO;
                oSolicitud.solicitud1.flagDeLicitacionSpecified = true;
                oSolicitud.solicitud1.tipoDeBolsa = String.Empty;
                oSolicitud.solicitud1.tipoDeDespacho = ConfigurationManager.AppSettings["constTextoPDV"].ToString();
                oSolicitud.solicitud1.cliente = oCliente;
                oSolicitud.solicitud1.puntodeVenta = oOficina;
                //gaa20170215
                if (objCliente.buroConsultado == null)
                    objCliente.buroConsultado = string.Empty;
                if (Funciones.isNumeric(objCliente.buroConsultado))
                {
                BLEvaluacion objEvaluacion = new BLEvaluacion();
                oSolicitud.solicitud1.buroConsultado = objEvaluacion.ObtenerBuroDescripcion(objCliente.buroConsultado);
                }
                else
                    oSolicitud.solicitud1.buroConsultado = objCliente.buroConsultado;
                //fin gaa20170215

                oSolicitud.solicitud1.totalPlanes = 1; /*PROY-31636-RENTESEG*/

                oEvaluacionCliente.solicitud = oSolicitud;
            }
            return oEvaluacionCliente;
        }

        private string ResultadoAutonomia(BEOfrecimiento oOfrecimiento, string strTipoDoc, int nroPlanes, double dblCF, double dblMontoFacturado)
        {
            string strAutonomia = "N";
            double dblMontoCFPermitido;

            if (oOfrecimiento.TieneAutonomia != "SIN_REGLAS")
            {
                if (Funciones.CheckStr(oOfrecimiento.Restriccion) != "NO")
                    strAutonomia = "NO_CONDICION";
                else
                {
                    if (strTipoDoc == consTipoDocRUC)
                    {
                        if (oOfrecimiento.CantidadDeLineasAdicionalesRUC >= nroPlanes)
                        {
                            if (oOfrecimiento.TipoDeAutonomiaCargoFijo == "REFERENCIAL")
                                dblMontoCFPermitido = Funciones.CheckDbl(oOfrecimiento.MontoCFParaRUC * (dblMontoFacturado / 100), 2);
                            else
                                dblMontoCFPermitido = oOfrecimiento.MontoCFParaRUC;

                            if (dblMontoCFPermitido >= dblCF)
                                strAutonomia = "S";
                        }
                    }
                    else if (oOfrecimiento.CantidadDeLineasMaximas >= nroPlanes)
                        strAutonomia = "S";
                }
            }
            else
                strAutonomia = oOfrecimiento.TieneAutonomia;

            return strAutonomia;
        }

        private BEVistaEvaluacion ResultadoOfrecimiento(List<BEOfrecimiento> objOfrecimiento)
        {
            double dblImporte = 0.0;
            double dblImporteTotal = 0.0;
            double dblNroGarantia = 0.0;
            string constTipoGarantiaNinguno = ConfigurationManager.AppSettings["constCodTipoGarantiaNinguno"].ToString();
            string constGarantiaNinguno = string.Empty;

            BEGarantia objGarantia;
            List<BEGarantia> objListaGarantia = new List<BEGarantia>();
            BEVistaEvaluacion objVistaEvaluacion = new BEVistaEvaluacion();

            List<BEItemGenerico> objListaProducto = (new BLGeneral()).ListarProducto();
            List<BEItemGenerico> objListaTipoGarantia = (new BLGeneral()).ListarTipoGarantia("", "1");

            foreach (BEItemGenerico objItem in objListaTipoGarantia)
            {
                if (objItem.Codigo == constTipoGarantiaNinguno)
                {
                    constGarantiaNinguno = objItem.Descripcion.ToUpper();
                    break;
                }
            }

            foreach (BEOfrecimiento obj in objOfrecimiento)
            {
                GeneradorLog _objLog = new GeneradorLog(Funciones.CheckStr(obj.In_doc_cliente), "", null, "WEB");
        
                _objLog.CrearArchivolog("[=======================================]", null, null); 
                objGarantia = new BEGarantia();

                _objLog.CrearArchivolog("[valor][obj.IdFila]", Funciones.CheckStr(obj.IdFila), null); 
                objGarantia.IdFila = obj.IdFila;
                _objLog.CrearArchivolog("[valor][obj.idGarantia]", constTipoGarantiaNinguno, null); 
                objGarantia.idGarantia = constTipoGarantiaNinguno;
                _objLog.CrearArchivolog("[valor][garantia]", constGarantiaNinguno, null); 
                objGarantia.garantia = constGarantiaNinguno;
                _objLog.CrearArchivolog("[valor][obj.IdProducto]",Funciones.CheckStr(obj.IdProducto), null); 
                objGarantia.idProducto = obj.IdProducto;
                _objLog.CrearArchivolog("[valor][obj.CargoFijo]", Funciones.CheckStr(Funciones.CheckDbl(obj.CargoFijo, 2)), null); 
                objGarantia.CF = Funciones.CheckDbl(obj.CargoFijo, 2);
                objGarantia.nroGarantia = 1;
                _objLog.CrearArchivolog("[valor][obj.Plan]", Funciones.CheckStr(obj.Plan), null); 
                objGarantia.plan = obj.Plan;

                foreach (BEItemGenerico objItem in objListaProducto)
                {
                    if (objItem.Codigo == obj.IdProducto)
                    {
                        objGarantia.producto = objItem.Descripcion;
                        _objLog.CrearArchivolog("[valor][objGarantia.producto]", objGarantia.producto, null); 
                        break;
                    }
                }
                _objLog.CrearArchivolog("[valor][obj.TieneAutonomia]", obj.TieneAutonomia, null); 

                if (obj.TieneAutonomia != "SIN_REGLAS")
                {
                    foreach (BEItemGenerico objItem in objListaTipoGarantia)
                    {
                        if (objItem.Descripcion.ToUpper() == obj.TipoDeGarantia.ToUpper())
                        {
                            _objLog.CrearArchivolog("[valor][objItem.Descripcion]", Funciones.CheckStr(objItem.Descripcion), null); 
                            objGarantia.garantia = objItem.Descripcion.ToUpper();
                            _objLog.CrearArchivolog("[valor][objItem.Codigo]", Funciones.CheckStr(objItem.Codigo), null); 
                            objGarantia.idGarantia = objItem.Codigo;
                            break;
                        }
                    }

                    _objLog.CrearArchivolog("[valor][obj.Tipodecobro]", Funciones.CheckStr(obj.Tipodecobro), null); 
                    if (obj.Tipodecobro == "REFERENCIAL")
                    {

                        _objLog.CrearArchivolog("[valor][obj.MontoDeGarantia]", Funciones.CheckStr(obj.MontoDeGarantia), null);   
                        _objLog.CrearArchivolog("[valor][obj.CargoFijo]", Funciones.CheckStr(obj.CargoFijo), null);                
                        dblImporte = Math.Round(obj.MontoDeGarantia * obj.CargoFijo);
                    }

                    else {

                        _objLog.CrearArchivolog("[valor][obj.MontoDeGarantia]", Funciones.CheckStr(obj.MontoDeGarantia), null);          
                        dblImporte = obj.MontoDeGarantia;
                    }
                    _objLog.CrearArchivolog("[valor][dblImporte]", Funciones.CheckStr(dblImporte), null);           
                    if (obj.CargoFijo > 0)
                    {
                        // CALCULO IMPORTE DTH
                        if (obj.IdProducto == consTipoProductoDTH)
                        {
                            if (dblImporte % 5 > 0)
                                dblImporte = 5 * (Funciones.CheckInt(dblImporte / 5) + 1);
                        }
                        _objLog.CrearArchivolog("[valor][dblImporte]", Funciones.CheckStr(dblImporte), null);                 
                        _objLog.CrearArchivolog("[valor sin redondear][dblNroGarantia]", Funciones.CheckStr(Math.Round(dblImporte / obj.CargoFijo, 2)), null);   
                        _objLog.CrearArchivolog("[valor redondeado][dblNroGarantia]", Funciones.CheckStr(Math.Round(dblImporte / obj.CargoFijo, 0)), null);   
                        dblNroGarantia = Math.Round(dblImporte / obj.CargoFijo, 0);
                    }

                    objGarantia.nroGarantia = dblNroGarantia;
                    objGarantia.importe = Funciones.CheckDbl(dblImporte, 2);
                }

                dblImporteTotal += objGarantia.importe;
                objListaGarantia.Add(objGarantia);

                //INI: PROY-140335 RF1
                objVistaEvaluacion.flagEjecucionConsultaPrevia = "NO";

                if (obj.ejecucionConsultaPrevia == "SI") {
                    objVistaEvaluacion.flagEjecucionConsultaPrevia = "SI";
                }
                //FIN: PROY-140335 RF1
            }

            objVistaEvaluacion.oGarantia = new List<BEGarantia>(objListaGarantia);

            string strCostoInstalacion = ConfigurationManager.AppSettings["constMsjSinCostoInstalacion"].ToString();
            objVistaEvaluacion.oOfrecimiento = new List<BEOfrecimiento>();

            foreach (BEOfrecimiento obj in objOfrecimiento)
            {
                if (obj.TieneAutonomia != "SIN_REGLAS")
                {
                    objVistaEvaluacion.riesgoClaro = obj.RiesgoEnClaro;
                    objVistaEvaluacion.comportamientoPago = obj.ComportamientoConsolidado.ToString();
                    objVistaEvaluacion.exoneraRA = "NO";
                    objVistaEvaluacion.tipoGarantia = obj.TipoDeGarantia;
                    objVistaEvaluacion.importeGarantia = dblImporteTotal;

                    if (obj.ProcesoDeExoneracionDeRentas == "SI")
                        objVistaEvaluacion.exoneraRA = obj.ProcesoDeExoneracionDeRentas;
                }
                objVistaEvaluacion.cargoFijo += obj.CargoFijo; 
                strCostoInstalacion = obj.CostoDeInstalacion.ToString();

                //PROY-29215 INICIO
                StringBuilder builder  = new StringBuilder();
                StringBuilder builderpago = new StringBuilder();
                
                if (obj.ListaFormaPago != null)
                {

                    foreach (string objLFormaPago in obj.ListaFormaPago)
                    {
                        builder.Append(objLFormaPago).Append("|");
                    }
                    string listaFormaPago = builder.ToString();
                    objVistaEvaluacion.formaPago = listaFormaPago;
                }
                
                if (obj.ListarCuotasPago != null)
                {
                    foreach (int objLCuotasPago in obj.ListarCuotasPago)
                    {                       
                        builderpago.Append(objLCuotasPago).Append("|");
                    }
                    string listaCuota = builderpago.ToString();
                    objVistaEvaluacion.nrocuota = listaCuota;
                }
                //PROY-29215 FIN 

                if (objVistaEvaluacion.planAutonomia == null) objVistaEvaluacion.planAutonomia = string.Format("{0};{1};{2}", obj.IdFila, obj.TieneAutonomia, strCostoInstalacion);
                else objVistaEvaluacion.planAutonomia = string.Format("{0}|{1};{2};{3}", objVistaEvaluacion.planAutonomia, obj.IdFila, obj.TieneAutonomia, strCostoInstalacion);

                objVistaEvaluacion.oOfrecimiento.Add(obj);
            }

            // PROY 30748 INI
            foreach (BEOfrecimiento obj in objOfrecimiento)
            {
                objVistaEvaluacion.ListProac = obj.ListaPlanProac;
                objVistaEvaluacion.NroCuotaProac = obj.NroCuotaProac;
                objVistaEvaluacion.cantidadDePlanesPorProducto = obj.nroLineaActivoxProducto;
                objVistaEvaluacion.creditScore = obj.creditScore;
                objVistaEvaluacion.montoAnticipadoInstalacion = obj.MontoAnticipadoInstalacion; //PROY-140546 Cobro Anticipado de Instalacion
                objVistaEvaluacion.tipoCobroAnticipadoInstalacion = obj.TipoCobroAnticipadoInstalacion; //PROY-140546 Cobro Anticipado de Instalacion
            }
            // PROY 30748 FIN 
            return objVistaEvaluacion;
        }

        private int CalcularNroPlanesxProducto(List<BEOfertaBRMS> objLista, List<BEBilletera> objBilleteraActual)
        {
            int nroPlanes = objBilleteraActual.Count;
            foreach (BEBilletera obj in objBilleteraActual)
            {
                foreach (BEOfertaBRMS obj1 in objLista)
                {
                    foreach (BEBilletera obj2 in obj1.oBilletera)
                    {
                        if (obj.idBilletera == obj2.idBilletera) nroPlanes++;
                    }
                }
            }
            return nroPlanes;
        }

        private int CalcularNroPlanesActivoxProducto(List<BEPlanBilletera> objLista, List<BEBilletera> objBilleteraActual)
        {
            int nroPlanes = 0;
            if (objLista != null)
            {
                foreach (BEBilletera obj in objBilleteraActual)
                {
                    foreach (BEPlanBilletera obj1 in objLista)
                    {
                        foreach (BEBilletera obj2 in obj1.oBilletera)
                        {
                            if (obj.idBilletera == obj2.idBilletera)
                                nroPlanes += obj1.nroPlanes;
                        }
                    }
                }
            }
            return nroPlanes;
        }

        private double CalcularMontoxProducto(List<BEBilletera> objLista, List<BEBilletera> objBilleteraActual)
        {
            double dblMonto = 0.0;
            if (objLista != null)
            {
                foreach (BEBilletera obj in objLista)
                {
                    foreach (BEBilletera obj1 in objBilleteraActual)
                    {
                        if (obj.idBilletera == obj1.idBilletera)
                        {
                            dblMonto += obj.monto;
                            break;
                        }
                    }
                }
            }
            return dblMonto;
        }

        private double CalcularCFEvaluado(List<BEOfertaBRMS> objListaEvaluado, List<BEBilletera> objBilleteraActual)
        {
            double dblCF = 0.0;
            foreach (BEBilletera obj in objBilleteraActual)
            {
                foreach (BEOfertaBRMS obj1 in objListaEvaluado)
                {
                    foreach (BEBilletera obj2 in obj1.oBilletera)
                    {
                        if (obj.idBilletera == obj2.idBilletera)
                        {
                            dblCF += obj1.cargoFijo;
                            break;
                        }
                    }
                }
            }
            return dblCF;
        }

        private WS.WSReglasCrediticia.tipoDeDocumento ObtenerTipoDocumento(string strTipoDoc, string strNroDoc)
        {
            if (strTipoDoc == ConfigurationManager.AppSettings["constCodTipoDocumentoDNI"].ToString())
                return WS.WSReglasCrediticia.tipoDeDocumento.DNI;

            if (strTipoDoc == ConfigurationManager.AppSettings["constCodTipoDocumentoCEX"].ToString())
                return WS.WSReglasCrediticia.tipoDeDocumento.CE;

            if (strTipoDoc == ConfigurationManager.AppSettings["constCodTipoDocumentoRUC"].ToString())
            {
                if (strNroDoc.Substring(0, 1) == "1") return WS.WSReglasCrediticia.tipoDeDocumento.RUC10;
                else return WS.WSReglasCrediticia.tipoDeDocumento.RUC20;
            }
            //INI PROY-31636
            if (strTipoDoc == ReadKeySettings.Key_codigoDocPasaporte07)
                return WS.WSReglasCrediticia.tipoDeDocumento.PASAPORTE;
            if (strTipoDoc == ReadKeySettings.Key_codigoDocCIRE)
                return WS.WSReglasCrediticia.tipoDeDocumento.CIRE;
            if (strTipoDoc == ReadKeySettings.Key_codigoDocCIE)
                return WS.WSReglasCrediticia.tipoDeDocumento.CIE;
            if (strTipoDoc == ReadKeySettings.Key_codigoDocCPP)
                return WS.WSReglasCrediticia.tipoDeDocumento.CPP;
            if (strTipoDoc == ReadKeySettings.Key_codigoDocCTM)
                return WS.WSReglasCrediticia.tipoDeDocumento.CTM;
            //FIN PROY-31636
            return WS.WSReglasCrediticia.tipoDeDocumento.DNI;
        }

        private void grabarLog(WS.WSReglasCrediticia.ClaroEvalClientesReglasRequest oEvaluacion, ref BEOfrecimiento oOfrecimiento)
        {
            string strSolicitud = String.Empty;
            string strCliente = String.Empty;
            string strDirCliente = String.Empty;
            string strDocCliente = String.Empty;
            string strOferta = String.Empty;
            string strCampana = String.Empty;
            string strPlan = String.Empty;
            string strOficina = String.Empty;
            string strDirOficina = String.Empty;
            string strRepLegal = String.Empty;
            string strServicio = String.Empty;
            string strEquipo = String.Empty;
            string strOfrecimiento = String.Empty;
            string strCampanaOfrecimiento = String.Empty; //PROY-140439 BRMS CAMPA�A NAVIDE�A

            WS.WSReglasCrediticia.solicitud1 oSolicitud;
            WS.WSReglasCrediticia.cliente oCliente;
            WS.WSReglasCrediticia.oferta oOferta;

            oSolicitud = oEvaluacion.solicitud.solicitud1;
            oCliente = oEvaluacion.solicitud.solicitud1.cliente;
            oOferta = oEvaluacion.solicitud.solicitud1.oferta;

            // Informaci�n Solicitud --------------------------------------------
            strSolicitud += oSolicitud.cargoFijoDeBolsa + "|";
            strSolicitud += oSolicitud.flagDeLicitacion.ToString() + "|";
            strSolicitud += oSolicitud.tipoDeBolsa + "|";
            strSolicitud += oSolicitud.tipoDeDespacho + "|";
            strSolicitud += oSolicitud.tipoDeOperacion + "|";
            strSolicitud += oSolicitud.transaccion.ToString() + "|"; //PROY-24724-IDEA-28174 - INICIO
            strSolicitud += oSolicitud.fechaEjecucion.ToString() + "|";
            strSolicitud += oSolicitud.horaEjecucion.ToString(); //PROY-24724-IDEA-28174 - FIN
//gaa20170215
            strSolicitud += "|" + Funciones.CheckStr(oSolicitud.buroConsultado);
//gaa20170215
            // Informaci�n Cliente ----------------------------------------------
            strCliente += oCliente.cantidadDeLineasActivas + "|";
            strCliente += oCliente.cantidadDePlanesPorProducto + "|";
            strCliente += oCliente.comportamientoDePago + "|";
            strCliente += oCliente.creditScore + "|";
            strCliente += oCliente.edad + "|";
            strCliente += oCliente.facturacionPromedioClaro + "|";
            strCliente += oCliente.facturacionPromedioProducto + "|";
            strCliente += oCliente.limiteDeCreditoDisponible + "|";
            strCliente += oCliente.riesgo + "|";
            strCliente += oCliente.sexo + "|";
            strCliente += oCliente.tiempoDePermanencia + "|";
            strCliente += oCliente.tipo + "|";
            //PROY-140230-MAS-INI
            strCliente += oCliente.deuda + "|";//PROY-29121
            //PROY-32439 MAS
            strCliente += oCliente.segmento + "|";
            //PROY-32439 MAS
            _objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140230 - SEGMENTO DEL CLIENTE][grabarLog][INI]", ""), null, null);
            _objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140230 - SEGMENTO DEL CLIENTE][grabarLog][tipoDocumento]", oCliente.segmento), null, null);
            _objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140230 - SEGMENTO DEL CLIENTE][grabarLog][FIN]", ""), null, null);
            //PROY-140230-MAS-FIN
            //PROY-32439 MAS INI 7campos al brms.
            ValidacionDeudaBRMS objVariablesEntradaNVoBRMS = new ValidacionDeudaBRMS();
            objVariablesEntradaNVoBRMS = (ValidacionDeudaBRMS)HttpContext.Current.Session["ObjNvoBRMS"];
            strCliente += objVariablesEntradaNVoBRMS.request.cliente.montoDeudaVencida + "|";
            strCliente += objVariablesEntradaNVoBRMS.request.cliente.montoDeudaCastigada + "|";
            strCliente += objVariablesEntradaNVoBRMS.request.cliente.disputa.monto + "|";
            strCliente += objVariablesEntradaNVoBRMS.request.cliente.disputa.cantidad + "|";
            strCliente += objVariablesEntradaNVoBRMS.request.cliente.disputa.antiguedad + "|";
            strCliente += objVariablesEntradaNVoBRMS.request.cliente.montoDeuda + "|";
            strCliente += objVariablesEntradaNVoBRMS.request.cliente.antiguedadDeuda + "|"; //PROY-31948
            //PROY-32439 MAS FIN
            //PROY-31948 INI
            strCliente += oCliente.montoPendienteCuotasSistema + "|";//Monto pendiente en cuotas en sistema
            strCliente += oCliente.cantidadPlanesCuotasPendientesSistema + "|";//Cantidad de planes con cuotas pendientes en sistema
            strCliente += oCliente.cantidadMaximaCuotasPendientesSistema + "|";//Cantidad m�xima de cuotas pendientes en sistema
            strCliente += oCliente.montoPendienteCuotasUltimasVentas + "|";//Monto pendiente en cuotas �ltimas ventas
            strCliente += oCliente.cantidadPlanesCuotasPendientesUltimasVentas + "|";//Cantidad de planes con cuotas pendientes �ltimas ventas
            strCliente += oCliente.cantidadMaximaCuotasPendientesUltimasVentas;//cantidad m�xima de cuotas pendientes           
            
            //PROY-31948 FIN
            /*PROY 32438-AGREGAR NUEVAS VARIABLES DEL BURO-BRMS*/
            _objLog.CrearArchivolog("- INI: PARAMETROS DE SALIDA PROY-32438" + null, null, null);
            var flagApagado = HttpContext.Current.Session["FlagApagado32438"] ?? ""; //PROY-32438 "Flag Apagado/Encendido"
            _objLog.CrearArchivolog("- GRABARLOG - flagApagado -> " + flagApagado, null, null);
            if (flagApagado.ToString().Equals("0")) //PROY-32438 "Flag Apagado/Encendido" INI
            {

                if (oCliente.contribuyente != null)
                {
                    _objLog.CrearArchivolog("- GRABARLOG - oCliente.contribuyente -> " + oCliente.contribuyente, null, null);
                    strCliente += "|" + oCliente.contribuyente.tipo + "|";
                    strCliente += oCliente.contribuyente.nombreComercial + "|";
                    strCliente += oCliente.contribuyente.fechaInicioActividades.ToShortDateString() + "|";
                    strCliente += oCliente.contribuyente.estado + "|";
                    strCliente += oCliente.contribuyente.condicion + "|";
                    strCliente += HttpContext.Current.Session["CodContribuyente"] + " - " + oCliente.contribuyente.CIU + "|";
                    strCliente += oCliente.contribuyente.cantidadTrabajadores + "|";
                    strCliente += oCliente.contribuyente.autorizacionImpresion + "|";
                    strCliente += oCliente.contribuyente.sistemaEmisionElectronica + "|";
                    strCliente += oCliente.contribuyente.cantidadMesesInicioActividades;
                    _objLog.CrearArchivolog("- GRABARLOG - strCliente -> " + strCliente, null, null);
                }
                else
                {
                    _objLog.CrearArchivolog("- GRABARLOG - oCliente.contribuyente -> " + "DATOS VACIOS", null, null);
                    strCliente += "|" + "" + "|";
                    strCliente += "" + "|";
                    strCliente += "" + "|";
                    strCliente += "" + "|";
                    strCliente += "" + "|";
                    strCliente += "" + "|";
                    strCliente += "" + "|";
                    strCliente += "" + "|";
                    strCliente += "" + "|";
                    strCliente += "";
                    _objLog.CrearArchivolog("- GRABARLOG - strCliente -> " + strCliente, null, null);
                }
            }
            else {
                _objLog.CrearArchivolog("- GRABARLOG - flagApagado DISTINTO 0 - oCliente.contribuyente -> " + "DATOS VACIOS", null, null);
                strCliente += "|" + "" + "|";
                strCliente += "" + "|";
                strCliente += "" + "|";
                strCliente += "" + "|";
                strCliente += "" + "|";
                strCliente += "" + "|";
                strCliente += "" + "|";
                strCliente += "" + "|";
                strCliente += "" + "|";
                strCliente += "";
                _objLog.CrearArchivolog("- GRABARLOG - strCliente -> " + strCliente, null, null);
            }
            //PROY-32438 "Flag Apagado/Encendido" FIN
            ////PROY-140579 0412 NN INI
            strCliente += "|" + oCliente.flagWhitelist;
            _objLog.CrearArchivolog("BLReglaCrediticia - grabarLog - oCliente.flagWhitelist-> " + oCliente.flagWhitelist, null, null);
            ////PROY-140579 0412 NN FIN
            _objLog.CrearArchivolog("- FIN: PARAMETROS DE SALIDA PROY-32438" + null, null, null);
            /*FIN 32438-AGREGAR NUEVAS VARIABLES DEL BURO-BRMS*/
            // Informaci�n Direcci�n Cliente ---------------------------------------

            #region [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo m�vil] | [Grabar variables en los log's]
            _objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140743 - Venta en cuotas accesorios - SEGMENTO DEL CLIENTE][grabarLog][INI]", ""), null, null);
            bool strMensajeBrms = (bool)HttpContext.Current.Session["strRespuestaBRMS"];
            if (strMensajeBrms)
            {
                strCliente += string.Format("|{0}|", Funciones.CheckStr(oCliente.montoCuotasPendientesAcc));//39
                strCliente += string.Format("{0}|", Funciones.CheckStr(oCliente.cantidadLineaCuotasPendientesAcc));//40
                strCliente += string.Format("{0}|", Funciones.CheckStr(oCliente.cantidadMaximaCuotasPendientesAcc));//41
                strCliente += string.Format("{0}|", Funciones.CheckStr(oCliente.montoCuotasPendientesAccUltiVenta));//42
                strCliente += string.Format("{0}|", Funciones.CheckStr(oCliente.cantidadLineaCuotasPendientesAccUltiVenta));//43
                strCliente += string.Format("{0}", Funciones.CheckStr(oCliente.cantidadMaximaCuotasPendientesAccUltiVenta));//44
            }
            else
            {
                strCliente += string.Format("|{0}|", "");
                strCliente += string.Format("{0}|", "");
                strCliente += string.Format("{0}|", "");
                strCliente += string.Format("{0}|", "");
                strCliente += string.Format("{0}|", "");
                strCliente += string.Format("{0}", "");
            }
            _objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140743 - Venta en cuotas accesorios - SEGMENTO DEL CLIENTE][strCliente]", strCliente.ToString()), null, null);
            _objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140743 - Venta en cuotas accesorios - SEGMENTO DEL CLIENTE][grabarLog][FIN]", ""), null, null);
            #endregion

            if (oCliente.direccion != null)
            {
                strDirCliente += oCliente.direccion.codigoDePlano + "|";
                strDirCliente += oCliente.direccion.departamento + "|";
                strDirCliente += oCliente.direccion.distrito + "|";
                strDirCliente += oCliente.direccion.provincia + "|";
                strDirCliente += oCliente.direccion.region;
            }

            // Informaci�n Documento Cliente ---------------------------------------
            strDocCliente += oCliente.documento.descripcion.ToString() + "|";
            strDocCliente += oCliente.documento.numero;

            // Informaci�n Representante Cliente -----------------------------------
            if (oSolicitud.cliente.representanteLegal != null)
            {
                int _rrll = oSolicitud.cliente.representanteLegal.Count();
                foreach (WS.WSReglasCrediticia.representanteLegal rrll in oSolicitud.cliente.representanteLegal)
                {
                    strRepLegal += rrll.documento.descripcion.ToString() + "|";
                    strRepLegal += rrll.documento.numero.ToString() + "|";
                    strRepLegal += rrll.riesgo + "|";
                    /*PROY-32438 INI*/

                    if (flagApagado.ToString().Equals("0")) //PROY-32438 "Flag Apagado/Encendido" INI
                    {
                        strRepLegal += Funciones.CheckStr(rrll.cargo + "|").ToString();//prueba
                        if (rrll.fechaNombramiento != DateTime.Parse("01/01/0001"))
                        {
                        strRepLegal += rrll.fechaNombramiento.ToShortDateString() + "|";
                            strRepLegal += rrll.cantidadMesesNombramiento;
                        }
                        else
                        {
                            strRepLegal += "|";
                        }
                        if (_rrll > 1)
                        {
                            strRepLegal += "_";
                        }
                        _rrll--;
                    } //PROY-32438 "Flag Apagado/Encendido" FIN

                    /*PROY-32438 FIN*/
                   
                }
            }

            // Informaci�n Servicios Cliente --------------------------------------
            if (oOferta.planSolicitado.servicio != null)
            {
                foreach (WS.WSReglasCrediticia.servicio oServicio in oOferta.planSolicitado.servicio)
                {
                    strServicio += oServicio.nombre + "_";
                }
            }

            // Informaci�n Equipos Cliente ----------------------------------------
            if (oSolicitud.equipo != null)
            {
                foreach (WS.WSReglasCrediticia.equipo oEquipo in oSolicitud.equipo)
                {
                    strEquipo += oEquipo.costo + "|";
                    strEquipo += oEquipo.cuotas + "|";
                    strEquipo += oEquipo.formaDePago + "|";
                    strEquipo += oEquipo.gama + "|";
                    strEquipo += oEquipo.modelo + "|";
                    strEquipo += oEquipo.montoDeCuota + "|";
                    strEquipo += oEquipo.porcentajecuotaInicial + "|";
                    strEquipo += oEquipo.precioDeVenta + "|";
                    strEquipo += oEquipo.tipoDeDeco + "|";
                    strEquipo += oEquipo.tipoOperacionKit + "|"; //PROY-140579 :: INI - RF02
                    strEquipo += Funciones.CheckDbl(oEquipo.montoDeCuotaComercial) + "|";
                    strEquipo += Funciones.CheckDbl(oEquipo.montoDeCuotaInicialComercial) + "_"; //PROY-140579 :: FIN - RF02
                    _objLog.CrearArchivolog("- PROY-140579 montoDeCuotaComercial-> " + Funciones.CheckDbl(oEquipo.montoDeCuotaComercial), null, null);//PROY-140579
                    _objLog.CrearArchivolog("- PROY-140579 montoDeCuotaInicialComercial-> " + Funciones.CheckDbl(oEquipo.montoDeCuotaInicialComercial), null, null);//PROY-140579
                }
            }

            // Informaci�n Oferta --------------------------------------------------
            strOferta += oOferta.cantidadDeDecos + "|";
            strOferta += oOferta.casoEpecial + "|";
            strOferta += oOferta.controlDeConsumo + "|";
            strOferta += oOferta.kitDeInstalacion + "|";
            strOferta += oOferta.plazoDeAcuerdo + "|";
            strOferta += oOferta.productoComercial + "|";
            strOferta += oOferta.segmentoDeOferta + "|";
            strOferta += oOferta.tipoDeOperacionEmpresa + "|";
            strOferta += oOferta.tipoDeProducto + "|";
            strOferta += oOferta.modalidadCedente + "|";
            strOferta += oOferta.operadorCedente + "|";
            strOferta += oOferta.combo + "|"; //PROY-24724-IDEA-28174 - INICIO
            strOferta += oOferta.proteccionMovil+ "|"; //PROY-24724-IDEA-28174 - FIN
            strOferta += oOferta.mesOperadorCedente;
//gaa20170215
            strOferta += "|" + Funciones.CheckStr(oOferta.cantidadLineasSEC) + '|';
            //PROY-140335 RF1 INI
            strOferta += Funciones.CheckStr(oOferta.montoCFSEC) + '|'; 
            var flagConsultaPrevia = string.Empty;
            if (oSolicitud.tipoDeOperacion == ConfigurationManager.AppSettings["consDescPortabilidad"].ToString() && oSolicitud.transaccion == ConfigurationManager.AppSettings["constTrxEvaluacion"].ToString())
            {
                if (oOferta.tipoDeProducto == "MOVIL" || oOferta.tipoDeProducto == "BAM")
            {
                flagConsultaPrevia =oOferta.flagConsultaPrevia.ToString();
            }
            }
            strOferta += Funciones.CheckStr(flagConsultaPrevia);
            //PROY-140335 RF1 FIN
//gaa20170215
            #region [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo m�vil] | [Grabar variables en los log's]
            bool strMensajeBrms2 = (bool)HttpContext.Current.Session["strRespuestaBRMS"];
            if (strMensajeBrms2)
            {
                strOferta += string.Format("|{0}|", Funciones.CheckStr(oOferta.promociones));//17
                strOferta += string.Format("{0}", Funciones.CheckStr(oOferta.productoCuentaAFacturar));//18
            }
            else
            {
                strOferta += string.Format("|{0}|", "");
                strOferta += string.Format("{0}", "");
            }
            #endregion
            // Informaci�n Plan Solicitado -------------------------------------------
            strPlan += oOferta.planSolicitado.cargoFijo + "|";
            strPlan += oOferta.planSolicitado.descripcion + "|";
            strPlan += oOferta.planSolicitado.paquete + "|";

            // Informaci�n Campa�a ---------------------------------------------------
            //PROY-140439 BRMS CAMPA�A NAVIDE�A::INI
            if (!string.IsNullOrEmpty(oOferta.campana.tipo)) {
            strCampana += oOferta.campana.grupo + "|";
            strCampana += oOferta.campana.tipo;
            }
            //PROY-140439 BRMS CAMPA�A NAVIDE�A::FIN

            // Informaci�n Punto de Venta --------------------------------------------
            strOficina += oSolicitud.puntodeVenta.calidadDeVendedor + "|";
            strOficina += oSolicitud.puntodeVenta.canal + "|";
            strOficina += oSolicitud.puntodeVenta.codigo + "|";
            strOficina += oSolicitud.puntodeVenta.descripcion + "|";

            // Informaci�n Direcci�n Punto de Venta ----------------------------------
            strDirOficina += oSolicitud.puntodeVenta.direccion.codigoDePlano + "|";
            strDirOficina += oSolicitud.puntodeVenta.direccion.departamento + "|";
            strDirOficina += oSolicitud.puntodeVenta.direccion.distrito + "|";
            strDirOficina += oSolicitud.puntodeVenta.direccion.provincia + "|";
            strDirOficina += oSolicitud.puntodeVenta.direccion.region;

            // PROY-140439 BRMS CAMPA�A NAVIDE�A::INI
            // Lista de Campa�as ofrecimiento ---------------------------------------------------
            if (oOfrecimiento.CampanasNavidad != null)
            {
                if (oOfrecimiento.CampanasNavidad.Length > 0)
                {
                    StringBuilder campanas = new StringBuilder();
                    foreach (var item in oOfrecimiento.CampanasNavidad)
                    {
                        campanas.AppendFormat("{0}{1}", item, ";");
                    }
                    string campanas2 = Funciones.CheckStr(campanas);
                    campanas2 = campanas2.Substring(0, campanas2.Length - 1);
                    strCampanaOfrecimiento = Funciones.CheckStr(campanas2);
                }
            }
            // PROY-140439 BRMS CAMPA�A NAVIDE�A::FIN

            oOfrecimiento.In_solicitud = strSolicitud;
            oOfrecimiento.In_cliente = strCliente;
            oOfrecimiento.In_direccion_cliente = strDirCliente;
            oOfrecimiento.In_doc_cliente = strDocCliente;
            oOfrecimiento.In_rrll_cliente = strRepLegal;
            oOfrecimiento.In_equipo = strEquipo;
            oOfrecimiento.In_oferta = strOferta;
            oOfrecimiento.In_campana = strCampana;
            // oOfrecimiento.In_plan_actual
            oOfrecimiento.In_plan_solicitado = strPlan;
            oOfrecimiento.In_servicio = strServicio;
            oOfrecimiento.In_pdv = strOficina;
            oOfrecimiento.In_direccion_pdv = strDirOficina;
            oOfrecimiento.Lista_ofrecimientocampanas = strCampanaOfrecimiento; //PROY-140439 BRMS CAMPA�A NAVIDE�A
        }

        //PROY-140439 BRMS CAMPA�A NAVIDE�A::INI
        public BEVistaEvaluacion ValidacionCampana(BEClienteCuenta objCliente, List<BEDireccionCliente> objDireccion, string strDatos, string strTieneProteccionMovil, string CodTipoProductoActual, BECuota objCuotaOAC, BECuota objCuotaPVU, string prodFacturar) //PROY-140743
        {
            BEVistaEvaluacion objVistaEvaluacion = null;
            List<BEOfrecimiento> objListaOfrecimiento = new List<BEOfrecimiento>();

            // Consulta BRMS
            ConsultarReglasCampana(objCliente, objDireccion, "V", strDatos, objCuotaOAC, objCuotaPVU, strTieneProteccionMovil, CodTipoProductoActual, Funciones.CheckStr(prodFacturar), ref objListaOfrecimiento);//PROY-140743

            objVistaEvaluacion = ResultadoOfrecimiento(objListaOfrecimiento);

            return objVistaEvaluacion;
        }

        public void ConsultarReglasCampana(BEClienteCuenta objCliente, List<BEDireccionCliente> objDireccion, string strFlgCuota, string strDatos, BECuota objCuotaOAC, BECuota objCuotaPVU, string strTieneProteccionMovil, string CodTipoProductoActual, string prodFacturar, ref List<BEOfrecimiento> objListaOfrecimiento) //PROY-140743
        {
            try
            {
                string strTipoDoc = objCliente.tipoDoc;
                string nroDocumento = objCliente.nroDoc;
                _objLog = new GeneradorLog(null, nroDocumento, null, consPaginaEvaluacion);
                _objLog.CrearArchivolog("[INICIO][ConsultarReglasCampana]", null, null);
                _objLog.CrearArchivolog(string.Format("{0} => {1}", "[ConsultarReglasCampana][objCliente]", objCliente), null, null);
                _objLog.CrearArchivolog(string.Format("{0} => {1}", "[ConsultarReglasCampana][strFlgCuota]", strFlgCuota), null, null);
                _objLog.CrearArchivolog(string.Format("{0} => {1}", "[ConsultarReglasCampana][strDatos]", strDatos), null, null);
                _objLog.CrearArchivolog(string.Format("{0} => {1}", "[ConsultarReglasCampana][objCuotaOAC]", objCuotaOAC), null, null);
                _objLog.CrearArchivolog(string.Format("{0} => {1}", "[ConsultarReglasCampana][objCuotaPVU]", objCuotaPVU), null, null);
                _objLog.CrearArchivolog(string.Format("{0} => {1}", "[ConsultarReglasCampana][strTieneProteccionMovil]", strTieneProteccionMovil), null, null);
                _objLog.CrearArchivolog(string.Format("{0} => {1}", "[ConsultarReglasCampana][CodTipoProductoActual]", CodTipoProductoActual), null, null);

                string[] arrDatosGeneral = strDatos.Split('|');
                string strTipoOperacion = arrDatosGeneral[0];
                string strOferta = arrDatosGeneral[1];
                string strCasoEspecial = arrDatosGeneral[2];
                string strTipoModalidad = arrDatosGeneral[3].ToUpper();
                string strOperadorCedente = arrDatosGeneral[4].ToUpper();
                string strModalidadVenta = arrDatosGeneral[5].ToUpper();
                string strCombo = arrDatosGeneral[6];
                string strComboTexto = arrDatosGeneral[7].ToUpper();

                if (strOferta == "SELECCIONE...") strOferta = string.Empty;

                BEOfrecimiento objOfrecimiento = null;
                objListaOfrecimiento = new List<BEOfrecimiento>();

                _objLog.CrearArchivolog("[ConsultarReglasCampana][Inicio Obtener datosGeneralEvaluacion]", null, null);

                //Consulta Datos Evaluaci�n BRMS
                WS.WSReglasCrediticia.ClaroEvalClientesReglasRequest oEvaluacionCliente = datosGeneralEvaluacion(objCliente);
                WS.WSReglasCrediticia.oferta oOferta;
                WS.WSReglasCrediticia.solicitud1 oSolicitud1;
                WS.WSReglasCrediticia.planActual oPlanActual = new WSReglasCrediticia.planActual();

                _objLog.CrearArchivolog("[ConsultarReglasCampana][Fin Obtener datosGeneralEvaluacion]", null, null);

                oSolicitud1 = oEvaluacionCliente.solicitud.solicitud1;

                _objLog.CrearArchivolog("[ConsultarReglasCampana][Inicio Obtener objVariablesEntradaNVoBRMS]", null, null);
                //Ini-Cliente
                ValidacionDeudaBRMS objVariablesEntradaNVoBRMS = new ValidacionDeudaBRMS();
                objVariablesEntradaNVoBRMS = (ValidacionDeudaBRMS)HttpContext.Current.Session["ObjNvoBRMS"];
                oSolicitud1.cliente.antiguedadDeuda = objVariablesEntradaNVoBRMS.request.cliente.antiguedadDeuda;
                oSolicitud1.cliente.antiguedadMontoDisputa = objVariablesEntradaNVoBRMS.request.cliente.disputa.antiguedad;
                oSolicitud1.cliente.montoDeudaVencida = objVariablesEntradaNVoBRMS.request.cliente.montoDeudaVencida;
                oSolicitud1.cliente.montoDeudaCastigada = objVariablesEntradaNVoBRMS.request.cliente.montoDeudaCastigada;
                oSolicitud1.cliente.montoDisputa = objVariablesEntradaNVoBRMS.request.cliente.disputa.monto;
                oSolicitud1.cliente.cantidadMontoDisputa = objVariablesEntradaNVoBRMS.request.cliente.disputa.cantidad;
                oSolicitud1.cliente.montoTotalDeuda = objVariablesEntradaNVoBRMS.request.cliente.montoDeuda;
                oSolicitud1.cliente.montoPendienteCuotasSistema = objCuotaOAC.montoPendienteCuotasSistema;
                oSolicitud1.cliente.cantidadPlanesCuotasPendientesSistema = objCuotaOAC.cantidadPlanesCuotasPendientesSistema;
                oSolicitud1.cliente.cantidadMaximaCuotasPendientesSistema = objCuotaOAC.cantidadMaximaCuotasPendientesSistema;
                oSolicitud1.cliente.montoPendienteCuotasUltimasVentas = objCuotaPVU.montoPendienteCuotasUltimasVentas;
                oSolicitud1.cliente.cantidadPlanesCuotasPendientesUltimasVentas = objCuotaPVU.cantidadPlanesCuotasPendientesUltimasVentas;
                oSolicitud1.cliente.cantidadMaximaCuotasPendientesUltimasVentas = objCuotaPVU.cantidadMaximaCuotasPendientesUltimasVentas;
                oSolicitud1.cliente.deuda = objCliente.deudaCliente;

                _objLog.CrearArchivolog("[ConsultarReglasCampana][Fin Obtener objVariablesEntradaNVoBRMS]", null, null);

                int cantplanes = 0;
                double montofac = 0;
                double montoNOfac = 0;
                double LCDisponible = 0;

                _objLog.CrearArchivolog("[ConsultarReglasCampana][Inicio Obtener cantidadDePlanesPorProducto]", null, null);
                if (objCliente.oPlanesActivosxBilletera != null)
                {
                foreach (var item in objCliente.oPlanesActivosxBilletera)
                {
                    if (item.idBilletera == 2)
                    {
                        cantplanes++;
                    }
                }
                }

                _objLog.CrearArchivolog("[ConsultarReglasCampana][Fin Obtener cantidadDePlanesPorProducto]", null, null);

                _objLog.CrearArchivolog("[ConsultarReglasCampana][Inicio Obtener facturacionPromedioProducto]", null, null);


                if (objCliente.oMontoFacturadoxBilletera != null && objCliente.oMontoNoFacturadoxBilletera != null)
                {
                foreach (var item in objCliente.oMontoFacturadoxBilletera)
                {
                    if (item.idBilletera == 2)
                    {
                        montofac = montofac + item.monto;
                    }
                }

                _objLog.CrearArchivolog("[ConsultarReglasCampana][Fin Obtener facturacionPromedioProducto]", null, null);

                _objLog.CrearArchivolog("[ConsultarReglasCampana][Inicio Obtener limiteDeCreditoDisponible]", null, null);

                foreach (var item in objCliente.oMontoNoFacturadoxBilletera)
                {
                    if (item.idBilletera == 2)
                    {
                        montoNOfac = montoNOfac + item.monto;
                    }
                }
                }

                if (objCliente.oLCDisponiblexBilletera != null)
                {
                foreach (var item in objCliente.oLCDisponiblexBilletera)
                {
                    if (item.idBilletera == 2)
                    {
                        LCDisponible = item.monto;
                        break;
                    }
                }
                }
                
                oSolicitud1.cliente.cantidadDePlanesPorProducto = cantplanes;
                oSolicitud1.cliente.facturacionPromedioProducto = Funciones.CheckDbl(montofac + montoNOfac, 2);
                oSolicitud1.cliente.limiteDeCreditoDisponible = LCDisponible;

                _objLog.CrearArchivolog("[ConsultarReglasCampana][Fin Obtener limiteDeCreditoDisponible]", null, null);

                /*PROY-140743 - INI*/
                double montoCuotasPendientesAcc = 0;
                int cantidadLineaCuotasPendientesAcc = 0;
                int cantidadMaximaCuotasPendientesAcc = 0;
                double montoCuotasPendientesAccUltiVenta = 0;
                int cantidadLineaCuotasPendientesAccUltiVenta = 0;
                int cantidadMaximaCuotasPendientesAccUltiVenta = 0;

                bool respuestaBR = ObtenerVariablesBRMS(Funciones.CheckStr(objCliente.nroDoc), strTipoOperacion, ref montoCuotasPendientesAcc, ref cantidadLineaCuotasPendientesAcc, ref cantidadMaximaCuotasPendientesAcc, ref montoCuotasPendientesAccUltiVenta, ref cantidadLineaCuotasPendientesAccUltiVenta, ref cantidadMaximaCuotasPendientesAccUltiVenta);

                if (respuestaBR)
                {
                    oSolicitud1.cliente.montoCuotasPendientesAcc = Funciones.CheckDbl(montoCuotasPendientesAcc);
                    oSolicitud1.cliente.cantidadLineaCuotasPendientesAcc = Funciones.CheckInt(cantidadLineaCuotasPendientesAcc);
                    oSolicitud1.cliente.cantidadMaximaCuotasPendientesAcc = Funciones.CheckInt(cantidadMaximaCuotasPendientesAcc);
                    oSolicitud1.cliente.montoCuotasPendientesAccUltiVenta = Funciones.CheckDbl(montoCuotasPendientesAccUltiVenta);
                    oSolicitud1.cliente.cantidadLineaCuotasPendientesAccUltiVenta = Funciones.CheckInt(cantidadLineaCuotasPendientesAccUltiVenta);
                    oSolicitud1.cliente.cantidadMaximaCuotasPendientesAccUltiVenta = Funciones.CheckInt(cantidadMaximaCuotasPendientesAccUltiVenta);
                }
                /*PROY-140743 - FIN*/

                //Fin-Cliente

                _objLog.CrearArchivolog("[ConsultarReglasCampana][Inicio Obtener oListaEquipo]", null, null);

                WS.WSReglasCrediticia.equipo[] oListaEquipo = new WS.WSReglasCrediticia.equipo[1];
                oListaEquipo[0] = new WSReglasCrediticia.equipo()
                {
                    costo = 0,
                    cuotas = 0,
                    factorDePagoInicial = 0,
                    factorDeSubsidio = 0,
                    formaDePago = strModalidadVenta,
                    gama = string.Empty,
                    modelo = string.Empty,
                    montoDeCuota = 0,
                    montoDeCuotaComercial = 0,
                    montoDeCuotaInicialComercial = 0,
                    porcentajecuotaInicial = 0,
                    precioDeVenta = 0,
                    riesgo = 0,
                    tipoDeDeco = string.Empty,
                    tipoOperacionKit = string.Empty,

                };

                oSolicitud1.equipo = oListaEquipo;

                _objLog.CrearArchivolog("[ConsultarReglasCampana][Fin Obtener oListaEquipo]", null, null);

                oSolicitud1.fechaEjecucion = DateTime.Now.Date;
                oSolicitud1.horaEjecucion = DateTime.Now.Hour;

                _objLog.CrearArchivolog("[ConsultarReglasCampana][Inicio Obtener Oferta]", null, null);
                //Ini-Oferta
                oOferta = new WS.WSReglasCrediticia.oferta();
                oOferta.campana = new WS.WSReglasCrediticia.campana();
                oOferta.campana.tipo = string.Empty;
                oOferta.cantidadLineasSEC = 0;
                oOferta.casoEpecial = (string.IsNullOrEmpty(strCasoEspecial)) ? "REGULAR" : strCasoEspecial;
                oOferta.combo = strComboTexto;
                oOferta.controlDeConsumo = string.Empty;
                oOferta.kitDeInstalacion = string.Empty;
                oOferta.mesOperadorCedente = -1; //Valor por default porque no hay consulta previa.
                oOferta.modalidadCedente = strTipoModalidad;
                oOferta.montoCFSEC = 0;
                oOferta.operadorCedente = strOperadorCedente;


                _objLog.CrearArchivolog("[ConsultarReglasCampana][Inicio Obtener Plan Actual]", null, null);
                //Plan Actual
                oPlanActual.cantidadCuotasPendientes = objCuotaOAC.cantidadCuotasPendientes;
                oPlanActual.montoPendienteCuotas = objCuotaOAC.montoPendienteCuotas;
                oOferta.planActual = oPlanActual;
                _objLog.CrearArchivolog("[ConsultarReglasCampana][Fin Obtener Plan Actual]", null, null);

                _objLog.CrearArchivolog("[ConsultarReglasCampana][Inicio Obtener Plan Solicitado]", null, null);
                //Plan Solicitado
                oOferta.planSolicitado = new WS.WSReglasCrediticia.planSolicitado();
                oOferta.planSolicitado.cargoFijo = 0;
                oOferta.planSolicitado.descripcion = string.Empty;
                oOferta.planSolicitado.paquete = string.Empty;
                _objLog.CrearArchivolog("[ConsultarReglasCampana][Fin Obtener Plan Solicitado]", null, null);

                oOferta.plazoDeAcuerdo = string.Empty;
                List<BEItemGenerico> objListaProducto = new BLGeneral().ListarProducto();
                foreach (var item in objListaProducto)
                {
                    if (item.Codigo == CodTipoProductoActual)
                    {
                        oOferta.productoComercial = item.Descripcion;
                        oOferta.tipoDeProducto = item.Descripcion;
                        break;
                    }
                }
                oOferta.segmentoDeOferta = strOferta;
                oOferta.tipoDeOperacionEmpresa = string.Empty;
                oOferta.proteccionMovil = strTieneProteccionMovil.Equals("SI") ? WS.WSReglasCrediticia.tipoSiNo.SI : WS.WSReglasCrediticia.tipoSiNo.NO;

                /*PROY-140743 - INI*/
                if (respuestaBR)
                {
                    oOferta.promociones = Funciones.CheckStr(HttpContext.Current.Session["strPromocionVV"]);
                    oOferta.productoCuentaAFacturar = Funciones.CheckStr(prodFacturar);
                }
                /*PROY-140743 - FIN*/

                oSolicitud1.oferta = oOferta;
                //Fin-Oferta
                _objLog.CrearArchivolog("[ConsultarReglasCampana][Fin Obtener Oferta]", null, null);

                oSolicitud1.tipoDeOperacion = strTipoOperacion;
                oSolicitud1.totalPlanes = objCliente.totalplanes;
                oSolicitud1.transaccion = ConfigurationManager.AppSettings["constTrxValidacionCamp"].ToString();
                _objLog.CrearArchivolog(string.Format("{0} => {1}", "[ConsultarReglasCampana][oSolicitud1.transaccion]", oSolicitud1.transaccion), null, null);

                _objLog.CrearArchivolog("[ConsultarReglasCampana][Inicio Metodo ConsultaReglaCrediticia]", null, null);
                // Consulta BRMS
                oEvaluacionCliente.DecisionID = "1";
                oEvaluacionCliente.solicitud.solicitud1 = oSolicitud1;
                objOfrecimiento = (new WS.BWReglasCreditica()).ConsultaReglaCrediticia(nroDocumento, Funciones.CheckStr(prodFacturar), oEvaluacionCliente, ref objMensaje);//PROY-140743
                objOfrecimiento.creditScore = oEvaluacionCliente.solicitud.solicitud1.cliente.creditScore;

                _objLog.CrearArchivolog("[ConsultarReglasCampana][Fin Metodo ConsultaReglaCrediticia]", null, null);

                _objLog.CrearArchivolog(string.Format("{0} => {1}", "[ConsultarReglasCampana][objOfrecimiento.CampanasNavidad]", Funciones.CheckStr(objOfrecimiento.CampanasNavidad)), null, null);

                objOfrecimiento.TipoDocCli = objCliente.tipoDoc;
                objOfrecimiento.NumDocCli = objCliente.nroDoc;
                // Grabar Log
                _objLog.CrearArchivolog("[INICIO][ConsultarReglasCampana][grabarLog]", null, null);
                grabarLog(oEvaluacionCliente, ref objOfrecimiento);
                _objLog.CrearArchivolog("[FIN][ConsultarReglasCampana][grabarLog]", null, null);
                _objLog.CrearArchivolog("[Consulta BRMS]", objOfrecimiento, null);

                objListaOfrecimiento.Add(objOfrecimiento);

            }
            catch (Exception ex)
            {
                _objLog = new GeneradorLog(null, objCliente.nroDoc, null, "WEB");
                _objLog.CrearArchivolog(string.Format("{0} => {1}|{2}", "[ERROR]", ex.Message, ex.StackTrace), null, null);
                throw ex;
            }
        }
        //PROY-140439 BRMS CAMPA�A NAVIDE�A::FIN

        #region [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo m�vil]
        public bool ObtenerVariablesBRMS(string nroDocumento, string tipoDeOperacion, ref double montoCuotasPendientesAcc, ref int cantidadLineaCuotasPendientesAcc, ref int cantidadMaximaCuotasPendientesAcc, ref double montoCuotasPendientesAccUltiVenta, ref int cantidadLineaCuotasPendientesAccUltiVenta, ref int cantidadMaximaCuotasPendientesAccUltiVenta)
        {
            bool respuesta = false;
            _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0}{1}{0}", "**********************************************", " INICIO - ObtenerVariablesBRMS "), null, null);
            try
            {
                if (Funciones.CheckStr(ReadKeySettings.Key_FlagGeneralVtaCuotas).Equals("1"))
                {
                    string datosCliente = (String)HttpContext.Current.Session["objClienteDI"];
                    string[] arrdatosCliente = datosCliente.Split(Convert.ToChar("|"));
                    string tipoTransaccionBRMS = arrdatosCliente[2].Trim();

                    //if (tipoTransaccionBRMS != "Evaluar Cliente" && tipoDeOperacion.Equals(Funciones.CheckStr(ReadKeySettings.Key_DesVentaVarias)))
                    if (tipoDeOperacion.Equals(Funciones.CheckStr(ReadKeySettings.Key_DesVentaVarias)))
                    {
                        respuesta = ObtenerVariablesBRMSVtaCuotas(nroDocumento, ref montoCuotasPendientesAcc, ref cantidadLineaCuotasPendientesAcc, ref cantidadMaximaCuotasPendientesAcc, ref montoCuotasPendientesAccUltiVenta, ref cantidadLineaCuotasPendientesAccUltiVenta, ref cantidadMaximaCuotasPendientesAccUltiVenta);
                    }
                }
            }
            catch (Exception ex)
            {
                respuesta = false;
                _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0}{1}{0}", "**********************************************", " ERROR - ObtenerVariablesBRMS "), null, null);
                _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0} ===> {1}", " [ObtenerVariablesBRMS] [Message] ", ex.Message), null, null);
                _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0} ===> {1}", " [ObtenerVariablesBRMS] [StackTrace] ", ex.StackTrace), null, null);
            }
            HttpContext.Current.Session["strRespuestaBRMS"] = respuesta;
            _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0} ===> {1}", " [ObtenerVariablesBRMS] [respuesta] ", respuesta), null, null);
            _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0}{1}{0}", "**********************************************", " FIN - ObtenerVariablesBRMS "), null, null);
            
            return respuesta;
        }

        public bool ObtenerVariablesBRMSVtaCuotas(string nroDocumento, ref double montoCuotasPendientesAcc, ref int cantidadLineaCuotasPendientesAcc, ref int cantidadMaximaCuotasPendientesAcc, ref double montoCuotasPendientesAccUltiVenta, ref int cantidadLineaCuotasPendientesAccUltiVenta, ref int cantidadMaximaCuotasPendientesAccUltiVenta)
        {
            _objLog.CrearArchivologWS("PROY-140743 - IDEA-141192 - Venta Cuotas: ", string.Format("{0}{1}{0}", "**********************************************", " INICIO - ObtenerVariablesBRMSVtaCuotas "), null, null);
            RestVentasCuotas rService = new RestVentasCuotas();
            ObtenerVariablesBRMSResponse objResponse = new ObtenerVariablesBRMSResponse();
            Dictionary<string, string> dcParameters = new Dictionary<string, string>();
            HttpContext.Current.Session["strMensajeErrorBRMS"] = string.Empty;
            bool respuesta = false;
            try
            {
                BEClienteCuenta objClienteConsulta = (BEClienteCuenta)HttpContext.Current.Session["objCliente" + nroDocumento];
                string strTipoDOC = Funciones.CheckStr(objClienteConsulta.tipoDoc);

                dcParameters.Add("tipoDoc", strTipoDOC);
                dcParameters.Add("numeroDoc", nroDocumento);
                dcParameters.Add("linea", string.Empty);
                dcParameters.Add("rangoHoras", ReadKeySettings.Key_RangoHoras);

                #region Datos Auditoria
                BEAuditoriaRequest objBeAuditoriaRequest = new BEAuditoriaRequest();
                objBeAuditoriaRequest.idTransaccion = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                objBeAuditoriaRequest.timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
                objBeAuditoriaRequest.userId = Funciones.CheckStr(CurrentUsuario);
                objBeAuditoriaRequest.msgid = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                objBeAuditoriaRequest.dataPower = true;
                objBeAuditoriaRequest.accept = "application/json";
                objBeAuditoriaRequest.urlTimeOut_Rest = Funciones.CheckStr(ConfigurationManager.AppSettings["ConsMejorasPorta_TimeOut"]);
                objBeAuditoriaRequest.wsIp = Funciones.CheckStr(ConfigurationManager.AppSettings["conswsipPedidosTienda"]);
                objBeAuditoriaRequest.ipTransaccion = Funciones.CheckStr(HttpContext.Current.Session["CurrentTerminal"]);
                objBeAuditoriaRequest.usuarioAplicacion = Funciones.CheckStr(CurrentUsuario);
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

                #region Response Servicio

                objResponse = rService.ObtenerVariablesBRMS(dcParameters, objBeAuditoriaRequest);

                string strCodRespuesta = Funciones.CheckStr(objResponse.MessageResponse.Body.variablesBRMSResponse.responseStatus.codigoRespuesta);
                string strMsjRespuesta = Funciones.CheckStr(objResponse.MessageResponse.Body.variablesBRMSResponse.responseStatus.mensajeRespuesta);

                _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0} ===> {1}", " [ObtenerVariablesBRMSVtaCuotas] [codigoRespuesta] ", strCodRespuesta), null, null);
                _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0} ===> {1}", " [ObtenerVariablesBRMSVtaCuotas] [mensajeRespuesta] ", strMsjRespuesta), null, null);

                if (strCodRespuesta.Equals("0"))
                {
                    respuesta = true;

                    montoCuotasPendientesAcc = Funciones.CheckDbl(objResponse.MessageResponse.Body.variablesBRMSResponse.responseData.montoPendCuotas);
                    cantidadLineaCuotasPendientesAcc = Funciones.CheckInt(objResponse.MessageResponse.Body.variablesBRMSResponse.responseData.cantPendCuotas); ;
                    cantidadMaximaCuotasPendientesAcc = Funciones.CheckInt(objResponse.MessageResponse.Body.variablesBRMSResponse.responseData.cantMaxPendCuotas);
                    montoCuotasPendientesAccUltiVenta = Funciones.CheckDbl(objResponse.MessageResponse.Body.variablesBRMSResponse.responseData.montoPendUltVentas);
                    cantidadLineaCuotasPendientesAccUltiVenta = Funciones.CheckInt(objResponse.MessageResponse.Body.variablesBRMSResponse.responseData.cantPendUltVentas);
                    cantidadMaximaCuotasPendientesAccUltiVenta = Funciones.CheckInt(objResponse.MessageResponse.Body.variablesBRMSResponse.responseData.cantMaxPendUltVentas);
                }
                else
                {
                    HttpContext.Current.Session["strMensajeErrorBRMS"] = Funciones.CheckStr(ReadKeySettings.Key_MsjNoVariablesBRMS);
                }
                #endregion
            }
            catch (Exception ex)
            {
                HttpContext.Current.Session["strMensajeErrorBRMS"] = Funciones.CheckStr(ReadKeySettings.Key_MsjNoVariablesBRMS);
                _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0}{1}{0}", "**********************************************", " ERROR - ObtenerVariablesBRMSVtaCuotas "), null, null);
                _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0} ===> {1}", " [ObtenerVariablesBRMSVtaCuotas] [Message] ", ex.Message), null, null);
                _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0} ===> {1}", " [ObtenerVariablesBRMSVtaCuotas] [StackTrace] ", ex.StackTrace), null, null);
            }
            _objLog.CrearArchivologWS("[PROY-140743 - IDEA-141192 - Venta Cuotas] ", string.Format("{0}{1}{0}", "**********************************************", " FIN - ObtenerVariablesBRMSVtaCuotas "), null, null);

            return respuesta;
        }

        static string CurrentUsuario
        {
            get
            {
                string strDomainUser = HttpContext.Current.Request.ServerVariables["LOGON_USER"];
                string strUser = strDomainUser.Substring(strDomainUser.IndexOf("\\", System.StringComparison.Ordinal) + 1);
                return strUser.ToUpper();
            }
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

        static string CurrentTerminal
        {
            get
            {
                string ip_cliente = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(ip_cliente))
                {
                    ip_cliente = HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
                }
                return ip_cliente;
            }
        }

        static string InvertObtenerDoc(string ObtenerDocumento)
        {
            if (ObtenerDocumento.Equals(WS.WSReglasCrediticia.tipoDeDocumento.DNI))
                return Funciones.CheckStr(ConfigurationManager.AppSettings["constCodTipoDocumentoDNI"]);
            if (ObtenerDocumento.Equals(WS.WSReglasCrediticia.tipoDeDocumento.RUC10) || ObtenerDocumento.Equals(WS.WSReglasCrediticia.tipoDeDocumento.RUC20))
                return Funciones.CheckStr(ConfigurationManager.AppSettings["constCodTipoDocumentoRUC"]);
            if (ObtenerDocumento.Equals(WS.WSReglasCrediticia.tipoDeDocumento.CE))
                return Funciones.CheckStr(ConfigurationManager.AppSettings["constCodTipoDocumentoCEX"]);
            if (ObtenerDocumento.Equals(WS.WSReglasCrediticia.tipoDeDocumento.CIRE))
                return ReadKeySettings.Key_codigoDocCIRE;
            if (ObtenerDocumento.Equals(WS.WSReglasCrediticia.tipoDeDocumento.CIE))
                return ReadKeySettings.Key_codigoDocCIE;
            if (ObtenerDocumento.Equals(WS.WSReglasCrediticia.tipoDeDocumento.CPP))
                return ReadKeySettings.Key_codigoDocCPP;
            if (ObtenerDocumento.Equals(WS.WSReglasCrediticia.tipoDeDocumento.CTM))
                return ReadKeySettings.Key_codigoDocCTM;

            return Funciones.CheckStr(ConfigurationManager.AppSettings["constCodTipoDocumentoDNI"]);
        }
        #endregion
    }
}
