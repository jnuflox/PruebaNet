using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.AfiliacionDEAUDebito.ConsultarAfiliacion
{

    [DataContract]
    [Serializable]
    public class BEListaConsultarAfiliacionDebito
    {
        [DataMember (Name = "idAfiliacion")]
        public string idAfiliacion { get; set; }

        [DataMember(Name = "idEntidad")]
        public string idEntidad { get; set; }

        [DataMember(Name = "idOrigenCuenta")]
        public string idOrigenCuenta { get; set; }

        [DataMember(Name = "codCuenta")]
        public string codCuenta { get; set; }

        [DataMember(Name = "idServPpal")]
        public string idServPpal { get; set; }

        [DataMember(Name = "montoMaximo")]
        public string montoMaximo { get; set; }

        [DataMember(Name = "tarjetaId")]
        public string tarjetaId { get; set; }

        [DataMember(Name = "idMoneda")]
        public string idMoneda { get; set; }

        [DataMember(Name = "origenAfiliacion")]
        public string origenAfiliacion { get; set; }

        [DataMember(Name = "numSolicitud")]
        public string numSolicitud { get; set; }

        [DataMember(Name = "flgMontoMaximo")]
        public string flgMontoMaximo { get; set; }

        [DataMember(Name = "monedaMontomaximo")]
        public string monedaMontomaximo { get; set; }

        [DataMember(Name = "canal")]
        public string canal { get; set; }

        [DataMember(Name = "estado")]
        public string estado { get; set; }

        [DataMember(Name = "comentario")]
        public string comentario { get; set; }

        [DataMember(Name = "usuarioReg")]
        public string usuarioReg { get; set; }

        [DataMember(Name = "fechaReg")]
        public string fechaReg { get; set; }

        [DataMember(Name = "usuarioModif")]
        public string usuarioModif { get; set; }

        [DataMember(Name = "fechaModif")]
        public string fechaModif { get; set; }

        [DataMember(Name = "numTarjeta")]
        public string numTarjeta { get; set; }

    }
}
