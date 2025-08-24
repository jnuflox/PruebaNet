using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ValidarCampanaRest.Response
{
    [DataContract]
    [Serializable]
    public class ValidarCampanaMessageResponse
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersResponse header { get; set; }

        [DataMember(Name = "Body")]
        public ValidarCampanaBodyResponse body { get; set; }

        public ValidarCampanaMessageResponse()
        {
            header = new DataPowerRest.HeadersResponse();
            body = new ValidarCampanaBodyResponse();
        }
    }
}
