using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.BeneficioFullClaro.registrarClienteFCDsctoCargoFijo.Request
{
    [DataContract]
    [Serializable]
    public class BodyRequestRegistrarClienteFCDsctCF
    {
        [DataMember(Name = "PI_CLBCV_TIPO_DOC")]
        public string PI_CLBCV_TIPO_DOC { get; set; }

        [DataMember(Name = "PI_CLBCV_NRO_DOC")]
        public string PI_CLBCV_NRO_DOC { get; set; }

        [DataMember(Name = "PI_CLBCV_LINEA")]
        public string PI_CLBCV_LINEA { get; set; }

        [DataMember(Name = "PI_CLBCV_COID")]
        public string PI_CLBCV_COID { get; set; }

        [DataMember(Name = "PI_CLBCV_CUSTOMERID")]
        public string PI_CLBCV_CUSTOMERID { get; set; }

        [DataMember(Name = "PI_CLBCV_ESTADO")]
        public string PI_CLBCV_ESTADO { get; set; }

        [DataMember(Name = "PI_CLBCV_PLATAFORMA")]
        public string PI_CLBCV_PLATAFORMA { get; set; }

        [DataMember(Name = "PI_CLBCV_TIPO_SERVICIO")]
        public string PI_CLBCV_TIPO_SERVICIO { get; set; }

        [DataMember(Name = "PI_CLBCV_COD_CAMPANA")]
        public string PI_CLBCV_COD_CAMPANA { get; set; }

        [DataMember(Name = "PI_CLBCV_CAMPANA")]
        public string PI_CLBCV_CAMPANA { get; set; }

        [DataMember(Name = "PI_CLBCN_NUM_SEC")]
        public string PI_CLBCN_NUM_SEC { get; set; }

        [DataMember(Name = "PI_CLBCV_NUM_CONTRATO")]
        public string PI_CLBCV_NUM_CONTRATO { get; set; }

        [DataMember(Name = "PI_CLBCV_COD_PRODUCTO")]
        public string PI_CLBCV_COD_PRODUCTO { get; set; }

        [DataMember(Name = "PI_CLBCV_PLAN")]
        public string PI_CLBCV_PLAN { get; set; }

        [DataMember(Name = "PI_CLBCV_PLAN_PVU")]
        public string PI_CLBCV_PLAN_PVU { get; set; }

        [DataMember(Name = "PI_CLBCV_BONO")]
        public string PI_CLBCV_BONO { get; set; }

        [DataMember(Name = "PI_CLBCV_MESES")]
        public string PI_CLBCV_MESES { get; set; }

        [DataMember(Name = "PI_CLBCV_FULL_CLARO")]
        public string PI_CLBCV_FULL_CLARO { get; set; }

        [DataMember(Name = "PI_CLBCV_USU_REG")]
        public string PI_CLBCV_USU_REG { get; set; }

        [DataMember(Name = "listaRequestOpcional")]
        public listaRequestOpcional listaRequestOpcional { get; set; }

        public BodyRequestRegistrarClienteFCDsctCF()
        {
            listaRequestOpcional = new listaRequestOpcional();
        }
    }
}
