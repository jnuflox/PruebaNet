using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity.DataPowerRest.Generic;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claro_vent_pedidostienda.Response
{
    [DataContract]
    [Serializable]
    public class SolicitudExcepcionPrecioResponseBody : IBodyResponse
    {
        [DataMember(Name = "auditResponse")]
        public AuditResponse auditResponse { get; set; }

        public SolicitudExcepcionPrecioResponseBody()
        {

        }
    }
}
