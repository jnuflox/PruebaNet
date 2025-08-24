using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ActualizarHistoricoProactiva.Response
{
    [DataContract]
    [Serializable]
    public class ActualizarHistoricoResponse
    {
        [DataMember(Name = "MessageResponse")]
        public ActualizarHistoricoMessageResponse MessageResponse { get; set; }

        public ActualizarHistoricoResponse()
        {
            MessageResponse = new ActualizarHistoricoMessageResponse();
        }
    }
}
