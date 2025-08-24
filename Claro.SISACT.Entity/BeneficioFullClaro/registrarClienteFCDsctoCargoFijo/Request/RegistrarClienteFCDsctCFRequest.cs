using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.BeneficioFullClaro.registrarClienteFCDsctoCargoFijo.Request
{
    [DataContract]
    [Serializable]
    public class RegistrarClienteFCDsctCFRequest
    {
        [DataMember(Name = "MessageRequest")]
        public MessageRequestRegistrarClienteFCDsctCF MessageRequest { get; set; }

        public RegistrarClienteFCDsctCFRequest()
        {
            MessageRequest = new MessageRequestRegistrarClienteFCDsctCF();
        }
    }
}
