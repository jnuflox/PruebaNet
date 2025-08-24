using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claro_vent_operacionescontingencia.desactivarContingencia
{
    [Serializable]
    [DataContract]
    public class MessageResponseDesactivarContingencia
    {
        [DataMember(Name = "MessageResponse")]
        public ResponseDesactivarContingencia MessageResponse { get; set; }

        public MessageResponseDesactivarContingencia()
        {
            MessageResponse = new ResponseDesactivarContingencia();
        }
    }
}
