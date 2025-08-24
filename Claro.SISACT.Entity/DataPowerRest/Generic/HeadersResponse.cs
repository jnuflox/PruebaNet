using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.DataPowerRest.Generic
{
    
    [DataContract]
    [Serializable]  
    public class HeadersResponse
    {
        [DataMember(Name = "HeaderResponse")]
        public HeaderResponse HeaderResponse { get; set; }

        public void setHeaderResponse(HeaderResponse objHeader)
        {
            this.HeaderResponse = objHeader;
        }
    }
}
