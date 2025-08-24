using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace Claro.SISACT.Entity.ValidarCampanaRest.Response
{
    [DataContract]
    [Serializable]
    public class ValidarCampanaBodyResponse
    {
        [DataMember(Name = "validarCampanaResponse")]
        public ValidarCampanaResponse validarCampanaResponse { get; set; }
    }
}
