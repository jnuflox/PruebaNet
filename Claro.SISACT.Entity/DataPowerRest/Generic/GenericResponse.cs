using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.DataPowerRest.Generic
{
    [DataContract()]
    [Serializable]
    public class GenericResponse
    {
        [DataMember(Name = "MessageResponse")]
        private MessageResponse MessageResponse;

        public MessageResponse getMessageResponse()
        {
            return this.MessageResponse;
        }

        public GenericResponse()
        {
            this.MessageResponse = new MessageResponse();
        }
    }
}
