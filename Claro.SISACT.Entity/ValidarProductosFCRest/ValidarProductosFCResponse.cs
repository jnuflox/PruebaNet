using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ValidarProductosFCRest
{
    [DataContract]
    [Serializable]
    public class ValidarProductosFCResponse
    {
        [DataMember(Name = "MessageResponse")]
        public MessageResponseValidarProductosFC MessageResponse { get; set; }

        public ValidarProductosFCResponse()
        {
            MessageResponse = new MessageResponseValidarProductosFC();
        }
    }
}
