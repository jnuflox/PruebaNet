using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity;
using Claro.SISACT.Data;
using System.Collections;
using System.Configuration;
using System.Data;
using Claro.SISACT.Common;
namespace Claro.SISACT.Business
{
    public class BLEvaluacion
    {
        public BLEvaluacion() { }

        public List<BEBilletera> ObtenerLCxBilletera(string strRiesgo, string strTipoDoc, string strNroDocumento, string essaludSunat, string strClienteNuevo, double dblLC)
        {
            return new DAEvaluacion().ObtenerLCxBilletera(strRiesgo, strTipoDoc, strNroDocumento, essaludSunat, strClienteNuevo, dblLC);
        }
        public List<BEBilletera> ObtenerMontoFactxBilletera(string strNroDocumento, string strCadena)
        {
            return new DAEvaluacion().ObtenerMontoFactxBilletera(strNroDocumento, strCadena);
        }
        public List<BEPlanBilletera> ObtenerBilleteraxPlan(string strListaPlan)
        {
            return new DAEvaluacion().ObtenerBilleteraxPlan(strListaPlan);
        }
        public List<BEPlanBilletera> ObtenerPlanesxBilletera(int flgSistema)
        {
            return new DAEvaluacion().ObtenerPlanesxBilletera(flgSistema);
        }

        public DataSet ObtenerDatosEvaluacion(string strOficina, string strTipoDoc, string strNroDoc, string strNroOperacion)
        {
            return new DAEvaluacion().ObtenerDatosEvaluacion(strOficina, strTipoDoc, strNroDoc, strNroOperacion);
        }
        public List<BEEquipoBRMS> ConsultarDetalleDecoxKIT(string idKIT)
        {
            return new DAEvaluacion().ConsultarDetalleDecoxKIT(idKIT);
        }
        public double ObtenerCFPromocional(string strIdCampana)
        {
            return new DAEvaluacion().ObtenerCFPromocional(strIdCampana);
        }
        public string ConsultarTextoRangoLC(string strTipoDocumento, string strNroDocumento, double dblLC)
        {
            return new DAEvaluacion().ConsultarTextoRangoLC(strTipoDocumento, strNroDocumento, dblLC);
        }
        public List<BEItemGenerico> ObtenerPlanesBSCSxCE(string strCasoEspecial)
        {
            return new DAEvaluacion().ObtenerPlanesBSCSxCE(strCasoEspecial);
        }
        public string ConsultarBlackListCE(string strCasoEspecial, string strTipoDoc, string strNroDoc, ref double dblCfMaximo)
        {
            return new DAEvaluacion().ConsultarBlackListCE(strCasoEspecial, strTipoDoc, strNroDoc, ref dblCfMaximo);
        }
        public void ConsultarCEReglas(string strCasoEspecial, ref string listaCEPlanBscs, ref string listaCEPlan, ref string listaCEProducto)
        {
            new DAEvaluacion().ConsultarCEReglas(strCasoEspecial, ref listaCEPlanBscs, ref listaCEPlan, ref listaCEProducto);
        }
        public string ValidarVendedorDNI(string strNroDocumento)
        {
            return new DAEvaluacion().ValidarVendedorDNI(strNroDocumento);
        }
        public bool InsertarDatosBRMS(Int64 nroSEC, Int64 pln_codigo, BEOfrecimiento oOfrecimiento)
        {
            return new DAEvaluacion().InsertarDatosBRMS(nroSEC, pln_codigo, oOfrecimiento);
        }
        public DataSet ObtenerDatosBRMS(Int64 nroSEC)
        {
            return new DAEvaluacion().ObtenerDatosBRMS(nroSEC);
        }
         public DataTable ObtenerDetallePlanes(Int64 nroSECPadre, Int64 nroSEC)
        {
             return new DAEvaluacion().ObtenerDetallePlanes(nroSECPadre, nroSEC);
        }
        public DataSet ObtenerDetalleSrvCuota(Int64 nroSEC)
        {
            return new DAEvaluacion().ObtenerDetalleSrvCuota(nroSEC);
        }
        public DataTable ListarMetricaEvaluacion(Int64 nroSEC)
        {
            return new DAEvaluacion().ListarMetricaEvaluacion(nroSEC);
        }
        public string ObtenerFechaHoraBD(string nroDocumento)
        {
            return new DAEvaluacion().ObtenerFechaHoraBD(nroDocumento);
        }
        public Int64 ValidarSecConcurrente(string tipoDocumento, string nroDocumento, string strHoraInicio)
        {
            return new DAEvaluacion().ValidarSecConcurrente(tipoDocumento, nroDocumento, strHoraInicio);
        }
        public bool InsertarDatosGarantia(Int64 nroSEC, Int64 pln_codigo, BEGarantia objGarantia)
        {
            return new DAEvaluacion().InsertarDatosGarantia(nroSEC, pln_codigo, objGarantia);
        }
        public bool InsertarDatosDescuento(Int64 nroSEC, Int64 spln_codigo, string prdc_codigo, string plnv_codigo, string plnv_descripcion, string usuario, string idCombo)
        {
            return new DAEvaluacion().InsertarDatosDescuento(nroSEC, spln_codigo, prdc_codigo, plnv_codigo, plnv_descripcion, usuario, idCombo);
        }
//gaa20170327
        public string ObtenerBuroDescripcion(string strBuroCodigo)
        {
            return new DAEvaluacion().ObtenerBuroDescripcion(strBuroCodigo);
        }
//fin gaa20170327
        // INICIO - PROY - 30748 
        public DataSet ObtenerDatosBRMSPROA(Int64 nroSEC)
        {
            return new DAEvaluacion().ObtenerDatosBRMSPROA(nroSEC);
        }

