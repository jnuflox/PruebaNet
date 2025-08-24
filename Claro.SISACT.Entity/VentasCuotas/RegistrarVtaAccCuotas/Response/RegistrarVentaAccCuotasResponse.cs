using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.VentasCuotas.RegistrarVtaAccCuotas.Response
{
    [DataContract]
    [Serializable]
    public class RegistrarVentaAccCuotasResponse
    {
        [DataMember(Name = "MessageResponse")]
        public RegistrarVentaAccCuotasMessageResponse MessageResponse { get; set; }

        public RegistrarVentaAccCuotasResponse()
        {
            MessageResponse = new RegistrarVentaAccCuotasMessageResponse();
        }
    }
}
