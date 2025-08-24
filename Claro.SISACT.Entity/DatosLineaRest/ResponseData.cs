using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.DatosLineaRest
{
    [DataContract]
    [Serializable]
    public class ResponseData
    {
        [DataMember(Name = ("cliente"))]
        public ResponseDataCliente cliente { get; set; }

        [DataMember(Name = ("ticklers"))]
        public List<ResponseDataTicklers> ticklers { get; set; }

        [DataMember(Name = ("productos"))]
        public List<ResponseDataProductos> productos { get; set; }
    }
}
