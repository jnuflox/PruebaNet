using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.BeneficioFullClaro.validarAplicaDsctoCargoFijo.Request
{
    [DataContract]
    [Serializable]
    public class MessageRequestValidarAplicaDsctoCF
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersRequest Header { get; set; }

        [DataMember(Name = "Body")]
        public BodyRequestValidarAplicaDsctoCF Body { get; set; }

        public MessageRequestValidarAplicaDsctoCF()
        {
            Header = new DataPowerRest.HeadersRequest();
            Body = new BodyRequestValidarAplicaDsctoCF();
        }
    }
}
