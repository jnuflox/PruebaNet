using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.AfiliaciónDEAUAsistidosWSRest.EnviaLink
{
    [DataContract]
    [Serializable]
    public class MessageRequestEnvioLinkDebAuto
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersRequest header { get; set; }
        [DataMember(Name = "Body")]
        public BodyRequestEnviaLinkDEAU body { get; set; }

        public MessageRequestEnvioLinkDebAuto()
        {
            header = new DataPowerRest.HeadersRequest();
            body = new BodyRequestEnviaLinkDEAU();
        }
    }
}
