using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Claro.SISACT.Entity.GenericRequestResponse;
using Claro.SISACT.Entity.DataPowerRest.Generic;
using Claro.SISACT.Entity.claro_vent_pedidostienda.Response;

namespace Claro.SISACT.Entity.claro_vent_pedidostienda.Request
{
    [DataContract]
    [Serializable]
    public class BodyRequestAprobarPT : IBodyRequest
    {        
        [DataMember(Name = "aprobarExcepPreRequest")]
        public BEDatosPTDetalle aprobarExcepPreRequest { get; set; }

        [DataMember(Name = "anularExcepPreRequest")]
        public BEDatosPTDetalle anularExcepPreRequest { get; set; }

        [DataMember(Name = "actualizarExcepPreRequest")]
        public BEDatosPTDetalle actualizarExcepPreRequest { get; set; }


    }
}
