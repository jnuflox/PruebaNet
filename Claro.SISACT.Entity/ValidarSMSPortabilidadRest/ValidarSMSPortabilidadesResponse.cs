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
    public class ValidarSMSPortabilidadesResponse
    {
        [DataMember(Name = "MessageResponse")]
        public MessageResponseValidSMSPorta MessageResponse { get; set; }

        public ValidarSMSPortabilidadesResponse()
        {
            MessageResponse = new MessageResponseValidSMSPorta();
        }
    }
}
