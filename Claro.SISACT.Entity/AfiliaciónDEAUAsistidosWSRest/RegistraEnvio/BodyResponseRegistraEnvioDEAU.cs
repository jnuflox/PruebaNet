using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.AfiliaciónDEAUAsistidosWSRest.RegistraEnvio
{
    [DataContract]
    [Serializable]
    public class BodyResponseRegistraEnvioDEAU
    {
        //[DataMember(Name = "idTransaccion")]
        //public string idTransaccion { get; set; }
        [DataMember(Name = "codigoRespuesta")]
        public string codigoRespuesta { get; set; }
        [DataMember(Name = "mensajeRespuesta")]
        public string mensajeRespuesta { get; set; }
        [DataMember(Name = "idAfiliacion")]
        public string idAfiliacion { get; set; }

    }
}
