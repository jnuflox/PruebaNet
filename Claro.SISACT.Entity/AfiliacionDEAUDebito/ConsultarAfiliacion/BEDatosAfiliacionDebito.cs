using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.AfiliacionDEAUDebito.ConsultarAfiliacion
{

    [DataContract]
    [Serializable]
    public class BEDatosAfiliacionDebito
    {

        [DataMember(Name = "nombresCliente")]
        public string nombresCliente { get; set; }

        [DataMember(Name = "numeroDocumento")]
        public string numeroDocumento { get; set; }
    }
}
