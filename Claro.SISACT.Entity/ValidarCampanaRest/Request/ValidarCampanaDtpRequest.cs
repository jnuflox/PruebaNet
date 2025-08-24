using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ValidarCampanaRest.Request
{
    [DataContract]
    [Serializable]
    public class ValidarCampanaDtpRequest
    {
        [DataMember(Name = "MessageRequest")]
        public ValidarCampanaMessageRequest MessageRequest { get; set; }

        public ValidarCampanaDtpRequest()
        {
            MessageRequest = new ValidarCampanaMessageRequest();
        }
    }
}
