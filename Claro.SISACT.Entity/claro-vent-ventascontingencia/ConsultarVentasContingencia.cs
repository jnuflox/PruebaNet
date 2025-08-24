using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Claro.SISACT.Entity.DataPowerRest.Generic;

namespace Claro.SISACT.Entity.claro_vent_ventascontingencia
{
    [DataContract]
    [Serializable]
    public class ConsultarVentasContingencia
    {
        [DataMember(Name = "auditResponse")]
        public AuditResponse auditResponse { get; set; }

        [DataMember(Name = "ventasContingencia")]
        public List<BEVentasContingencia> ventasContingencia { get; set; }

        [DataMember(Name = "responseOpcional")]
        public List<BEOpcional> responseOpcional { get; set; }

        public ConsultarVentasContingencia()
        {
            auditResponse = new AuditResponse();
            ventasContingencia = new List<BEVentasContingencia>();
            responseOpcional = new List<BEOpcional>();
        }
    }
}
