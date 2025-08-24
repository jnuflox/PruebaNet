//PROY-140546
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity.DataPowerRest;

namespace Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest.DataPower.RegistraHistorial
{
    public class RegistraHistorialMessageRequest
    {
        public HeadersRequest Header { get; set; }
        public RegistraHistorialBody Body { get; set; }
    }
}
