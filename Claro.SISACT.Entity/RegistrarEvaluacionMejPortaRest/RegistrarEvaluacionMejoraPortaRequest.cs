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
    public class RegistrarEvaluacionMejoraPortaRequest
    {
        [DataMember(Name = "MessageRequest")]
        public  RegistrarEvaluacionMejPorMessageRequest MessageRequest { get; set;}

        public RegistrarEvaluacionMejoraPortaRequest()
        {
            MessageRequest = new RegistrarEvaluacionMejPorMessageRequest();
        }
    }
}
