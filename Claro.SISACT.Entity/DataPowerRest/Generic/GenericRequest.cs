using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.DataPowerRest.Generic
{
    [Serializable]
    [DataContract()]
    public class GenericRequest
    {
        [DataMember(Name = "MessageRequest")]
        private MessageRequest MessageRequest;

        public GenericRequest()
        {
            MessageRequest = new MessageRequest();
        }


        public MessageRequest getMessageRequest()
        {
            return this.MessageRequest;
        }
    }
}
