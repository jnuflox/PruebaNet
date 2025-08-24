using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.RegistrarEvaluacionBRMSDP.Response
{
    [DataContract]
    [Serializable] //PROY-140579 - BRMS
    public class RegistrarEvaluacionBRMSDPResponse
    {
        [DataMember(Name = "MessageResponse")]
        public RegistrarEvaluacionBRMSDPMessageResponse MessageResponse { get; set; }

        public RegistrarEvaluacionBRMSDPResponse()
        {
            MessageResponse = new RegistrarEvaluacionBRMSDPMessageResponse();
        }
    }
}
