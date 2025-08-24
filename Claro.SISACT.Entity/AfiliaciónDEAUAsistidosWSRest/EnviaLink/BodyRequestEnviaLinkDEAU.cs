using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.AfiliaciónDEAUAsistidosWSRest.EnviaLink
{
    [DataContract]
    [Serializable]
    public class BodyRequestEnviaLinkDEAU

    {
        /*[DataMember(Name="Body")]
        public BEEnviaLinkDEAURequest body { get; set; }

        public BodyRequestEnviaLinkDEAU()
        {
            body = new BEEnviaLinkDEAURequest();
        }*/

        [DataMember(Name = "idTransaccion")]
        public string idTransaccion { get; set; }
        [DataMember(Name = "tipoFlujo")]
        public string tipoFlujo { get; set; }
        [DataMember(Name = "msisdn")]
        public string msisdn { get; set; }
        [DataMember(Name = "codCanal")]
        public string codCanal { get; set; }
        [DataMember(Name = "correo")]
        public string correo { get; set; }
        [DataMember(Name = "inicioVigenciaLink")]
        public string inicioVigenciaLink { get; set; }
        [DataMember(Name = "finVigenciaLink")]
        public string finVigenciaLink { get; set; }
        [DataMember(Name = "clickMaximoLink")]
        public string clickMaximoLink { get; set; }
        [DataMember(Name = "longitudHash")]
        public string longitudHash { get; set; }
        [DataMember(Name = "descripcion")]
        public string descripcion { get; set; }
        [DataMember(Name = "ip")]
        public string ip { get; set; }

    }
}
