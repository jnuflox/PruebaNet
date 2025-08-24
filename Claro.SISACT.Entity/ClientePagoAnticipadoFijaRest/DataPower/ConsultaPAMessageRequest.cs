//PROY-140546
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest.DataPower
{
    public class ConsultaPAMessageRequest
    {
        public ConsultaPAHeader Header { get; set; }
        public ConsultaPABody Body { get; set; }
    }
}
