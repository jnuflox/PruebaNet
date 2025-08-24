using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claro_ope_comportamientoCliente.consultarComportamientoPago
{
    [DataContract]
    [Serializable]
    public class ConsultarResponseComportamientoCliente
    {
        [DataMember(Name = "responseAudit")]
        public BodyResponseAudit responseAudit { get; set; }
        [DataMember(Name = "responseData")]
        public BodyResponseData responseData { get; set; }
    }
}
