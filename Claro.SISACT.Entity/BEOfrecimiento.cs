using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEOfrecimiento
    {
        public int IdFila { get; set; }
        public string IdProducto { get; set; }
        public double CargoFijo { get; set; }
        public string TieneAutonomia { get; set; }
        //------------------------------------------
        public string In_solicitud { get; set; }
        public string In_cliente { get; set; }
        public string In_direccion_cliente { get; set; }
        public string In_doc_cliente { get; set; }
        public string In_rrll_cliente { get; set; }
        public string In_equipo { get; set; }
        public string In_oferta { get; set; }
        public string In_campana { get; set; }
        public string In_plan_actual { get; set; }
        public string In_plan_solicitado { get; set; }
        public string In_servicio { get; set; }
        public string In_pdv { get; set; }
        public string In_direccion_pdv { get; set; }
        //------------------------------------------
        public int nroLineaActivoxProducto { get; set; }
        public int nroLineaEvaluadoxProducto { get; set; }
        public double montoFacturadoxProducto { get; set; }

        public int CantidadDeLineasMaximas { get; set; }
        public int CantidadDeLineasAdicionalesRUC { get; set; }
        public int CantidadDeLineasRenovaciones { get; set; }
        public string AutonomiaRenovacion { get; set; }
        public double MontoCFParaRUC { get; set; }
        public string TipoDeAutonomiaCargoFijo { get; set; }
        public string ControlDeConsumo { get; set; }
        public double CostoDeInstalacion { get; set; }
        public int CantidadDeAplicacionesRenta { get; set; }
        public int FrecuenciaDeAplicacionMensual { get; set; }
        public int MesInicioRentas { get; set; }
        public double MontoDeGarantia { get; set; }
        public string Tipodecobro { get; set; }
        public string TipoDeGarantia { get; set; }
        public double LimiteDeCreditoCobranza { get; set; }
        public double MontoTopeAutomatico { get; set; }
        public string PrioridadPublicar { get; set; }
        public string ProcesoDeExoneracionDeRentas { get; set; }
        public string ProcesoIDValidator { get; set; }
        public string ProcesoValidacionInternaClaro { get; set; }
        public string Publicar { get; set; }
        public string Restriccion { get; set; }
        public string CapacidadDePago { get; set; }
        public int ComportamientoConsolidado { get; set; }
        public int ComportamientoDePagoC1 { get; set; }
        public double CostoTotalEquipos { get; set; }
        public double FactorDeEndeudamientoCliente { get; set; }
        public double FactorDeRenovacionCliente { get; set; }
        public double PrecioDeVentaTotalEquipos { get; set; }
        public string RiesgoEnClaro { get; set; }
        public string RiesgoOferta { get; set; }
        public int RiesgoTotalEquipo { get; set; }
        public string RiesgoTotalRepLegales { get; set; }
        public string Mensaje { get; set; }

        public List<BECuota> ListaCuotas { get; set; }
        public string Plan { get; set; }
        public string Combo { get; set; }
        public double DescuentoCF { get; set; }
        public List<BEPlan> ListaPlanProac { get; set; }//PROY-30748
        public int NroCuotaProac { get; set; }//PROY-30748
        public double creditScore { get; set; }//PROY-30748
        /*------------------------PROY 29123----------------------*/
        public int MaximoCuotas { get; set; }
        public double PrecioEquipoMaximo { get; set; }
        public string MostrarMensaje { get; set; }

        //PROY-31948 INI
        //VARIABLE BRMS
        public string ResultadoEvaluacionCuotas { get; set; }
        //PROY-31948 FIN

        //INICIOPROY-29215
        public List<String> ListaFormaPago { get; set; }
        public List<Int32> ListarCuotasPago { get; set; }
        public int NroCuota { get; set; }
        public string FormaPago { get; set; }
        //PROY-29215 FIN 
        //PROY-140335 INI RFI
        public string ejecucionConsultaPrevia { get; set; }
        public string numeroLinea { get; set; }
        //PROY-140335 FIN RFI

        //PROY-140439 BRMS CAMPAÑA NAVIDEÑA::INI
        public string TipoDocCli { get; set; }
        public string NumDocCli { get; set; }
        public string Lista_ofrecimientocampanas { get; set; }
        public string[] CampanasNavidad { get; set; }
        //PROY-140439 BRMS CAMPAÑA NAVIDEÑA::FIN
        //PROY-140579 INICIO RF02
        public string MotivoDeRestriccion { get; set; }
        public string MostrarMotivoDeRestriccion { get; set; }        
        //PROY-140579 FIN RF02
		//INICIO PROY-140546 Cobro Anticipado de Instalacion
        public double MontoAnticipadoInstalacion { get; set; }
        public string TipoCobroAnticipadoInstalacion { get; set; }
        //FIN PROY-140546 Cobro Anticipado
    }
}
