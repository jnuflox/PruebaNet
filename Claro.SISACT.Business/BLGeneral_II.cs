using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using Claro.SISACT.Entity;
using Claro.SISACT.Data;

namespace Claro.SISACT.Business
{
    public class BLGeneral_II
    {
        public DataTable ListarEquipoGama()
        {
            return new DAGeneral_II().ListarEquipoGama();
        }

        public ArrayList ListarKitsDTH(string p_tipo_operacion, string p_cod_campania, string p_plazo_acuerdo, string p_plan)
        {
            return new DAGeneral_II().ListarKitsDTH(p_tipo_operacion, p_cod_campania, p_plazo_acuerdo, p_plan);
        }

        public List<BEItemGenerico> ListarTopeAutomatico(string pstrPlanCodigo)
        {
            return new DAGeneral_II().ListarTopeAutomatico(pstrPlanCodigo);
        }

        public List<BEItemGenerico> ListarTipoProductoxOferta(string pstrOferta, string pstrFlujo, string pstrCasoEspecial, string pstrTipoDoc, string pstrModalidadVenta)
        {
            return new DAGeneral_II().ListarTipoProductoxOferta(pstrOferta, pstrFlujo, pstrCasoEspecial, pstrTipoDoc, pstrModalidadVenta);
        }

        public List<BECasoEspecial> ListarCasoEspecial(string pstrOferta, string strTipoFlujo, string strTipoOperacion, string pstrOficina)
        {
            DAGeneral_II obj = new DAGeneral_II();
            return obj.ListarCasoEspecial(pstrOferta, strTipoFlujo, strTipoOperacion, pstrOficina);
        }

        public List<BEItemGenerico> ListarPlazoAcuerdo(string pstrTipoProducto, string pstrCasoEspecial, string pstrModalidadVenta)
        {
            DAGeneral_II objDAGeneralII = new DAGeneral_II();
            return objDAGeneralII.ListarPlazoAcuerdo(pstrTipoProducto, pstrCasoEspecial, pstrModalidadVenta);
        }

        public List<BEItemGenerico> ListarPaquete(string pstrDocumento, string pstrOferta, string pstrPlazo)
        {
            return new DAGeneral_II().ListarPaquete(pstrDocumento, pstrOferta, pstrPlazo);
        }

        public List<BESecServicio_AP> ListarPlanTopeConfig(string pstrPlan, string pstrCasoEspecial)
        {
            return new DAGeneral_II().ListarPlanTopeConfig(pstrPlan, pstrCasoEspecial);
        }

        public ArrayList ListarServiciosXPaqPlan(string codPaquete, string codPlan, int idSecuencia)
        {
            return new DAGeneral_II().ListarServiciosXPaqPlan(codPaquete, codPlan, idSecuencia);
        }

        public ArrayList ConsultarListaServicios(string p_plan_tarifario, string p_tipo_cliente, string p_mandt)
        {
            return new DAGeneral_II().ConsultarListaServicios(p_plan_tarifario, p_tipo_cliente, p_mandt);
        }

        public List<BESecPlan_AP> ListarPlanesXPaquete(string codPaquete)
        {
            return new DAGeneral_II().ListarPlanesXPaquete(codPaquete);
        }

        public List<BEItemGenerico> ListarCuota(string pstrDocumento, string pstrRiesgo, string pstrPlan, string pstrPlazo,
            int pintNroPlanes, string pstrCasoEspecial)
        {
            return new DAGeneral_II().ListarCuota(pstrDocumento, pstrRiesgo, pstrPlan, pstrPlazo, pintNroPlanes, pstrCasoEspecial);
        }

        public DataTable ListarPaquete3Play(Int64 plngIdSolucion, out int pintCodError, out string pstrMsjError)
        {
            DAGeneral_II objDAGeneralII = new DAGeneral_II();
            return objDAGeneralII.ListarPaquete3Play(plngIdSolucion, out pintCodError, out pstrMsjError);
        }

        public List<BEServicioHFC> ListarPlanesXPaquete3Play(string pstrPaquete)
        {
            DAGeneral_II objDAGeneralII = new DAGeneral_II();
            return objDAGeneralII.ListarPlanesXPaquete3Play(pstrPaquete);
        }

        public string ConsultarProductoPaquete(string pPaquete)
        {
            DAGeneral_II objDAGeneralII = new DAGeneral_II();
            return objDAGeneralII.ConsultarProductoPaquete(pPaquete);
        }

