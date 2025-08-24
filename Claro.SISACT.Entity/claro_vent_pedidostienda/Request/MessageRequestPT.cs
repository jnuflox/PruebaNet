using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Claro.SISACT.Entity.DataPowerRest.Generic;


namespace Claro.SISACT.Entity.claro_vent_pedidostienda.Request
{
    [DataContract]
    [Serializable]
    public class MessageRequestPT:GenericRequest
    {
        public MessageRequestPT()
        {
            this.getMessageRequest().setBody(new BodyRequestAprobarPT());
        }
    }
}
