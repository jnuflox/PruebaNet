using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEPlanProactivo
    {
        public List<BEPlan> Planes { get; set; }
        public List<BEPlanBSSEval> PlanBSSEval { get; set; }
        public int NroCuota { get; set; }
        public int TotalPlanes { get; set; }
        public int BSSEvalTotalPlanes { get; set; }
        public double CFServAdic { get; set; }
        public string ServAdic { get; set; }
        public BEPlanProactivo()
        {
            Planes = new List<BEPlan>();
        }
        public string CadenaDatos { get; set; }
        public string CadenaEquipo { get; set; }
        public string CadenaPlan { get; set; }
        public string CadenaServicio { get; set; }
        public string idtipoOper { get; set; }
        public string idmodventa { get; set; }
        public string familia { get; set; }
        public int cantidadDePlanesPorProducto { get; set; }
        public double creditScore { get; set; }
        public string PuntodeDespacho { get; set; }
        public string idFLujo { get; set; }


    }
}
