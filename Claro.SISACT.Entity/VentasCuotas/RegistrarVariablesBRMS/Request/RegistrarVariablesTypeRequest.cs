using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Claro.SISACT.Entity.GenericRequestResponse;

namespace Claro.SISACT.Entity.VentasCuotas.RegistrarVariablesBRMS.Request
{
    [DataContract]
    [Serializable]
    public class RegistrarVariablesTypeRequest
    {

        [DataMember(Name = "codHistorico")]
        public string codHistorico { get; set; }

        [DataMember(Name = "nroSec")]
        public string nroSec { get; set; }

        [DataMember(Name = "montoPendCuotasAcc")]
        public string montoPendCuotasAcc { get; set; }

        [DataMember(Name = "cantLineasCuotasPendAcc")]
        public string cantLineasCuotasPendAcc { get; set; }

        [DataMember(Name = "cantMaxCuotasPendAcc")]
        public string cantMaxCuotasPendAcc { get; set; }

        [DataMember(Name = "montoPendCuotasAccUV")]
        public string montoPendCuotasAccUV { get; set; }

        [DataMember(Name = "cantLineasCuotaPendAccUV")]
        public string cantLineasCuotaPendAccUV { get; set; }

        [DataMember(Name = "cantMaxCuotasPendAccUV")]
        public string cantMaxCuotasPendAccUV { get; set; }

        [DataMember(Name = "promocionesAccCuotas")]
        public string promocionesAccCuotas { get; set; } 

        [DataMember(Name = "codTipoDocCliente")]
        public string codTipoDocCliente { get; set; }

        [DataMember(Name = "numeroDocCliente")]
        public string numeroDocCliente { get; set; }

        [DataMember(Name = "correlativo")]
        public string correlativo { get; set; }

        [DataMember(Name = "listaOpcional")]
        public List<BEListaOpcional> listaOpcional { get; set; }

        [DataMember(Name = "productoCuentaAFacturar")]
        public string productoCuentaAFacturar { get; set; }

    }
}
