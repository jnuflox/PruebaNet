using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.RegistrarOmisionPINRest
{
    [DataContract]
    [Serializable]
    public class BodyResponseRegistrarOmisionPIN
    {
        [DataMember(Name = "responseAudit")]
        public BEResponseAudit responseAudit { get; set; }

        public BodyResponseRegistrarOmisionPIN()
        {
            responseAudit = new BEResponseAudit();
        }
    }
}
