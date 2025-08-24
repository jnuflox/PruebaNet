using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claro_int_consultacliente.consultarDatos
{
    [DataContract]
    [Serializable]
    public class MessageResponseConsultarCliente
    {
        [DataMember(Name = "consultarDatosResponse")]
        public consultarDatosResponse consultarDatosResponse { get; set; }
    }
}
