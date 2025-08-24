using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Claro.SISACT.Entity.GenericRequestResponse;

namespace Claro.SISACT.Entity.claro_int_consultacliente.consultarDatos
{
    [DataContract]
    [Serializable]
    public class BodyRequestConsultarCliente
    {
        [DataMember(Name = "tipoDocumento")]
        public string tipoDocumento { get; set; }

        [DataMember(Name = "numeroDocumento")]
        public string numeroDocumento { get; set; }

        [DataMember(Name = "listaOpcional")]
        public List<BEListaOpcional> listaOpcional { get; set; }
    }
}
