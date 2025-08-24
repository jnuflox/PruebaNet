using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Claro.SISACT.Entity.GenericRequestResponse;

namespace Claro.SISACT.Entity.claro_vent_fullclaroCBIO
{
    [DataContract]
    [Serializable]
    public class ConsultarPendientesActivasCBIORequest
    {
        [DataMember(Name = "tipoDocumento")]
        public string tipoDocumento { get; set; }

        [DataMember(Name = "nroDocumento")]
        public string nroDocumento { get; set; }

        [DataMember(Name = "planServCodigo")]
        public string planServCodigo { get; set; }

        [DataMember(Name = "tipoServicio")]
        public string tipoServicio { get; set; }

        [DataMember(Name = "listaOpcional")]
        public List<BEListaOpcional> listaOpcional { get; set; }
    }
}