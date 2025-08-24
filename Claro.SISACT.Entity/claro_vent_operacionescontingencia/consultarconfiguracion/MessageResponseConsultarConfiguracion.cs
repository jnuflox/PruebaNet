using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claro_vent_operacionescontingencia.consultarconfiguracion
{
    [Serializable]
    [DataContract]
    public class MessageResponseConsultarConfiguracion
    {
        [DataMember(Name = "MessageResponse")]
        public ResponseConsultarConfiguracion MessageResponse { get; set; }

        public MessageResponseConsultarConfiguracion()
        {
            MessageResponse = new ResponseConsultarConfiguracion();
        }
    }
}
