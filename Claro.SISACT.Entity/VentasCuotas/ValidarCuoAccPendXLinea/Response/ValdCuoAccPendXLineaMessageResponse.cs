using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.VentasCuotas.ValidarCuoAccPendXLinea.Response
{
    [DataContract]
    [Serializable]
    public class ValdCuoAccPendXLineaMessageResponse
    {

        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersResponse Header { get; set; }

        [DataMember(Name = "Body")]
        public ValdCuoAccPendXLineaBodyResponse Body { get; set; }

        public ValdCuoAccPendXLineaMessageResponse()
        {
            Header = new DataPowerRest.HeadersResponse();
            Body = new ValdCuoAccPendXLineaBodyResponse();
        }

    }
}
