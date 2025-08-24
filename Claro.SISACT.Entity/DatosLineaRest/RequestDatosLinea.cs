using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.DatosLineaRest
{
    [DataContract]
    [Serializable]
    public class RequestDatosLinea
    {
        [DataMember(Name = "msisdn")]
        public string msisdn { get; set; }
        [DataMember(Name = "listaOpcional")]
        public List<ListaOpcional> listaOpcional { get; set; }
    }
}
