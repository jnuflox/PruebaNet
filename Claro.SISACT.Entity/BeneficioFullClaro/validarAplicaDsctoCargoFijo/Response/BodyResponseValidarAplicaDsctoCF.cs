using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.BeneficioFullClaro.validarAplicaDsctoCargoFijo.Response
{
    [DataContract]
    [Serializable]
    public class BodyResponseValidarAplicaDsctoCF
    {
        [DataMember(Name = "PO_CURSOR")]
        public PO_CURSOR PO_CURSOR { get; set; }

        [DataMember(Name = "PO_COD_RESULTADO")]
        public string PO_COD_RESULTADO { get; set; }

        [DataMember(Name = "PO_MSJ_RESULTADO")]
        public string PO_MSJ_RESULTADO { get; set; }

        public BodyResponseValidarAplicaDsctoCF()
        {
            PO_CURSOR = new PO_CURSOR();
        }
    }
}
