using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ActualizarEvaluacionBRMSDP.Request
{
    [DataContract]
    [Serializable] //PROY-140579 - BRMS
    public class ActualizarEvaluacionBrmsBodyRequest
    {
        [DataMember(Name = "actualizarEvaluacionRequest")]
        public ActualizarEvaluacionBrmsRequest actualizarEvaluacionRequest { get; set; }

        public ActualizarEvaluacionBrmsBodyRequest()
        {
            actualizarEvaluacionRequest = new ActualizarEvaluacionBrmsRequest();
        }
    }
}
