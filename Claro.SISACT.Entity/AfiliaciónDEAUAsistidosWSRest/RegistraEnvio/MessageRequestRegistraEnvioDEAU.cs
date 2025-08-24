using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.AfiliaciónDEAUAsistidosWSRest.RegistraEnvio
{
    [DataContract]
    [Serializable]
    public class MessageRequestRegistraEnvioDEAU
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersRequest header { get; set; }
        [DataMember(Name = "Body")]
        public BodyRequestRegistraEnvioDEAU body { get; set; }

        public MessageRequestRegistraEnvioDEAU() {

            header = new DataPowerRest.HeadersRequest();
            body = new BodyRequestRegistraEnvioDEAU();
        }


    }
}
