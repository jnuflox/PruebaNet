using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ConsultarCantidadCampaniaRest
{
   
    //PROY-140245
    [DataContract]
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEAuditResponse
    {
        [DataMember(Name = "idTransaccion")]
        public string idTransaccion { get; set; }
        [DataMember(Name = "codigoRespuesta")]
        public string codigoRespuesta { get; set; }
        [DataMember(Name = "mensajeRespuesta")]
        public string mensajeRespuesta { get; set; }
    }
}
