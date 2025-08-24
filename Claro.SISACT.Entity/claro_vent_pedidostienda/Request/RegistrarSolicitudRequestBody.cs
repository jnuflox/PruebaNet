using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Claro.SISACT.Entity.DataPowerRest.Generic;

namespace Claro.SISACT.Entity.claro_vent_pedidostienda.Request
{
    public class RegistrarSolicitudRequestBody : IBodyRequest
    {
        [DataMember(Name = "registrarRequest")]
        public BESolicitudExcepPrecio registrarRequest { get; set; }

        public RegistrarSolicitudRequestBody(BESolicitudExcepPrecio objBESolExcepPrecio)
        {
            this.registrarRequest = objBESolExcepPrecio;
        }
    }
}
