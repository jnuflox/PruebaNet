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
    public class ConsultarClienteFullClaroRequest
    {
        [DataMember(Name = "tipoDocumento")]
        public string tipoDocumento { get; set; }
        [DataMember(Name = "nroDocumento")]
        public string nroDocumento { get; set; }
    }
}