        public bool InsertarDatosBRMSPROA(Int64 nroSEC, Int64 pln_codigo, BEOfrecimiento oOfrecimiento)
        {
            return new DAEvaluacion().InsertarDatosBRMSPROA(nroSEC, pln_codigo, oOfrecimiento);
        }
        // FIN - PROY - 30748
        //PROY-30166-IDEA–38863-INICIO
        public bool ActualizarMontoInicial(Int64 intSopln, double intMontoCuota, double dblPorcentajeCuota, ref string strCodRpta, ref string strMsjRpta) //PROY-30166-IDEA–38863-INICIO
        {
            DAEvaluacion obj = new DAEvaluacion();
            return obj.ActualizarMontoInicial(intSopln, intMontoCuota, dblPorcentajeCuota, ref strCodRpta, ref strMsjRpta);
        } //PROY-30166-IDEA–38863-FIN

        //PROY-30166-INI
        public string ObtenerCuotaIniCom(string CodListaPrecio, string CodMaterial, string CodPlazo)
        {
            DAEvaluacion obj = new DAEvaluacion();
            return obj.ObtenerCuotaIniCom(CodListaPrecio, CodMaterial, CodPlazo);
        }
        //PROY-30166-FIN

        //PROY-32439 MAS INI
        public DataSet ObtenerDatos_Validacion_Cliente_BRMS(Int64 nroSEC)
        {
            return new DAEvaluacion().ObtenerDatos_Validacion_Cliente_BRMS(nroSEC);
        }
        //PROY-32439 MAS FIN
        
