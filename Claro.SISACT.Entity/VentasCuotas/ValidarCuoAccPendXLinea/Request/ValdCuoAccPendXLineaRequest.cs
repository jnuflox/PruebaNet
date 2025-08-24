using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.VentasCuotas.ValidarCuoAccPendXLinea.Request
{
    [DataContract]
    [Serializable]
    public class ValdCuoAccPendXLineaRequest
    {
         [DataMember(Name = "MessageRequest")]
        public ValdCuoAccPendXLineaMessageRequest MessageRequest { get; set; }

         public ValdCuoAccPendXLineaRequest()
        {
            MessageRequest = new ValdCuoAccPendXLineaMessageRequest();
        }
    }
}
