using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
//PROY-140618
namespace Claro.SISACT.Entity.RegistrarEvaluacionMejPortaRest
{
    [DataContract]
    [Serializable]
    public class RegistrarEvaluacionMejPorMessageRequest
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersRequest header { get; set; }
        [DataMember(Name = "Body")]
        public BodyRegistrarEvaluacionMejPor body { get; set; }

        public RegistrarEvaluacionMejPorMessageRequest()
        {
            header = new DataPowerRest.HeadersRequest();
            body = new BodyRegistrarEvaluacionMejPor();
        }
    }
}
