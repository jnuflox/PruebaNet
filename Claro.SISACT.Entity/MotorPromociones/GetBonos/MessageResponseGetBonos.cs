using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Claro.SISACT.Entity.MotorPromociones.GetBonos
{
    [Serializable]
    [DataContract]
    public class MessageResponseGetBonos
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersResponse header { get; set; }

        [DataMember(Name = "Body")]
        public BodyResponseGetBonos body { get; set; }

        public MessageResponseGetBonos()
        {
            header = new DataPowerRest.HeadersResponse();
            body = new BodyResponseGetBonos();
        }
    }
}
