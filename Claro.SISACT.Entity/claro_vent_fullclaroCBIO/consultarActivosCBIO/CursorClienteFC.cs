using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claro_vent_fullclaroCBIO.consultarActivosCBIO
{
    [DataContract]
    [Serializable]
    public class CursorClienteFC
    {
        [DataMember(Name = "tipoDoc")]
        public string tipoDoc { get; set; }

        [DataMember(Name = "numDoc")]
        public string numDoc { get; set; }

        [DataMember(Name = "codProducto")]
        public string codProducto { get; set; }

        [DataMember(Name = "beneficio")]
        public string beneficio { get; set; }

        [DataMember(Name = "tipoOperacion")]
        public string tipoOperacion { get; set; }

        [DataMember(Name = "flagPorta")]
        public string flagPorta { get; set; }

        [DataMember(Name = "fechaReg")]
        public string fechaReg { get; set; }

        [DataMember(Name = "linea")]
        public string linea { get; set; }

        [DataMember(Name = "tipoServicio")]
        public string tipoServicio { get; set; }

        [DataMember(Name = "numContrato")]
        public string numContrato { get; set; }

        [DataMember(Name = "coId")]
        public string coId { get; set; }

        [DataMember(Name = "customerId")]
        public string customerId { get; set; }
    }
}
