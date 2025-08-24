using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.RegistrarOmisionPINRest
{
    [DataContract]
    [Serializable]
    public class RequestRegistrarOmisionPIN
    {
        [DataMember(Name = "MessageRequest")]
        public MessageRequestRegistrarOmisionPIN MessageRequest { get; set; }

        public RequestRegistrarOmisionPIN()
        {
            MessageRequest = new MessageRequestRegistrarOmisionPIN();
        }
    }
}
