using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BESolicitudPersona : BECliente
    {
        public BESolicitudPersona() { }

        public Int64 SOLIN_CODIGO { get; set; }
        public string OVENC_CODIGO { get; set; }
        public string CANAC_CODIGO { get; set; }
        public string SOLIN_USU_VEN { get; set; }
        public string SOLIC_EXI_BSC_FIN { get; set; }
        public string ANALC_CODIGO { get; set; }
        public string TDOCC_CODIGO { get; set; }
        public string CLIEC_NUM_DOC { get; set; }
        public string CLIEV_RAZ_SOC { get; set; }
        public string CLIEN_PROM_VEN { get; set; }
        public string TPROC_CODIGO { get; set; }
        public int SEGMN_CODIGO { get; set; }
        public string TCLIC_CODIGO { get; set; }
        public string TVENC_CODIGO { get; set; }
        public string TACTC_CODIGO { get; set; }
        public string TOPEN_CODIGO { get; set; }
        public string SOLIC_PLA_MAX_1 { get; set; }
        public string SOLIC_PLA_MAX_2 { get; set; }
        public string SOLIC_PLA_MAX_3 { get; set; }
        public string PACUC_CODIGO { get; set; }
        public string FPAGC_CODIGO { get; set; }
        public int SOLIN_CAN_LIN { get; set; }
        public string RFINC_CODIGO { get; set; }
        public string MRECC_CODIGO { get; set; }
        public string SOLIC_TIP_CAR_MAN { get; set; }
        public double SOLIN_IMP_DG_MAN { get; set; }
        public string ESTAC_CODIGO { get; set; }
        public string TEVAC_CODIGO { get; set; }
        public string SOLIC_FLA_TER { get; set; }
        public string SOLIV_DES_EST { get; set; }
        public string SOLIV_DES_OFI_VEN { get; set; }
        public string SOLIV_DES_RES_FIN { get; set; }
        public string SOLIV_DES_TIP_ACT { get; set; }
        public string SOLIV_COM_PUN_VEN { get; set; }
        public string SOLIV_COM_EVALUADOR { get; set; }
        public string SOLIC_USU_CRE { get; set; }
        public string CLIEV_NOM { get; set; }
        public string CLIEV_APE_PAT { get; set; }
        public string CLIEV_APE_MAT { get; set; }
        public string SOLIC_EVA_ESS { get; set; }
        public string SOLIC_EVA_SUN { get; set; }
        public string SOLIC_COD_RES_DIR { get; set; }
        public string SOLIV_DES_RES_DIR { get; set; }
        public string SOLIV_CAR_CLI { get; set; }
        public string SOLIC_TIP_CAR_FIJ { get; set; }
        public double SOLIN_IMP_DG { get; set; }
        public string SOLIV_RES_EXP_CON { get; set; }
        public string SOLIV_NUM_OPE_CON { get; set; }
        public double SOLIN_LIM_CRE_CON { get; set; }
        public double SOLIN_SUM_CAR_CON { get; set; }
        public double SOLIN_NUM_CAR_FIJ { get; set; }
        public string TCESC_CODIGO { get; set; }
        public string SOLIC_SCO_TXT_CON { get; set; }
        public string SOLIN_SCO_NUM_CON { get; set; }
        public Int64 SOLIN_CODIGO_PADRE { get; set; }
        public string FLAG_INFOCORP { get; set; }
        public string HINFV_MENSAJE { get; set; }
        public string RUCEMPLEADOR { get; set; }
        public string NOMBREEMPRESA { get; set; }
        public string CODCAMPANNA { get; set; }
        public string SOLIC_EXI_BSC_CON { get; set; }
        public string VENDEDOR_ID { get; set; }
        public string FLAG_CONSUMO { get; set; }
        public string SOLIV_FLAG_CORR { get; set; }
        public string SOLIV_CORREO { get; set; }
        public string CLIEV_EST_CIV { get; set; }
        public string SOLIV_UBIGEO_INEI { get; set; }
        public string SOLIC_ORIGEN_LC_DC { get; set; }
        public string SOLIC_ANALISIS_DC { get; set; }
        public string SOLIC_SCORE_RANKING_OPER_DC { get; set; }
        public double SOLIN_PUNTAJE_DC { get; set; }
        public double SOLIN_LC_DATA_CREDITO_DC { get; set; }
        public double SOLIN_LC_BASE_EXTERNA_DC { get; set; }
        public double SOLIN_LC_CLARO_DC { get; set; }
        public string SOLIC_REGLAS_DURAS_DC { get; set; }
        public string SOLIC_ALERT_COMPORT_DC { get; set; }
        public string SOLIC_ALERTAS_DC { get; set; }
        public string SOLIC_CORRESP_SALDO_DC { get; set; }
        public DateTime CLIED_FEC_NAC { get; set; }
        public DateTime CLIED_FEC_NAC_PDV { get; set; }
        public double LC_DISPONIBLE { get; set; }
        public double CF_MENORES { get; set; }
        public double CF_MAYORES { get; set; }
        public double DEUDA { get; set; }
        public string BLOQUEO { get; set; }
        public Int64 CLIEN_SEC_ASOCIADA { get; set; }
        public string RESPUESTA_DC { get; set; }
        public string PRDC_CODIGO { get; set; }
        public Int64 SOLIN_GRUPO_SEC { get; set; }
        public string CLIEV_CALIFICACION_PAGO { get; set; }
        public int BURO_CREDITICIO { get; set; }
        public double SOLIN_IMP_DG_GRUPO_SEC { get; set; }
        public double SOLIN_CF_GRUPO_SEC { get; set; }
        public string CLIEV_RIESGO_CLARO { get; set; }
        public string CLIEV_COMPORTA_PAGO { get; set; }
        public string CLIEC_FLAG_EXONERAR_RA { get; set; }

        public string FLAG_PORTABILIDAD { get; set; }
        public int PORT_OPER_CED { get; set; }
        public string PORT_ESTADO { get; set; }
        public string PORT_TELEF_CONT { get; set; }
        public string PORT_FLAG_REC_OPE_CED { get; set; }
        public string PORT_CARGO_FIJO_OPE_CED { get; set; }
        public string PORT_NRO_FOLIO { get; set; }
        public string SOLIV_TEL_SMS { get; set; }
        public string TLINC_CODIGO_CEDENTE { get; set; }

        public string FLAG_VTA_PROACTIVA { get; set; }
        public string CAMPV_CODIGO { get; set; }
        public string CLIEC_VEN_DNI { get; set; }
        public double SOLIN_KIT_COS_INST { get; set; }
        public string CLIEC_BLOQUEO { get; set; }
        public string CLIEV_NRO_CARTA_PRESELEC { get; set; }
        public string CLIEV_OPERADOR { get; set; }
        public string CLIEV_PAGINA_CLARO { get; set; }
        public double SOLIN_CF_ALQUILER_KIT { get; set; }

        public ArrayList oPlanDetalle { get; set; }
        public int idFila { get; set; }
        public string NRO_DOCUMENTO { get; set; }
        //gaa20151210
        public string FLAG_VALIDARSECPENDIENTE { get; set; }
        //fin gaa20151210
        public string CLIEC_CODNACION  { get; set; } //PROY-31636
        public string CLIEV_DESCNACION { get; set; } //PROY-31636
        public string DEUDA_CLIENTE { get; set; }//PROY-29121

        //PROY-29215 INICIO
        public string FORMA_PAGO { get; set; }
        public int NRO_CUOTA { get; set; }
        //PROY-29215 FIN
    }
}
