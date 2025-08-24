using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ActualizarHistoricoProactiva.Response
{
    [DataContract]
    [Serializable]
    public class ActualizarHistoricoBodyResponse
    {
        [DataMember(Name = "actualizarHistoricoProactivaResponse")]
        public ActualizarHistoricoProactivaResponse actualizarHistoricoProactivaResponse { get; set; }

        public ActualizarHistoricoBodyResponse()
        {
            actualizarHistoricoProactivaResponse = new ActualizarHistoricoProactivaResponse();
        }
    }
}
