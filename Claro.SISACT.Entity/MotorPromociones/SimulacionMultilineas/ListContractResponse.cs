using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Claro.SISACT.Entity.MotorPromociones.SimulacionMultilineas
{
    [Serializable]
    [DataContract]
    public class ListContractResponse
    {
        [DataMember(Name = "promotionCode")]
        public string promotionCode { get; set; }

        [DataMember(Name = "msisdn")]
        public string msisdn { get; set; }

        [DataMember(Name = "plan")]
        public string plan { get; set; }

        public ListContractResponse()
        {
            promotionCode = string.Empty;
            msisdn = string.Empty;
            plan = string.Empty;
        }
    }
}
