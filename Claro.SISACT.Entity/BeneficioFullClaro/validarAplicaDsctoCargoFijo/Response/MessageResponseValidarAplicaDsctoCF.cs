using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.BeneficioFullClaro.validarAplicaDsctoCargoFijo.Response
{
    [DataContract]
    [Serializable]
    public class MessageResponseValidarAplicaDsctoCF
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersResponse Header { get; set; }

        [DataMember(Name = "Body")]
        public BodyResponseValidarAplicaDsctoCF Body { get; set; }

        public MessageResponseValidarAplicaDsctoCF()
        {
            Header = new DataPowerRest.HeadersResponse();
            Body = new BodyResponseValidarAplicaDsctoCF();
        }
    }
}
