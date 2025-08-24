using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.VentasCuotas.RegistrarVtaAccCuotas.Request
{
    [DataContract]
    [Serializable]
    public class RegistrarVentaAccCuotasBodyRequest
    {
        [DataMember(Name = "ventaAccesorioRequest")]
        public RegistrarVentaAccCuotasTypeRequest ventaAccesorioRequest { get; set; }

        public RegistrarVentaAccCuotasBodyRequest()
        {
            ventaAccesorioRequest = new RegistrarVentaAccCuotasTypeRequest();
        }
    }

}
