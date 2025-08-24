using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Claro.SISACT.Entity.GenericRequestResponse;

namespace Claro.SISACT.Entity.claro_vent_fullclaroCBIO.consultarActivosCBIO
{
    [DataContract]
    [Serializable]
    public class ConsultarActivosCBIOResponse
    {
        [DataMember(Name = "idTransaccion")]
        public string idTransaccion { get; set; }

        [DataMember(Name = "codigoRespuesta")]
        public string codigoRespuesta { get; set; }

        [DataMember(Name = "mensajeRespuesta")]
        public string mensajeRespuesta { get; set; }

        [DataMember(Name = "responseData")]
        public ResponseDataConPendActivaCBIO responseData { get; set; }

        public ConsultarActivosCBIOResponse()
        {
            responseData = new ResponseDataConPendActivaCBIO();
        }
    }
}
