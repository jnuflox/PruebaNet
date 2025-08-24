using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Claro.SISACT.Entity.DataPowerRest.Generic;

namespace Claro.SISACT.Entity.ConsultaSOT.Response
{
    #region [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil]
    [DataContract]
    [Serializable]
    public class GetDataConsultaSotBodyResponse
    {
        [DataMember(Name = "auditResponse")]
        public AuditResponse auditResponse { get; set; }

        [DataMember(Name = "datosSotCliente")]
        public List<GetDataConsultaSotTypeResponse> datosSotCliente { get; set; }
    }
    #endregion
}
