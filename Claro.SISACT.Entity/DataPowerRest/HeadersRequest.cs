using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
namespace Claro.SISACT.Entity.DataPowerRest
{
  
    //PROY-140245
    [DataContract]
    [Serializable] //PROY-140126 - IDEA 140248 
    public class HeadersRequest
    {
        [DataMember(Name = "HeaderRequest")]
        public HeaderRequest HeaderRequest { get; set; }
    }
}
