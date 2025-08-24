using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ValidarCampanaRest.Request
{
    [DataContract]
    [Serializable]
    public class ValidarCampanaRequest
    {
        [DataMember(Name = "flagLinea")]
        public string flagLinea { get; set; }

        [DataMember(Name = "tipoDocumento")]
        public string tipoDocumento { get; set; }

        [DataMember(Name = "numeroDocumento")]
        public string numeroDocumento { get; set; }
    }
}
