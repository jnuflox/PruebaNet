using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.BeneficioFullClaro.validarAplicaDsctoCargoFijo.Response
{
    [DataContract]
    [Serializable]
    public class ValidarAplicaDsctoCFResponse
    {
        [DataMember(Name = "MessageResponse")]
        public MessageResponseValidarAplicaDsctoCF MessageResponse { get; set; }

        public ValidarAplicaDsctoCFResponse()
        {
            MessageResponse = new MessageResponseValidarAplicaDsctoCF();
        }
    }
}
