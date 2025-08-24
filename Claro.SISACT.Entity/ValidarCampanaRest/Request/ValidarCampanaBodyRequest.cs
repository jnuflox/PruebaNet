using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ValidarCampanaRest.Request
{
    public class ValidarCampanaBodyRequest
    {
        [DataMember(Name = "validarCampanaRequest")]
        public ValidarCampanaRequest validarCampanaRequest { get; set; }
    }
}
