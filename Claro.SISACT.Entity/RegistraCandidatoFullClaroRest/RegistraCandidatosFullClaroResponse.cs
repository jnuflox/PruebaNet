using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.RegistraCandidatoFullClaroRest
{
    [DataContract]
    [Serializable]
    public class RegistraCandidatosFullClaroResponse
    {
        [DataMember(Name = "MessageResponse")]
        public MessageResponseRegCandidatoFC MessageResponse { get; set; }

        public RegistraCandidatosFullClaroResponse()
        {
            MessageResponse = new MessageResponseRegCandidatoFC();
        }
    }
}
