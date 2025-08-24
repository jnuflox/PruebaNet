using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.VentasCuotas.ObtenerDatosPedidoAccCuotas.Response
{
    [DataContract]
    [Serializable]
    public class BEObtenerDatosPedidoAccCuotas
    {
        [DataMember(Name = "idVentaCuota")]
        public string idVentaCuota { get; set; }
        [DataMember(Name = "numeroPedido")]
        public string numeroPedido { get; set; }
        [DataMember(Name = "lineaFacturar")]
        public string lineaFacturar { get; set; }
        [DataMember(Name = "modalidadVenta")]
        public string modalidadVenta { get; set; }
        [DataMember(Name = "numeroCuotas")]
        public string numeroCuotas { get; set; }
        [DataMember(Name = "flagCargoRecibo")]
        public string flagCargoRecibo { get; set; }
        [DataMember(Name = "cuotaInicialFinal")]
        public string cuotaInicialFinal { get; set; }
        [DataMember(Name = "tipoDocEvalCred")]
        public string tipoDocEvalCred { get; set; }
        [DataMember(Name = "numDocEvalCred")]
        public string numDocEvalCred { get; set; }
        [DataMember(Name = "numeroSec")]
        public string numeroSec { get; set; }
        [DataMember(Name = "cuocCodigo")]
        public string cuocCodigo { get; set; }
        [DataMember(Name = "cuotasBrms")]
        public string cuotasBrms { get; set; }
        [DataMember(Name = "precioLista")]
        public string precioLista { get; set; }
        [DataMember(Name = "cargoFijo")]
        public string cargoFijo { get; set; }
        [DataMember(Name = "cuotaInicial")]
        public string cuotaInicial { get; set; }
        [DataMember(Name = "flagRRLLEvalCred")]
        public string flagRRLLEvalCred { get; set; }
        [DataMember(Name = "codPromocion")]
        public string codPromocion { get; set; }
        [DataMember(Name = "descPromocion")]
        public string descPromocion { get; set; }
        [DataMember(Name = "codAccesorio")]
        public string codAccesorio { get; set; }
        [DataMember(Name = "grupoMaterial")]
        public string grupoMaterial { get; set; }
        [DataMember(Name = "servidorSEC")]
        public string servidorSEC { get; set; }
        [DataMember(Name = "tipoProdFacturar")]
        public string tipoProdFacturar { get; set; }
        [DataMember(Name = "servidorVenta")]
        public string servidorVenta { get; set; }
        [DataMember(Name = "customerID")]
        public string customerID { get; set; }
        [DataMember(Name = "coID")]
        public string coID { get; set; }
        [DataMember(Name = "cuentaFacturar")]
        public string cuentaFacturar { get; set; }
        [DataMember(Name = "flagPagoSec")]
        public string flagPagoSec { get; set; }
        [DataMember(Name = "descPlanFijo")]
        public string descPlanFijo { get; set; }
        
    }
}
