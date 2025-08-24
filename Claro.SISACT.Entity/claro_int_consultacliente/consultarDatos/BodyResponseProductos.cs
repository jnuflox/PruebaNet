using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claro_int_consultacliente.consultarDatos
{
    [DataContract]
    [Serializable]
    public class BodyResponseProductos
    {
        [DataMember(Name = "productId")]
        public string productId { get; set; }

        [DataMember(Name = "productOfferingId")]
        public string productOfferingId { get; set; }

        [DataMember(Name = "productOfferingType")]
        public string productOfferingType { get; set; }

        [DataMember(Name = "productStatus")]
        public string productStatus { get; set; }

        [DataMember(Name = "effectiveDate")]
        public string effectiveDate { get; set; }

        [DataMember(Name = "cargoFijo")]
        public string cargoFijo { get; set; }

        [DataMember(Name = "cargoFijoCalculado")]
        public string cargoFijoCalculado { get; set; }
    }
}
