using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claroventacobertura.validarcobertura
{
    [DataContract]
    [Serializable]
    public class ResponseValidarCobertura
    {
        [DataMember(Name = "MessageResponse")]
        public MessageResponseValidarCobertura MessageResponse { get; set; }

        public ResponseValidarCobertura()
        {
            MessageResponse = new MessageResponseValidarCobertura();
        }
    }
}
