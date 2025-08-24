using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.DatosLineaRest
{
    [DataContract]
    [Serializable]
    public class BodyResponseDatosLinea
    {
        [DataMember(Name = ("datosLineaResponse"))]
        public ResponseDatosLinea datosLineaResponse { get; set; }

        public BodyResponseDatosLinea()
        {
            datosLineaResponse = new ResponseDatosLinea();
        }
    }
}
