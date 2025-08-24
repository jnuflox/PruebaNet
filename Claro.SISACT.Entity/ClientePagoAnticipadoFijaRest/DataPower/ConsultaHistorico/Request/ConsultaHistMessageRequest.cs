//PROY-140546
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity.DataPowerRest;

namespace Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest.DataPower.ConsultaHistorico.Request
{
    [Serializable]
    public class ConsultaHistMessageRequest
    {
        public ConsultaPAHeader Header { get; set; }
        public ConsultaHistBodyRequest Body { get; set; }
    }
}
