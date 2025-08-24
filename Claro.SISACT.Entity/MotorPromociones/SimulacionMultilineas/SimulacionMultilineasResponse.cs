using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Claro.SISACT.Entity.MotorPromociones.SimulacionMultilineas
{
    [Serializable]
    [DataContract]
    public class SimulacionMultilineasResponse
    {
        [DataMember(Name = "MessageResponse")]
        public MessageResponseSimulacionMultilineas MessageResponse { get; set; }

        public SimulacionMultilineasResponse()
        {
            MessageResponse = new MessageResponseSimulacionMultilineas();
        }
    }
}
