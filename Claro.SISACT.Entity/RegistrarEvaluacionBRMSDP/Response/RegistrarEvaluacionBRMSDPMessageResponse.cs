using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.RegistrarEvaluacionBRMSDP.Response
{
    [DataContract]
    [Serializable] //PROY-140579 - BRMS
    public class RegistrarEvaluacionBRMSDPMessageResponse
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersResponse header { get; set; }

        [DataMember(Name = "Body")]
        public RegistrarEvaluacionBRMSDPBodyResponse body { get; set; }

        public RegistrarEvaluacionBRMSDPMessageResponse()
        {
            header = new DataPowerRest.HeadersResponse();
            body = new RegistrarEvaluacionBRMSDPBodyResponse();
        }
    }
}
