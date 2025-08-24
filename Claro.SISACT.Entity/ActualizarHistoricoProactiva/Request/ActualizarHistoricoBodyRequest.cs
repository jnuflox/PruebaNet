using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ActualizarHistoricoProactiva.Request
{
    [DataContract]
    [Serializable] 
    public class ActualizarHistoricoBodyRequest
    {
        [DataMember(Name = "actualizarHistoricoProactivaRequest")]
        public ActualizarHistoricoProactivaRequest actualizarHistoricoProactivaRequest { get; set; }

        public ActualizarHistoricoBodyRequest()
        {
            actualizarHistoricoProactivaRequest = new ActualizarHistoricoProactivaRequest();
        }
    }
}
