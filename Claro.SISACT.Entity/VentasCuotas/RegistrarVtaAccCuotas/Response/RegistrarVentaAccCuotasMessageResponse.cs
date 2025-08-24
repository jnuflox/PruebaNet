using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.VentasCuotas.RegistrarVtaAccCuotas.Response
{
    [DataContract]
    [Serializable]
    public class RegistrarVentaAccCuotasMessageResponse
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersResponse Header { get; set; }

        [DataMember(Name = "Body")]
        public RegistrarVentaAccCuotasBodyResponse Body { get; set; }

        public RegistrarVentaAccCuotasMessageResponse()
        {
            Header = new DataPowerRest.HeadersResponse();
            Body = new RegistrarVentaAccCuotasBodyResponse();
        }

    }
}
