using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ConsultaSOT.Response
{
    [DataContract]
    [Serializable]
    public class GetDataConsultaSotMessageResponse
    {
        #region [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil]
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersResponse Header { get; set; }

        [DataMember(Name = "Body")]
        public GetDataConsultaSotBodyResponse Body { get; set; }

        public GetDataConsultaSotMessageResponse()
        {
            Header = new DataPowerRest.HeadersResponse();
            Body = new GetDataConsultaSotBodyResponse();
        }
    }
        #endregion

}
