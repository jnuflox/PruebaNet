using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.AfiliaciónDEAUAsistidosWSRest.RegistraEnvio
{
    [DataContract]
    [Serializable]
    public class MessageResponseRegistraEnvioDEAU
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeaderResponse header { get; set; }

        [DataMember(Name = "Body")]
        public BodyResponseRegistraEnvioDEAU body { get; set; }

        public MessageResponseRegistraEnvioDEAU()
        {
            header = new DataPowerRest.HeaderResponse();
            body = new BodyResponseRegistraEnvioDEAU();
        }
    }
}