        public string ValidarSECRecurrente(string strTipoDocumento, string strNroDocumento, string strOferta, string strCasoEspecial,
            string strCadenaDetalle, ref string flgReingresoSec)
        {
            return new DAGeneral_II().ValidarSECRecurrente(strTipoDocumento, strNroDocumento, strOferta, strCasoEspecial, strCadenaDetalle, ref flgReingresoSec);
        }
         public List<BEPlanDetalleHFC> DetalleOferta3Play(Int64 nroSEC, string strTipoProducto)
        {
            return new DAGeneral_II().DetalleOferta3Play(nroSEC, strTipoProducto);
        }

        public double ObtenerPrecioKit(string pstrCodCampana, string pstrCodPlaza, int pintcodKit)
        {
            DAGeneral_II objDAGeneralII = new DAGeneral_II();
            return objDAGeneralII.ObtenerPrecioKit(pstrCodCampana, pstrCodPlaza, pintcodKit);
        }

        public double ObtenerCFAlquilerKit(int pintcodKit, int pintCampania, string pintPlazo)
        {
            DAGeneral_II objDAGeneralII = new DAGeneral_II();
            return objDAGeneralII.ObtenerCFAlquilerKit(pintcodKit, pintCampania, pintPlazo);
        }

        //gaa20140711: Datos de combos
        public List<BEItemGenerico> ListarCombo(string pstrOficina, string pstrOferta, string pstrTipoOperacion, string pstrTipoFlujo, string pstrTipoDocumento, string pstrModalidad)
        {
            return new DAGeneral_II().ListarCombo(pstrOficina, pstrOferta, pstrTipoOperacion, pstrTipoFlujo, pstrTipoDocumento, pstrModalidad);
        }

        public string ListarCampanasCombo(string pstrComboCodigo)
        {
            DAGeneral_II objDAGeneralII = new DAGeneral_II();
            return objDAGeneralII.ListarCampanasCombo(pstrComboCodigo);
        }

        public string ListarPlazosCombo(string pstrComboCodigo)
        {
            DAGeneral_II objDAGeneralII = new DAGeneral_II();
            return objDAGeneralII.ListarPlazosCombo(pstrComboCodigo);
        }

        public List<BEEquipo> ListarEquipo3Play(string pstrTipoProducto, string pstrPlan)
        {
            return new DAGeneral_II().ListarEquipo3Play(pstrTipoProducto, pstrPlan);
        }
        public List<BEItemGenerico> ListarPlanListaPrecio(string strPlan)
        {
            return new DAGeneral_II().ListarPlanListaPrecio(strPlan);
        }
        public List<BEPlanDetalleHFC> EquiposOferta3Play(Int64 nroSEC, string strTipoProducto)
        {
            return new DAGeneral_II().EquiposOferta3Play(nroSEC, strTipoProducto);
        }
        public bool ValidarDNIVendedor(string pstrDNI)
        {
            return new DAGeneral_II().ValidarDNIVendedor(pstrDNI);
        }
        public List<BEItemGenerico> ListarPaquete3Play(string pstrCombo, string pstrCampana, string pstrPlazo, string pstrIdProducto)
        {
            return new DAGeneral_II().ListarPaquete3Play(pstrCombo, pstrCampana, pstrPlazo, pstrIdProducto);
        }
         public List<BEPlan> ListarPlanesXPaquete3Play(string pstrCombo, string pstrCampana, string pstrPlazo, string pstrPaquete, string pstrTipoOperacion, string pstrFlagPorta, string pstrIdProducto)
        {
            return new DAGeneral_II().ListarPlanesXPaquete3Play(pstrCombo, pstrCampana, pstrPlazo, pstrPaquete, pstrTipoOperacion, pstrFlagPorta, pstrIdProducto);
        }
         public List<BEServicioHFC> ListarServiciosXPlan3Play(string pstrPlan, string pstrIdProducto)
        {
             return new DAGeneral_II().ListarServiciosXPlan3Play(pstrPlan, pstrIdProducto);
        }
        //gaa20150714
        //public List<BEItemGenerico> ListarCampana(string pstrComboCodigo, string pstrOficina, string pstrOferta, string strTipoProducto, string pstrCasoEspecial, string pModalidadVenta)
        public List<BEItemGenerico> ListarCampana(string pstrComboCodigo, string pstrOficina, string pstrOferta, string strTipoProducto, string pstrCasoEspecial, string pModalidadVenta, string pstrFlujo, string pstrTipoOperacion)
        {
            //gaa20150714
            //return new DAGeneral_II().ListarCampana(pstrComboCodigo, pstrOficina, pstrOferta, strTipoProducto, pstrCasoEspecial, pModalidadVenta);
            return new DAGeneral_II().ListarCampana(pstrComboCodigo, pstrOficina, pstrOferta, strTipoProducto, pstrCasoEspecial, pModalidadVenta, pstrFlujo, pstrTipoOperacion);
            //fin gaa20150714
        }
        public List<BEItemGenerico> ListarPlazo(string pstrTipoProducto, string pstrCasoEspecial, string pModalidadVenta)
        {
            return new DAGeneral_II().ListarPlazo(pstrTipoProducto, pstrCasoEspecial, pModalidadVenta);
        }
        //gaa20161020
        //public List<BEPlan> ListarPlanTarifario(string strTipoFlujo, string strTipoDocumento, string strTipoOferta, string strTipoOperacion, string strTipoProducto,
        //                                        string strCasoEspecial, string strCampana, string strPlazo, string strOficina, string strCombo, ref string filtro)
        public List<BEPlan> ListarPlanTarifario(string strTipoFlujo, string strTipoDocumento, string strTipoOferta, string strTipoOperacion, string strTipoProducto,
                                            string strCasoEspecial, string strCampana, string strPlazo, string strOficina, string strCombo, string strFamilia, ref string filtro)    
        {
            //gaa20161020
            //return new DAGeneral_II().ListarPlanTarifario(strTipoFlujo, strTipoDocumento, strTipoOferta, strTipoOperacion, strTipoProducto,
            //                                                    strCasoEspecial, strCampana, strPlazo, strOficina, strCombo, ref filtro);
            return new DAGeneral_II().ListarPlanTarifario(strTipoFlujo, strTipoDocumento, strTipoOferta, strTipoOperacion, strTipoProducto,
                                                    strCasoEspecial, strCampana, strPlazo, strOficina, strCombo, strFamilia, ref filtro);
            //fin gaa20161020
        }

