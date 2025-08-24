using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Claro.SISACT.Entity.MotorPromociones.GetBonos
{
    [Serializable]
    [DataContract]
    public class ListResultResponse
    {
        [DataMember(Name = "msisdn")]
        public string msisdn { get; set; }

        [DataMember(Name = "bonos")]
        public List<ListBonosResponse> bonos { get; set; }

        public ListResultResponse()
        {
            msisdn = string.Empty;
            bonos = new List<ListBonosResponse>();
        }
    }
}
