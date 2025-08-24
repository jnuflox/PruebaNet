using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ActualizarEvaluacionBRMSDP.Request
{
    [DataContract]
    [Serializable] //PROY-140579 - BRMS
    public class ActualizarEvaluacionBrmsMsgRequest
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersRequest header { get; set; }
        [DataMember(Name = "Body")]
        public ActualizarEvaluacionBrmsBodyRequest body { get; set; }

        public ActualizarEvaluacionBrmsMsgRequest()
        {
            header = new DataPowerRest.HeadersRequest();
            body = new ActualizarEvaluacionBrmsBodyRequest();
        }
    }
}
