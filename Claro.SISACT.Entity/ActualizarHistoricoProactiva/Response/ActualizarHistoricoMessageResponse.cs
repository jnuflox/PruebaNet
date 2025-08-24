using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ActualizarHistoricoProactiva.Response
{
    [DataContract]
    [Serializable]
    public class ActualizarHistoricoMessageResponse
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersResponse header { get; set; }

        [DataMember(Name = "Body")]
        public ActualizarHistoricoBodyResponse body { get; set; }

        public ActualizarHistoricoMessageResponse()
        {
            header = new DataPowerRest.HeadersResponse();
            body = new ActualizarHistoricoBodyResponse();
        }
    }
}
