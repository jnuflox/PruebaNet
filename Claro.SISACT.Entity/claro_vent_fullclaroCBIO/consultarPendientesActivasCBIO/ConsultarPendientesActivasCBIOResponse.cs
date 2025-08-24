using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Claro.SISACT.Entity.GenericRequestResponse;

namespace Claro.SISACT.Entity.claro_vent_fullclaroCBIO
{
    [DataContract]
    [Serializable]
    public class ConsultarPendientesActivasCBIOResponse
    {
        [DataMember(Name = "idTransaccion")]
        public string idTransaccion { get; set; }

        [DataMember(Name = "mensajeError")]
        public string mensajeError { get; set; }

        [DataMember(Name = "codigoRespuesta")]
        public string codigoRespuesta { get; set; }

        [DataMember(Name = "mensajeRespuesta")]
        public string mensajeRespuesta { get; set; }

        [DataMember(Name = "responseData")]
        public ResponseDataConPendActivaCBIO responseData { get; set; }
    }
}
