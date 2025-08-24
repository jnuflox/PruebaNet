using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.VentasCuotas.ValidarMaterialAccCuota.Response
{
    [DataContract]
    [Serializable]
    public class ValidarMaterialAccCuotaMessageResponse
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersResponse Header { get; set; }

        [DataMember(Name = "Body")]
        public ValidarMaterialAccCuotaBodyResponse Body { get; set; }

        public ValidarMaterialAccCuotaMessageResponse()
        {
            Header = new DataPowerRest.HeadersResponse();
            Body = new ValidarMaterialAccCuotaBodyResponse();
        }

    }
}
