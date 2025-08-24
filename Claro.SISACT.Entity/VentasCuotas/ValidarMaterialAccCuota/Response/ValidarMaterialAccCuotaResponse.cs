using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.VentasCuotas.ValidarMaterialAccCuota.Response
{
    [DataContract]
    [Serializable]
    public class ValidarMaterialAccCuotaResponse
    {
         [DataMember(Name = "MessageResponse")]
        public ValidarMaterialAccCuotaMessageResponse MessageResponse { get; set; }

         public ValidarMaterialAccCuotaResponse()
        {
            MessageResponse = new ValidarMaterialAccCuotaMessageResponse();
        }
    }
}
