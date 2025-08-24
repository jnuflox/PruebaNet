using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity
{
    [DataContract]
    [Serializable]
    public class BESolicitudExcepPrecio
    {
        public BESolicitudExcepPrecio() { }

        [DataMember(Name = "solinCodigo")]
        public string solinCodigo { get; set; }

        [DataMember(Name = "pedidoTienda")]
        public string pedidoTienda { get; set; }

        [DataMember(Name = "pedidoSinergia")]
        public string pedidoSinergia { get; set; }

        [DataMember(Name = "precioTienda")]
        public string precioTienda { get; set; }

        [DataMember(Name = "precioSisact")]
        public string precioSisact { get; set; }

        [DataMember(Name = "codigoOficina")]
        public string codigoOficina { get; set; }

        [DataMember(Name = "usuarioRegistro")]
        public string usuarioRegistro { get; set; }

        [DataMember(Name = "nodoRegistro")]
        public string nodoRegistro { get; set; }

        [DataMember(Name = "estado")]
        public string estado { get; set; }

        [DataMember(Name = "estadoPosterior")]
        public string estadoPosterior { get; set; }

        [DataMember(Name = "idFlujo")]
        public string idFlujo { get; set; }

        [DataMember(Name = "cuotaInicialTienda")]
        public string cuotaInicialTienda { get; set; }

        [DataMember(Name = "cuotaInicialSisact")]
        public string cuotaInicialSisact { get; set; }
    }
}
