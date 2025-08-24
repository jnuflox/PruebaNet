using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity.DataPowerRest.Generic;

namespace Claro.SISACT.Entity.claro_vent_pedidostienda.Request
{
    public class RegistrarSolicitudRequest : GenericRequest
    {
        public RegistrarSolicitudRequest(BESolicitudExcepPrecio objBESolExcepPrecio)
        {
            this.getMessageRequest().setBody(new RegistrarSolicitudRequestBody(objBESolExcepPrecio));
        }
    }
}
