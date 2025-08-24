using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Claro.SISACT.Entity.ActualizarEvaluacionBRMSDP.Request
{
    [DataContract]
    [Serializable] //PROY-140579 - BRMS
    public class ActualizarEvaluacionBrmsDPRequest
    {
        [DataMember(Name = "MessageRequest")]
        public ActualizarEvaluacionBrmsMsgRequest MessageRequest { get; set; }
        public ActualizarEvaluacionBrmsDPRequest()
        {
            MessageRequest = new ActualizarEvaluacionBrmsMsgRequest();
        }
    }
}
