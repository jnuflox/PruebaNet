using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Claro.SISACT.Entity.MotorPromociones.GetMultilineaInfo
{
    [Serializable]
    [DataContract]
    public class ListResultResponseG
    {
        [DataMember(Name = "contractStatus")]
        public string contractStatus { get; set; }

        [DataMember(Name = "planDesc")]
        public string planDesc { get; set; }

        [DataMember(Name = "system")]
        public string system { get; set; }

        [DataMember(Name = "cargoFijo")]
        public string cargoFijo { get; set; }

        [DataMember(Name = "coCode")]
        public string coCode { get; set; }

        [DataMember(Name = "coId")]
        public string coId { get; set; }

        [DataMember(Name = "technology")]
        public string technology { get; set; }

        [DataMember(Name = "msisdn")]
        public string msisdn { get; set; }

        [DataMember(Name = "plan")]
        public string plan { get; set; }

        public ListResultResponseG()
        {
            contractStatus = string.Empty;
            planDesc = string.Empty;
            system = string.Empty;
            cargoFijo = string.Empty;
            coCode = string.Empty;
            coId = string.Empty;
            technology = string.Empty;
            msisdn = string.Empty;
            plan = string.Empty;
        }
    }
}
