using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claroventacobertura.validarcobertura
{
    [DataContract]
    [Serializable]
    public class SolicitudCobertura
    {
        [DataMember(Name = "tipoProducto")]
        public string tipoProducto { get; set; }

        [DataMember(Name = "modalidad")]
        public string modalidad { get; set; }

        [DataMember(Name = "venta")]
        public string venta { get; set; }

        [DataMember(Name = "monto")]
        public string monto { get; set; }

        [DataMember(Name = "equipo")]
        public string equipo { get; set; }

        [DataMember(Name = "plan")]
        public string plan { get; set; }

        public SolicitudCobertura()
        {
            tipoProducto = string.Empty;
            modalidad = string.Empty;
            venta = string.Empty;
            monto = string.Empty;
            equipo = string.Empty;
            plan = string.Empty;
        }
    }
}
