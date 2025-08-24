using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity
{
    [DataContract]
    [Serializable]
    public class BENuevosProductosFC
    {
        [DataMember(Name = "grupo")]
        public string grupo {get; set; }

        [DataMember(Name = "planPvu")]
        public string planPvu {get; set; }

        [DataMember(Name = "tmcode")]
        public string tmcode {get; set; }

        [DataMember(Name = "tipo")]
        public string tipo {get; set; }

        [DataMember(Name = "verifica")]
        public string verifica {get; set; }

        public string soplnOrden {get; set; }

        public string descPlan {get; set; }

        public string linea {get; set; }

        [DataMember(Name = "po_id")]
        public string po_id { get; set; } //INICIATIVA-805 Campana Descuento Cargo Fijo

    }
}
