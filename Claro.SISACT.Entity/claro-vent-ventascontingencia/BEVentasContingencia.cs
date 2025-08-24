using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claro_vent_ventascontingencia
{
    [DataContract]
    [Serializable]
    public class BEVentasContingencia
    {
        [DataMember(Name = "fechaPago")]
        public string fechaPago { get; set; }

        [DataMember(Name = "tipoVenta")]
        public string tipoVenta { get; set; }

        [DataMember(Name = "numPedido")]
        public string numPedido { get; set; }

        [DataMember(Name = "numContrato")]
        public string numContrato { get; set; }

        [DataMember(Name = "numSec")]
        public string numSec { get; set; }

        [DataMember(Name = "tipoDocumento")]
        public string tipoDocumento { get; set; }

        [DataMember(Name = "descDocumento")]
        public string descDocumento { get; set; }

        [DataMember(Name = "numDocumento")]
        public string numDocumento { get; set; }

        [DataMember(Name = "tipoDocumentoBio")]
        public string tipoDocumentoBio { get; set; }

        [DataMember(Name = "descDocumentoBio")]
        public string descDocumentoBio { get; set; }

        [DataMember(Name = "numDocumentoBio")]
        public string numDocumentoBio { get; set; }

        [DataMember(Name = "nombreCliente")]
        public string nombreCliente { get; set; }

        [DataMember(Name = "codCanal")]
        public string codCanal { get; set; }

        [DataMember(Name = "codOficina")]
        public string codOficina { get; set; }

        [DataMember(Name = "codVendedor")]
        public string codVendedor { get; set; }

        [DataMember(Name = "codOperacion")]
        public string codOperacion { get; set; }

        [DataMember(Name = "descOperacion")]
        public string descOperacion { get; set; }

        [DataMember(Name = "tipoOperacion")]
        public string tipoOperacion { get; set; }

        [DataMember(Name = "tipoContingencia")]
        public string tipoContingencia { get; set; }

        [DataMember(Name = "estadoActivacion")]
        public string estadoActivacion { get; set; }

        [DataMember(Name = "codBiometria")]
        public string codBiometria { get; set; }

        [DataMember(Name = "descBiometria")]
        public string descBiometria { get; set; }

        [DataMember(Name = "codNoBiometria")]
        public string codNoBiometria { get; set; }

        [DataMember(Name = "descNoBiometria")]
        public string descNoBiometria { get; set; }

        [DataMember(Name = "linea")]
        public string linea { get; set; }

        [DataMember(Name = "codProducto")]
        public string codProducto { get; set; }

        [DataMember(Name = "estadoPago")]
        public string estadoPago { get; set; }

        [DataMember(Name = "cadena1")]
        public string cadena1 { get; set; }

        [DataMember(Name = "cadena2")]
        public string cadena2 { get; set; }

        [DataMember(Name = "cadena3")]
        public string cadena3 { get; set; }

        [DataMember(Name = "cadena4")]
        public string cadena4 { get; set; }

        [DataMember(Name = "cadena5")]
        public string cadena5 { get; set; }

        [DataMember(Name = "cadena6")]
        public string cadena6 { get; set; }

        [DataMember(Name = "reserva")]
        public string reserva { get; set; }

        [DataMember(Name = "usuario")]
        public string usuario { get; set; }
    }
}
