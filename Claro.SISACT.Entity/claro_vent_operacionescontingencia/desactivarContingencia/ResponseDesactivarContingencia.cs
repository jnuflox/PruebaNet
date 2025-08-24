using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claro_vent_operacionescontingencia.desactivarContingencia
{
    [Serializable]
    [DataContract]
    public class ResponseDesactivarContingencia
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersResponse Header { get; set; }

        [DataMember(Name = "Body")]
        public BodyResponseDesactivarContingencia Body { get; set; }

        public ResponseDesactivarContingencia()
        {
            Header = new DataPowerRest.HeadersResponse();
            Body = new BodyResponseDesactivarContingencia();

        }
    }
}
