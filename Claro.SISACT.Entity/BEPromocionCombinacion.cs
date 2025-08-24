using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    #region [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil]
    [Serializable]
    public class BEPromocionCombinacion
    {
        public int PCOMN_CODIGO_COMBINACION { get; set; }
        public int PCOMN_CODIGO_PROMO { get; set; }
        public string PCOMV_COD_MAT_EQUIPO { get; set; }
        public string PCOMV_COD_PLAN { get; set; }
        public string PCOMV_COD_MAT_ACCESORIO { get; set; }
        public string PCOMV_COD_TIPO_PROD { get; set; } // Campo para enviar al servicio de Datos SOT
    }
    #endregion
}
