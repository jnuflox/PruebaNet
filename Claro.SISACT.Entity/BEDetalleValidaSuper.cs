using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity
{
    //PROY-140419 Autorizar Portabilidad sin PIN
    [DataContract]
    [Serializable]
    public class BEDetalleValidaSuper
    {
        [DataMember(Name = "nroSec")]
        public Int64 nroSec { get; set; }

        [DataMember(Name = "lineaPorta")]
        public string lineaPorta { get; set; }

        [DataMember(Name = "consecutivo")]
        public string consecutivo { get; set; }

        [DataMember(Name = "usuario")]
        public string usuario { get; set; }
    }
}
