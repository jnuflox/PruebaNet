using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Claro.SISACT.Entity.MotorPromociones.GetMultilineaInfo
{
    [Serializable]
    [DataContract]
    public class GetMultilineaInfoRequest
    {
        [DataMember(Name = "MessageRequest")]
        public MessageRequestGetMultilineaInfo MessageRequest { get; set; }

        public GetMultilineaInfoRequest()
        {
            MessageRequest = new MessageRequestGetMultilineaInfo();
        }
    }
}
