using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ValidarProductosFCRest
{
    //PROY-FULLCLARO.V2
    [DataContract]
    [Serializable]
    public class ProductosFCRestRequest
    {
        [DataMember(Name = "strTipoProducto")]
        public string strTipoProducto { get; set; }
        [DataMember(Name = "strPlanesServicios")]
        public string strPlanes { get; set; }
        [DataMember(Name = "strTipoDocumento")]
        public string strTipoDocumento { get; set; }
        [DataMember(Name = "strNroDocumento")]
        public string strNroDocumento { get; set; }
    }
}
