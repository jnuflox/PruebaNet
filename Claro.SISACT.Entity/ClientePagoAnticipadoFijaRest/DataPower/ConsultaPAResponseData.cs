//PROY-140546
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest.DataPower
{

    public class ConsultaPAResponseData
    {
        public List<PagoAnticipado> pagoAnticipado { get; set; }
    }

    public class ConsultaPAResponseStatus
    {
        
        public string idTransaccion { get; set; }

        public string codigoRespuesta { get; set; }

        public string mensajeRespuesta { get; set; }

    }
}
