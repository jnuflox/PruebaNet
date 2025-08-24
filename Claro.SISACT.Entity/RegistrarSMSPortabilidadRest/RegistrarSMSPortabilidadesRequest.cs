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
    public class RegistrarSMSPortabilidadesRequest
    {
        [DataMember(Name = "MessageRequest")]
        public MessageRequestRegSMSPorta MessageRequest { get; set; }

        public RegistrarSMSPortabilidadesRequest()
        {
            MessageRequest = new MessageRequestRegSMSPorta();
        }
    }
}
