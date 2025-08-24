using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.AfiliaciónDEAUAsistidosWSRest.EnviaLink
{
    [DataContract]
    [Serializable]
    public class ResponseEnviaLinkDEAU
    {
         [DataMember(Name = "MessageResponse")]
        public MessageResponseEnviaLinkDEAU MessageResponse { get; set; }

         public ResponseEnviaLinkDEAU()
        {
            MessageResponse = new MessageResponseEnviaLinkDEAU();
        }
    }
}
