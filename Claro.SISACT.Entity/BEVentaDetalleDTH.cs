using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEVentaDetalleDTH
    {
        public Int64 ID_DOCUMENTO { get; set; }
        public string TELEFONO { get; set; }
        public string MATERIAL { get; set; }
        public string MATERIAL_DESC { get; set; }
        public Int64 CANTIDAD { get; set; }
        public double PRECIO { get; set; }
        public double DESCUENTO { get; set; }
        public double SUBTOTAL { get; set; }
        public double IGV { get; set; }
        public double TOTAL { get; set; }
        public string PLAN { get; set; }
        public string PLAN_DESC { get; set; }
        public string CAMPANA { get; set; }
        public string CAMPANA_DESC { get; set; }
        public string LISTA_PRECIO { get; set; }
        public string LISTA_PRECIO_DESC { get; set; }
        public Int64 ORDEN { get; set; }
        public string IMEI19 { get; set; }
        public string PLAZO { get; set; }		
    }
}
