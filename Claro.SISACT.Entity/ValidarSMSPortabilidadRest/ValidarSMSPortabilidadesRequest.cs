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
    public class ValidarSMSPortabilidadesRequest
    {
        [DataMember(Name = "MessageRequest")]
        public MessageRequestValidSMSPorta MessageRequest { get; set; }

        public ValidarSMSPortabilidadesRequest()
        {
            MessageRequest = new MessageRequestValidSMSPorta();
        }
    }
}
