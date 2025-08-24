using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claroventacobertura.validarcobertura
{
    [DataContract]
    [Serializable]
    public class DireccionCobertura
    {
        [DataMember(Name = "departamento")]
        public string departamento { get; set; }

        [DataMember(Name = "provincia")]
        public string provincia { get; set; }

        [DataMember(Name = "distrito")]
        public string distrito { get; set; }

        [DataMember(Name = "direccion")]
        public string direccion { get; set; }

        public DireccionCobertura()
        {
            departamento = string.Empty;
            provincia = string.Empty;
            distrito = string.Empty;
            direccion = string.Empty;
        }
    }
}
