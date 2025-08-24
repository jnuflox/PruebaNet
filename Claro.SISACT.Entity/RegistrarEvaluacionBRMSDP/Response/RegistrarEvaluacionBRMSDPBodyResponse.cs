using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.RegistrarEvaluacionBRMSDP.Response
{
    [DataContract]
    [Serializable] //PROY-140579 - BRMS
    public class RegistrarEvaluacionBRMSDPBodyResponse
    {
        [DataMember(Name = "registrarEvaluacionBRMSResponse")]
        public RegistrarEvaluacionBRMSResponse registrarEvaluacionBRMSResponse { get; set; }

        [DataMember(Name = "resultado")]
        public string resultado { get; set; }

        [DataMember(Name = "claroFault")]
        public ClaroFault claroFault { get; set; }

        public RegistrarEvaluacionBRMSDPBodyResponse()
        {
            registrarEvaluacionBRMSResponse = new RegistrarEvaluacionBRMSResponse();
            resultado = string.Empty;
            claroFault = new ClaroFault();
        }
    }
}
