using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ConsultaSOT.Response
{
    #region [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil]
    [DataContract]
    [Serializable]
    public class GetDataConsultaSotTypeResponse
    {
        [DataMember(Name = "customerId")]
        public string customerId { get; set; }

        [DataMember(Name = "numeroSec")]
        public string numeroSec { get; set; }

        [DataMember(Name = "numeroSot")]
        public string numeroSot { get; set; }

        [DataMember(Name = "tipoProducto")]
        public string tipoProducto { get; set; }

        [DataMember(Name = "tipoVenta")]
        public string tipoVenta { get; set; }

        [DataMember(Name = "puntoVenta")]
        public string puntoVenta { get; set; }

        [DataMember(Name = "codigoEstado")]
        public string codigoEstado { get; set; }

        [DataMember(Name = "estadoSot")]
        public string estadoSot { get; set; }

        [DataMember(Name = "fecha")]
        public string fecha { get; set; }

        [DataMember(Name = "detalleSot")]
        public List<BEDetalleSot> detalleSot { get; set; }
    }
    #endregion
}
