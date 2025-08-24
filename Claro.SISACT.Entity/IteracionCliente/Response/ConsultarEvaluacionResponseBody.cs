using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity.DataPowerRest.Generic;
using System.Runtime.Serialization;
//PROY-140618
namespace Claro.SISACT.Entity.IteracionCliente.Response
{
    [DataContract]
    [Serializable]
    public class ConsultarEvaluacionResponseBody : IBodyResponse
    {
        [DataMember(Name = "consultarEvaluacionResponse")]
        public ConsultarEvaluacionResponseType consultarEvaluacionResponse { get; set; }

        public ConsultarEvaluacionResponseBody()
        {
            this.consultarEvaluacionResponse = new ConsultarEvaluacionResponseType();
        }
    }
}
