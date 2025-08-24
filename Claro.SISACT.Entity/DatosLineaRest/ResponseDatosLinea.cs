using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.DatosLineaRest
{
    [DataContract]
    [Serializable]
    public class ResponseDatosLinea
    {
        [DataMember(Name = "responseAudit")]
        public AuditoriaRest responseAudit { get; set; }

        [DataMember(Name = "responseData")]
        public ResponseData responseData { get; set; }
    }
}
