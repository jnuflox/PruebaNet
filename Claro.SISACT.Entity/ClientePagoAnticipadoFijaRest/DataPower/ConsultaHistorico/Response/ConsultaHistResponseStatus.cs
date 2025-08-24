//PROY-140546
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest.DataPower.ConsultaHistorico.Response
{
    [Serializable]
    public class ConsultaHistResponseStatus
    {
        public string idTransaccion { get; set; }
        public string codigoRespuesta { get; set; }
        public string mensajeRespuesta { get; set; }
    }
}
