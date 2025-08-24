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
    public class MessageRequestValidSMSPorta
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersRequest header { get; set; }
        [DataMember(Name = "Body")]
        public ValidarSMSPortabilidadRequest body { get; set; }

        public MessageRequestValidSMSPorta()
        {
            header = new DataPowerRest.HeadersRequest();
            body = new ValidarSMSPortabilidadRequest();
        }
    }
}
