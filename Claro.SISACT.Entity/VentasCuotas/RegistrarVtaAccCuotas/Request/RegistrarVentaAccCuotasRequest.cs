using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.VentasCuotas.RegistrarVtaAccCuotas.Request
{
    [DataContract]
    [Serializable]
    public class RegistrarVentaAccCuotasRequest
    {

        [DataMember(Name = "MessageRequest")]
        public RegistrarVentaAccCuotasMessageRequest MessageRequest { get; set; }

        public RegistrarVentaAccCuotasRequest()
        {
            MessageRequest = new RegistrarVentaAccCuotasMessageRequest();
        }

    }
}
