using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claro_vent_fullclaroCBIO.consultarActivosCBIO
{
    [DataContract]
    [Serializable]
    public class Request
    {
        [DataMember(Name = "consultarActivosCBIORequest")]
        public ConsultarActivosCBIORequest consultarActivosCBIORequest { get; set; }

        public Request()
        {
            consultarActivosCBIORequest = new ConsultarActivosCBIORequest();
        }
    }
}
