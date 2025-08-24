using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.VentasCuotas.ValidarCuoAccPendXLinea.Request
{
    [DataContract]
    [Serializable]
    public class ValdCuoAccPendXLineaMessageRequest
    {
         [DataMember(Name = "Header")]
        public DataPowerRest.Generic.HeadersRequest Header { get; set; }

        [DataMember(Name = "Body")]
        public ValdCuoAccPendXLineaBodyRequest Body { get; set; }

        public ValdCuoAccPendXLineaMessageRequest()
        {
            Header = new DataPowerRest.Generic.HeadersRequest();
            Body = new ValdCuoAccPendXLineaBodyRequest();
        }

    }
}
