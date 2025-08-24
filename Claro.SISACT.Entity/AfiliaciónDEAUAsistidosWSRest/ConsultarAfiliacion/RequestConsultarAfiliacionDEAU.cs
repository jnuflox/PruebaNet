using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.AfiliaciónDEAUAsistidosWSRest.ConsultarAfiliacion
{
    [DataContract]
    [Serializable]
    public class RequestConsultarAfiliacionDEAU
    {
         [DataMember(Name = "MessageRequest")]
        public MessageRequestConsultarAfiliacionDEAU MessageRequest { get; set; }

         public RequestConsultarAfiliacionDEAU()
        {
            MessageRequest = new MessageRequestConsultarAfiliacionDEAU();
        }
    }
}
