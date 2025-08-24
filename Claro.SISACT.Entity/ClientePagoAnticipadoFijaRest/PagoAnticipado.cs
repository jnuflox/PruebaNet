//PROY-140546
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest
{
    public class PagoAnticipado
    {
        public string codigo { get; set; }
        public string numeroSolicitud { get; set; }
        public string numeroContrato { get; set; }
        public string numeroSolicitudPlan { get; set; }
        public string tipoProducto { get; set; }
        public string montoTotalInstalacion { get; set; }
        public string montoInicialInstalacion { get; set; }
        public string porcentajeMontoInicial { get; set; }
        public string tipoInicial { get; set; }
        public string numeroDocumento { get; set; }
        public string nombreCliente { get; set; }
        public string fechaProgramacion1 { get; set; }
        public string franjaHoraria1 { get; set; }
        public string fechaProgramacion2 { get; set; }
        public string franjaHoraria2 { get; set; }
        public string fechaProgramacion3 { get; set; }
        public string franjaHoraria3 { get; set; }
        public string estado { get; set; }
        public string fechaRegistro { get; set; }
        public string usuarioRegistro { get; set; }
        public string fechaActualizacion { get; set; }
        public string usuarioActualizacion { get; set; }
        public string transaccionPago { get; set; }
        public string fechaTransaccionPago { get; set; }
        public string medioPago { get; set; }
    }
}
