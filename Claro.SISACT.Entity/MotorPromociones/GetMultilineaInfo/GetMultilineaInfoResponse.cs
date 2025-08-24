using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Claro.SISACT.Entity.MotorPromociones.GetMultilineaInfo
{
    [Serializable]
    [DataContract]
    public class GetMultilineaInfoResponse
    {
        [DataMember(Name = "MessageResponse")]
        public MessageResponseGetMultilineaInfo MessageResponse { get; set; }

        public GetMultilineaInfoResponse()
        {
            MessageResponse = new MessageResponseGetMultilineaInfo();
        }
    }
}
