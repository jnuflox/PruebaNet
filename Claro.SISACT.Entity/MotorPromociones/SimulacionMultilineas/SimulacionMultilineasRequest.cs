using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Claro.SISACT.Entity.MotorPromociones.SimulacionMultilineas
{
    [Serializable]
    [DataContract]
    public class SimulacionMultilineasRequest
    {
        [DataMember(Name = "MessageRequest")]
        public MessageRequestSimulacionMultilineas MessageRequest { get; set; }

        public SimulacionMultilineasRequest()
        {
            MessageRequest = new MessageRequestSimulacionMultilineas();
        }
    }
}
