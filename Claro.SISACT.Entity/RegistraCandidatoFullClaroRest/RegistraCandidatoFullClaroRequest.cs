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
    public class RegistraCandidatoFullClaroRequest
    {
        [DataMember(Name = "registrarCandidatoRequest")]
        public BECabeceraFC registrarCandidatoRequest { get; set; }
        
    }
}
