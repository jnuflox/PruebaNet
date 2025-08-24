using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Claro.SISACT.Entity.MotorPromociones.SimulacionMultilineas
{
    [Serializable]
    [DataContract]
    public class BodyResponseSimulacionMultilineas
    {
        [DataMember(Name = "result")]
        public ResultResponse result { get; set; }

        [DataMember(Name = "tipoDocumento")]
        public string tipoDocumento { get; set; }

        [DataMember(Name = "numeroDocumento")]
        public string numeroDocumento { get; set; }

        [DataMember(Name = "responseAudit")]
        public ResponseAudit responseAudit { get; set; }

        public BodyResponseSimulacionMultilineas()
        {
            result = new ResultResponse();
            tipoDocumento = string.Empty;
            numeroDocumento = string.Empty;
            responseAudit = new ResponseAudit();

        }
    }
}