        //PROY-FULLCLARO ::INICIO
        public static bool InsertarBono_FullClaro(Int64 nroSEC, string beneficio, bool flagPorta, string usuario, string op, BESolicitudPersona objSolPersona, BESolicitudEmpresa objSolEmpresa, ArrayList listaPlanDetalle, ref string strCodRpta, ref string strMsjRpta)
        {
            bool salida = false;
            ArrayList lstPlanDetalle;
            if (op.Equals("P"))
            {
                salida = DAEvaluacion.InsertarBonoCabecera_FullClaro(nroSEC, beneficio, flagPorta, usuario, objSolPersona.TDOCC_CODIGO, objSolPersona.CLIEC_NUM_DOC, objSolPersona.PRDC_CODIGO, objSolPersona.SOLIV_DES_EST, objSolPersona.FLAG_PORTABILIDAD, ref strCodRpta, ref strMsjRpta);
                if (objSolPersona.oPlanDetalle != null)
                {
                    lstPlanDetalle = objSolPersona.oPlanDetalle;
                    foreach (BEPlanDetalleVenta objPlanDetalle in lstPlanDetalle)
                    {
                        salida = DAEvaluacion.InsertarBonoDetalle_FullClaro(nroSEC, Funciones.CheckStr(objSolPersona.CLIEC_NUM_DOC), Funciones.CheckStr(objPlanDetalle.PLANC_CODIGO), Funciones.CheckStr(objPlanDetalle.TELEFONO), usuario, ref strCodRpta, ref strMsjRpta);
                    }
                }
            }
            else
            {
                salida = DAEvaluacion.InsertarBonoCabecera_FullClaro(nroSEC, beneficio, flagPorta, usuario, Funciones.CheckStr(objSolEmpresa.TDOCC_CODIGO), Funciones.CheckStr(objSolEmpresa.CLIEC_NUM_DOC), Funciones.CheckStr(objSolEmpresa.PRDC_CODIGO), Funciones.CheckStr(objSolEmpresa.SOLIV_DES_EST), Funciones.CheckStr(objSolEmpresa.FLAG_PORTABILIDAD), ref strCodRpta, ref strMsjRpta);
                if (listaPlanDetalle != null)
                {
                    lstPlanDetalle = listaPlanDetalle;
                    foreach (BEPlanDetalleVenta objPlanDetalle in lstPlanDetalle)
                    {
                        salida = DAEvaluacion.InsertarBonoDetalle_FullClaro(nroSEC, Funciones.CheckStr(objSolEmpresa.CLIEC_NUM_DOC), Funciones.CheckStr(objPlanDetalle.PLANC_CODIGO), Funciones.CheckStr(objPlanDetalle.TELEFONO), usuario, ref strCodRpta, ref strMsjRpta);
                    }
                }
            }
            return salida;
        }

        //PROY-FULLCLARO ::FIN

        //INC-SMS_PORTA_INI
        public static bool RegistrarTrazabilidadPinSMSPorta(string strTipoDocumento, string strNumeroDocumento, string strLinea, string strUserCreacion, string strNodoSisact, ref string strCodRpta, ref string strMsjRpta, ref Int64 intCodigo)
        {
            bool salida = false;
            salida = DAEvaluacion.RegistrarTrazabilidadPinSMSPorta(strTipoDocumento, strNumeroDocumento, strLinea, strUserCreacion, strNodoSisact, ref strCodRpta, ref strMsjRpta, ref intCodigo);
            return salida;
        }

        public static bool ActualizarTrazabilidadPinSMSPorta(string strTipoDocumento, string strNumeroDocumento, string strLinea, Int64 intCodigo, ref string strCodRpta, ref string strMsjRpta)
        {
            bool salida = false;
            salida = DAEvaluacion.ActualizarTrazabilidadPinSMSPorta(strTipoDocumento, strNumeroDocumento, strLinea, intCodigo, ref strCodRpta, ref strMsjRpta);
            return salida;
        }

        public static string ObtenerTrazabilidadPinSMSPorta(string strTipoDocumento, string strNumeroDocumento, Int64 codigoPorta, ref string strCodRpta, ref string strMsjRpta)
        {
            return DAEvaluacion.ObtenerTrazabilidadPinSMSPorta(strTipoDocumento, strNumeroDocumento, codigoPorta, ref strCodRpta, ref strMsjRpta);
        }
        //INC-SMS_PORTA_FIN

        //INICIO PROY-140419 Autorizar Portabilidad sin PIN
        public static bool GrabarValidaSupervisor(string DatosCabeceraValidador, string DatosDetalleValidador, ref string strCodRpta, ref string strMsjRpta)
        {
            bool salida = false;
            salida = DAEvaluacion.GrabarValidaSupervisor(DatosCabeceraValidador, DatosDetalleValidador, ref strCodRpta, ref strMsjRpta);
            return salida;
        }
        //FIN PROY-140419 Autorizar Portabilidad sin PIN

