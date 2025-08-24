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
   public class ValidarCantidadCampaniaRequest
   {
        [DataMember(Name = "auditRequest")]
        public BEAuditRequest auditRequest { get; set; }
        [DataMember(Name = "tipoDocumento")]
        public string codTipoDocumento { get; set; }
        [DataMember(Name = "numeroDocumento")]
        public string numDocumento { get; set; }
       //cambio
        [DataMember(Name = "casoEspecial")]
        public string codCasoEspecial { get; set; }
        [DataMember(Name = "tipoOperacion")]
        public string codTipoOperacion { get; set; }
        [DataMember(Name = "codAplicativo")]
        public string codAplicativo { get; set; }
        [DataMember(Name = "codTipoProducto")]
        public string codTipoProducto { get; set; }
        [DataMember(Name = "descTipoProducto")]
        public string descTipoProducto { get; set; }
   }
}
