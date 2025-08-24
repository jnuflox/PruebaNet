using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.RegistraCandidatoFullClaroRest
{
    [DataContract]
    [Serializable]
    public class MessageResponseRegCandidatoFC
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersResponse header { get; set; }
        [DataMember(Name = "Body")]
        public BodyResponseRegCandidatoFC body { get; set; }

        public MessageResponseRegCandidatoFC()
        {
            header = new DataPowerRest.HeadersResponse();
            body = new BodyResponseRegCandidatoFC();
        }
    }
}
