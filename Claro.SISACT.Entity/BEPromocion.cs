using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    #region [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] 
    [Serializable]
    public class BEPromocion
    {
        public int PACCN_CODIGO_PROMO { get; set; }
        public string PACCV_DESC_PROMO { get; set; }
        public string PACCV_COD_CAMPANA { get; set; }
        public string PACCV_CONCAT_COD_TIP_OPERA { get; set; }
        public string PACCV_CONCAT_COD_TIP_CLIEN { get; set; }
        public string PACCV_CONCAT_COD_CANAL { get; set; }
        public string PACCV_CONCAT_MOD_VENTA { get; set; }
        public int PACCN_VIGENCIA_SEC { get; set; }
    }
    #endregion
}
