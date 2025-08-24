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
    public class HeaderResponse
    {
        [DataMember(Name = "consumer")]
        public string Consumer { get; set; }
        [DataMember(Name = "pid")]
        public string Pid { get; set; }
        [DataMember(Name = "timestamp")]
        public string Timestamp { get; set; }
        [DataMember(Name = "varArg")]
        public string VarArg { get; set; }
        [DataMember(Name = "status")]
        public Status Status { get; set; }
    }
}
