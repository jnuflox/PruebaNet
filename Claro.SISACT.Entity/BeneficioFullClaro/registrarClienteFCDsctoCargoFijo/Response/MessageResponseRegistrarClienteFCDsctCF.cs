using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.BeneficioFullClaro.registrarClienteFCDsctoCargoFijo.Response
{
    [DataContract]
    [Serializable]
    public class MessageResponseRegistrarClienteFCDsctCF
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersResponse Header { get; set; }

        [DataMember(Name = "Body")]
        public BodyResponseRegistrarClienteFCDsctCF Body { get; set; }

        public MessageResponseRegistrarClienteFCDsctCF()
        {
            Header = new DataPowerRest.HeadersResponse();
            Body = new BodyResponseRegistrarClienteFCDsctCF();
        }
    }
}
