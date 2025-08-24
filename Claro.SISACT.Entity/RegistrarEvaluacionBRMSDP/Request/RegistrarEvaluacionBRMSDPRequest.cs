using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.RegistrarEvaluacionBRMSDP.Request
{
    [DataContract]
    [Serializable]
    public class RegistrarEvaluacionBRMSDPRequest
    {
        [DataMember(Name = "MessageRequest")]
        public RegistrarEvaluacionBRMSDPMessageRequest MessageRequest { get; set; }
        public RegistrarEvaluacionBRMSDPRequest ()
        {
            MessageRequest = new RegistrarEvaluacionBRMSDPMessageRequest();
        }
    }
}
