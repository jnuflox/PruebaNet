using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Claro.SISACT.Entity.RegistrarEvaluacionBRMSDP.Response;
using Claro.SISACT.Entity.claro_int_consultacliente.consultarDatos;

namespace Claro.SISACT.Entity.VentasCuotas.ObtenerVariablesBRMS.Response
{
    [DataContract]
    [Serializable]
    public class ObtenerVariablesBRMSTypeResponse
    {
        [DataMember(Name = "responseStatus")]
        public ResponseStatus responseStatus { get; set; }

        [DataMember(Name = "responseData")]
        public BEObtenerVariablesBRMS responseData { get; set; }

    }
}
