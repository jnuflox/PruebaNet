using System;
using System.Collections;
using System.Collections.Generic;

namespace Claro.SISACT.Entity
{
	/// <summary>
	/// Descripción breve de PlanSolHFC.
	/// </summary>
    [Serializable] //PROY-140126 - IDEA 140248 
	public class BEPlanSolicitudHFC
	{
        public Int64 IdSolucion { get; set; }
        public string Solucion { get; set; }
        public Int64 IdPaquete { get; set; }
        public string Paquete { get; set; }
        public string IdPlazo { get; set; }
        public string Plazo { get; set; }
        public string IdCampana { get; set; }
        public string Campana { get; set; }
        public string IdPlan { get; set; }
        public string Plan { get; set; }
        public string Usuario { get; set; }
        public List<BEPlanDetalleHFC> oServicio { get; set; }
        public ArrayList oPromocion { get; set; }
        public List<BEPlanEquipoHFC> oEquipo { get; set; }
        public string Telefono { get; set; }
        //PROY-29215 INICIO
        public  int NroCuota{ get; set; }
        public  string FormaPago { get; set; }
        //PROY-29215 FIN
	}
}
