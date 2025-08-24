using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claro_vent_fullclaroCBIO.consultarActivosCBIO
{
    [DataContract]
    [Serializable]
    public class ResponseDataConPendActivaCBIO
    {
        [DataMember(Name = "cursorClienteFC")]
        public List<CursorClienteFC> cursorClienteFC { get; set; }
    }
}
