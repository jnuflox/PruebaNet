using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.DataPowerRest.Generic
{
    [DataContract]
    [Serializable] 
    public class MessageResponse
    {
        [DataMember(Name = "Header")]
        private HeadersResponse Header;

        [DataMember(Name = "Body")]
        private IBodyResponse Body;

        public HeadersResponse getHeader()
        {
            return this.Header;
        }

        public IBodyResponse getBody()
        {
            return this.Body;
        }

        public void setBody(IBodyResponse objBody)
        {
            this.Body = objBody;
        }

        public MessageResponse()
        {
            this.Header = new HeadersResponse();
        }
    }
}
