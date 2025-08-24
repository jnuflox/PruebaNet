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
    public class DataRegResponse
    {
        [DataMember(Name = "envioSms")]
        public string envioSms { get; set; }
        [DataMember(Name = "numeroIntentos")]
        public string numeroIntentos { get; set; }
        [DataMember(Name = "numeroReintentos")]
        public string numeroReintentos { get; set; }
        [DataMember(Name = "tiempoVentanaModal")]
        public string tiempoVentanaModal { get; set; }
    }
}
