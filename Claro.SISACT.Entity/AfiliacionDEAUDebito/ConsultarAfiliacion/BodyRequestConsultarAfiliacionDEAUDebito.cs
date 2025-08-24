using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.AfiliacionDEAUDebito.ConsultarAfiliacion
{

    [DataContract]
    [Serializable]
    public class BodyRequestConsultarAfiliacionDEAUDebito
    {
        [DataMember(Name = "tipo")]
        public string tipo { get; set; }

        [DataMember(Name = "tipoConsulta")]
        public string tipoConsulta { get; set; }
    }
}
