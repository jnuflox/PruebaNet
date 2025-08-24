using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity
{
    //PROY-140419 Autorizar Portabilidad sin PIN
    [DataContract]
    [Serializable]
    public class BECabeceraValidaSuperv
    {
        [DataMember(Name = "nroSec")]
        public Int64 nroSec { get; set; }

        [DataMember(Name = "tipoDoc")]
        public string tipoDoc { get; set; }

        [DataMember(Name = "numDoc")]
        public string numDoc { get; set; }

        [DataMember(Name = "usuarioAut")]
        public string usuarioAut { get; set; }

        [DataMember(Name = "codPDV")]
        public string codPDV { get; set; }

        [DataMember(Name = "descPDV")]
        public string descPDV { get; set; }

        [DataMember(Name = "canalPDV")]
        public string canalPDV { get; set; }

        [DataMember(Name = "tipoVenta")]
        public string tipoVenta { get; set; }

        [DataMember(Name="nodo")]
        public string nodo { get; set; }

        [DataMember(Name="usuario")]
        public string usuario { get; set; }

        [DataMember(Name = "datosDetalleValidaSuper")]
        public List<BEDetalleValidaSuper> datosDetalleValidaSuper { get; set; }
    }
}
