using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.AfiliaciónDEAUAsistidosWSRest.ConsultarAfiliacion
{
    [DataContract]
    [Serializable]
    public class MessageRequestConsultarAfiliacionDEAU
    {
         [DataMember(Name = "Header")]
        public DataPowerRest.HeadersRequest header { get; set; }
        [DataMember(Name = "Body")]
        public BodyRequestConsultarAfiliacionDEAU body { get; set; }

        public MessageRequestConsultarAfiliacionDEAU()
        {
            header = new DataPowerRest.HeadersRequest();
            body = new BodyRequestConsultarAfiliacionDEAU();
        }
    }
}
