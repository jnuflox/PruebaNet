using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.AfiliacionDEAUDebito.ConsultarAfiliacion
{

    [DataContract]
    [Serializable]
    public class ResponseConsultarAfiliacionDEAUDebito
    {

        [DataMember(Name = "MessageResponse")]
        public MessageResponseConsultarAfiliacionDEAUDebito MessageResponse { get; set; }

        public ResponseConsultarAfiliacionDEAUDebito()
        {
            MessageResponse = new MessageResponseConsultarAfiliacionDEAUDebito();
        }
    }
}
