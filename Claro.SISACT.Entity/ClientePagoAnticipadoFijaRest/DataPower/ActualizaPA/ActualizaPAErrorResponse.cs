//PROY-140546
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest.DataPower.ActualizaPA
{
    public class ActualizaPAErrorResponse
    {
        public Int64 numeroSolicitud { get; set; }
        public string codigoError { get; set; }
        public string descripcionError { get; set; }
    }
}
