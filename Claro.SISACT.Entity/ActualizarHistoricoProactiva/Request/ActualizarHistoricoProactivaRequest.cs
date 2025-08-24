using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ActualizarHistoricoProactiva.Request
{
    [DataContract]
    [Serializable] 
    public class ActualizarHistoricoProactivaRequest
    {
        [DataMember(Name = "codigoIOBHN")]
        public string codigoIOBHN { get; set; }

        [DataMember(Name = "codigoSolin")]
        public string codigoSolin { get; set; }
    }
}
