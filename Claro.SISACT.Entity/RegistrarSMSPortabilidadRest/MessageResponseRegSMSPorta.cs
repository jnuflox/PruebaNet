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
    public class MessageResponseRegSMSPorta
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersResponse header { get; set; }
        [DataMember(Name = "Body")]
        public BodyResponseRegSMSPorta body { get; set; }

        public MessageResponseRegSMSPorta()
        {
            header = new DataPowerRest.HeadersResponse();
            body = new BodyResponseRegSMSPorta();
        }
    }
}
