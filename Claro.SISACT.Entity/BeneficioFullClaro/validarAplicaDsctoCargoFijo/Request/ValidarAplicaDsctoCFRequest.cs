using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.BeneficioFullClaro.validarAplicaDsctoCargoFijo.Request
{
    [DataContract]
    [Serializable]
    public class ValidarAplicaDsctoCFRequest
    {
        [DataMember(Name = "MessageRequest")]
        public MessageRequestValidarAplicaDsctoCF MessageRequest { get; set; }

        public ValidarAplicaDsctoCFRequest()
        {
            MessageRequest = new MessageRequestValidarAplicaDsctoCF();
        }
    }
}
