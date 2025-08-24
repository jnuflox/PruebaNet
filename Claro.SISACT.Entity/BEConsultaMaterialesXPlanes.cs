using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
   public class BEConsultaMaterialesXPlanes
    {
        public string CodigoMaterial { get; set; }
        public string CodigoCampania { get; set;}
        public string CodigoPlan { get; set; }
        public string CodigoListaPrecio { get; set; }
        public string DescripcionListaPrecios { get; set; }
    }
}
