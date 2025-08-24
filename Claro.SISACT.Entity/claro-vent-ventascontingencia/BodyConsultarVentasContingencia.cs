using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claro_vent_ventascontingencia
{
    [DataContract]
    [Serializable] //PROY-140715
    public class BodyConsultarVentasContingencia
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeaderResponse Header { get; set; }

        [DataMember(Name = "Body")]
        public ConsultarVentasContingenciaResponse Body { get; set; }

        public BodyConsultarVentasContingencia()
        {
            Header = new DataPowerRest.HeaderResponse();
            Body = new ConsultarVentasContingenciaResponse();
        }

    }
}
