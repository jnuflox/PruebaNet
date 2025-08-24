using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Claro.SISACT.Entity.DataPowerRest.Generic;

namespace Claro.SISACT.Entity.claro_vent_pedidostienda.Response
{
    [DataContract]
    [Serializable]
    public class PedidosTiendaResponseType
    {
        [DataMember(Name = "auditResponse")]
        public AuditResponse auditResponse {get;set;}

     
        [DataMember(Name = "dataResponse")]
        public BEDataResponse dataResponse { get; set; }

        public PedidosTiendaResponseType()
        {
            this.dataResponse = new BEDataResponse();
            this.auditResponse = new AuditResponse();
        }
    }


    
}
