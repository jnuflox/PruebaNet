using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.RegistrarEvaluacionBRMSDP.Response
{
    [DataContract]
    [Serializable] //PROY-140579 - BRMS
    public class RegistrarEvaluacionBRMSResponse
    {
        [DataMember(Name = "responseStatus")]
        public ResponseStatus responseStatus { get; set; }

        public RegistrarEvaluacionBRMSResponse()
        {
            responseStatus = new ResponseStatus();
        }

    }
}
