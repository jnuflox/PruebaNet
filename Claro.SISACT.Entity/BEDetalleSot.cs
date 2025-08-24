using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity
{
    #region [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil]
    [DataContract]
    [Serializable]
    public class BEDetalleSot
    {
        [DataMember(Name = "numeroSot")]
        public string numeroSot { get; set; }

        [DataMember(Name = "codigoEstado")]
        public string codigoEstado { get; set; }

        [DataMember(Name = "estadoSot")]
        public string estadoSot { get; set; }

        [DataMember(Name = "fecha")]
        public string fecha { get; set; }

        [DataMember(Name = "hora")]
        public string hora { get; set; }

        [DataMember(Name = "descripcion")]
        public string descripcion { get; set; }
    }
    #endregion
}
