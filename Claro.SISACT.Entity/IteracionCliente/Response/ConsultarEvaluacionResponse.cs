using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity.DataPowerRest.Generic;
//PROY-140618
namespace Claro.SISACT.Entity.IteracionCliente.Response
{
    public class ConsultarEvaluacionResponse : GenericResponse
    {
        public ConsultarEvaluacionResponse()
        {
            this.getMessageResponse().setBody(new ConsultarEvaluacionResponseBody());
        }
    }
}
