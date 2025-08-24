using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claro_vent_ventascontingencia
{
    [DataContract]
    [Serializable] //PROY-140715
    public class MessageResponseVentasContingencia
    {
        [DataMember(Name = "MessageResponse")]
        public BodyConsultarVentasContingencia MessageResponse { get; set; }

        public MessageResponseVentasContingencia()
        {
            MessageResponse = new BodyConsultarVentasContingencia();
        }

    }
}
