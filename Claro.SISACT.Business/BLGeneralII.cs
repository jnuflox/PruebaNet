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
    public class BLGeneralII
    {
        //public BESolicitudPersona DetalleEvaluacionNatural(Int64 nroSEC)
        //{
        //    DAGeneralII obj = new DAGeneralII();
        //    return obj.DetalleEvaluacionNatural(nroSEC);
        //}

        public ArrayList ListarPlanesSolicitudPersona(string p_sec)
        {
            DAGeneralII objDAGeneralII = new DAGeneralII();
            return objDAGeneralII.ListarPlanesSolicitudPersona(p_sec);
        }

        public List<BEPlan> ListarPlanTarifario(string pstrOferta, string pstrProducto, string pstrDespacho, string pstrFlujo, string pstrDocumento,
            string pstrOficina, string pstrCasoEspecial, string pstrPlazo, string pstrRiesgo)
        {
            return new DAGeneralII().ListarPlanTarifario(pstrOferta, pstrProducto, pstrDespacho, pstrFlujo, pstrDocumento, pstrOficina,
                pstrCasoEspecial, pstrPlazo, pstrRiesgo);
        }

        public DataTable ListarEquipoGama()
        {
            return new DAGeneralII().ListarEquipoGama();
        }

        public List<BEItemGenerico> ListarCampaniaCE(string pstrCasoEspecial)
        {
            return new DAGeneralII().ListarCampaniaCE(pstrCasoEspecial);
        }

        public List<BEItemGenerico> ListarNoMostrarCampania(string pstrDocumento, string pstrRiesgo, string pstrPlan, string pstrPlazo, string pstrOficina)
        {
            return new DAGeneralII().ListarNoMostrarCampania(pstrDocumento, pstrRiesgo, pstrPlan, pstrPlazo, pstrOficina);
        }

        public List<BEItemGenerico> ListarCampanaDTH(string pstrCodPlazo, string pstrCodPlan)
        {
            DAGeneralII objDAGeneralII = new DAGeneralII();
            return objDAGeneralII.ListarCampanaDTH(pstrCodPlazo, pstrCodPlan);
        }

        public List<BEServicio> ListarServicioDTH(string pstrCodPlan)
        {
            DAGeneralII objDAGeneralII = new DAGeneralII();
            return objDAGeneralII.ListarServicioDTH(pstrCodPlan);
        }

        public ArrayList ListarKitsDTH(string p_tipo_operacion, string p_cod_campania, string p_plazo_acuerdo)
        {
            return new DAGeneralII().ListarKitsDTH(p_tipo_operacion, p_cod_campania, p_plazo_acuerdo);
        }

        public List<BEItemGenerico> ListarTopeAutomatico(string pstrPlanCodigo)
        {
            return new DAGeneralII().ListarTopeAutomatico(pstrPlanCodigo);
        }

        public List<BEItemGenerico> ListarTipoProductoxOferta(string pstrOferta, string pstrFlujo, string pstrCasoEspecial, string pstrTipoDoc, string pstrOficina)
        {
            return new DAGeneralII().ListarTipoProductoxOferta(pstrOferta, pstrFlujo, pstrCasoEspecial, pstrTipoDoc, pstrOficina);
        }

        public List<BECasoEspecial> ListarCasoEspecial(string pstrOferta, string pstrDocumento, string pstrOficina)
        {
            DAGeneralII obj = new DAGeneralII();
            return obj.ListarCasoEspecial(pstrOferta, pstrDocumento, pstrOficina);
        }

        public List<BEItemGenerico> ListarPlazoAcuerdo(string pstrTipoProducto, string pstrCasoEspecial)
        {
            DAGeneralII objDAGeneralII = new DAGeneralII();
            return objDAGeneralII.ListarPlazoAcuerdo(pstrTipoProducto, pstrCasoEspecial);
        }
//gaa20140414
        public List<BEItemGenerico> ListarPlazoAcuerdoHFC(string pstrCampana)
        {
            DAGeneralII objDAGeneralII = new DAGeneralII();
            return objDAGeneralII.ListarPlazoAcuerdoHFC(pstrCampana);
        }
//fin gaa20140414
        public List<BEItemGenerico> ListarPaqueteUni(string pstrDocumento, string pstrOferta, string pstrPlazo)
        {
            return new DAGeneralII().ListarPaqueteUni(pstrDocumento, pstrOferta, pstrPlazo);
        }

        public ArrayList ListarPlanIndiRestServ(string pstrPlan, string pstrCasoEspecial)
        {
            return new DAGeneralII().ListarPlanIndiRestServ(pstrPlan, pstrCasoEspecial);
        }

        public ArrayList ListarServiciosXPaqPlan(string codPaquete, string codPlan, int idSecuencia)
        {
            return new DAGeneralII().ListarServiciosXPaqPlan(codPaquete, codPlan, idSecuencia);
        }

        public ArrayList ConsultarListaServicios(string p_plan_tarifario, string p_tipo_cliente, string p_mandt)
        {
            return new DAGeneralII().ConsultarListaServicios(p_plan_tarifario, p_tipo_cliente, p_mandt);
        }

        public ArrayList ListarPlanesXPaquete(string codPaquete)
        {
            return new DAGeneralII().ListarPlanesXPaquete(codPaquete);
        }

        public List<BEItemGenerico> ListarCuota(string pstrDocumento, string pstrRiesgo, string pstrPlan, string pstrPlazo,
            int pintNroPlanes, string pstrCasoEspecial)
        {
            return new DAGeneralII().ListarCuota(pstrDocumento, pstrRiesgo, pstrPlan, pstrPlazo, pintNroPlanes, pstrCasoEspecial);
        }

        public List<BEItemGenerico> ListarTipoProducto(string pstrOferta, string pstrFlujo)
        {
            return new DAGeneralII().ListarTipoProducto(pstrOferta, pstrFlujo);
        }

        public ArrayList ListarPromocionesXPaquete3Play(string pstrPaquete)
        {
            DAGeneralII objDAGeneralII = new DAGeneralII();
            return objDAGeneralII.ListarPromocionesXPaquete3Play(pstrPaquete);
        }

        public DataTable ListarSolucion3Play(string pstrTipoServicio, out int pintCodError, out string pstrMsjError)
        {
            DAGeneralII objDAGeneralII = new DAGeneralII();
            return objDAGeneralII.ListarSolucion3Play(pstrTipoServicio, out pintCodError, out pstrMsjError);
        }

        public DataTable ListarPaquete3Play(Int64 plngIdSolucion, out int pintCodError, out string pstrMsjError)
        {
            DAGeneralII objDAGeneralII = new DAGeneralII();
            return objDAGeneralII.ListarPaquete3Play(plngIdSolucion, out pintCodError, out pstrMsjError);
        }

        public List<BEServicioHFC> ListarPlanesXPaquete3Play(string pstrPaquete)
        {
            DAGeneralII objDAGeneralII = new DAGeneralII();
            return objDAGeneralII.ListarPlanesXPaquete3Play(pstrPaquete);
        }

        public List<BEItemGenerico> ListarCampanaDTH1(string pstrOficina)
        {
            return new DAGeneralII().ListarCampanaDTH1(pstrOficina);
        }

        public List<BEPlan> ListarPlanDTH(string pstrOferta, string pstrCampana)
        {
            return new DAGeneralII().ListarPlanDTH(pstrOferta, pstrCampana);
        }

        public List<BEItemGenerico> ListarPlazoDTH(string pstrCodPlan)
        {
            return new DAGeneralII().ListarPlazoDTH(pstrCodPlan);
        }

        public string ConsultarProductoPaquete(string pPaquete)
        {
            DAGeneralII objDAGeneralII = new DAGeneralII();
            return objDAGeneralII.ConsultarProductoPaquete(pPaquete);
        }

        public string ValidarSECRecurrente(string strTipoDocumento, string strNroDocumento, string strOferta, string strCasoEspecial,
            string strCadenaDetalle, ref string flgReingresoSec)
        {
            return new DAGeneralII().ValidarSECRecurrente(strTipoDocumento, strNroDocumento, strOferta, strCasoEspecial, strCadenaDetalle, ref flgReingresoSec);
        }

        public List<BEPlanDetalleHFC> DetalleOferta3Play(Int64 nroSEC)
        {
            return new DAGeneralII().DetalleOferta3Play(nroSEC);
        }

        public ArrayList ListarPlanDTH(string codPlan, string codTipoProd, string codTipoVenta, string codPlazo, string tipoPlan, string codCampana)
        {
            return new DAGeneralII().ListarPlanDTH(codPlan, codTipoProd, codTipoVenta, codPlazo, tipoPlan, codCampana);
        }

        public ArrayList ConsultarListaServiciosDTH(string p_plan_tarifario)
        {
            return new DAGeneralII().ConsultarListaServiciosDTH(p_plan_tarifario);
        }

        public double ObtenerPrecioKit(string pstrCodCampana, string pstrCodPlaza, int pintcodKit)
        {
            DAGeneralII objDAGeneralII = new DAGeneralII();
            return objDAGeneralII.ObtenerPrecioKit(pstrCodCampana, pstrCodPlaza, pintcodKit);
        }

        public double ObtenerCFAlquilerKit(int pintcodKit, int pintCampania, string pintPlazo)
        {
            DAGeneralII objDAGeneralII = new DAGeneralII();
            return objDAGeneralII.ObtenerCFAlquilerKit(pintcodKit, pintCampania, pintPlazo);
        }

    }
}