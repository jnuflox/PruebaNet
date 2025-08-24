
using System;
using System.Collections;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEPlanBSSEval
    {
        //INICIO PROY-30748
        private double _MontoRA;
        private double _PrecionVenta;
        private int _NroCuota;
        private double _CuotaInicial;
        private double _TotalPagar;
        private double _CostoEquipo;//EMMH
        private double _TopeMonto;//EMMH
        private string _IdListaPrecio;//EMMH
        private string _DesListaPrecio;//EMMH

        public string cantidadDeLineasAdicionalesRUC { get; set; }
        public string cantidadDeLineasMaximas { get; set; }
        public string capacidadDePago { get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public double  factorDeEndeudamientoCliente { get; set; }
        public double factorDeRenovacionCliente { get; set; }
        public double montoCFParaRUC { get; set; }
        public double montoDeGarantia { get; set; }

        public double precioDeVentaTotalEquipos { get; set; }
        public string procesoIDValidator { get; set; }
        public string restriccion { get; set; }
        public double riesgoTotalEquipo { get; set; }
        public string tipodecobro { get; set; }
        public double cargoFijo { get; set; }
        public string cantidad { get; set; }
        public double totalPagar { get; set; }

        public BECuota Cuota { get; set; }

        public string CodListaPrecio { get; set; }//PROY 30748 F2 MDE
      
        public double CuotaInicial { set { _CuotaInicial = value; } get { return _CuotaInicial; } }
        //public int NroCuota { set { _NroCuota = value; } get { return _NroCuota; } }
        public double PrecionVenta { set { _PrecionVenta = value; } get { return _PrecionVenta; } }
        public double MontoRA { set { _MontoRA = value; } get { return _MontoRA; } }
        public string TipoDeAutonomiaCargoFijo { get; set; }
        public string TieneAutonomia { get; set; }
        public double cargoFijoBase { get; set; }
        public string Equipo { get; set; }
        public double CostoEquipo { set { _CostoEquipo = value; } get { return _CostoEquipo; } } //EMMH
        public double TopeMonto { set { _TopeMonto = value; } get { return _TopeMonto; } } //EMMH
        public string IdListaPrecio { set { _IdListaPrecio = value; } get { return _IdListaPrecio; } } //EMMH
        public string DesListaPrecio { set { _DesListaPrecio = value; } get { return _DesListaPrecio; } } //EMMH
        public string ServiciosAdicionales { get; set; }
        //FIN PROY-30748
        public string ejecucionConsultaPrevia { get; set; } //PROY-140335 RF1

        public string mostrarMotivoDeRestriccion { get; set; } //PROY-140579
        public string motivoDeRestriccion { get; set; } //PROY-140579
    }
}
