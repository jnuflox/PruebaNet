using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    public class BEMultipunto
    {

        public string CANAC_CODIGO { get; set; }
        public string TOFIC_CODIGO { get; set; }
        public int N_IDOFICINA { get; set; }
        public string V_OFICINA_ORIGEN { get; set; }
        public string OVENV_DESCRIPCION { get; set; }
        public string OVENC_REGION { get; set; }
        public string C_FLAGEVALUACION { get; set; }
        public string C_FLAGVENTA { get; set; }
        public string C_ESTADO { get; set; }
        public string V_OFICINA_VENTA { get; set; }
    }
}
