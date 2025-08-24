using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.AfiliacionDEAUDebito.ConsultarAfiliacion
{

    [DataContract]
    [Serializable]
    public class BodyResponseConsultarAfiliacionDEAUDebito
    {
        [DataMember(Name = "codigoRespuesta")]
        public string codigoRespuesta { get; set; }

        [DataMember(Name = "mensajeRespuesta")]
        public string mensajeRespuesta { get; set; }

        [DataMember(Name = "listaAfiliacion")]
        public List<BEListaConsultarAfiliacionDebito> listaAfiliacion { get; set; }

        [DataMember(Name = "listaNotificacion")]
        public List<BEListaNotificacionDebito> listaNotificacion { get; set; }

        [DataMember(Name = "ubicacionData")]
        public string ubicacionData { get; set; }

        [DataMember(Name = "datosAfiliacion")]
        public BEDatosAfiliacionDebito datosAfiliacion { get; set; }


    }
}
