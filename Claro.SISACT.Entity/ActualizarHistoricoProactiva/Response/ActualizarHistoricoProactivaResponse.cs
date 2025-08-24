using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ActualizarHistoricoProactiva.Response
{
    [DataContract]
    [Serializable]
    public class ActualizarHistoricoProactivaResponse
    {
        [DataMember(Name = "responseStatus")]
        public ActualizarHistoricoResponseStatus responseStatus { get; set; }

        public ActualizarHistoricoProactivaResponse()
        {
            responseStatus = new ActualizarHistoricoResponseStatus();
        }
    }
}
