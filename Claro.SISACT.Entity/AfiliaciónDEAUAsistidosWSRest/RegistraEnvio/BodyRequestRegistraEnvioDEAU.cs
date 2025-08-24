using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.AfiliaciónDEAUAsistidosWSRest.RegistraEnvio
{
    [DataContract]
    [Serializable]
    public class BodyRequestRegistraEnvioDEAU
    {
        [DataMember(Name = "nroSec")]
        public string nroSec { get; set; }

        [DataMember(Name = "tipoDocumento")]
        public string tipoDocumento { get; set; }

        [DataMember(Name = "descDocumento")]
        public string descDocumento { get; set; }

        [DataMember(Name = "numDocumento")]
        public string numDocumento { get; set; }

        [DataMember(Name = "nombresCliente")]
        public string nombresCliente { get; set; }

        [DataMember(Name = "apellidosCliente")]
        public string apellidosCliente { get; set; }

        [DataMember(Name = "correoCliente")]
        public string correoCliente { get; set; }

        [DataMember(Name = "tipoCliente")]
        public string tipoCliente { get; set; }

        [DataMember(Name = "codTipoOperacion")]
        public string codTipoOperacion { get; set; }

        [DataMember(Name = "tipoOperacion")]
        public string tipoOperacion { get; set; }

        [DataMember(Name = "tipoProducto")]
        public string tipoProducto { get; set; }

        [DataMember(Name = "canalVenta")]
        public string canalVenta { get; set; }

        [DataMember(Name = "codPdv")]
        public string codPdv { get; set; }

        [DataMember(Name = "descPdv")]
        public string descPdv { get; set; }

        [DataMember(Name = "origenSolicitud")]
        public string origenSolicitud { get; set; }

        [DataMember(Name = "idEntidad")]
        public string idEntidad { get; set; }

        [DataMember(Name = "descEntidad")]
        public string descEntidad { get; set; }

        [DataMember(Name = "idOrigenCuenta")]
        public string idOrigenCuenta { get; set; }

        [DataMember(Name = "idMoneda")]
        public string idMoneda { get; set; }

        [DataMember(Name = "customerId")]
        public string customerId { get; set; }

        [DataMember(Name = "idServPpal")]
        public string idServPpal { get; set; }

        [DataMember(Name = "origenAfiliacion")]
        public string origenAfiliacion { get; set; }

        [DataMember(Name = "nroSolicitud")]
        public string nroSolicitud { get; set; }

        [DataMember(Name = "tarjetaId")]
        public string tarjetaId { get; set; }

        [DataMember(Name = "numTarjeta")]
        public string numTarjeta { get; set; }

        [DataMember(Name = "flagMontoMaximo")]
        public string flagMontoMaximo { get; set; }

        [DataMember(Name = "monedaMontoMaximo")]
        public string monedaMontoMaximo { get; set; }

        [DataMember(Name = "montoMaximo")]
        public string montoMaximo { get; set; }

        [DataMember(Name = "canalMp")]
        public string canalMp { get; set; }

        [DataMember(Name = "estadoMp")]
        public string estadoMp { get; set; }

        [DataMember(Name = "comentario")]
        public string comentario { get; set; }

        [DataMember(Name = "correoClienteEnvioLink")]
        public string correoClienteEnvioLink { get; set; }

        [DataMember(Name = "telefonoClienteEnvioLink")]
        public string telefonoClienteEnvioLink { get; set; }

        [DataMember(Name = "tipiServicio")]
        public string tipiServicio { get; set; }

        [DataMember(Name = "tipiValor")]
        public string tipiValor { get; set; }

        [DataMember(Name="estadoEnvioLink")]
        public string estadoEnvioLink { get; set; } 
       
        [DataMember(Name = "estadoVenta")]
        public string estadoVenta { get; set; }

        [DataMember(Name = "estadoAfiliacion")]
        public string estadoAfiliacion { get; set; }

        [DataMember(Name = "telefonoClienteNotif")]
        public string telefonoClienteNotif { get; set; }

        [DataMember(Name = "correoClienteNotif")]
        public string correoClienteNotif { get; set; }

        [DataMember(Name = "descCampanias ")]
        public string descCampanias { get; set; }

        [DataMember(Name = "fechaVencTarjeta")]
        public string fechaVencTarjeta { get; set; }
    }
}
