using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.VentasCuotas.ValidarCuoAccPendXLinea.Request
{
    [DataContract]
    [Serializable]
    public class ValdCuoAccPendXLineaBodyRequest
    {
        [DataMember(Name = "bodyRequest")]
        public ValdCuoAccPendXLineaTypeRequest bodyRequest { get; set; }

        public ValdCuoAccPendXLineaBodyRequest()
        {
            bodyRequest = new ValdCuoAccPendXLineaTypeRequest();
        }
    }
}
