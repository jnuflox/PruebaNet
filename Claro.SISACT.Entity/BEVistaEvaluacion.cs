using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEVistaEvaluacion
    {
        public string exoneraRA { get; set; }
        public string riesgoClaro { get; set; }
        public string comportamientoPago { get; set; }
        public double LCDisponible { get; set; }
        public string rangoLCDisponible { get; set; }
        public string poderes { get; set; }
        public double importeGarantia { get; set; }
        public double cargoFijo { get; set; }
        public string tipoGarantia { get; set; }
        public string planAutonomia { get; set; }
        //PROY-29215 INICIO
        public string nrocuota { get; set; }
        public string formaPago { get; set; }
        //PROY-29215 FIN
        public List<BEOfrecimiento> oOfrecimiento { get; set; }
        public List<BEGarantia> oGarantia { get; set; }
        public List<BEItemMensaje> oMensaje { get; set; }
        public List<BEPlan> ListProac { get; set; } //PROY-30748
        public int NroCuotaProac { get; set; } //PROY-30748
        public int cantidadDePlanesPorProducto { get; set; } //PROY-30748
        public double creditScore { get; set; }  //PROY-30748
        public string flagEjecucionConsultaPrevia { get; set; } //PROY-140335 RF1
		//INICIO PROY-140546
        public double montoAnticipadoInstalacion { get; set; }
        public string tipoCobroAnticipadoInstalacion { get; set; }
        //FIN PROY-140546        
    }
}
