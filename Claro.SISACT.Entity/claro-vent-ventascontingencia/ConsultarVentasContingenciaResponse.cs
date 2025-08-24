using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Claro.SISACT.Entity.DataPowerRest.Generic;

namespace Claro.SISACT.Entity.claro_vent_ventascontingencia
{
    [DataContract]
    [Serializable] //PROY-140715
    public class ConsultarVentasContingenciaResponse
    {
        [DataMember(Name = "consultarVentasContingenciaResponse")]
        public ConsultarVentasContingencia consultarVentasContingenciaResponse { get; set; }

        public ConsultarVentasContingenciaResponse()
        {
            consultarVentasContingenciaResponse = new ConsultarVentasContingencia();
        }
    }
}
