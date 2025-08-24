using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Claro.SISACT.Entity.RegistrarEvaluacionBRMSDP.Response;
using Claro.SISACT.Entity.GenericRequestResponse;

namespace Claro.SISACT.Entity.VentasCuotas.RegistrarVariablesBRMS.Response
{
    [DataContract]
    [Serializable]
    public class RegistrarVariablesTypeResponse
    {
        [DataMember(Name = "responseStatus")]
        public ResponseStatus responseStatus { get; set; }

          [DataMember(Name = "listaOpcional")]
        public List<BEListaOpcional> listaOpcional { get; set; }


        public RegistrarVariablesTypeResponse()
        {
            responseStatus = new ResponseStatus();
            listaOpcional = new List<BEListaOpcional>();
        }

    }
}
