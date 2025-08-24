using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEDescuento
    {
        public string idCombo { get; set; }
        public string idProducto { get; set; }
        public string tipoDescuento { get; set; }
        public double montoDescuento { get; set; }
        public int mesesAplicacion { get; set; }

        public enum TIPO_DESCUENTO
        {
            DSCTO_MONTO = 1,
            DSCTO_PORCENTAJE = 2
        }
    }
}
