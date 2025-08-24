using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Claro.SISACT.Entity.MotorPromociones.SimulacionMultilineas
{
    [Serializable]
    [DataContract]
    public class MessageRequestSimulacionMultilineas
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersRequest Header { get; set; }

        [DataMember(Name = "Body")]
        public BodyRequestSimulacionMultilineas Body { get; set; }

        public MessageRequestSimulacionMultilineas()
        {
            Header = new DataPowerRest.HeadersRequest();
            Body = new BodyRequestSimulacionMultilineas();
        }
    }
}
