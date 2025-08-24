using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.DataPowerRest.Generic
{
    [Serializable]
    [DataContract()]
    public class MessageRequest
    {
        [DataMember(Name = "Header")]
        private HeadersRequest Header { get; set; }

        [DataMember(Name = "Body")]
        private IBodyRequest Body { get; set; }

        public HeadersRequest getHeader()
        {
            return this.Header;
        }

        public void setHeader(HeadersRequest objHeader)
        {
            this.Header = objHeader;
        }

        public IBodyRequest getBody()
        {
            return this.Body;
        }

        public void setBody(IBodyRequest objBody)
        {
            this.Body = objBody;
        }

        public MessageRequest()
        {
            this.Header = new HeadersRequest();
        }
    }
}
