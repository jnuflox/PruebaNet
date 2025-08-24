using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.VentasCuotas.ValidarCuoAccPendXLinea.Response
{
    [DataContract]
    [Serializable]
    public class ValdCuoAccPendXLineaBodyResponse
    {
        [DataMember(Name = "bodyResponse")]
        public ValdCuoAccPendXLineaTypeResponse bodyResponse { get; set; }

        public ValdCuoAccPendXLineaBodyResponse()
        {
            bodyResponse = new ValdCuoAccPendXLineaTypeResponse();
        }
    }
}
