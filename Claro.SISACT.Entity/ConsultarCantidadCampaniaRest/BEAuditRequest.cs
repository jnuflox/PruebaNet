using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//PROY-140245 
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ConsultarCantidadCampaniaRest
{
    //PROY-140245
    [DataContract]
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEAuditRequest
    {
        [DataMember(Name = "idTransaccion")]
        public string idTransaccion { get; set; }
        [DataMember(Name = "ipAplicacion")]
        public string ipAplicacion { get; set; }
        [DataMember(Name = "nombreAplicacion")]
        public string nombreAplicacion { get; set; }
        [DataMember(Name = "usuarioAplicacion")]
        public string usuarioAplicacion { get; set; }
    }
}
