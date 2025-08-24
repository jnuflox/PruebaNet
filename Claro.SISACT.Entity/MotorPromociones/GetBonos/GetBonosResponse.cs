using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Claro.SISACT.Entity.MotorPromociones.GetBonos
{
    [Serializable]
    [DataContract]
    public class GetBonosResponse
    {
        [DataMember(Name = "MessageResponse")]
        public MessageResponseGetBonos MessageResponse { get; set; }

        public GetBonosResponse()
        {
            MessageResponse = new MessageResponseGetBonos();
        }
    }
}
