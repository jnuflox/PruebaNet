using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity.DataPowerRest.Generic;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claro_vent_pedidostienda.Response
{
    [DataContract]
    [Serializable]
    public class PedidosTiendaResponseBody : IBodyResponse
    {

        [DataMember(Name = "auditResponse")]
        public AuditResponse auditResponse {get;set;}

     
        [DataMember(Name = "dataResponse")]
        public BEDataResponse dataResponse { get; set; }




        public PedidosTiendaResponseBody()
        {
            this.dataResponse = new BEDataResponse();
            this.auditResponse = new AuditResponse();
        }


    }
}
