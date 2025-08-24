using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ConsultarClienteFullClaroRest
{
    [DataContract]
    [Serializable]
    public class DataConsResponse
    {
        [DataMember(Name = "lineasFija")]
        public string lineasFija { get; set; }
        [DataMember(Name = "PlanesFija")]
        public string PlanesFija { get; set; }
        [DataMember(Name = "lineasMovil")]
        public string lineasMovil { get; set; }
        [DataMember(Name = "PlanesMovil")]
        public string PlanesMovil { get; set; }
        [DataMember(Name = "CustomerId")]
        public string CustomerId { get; set; }
    }
}
