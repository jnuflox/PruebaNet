using System;
using System.Collections.Generic;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEAcuerdo
    {
        public BEAcuerdo() { }
        public Int64 IdContrato { get; set; }
        public string CodPuntoVenta { get; set; }
        public string DesPuntoVenta { get; set; }
        public string NumeroPCS { get; set; }
        public string FechaContrato { get; set; }
        public Int64 Solin_codigo { get; set; }
        public string Resultado { get; set; }
        public string CodVendedor { get; set; }
        public string LimiteCredito { get; set; }
        public string ScoreCrediticio { get; set; }
        public string ControlFraude { get; set; }
        public string NroDocCliente { get; set; }
        public string TipoDocCliente { get; set; }
        public string DesTipoDocCliente { get; set; }
        public string CodTipoVenta { get; set; }
        public string DesTipoVenta { get; set; }
        public string CodTipoOperacion { get; set; }
        public string DesTipoOperacion { get; set; }
        public string Usuario { get; set; }
        public string CargoFijoTotal { get; set; }
        public string CodTipoClienteActivacion { get; set; }
        public string DesTipoClienteActivacion { get; set; }
        public string NombreVendedor { get; set; }
        public string NombreCliente { get; set; }
        public string ApellidoPatCliente { get; set; }
        public string ApellidoMatCliente { get; set; }
        public string RazonSocial { get; set; }
        public string AnalistaCredito { get; set; }
        public string Aprobador { get; set; }
    }
}
