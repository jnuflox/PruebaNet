using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity.DataPowerRest.Generic;

namespace Claro.SISACT.Entity.claro_vent_pedidostienda.Response
{
    public class PedidosTiendaResponse : GenericResponse
    {
        public PedidosTiendaResponse()
        {
            this.getMessageResponse().setBody(new PedidosTiendaResponseBody());
        }
    }
}
