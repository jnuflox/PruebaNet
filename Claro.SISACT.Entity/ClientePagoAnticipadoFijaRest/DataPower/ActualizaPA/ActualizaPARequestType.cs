//PROY-140546
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest.DataPower.ActualizaPA
{
    public class ActualizaPARequestType
    {
        public Int64 numeroSolicitud { get; set; }
        public string estado { get; set; }
        public string usuarioActualizacion { get; set; }
        public string montoInicialModificado { get; set; }
        public long transaccionPago { get; set; }
        public string fechaTransaccionPago { get; set; }
        public string medioPago { get; set; }
    }
}
