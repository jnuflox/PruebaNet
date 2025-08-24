using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ActualizarEvaluacionBRMSDP.Request
{
    [DataContract]
    [Serializable] //PROY-140579 - BRMS
    public class ActualizarEvaluacionBrmsRequest
    {
        [DataMember(Name = "codigoIOBHN")]
        public string codigoIOBHN { get; set; }

        [DataMember(Name = "codigoSolin")]
        public string codigoSolin { get; set; }
    }
}
