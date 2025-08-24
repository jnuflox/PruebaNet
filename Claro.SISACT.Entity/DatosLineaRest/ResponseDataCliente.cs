using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.DatosLineaRest
{
    [DataContract]
    [Serializable]
    public class ResponseDataCliente
    {
        [DataMember(Name = "nombre")]
        public string nombre { get; set; }

        [DataMember(Name = "apellidos")]
        public string apellidos { get; set; }

        [DataMember(Name = "razonSocial")]
        public string razonSocial { get; set; }

        [DataMember(Name = "tipDoc")]
        public string tipDoc { get; set; }

        [DataMember(Name = "numDoc")]
        public string numDoc { get; set; }

        [DataMember(Name = "rucDni")]
        public string rucDni { get; set; }

        [DataMember(Name = "coId")]
        public string coId { get; set; }

        [DataMember(Name = "coIdPub")]
        public string coIdPub { get; set; }

        [DataMember(Name = "csId")]
        public string csId { get; set; }

        [DataMember(Name = "csIdPub")]
        public string csIdPub { get; set; }

        [DataMember(Name = "tipoCliente")]
        public string tipoCliente { get; set; }

        [DataMember(Name = "codigotipoCliente")]
        public string codigotipoCliente { get; set; }

        [DataMember(Name = "codigoplanTarifario")]
        public string codigoplanTarifario { get; set; }

        [DataMember(Name = "desplanTarifario")]
        public string desplanTarifario { get; set; }

        [DataMember(Name = "estado")]
        public string estado { get; set; }

        [DataMember(Name = "descEstado")]
        public string descEstado { get; set; }

        [DataMember(Name = "iccid")]
        public string iccid { get; set; }

        [DataMember(Name = "imsi")]
        public string imsi { get; set; }
    }
}
