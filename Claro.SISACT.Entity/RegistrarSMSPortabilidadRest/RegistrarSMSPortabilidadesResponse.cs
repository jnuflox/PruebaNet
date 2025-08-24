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
    public class RegistrarSMSPortabilidadesResponse
    {
        [DataMember(Name = "MessageResponse")]
        public MessageResponseRegSMSPorta MessageResponse { get; set; }

        public RegistrarSMSPortabilidadesResponse()
        {
            MessageResponse = new MessageResponseRegSMSPorta();
        }
    }
}
