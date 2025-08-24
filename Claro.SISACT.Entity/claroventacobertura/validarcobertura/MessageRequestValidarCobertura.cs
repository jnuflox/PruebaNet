using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claroventacobertura.validarcobertura
{
    [DataContract]
    [Serializable]
    public class MessageRequestValidarCobertura
    {
       [DataMember(Name = "Header")]
        public DataPowerRest.HeadersRequest Header { get; set; }

        [DataMember(Name = "Body")]
        public BodyRequestValidarCobertura Body { get; set; }

        public MessageRequestValidarCobertura()
        {
            Header = new DataPowerRest.HeadersRequest();
            Body = new BodyRequestValidarCobertura();
        }
    }
}
