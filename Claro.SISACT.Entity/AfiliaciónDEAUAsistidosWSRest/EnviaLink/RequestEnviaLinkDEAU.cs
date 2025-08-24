using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.AfiliaciónDEAUAsistidosWSRest.EnviaLink
{
    [DataContract]
    [Serializable]
    public class RequestEnviaLinkDEAU
    {
        [DataMember(Name = "MessageRequest")]
        public MessageRequestEnvioLinkDebAuto MessageRequest { get; set; }

        public RequestEnviaLinkDEAU()
        {
            MessageRequest = new MessageRequestEnvioLinkDebAuto();
        }
    }
}
