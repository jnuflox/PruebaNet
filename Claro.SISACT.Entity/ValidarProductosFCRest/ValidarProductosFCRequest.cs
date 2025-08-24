using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ValidarProductosFCRest
{
    //PROY-FULLCLARO.v2
    [DataContract]
    [Serializable]
    public class ValidarProductosFCRequest
    {
        [DataMember(Name = "MessageRequest")]
        public MessageResquestValidarProductosFC MessageRequest { get; set; }

        public ValidarProductosFCRequest()
        {
            MessageRequest = new MessageResquestValidarProductosFC();
        }
    }
}
