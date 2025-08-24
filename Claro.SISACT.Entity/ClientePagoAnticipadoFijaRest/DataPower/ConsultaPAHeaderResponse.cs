//PROY-140546
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest.DataPower
{
    public class ConsultaPAHeaderResponse
    {
        public string consumer { get; set; }
        public string pid { get; set; }
        public string timestamp { get; set; }
        public string VarArg { get; set; }
        public ConsultaPAStatus status  { get; set; }

    }
}
