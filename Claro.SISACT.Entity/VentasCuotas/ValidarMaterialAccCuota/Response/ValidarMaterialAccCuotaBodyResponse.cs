using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.VentasCuotas.ValidarMaterialAccCuota.Response
{
    [DataContract]
    [Serializable]
    public class ValidarMaterialAccCuotaBodyResponse
    {
        [DataMember(Name = "validarMaterialAccCuotaResponse")]
        public ValidarMaterialAccCuotaTypeResponse validarMaterialAccCuotaResponse { get; set; }

        public ValidarMaterialAccCuotaBodyResponse()
        {
            validarMaterialAccCuotaResponse = new ValidarMaterialAccCuotaTypeResponse();
        }

    }
}
