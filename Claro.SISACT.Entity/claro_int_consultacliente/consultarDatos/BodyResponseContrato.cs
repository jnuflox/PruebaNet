using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claro_int_consultacliente.consultarDatos
{
    [DataContract]
    [Serializable]
    public class BodyResponseContrato
    {
        [DataMember(Name = "coId")]
        public string coId { get; set; }

        [DataMember(Name = "coIdPub")]
        public string coIdPub { get; set; }

        [DataMember(Name = "coStatus")]
        public string coStatus { get; set; }

        [DataMember(Name = "coActivated")]
        public string coActivated { get; set; }

        [DataMember(Name = "coLastReason")]
        public string coLastReason { get; set; }

        [DataMember(Name = "dirNum")]
        public string dirNum { get; set; }

        [DataMember(Name = "coLastStatusChangeDate")]
        public string coLastStatusChangeDate { get; set; }

        [DataMember(Name = "cargoFijo")]
        public string cargoFijo { get; set; }

        [DataMember(Name = "ticklers")]
        public List<BodyResponseTicklers> ticklers { get; set; }

        [DataMember(Name = "productos")]
        public List<BodyResponseProductos> productos { get; set; }
    }
}
