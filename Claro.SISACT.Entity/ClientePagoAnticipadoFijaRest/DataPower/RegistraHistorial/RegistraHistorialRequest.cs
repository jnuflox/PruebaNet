//PROY-140546
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest.DataPower.RegistraHistorial
{
    public class RegistraHistorialRequest
    {
        public Int64 numeroSolicitud { get; set; }
        public Int64 numeroSolicitudPlan { get; set; }
        public string tipoProducto { get; set; }
        public string codigoPlan { get; set; }
        public string descripcionPlan { get; set; }
        public string fechaRegistro { get; set; }
        public double costoInstalacion { get; set; }
        public double costoInstalacionManual { get; set; }
        public string formaPago { get; set; }
        public string formaPagoManual { get; set; }
        public Int64 numeroCuotas { get; set; }
        public Int64 numeroCuotasManual { get; set; }
        public double montoAnticipadoInstalacion { get; set; }
        public double montoAnticipadoInstalacionManual { get; set; }
        public string usuarioActualizacion { get; set; }
        public Int64 grupoSolicitud { get; set; }
        public string puntoVenta { get; set; }
    }
}
