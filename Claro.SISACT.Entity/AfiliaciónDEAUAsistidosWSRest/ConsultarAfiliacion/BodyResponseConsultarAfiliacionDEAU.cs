using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.AfiliaciónDEAUAsistidosWSRest.ConsultarAfiliacion
{
    [DataContract]
    [Serializable]
    public class BodyResponseConsultarAfiliacionDEAU
    {
        [DataMember(Name = "codigoRespuesta")]
        public string codigoRespuesta { get; set; }

        [DataMember(Name = "mensajeRespuesta")]
        public string mensajeRespuesta { get; set; }

        [DataMember(Name = "listAfiliacion")]
        public List<BEListaConsultarAfiliacion> listAfiliacion { get; set; }

    }
}
