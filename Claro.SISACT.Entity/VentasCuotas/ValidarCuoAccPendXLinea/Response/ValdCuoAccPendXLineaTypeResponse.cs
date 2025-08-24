using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Claro.SISACT.Entity.GenericRequestResponse;
using Claro.SISACT.Entity.RegistrarEvaluacionBRMSDP.Response;

namespace Claro.SISACT.Entity.VentasCuotas.ValidarCuoAccPendXLinea.Response
{
    [DataContract]
    [Serializable]
    public class ValdCuoAccPendXLineaTypeResponse
    {
        [DataMember(Name = "responseStatus")]
        public ResponseStatus responseStatus { get; set; }

          [DataMember(Name = "listaOpcional")]
        public List<BEListaOpcional> listaOpcional { get; set; }


          public ValdCuoAccPendXLineaTypeResponse()
        {
            responseStatus = new ResponseStatus();
            listaOpcional = new List<BEListaOpcional>();
        }

    }
}