        public List<BESecServicio_AP> ListarServiciosXPlan(string pstrTipoProducto, string pstrPlan)
        {
            return new DAGeneral_II().ListarServiciosXPlan(pstrTipoProducto, pstrPlan);
        }
        public List<BEPlan> ListarPlanesCombo(string pstrComboCodigo)
        {
            return new DAGeneral_II().ListarPlanesCombo(pstrComboCodigo);
        }
        public List<BEDescuento> ListarComboDescuento(string pstrComboCodigo)
        {
            return new DAGeneral_II().ListarComboDescuento(pstrComboCodigo);
        }
        public string ListarComboxProducto(string pstrComboCodigo)
        {
            return new DAGeneral_II().ListarComboxProducto(pstrComboCodigo);
        }
        public List<BEItemGenerico> ListarComboEquipo(string pstrComboCodigo)
        {
            return new DAGeneral_II().ListarComboEquipo(pstrComboCodigo);
        }
        public string ObtenerCampanaSap(string strCampana)
        {
            return new DAGeneral_II().ObtenerCampanaSap(strCampana);
        }
        public List<BEItemGenerico> ListarItemxPDV(int intTipoItem, string strOficina)
        {
            return new DAGeneral_II().ListarItemxPDV(intTipoItem, strOficina);
        }
//gaa20161020
        public List<BEItemGenerico> ListarFamiliaPlan (string strModalidad, string strCampana)
        {
            return new DAGeneral_II().ListarFamiliaPlan(strModalidad, strCampana);
        }
//fin gaa20161020

       //PROY-29121-INI

        public List<BEItemGenerico> ListarPlanTelFija(string strProductoCododigo)
        {
            return new DAGeneral_II().ListarPlanTelFija(strProductoCododigo);
        }
        //PROY-29121-FIN

        //PROY_33313 INICIO
        public void ListarEstadoFlaj(Int64 nroSEC, out string P_RESULTADO, out string P_NRO_RESULTADO, out string P_DES_RESULTADO)
        {
            DAGeneral_II objtos = new DAGeneral_II();
            objtos.ListarEstadoFlaj(nroSEC, out P_RESULTADO, out P_NRO_RESULTADO, out P_DES_RESULTADO);
        }
        //PROY_33313 FIN

