using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.DataPowerRest.Generic
{
  
    [DataContract]
    [Serializable] 
    public class HeadersRequest
    {
        [DataMember(Name = "HeaderRequest")]
        public HeaderRequest HeaderRequest { get; set; }
        
        public void setHeader( HeaderRequest objHeaderRequest)
        {
            this.HeaderRequest = objHeaderRequest;
        }
    }
}
