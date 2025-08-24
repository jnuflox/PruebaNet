using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.VentasCuotas.ObtenerDatosPedidoAccCuotas.Response
{
    [DataContract]
    [Serializable]
    public class ObtenerDatosPedidoAccCuotasMessageResponse
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersResponse Header { get; set; }

        [DataMember(Name = "Body")]
        public ObtenerDatosPedidoAccCuotasBodyResponse Body { get; set; }

        public ObtenerDatosPedidoAccCuotasMessageResponse()
        {
            Header = new DataPowerRest.HeadersResponse();
            Body = new ObtenerDatosPedidoAccCuotasBodyResponse();
        }
    }
}
