using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.VentasCuotas.ObtenerDatosPedidoAccCuotas.Response
{
    [DataContract]
    [Serializable]
    public class ObtenerDatosPedidoAccCuotasBodyResponse
    {
         [DataMember(Name = "datosPedidoResponse")]
        public ObtenerDatosPedidoAccCuotasTypeResponse datosPedidoResponse { get; set; }

        public ObtenerDatosPedidoAccCuotasBodyResponse()
        {
            datosPedidoResponse = new ObtenerDatosPedidoAccCuotasTypeResponse();
        }
        
    }
}
