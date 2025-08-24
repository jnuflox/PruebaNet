using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.DatosLineaRest
{
    [DataContract]
    [Serializable]
    public class ResponseDataProductos
    {
        [DataMember(Name = "productOfferingId")]
        public string productOfferingId { get; set; }

        [DataMember(Name = "productOfferingType")]
        public string productOfferingType { get; set; }

        [DataMember(Name = "productStatus")]
        public string productStatus { get; set; }

        [DataMember(Name = "effectiveDate")]
        public string effectiveDate { get; set; }
    }
}
