using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Claro.SISACT.Entity.MotorPromociones.GetBonos
{
    [Serializable]
    [DataContract]
    public class GetBonosRequest
    {
        [DataMember(Name = "MessageRequest")]
        public MessageRequestGetBonos MessageRequest { get; set; }

        public GetBonosRequest()
        {
            MessageRequest = new MessageRequestGetBonos();
        }
    }
}
