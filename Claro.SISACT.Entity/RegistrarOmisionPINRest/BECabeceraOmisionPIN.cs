using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.RegistrarOmisionPINRest
{
    [DataContract]
    [Serializable] 
    public class BECabeceraOmisionPIN
    {
        [DataMember(Name = "nroSec")]
        public string nroSec { get; set; }

        [DataMember(Name = "tipoDoc")]
        public string tipoDoc { get; set; }

        [DataMember(Name = "numDoc")]
        public string numDoc { get; set; }

        [DataMember(Name = "usuAutorizador")]
        public string usuAutorizador { get; set; }

        [DataMember(Name = "codPDV")]
        public string codPDV { get; set; }

        [DataMember(Name = "descPDV")]
        public string descPDV { get; set; }

        [DataMember(Name = "canalPDV")]
        public string canalPDV { get; set; }

        [DataMember(Name = "tipoVenta")]
        public string tipoVenta { get; set; }

        [DataMember(Name = "nodo")]
        public string nodo { get; set; }

        [DataMember(Name = "usuRegistro")]
        public string usuRegistro { get; set; }

        [DataMember(Name = "idCausal")]
        public string idCausal { get; set; }

        [DataMember(Name = "desCausal")]
        public string desCausal { get; set; }

        [DataMember(Name = "detalle")]
        public List<BEDetalleOmisionPIN> detalleOmision { get; set; }

    }
}
