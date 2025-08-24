using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Claro.SISACT.Entity.MotorPromociones.SimulacionMultilineas
{
    [Serializable]
    [DataContract]
    public class MessageResponseSimulacionMultilineas
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersResponse header { get; set; }

        [DataMember(Name = "Body")]
        public BodyResponseSimulacionMultilineas body { get; set; }

        public MessageResponseSimulacionMultilineas()
        {
            header = new DataPowerRest.HeadersResponse();
            body = new BodyResponseSimulacionMultilineas();
        }
    }
}
