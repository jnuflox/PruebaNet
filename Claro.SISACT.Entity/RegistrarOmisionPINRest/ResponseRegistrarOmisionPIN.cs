using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.RegistrarOmisionPINRest
{
    [DataContract]
    [Serializable]
    public class ResponseRegistrarOmisionPIN
    {
        [DataMember(Name = "MessageResponse")]
        public MessageResponseRegistrarOmisionPIN MessageResponse { get; set; }

        public ResponseRegistrarOmisionPIN()
        {
            MessageResponse = new MessageResponseRegistrarOmisionPIN();
        }
    }
}
