using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ConsultarClienteFullClaroRest
{
    //PROY-FULLCLARO
    [DataContract]
    [Serializable]
    public class ConsultarClientesFullClaroRequest
    {
        [DataMember(Name = "MessageRequest")]
        public MessageResquestConsClienteFC MessageRequest { get; set; }

        public ConsultarClientesFullClaroRequest()
        {
            MessageRequest = new MessageResquestConsClienteFC();
        }
    }
}
