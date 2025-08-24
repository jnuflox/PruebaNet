//PROY-140546
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity.DataPowerRest;

namespace Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest.DataPower.ActualizaPA
{
    public class ActualizaPAMessageResponse
    {
        public HeadersResponse Header { get; set; }
        public ActualizaPAResponseBody Body { get; set; }
    }
}
