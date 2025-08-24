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
    public class RegistrarEvaluacionResponse
    {
        [DataMember(Name = "mensajeError")]
        public string mensajeError { get; set; }

        [DataMember(Name = "codigoRespuesta")]
        public string codigoRespuesta { get; set; }

        [DataMember(Name = "mensajeRespuesta")]
        public string mensajeRespuesta { get; set; }

        [DataMember(Name = "idEvaluacion")]
        public Int64 idEvaluacion { get; set; }
    }
}
