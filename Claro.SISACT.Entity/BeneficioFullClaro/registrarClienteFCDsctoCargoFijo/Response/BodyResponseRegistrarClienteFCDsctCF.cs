using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.BeneficioFullClaro.registrarClienteFCDsctoCargoFijo.Response
{
    [DataContract]
    [Serializable]
    public class BodyResponseRegistrarClienteFCDsctCF
    {
        [DataMember(Name = "PO_COD_RESULTADO")]
        public string PO_COD_RESULTADO { get; set; }

        [DataMember(Name = "PO_MSJ_RESULTADO")]
        public string PO_MSJ_RESULTADO { get; set; }
    }
}
