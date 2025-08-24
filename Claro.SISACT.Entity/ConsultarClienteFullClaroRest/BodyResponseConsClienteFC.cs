using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Claro.SISACT.Entity;

namespace Claro.SISACT.Entity.ConsultarClienteFullClaroRest
{
    [DataContract]
    [Serializable]
    public class BodyResponseConsClienteFC
    {
        [DataMember(Name = "codigoRespuesta")]
        public string codigoRespuesta { get; set; }
        [DataMember(Name = "mensajeRespuesta")]
        public string mensajeRespuesta { get; set; }
        [DataMember(Name = "cursorCliente")]
        public List<BEDatosClienteFC> dataRespuesta { get; set; }
    }
}
