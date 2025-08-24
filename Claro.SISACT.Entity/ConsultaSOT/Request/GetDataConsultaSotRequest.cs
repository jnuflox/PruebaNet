using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ConsultaSOT.Request
{
    #region [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil]
    [DataContract]
    [Serializable]
    public class GetDataConsultaSotRequest
    {
        [DataMember(Name = "MessageRequest")]
        public GetDataConsultaSotMessageRequest MessageRequest { get; set; }

        public GetDataConsultaSotRequest()
        {
            MessageRequest = new GetDataConsultaSotMessageRequest();
        }

    }
    #endregion
}
