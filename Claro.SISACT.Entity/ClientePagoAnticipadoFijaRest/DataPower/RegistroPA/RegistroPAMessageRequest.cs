//PROY-140546
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity.DataPowerRest;

namespace Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest.DataPower.RegistroPA
{
    public class RegistroPAMessageRequest
    {
        public HeadersRequest Header { get; set; }
        public RegistroPABody Body { get; set; }
    }
}
