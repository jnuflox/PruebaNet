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
    public class MessageRequestRegSMSPorta
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersRequest header { get; set; }
        [DataMember(Name = "Body")]
        public RegistrarSMSPortabilidadRequest body { get; set; }

        public MessageRequestRegSMSPorta()
        {
            header = new DataPowerRest.HeadersRequest();
            body = new RegistrarSMSPortabilidadRequest();
        }
    }
}
