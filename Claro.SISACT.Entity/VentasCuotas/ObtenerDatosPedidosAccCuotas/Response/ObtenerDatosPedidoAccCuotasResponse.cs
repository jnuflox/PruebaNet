using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.VentasCuotas.ObtenerDatosPedidoAccCuotas.Response
{

    [DataContract]
    [Serializable]
    public class ObtenerDatosPedidoAccCuotasResponse
    {
         [DataMember(Name = "MessageResponse")]
        public ObtenerDatosPedidoAccCuotasMessageResponse MessageResponse { get; set; }

         public ObtenerDatosPedidoAccCuotasResponse()
        {
            MessageResponse = new ObtenerDatosPedidoAccCuotasMessageResponse();
        }

    }
}
