using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.RegistrarOmisionPINRest
{
    [DataContract]
    [Serializable]
    public class BodyRequestRegistrarOmisionPIN
    {
        [DataMember(Name = "registrarOmisionPinRequest")]
        public BECabeceraOmisionPIN registrarOmisionPinRequest { get; set; }
    }
}
