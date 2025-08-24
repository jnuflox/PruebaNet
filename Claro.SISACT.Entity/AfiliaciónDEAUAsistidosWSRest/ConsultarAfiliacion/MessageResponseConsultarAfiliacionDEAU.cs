using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.AfiliaciónDEAUAsistidosWSRest.ConsultarAfiliacion
{
    [DataContract]
    [Serializable]
    public class MessageResponseConsultarAfiliacionDEAU
    {
         [DataMember(Name = "Header")]
        public DataPowerRest.HeadersResponse header { get; set; }
        [DataMember(Name = "Body")]
        public BodyResponseConsultarAfiliacionDEAU body { get; set; }

        public MessageResponseConsultarAfiliacionDEAU()
        {
            header = new DataPowerRest.HeadersResponse();
            body = new BodyResponseConsultarAfiliacionDEAU();
        }
    }
}
