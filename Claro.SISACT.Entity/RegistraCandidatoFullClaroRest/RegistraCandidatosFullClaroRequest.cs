using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.RegistraCandidatoFullClaroRest
{
    //PROY-FULLCLARO
    [DataContract]
    [Serializable]
    public class RegistraCandidatosFullClaroRequest
    {
        [DataMember(Name = "MessageRequest")]
        public MessageRequestRegCandidatoFC MessageRequest { get; set; }

        public RegistraCandidatosFullClaroRequest()
        {
            MessageRequest = new MessageRequestRegCandidatoFC();
        }
    }
}
