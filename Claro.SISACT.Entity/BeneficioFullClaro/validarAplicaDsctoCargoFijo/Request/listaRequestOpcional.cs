using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.BeneficioFullClaro.validarAplicaDsctoCargoFijo.Request
{
    [DataContract]
    [Serializable]
    public class listaRequestOpcional
    {
        [DataMember(Name = "responseOpcional")]
        public List<responseOpcional> responseOpcional { get; set; }

        public listaRequestOpcional()
        {
            responseOpcional = new List<responseOpcional>();
        }
    }
}
