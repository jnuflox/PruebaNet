using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Claro.SISACT.Entity.RegistrarEvaluacionBRMSDP.Response;
using Claro.SISACT.Entity.GenericRequestResponse;

namespace Claro.SISACT.Entity.VentasCuotas.RegistrarVtaAccCuotas.Response
{
    [DataContract]
    [Serializable]
    public class RegistrarVentaAccCuotasTypeResponse
    {
        [DataMember(Name = "responseStatus")]
        public ResponseStatus responseStatus { get; set; }

          [DataMember(Name = "listaOpcional")]
        public List<BEListaOpcional> listaOpcional { get; set; }


          public RegistrarVentaAccCuotasTypeResponse()
        {
            responseStatus = new ResponseStatus();
            listaOpcional = new List<BEListaOpcional>();
        }
    }
}
