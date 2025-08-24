using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.RegistrarSMSPortabilidadRest
{
    //PROY-SMS PORTABILIDAD
    [DataContract]
    [Serializable]
    public class RegistrarSMSPortabilidadRequest
    {
        [DataMember(Name = "linea")]
        public string linea { get; set; }
        [DataMember(Name = "numeroDocumento")]
        public string numeroDocumento { get; set; }
        [DataMember(Name = "flagEnvioSMS")]
        public string flagEnvioSMS { get; set; }
        [DataMember(Name = "flagSMSIndico")]
        public string flagSMSIndico { get; set; }
    }
}
