using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEDataCreditoINOUT
    {
        public int IODCN_CODIGO { get; set; }
        public string IODCV_NUM_OPERACION { get; set; }
        public string IODCV_INPUT_VALORES { get; set; }
        public string IODCV_OUTPUT_VALORES { get; set; }
        public DateTime IODCD_FECHA_REGISTRO { get; set; }
        public string IODCV_TIPO_DOCUMENTO { get; set; }
        public string IODCV_NUM_DOCUMENTO { get; set; }
        public string IODCV_USUARIO_REGISTRO { get; set; }
        public string IODCV_COD_PUNTO_VENTA { get; set; }
        public string IODCC_FORMA_PAGO { get; set; }
        public string IODCC_TIPO_ACTIVACION { get; set; }
        public string IODCC_TIPO_CLIENTE { get; set; }
        public string IODCC_TIPO_VENTA { get; set; }
        public string IODCC_PLAZO_ACUERDO { get; set; }
        public string IODCC_PLAN1 { get; set; }
        public string IODCC_PLAN2 { get; set; }
        public string IODCC_PLAN3 { get; set; }
        public double IODCI_NUM_CF { get; set; }
        public string IODCC_CONTROL_CONSUMO { get; set; }
        public string IODCC_FLAG_ESSALUD { get; set; }
        public string IODCC_FLAG_SUNAT { get; set; }
        public string IODCC_RIESGO { get; set; }
        public string IODCC_LIMITE_CREDITO { get; set; }
        public string IODCC_SCORE_TEXTO { get; set; }
        public string IODCC_SCORE_NUMERO { get; set; }
        public string IODCC_RESPUESTA_DC { get; set; }
        public string IODCC_TIPO_GARANTIA { get; set; }
        public double IODCN_TOTAL_IMPORTE { get; set; }
        public string IODCV_APE_PATERNO { get; set; }
        public string IODCV_APE_MATERNO { get; set; }
        public string IODCV_NOMBRES { get; set; }
        public int IODCN_SOLIN_CODIGO { get; set; }
        public string IODCV_TIPO_SEC { get; set; }
        public string IODCV_UBIGEO { get; set; }
        public string IODCC_TIPO_CLIENTE_DC { get; set; }
        public string IODCC_ESTADO_CIVIL_DC { get; set; }
        public string IODCC_ORIGEN_LC_DC { get; set; }
        public string IODCC_ANALISIS_DC { get; set; }
        public string IODCC_SCORE_RANKING_OPER_DC { get; set; }
        public int IODCN_PUNTAJE_DC { get; set; }
        public double IODCN_LC_DATA_CREDITO_DC { get; set; }
        public double IODCN_LC_BASE_EXTERNA_DC { get; set; }
        public double IODCN_LC_CLARO_DC { get; set; }
        public string IODCC_RAZONES_DC { get; set; }
        public string IODCD_FECHA_NACE_CLIENTE_DC { get; set; }

        //INICIO: PROY-20054-IDEA-23849
        public int IODCC_CODIGOBURO { get; set; }
        //FIN 
    }
}
