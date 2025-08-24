using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.AfiliaciónDEAUAsistidosWSRest.EnviaLink
{
    [DataContract]
    [Serializable]
    public class BodyResponseEnviaLinkDEAU
    {
       /* [DataMember(Name="Body")]
    public BEEnviaLinkDEAUResponse body{get; set;}

        public BodyResponseEnviaLinkDEAU() {
            body = new BEEnviaLinkDEAUResponse();
        }*/

        [DataMember(Name = "codigoRespuesta")]
        public string codigoRespuesta { get; set; }

        [DataMember(Name = "mensajeRespuesta")]
        public string mensajeRespuesta { get; set; }

        [DataMember(Name = "idTransaccion")]
        public string idTransaccion { get; set; }

        [DataMember(Name = "linkCliente")]
        public string linkCliente { get; set; }

        [DataMember(Name = "fechaexpiracion")]
        public string fechaexpiracion { get; set; }
    }
}
