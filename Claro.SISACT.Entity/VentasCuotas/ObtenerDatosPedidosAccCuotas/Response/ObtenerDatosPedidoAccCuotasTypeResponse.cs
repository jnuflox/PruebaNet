using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Claro.SISACT.Entity.RegistrarEvaluacionBRMSDP.Response;

namespace Claro.SISACT.Entity.VentasCuotas.ObtenerDatosPedidoAccCuotas.Response
{

    [DataContract]
    [Serializable]
    public class ObtenerDatosPedidoAccCuotasTypeResponse
    {
        [DataMember(Name = "responseStatus")]
        public ResponseStatus responseStatus { get; set; }

        
        [DataMember(Name = "responseData")]
        public List<BEObtenerDatosPedidoAccCuotas> responseData { get; set; }
    }
}


    