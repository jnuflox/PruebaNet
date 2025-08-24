using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claro_vent_fullclaroCBIO
{
    [DataContract]
    [Serializable]
    public class ListaOnHold
    {
        [DataMember(Name = "coId")]
        public string coId { get; set; }

        [DataMember(Name = "linea")]
        public string linea { get; set; }

        [DataMember(Name = "tipoServicio")]
        public string tipoServicio { get; set; }

        [DataMember(Name = "tmcode")]
        public string tmcode { get; set; }

        [DataMember(Name = "desTmcode")]
        public string desTmcode { get; set; }

        [DataMember(Name = "customerId")]
        public string customerId { get; set; }
    }
}
