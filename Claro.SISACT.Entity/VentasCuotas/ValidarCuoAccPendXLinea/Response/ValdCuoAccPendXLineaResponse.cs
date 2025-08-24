using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.VentasCuotas.ValidarCuoAccPendXLinea.Response
{
    [DataContract]
    [Serializable]
    public class ValdCuoAccPendXLineaResponse
    {
        [DataMember(Name = "MessageResponse")]
        public ValdCuoAccPendXLineaMessageResponse MessageResponse { get; set; }

        public ValdCuoAccPendXLineaResponse()
        {
            MessageResponse = new ValdCuoAccPendXLineaMessageResponse();
        }
    }
}
