using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ActualizarEvaluacionBRMSDP.Response
{
    [DataContract]
    [Serializable] //PROY-140579 - BRMS
    public class ActualizarEvaluacionBrmsMsgResponse
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersResponse header { get; set; }

        [DataMember(Name = "Body")]
        public ActualizarEvaluacionBrmsBodyResponse body { get; set; }

        public ActualizarEvaluacionBrmsMsgResponse()
        {
            header = new DataPowerRest.HeadersResponse();
            body = new ActualizarEvaluacionBrmsBodyResponse();
        }
    }
}
