using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BETopeConsumo
    {

        public string ID { get; set; }
        public string CodigoProducto { get; set; }
        public string CodigoServicio { get; set; }
        public string CodigoPlan { get; set; }
        public string DescripcionServicio { get; set; }
        public double Monto { get; set; }
        public string Minuto { get; set; }
    }
}
