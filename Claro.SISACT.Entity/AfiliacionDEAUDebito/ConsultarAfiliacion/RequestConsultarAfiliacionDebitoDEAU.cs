using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.AfiliacionDEAUDebito.ConsultarAfiliacion
{

    [DataContract]
    [Serializable]
    public class RequestConsultarAfiliacionDebitoDEAU
    {
        [DataMember(Name = "MessageRequest")]
        public MessageRequestConsultarAfiliacionDEAUDebito MessageRequest { get; set; }

        public RequestConsultarAfiliacionDebitoDEAU()
        {
            MessageRequest = new MessageRequestConsultarAfiliacionDEAUDebito();
        }
    }
}
