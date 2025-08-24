using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claro_vent_fullclaroCBIO.consultarPendientesActivasCBIO
{
    [DataContract]
    [Serializable]
    public class RequestConsultarPendientesActivasCBIO
    {
        [DataMember(Name = "consultarPendienteActivaCBIORequestType")]
        public ConsultarPendientesActivasCBIORequest consultarPendientesActivasCBIORequest { get; set; }

        public RequestConsultarPendientesActivasCBIO()
        {
            consultarPendientesActivasCBIORequest = new ConsultarPendientesActivasCBIORequest();
        }
    }
}
