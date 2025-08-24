using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claro_vent_fullclaroCBIO.consultarActivosCBIO
{
    [DataContract]
    [Serializable]
    public class CursorCliente
    {
        [DataMember(Name = "coId")]
        public string coId { get; set; }

        [DataMember(Name = "linea")]
        public string linea { get; set; }
        [DataMember(Name = "tipServicio")]
        public string tipServicio { get; set; }

        [DataMember(Name = "tmCode")]
        public string tmCode { get; set; }

        [DataMember(Name = "desTmcode")]
        public string desTmcode { get; set; }

        [DataMember(Name = "customerId")]
        public string customerId { get; set; }
    }
}
