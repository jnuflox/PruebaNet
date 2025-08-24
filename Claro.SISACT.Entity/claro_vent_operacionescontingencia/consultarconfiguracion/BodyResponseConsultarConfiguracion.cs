using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claro_vent_operacionescontingencia.consultarconfiguracion
{
    [Serializable]
    [DataContract]
    public class BodyResponseConsultarConfiguracion
    {
        [DataMember(Name = "consultarConfiguracionResponse")]
        public ConsultarConfiguracionResponse consultarconfiguracionResponse { get; set; }

        public BodyResponseConsultarConfiguracion()
        {
            consultarconfiguracionResponse = new ConsultarConfiguracionResponse();
        }
    }
}
