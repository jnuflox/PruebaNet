using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.AfiliaciónDEAUAsistidosWSRest.EnviaLink
{
    [DataContract]
    [Serializable]
    public class MessageResponseEnviaLinkDEAU
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersResponse header { get; set; }
        [DataMember(Name = "Body")]
        public BodyResponseEnviaLinkDEAU body { get; set; }

        public MessageResponseEnviaLinkDEAU()
        {
            header = new DataPowerRest.HeadersResponse();
            body = new BodyResponseEnviaLinkDEAU();
        }
    }
}
