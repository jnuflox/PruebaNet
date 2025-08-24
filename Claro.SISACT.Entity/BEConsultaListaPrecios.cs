using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEConsultaListaPrecios
    {
        public string CodigoListaPrecio { get; set; }
        public string DescripcionListaPrecios { get; set; }
        public double PrecioBase { get; set; }
        public string CodigoMaterial { get; set; }
    }
}
