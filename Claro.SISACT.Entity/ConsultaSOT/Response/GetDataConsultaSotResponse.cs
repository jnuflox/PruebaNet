using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ConsultaSOT.Response
{
    #region [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil]
    [DataContract]
    [Serializable]
    public class GetDataConsultaSotResponse
    {
        [DataMember(Name = "MessageResponse")]
        public GetDataConsultaSotMessageResponse MessageResponse { get; set; }

        public GetDataConsultaSotResponse()
        {
            MessageResponse = new GetDataConsultaSotMessageResponse();
        }
    }
    #endregion
}
