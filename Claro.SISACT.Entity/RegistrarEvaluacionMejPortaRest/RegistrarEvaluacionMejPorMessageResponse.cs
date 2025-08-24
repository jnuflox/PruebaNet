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
    public class RegistrarEvaluacionMejPorMessageResponse
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersResponse header { get; set; }

        [DataMember(Name = "Body")]
        public BodyRegistrarEvaluacionResponse body { get; set; }

        public RegistrarEvaluacionMejPorMessageResponse()
        {
            header = new DataPowerRest.HeadersResponse();
            body = new BodyRegistrarEvaluacionResponse();
        }
    }
}
