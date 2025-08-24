//PROY-140546
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest.DataPower
{
    public class ConsultaPARequest
    {
        public string tipoConsulta { get; set; }
        public Int64 numeroSolicitud { get; set; }
        public string numeroDocumento { get; set; }
        public string estado { get; set; }
    }
}