        //PROY-140439 BRMS CAMPAÑA NAVIDEÑA::INI
        public static DataSet ObtenerDatosBRMSValidacionCampana(Int64 nroSEC) 
        {
            return DAEvaluacion.ObtenerDatosBRMSValidacionCampana(nroSEC);
        }

        public static Int64 InsertarDatosBRMSValidacionCampanas(BEOfrecimiento oOfrecimiento)
        {
            return DAEvaluacion.InsertarDatosBRMSValidacionCampanas(oOfrecimiento);
        }

        public static bool ActualizarBRMSValidacionCampanas(Int64 nroSEC, string idsBRMS, ref string strCodRpta, ref string strMsjRpta)
        {
            DAEvaluacion obj = new DAEvaluacion();
            string[] id = idsBRMS.Split(',');
            bool rpta = false;
            for (int i = 0; i < id.Length; i++)
            {
                rpta = DAEvaluacion.ActualizarBRMSValidacionCampanas(nroSEC, Funciones.CheckInt64(id[i]), ref strCodRpta, ref strMsjRpta);
            }
            return rpta;
        }
        //PROY-140439 BRMS CAMPAÑA NAVIDEÑA::FIN

        //PROY-140457-DEBITO AUTOMATICO-INI
        public static List<BEItemGenerico> ListarEntidad(string idSolicitud)
        {
            return DAEvaluacion.ListarEntidad(idSolicitud);
        }

        public static List<BEItemGenerico> ListarCuenta(string idCuenta)
        {
            return DAEvaluacion.ListarCuenta(idCuenta);
        }

        public static bool GrabarAfiliacionDebitoAutomatico(BEDebitoAutomatico objDebito, string cadenaDetalle, ref string strCodRpta, ref string strMsjRpta) 
        {
            bool salida = false;
            salida = DAEvaluacion.GrabarAfiliacionDebitoAutomatico(objDebito, cadenaDetalle,ref strCodRpta, ref strMsjRpta);
            return salida;
        }
        //PROY-140457-DEBITO AUTOMATICO-FIN

        //PROY-140257-INI        
        public void Consultar_Riesgo_Corp(string TipoDoc, string DescripcionRiesgo, ref string strCodRiesgo, ref string strCodRes, ref string strMsjRespuesta)
        {
            new DAEvaluacion().Consultar_Riesgo_Corp(TipoDoc, DescripcionRiesgo, ref strCodRiesgo, ref strCodRes, ref strMsjRespuesta);
        }
        //PROY-140257-FIN
         
        //PROY-140585 F2 - INICIO
        public static bool ActualizarSECSMSPortabilidad(Int64 intCodigoSMSPN, Int64 intNroSEC, ref string strCodRpta, ref string strMsjRpta)
        {
            bool salida = false;
            salida = DAEvaluacion.ActualizarSECSMSPortabilidad(intCodigoSMSPN, intNroSEC, ref strCodRpta, ref strMsjRpta);
            return salida;
        }        
        //PROY-140585 F2 - FIN

        //PROY-140736- INICIO
        public static int RegistrarBuyBack(int intsopln_codigo, string strcodigocupon, string strimei,string strcodmaterial,string straplicacion,
            string strusuario, string strnodo, ref int intrpta)
        {
            int salida = 0;           salida = DAEvaluacion.RegistrarBuyBack(intsopln_codigo, strcodigocupon, strimei, strcodmaterial, straplicacion,
             strusuario, strnodo,ref intrpta);
            return salida;
        }
        //PROY-140736 - FIN

        #region [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] | [Consultas BSCS]
        public static List<BEItemGenerico> ConsultaClienteBSCS(string strDesDoc, string strNroDoc, ref string strCodRpta, ref string strMsjRpta)
        {
            return DAEvaluacion.ConsultaClienteBSCS(strDesDoc, strNroDoc, ref strCodRpta, ref strMsjRpta);
        }
        #endregion
    }
}
