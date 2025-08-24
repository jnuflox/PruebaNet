using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.BeneficioFullClaro.validarAplicaDsctoCargoFijo.Response
{
    [DataContract]
    [Serializable]
    public class PO_CURSOR_Row
    {
        [DataMember(Name = "CCFCN_ID")]
        public string CCFCN_ID { get; set; }

        [DataMember(Name = "CCFCV_COD_CAMPANA")]
        public string CCFCV_COD_CAMPANA { get; set; }

        [DataMember(Name = "CCFCV_CAMPANA")]
        public string CCFCV_CAMPANA { get; set; }

        [DataMember(Name = "CCFCV_PLAN_PVU")]
        public string CCFCV_PLAN_PVU { get; set; }

        [DataMember(Name = "CCFCV_TMCODE_POID")]
        public string CCFCV_TMCODE_POID { get; set; }

        [DataMember(Name = "CCFCV_BONO")]
        public string CCFCV_BONO { get; set; }

        [DataMember(Name = "CCFCV_MESES")]
        public string CCFCV_MESES { get; set; }

        [DataMember(Name = "CCFCV_ESTADO")]
        public string CCFCV_ESTADO { get; set; }

        [DataMember(Name = "CCFCV_PLATAFORMA")]
        public string CCFCV_PLATAFORMA { get; set; }

        [DataMember(Name = "CCFCV_TIPO_SERVICIO")]
        public string CCFCV_TIPO_SERVICIO { get; set; }

        [DataMember(Name = "CCFCV_USU_REG")]
        public string CCFCV_USU_REG { get; set; }

        [DataMember(Name = "CCFCD_FEC_REG")]
        public string CCFCD_FEC_REG { get; set; }
    }
}
