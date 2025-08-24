using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.VentasCuotas.RegistrarVtaAccCuotas.Response
{
    [DataContract]
    [Serializable]
    public class RegistrarVentaAccCuotasBodyResponse
    {
        [DataMember(Name = "ventaAccesorioResponse")]
        public RegistrarVentaAccCuotasTypeResponse ventaAccesorioResponse { get; set; }

        public RegistrarVentaAccCuotasBodyResponse()
        {
            ventaAccesorioResponse = new RegistrarVentaAccCuotasTypeResponse();
        }

    }
}