        //Proy-140360  inicio
        public void ListarEstadoFlaj2(Int64 nroSEC, out string P_RESULTADO, out string P_NRO_RESULTADO, out string P_DES_RESULTADO)
        {
            DAGeneral_II objtos = new DAGeneral_II();
            objtos.ListarEstadoFlaj2(nroSEC, out P_RESULTADO, out P_NRO_RESULTADO, out P_DES_RESULTADO);
        }
        //Proy-140360  fin

        #region INI - PROY-32581        
        public List<BEPlan> ListarPlanesXPaqueteLTE(string pstrCombo, string pstrCampana, string pstrPlazo, string pstrPaquete, string pstrTipoOperacion, string pstrFlagPorta, string pstrIdProducto, string pstrGsrvcCodigo)
        {
            return new DAGeneral_II().ListarPlanesXPaqueteLTE(pstrCombo, pstrCampana, pstrPlazo, pstrPaquete, pstrTipoOperacion, pstrFlagPorta, pstrIdProducto, pstrGsrvcCodigo);
        }
        #endregion FIN - PROY-32581

        //INI: INICIATIVA-219
        public BEPlan_CBIO ListarPlanesCBIO(string strPO_ID)
        {
            DAGeneral_II objtos = new DAGeneral_II();
            return objtos.ListarPlanesCBIO(strPO_ID);
        }

        public BEPlan_CBIO ListarDatosPlanesCBIO(string strPO_ID, string estado, string tipCliente)
        {
            DAGeneral_II objtos = new DAGeneral_II();
            return objtos.ListarDatosPlanesCBIO(strPO_ID, estado, tipCliente);
        }

        public BEPlan_CBIO ListarPlanesCBIO_Catalogo(string strPO_ID)
        {
            DAGeneral_II objtos = new DAGeneral_II();
            return objtos.ListarPlanesCBIO_Catalogo(strPO_ID);
        }

        public void ObtenerDescripcionTicklers(string strCodigoTicklerCBIO, ref string strDescripcionTickler, ref string strTipoTickler, ref string strCodigoTicklerBSCS, ref string strMsjRespuesta)
        {
            DAGeneral_II objtos = new DAGeneral_II();
            objtos.ObtenerDescripcionTicklers(strCodigoTicklerCBIO, ref strDescripcionTickler, ref strTipoTickler, ref strCodigoTicklerBSCS, ref strMsjRespuesta);
        }

        public void ObtenerDescripcionServiciosAdic(string strCodigoServicio, ref string strDescripcionServicio, ref string strCodigoRespuesta, ref string strMsjRespuesta)
        {
            DAGeneral_II objtos = new DAGeneral_II();
            objtos.ObtenerDescripcionServiciosAdic(strCodigoServicio, ref strDescripcionServicio, ref strCodigoRespuesta, ref strMsjRespuesta);
        }

        public void ConsultarFlagCBIO(string[] arrParamConsultaCBIO, ref string strCodigoRespuesta, ref string strMensajeRespuesta)
        {
            DAGeneral_II objtos = new DAGeneral_II();
            objtos.ConsultarFlagCBIO(arrParamConsultaCBIO, ref strCodigoRespuesta, ref strMensajeRespuesta);
        }

        public BEItemGenerico ListarTopeAutomaticoCBIO(string pstrPlanCodigo,int idConsulta)
        {
            return new DAGeneral_II().ListarTopeAutomaticoCBIO(pstrPlanCodigo, idConsulta);
        }        
        
        public BEPlan ObtenerPlanBSCS(string strtipoServ, string strcodPlan, ref string codRespuesta, ref string msjRespuesta)
        {
            return new DAGeneral_II().ObtenerPlanBSCS(strtipoServ, strcodPlan, ref codRespuesta, ref msjRespuesta);
        }

        public Boolean ValidaPlanFullClaro(string strPoBasic, ref string codPlanRespuesta, ref string codRespuesta, ref string msjRespuesta)
        {
            return new DAGeneral_II().ValidaPlanFullClaro(strPoBasic, ref codPlanRespuesta, ref codRespuesta, ref msjRespuesta);
        }  
        //FIN: INICIATIVA-219

        //INICIO INC000003048070 

        public static void ConsultarCandidatoBono(string strTipoDocumento, string nroDocumento, ref string estadoBonoBSCSFullClaro)
        {
            DAGeneral_II objtos = new DAGeneral_II();

            objtos.ConsultarCandidatoBono(strTipoDocumento, nroDocumento, ref  estadoBonoBSCSFullClaro);
           
        }

        //FIN INC000003048070 

    }
}