using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claroventacobertura.validarcobertura
{
    [DataContract]
    [Serializable]
    public class ValidarCoberturaRequest
    {
        [DataMember(Name = "idTransaccion")]
        public string idTransaccion { get; set; }

        [DataMember(Name = "codAplicacion")]
        public string codAplicacion { get; set; }

        [DataMember(Name = "latitud")]
        public string latitud { get; set; }

        [DataMember(Name = "longitud")]
        public string longitud { get; set; }

        [DataMember(Name = "tipoTecnologia")]
        public string tipoTecnologia { get; set; }

        [DataMember(Name = "motivo")]
        public string motivo { get; set; }

        [DataMember(Name = "cliente")]
        public ClienteCobertura cliente { get; set; }

        [DataMember(Name = "direccion")]
        public DireccionCobertura direccion { get; set; }

        [DataMember(Name = "solicitud")]
        public SolicitudCobertura solicitud { get; set; }

        public ValidarCoberturaRequest()
        {
            idTransaccion = string.Empty;
            codAplicacion = string.Empty;
            latitud = string.Empty;
            longitud = string.Empty;
            tipoTecnologia = string.Empty;
            motivo = string.Empty;
            cliente = new ClienteCobertura();
            direccion = new DireccionCobertura();
            solicitud = new SolicitudCobertura();
        }
    }
}
