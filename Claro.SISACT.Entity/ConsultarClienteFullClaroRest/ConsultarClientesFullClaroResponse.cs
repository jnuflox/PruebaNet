using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ConsultarClienteFullClaroRest
{

    [DataContract]
    [Serializable]
    public class ConsultarClientesFullClaroResponse
    {
        [DataMember(Name = "MessageResponse")]
        public MessageResponseConsClienteFC MessageResponse { get; set; }

        public ConsultarClientesFullClaroResponse()
        {
            MessageResponse = new MessageResponseConsClienteFC();
        }
    }
}
