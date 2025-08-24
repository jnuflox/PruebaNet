using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claroventacobertura.validarcobertura
{
    [DataContract]
    [Serializable]
    public class RequestValidarCobertura
    {
        [DataMember(Name = "MessageRequest")]
        public MessageRequestValidarCobertura MessageRequest { get; set; }

        public RequestValidarCobertura()
        {
            MessageRequest = new MessageRequestValidarCobertura();
        }
    }
}
