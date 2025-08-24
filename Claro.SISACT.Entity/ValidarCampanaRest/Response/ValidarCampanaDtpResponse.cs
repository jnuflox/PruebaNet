using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ValidarCampanaRest.Response
{
    [DataContract]
    [Serializable]
    public class ValidarCampanaDtpResponse
    {
        [DataMember(Name = "MessageResponse")]
        public ValidarCampanaMessageResponse MessageResponse { get; set; }

        public ValidarCampanaDtpResponse()
        {
            MessageResponse = new ValidarCampanaMessageResponse();
        }
    }
}
