using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claro_int_consultacliente.consultarDatos
{
    [DataContract]
    [Serializable]
    public class BodyResponseCustomer
    {
        [DataMember(Name = "customerId")]
        public string customerId { get; set; }

        [DataMember(Name = "custCode")]
        public string custCode { get; set; }

        [DataMember(Name = "customerIdPub")]
        public string customerIdPub { get; set; }

        [DataMember(Name = "status")]
        public string status { get; set; }

        [DataMember(Name = "billingCycle")]
        public string billingCycle { get; set; }

        [DataMember(Name = "contratos")]
        public List<BodyResponseContrato> contratos { get; set; }

        [DataMember(Name = "cargoFijo")]
        public string cargoFijo { get; set; }
    }
}
