using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity
{
    [DataContract]
    [Serializable]
    public class BEconfiguracionVenta
    {
        [DataMember(Name = "sistema")]
        public string sistema { get; set; }
        [DataMember(Name = "codVenta")]
        public string codVenta { get; set; }
        [DataMember(Name = "descVenta")]
        public string descVenta { get; set; }
        [DataMember(Name = "contingencia1")]
        public string contingencia1 { get; set; }
        [DataMember(Name = "contingencia2")]
        public string contingencia2 { get; set; }
        [DataMember(Name = "extranjero")]
        public string extranjero { get; set; }
    }
}
