using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ValidarSMSPortabilidadRest
{
    //PROY-SMS PORTABILIDAD
    [DataContract]
    [Serializable]
    public class ValidarSMSPortabilidadRequest
    {
        [DataMember(Name = "envioSms")]
        public string envioSms { get; set; }
        [DataMember(Name = "codigo")]
        public string codigo { get; set; }
    }
}
