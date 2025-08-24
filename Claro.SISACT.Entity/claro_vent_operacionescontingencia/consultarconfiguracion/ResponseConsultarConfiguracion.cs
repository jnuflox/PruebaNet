using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claro_vent_operacionescontingencia.consultarconfiguracion
{
    [Serializable]
    [DataContract]
    public class ResponseConsultarConfiguracion
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersResponse Header { get; set; }

        [DataMember(Name = "Body")]
        public BodyResponseConsultarConfiguracion Body { get; set; }

        public ResponseConsultarConfiguracion()
        {
            Header = new DataPowerRest.HeadersResponse();
            Body = new BodyResponseConsultarConfiguracion();

        }
    }
}
