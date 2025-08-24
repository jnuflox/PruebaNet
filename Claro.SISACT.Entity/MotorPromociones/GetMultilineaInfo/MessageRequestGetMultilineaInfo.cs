using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Claro.SISACT.Entity.MotorPromociones.GetMultilineaInfo
{
    [Serializable]
    [DataContract]
    public class MessageRequestGetMultilineaInfo
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersRequest Header { get; set; }

        [DataMember(Name = "Body")]
        public BodyRequestGetMultilineaInfo Body { get; set; }

        public MessageRequestGetMultilineaInfo()
        {
            Header = new DataPowerRest.HeadersRequest();
            Body = new BodyRequestGetMultilineaInfo();
        }
    }
}
