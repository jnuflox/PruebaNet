using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ValidarCampanaRest.Response
{
    [DataContract]
    [Serializable]
    public class ValidarCampanaResponse
    {
        [DataMember(Name = "responseStatus")]
        public ValidarCampanaStatusResponse responseStatus { get; set; }

        [DataMember(Name = "responseData")]
        public ValidarCampanaDataResponse responseData { get; set; }
    }
}
