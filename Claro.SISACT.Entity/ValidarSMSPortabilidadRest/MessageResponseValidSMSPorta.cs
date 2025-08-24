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
    public class MessageResponseValidSMSPorta
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersResponse header { get; set; }
        [DataMember(Name = "Body")]
        public BodyResponseValidSMSPorta body { get; set; }

        public MessageResponseValidSMSPorta()
        {
            header = new DataPowerRest.HeadersResponse();
            body = new BodyResponseValidSMSPorta();
        }
    }
}
