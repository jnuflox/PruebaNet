using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claroventacobertura.validarcobertura
{
    [DataContract]
    [Serializable]
    public class ResponseStatus
    {
        [DataMember(Name = "status")]
        public string status { get; set; }

        [DataMember(Name = "codigoRespuesta")]
        public string codigoRespuesta { get; set; }

        [DataMember(Name = "descripcionRespuesta")]
        public string descripcionRespuesta { get; set; }

        [DataMember(Name = "ubicacionError")]
        public string ubicacionError { get; set; }

        [DataMember(Name = "fecha")]
        public string fecha { get; set; }

        [DataMember(Name = "origen")]
        public string origen { get; set; }

        [DataMember(Name = "detalleError")]
        public detalleError detalleError { get; set; }

        public ResponseStatus()
        {
            status = string.Empty;
            codigoRespuesta = string.Empty;
            descripcionRespuesta = string.Empty;
            ubicacionError = string.Empty;
            fecha = string.Empty;
            origen = string.Empty;
            detalleError = new detalleError();
        }
    }
}
