using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claro_vent_pedidostienda.Request
{
    [DataContract]
    [Serializable]
    public class MessageResponsePT
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeaderResponse Header { get; set; }

        [DataMember(Name = "Body")]
        public  BodyRequestAprobarPT Body{ get; set; }

        public MessageResponsePT()
        {
            Header = new DataPowerRest.HeaderResponse();
            Body = new BodyRequestAprobarPT();
        }

    }
}
