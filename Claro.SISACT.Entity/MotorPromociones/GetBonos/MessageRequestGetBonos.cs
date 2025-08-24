using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Claro.SISACT.Entity.MotorPromociones.GetBonos
{
    [Serializable]
    [DataContract]
    public class MessageRequestGetBonos
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersRequest Header { get; set; }

        [DataMember(Name = "Body")]
        public BodyRequestGetBonos Body { get; set; }

        public MessageRequestGetBonos()
        {
            Header = new DataPowerRest.HeadersRequest();
            Body = new BodyRequestGetBonos();
        }
    }
}
