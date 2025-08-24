using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.DatosLineaRest
{
    [DataContract]
    [Serializable]
    public class ListaOpcional
    {
        [DataMember(Name = "clave")]
        public string clave { get; set; }
        [DataMember(Name = "valor")]
        public string valor { get; set; }
    }
}