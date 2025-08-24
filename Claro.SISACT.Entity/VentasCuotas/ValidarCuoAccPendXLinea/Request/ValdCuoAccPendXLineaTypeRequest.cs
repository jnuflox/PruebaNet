using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Claro.SISACT.Entity.GenericRequestResponse;

namespace Claro.SISACT.Entity.VentasCuotas.ValidarCuoAccPendXLinea.Request
{
    [DataContract]
    [Serializable]
    public class ValdCuoAccPendXLineaTypeRequest
    {
        [DataMember(Name = "numeroPedido")]
        public string numeroPedido { get; set; }

        [DataMember(Name = "lineaFacturar")]
        public string lineaFacturar { get; set; }

        [DataMember(Name = "tipoDocCliente")]
        public string tipoDocCliente { get; set; }

        [DataMember(Name = "docCliente")]
        public string docCliente { get; set; }

        [DataMember(Name = "cantidadXhoras")]
        public string cantidadXhoras { get; set; }

        [DataMember(Name = "coID")]
        public string coID { get; set; }

        [DataMember(Name = "listaOpcional")]
        public List<BEListaOpcional> listaOpcional { get; set; }

    }
}
