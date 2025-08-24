using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    #region [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil]
    [Serializable]
    public class BEConsultaStock
    {
        public string CodigoMaterial { get; set; }
        public string DescripcionMaterial { get; set; }
        public int StockVenta { get; set; }
        public string TipoMaterial { get; set; }
        public string CodigOficina { get; set; }
        public string DescripcionOficina { get; set; }
        public decimal PrecioBase { get; set; }
        public decimal PrecioCompra { get; set; }
    }
    #endregion
}
