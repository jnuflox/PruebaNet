using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.RegistrarOmisionPINRest
{
    [DataContract]
    [Serializable]
    public class MessageResponseRegistrarOmisionPIN
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersResponse header { get; set; }

        [DataMember(Name = "Body")]
        public BodyResponseRegistrarOmisionPIN body { get; set; }

        public MessageResponseRegistrarOmisionPIN()
        {
            header = new DataPowerRest.HeadersResponse();
            body = new BodyResponseRegistrarOmisionPIN();
        }
    }
}
