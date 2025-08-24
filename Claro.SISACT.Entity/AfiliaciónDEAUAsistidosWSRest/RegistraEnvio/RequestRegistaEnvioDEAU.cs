using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.AfiliaciónDEAUAsistidosWSRest.RegistraEnvio
{
    [DataContract]
    [Serializable]
    public class RequestRegistaEnvioDEAU
    {
         [DataMember(Name = "MessageRequest")]
        public MessageRequestRegistraEnvioDEAU MessageRequest { get; set; }

         public RequestRegistaEnvioDEAU()
        {
            MessageRequest = new MessageRequestRegistraEnvioDEAU();
        }
    }
}
