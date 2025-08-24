using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claroventacobertura.validarcobertura
{
    [DataContract]
    [Serializable]
    public class MessageResponseValidarCobertura
    {
         [DataMember(Name = "Header")]
        public DataPowerRest.HeadersResponse header { get; set; }

        [DataMember(Name = "Body")]
        public BodyResponseValidarCobertura body { get; set; }

        public MessageResponseValidarCobertura()
        {
            header = new DataPowerRest.HeadersResponse();
            body = new BodyResponseValidarCobertura();
        }
    }
}
