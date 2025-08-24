using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity
{
    [DataContract]
    [Serializable]
    public class BEconfiguracionGeneral
    {
        [DataMember(Name = "codContingencia")]
        public string codContingencia { get; set; }
        [DataMember(Name = "descContingencia")]
        public string descContingencia { get; set; }
        [DataMember(Name = "sistema")]
        public string sistema { get; set; }
        [DataMember(Name = "canal")]
        public string canal { get; set; }
        [DataMember(Name = "puntoventa")]
        public string puntoventa { get; set; }
        [DataMember(Name = "operacion")]
        public string operacion { get; set; }
        [DataMember(Name = "opcionventa")]
        public string opcionventa { get; set; }
        [DataMember(Name = "producto")]
        public string producto { get; set; }
    }
}
