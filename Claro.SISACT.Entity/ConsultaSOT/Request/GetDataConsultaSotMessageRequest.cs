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
    public class GetDataConsultaSotMessageRequest
    {

        [DataMember(Name = "Body")]
        public GetDataConsultaSotBodyRequest Body { get; set; }

        [DataMember(Name = "Header")]
        public DataPowerRest.Generic.HeadersRequest Header { get; set; }

        public GetDataConsultaSotMessageRequest()
        {
            Header = new DataPowerRest.Generic.HeadersRequest();
            Body = new GetDataConsultaSotBodyRequest();
        }


    }
    #endregion

}
