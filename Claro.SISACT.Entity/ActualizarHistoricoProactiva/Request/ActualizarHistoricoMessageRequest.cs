using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ActualizarHistoricoProactiva.Request
{
    [DataContract]
    [Serializable] 
    public  class ActualizarHistoricoMessageRequest
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersRequest header { get; set; }
        [DataMember(Name = "Body")]
        public ActualizarHistoricoBodyRequest body { get; set; }

        public ActualizarHistoricoMessageRequest()
        {
            header = new DataPowerRest.HeadersRequest();
            body = new ActualizarHistoricoBodyRequest();
        }
    }
}
