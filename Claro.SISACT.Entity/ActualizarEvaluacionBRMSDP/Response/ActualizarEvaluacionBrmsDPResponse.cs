using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ActualizarEvaluacionBRMSDP.Response
{
    [DataContract]
    [Serializable] //PROY-140579 - BRMS
    public class ActualizarEvaluacionBrmsDPResponse
    {
        [DataMember(Name = "MessageResponse")]
        public ActualizarEvaluacionBrmsMsgResponse MessageResponse { get; set; }

        public ActualizarEvaluacionBrmsDPResponse()
        {
            MessageResponse = new ActualizarEvaluacionBrmsMsgResponse();
        }
    }
}
