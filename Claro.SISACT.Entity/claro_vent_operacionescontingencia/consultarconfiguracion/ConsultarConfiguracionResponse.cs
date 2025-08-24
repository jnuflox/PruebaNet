using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Claro.SISACT.Entity.RegistrarOmisionPINRest;
using Claro.SISACT.Entity.claro_vent_ventascontingencia;

namespace Claro.SISACT.Entity.claro_vent_operacionescontingencia.consultarconfiguracion
{
    [Serializable]
    [DataContract]
    public class ConsultarConfiguracionResponse
    {
        [DataMember(Name = "auditResponse")]
        public BEResponseAudit auditResponse { get; set; }

        [DataMember(Name = "consultarConfiguracion")]
        public BEconsultarConfiguracion consultarConfiguracion { get; set; }

        [DataMember(Name = "responseOpcional")]
        public List<BEOpcional> responseOpcional { get; set; }

        public ConsultarConfiguracionResponse()
        {
            auditResponse = new BEResponseAudit();
            consultarConfiguracion = new BEconsultarConfiguracion();
            responseOpcional = new List<BEOpcional>();
        }
    }
}
