using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEPlanPromocionHFC
    {
        public Int64 IDDET { get; set; }
        public Int64 IdProducto { get; set; }
        public Int64 IdLinea { get; set; }
        public Int64 IdPromocion { get; set; }
        public string Promocion { get; set; }
    }
}
