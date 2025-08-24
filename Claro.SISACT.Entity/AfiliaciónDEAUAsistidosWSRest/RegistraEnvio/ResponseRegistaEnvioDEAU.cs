using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.AfiliaciónDEAUAsistidosWSRest.RegistraEnvio
{
    [DataContract]
    [Serializable]
    public class ResponseRegistaEnvioDEAU
    {
        [DataMember(Name = "MessageResponse")]
        public MessageResponseRegistraEnvioDEAU MessageResponse { get; set; }

        public ResponseRegistaEnvioDEAU() {
            MessageResponse = new MessageResponseRegistraEnvioDEAU();
        }
    }
}
