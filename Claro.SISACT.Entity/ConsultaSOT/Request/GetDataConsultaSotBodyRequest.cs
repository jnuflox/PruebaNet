using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ConsultaSOT.Request
{
    #region [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil]
    [DataContract]
    [Serializable]
    public class GetDataConsultaSotBodyRequest
    {
        [DataMember(Name = "numeroDocumento")]
        public string numeroDocumento { get; set; }

        [DataMember(Name = "tipoDocumento")]
        public string tipoDocumento { get; set; }

        [DataMember(Name = "tipoOperacion")]
        public string tipoOperacion { get; set; }

        [DataMember(Name = "tipoProducto")]
        public string tipoProducto { get; set; }
    }
    #endregion

}
