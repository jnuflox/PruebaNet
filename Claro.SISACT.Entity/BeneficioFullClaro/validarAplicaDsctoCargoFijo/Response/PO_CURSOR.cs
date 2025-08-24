using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.BeneficioFullClaro.validarAplicaDsctoCargoFijo.Response
{
    [DataContract]
    [Serializable]
    public class PO_CURSOR
    {
        [DataMember(Name = "PO_CURSOR_Row")]
        public List<PO_CURSOR_Row> PO_CURSOR_Row { get; set; }

        public PO_CURSOR()
        {
            PO_CURSOR_Row = new List<PO_CURSOR_Row>();
        }
    }
}
