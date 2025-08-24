using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.RegistrarEvaluacionBRMSDP.Response
{
    [DataContract]
    [Serializable] //PROY-140579 - BRMS
    public class ClaroFault
    {
        [DataMember(Name = "idAudit")]
        public string idAudit { get; set; }

        [DataMember(Name = "codeError")]
        public string codeError { get; set; }

        [DataMember(Name = "descriptionError")]
        public string descriptionError { get; set; }

        [DataMember(Name = "locationError")]
        public string locationError { get; set; }

        [DataMember(Name = "date")]
        public string date { get; set; }

        [DataMember(Name = "originError")]
        public string originError { get; set; }
    }
}
