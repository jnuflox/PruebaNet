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
    public class RegistrarEvaluacionMejoraPortaResponse
    {
        [DataMember(Name = "MessageResponse")]
        public RegistrarEvaluacionMejPorMessageResponse MessageResponse { get; set; }

        public RegistrarEvaluacionMejoraPortaResponse()
        {
            MessageResponse = new RegistrarEvaluacionMejPorMessageResponse();
        }

    }
}
