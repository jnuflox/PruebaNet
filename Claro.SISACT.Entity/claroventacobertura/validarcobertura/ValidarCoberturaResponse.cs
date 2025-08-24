using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claroventacobertura.validarcobertura
{
    [DataContract]
    [Serializable]
    public class ValidarCoberturaResponse
    {
        [DataMember(Name = "responseStatus")]
        public ResponseStatus responseStatus { get; set; }

        [DataMember(Name = "responseData")]
        public ResponseData responseData { get; set; }
    }
}
