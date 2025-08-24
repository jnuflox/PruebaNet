//PROY-140546
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest.DataPower.ConsultaHistorico.Response
{
    [Serializable]
    public class ConsultaHistBodyResponse
    {
        public ConsultaHistResponse consultaHistResponseType { get; set; }
        public string claroFault { get; set; }
    }
}
