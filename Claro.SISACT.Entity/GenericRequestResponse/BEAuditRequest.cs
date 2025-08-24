using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.GenericRequestResponse
{
    [DataContract]
    [Serializable]
    public class BEAuditRequest
    {
        [DataMember(Name = "idTransaccion")]
        public string idTransaccion { get; set; }
        [DataMember(Name = "ipAplicacion")]
        public string ipAplicacion { get; set; }
        [DataMember(Name = "nombreAplicacion")]
        public string nombreAplicacion { get; set; }
        [DataMember(Name = "usuarioAplicacion")]
        public string usuarioAplicacion { get; set; }
    }
}
