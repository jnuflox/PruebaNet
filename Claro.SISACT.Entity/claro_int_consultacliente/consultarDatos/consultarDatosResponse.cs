using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Claro.SISACT.Entity.GenericRequestResponse;

namespace Claro.SISACT.Entity.claro_int_consultacliente.consultarDatos
{
    [DataContract]
    [Serializable]
    public class consultarDatosResponse
    {
        [DataMember(Name = "responseAudit")]
        public BEAuditResponse responseAudit { get; set; }

        [DataMember(Name = "responseData")]
        public BodyResponseData responseData { get; set; }
    }
}
