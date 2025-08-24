using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ConsultarClienteFullClaroRest
{
    [DataContract]
    [Serializable]
    public class MessageResponseConsClienteFC
    {
        [DataMember(Name = "Body")]
        public BodyResponseConsClienteFC body { get; set; }

        public MessageResponseConsClienteFC()
        {
            body = new BodyResponseConsClienteFC();
        }
    }
}
