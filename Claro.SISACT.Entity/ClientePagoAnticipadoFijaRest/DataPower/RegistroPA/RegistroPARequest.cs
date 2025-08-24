//PROY-140546
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest.DataPower.RegistroPA
{
    public class RegistroPARequest
    {
        public Int64 numeroSolicitud { get; set; }
        public Int64 numeroContrato { get; set; }
        public Int64 numeroSolicitudPlan { get; set; }
        public string tipoProducto { get; set; }
        public double montoTotalInstalacion { get; set; }
        public double montoInicialInstalacion { get; set; }
        public double montoInicialModificado { get; set; }
        public double porcentajeMontoInicial { get; set; }
        public string tipoInicial { get; set; }
        public string numeroDocumento { get; set; }
        public string nombreCliente { get; set; }
        public string fechaProgramacion1 { get; set; }
        public string franjaHoraria1 { get; set; }
        public string fechaProgramacion2 { get; set; }
        public string franjaHoraria2 { get; set; }
        public string fechaProgramacion3 { get; set; }
        public string franjaHoraria3 { get; set; }
        public string usuarioRegistro { get; set; }
        public string correoiClaro { get; set; }
        public string flagPublicar { get; set; }
        public string medioPago { get; set; }
        public string tipoOficina { get; set; }
        public string ovencCodigo { get; set; }
    }
}
