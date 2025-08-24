using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Claro.SISACT.Entity.RegistrarEvaluacionBRMSDP.Response;
using Claro.SISACT.Entity.claro_int_consultacliente.consultarDatos;
using Claro.SISACT.Entity.GenericRequestResponse;

namespace Claro.SISACT.Entity.VentasCuotas.ValidarMaterialAccCuota.Response
{
    [DataContract]
    [Serializable]
    public class ValidarMaterialAccCuotaTypeResponse
    {
        [DataMember(Name = "responseStatus")]
        public ResponseStatus responseStatus { get; set; }

        [DataMember(Name = "responseData")]
        public List<BEValidarMaterialAccCuota> responseData { get; set; }

    }
}
