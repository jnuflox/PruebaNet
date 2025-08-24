using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ActualizarHistoricoProactiva.Request
{
    [DataContract]
    [Serializable]
    public class ActualizarHistoricoRequest
    {
        [DataMember(Name = "MessageRequest")]
        public ActualizarHistoricoMessageRequest MessageRequest { get; set; }

        public ActualizarHistoricoRequest()
        {
            MessageRequest = new ActualizarHistoricoMessageRequest();
        }
    }
}
