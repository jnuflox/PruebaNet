using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEGarantia
    {
        public int IdFila { get; set; }
        public string idGarantia { get; set; }
        public string garantia { get; set; }
        public double nroGarantia { get; set; }
        public double importe { get; set; }
        public string idProducto { get; set; }
        public string producto { get; set; }
        public double CF { get; set; }

        public string idPlan { get; set; }
        public string plan { get; set; }
    }
}
