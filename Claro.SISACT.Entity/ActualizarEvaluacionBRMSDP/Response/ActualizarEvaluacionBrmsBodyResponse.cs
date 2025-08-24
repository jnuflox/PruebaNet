using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ActualizarEvaluacionBRMSDP.Response
{
    [DataContract]
    [Serializable] //PROY-140579 - BRMS
    public class ActualizarEvaluacionBrmsBodyResponse
    {
        [DataMember(Name = "actualizarBRMSResponse")]
        public ActualizarBRMSResponse actualizarBRMSResponse { get; set; }

        [DataMember(Name = "claroFault")]
        public ClaroFault claroFault { get; set; }

        public ActualizarEvaluacionBrmsBodyResponse()
        {
            actualizarBRMSResponse = new ActualizarBRMSResponse();
            claroFault = new ClaroFault();
        }

    }
}
