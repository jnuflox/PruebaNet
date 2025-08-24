//PROY-140546
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest.DataPower.ConsultaHistorico.Response
{
    [Serializable]
    public class ConsultaHistResponse
    {
        public ConsultaHistResponseData responseData { get; set; }
        public ConsultaHistResponseStatus responseStatus { get; set; }        
    }
}
