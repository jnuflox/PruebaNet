using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.BeneficioFullClaro.registrarClienteFCDsctoCargoFijo.Response
{
    [DataContract]
    [Serializable]
    public class RegistrarClienteFCDsctCFResponse
    {
        [DataMember(Name = "MessageResponse")]
        public MessageResponseRegistrarClienteFCDsctCF MessageResponse { get; set; }

        public RegistrarClienteFCDsctCFResponse()
        {
            MessageResponse = new MessageResponseRegistrarClienteFCDsctCF();
        }
    }
}
