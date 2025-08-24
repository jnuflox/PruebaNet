using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.RegistrarEvaluacionBRMSDP.Request
{
    [DataContract]
    [Serializable]
    public class RegistrarEvaluacionBRMSDPBodyRequest //PROY-140579 - BRMS
    {
        [DataMember(Name = "registrarDatosRequest")]
        public RegistrarEvaluacionBrmsRequest registrarDatosRequest { get; set; }
    }
}
