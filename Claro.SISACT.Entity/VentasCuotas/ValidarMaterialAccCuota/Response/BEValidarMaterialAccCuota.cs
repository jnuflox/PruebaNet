using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.VentasCuotas.ValidarMaterialAccCuota.Response
{
    [DataContract]
    [Serializable]
    public class BEValidarMaterialAccCuota
    {
        [DataMember(Name = "codMaterial")]
        public string codMaterial { get; set; }

        [DataMember(Name = "nomLargoMaterial")]
        public string nomLargoMaterial { get; set; }

        [DataMember(Name = "marca")]
        public string marca { get; set; }

        [DataMember(Name = "tipoAccesorio")]
        public string tipoAccesorio { get; set; }
    }
}
