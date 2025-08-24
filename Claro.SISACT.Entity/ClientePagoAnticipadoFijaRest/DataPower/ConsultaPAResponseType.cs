//PROY-140546
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest.DataPower
{
    public class ConsultaPAResponseType
    {
        public ConsultaPAResponseData responseData { get; set; }

        public ConsultaPAResponseStatus responseStatus { get; set; }
    }
}
