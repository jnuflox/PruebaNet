//PROY-140546
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity.DataPowerRest;

namespace Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest.DataPower.RegistroPA
{
    public class RegistroPAMessageResponse
    {
        //public RegistroPAHeader Header { get; set; }

        public HeadersResponse Header { get; set; }
        public RegistroPAResponseBody Body { get; set; }
    }
}
