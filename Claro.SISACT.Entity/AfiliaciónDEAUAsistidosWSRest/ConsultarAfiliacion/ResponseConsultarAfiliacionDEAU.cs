using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.AfiliaciónDEAUAsistidosWSRest.ConsultarAfiliacion
{
    [DataContract]
    [Serializable]
    public class ResponseConsultarAfiliacionDEAU
    {
         [DataMember(Name = "MessageResponse")]
        public MessageResponseConsultarAfiliacionDEAU MessageResponse { get; set; }

         public ResponseConsultarAfiliacionDEAU()
        {
            MessageResponse = new MessageResponseConsultarAfiliacionDEAU();
        }

    }
}
