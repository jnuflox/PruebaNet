using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Claro.SISACT.Entity.GenericRequestResponse;

namespace Claro.SISACT.Entity.VentasCuotas.RegistrarVtaAccCuotas.Request
{
    [DataContract]
    [Serializable]
    public class RegistrarVentaAccCuotasTypeRequest
    {
        [DataMember(Name = "numeroPedido")]
        public string numeroPedido { get; set; }

        [DataMember(Name = "lineaFacturar")]
        public string lineaFacturar { get; set; }

        [DataMember(Name = "modalidadVenta")]
        public string modalidadVenta { get; set; }

        [DataMember(Name = "nroCuotas")]
        public string nroCuotas { get; set; }

        [DataMember(Name = "flagCargoRecibo")]
        public string flagCargoRecibo { get; set; }

        [DataMember(Name = "cuotaInicialFinal")]
        public string cuotaInicialFinal { get; set; }

        [DataMember(Name = "tipoDocEvalCred")]
        public string tipoDocEvalCred { get; set; }

        [DataMember(Name = "nroDocEvalCred")]
        public string nroDocEvalCred { get; set; }

        [DataMember(Name = "numeroSec")]
        public string numeroSec { get; set; }

        [DataMember(Name = "cuocCodigo")]
        public string cuocCodigo { get; set; }

        [DataMember(Name = "cuotasBrms")]
        public string cuotasBrms { get; set; }

        [DataMember(Name = "cuotaInicialBrms")]
        public string cuotaInicialBrms { get; set; }

        [DataMember(Name = "precioLista")]
        public string precioLista { get; set; }

        [DataMember(Name = "cargoFijo")]
        public string cargoFijo { get; set; }

        [DataMember(Name = "cuotaInicial")]
        public string cuotaInicial { get; set; }

        [DataMember(Name = "flagRepreLegalEC")]
        public string flagRepreLegalEC { get; set; }

        [DataMember(Name = "codPromocion")]
        public string codPromocion { get; set; }

        [DataMember(Name = "descPromocion")]
        public string descPromocion { get; set; }

        [DataMember(Name = "codAccesorio")]
        public string codAccesorio { get; set; }

        [DataMember(Name = "grupoMaterial")]
        public string grupoMaterial { get; set; }

        [DataMember(Name = "servidorSec")]
        public string servidorSec { get; set; }

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

        [DataMember(Name = "descMaterialAcc")]
        public string descMaterialAcc { get; set; }

        [DataMember(Name = "listaOpcional")]
        public List<BEListaOpcional> listaOpcional { get; set; }

        //[PROY-140743-VENTA DE ACCESORIOS EN CUOTAS][FENCALAD]
        [DataMember(Name = "descPlanFijo")]
        public string descPlanFijo { get; set; }

  
    }

}
