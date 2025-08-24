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
    public class BodyRegistrarEvaluacionMejPor
    {
        [DataMember(Name = "registrarEvaluacionRequest")]
        public RegistrarEvaluacionRequest registrarEvaluacionRequest { get; set; }

        public BodyRegistrarEvaluacionMejPor()
        {
            registrarEvaluacionRequest = new RegistrarEvaluacionRequest();
        }
    }
}
