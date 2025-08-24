using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claro_vent_operacionescontingencia.desactivarContingencia
{
    [Serializable]
    [DataContract]
    public class BodyResponseDesactivarContingencia
    {
        [DataMember(Name = "desactivarContingenciaResponse")]
        public DesactivarContingenciaResponse desactivarContingenciaResponse { get; set; }

        public BodyResponseDesactivarContingencia()
        {
            desactivarContingenciaResponse = new DesactivarContingenciaResponse();
        }
    }
}
