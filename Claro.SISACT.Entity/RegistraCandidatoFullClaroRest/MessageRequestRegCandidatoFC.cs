using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.RegistraCandidatoFullClaroRest
{
    //PROY-FULLCLARO.V2
    [DataContract]
    [Serializable]
    public class MessageRequestRegCandidatoFC
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersRequest header { get; set; }
        [DataMember(Name = "Body")]
        public RegistraCandidatoFullClaroRequest body { get; set; }

        public MessageRequestRegCandidatoFC()
        {
            header = new DataPowerRest.HeadersRequest();
            body = new RegistraCandidatoFullClaroRequest();
        }
    }
}
