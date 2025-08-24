using System;

namespace Claro.SISACT.Entity
{
	/// <summary>
	/// Descripción breve de PlanSolicitudDTH.
	/// </summary>
    [Serializable] //PROY-140126 - IDEA 140248 
	public class BEPlanSolicitudDTH
	{
        public BEVentaDTH VENTA { get; set; }
        public BEVentaDetalleDTH VENTA_DETALLE { get; set; }
	}
}
