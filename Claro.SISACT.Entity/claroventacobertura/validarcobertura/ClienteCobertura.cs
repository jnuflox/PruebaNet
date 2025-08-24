using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claroventacobertura.validarcobertura
{
    [DataContract]
    [Serializable]
    public class ClienteCobertura
    {
        [DataMember(Name = "tipoDoc")]
        public string tipoDoc { get; set; }

        [DataMember(Name = "numeroDoc")]
        public string numeroDoc { get; set; }

        [DataMember(Name = "nombres")]
        public string nombres { get; set; }

        [DataMember(Name = "apellidos")]
        public string apellidos { get; set; }

        [DataMember(Name = "correo")]
        public string correo { get; set; }

        [DataMember(Name = "telefonoContacto")]
        public string telefonoContacto { get; set; }

        public ClienteCobertura()
        {
            tipoDoc = string.Empty;
            numeroDoc = string.Empty;
            nombres = string.Empty;
            apellidos = string.Empty;
            correo = string.Empty;
            telefonoContacto = string.Empty;
        }
    }
}
