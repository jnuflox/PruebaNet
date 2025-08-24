using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claro_vent_ventascontingencia
{
    [DataContract]
    [Serializable]
    public class BEOpcional
    {
        [DataMember(Name = "campo")]
        public string campo { get; set; }

        [DataMember(Name = "valor")]
        public string valor { get; set; }

    }
}
