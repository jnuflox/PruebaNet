using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ValidarCampanaRest.Request
{
    [DataContract]
    [Serializable]
    public class ValidarCampanaMessageRequest
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersRequest header { get; set; }
        [DataMember(Name = "Body")]
        public ValidarCampanaBodyRequest body { get; set; }

        public ValidarCampanaMessageRequest()
        {
            header = new DataPowerRest.HeadersRequest();
            body = new ValidarCampanaBodyRequest();
        }
    }
}
