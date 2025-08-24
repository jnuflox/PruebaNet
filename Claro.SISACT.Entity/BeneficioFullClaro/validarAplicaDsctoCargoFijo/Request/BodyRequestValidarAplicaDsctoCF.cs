using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.BeneficioFullClaro.validarAplicaDsctoCargoFijo.Request
{
    [DataContract]
    [Serializable]
    public class BodyRequestValidarAplicaDsctoCF
    {
        [DataMember(Name = "PI_ESTADO")]
        public string PI_ESTADO { get; set; }

        [DataMember(Name = "PI_COD_CAMPANA")]
        public string PI_COD_CAMPANA { get; set; }

        [DataMember(Name = "PI_PLAN_PVU")]
        public string PI_PLAN_PVU { get; set; }

        [DataMember(Name = "PI_TMCODE_POID")]
        public string PI_TMCODE_POID { get; set; }

        [DataMember(Name = "listaRequestOpcional")]
        public listaRequestOpcional listaRequestOpcional { get; set; }

        public BodyRequestValidarAplicaDsctoCF()
        {
            listaRequestOpcional = new listaRequestOpcional();
        }
    }
}
