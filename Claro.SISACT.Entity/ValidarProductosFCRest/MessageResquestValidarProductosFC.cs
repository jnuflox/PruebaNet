using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ValidarProductosFCRest
{
    //PROY-FULLCLARO.V2
    [DataContract]
    [Serializable]
    public class MessageResquestValidarProductosFC
    {
        [DataMember(Name = "Body")]
        public ProductosFCRestRequest body { get; set; }

        public MessageResquestValidarProductosFC()
        {
            body = new ProductosFCRestRequest();
        }
    }
}
