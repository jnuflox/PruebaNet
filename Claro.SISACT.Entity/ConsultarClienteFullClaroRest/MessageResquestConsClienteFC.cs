using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ConsultarClienteFullClaroRest
{
    //PROY-FULLCLARO.V2
    [DataContract]
    [Serializable]
    public class MessageResquestConsClienteFC
    {
        
        [DataMember(Name = "Body")]
        public ConsultarClienteFullClaroRequest body { get; set; }

        public MessageResquestConsClienteFC()
        {
            body = new ConsultarClienteFullClaroRequest();
        }

    }
}
