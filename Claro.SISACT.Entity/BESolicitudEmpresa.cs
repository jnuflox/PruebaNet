using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BESolicitudEmpresa
    {
        private Int64 _SOLIN_CODIGO;
        private string _TPROC_CODIGO;
        private string _TACTC_CODIGO;
        private string _SOLIV_DES_TIP_ACT;
        private string _TPROV_DESCRIPCION;
        private int _SEGMN_CODIGO;
        private string _SEGMV_DESCRIPCION;
        private string _OVENV_DESCRIPCION;
        private string _OVENC_CODIGO;
        private string _CANAC_CODIGO;
        private string _ANEXO2;
        private string _ESTAV_DESCRIPCION;
        private string _ESTADO_ACTIVACION;
        private string _TVENC_CODIGO;
        private string _TVENV_DESCRIPCION;
        private DateTime _SOLID_FEC_REG;
        private DateTime _FECHA_APROBACION;
        private string _CLIEV_NOMBRE;
        private string _CLIEV_APE_PAT;
        private string _CLIEV_APE_MAT;
        private string _CLIEV_RAZ_SOC;
        private string _TDOCC_CODIGO;
        private string _ESTAC_CODIGO;
        private string _SOLIC_FLA_TER;

        private string _TIPO_MOTIVO_RECHAZO_ID;
        private string _TIPO_MOTIVO_RECHAZO_DES;
        private string _MOTIVO_RECHAZO;
        private int _CANTIDAD_LINEAS;

        private string _TEVAC_CODIGO;
        private string _USUAV_NOMBRE;
        private string _SOLIC_EXI_BSC_FIN;
        private string _CLIEC_NUM_DOC;
        private string _CLIEN_CAP_SOC;
        private string _PLANV_DESCRIPCION1;
        private string _PLANV_DESCRIPCION2;
        private string _PLANV_DESCRIPCION3;
        private string _PACUV_DESCRIPCION;
        private string _FPAGV_DESCRIPCION;
        private string _FPAGC_CODIGO;
        private int _SOLIN_CAN_LIN;
        private int _SOLIN_CAN_LIN_REG;
        private string _RFINC_CODIGO;
        private string _RFINV_DESCRIPCION;
        private string _TCARV_DESCRIPCION;
        private double _SOLIN_IMP_DG_MAN;
        private string _TDOCV_DESCRIPCION;
        private string _NUM_DOCU;
        private string _RAZON_SOCIAL;
        private string _CCLIC_CODIGO;
        private string _TAFIC_CODIGO;
        private string _SOLIC_COD_TAR;
        private string _SOLIC_COD_TIP_MON;
        private string _SOLIV_NUM_TAR;
        private string _SOLIV_NOM_TIT_TAR;
        private string _SOLIV_DOC_TIT_TAR;
        private DateTime _SOLIV_FEC_VEN_TAR;
        private string _SOLIN_MNTOMAX;
        private double _SOLIN_LIM_CRE_FIN;
        private string _SOLIC_SCO_TXT_FIN;
        private string _SOLIN_SCO_NUM_FIN;
        private string _SOLIN_LIM_MAX_FIN;
        private string _SOLIC_POSTCONTROL;
        private string _CLIEV_TEL_LEG;
        private string _CLIEV_PRE_DIR;
        private string _CLIEV_DIRECCION;
        private string _CLIEV_REF_DIR;
        private string _CLIEC_COD_DEP_DIR;
        private string _CLIEC_COD_PRO_DIR;
        private string _CLIEC_COD_DIS_DIR;
        private string _CLIEC_COD_POS_DIR;
        private string _CLIEV_PRE_DIR_FAC;
        private string _CLIEV_DIR_FAC;
        private string _CLIEV_REF_DIR_FAC;
        private string _CLIEC_COD_DEP_FAC;
        private string _CLIEC_COD_PRO_FAC;
        private string _CLIEC_COD_DIS_FAC;
        private string _CLIEC_COD_POS_FAC;
        private string _DEPAV_DESCRIPCION_LEGAL;
        private string _PROVV_DESCRIPCION_LEGAL;
        private string _DISTV_DESCRIPCION_LEGAL;
        private string _DEPAV_DESCRIPCION_FACTURACION;
        private string _PROVV_DESCRIPCION_FACTURACION;
        private string _DISTV_DESCRIPCION_FACTURACION;
        private string _RDIRV_DESCRIPCION;
        private string _PLANC_CODIGO1;
        private string _PLANC_CODIGO2;
        private string _PLANC_CODIGO3;
        private double _PLANN_CAR_FIJ1;
        private double _PLANN_CAR_FIJ2;
        private double _PLANN_CAR_FIJ3;
        private string _PACUC_CODIGO;
        private string _TCESC_CODIGO;
        private string _TCESC_DESCRIPCION;
        private string _TACTV_DESCRIPCION;
        private string _SOLIC_TIPO_EVALUACION;

        private string _FLEXV_DESCRIPCION;
        private string _AUTORIZADOR;
        private string _CLASC_CODIGO;
        private string _CLASV_DESCRIPCION;
        private string _OPERV_DESCRIPCION;
        private int _SOLIN_CAN_LIN_EXCOMP;
        private string _TRIEC_CODIGO;
        private string _APODV_NOM_REP_LEG;
        private string _APODV_APA_REP_LEG;
        private string _APODV_AMA_REP_LEG;
        private string _APODC_TIP_DOC_REP;
        private string _APODV_NUM_DOC_REP;
        private string _TPODC_CODIGO;
        private string _APODV_CAR_REP;
        private string _TDOCV_DESCRIPCION_REP;
        private double _SOLIN_NUM_CAR_FIJ;
        private double _SOLIN_NUM_CAR_FIJ_LINEA;
        private double _LIMITE_CREDITO1;
        private string _LETRA_SC1;
        private string _NUMERO_SC1;
        private double _LIMITE_CREDITO2;
        private string _LETRA_SC2;
        private int _NUMERO_SC2;
        private double _LIMITE_CREDITO3;
        private string _LETRA_SC3;
        private int _NUMERO_SC3;

        private Int64 _CONSULTOR_ID;
        private string _CONSULTOR_DES;
        private double _SOLIN_DEUDA_CLIENTE;
        private int _SOLIN_LINEA_CLIENTE;
        private double _SOLIN_ANTIGUEDAD;
        private double _SOLIN_ANTIGUEDAD_CLIENTE;

        private string _TOFIC_CODIGO;
        private string _TOFIV_DESCRIPCION;
        private string _SOLIV_COM_PUN_VEN;
        private string _SOLIV_COM_DG;

        private Int64 _SOLIN_CODIGO_PADRE;
        private string _SOLIC_FLAG_REINGRESO;

        private string _NRO_CONTRATO;
        private string _TIPO_OPERACION_ID;
        private string _TIPO_OPERACION_DES;
        private string _CUSTCODE;

        /*Modificado para registro de una solicitud*/
        private string _ANALC_CODIGO;
        private double _CLIEN_PROM_VEN;
        private string _TCLIC_CODIGO;
        private string _SOLIC_TIP_CAR_MAN;
        private string _SOLIV_DES_EST;
        private string _SOLIV_DES_OFI_VEN;
        private string _SOLIV_DES_RES_FIN;
        private string _SOLIV_COM_EVALUADOR;
        private string _SOLIC_USU_CRE;
        private Int64 _USUAN_CODIGO;
        private Int64 _FLEXN_CODIGO;
        private string _OPERC_CODIGO;
        private double _CANTIDAD_CARGOS_FIJOS;
        private Int64 _VENDEDOR_ID;
        private string _VENDEDOR_DES;
        private DateTime _SOLID_FEC_DEPOSITO;
        private string _SOLIV_COD_VOUCHER;
        private string _DISTRIBUIDOR_ID;
        private string _DISTRIBUIDOR_DES;
        private string _ACTIVADOR_ID;
        private string _SOLIV_NUM_OPE_CON;
        private string _SOLIV_COM_DESPACHO;
        private string _ALMAC_CODIGO;
        private string _ALMAV_DESCRIPCION;
        private string _SOLIC_FLAG_EMPRESA_TRAFICO;
        private DateTime _SOLID_FEC_ACTIV;
        private string _FLAG_RESPONSABLE_PUNTO_VENTA;
        private string _SOLIC_FLAG_EMPRESA_TOLERAN;
        private string _SOLIV_EMPRESA_TOP_DES;
        private double _SOLIN_SUM_CAR_FIN;
        private double _SOLIN_CAR_FIJO_ACTUAL;
        private double _SOLIN_SUBSIDIO_TOTAL;

        private DateTime _SOLID_FEC_OBS;
        private double _SOLIN_NUM_CAR_FIJ_ADI;
        private double _SOLIN_NUM_RA;
        private double _SOLIN_IMP_RA;
        private double _SOLIN_NUM_CAR_FIJ_SIS;

        private int _NRO_LINEAS_MAYOR_N_DIAS;
        private int _NRO_LINEAS_MENOR_N_DIAS;
        private int _DIAS_LINEAS_RECURRENTE;
        private int _NRO_LINEAS_RECURRENTE_ACTUAL;
        private string _TIPO_RIESGO_DES;
        private string _FECHA_REG_APROBACION;
        private string _SOLIV_FLAG_ENVIO;

        private List<BERepresentanteLegal> _REPRESENTANTE_LEGAL;

        private double _SOLIN_BOLSA_REF;

        private int _NRO_ORDEN;

        private Int64 _CAMPN_CODIGO;
        private Int64 _TIPO_CLIENTE_ID;
        private string _CONTV_ICCID;
        //private string _OFICINA_ID;
        //private string _OFICINA_DES;

        //T1239 
        private string _FLAG_PORTABILIDAD;
        private int _PORT_OPER_CED;
        private string _TLINC_CODIGO_CEDENTE;
        private string _PORT_SOLIN_NRO_FORMATO;
        private string _PORT_CARGO_FIJO_OPE_CED;
        private string _PORT_ESTADO;

        //E75606 - Portabilidad
        private string _PORT_ESTADO_DESC;
        private string _PORT_COM_MP;
        private string _OPE_DESCRIPCION;
        private string _PORT_TELEF_CONT;

        //INICIO - E75688
        private string _FLAG_CORREO;
        private string _SOLIV_CORREO;
        private string _SOLIV_TELCONF_SMS;
        //FIN

        private ArrayList _ARCHIVOS;
        private ArrayList _SOLICITUDES_PORTABILIDAD;
        private ArrayList _ARCHIVOS_PORTABILIDAD;

        //gaa20120718
        private string _TPROD_COMERCIALIZAR;
        //fin gaa20120718

        //E75785
        private string _CAMPV_DESCRIPCION;

        // T12618 - SMS Corporativo - INICIO
        private string _EMAIL_AUTORIZADO;
        // T12618 - SMS Corporativo - FIN

        private double _SOLIN_LINEA_CREDITO_CALC = 0.0; //--E75810 28/01/2011

        //maryta
        private string _DEPAV_DESCRIPCION;
        private string _PROVV_DESCRIPCION;
        private string _DISTV_DESCRIPCION;
        private string _DEPAV_DESCRIPCION_FAC;
        private string _PROVV_DESCRIPCION_FAC;
        private string _DISTV_DESCRIPCION_FAC;
        private string _CLIEV_CORREO;
        private string _DPCHV_DESCRIPCION;
        //fin maryta

        //JAR
        private string _CLIEV_PRE_TEL_LEG;
        private Int64 _DPCHN_CODIGO;
        private string _RGLPC_PODERES;
        private Int64 _TOPEN_CODIGO;
        private string _SOLIC_TIP_CAR_FIJ;
        private double _SOLIN_IMP_DG;
        private string _TPREC_DESCRIPCION;
        private string _TPREC_CODIGO;
        private string _SOLIN_USU_VEN;
        private string _CLIEV_CODIGO_SAP;
        //
        private double _SOLIN_SUM_CAR_CON = 0.0;

        //lucas
        private double _cant_pendiente;
        private string _matec_codigo;
        private string _Fecha_registro;
        private string _SOLIV_NUM_CON;
        private string _RIESGO;
        private string _PRDV_DESCRIPCION;
        private string _PRDC_CODIGO;

        private string _CLIEV_CALIFICACION_PAGO;
        private string _BURO_DESCRIPCION;
        private int _BURO_CODIGO;
        private Int64 _SOLIN_GRUPO_SEC;
        private double _SOLIN_IMP_DG_GRUPO_SEC;
        private double _SOLIN_CF_GRUPO_SEC;
        private double _CLIEN_MONTO_VENCIDO;

        private string _CLIEV_RIESGO_CLARO;
        private string _CLIEV_COMPORTA_PAGO;
        private string _CLIEC_FLAG_EXONERAR_RA;

        public string CREDV_OBS_FLEXIBILIZACION { get; set; }
        public string CREDV_MOTIVO { get; set; }
        public string TOPEV_DESCRIPCION { get; set; }
        public double SOLIN_KIT_COS_INST { get; set; }

        public double SOLIV_FACTOR_RENOVACION { get; set; }
        public string TELEFONO { get; set; }
        public double RENOF_CFACTUAL { get; set; }
        public string PLAN_ACTUAL { get; set; }
        public string COMBV_DESCRIPCION { get; set; }

        public string Fecha_registro
        {
            get { return _Fecha_registro; }
            set { _Fecha_registro = value; }
        }


        public string matec_codigo
        {
            get { return _matec_codigo; }
            set { _matec_codigo = value; }
        }

        public double cant_pendiente
        {
            get { return _cant_pendiente; }
            set { _cant_pendiente = value; }
        }


        public Int64 SOLIN_CODIGO
        {
            get { return _SOLIN_CODIGO; }
            set { _SOLIN_CODIGO = value; }
        }
        public string TPROC_CODIGO
        {
            get { return _TPROC_CODIGO; }
            set { _TPROC_CODIGO = value; }
        }
        public string TACTC_CODIGO
        {
            get { return _TACTC_CODIGO; }
            set { _TACTC_CODIGO = value; }
        }
        public string SOLIV_DES_TIP_ACT
        {
            get { return _SOLIV_DES_TIP_ACT; }
            set { _SOLIV_DES_TIP_ACT = value; }
        }
        public string TPROV_DESCRIPCION
        {
            get { return _TPROV_DESCRIPCION; }
            set { _TPROV_DESCRIPCION = value; }
        }
        public int SEGMN_CODIGO
        {
            get { return _SEGMN_CODIGO; }
            set { _SEGMN_CODIGO = value; }
        }
        public string SEGMV_DESCRIPCION
        {
            get { return _SEGMV_DESCRIPCION; }
            set { _SEGMV_DESCRIPCION = value; }
        }
        public string OVENV_DESCRIPCION
        {
            get { return _OVENV_DESCRIPCION; }
            set { _OVENV_DESCRIPCION = value; }
        }
        public string OVENC_CODIGO
        {
            get { return _OVENC_CODIGO; }
            set { _OVENC_CODIGO = value; }
        }
        public string CANAC_CODIGO
        {
            get { return _CANAC_CODIGO; }
            set { _CANAC_CODIGO = value; }
        }
        public string ANEXO2
        {
            get { return _ANEXO2; }
            set { _ANEXO2 = value; }
        }
        public string ESTAV_DESCRIPCION
        {
            get { return _ESTAV_DESCRIPCION; }
            set { _ESTAV_DESCRIPCION = value; }
        }
        public string ESTADO_ACTIVACION
        {
            get { return _ESTADO_ACTIVACION; }
            set { _ESTADO_ACTIVACION = value; }
        }
        public string TVENC_CODIGO
        {
            get { return _TVENC_CODIGO; }
            set { _TVENC_CODIGO = value; }
        }
        public string TVENV_DESCRIPCION
        {
            get { return _TVENV_DESCRIPCION; }
            set { _TVENV_DESCRIPCION = value; }
        }
        public DateTime SOLID_FEC_REG
        {
            get { return _SOLID_FEC_REG; }
            set { _SOLID_FEC_REG = value; }
        }
        public DateTime FECHA_APROBACION
        {
            get { return _FECHA_APROBACION; }
            set { _FECHA_APROBACION = value; }
        }
        public string CLIEV_NOMBRE
        {
            get { return _CLIEV_NOMBRE; }
            set { _CLIEV_NOMBRE = value; }
        }
        public string CLIEV_APE_PAT
        {
            get { return _CLIEV_APE_PAT; }
            set { _CLIEV_APE_PAT = value; }
        }
        public string CLIEV_APE_MAT
        {
            get { return _CLIEV_APE_MAT; }
            set { _CLIEV_APE_MAT = value; }
        }
        public string CLIEV_RAZ_SOC
        {
            get { return _CLIEV_RAZ_SOC; }
            set { _CLIEV_RAZ_SOC = value; }
        }
        public string TDOCC_CODIGO
        {
            get { return _TDOCC_CODIGO; }
            set { _TDOCC_CODIGO = value; }
        }
        public string ESTAC_CODIGO
        {
            get { return _ESTAC_CODIGO; }
            set { _ESTAC_CODIGO = value; }
        }
        public string SOLIC_FLA_TER
        {
            get { return _SOLIC_FLA_TER; }
            set { _SOLIC_FLA_TER = value; }
        }
        public string TIPO_MOTIVO_RECHAZO_ID
        {
            get { return _TIPO_MOTIVO_RECHAZO_ID; }
            set { _TIPO_MOTIVO_RECHAZO_ID = value; }
        }
        public int CANTIDAD_LINEAS
        {
            get { return _CANTIDAD_LINEAS; }
            set { _CANTIDAD_LINEAS = value; }
        }

        public string TIPO_MOTIVO_RECHAZO_DES
        {
            get { return _TIPO_MOTIVO_RECHAZO_DES; }
            set { _TIPO_MOTIVO_RECHAZO_DES = value; }
        }
        public string MOTIVO_RECHAZO
        {
            get { return _MOTIVO_RECHAZO; }
            set { _MOTIVO_RECHAZO = value; }
        }
        public string TEVAC_CODIGO
        {
            get { return _TEVAC_CODIGO; }
            set { _TEVAC_CODIGO = value; }
        }
        public string USUAV_NOMBRE
        {
            get { return _USUAV_NOMBRE; }
            set { _USUAV_NOMBRE = value; }
        }
        public string SOLIC_EXI_BSC_FIN
        {
            get { return _SOLIC_EXI_BSC_FIN; }
            set { _SOLIC_EXI_BSC_FIN = value; }
        }
        public string CLIEC_NUM_DOC
        {
            get { return _CLIEC_NUM_DOC; }
            set { _CLIEC_NUM_DOC = value; }
        }
        public string CLIEN_CAP_SOC
        {
            get { return _CLIEN_CAP_SOC; }
            set { _CLIEN_CAP_SOC = value; }
        }
        public string PLANV_DESCRIPCION1
        {
            get { return _PLANV_DESCRIPCION1; }
            set { _PLANV_DESCRIPCION1 = value; }
        }
        public string PLANV_DESCRIPCION2
        {
            get { return _PLANV_DESCRIPCION2; }
            set { _PLANV_DESCRIPCION2 = value; }
        }
        public string PLANV_DESCRIPCION3
        {
            get { return _PLANV_DESCRIPCION3; }
            set { _PLANV_DESCRIPCION3 = value; }
        }
        public string PACUV_DESCRIPCION
        {
            get { return _PACUV_DESCRIPCION; }
            set { _PACUV_DESCRIPCION = value; }
        }
        public string FPAGV_DESCRIPCION
        {
            get { return _FPAGV_DESCRIPCION; }
            set { _FPAGV_DESCRIPCION = value; }
        }
        public int SOLIN_CAN_LIN
        {
            get { return _SOLIN_CAN_LIN; }
            set { _SOLIN_CAN_LIN = value; }
        }
        public int SOLIN_CAN_LIN_REG
        {
            get { return _SOLIN_CAN_LIN_REG; }
            set { _SOLIN_CAN_LIN_REG = value; }
        }
        public string RFINC_CODIGO
        {
            get { return _RFINC_CODIGO; }
            set { _RFINC_CODIGO = value; }
        }
        public string RFINV_DESCRIPCION
        {
            get { return _RFINV_DESCRIPCION; }
            set { _RFINV_DESCRIPCION = value; }
        }
        public string TCARV_DESCRIPCION
        {
            get { return _TCARV_DESCRIPCION; }
            set { _TCARV_DESCRIPCION = value; }
        }
        public double SOLIN_IMP_DG_MAN
        {
            get { return _SOLIN_IMP_DG_MAN; }
            set { _SOLIN_IMP_DG_MAN = value; }
        }
        public string TDOCV_DESCRIPCION
        {
            get { return _TDOCV_DESCRIPCION; }
            set { _TDOCV_DESCRIPCION = value; }
        }
        public string NUM_DOCU
        {
            get { return _NUM_DOCU; }
            set { _NUM_DOCU = value; }
        }
        public string RAZON_SOCIAL
        {
            get { return _RAZON_SOCIAL; }
            set { _RAZON_SOCIAL = value; }
        }
        public string CCLIC_CODIGO
        {
            get { return _CCLIC_CODIGO; }
            set { _CCLIC_CODIGO = value; }
        }
        public string TAFIC_CODIGO
        {
            get { return _TAFIC_CODIGO; }
            set { _TAFIC_CODIGO = value; }
        }
        public string SOLIC_COD_TAR
        {
            get { return _SOLIC_COD_TAR; }
            set { _SOLIC_COD_TAR = value; }
        }
        public string SOLIC_COD_TIP_MON
        {
            get { return _SOLIC_COD_TIP_MON; }
            set { _SOLIC_COD_TIP_MON = value; }
        }
        public string SOLIV_NUM_TAR
        {
            get { return _SOLIV_NUM_TAR; }
            set { _SOLIV_NUM_TAR = value; }
        }
        public string SOLIV_NOM_TIT_TAR
        {
            get { return _SOLIV_NOM_TIT_TAR; }
            set { _SOLIV_NOM_TIT_TAR = value; }
        }
        public string SOLIV_DOC_TIT_TAR
        {
            get { return _SOLIV_DOC_TIT_TAR; }
            set { _SOLIV_DOC_TIT_TAR = value; }
        }
        public DateTime SOLIV_FEC_VEN_TAR
        {
            get { return _SOLIV_FEC_VEN_TAR; }
            set { _SOLIV_FEC_VEN_TAR = value; }
        }
        public string SOLIN_MNTOMAX
        {
            get { return _SOLIN_MNTOMAX; }
            set { _SOLIN_MNTOMAX = value; }
        }
        public double SOLIN_LIM_CRE_FIN
        {
            get { return _SOLIN_LIM_CRE_FIN; }
            set { _SOLIN_LIM_CRE_FIN = value; }
        }
        public string SOLIC_SCO_TXT_FIN
        {
            get { return _SOLIC_SCO_TXT_FIN; }
            set { _SOLIC_SCO_TXT_FIN = value; }
        }
        public string SOLIN_SCO_NUM_FIN
        {
            get { return _SOLIN_SCO_NUM_FIN; }
            set { _SOLIN_SCO_NUM_FIN = value; }
        }
        public string SOLIN_LIM_MAX_FIN
        {
            get { return _SOLIN_LIM_MAX_FIN; }
            set { _SOLIN_LIM_MAX_FIN = value; }
        }
        public string SOLIC_POSTCONTROL
        {
            get { return _SOLIC_POSTCONTROL; }
            set { _SOLIC_POSTCONTROL = value; }
        }
        public string CLIEV_TEL_LEG
        {
            get { return _CLIEV_TEL_LEG; }
            set { _CLIEV_TEL_LEG = value; }
        }
        public string CLIEV_PRE_DIR
        {
            get { return _CLIEV_PRE_DIR; }
            set { _CLIEV_PRE_DIR = value; }
        }
        public string CLIEV_DIRECCION
        {
            get { return _CLIEV_DIRECCION; }
            set { _CLIEV_DIRECCION = value; }
        }
        public string CLIEV_REF_DIR
        {
            get { return _CLIEV_REF_DIR; }
            set { _CLIEV_REF_DIR = value; }
        }
        public string CLIEC_COD_DEP_DIR
        {
            get { return _CLIEC_COD_DEP_DIR; }
            set { _CLIEC_COD_DEP_DIR = value; }
        }
        public string CLIEC_COD_PRO_DIR
        {
            get { return _CLIEC_COD_PRO_DIR; }
            set { _CLIEC_COD_PRO_DIR = value; }
        }
        public string CLIEC_COD_DIS_DIR
        {
            get { return _CLIEC_COD_DIS_DIR; }
            set { _CLIEC_COD_DIS_DIR = value; }
        }
        public string CLIEC_COD_POS_DIR
        {
            get { return _CLIEC_COD_POS_DIR; }
            set { _CLIEC_COD_POS_DIR = value; }
        }
        public string CLIEV_PRE_DIR_FAC
        {
            get { return _CLIEV_PRE_DIR_FAC; }
            set { _CLIEV_PRE_DIR_FAC = value; }
        }
        public string CLIEV_DIR_FAC
        {
            get { return _CLIEV_DIR_FAC; }
            set { _CLIEV_DIR_FAC = value; }
        }
        public string CLIEV_REF_DIR_FAC
        {
            get { return _CLIEV_REF_DIR_FAC; }
            set { _CLIEV_REF_DIR_FAC = value; }
        }
        public string CLIEC_COD_DEP_FAC
        {
            get { return _CLIEC_COD_DEP_FAC; }
            set { _CLIEC_COD_DEP_FAC = value; }
        }
        public string CLIEC_COD_PRO_FAC
        {
            get { return _CLIEC_COD_PRO_FAC; }
            set { _CLIEC_COD_PRO_FAC = value; }
        }
        public string CLIEC_COD_DIS_FAC
        {
            get { return _CLIEC_COD_DIS_FAC; }
            set { _CLIEC_COD_DIS_FAC = value; }
        }
        public string CLIEC_COD_POS_FAC
        {
            get { return _CLIEC_COD_POS_FAC; }
            set { _CLIEC_COD_POS_FAC = value; }
        }
        public string DEPAV_DESCRIPCION_LEGAL
        {
            get { return _DEPAV_DESCRIPCION_LEGAL; }
            set { _DEPAV_DESCRIPCION_LEGAL = value; }
        }
        public string PROVV_DESCRIPCION_LEGAL
        {
            get { return _PROVV_DESCRIPCION_LEGAL; }
            set { _PROVV_DESCRIPCION_LEGAL = value; }
        }
        public string DISTV_DESCRIPCION_LEGAL
        {
            get { return _DISTV_DESCRIPCION_LEGAL; }
            set { _DISTV_DESCRIPCION_LEGAL = value; }
        }
        public string DEPAV_DESCRIPCION_FACTURACION
        {
            get { return _DEPAV_DESCRIPCION_FACTURACION; }
            set { _DEPAV_DESCRIPCION_FACTURACION = value; }
        }
        public string PROVV_DESCRIPCION_FACTURACION
        {
            get { return _PROVV_DESCRIPCION_FACTURACION; }
            set { _PROVV_DESCRIPCION_FACTURACION = value; }
        }
        public string DISTV_DESCRIPCION_FACTURACION
        {
            get { return _DISTV_DESCRIPCION_FACTURACION; }
            set { _DISTV_DESCRIPCION_FACTURACION = value; }
        }
        public string RDIRV_DESCRIPCION
        {
            get { return _RDIRV_DESCRIPCION; }
            set { _RDIRV_DESCRIPCION = value; }
        }
        public string PLANC_CODIGO1
        {
            get { return _PLANC_CODIGO1; }
            set { _PLANC_CODIGO1 = value; }
        }
        public string PLANC_CODIGO2
        {
            get { return _PLANC_CODIGO2; }
            set { _PLANC_CODIGO2 = value; }
        }
        public string PLANC_CODIGO3
        {
            get { return _PLANC_CODIGO3; }
            set { _PLANC_CODIGO3 = value; }
        }
        public double PLANN_CAR_FIJ1
        {
            get { return _PLANN_CAR_FIJ1; }
            set { _PLANN_CAR_FIJ1 = value; }
        }
        public double PLANN_CAR_FIJ2
        {
            get { return _PLANN_CAR_FIJ2; }
            set { _PLANN_CAR_FIJ2 = value; }
        }
        public double PLANN_CAR_FIJ3
        {
            get { return _PLANN_CAR_FIJ3; }
            set { _PLANN_CAR_FIJ3 = value; }
        }
        public string PACUC_CODIGO
        {
            get { return _PACUC_CODIGO; }
            set { _PACUC_CODIGO = value; }
        }
        public string FPAGC_CODIGO
        {
            get { return _FPAGC_CODIGO; }
            set { _FPAGC_CODIGO = value; }
        }
        public string TCESC_CODIGO
        {
            get { return _TCESC_CODIGO; }
            set { _TCESC_CODIGO = value; }
        }
        public string TCESC_DESCRIPCION
        {
            get { return _TCESC_DESCRIPCION; }
            set { _TCESC_DESCRIPCION = value; }
        }
        public string SOLIC_TIPO_EVALUACION
        {
            get { return _SOLIC_TIPO_EVALUACION; }
            set { _SOLIC_TIPO_EVALUACION = value; }
        }
        public string TACTV_DESCRIPCION
        {
            get { return _TACTV_DESCRIPCION; }
            set { _TACTV_DESCRIPCION = value; }
        }
        public string FLEXV_DESCRIPCION
        {
            get { return _FLEXV_DESCRIPCION; }
            set { _FLEXV_DESCRIPCION = value; }
        }
        public string AUTORIZADOR
        {
            get { return _AUTORIZADOR; }
            set { _AUTORIZADOR = value; }
        }
        public string CLASC_CODIGO
        {
            get { return _CLASC_CODIGO; }
            set { _CLASC_CODIGO = value; }
        }
        public string CLASV_DESCRIPCION
        {
            get { return _CLASV_DESCRIPCION; }
            set { _CLASV_DESCRIPCION = value; }
        }
        public string OPERV_DESCRIPCION
        {
            get { return _OPERV_DESCRIPCION; }
            set { _OPERV_DESCRIPCION = value; }
        }
        public int SOLIN_CAN_LIN_EXCOMP
        {
            get { return _SOLIN_CAN_LIN_EXCOMP; }
            set { _SOLIN_CAN_LIN_EXCOMP = value; }
        }
        public string TRIEC_CODIGO
        {
            get { return _TRIEC_CODIGO; }
            set { _TRIEC_CODIGO = value; }
        }
        public string APODV_NOM_REP_LEG
        {
            get { return _APODV_NOM_REP_LEG; }
            set { _APODV_NOM_REP_LEG = value; }
        }
        public string APODV_APA_REP_LEG
        {
            get { return _APODV_APA_REP_LEG; }
            set { _APODV_APA_REP_LEG = value; }
        }
        public string APODV_AMA_REP_LEG
        {
            get { return _APODV_AMA_REP_LEG; }
            set { _APODV_AMA_REP_LEG = value; }
        }
        public string APODC_TIP_DOC_REP
        {
            get { return _APODC_TIP_DOC_REP; }
            set { _APODC_TIP_DOC_REP = value; }
        }
        public string APODV_NUM_DOC_REP
        {
            get { return _APODV_NUM_DOC_REP; }
            set { _APODV_NUM_DOC_REP = value; }
        }
        public string TPODC_CODIGO
        {
            get { return _TPODC_CODIGO; }
            set { _TPODC_CODIGO = value; }
        }
        public string APODV_CAR_REP
        {
            get { return _APODV_CAR_REP; }
            set { _APODV_CAR_REP = value; }
        }
        public string TDOCV_DESCRIPCION_REP
        {
            get { return _TDOCV_DESCRIPCION_REP; }
            set { _TDOCV_DESCRIPCION_REP = value; }
        }
        public double SOLIN_NUM_CAR_FIJ
        {
            get { return _SOLIN_NUM_CAR_FIJ; }
            set { _SOLIN_NUM_CAR_FIJ = value; }
        }
        public double SOLIN_NUM_CAR_FIJ_LINEA
        {
            get { return _SOLIN_NUM_CAR_FIJ_LINEA; }
            set { _SOLIN_NUM_CAR_FIJ_LINEA = value; }
        }
        public double LIMITE_CREDITO1
        {
            get { return _LIMITE_CREDITO1; }
            set { _LIMITE_CREDITO1 = value; }
        }
        public string LETRA_SC1
        {
            get { return _LETRA_SC1; }
            set { _LETRA_SC1 = value; }
        }
        public string NUMERO_SC1
        {
            get { return _NUMERO_SC1; }
            set { _NUMERO_SC1 = value; }
        }
        public double LIMITE_CREDITO2
        {
            get { return _LIMITE_CREDITO2; }
            set { _LIMITE_CREDITO2 = value; }
        }
        public string LETRA_SC2
        {
            get { return _LETRA_SC2; }
            set { _LETRA_SC2 = value; }
        }
        public int NUMERO_SC2
        {
            get { return _NUMERO_SC2; }
            set { _NUMERO_SC2 = value; }
        }
        public double LIMITE_CREDITO3
        {
            get { return _LIMITE_CREDITO3; }
            set { _LIMITE_CREDITO3 = value; }
        }
        public string LETRA_SC3
        {
            get { return _LETRA_SC3; }
            set { _LETRA_SC3 = value; }
        }
        public int NUMERO_SC3
        {
            get { return _NUMERO_SC3; }
            set { _NUMERO_SC3 = value; }
        }

        public string ANALC_CODIGO
        {
            get { return _ANALC_CODIGO; }
            set { _ANALC_CODIGO = value; }
        }
        public double CLIEN_PROM_VEN
        {
            get { return _CLIEN_PROM_VEN; }
            set { _CLIEN_PROM_VEN = value; }
        }
        public string TCLIC_CODIGO
        {
            get { return _TCLIC_CODIGO; }
            set { _TCLIC_CODIGO = value; }
        }
        public string SOLIC_TIP_CAR_MAN
        {
            get { return _SOLIC_TIP_CAR_MAN; }
            set { _SOLIC_TIP_CAR_MAN = value; }
        }
        public string SOLIV_DES_EST
        {
            get { return _SOLIV_DES_EST; }
            set { _SOLIV_DES_EST = value; }
        }
        public string SOLIV_DES_OFI_VEN
        {
            get { return _SOLIV_DES_OFI_VEN; }
            set { _SOLIV_DES_OFI_VEN = value; }
        }
        public string SOLIV_DES_RES_FIN
        {
            get { return _SOLIV_DES_RES_FIN; }
            set { _SOLIV_DES_RES_FIN = value; }
        }

        public string SOLIV_COM_EVALUADOR
        {
            get { return _SOLIV_COM_EVALUADOR; }
            set { _SOLIV_COM_EVALUADOR = value; }
        }
        public string SOLIC_USU_CRE
        {
            get { return _SOLIC_USU_CRE; }
            set { _SOLIC_USU_CRE = value; }
        }
        public Int64 USUAN_CODIGO
        {
            get { return _USUAN_CODIGO; }
            set { _USUAN_CODIGO = value; }
        }
        public Int64 FLEXN_CODIGO
        {
            get { return _FLEXN_CODIGO; }
            set { _FLEXN_CODIGO = value; }
        }
        public string OPERC_CODIGO
        {
            get { return _OPERC_CODIGO; }
            set { _OPERC_CODIGO = value; }
        }
        public double CANTIDAD_CARGOS_FIJOS
        {
            get { return _CANTIDAD_CARGOS_FIJOS; }
            set { _CANTIDAD_CARGOS_FIJOS = value; }
        }
        public Int64 VENDEDOR_ID
        {
            get { return _VENDEDOR_ID; }
            set { _VENDEDOR_ID = value; }
        }
        public string VENDEDOR_DES
        {
            get { return _VENDEDOR_DES; }
            set { _VENDEDOR_DES = value; }
        }
        public Int64 CONSULTOR_ID
        {
            get { return _CONSULTOR_ID; }
            set { _CONSULTOR_ID = value; }
        }
        public string CONSULTOR_DES
        {
            get { return _CONSULTOR_DES; }
            set { _CONSULTOR_DES = value; }
        }
        public double SOLIN_DEUDA_CLIENTE
        {
            get { return _SOLIN_DEUDA_CLIENTE; }
            set { _SOLIN_DEUDA_CLIENTE = value; }
        }
        public int SOLIN_LINEA_CLIENTE
        {
            get { return _SOLIN_LINEA_CLIENTE; }
            set { _SOLIN_LINEA_CLIENTE = value; }
        }
        public double SOLIN_ANTIGUEDAD
        {
            get { return _SOLIN_ANTIGUEDAD; }
            set { _SOLIN_ANTIGUEDAD = value; }
        }
        public double SOLIN_ANTIGUEDAD_CLIENTE
        {
            get { return _SOLIN_ANTIGUEDAD_CLIENTE; }
            set { _SOLIN_ANTIGUEDAD_CLIENTE = value; }
        }

        public string TOFIC_CODIGO
        {
            get { return _TOFIC_CODIGO; }
            set { _TOFIC_CODIGO = value; }
        }
        public string TOFIV_DESCRIPCION
        {
            get { return _TOFIV_DESCRIPCION; }
            set { _TOFIV_DESCRIPCION = value; }
        }
        public string SOLIV_COM_PUN_VEN
        {
            get { return _SOLIV_COM_PUN_VEN; }
            set { _SOLIV_COM_PUN_VEN = value; }
        }
        public string SOLIV_COM_DG
        {
            get { return _SOLIV_COM_DG; }
            set { _SOLIV_COM_DG = value; }
        }
        public Int64 SOLIN_CODIGO_PADRE
        {
            get { return _SOLIN_CODIGO_PADRE; }
            set { _SOLIN_CODIGO_PADRE = value; }
        }
        public string SOLIC_FLAG_REINGRESO
        {
            get { return _SOLIC_FLAG_REINGRESO; }
            set { _SOLIC_FLAG_REINGRESO = value; }
        }
        public DateTime SOLID_FEC_DEPOSITO
        {
            get { return _SOLID_FEC_DEPOSITO; }
            set { _SOLID_FEC_DEPOSITO = value; }
        }
        public string SOLIV_COD_VOUCHER
        {
            get { return _SOLIV_COD_VOUCHER; }
            set { _SOLIV_COD_VOUCHER = value; }
        }
        public string DISTRIBUIDOR_ID
        {
            get { return _DISTRIBUIDOR_ID; }
            set { _DISTRIBUIDOR_ID = value; }
        }
        public string DISTRIBUIDOR_DES
        {
            get { return _DISTRIBUIDOR_DES; }
            set { _DISTRIBUIDOR_DES = value; }
        }
        public string ACTIVADOR_ID
        {
            get { return _ACTIVADOR_ID; }
            set { _ACTIVADOR_ID = value; }
        }
        public string SOLIV_NUM_OPE_CON
        {
            get { return _SOLIV_NUM_OPE_CON; }
            set { _SOLIV_NUM_OPE_CON = value; }
        }
        public string SOLIV_COM_DESPACHO
        {
            get { return _SOLIV_COM_DESPACHO; }
            set { _SOLIV_COM_DESPACHO = value; }
        }
        public string ALMAC_CODIGO
        {
            get { return _ALMAC_CODIGO; }
            set { _ALMAC_CODIGO = value; }
        }
        public string ALMAV_DESCRIPCION
        {
            get { return _ALMAV_DESCRIPCION; }
            set { _ALMAV_DESCRIPCION = value; }
        }
        public string SOLIC_FLAG_EMPRESA_TRAFICO
        {
            get { return _SOLIC_FLAG_EMPRESA_TRAFICO; }
            set { _SOLIC_FLAG_EMPRESA_TRAFICO = value; }
        }
        public DateTime SOLID_FEC_ACTIV
        {
            get { return _SOLID_FEC_ACTIV; }
            set { _SOLID_FEC_ACTIV = value; }
        }

        public List<BERepresentanteLegal> REPRESENTANTE_LEGAL
        {
            get { return _REPRESENTANTE_LEGAL; }
            set { _REPRESENTANTE_LEGAL = value; }
        }

        public string NRO_CONTRATO
        {
            get { return _NRO_CONTRATO; }
            set { _NRO_CONTRATO = value; }
        }

        public string TIPO_OPERACION_ID
        {
            get { return _TIPO_OPERACION_ID; }
            set { _TIPO_OPERACION_ID = value; }
        }
        public string TIPO_OPERACION_DES
        {
            get { return _TIPO_OPERACION_DES; }
            set { _TIPO_OPERACION_DES = value; }
        }
        public string CUSTCODE
        {
            get { return _CUSTCODE; }
            set { _CUSTCODE = value; }
        }
        public string FLAG_RESPONSABLE_PUNTO_VENTA
        {
            get { return _FLAG_RESPONSABLE_PUNTO_VENTA; }
            set { _FLAG_RESPONSABLE_PUNTO_VENTA = value; }
        }
        public string SOLIC_FLAG_EMPRESA_TOLERAN
        {
            get { return _SOLIC_FLAG_EMPRESA_TOLERAN; }
            set { _SOLIC_FLAG_EMPRESA_TOLERAN = value; }
        }
        public string SOLIV_EMPRESA_TOP_DES
        {
            get { return _SOLIV_EMPRESA_TOP_DES; }
            set { _SOLIV_EMPRESA_TOP_DES = value; }
        }
        public int NRO_ORDEN
        {
            get { return _NRO_ORDEN; }
            set { _NRO_ORDEN = value; }
        }

        public double SOLIN_SUM_CAR_FIN
        {
            get { return _SOLIN_SUM_CAR_FIN; }
            set { _SOLIN_SUM_CAR_FIN = value; }
        }
        public double SOLIN_CAR_FIJO_ACTUAL
        {
            get { return _SOLIN_CAR_FIJO_ACTUAL; }
            set { _SOLIN_CAR_FIJO_ACTUAL = value; }
        }

        public double SOLIN_SUBSIDIO_TOTAL
        {
            get { return _SOLIN_SUBSIDIO_TOTAL; }
            set { _SOLIN_SUBSIDIO_TOTAL = value; }
        }

        public DateTime SOLID_FEC_OBS
        {
            get { return _SOLID_FEC_OBS; }
            set { _SOLID_FEC_OBS = value; }
        }
        public double SOLIN_NUM_CAR_FIJ_ADI
        {
            get { return _SOLIN_NUM_CAR_FIJ_ADI; }
            set { _SOLIN_NUM_CAR_FIJ_ADI = value; }
        }
        public double SOLIN_NUM_RA
        {
            get { return _SOLIN_NUM_RA; }
            set { _SOLIN_NUM_RA = value; }
        }
        public double SOLIN_IMP_RA
        {
            get { return _SOLIN_IMP_RA; }
            set { _SOLIN_IMP_RA = value; }
        }
        public double SOLIN_NUM_CAR_FIJ_SIS
        {
            get { return _SOLIN_NUM_CAR_FIJ_SIS; }
            set { _SOLIN_NUM_CAR_FIJ_SIS = value; }
        }

        public int NRO_LINEAS_MAYOR_N_DIAS
        {
            get { return _NRO_LINEAS_MAYOR_N_DIAS; }
            set { _NRO_LINEAS_MAYOR_N_DIAS = value; }
        }
        public int NRO_LINEAS_MENOR_N_DIAS
        {
            get { return _NRO_LINEAS_MENOR_N_DIAS; }
            set { _NRO_LINEAS_MENOR_N_DIAS = value; }
        }
        public int DIAS_LINEAS_RECURRENTE
        {
            get { return _DIAS_LINEAS_RECURRENTE; }
            set { _DIAS_LINEAS_RECURRENTE = value; }
        }
        public int NRO_LINEAS_RECURRENTE_ACTUAL
        {
            get { return _NRO_LINEAS_RECURRENTE_ACTUAL; }
            set { _NRO_LINEAS_RECURRENTE_ACTUAL = value; }
        }

        public string TIPO_RIESGO_DES
        {
            get { return _TIPO_RIESGO_DES; }
            set { _TIPO_RIESGO_DES = value; }
        }
        public string FECHA_REG_APROBACION
        {
            get { return _FECHA_REG_APROBACION; }
            set { _FECHA_REG_APROBACION = value; }
        }
        public string SOLIV_FLAG_ENVIO
        {
            get { return _SOLIV_FLAG_ENVIO; }
            set { _SOLIV_FLAG_ENVIO = value; }
        }
        public double SOLIN_BOLSA_REF
        {
            get { return _SOLIN_BOLSA_REF; }
            set { _SOLIN_BOLSA_REF = value; }
        }
        public Int64 CAMPN_CODIGO
        {
            get { return _CAMPN_CODIGO; }
            set { _CAMPN_CODIGO = value; }
        }
        public Int64 TIPO_CLIENTE_ID
        {
            get { return _TIPO_CLIENTE_ID; }
            set { _TIPO_CLIENTE_ID = value; }
        }
        public string CONTV_ICCID
        {
            get { return _CONTV_ICCID; }
            set { _CONTV_ICCID = value; }
        }
        //T12639
        public string FLAG_PORTABILIDAD
        {
            get { return _FLAG_PORTABILIDAD; }
            set { _FLAG_PORTABILIDAD = value; }
        }
        public int PORT_OPER_CED
        {
            set { _PORT_OPER_CED = value; }
            get { return _PORT_OPER_CED; }
        }
        public string TLINC_CODIGO_CEDENTE
        {
            set { _TLINC_CODIGO_CEDENTE = value; }
            get { return _TLINC_CODIGO_CEDENTE; }
        }
        public string PORT_SOLIN_NRO_FORMATO
        {
            set { _PORT_SOLIN_NRO_FORMATO = value; }
            get { return _PORT_SOLIN_NRO_FORMATO; }
        }
        public string PORT_CARGO_FIJO_OPE_CED
        {
            set { _PORT_CARGO_FIJO_OPE_CED = value; }
            get { return _PORT_CARGO_FIJO_OPE_CED; }
        }
        public string PORT_ESTADO
        {
            set { _PORT_ESTADO = value; }
            get { return _PORT_ESTADO; }
        }

        //E75606 - Portabilidad
        public string PORT_ESTADO_DESC
        {
            set { _PORT_ESTADO_DESC = value; }
            get { return _PORT_ESTADO_DESC; }
        }
        public string PORT_COM_MP
        {
            set { _PORT_COM_MP = value; }
            get { return _PORT_COM_MP; }
        }
        public string OPE_DESCRIPCION
        {
            set { _OPE_DESCRIPCION = value; }
            get { return _OPE_DESCRIPCION; }
        }
        public string PORT_TELEF_CONT
        {
            set { _PORT_TELEF_CONT = value; }
            get { return _PORT_TELEF_CONT; }
        }
        public ArrayList ARCHIVOS
        {
            get { return _ARCHIVOS; }
            set { _ARCHIVOS = value; }
        }
        public ArrayList SOLICITUDES_PORTABILIDAD
        {
            get { return _SOLICITUDES_PORTABILIDAD; }
            set { _SOLICITUDES_PORTABILIDAD = value; }
        }
        public ArrayList ARCHIVOS_PORTABILIDAD
        {
            get { return _ARCHIVOS_PORTABILIDAD; }
            set { _ARCHIVOS_PORTABILIDAD = value; }
        }

        //INICIO - E75688
        public string FLAG_CORREO { set { _FLAG_CORREO = value; } get { return _FLAG_CORREO; } }
        public string SOLIV_CORREO { set { _SOLIV_CORREO = value; } get { return _SOLIV_CORREO; } }
        public string SOLIV_TELCONF_SMS { set { _SOLIV_TELCONF_SMS = value; } get { return _SOLIV_TELCONF_SMS; } }
        //FIN - E75688

        //gaa20120718
        public string TPROD_COMERCIALIZAR
        {
            get { return _TPROD_COMERCIALIZAR; }
            set { _TPROD_COMERCIALIZAR = value; }
        }
        //fin gaa20120718

        public string CAMPV_DESCRIPCION
        {
            set { _CAMPV_DESCRIPCION = value; }
            get { return _CAMPV_DESCRIPCION; }
        }

        // T12618 - SMS Corporativo - INICIO
        public string EMAIL_AUTORIZADO
        {
            set { _EMAIL_AUTORIZADO = value; }
            get { return _EMAIL_AUTORIZADO; }
        }
        // T12618 - SMS Corporativo - FIN

        public double SOLIN_LINEA_CREDITO_CALC //-- E75810 28/01/2011
        {
            get { return _SOLIN_LINEA_CREDITO_CALC; }
            set { _SOLIN_LINEA_CREDITO_CALC = value; }
        }

        //maryta
        public string DEPAV_DESCRIPCION
        {
            get { return _DEPAV_DESCRIPCION; }
            set { _DEPAV_DESCRIPCION = value; }
        }
        public string PROVV_DESCRIPCION
        {
            get { return _PROVV_DESCRIPCION; }
            set { _PROVV_DESCRIPCION = value; }
        }
        public string DISTV_DESCRIPCION
        {
            get { return _DISTV_DESCRIPCION; }
            set { _DISTV_DESCRIPCION = value; }
        }
        public string DEPAV_DESCRIPCION_FAC
        {
            get { return _DEPAV_DESCRIPCION_FAC; }
            set { _DEPAV_DESCRIPCION_FAC = value; }
        }
        public string PROVV_DESCRIPCION_FAC
        {
            get { return _PROVV_DESCRIPCION_FAC; }
            set { _PROVV_DESCRIPCION_FAC = value; }
        }
        public string DISTV_DESCRIPCION_FAC
        {
            get { return _DISTV_DESCRIPCION_FAC; }
            set { _DISTV_DESCRIPCION_FAC = value; }
        }
        public string CLIEV_CORREO
        {
            get { return _CLIEV_CORREO; }
            set { _CLIEV_CORREO = value; }
        }
        public string DPCHV_DESCRIPCION
        {
            get { return _DPCHV_DESCRIPCION; }
            set { _DPCHV_DESCRIPCION = value; }
        }
        //fin maryta

        public string CLIEV_PRE_TEL_LEG
        {
            get { return _CLIEV_PRE_TEL_LEG; }
            set { _CLIEV_PRE_TEL_LEG = value; }
        }

        public Int64 DPCHN_CODIGO
        {
            get { return _DPCHN_CODIGO; }
            set { _DPCHN_CODIGO = value; }
        }

        public string RGLPC_PODERES
        {
            get { return _RGLPC_PODERES; }
            set { _RGLPC_PODERES = value; }
        }

        public Int64 TOPEN_CODIGO
        {
            get { return _TOPEN_CODIGO; }
            set { _TOPEN_CODIGO = value; }
        }

        public string SOLIC_TIP_CAR_FIJ
        {
            get { return _SOLIC_TIP_CAR_FIJ; }
            set { _SOLIC_TIP_CAR_FIJ = value; }
        }

        public double SOLIN_IMP_DG
        {
            get { return _SOLIN_IMP_DG; }
            set { _SOLIN_IMP_DG = value; }
        }

        public string TPREC_DESCRIPCION
        {
            get { return _TPREC_DESCRIPCION; }
            set { _TPREC_DESCRIPCION = value; }
        }

        public string TPREC_CODIGO
        {
            get { return _TPREC_CODIGO; }
            set { _TPREC_CODIGO = value; }
        }

        public string SOLIN_USU_VEN
        {
            get { return _SOLIN_USU_VEN; }
            set { _SOLIN_USU_VEN = value; }
        }
        public string CLIEV_CODIGO_SAP
        {
            get { return _CLIEV_CODIGO_SAP; }
            set { _CLIEV_CODIGO_SAP = value; }
        }

        public double SOLIN_SUM_CAR_CON
        {
            get { return _SOLIN_SUM_CAR_CON; }
            set { _SOLIN_SUM_CAR_CON = value; }
        }

        public string SOLIV_NUM_CON
        {
            get { return _SOLIV_NUM_CON; }
            set { _SOLIV_NUM_CON = value; }
        }
        public string RIESGO
        {
            get { return _RIESGO; }
            set { _RIESGO = value; }
        }
        public string PRDC_CODIGO
        {
            get { return _PRDC_CODIGO; }
            set { _PRDC_CODIGO = value; }
        }
        public string PRDV_DESCRIPCION
        {
            get { return _PRDV_DESCRIPCION; }
            set { _PRDV_DESCRIPCION = value; }
        }
        public string CLIEV_CALIFICACION_PAGO
        {
            get { return _CLIEV_CALIFICACION_PAGO; }
            set { _CLIEV_CALIFICACION_PAGO = value; }
        }
        public string BURO_DESCRIPCION
        {
            get { return _BURO_DESCRIPCION; }
            set { _BURO_DESCRIPCION = value; }
        }
        public int BURO_CODIGO
        {
            get { return _BURO_CODIGO; }
            set { _BURO_CODIGO = value; }
        }
        public Int64 SOLIN_GRUPO_SEC
        {
            get { return _SOLIN_GRUPO_SEC; }
            set { _SOLIN_GRUPO_SEC = value; }
        }
        public double SOLIN_IMP_DG_GRUPO_SEC
        {
            get { return _SOLIN_IMP_DG_GRUPO_SEC; }
            set { _SOLIN_IMP_DG_GRUPO_SEC = value; }
        }
        public double SOLIN_CF_GRUPO_SEC
        {
            get { return _SOLIN_CF_GRUPO_SEC; }
            set { _SOLIN_CF_GRUPO_SEC = value; }
        }
        public double CLIEN_MONTO_VENCIDO
        {
            get { return _CLIEN_MONTO_VENCIDO; }
            set { _CLIEN_MONTO_VENCIDO = value; }
        }
        public string CLIEV_RIESGO_CLARO
        {
            get { return _CLIEV_RIESGO_CLARO; }
            set { _CLIEV_RIESGO_CLARO = value; }
        }
        public string CLIEV_COMPORTA_PAGO
        {
            get { return _CLIEV_COMPORTA_PAGO; }
            set { _CLIEV_COMPORTA_PAGO = value; }
        }
        public string CLIEC_FLAG_EXONERAR_RA
        {
            get { return _CLIEC_FLAG_EXONERAR_RA; }
            set { _CLIEC_FLAG_EXONERAR_RA = value; }
        }
        public string SOLIC_DEUDA_CLIENTE { get; set; }//PROY-29121
        public string ID_MODALIDAD_VENTA { get; set; } //PROY-140223 IDEA-140462
    }
}
