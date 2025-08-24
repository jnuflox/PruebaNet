using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Claro.SISACT.Entity.MotorPromociones.GetMultilineaInfo
{
    [Serializable]
    [DataContract]
    public class MessageResponseGetMultilineaInfo
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersResponse header { get; set; }

        [DataMember(Name = "Body")]
        public BodyResponseGetMultilineaInfo body { get; set; }

        public MessageResponseGetMultilineaInfo()
        {
            header = new DataPowerRest.HeadersResponse();
            body = new BodyResponseGetMultilineaInfo();
        }
    }
}
