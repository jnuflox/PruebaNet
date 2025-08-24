using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable]
    public class BEFullClaroBeneficio
    {
        public BEFullClaroBeneficio() { }
        public string IdCandidato { get; set; }
        public string EstadoCandidato { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string CodigoProducto { get; set; }
        public string TipoOperacion { get; set; }
        public string Linea { get; set; }
        public string NumeroSEC { get; set; }

        public string NumeroContrato { get; set; }//INC000004280198
        public string FechaContrato { get; set; }//INC000004280198
        public string Pedido { get; set; }//INC000004280198
    }
}
