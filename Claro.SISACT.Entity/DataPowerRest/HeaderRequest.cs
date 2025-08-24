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
    public class HeaderRequest
    {
        [DataMember(Name = "consumer")]
        public string consumer { get; set; }
        [DataMember(Name = "country")]
        public string country { get; set; }
        [DataMember(Name = "dispositivo")]
        public string dispositivo { get; set; }
        [DataMember(Name = "language")]
        public string language { get; set; }
        [DataMember(Name = "modulo")]
        public string modulo { get; set; }
        [DataMember(Name = "msgType")]
        public string msgType { get; set; }
        [DataMember(Name = "operation")]
        public string operation { get; set; }
        [DataMember(Name = "pid")]
        public string pid { get; set; }
        [DataMember(Name = "system")]
        public string system { get; set; }
        [DataMember(Name = "timestamp")]
        public string timestamp { get; set; }
        [DataMember(Name = "userId")]
        public string userId { get; set; }
        [DataMember(Name = "wsIp")]
        public string wsIp { get; set; }
        [DataMember(Name = "VarArg")]
        public string VarArg { get; set; }
        //JRM - INI - INTICO
        [DataMember(Name = "access_token")]
        public string access_token { get; set; }
        //JRM - FIN - INTICO
    }
}
