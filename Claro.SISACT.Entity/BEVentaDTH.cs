using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEVentaDTH
    {
        public Int64 ID_DOCUMENTO { get; set; }
        public string TIPO_DOCUMENTO { get; set; }
        public string CANAL { get; set; }
        public string OFICINA_VENTA { get; set; }
        public DateTime FECHA_CREA { get; set; }
        public string TIPO_DOC_CLIENTE { get; set; }
        public string NRO_DOC_CLIENTE { get; set; }
        public string MONEDA { get; set; }
        public Int64 TOPEN_CODIGO { get; set; }
        public double TOTAL_VENTA { get; set; }
        public double SUBTOTAL_IMPUESTO { get; set; }
        public double SUBTOTAL_VENTA { get; set; }
        public DateTime FECHA_VENTA { get; set; }
        public string OBSERVACION { get; set; }
        public string TVENC_CODIGO { get; set; }
        public string NUMERO_REFERENCIA { get; set; }
        public string USUARIO_CREA { get; set; }
        public Int64 NUMERO_CUOTAS { get; set; }
        public string ESTADO { get; set; }
        public string VENDEDOR { get; set; }
        public string ORG_VENTA { get; set; }
        public Int64 NUMERO_SEC { get; set; }
    }
}
