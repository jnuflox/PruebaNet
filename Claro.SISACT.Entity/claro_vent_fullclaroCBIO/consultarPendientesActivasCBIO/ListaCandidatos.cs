using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claro_vent_fullclaroCBIO.consultarPendientesActivasCBIO
{
    [DataContract]
    [Serializable]
    public class ListaCandidatos
    {
        [DataMember(Name = "grupo")]
        public string grupo { get; set; }

        [DataMember(Name = "tmcode")]
        public string tmcode { get; set; }

        [DataMember(Name = "tipo")]
        public string tipo { get; set; }

        [DataMember(Name = "verifica")]
        public string verifica { get; set; }

        [DataMember(Name = "planPvu")]
        public string planPvu { get; set; }
    }
}
