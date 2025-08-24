using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Claro.SISACT.Entity.ActualizarEvaluacionBRMSDP.Response
{
    [DataContract]
    [Serializable] //PROY-140579 - BRMS
    public class ActualizarBRMSResponse
    {
        [DataMember(Name = "responseStatus")]
        public ResponseStatus responseStatus { get; set; }

        public ActualizarBRMSResponse()
        {
            responseStatus = new ResponseStatus();
        }
    }
}
