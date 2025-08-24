using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.RegistrarEvaluacionBRMSDP.Request
{
    [DataContract]
    [Serializable]
    public class RegistrarEvaluacionBRMSDPMessageRequest //PROY-140579 - BRMS
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersRequest header { get; set; }
        [DataMember(Name = "Body")]
        public RegistrarEvaluacionBRMSDPBodyRequest body { get; set; }

        public RegistrarEvaluacionBRMSDPMessageRequest()
        {
            header = new DataPowerRest.HeadersRequest();
            body = new RegistrarEvaluacionBRMSDPBodyRequest();
        }
    }
}
