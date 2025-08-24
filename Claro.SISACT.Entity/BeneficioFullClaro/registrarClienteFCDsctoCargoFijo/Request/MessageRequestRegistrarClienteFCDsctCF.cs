using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.BeneficioFullClaro.registrarClienteFCDsctoCargoFijo.Request
{
    [DataContract]
    [Serializable]
    public class MessageRequestRegistrarClienteFCDsctCF
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersRequest Header { get; set; }

        [DataMember(Name = "Body")]
        public BodyRequestRegistrarClienteFCDsctCF Body { get; set; }

        public MessageRequestRegistrarClienteFCDsctCF()
        {
            Header = new DataPowerRest.HeadersRequest();
            Body = new BodyRequestRegistrarClienteFCDsctCF();
        }
    }
}
