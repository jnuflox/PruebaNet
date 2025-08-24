using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Claro.SISACT.Entity.RegistrarOmisionPINRest;
using Claro.SISACT.Entity.claro_vent_ventascontingencia;
using Claro.SISACT.Entity.claro_vent_identidadcontingencia.validarconfiguracion;

namespace Claro.SISACT.Entity.claro_vent_operacionescontingencia.desactivarContingencia
{
    [Serializable]
    [DataContract]
    public class DesactivarContingenciaResponse
    {
        [DataMember(Name = "auditResponse")]
        public BEResponseAudit auditResponse { get; set; }

        [DataMember(Name = "desactivarContingencia")]
        public BEValidarConfiguracion desactivarContingencia { get; set; }

        [DataMember(Name = "responseOpcional")]
        public List<BEOpcional> responseOpcional { get; set; }

        public DesactivarContingenciaResponse()
        {
            auditResponse = new BEResponseAudit();
            desactivarContingencia = new BEValidarConfiguracion();
            responseOpcional = new List<BEOpcional>();
        }
    }
}
