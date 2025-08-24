using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.AfiliacionDEAUDebito.ConsultarAfiliacion
{

    [DataContract]
    [Serializable]
    public class MessageRequestConsultarAfiliacionDEAUDebito
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersRequest header { get; set; }
        [DataMember(Name = "Body")]
        public BodyRequestConsultarAfiliacionDEAUDebito body { get; set; }

        public MessageRequestConsultarAfiliacionDEAUDebito()
        {
            header = new DataPowerRest.HeadersRequest();
            body = new BodyRequestConsultarAfiliacionDEAUDebito();
        }
    }
}
