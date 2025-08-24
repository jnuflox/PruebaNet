using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.AfiliacionDEAUDebito.ConsultarAfiliacion
{

    [DataContract]
    [Serializable]
    public class MessageResponseConsultarAfiliacionDEAUDebito
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersResponse header { get; set; }
        [DataMember(Name = "Body")]
        public BodyResponseConsultarAfiliacionDEAUDebito body { get; set; }

        public MessageResponseConsultarAfiliacionDEAUDebito()
        {
            header = new DataPowerRest.HeadersResponse();
            body = new BodyResponseConsultarAfiliacionDEAUDebito();
        }
    }
}
