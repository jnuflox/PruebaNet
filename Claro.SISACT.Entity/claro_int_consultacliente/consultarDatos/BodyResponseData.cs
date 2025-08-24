using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Claro.SISACT.Entity.GenericRequestResponse;

namespace Claro.SISACT.Entity.claro_int_consultacliente.consultarDatos
{
    [DataContract]
    [Serializable]
    public class BodyResponseData
    {
        [DataMember(Name = "razonSocial")]
        public string razonSocial { get; set; }

        [DataMember(Name = "nombres")]
        public string nombres { get; set; }

        [DataMember(Name = "apellidos")]
        public string apellidos { get; set; }

        [DataMember(Name = "customer")]
        public List<BodyResponseCustomer> customer { get; set; }

        [DataMember(Name = "cantPlanes")]
        public string cantPlanes { get; set; }

        [DataMember(Name = "cantPlanesActivos")]
        public string cantPlanesActivos { get; set; }

        [DataMember(Name = "cantPlanesBloquedos")]
        public string cantPlanesBloquedos { get; set; }

        [DataMember(Name = "cantPlanesSuspendidos")]
        public string cantPlanesSuspendidos { get; set; }

        [DataMember(Name = "nro7")]
        public string nro7 { get; set; }

        [DataMember(Name = "nro30")]
        public string nro30 { get; set; }

        [DataMember(Name = "nro90")]
        public string nro90 { get; set; }

        [DataMember(Name = "nro90mas")]
        public string nro90mas { get; set; }

        [DataMember(Name = "nro180")]
        public string nro180 { get; set; }

        [DataMember(Name = "nro180mas")]
        public string nro180mas { get; set; }

        [DataMember(Name = "cargoFijo")]
        public string cargoFijo { get; set; }

        [DataMember(Name = "listaOpcional")]
        public List<BEListaOpcional> listaOpcional { get; set; }
    }
}
