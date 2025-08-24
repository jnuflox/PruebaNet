using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claro_vent_ventascontingencia
{
    [DataContract]
    [Serializable]
    public class BEVenta
    {
        [DataMember(Name = "tipoDoc")]
        public string tipoDoc { get; set; }

        [DataMember(Name = "numDoc")]
        public string numDoc { get; set; }

        [DataMember(Name = "numSec")]
        public string numSec { get; set; }

        [DataMember(Name = "numPedido")]
        public string numPedido { get; set; }

        [DataMember(Name = "fechaInicio")]
        public string fechaInicio { get; set; }

        [DataMember(Name = "fechaFin")]
        public string fechaFin { get; set; }

        [DataMember(Name = "estadoValidacion")]
        public string estadoValidacion { get; set; }

        [DataMember(Name = "codCanal")]
        public string codCanal { get; set; }

        [DataMember(Name = "codPdv")]
        public string codPdv { get; set; }

        [DataMember(Name = "tipoContingencia")]
        public string tipoContingencia { get; set; }

        [DataMember(Name = "estadoPago")]
        public string estadoPago { get; set; }
    }
}
