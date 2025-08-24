using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.AfiliacionDEAUDebito.ConsultarAfiliacion
{

    [DataContract]
    [Serializable]
    public class BEListaNotificacionDebito
    {

        [DataMember(Name = "notificacionId")]
        public string notificacionId { get; set; }

        [DataMember(Name = "afiliacionId")]
        public string afiliacionId { get; set; }

        [DataMember(Name = "tipo")]
        public string tipo { get; set; }

        [DataMember(Name = "destino")]
        public string destino { get; set; }

        [DataMember(Name = "estado")]
        public string estado { get; set; }

        [DataMember(Name = "usuarioReg")]
        public string usuarioReg { get; set; }

        [DataMember(Name = "fechaReg")]
        public string fechaReg { get; set; }

        [DataMember(Name = "usuarioModif")]
        public string usuarioModif { get; set; }

        [DataMember(Name = "fechaModif")]
        public string fechaModif { get; set; }
    }
}
