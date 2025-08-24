using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Claro.SISACT.Entity.MotorPromociones.GetMultilineaInfo
{
    [Serializable]
    [DataContract]
    public class BodyResponseGetMultilineaInfo
    {
        [DataMember(Name = "result")]
        public List<ListResultResponseG> result { get; set; }

        [DataMember(Name = "responseAudit")]
        public ResponseAudit responseAudit { get; set; }

        [DataMember(Name = "tipoDocumento")]
        public string tipoDocumento { get; set; }

        [DataMember(Name = "numeroDocumento")]
        public string numeroDocumento { get; set; }

        public BodyResponseGetMultilineaInfo()
        {
            result = new List<ListResultResponseG>();
            responseAudit = new ResponseAudit();
            tipoDocumento = string.Empty;
            numeroDocumento = string.Empty;
        }
    }
}
