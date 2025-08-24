using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claro_ope_comportamientoCliente.consultarComportamientoPago
{
    [DataContract]
    [Serializable]
    public class ConsultarRequestComportamientoCliente
    {
        [DataMember(Name = "tipoDocumento")]
        public int tipoDocumento { get; set; }
        [DataMember(Name = "numeroDocumento")]
        public string numeroDocumento { get; set; }
        [DataMember(Name = "listaOpcional")]
        public List<ListaOpcional> listaOpcional { get; set; }
    }
}
