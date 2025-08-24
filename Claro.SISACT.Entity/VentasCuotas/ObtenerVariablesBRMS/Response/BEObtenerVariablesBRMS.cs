using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.VentasCuotas.ObtenerVariablesBRMS.Response
{
    [DataContract]
    [Serializable]
    public class BEObtenerVariablesBRMS
    {
        [DataMember(Name = "montoPendCuotas")]
        public string montoPendCuotas { get; set; }

        [DataMember(Name = "cantPendCuotas")]
        public string cantPendCuotas { get; set; }

        [DataMember(Name = "cantMaxPendCuotas")]
        public string cantMaxPendCuotas { get; set; }

        [DataMember(Name = "montoPendUltVentas")]
        public string montoPendUltVentas { get; set; }

        [DataMember(Name = "cantPendUltVentas")]
        public string cantPendUltVentas { get; set; }

        [DataMember(Name = "cantMaxPendUltVentas")]
        public string cantMaxPendUltVentas { get; set; }
    }
}
