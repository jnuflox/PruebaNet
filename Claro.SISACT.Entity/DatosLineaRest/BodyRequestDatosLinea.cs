using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.DatosLineaRest
{
    [DataContract]
    [Serializable]
    public class BodyRequestDatosLinea
    {
        [DataMember(Name = ("datosLineaRequest"))]
        public RequestDatosLinea datosLineaRequest { get; set; }

        public BodyRequestDatosLinea()
        {
            datosLineaRequest = new RequestDatosLinea();
        }
    }
}
