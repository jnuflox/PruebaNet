using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.DataPowerRest.Generic
{
    
    [DataContract]
    [Serializable] 
    public class Status
    {
        [DataMember(Name = "type")]
        public string type { get; set; }
        [DataMember(Name = "code")]
        public string code { get; set; }
        [DataMember(Name = "message")]
        public string message { get; set; }
        [DataMember(Name = "msgid")]
        public string msgid { get; set; }
    }
}
