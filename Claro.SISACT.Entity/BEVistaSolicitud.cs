using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEVistaSolicitud
    {
        private Int64 _solin_codigo;
        private string _solic_fla_ter;
        private string _cliev_pre_tel_leg;
        private string _cliev_tel_leg;
        private string _cliev_pre_dir;
        private string _cliev_direccion;
        private string _cliev_ref_dir;
        private string _cliec_cod_dep_dir;
        private string _cliec_cod_pro_dir;
        private string _cliec_cod_dis_dir;
        private string _cliec_cod_pos_dir;
        private string _cliev_pre_dir_fac;
        private string _cliev_dir_fac;
        private string _cliev_ref_dir_fac;
        private string _cliec_cod_dep_fac;
        private string _cliec_cod_pro_fac;
        private string _cliec_cod_dis_fac;
        private string _cliec_cod_pos_fac;
        private string _cliev_pre_dir_tra;
        private string _cliev_dir_tra;
        private string _cliev_ref_dir_tra;
        private string _cliec_cod_dep_tra;
        private string _cliec_cod_pro_tra;
        private string _cliec_cod_dis_tra;
        private string _cliec_cod_pos_tra;
        private string _cliev_tel_ref_1;
        private string _cliev_tel_ref_2;
        private string _cliev_tel_fij_tra;
        private string _solin_lim_cre_fin;
        private string _solic_sco_txt_fin;
        private string _solin_sco_num_fin;
        private string _solin_lim_max_fin;
        private string _solic_postcontrol;
        private string _rdirc_codigo;
        private string _rfinc_codigo;
        private string _mrecc_codigo;
        private Double _solin_imp_dg_man;
        private string _solic_tip_car_man;
        private string _soliv_com_dg;
        private string _tevac_codigo;
        private string _estac_codigo;
        private string _soliv_des_est;
        private string _solic_usu_cre;
        private string _tafic_codigo;
        private string _solic_cod_tar;
        private string _solic_cod_tip_mon;
        private string _soliv_num_tar;
        private string _soliv_nom_tit_tar;
        private string _soliv_doc_tit_tar;
        private string _soliv_fec_ven_tar;
        private string _solin_mntomax;
        private string _soliv_num_ope_fin;
        private string _topen_codigo;
        private string _rucempleador;
        private string _nombreempresa;
        //private DateTime _SOLID_FEC_REG;
        /*Extras*/
        private string _CLIEC_NUM_DOC;
        private string _SOLIC_EVA_ESS;
        private string _SOLIC_EVA_SUN;
        private string _SOLIC_COD_RES_DIR;
        private string _SOLIC_DES_RES_DIR;
        private string _SOLIC_TIP_CAR_FIJ;
        private string _SOLIN_IMP_DG;
        private string _SOLIV_RES_EXP_CON;
        private string _SOLIV_NUM_OPE_CON;
        private string _SOLIN_LIM_CRE_CON;
        private string _SOLIN_SUM_CAR_CON;
        private string _SOLIN_NUM_CAR_FIJ;
        private string _SOLIV_DES_RES_FIN;
        private string _SOLIC_SCO_TXT_CON;
        private string _SOLIN_SCO_NUM_CON;
        private string _TDOCV_DESCRIPCION;
        private DateTime _SOLID_FEC_APR;
        private DateTime _solid_fec_reg;
        private string _CLIEV_NOMBRE;
        private string _CLIEV_APE_PAT;
        private string _CLIEV_APE_MAT;

        private string _SOLIV_DES_OFI_VEN;
        private int _SOLIN_CAN_LIN;
        private string _TCARV_DESCRIPCION;
        private string _TACTC_CODIGO;
        private string _SOLIV_MOTIVO_RECHAZO;
        private string _SOLIC_COD_APROB;

        //E75606 (inicio EXPRESS)
        private string _CLIEV_RAZ_SOC;
        private string _FLAG_PORTABILIDAD;
        private Int64 _PORT_OPER_CED;
        private string _PORT_ESTADO;
        private string _PARAV_DESCRIPCION;
        private string _PORT_TELEF_CONT;
        private string _PORT_FLAG_REC_OPE_CED;
        private string _PORT_CARGO_FIJO_OPE_CED;
        private string _PORT_COM_MP;
        private ArrayList _SOLICITUDES_PORTABILIDAD;
        private ArrayList _ARCHIVOS_PORTABILIDAD;

        //private DateTime _SOLID_FEC_REG; //Fecha Registro
        private string _PACUC_CODIGO; //Codigo Plazo Acuerdo
        private string _PACUV_DESCRIPCION; //Descripcion Plazo Acuerdo
        private string _FPAGV_DESCRIPCION; //Forma de Pago
        private string _TACTV_DESCRIPCION; //Tipo Activacion
        private string _TPROV_DESCRIPCION; //Tipo Producto
        private string _TVENV_DESCRIPCION; //Tipo Venta
        private string _OVENV_DESCRIPCION; //Oficina de Venta
        private string _TOFIV_DESCRIPCION; //Canal
        private string _TCESC_DESCRIPCION; //Caso especial
        private string _SOLIC_EXI_BSC_FIN; //Nuevo o Recurrente
        private string _RDIRV_DESCRIPCION; //Verificacion Datos
        private string _OPE_DESCRIPCION; //Descripcion Operador
        private string _TDOCC_CODIGO; //Codigo de Tipo Documento
        private string _TPROC_CODIGO; //Codigo de Tipo Producto
        //E75606 (fin EXPRESS)

        // E75606 - Cliente RUC
        private string _TCLIC_CODIGO;
        // E75606 - Cliente RUC

        //T13087 - Reporte Portabilidad
        private string _ESTAV_DESCRIPCION; //Descripcion estado sec
        private string _PORT_FEC_REG; //Fecha registro porta
        private string _ACPOT_FECHA_PROGRAMA; //Fecha programacion
        private string _PORT_NUMERO; //Numero Portabilidad
        private string _SOPOC_ICCID; //ICCID
        private string _SOPOC_IMEI; //IMEI
        private string _SOPOC_NROPEDIDO; //Numero pedido
        private string _SOPOV_NUM_DOCUMENTO; //Numero documento identidad
        private string _ACPOC_TIPO_DOC_VENTA; //Tipo documento venta
        private string _ACPOV_NUM_DOC_VENTA; //Numero documento venta
        private string _SOPOC_CONPAGO; //Pagado
        private string _CANAL_OFI; //Canal de Oficina de Ventas
        private string _PLANV_DESCRIPCION; //Descripcion Plan
        //T13087 - Reporte Portabilidad

        //T23398 - JULIO CHAVEZ - MANUEL LITO - Reporte Portabilidad
        private string _SOPOV_MOTIVO_DESC;
        //FIN T23398 - JULIO CHAVEZ - MANUEL LITO - Reporte Portabilidad

        //INICIO - E75688
        private string _CLIEV_FLAG_CORREO; //Canal de Oficina de Ventas
        private string _CLIEV_CORREO; //Descripcion Plan
        private DateTime _CLIED_FEC_NAC;
        private string _CLIEV_EST_CIV;
        private string _TITULO_PERSONA_COD;
        //FIN

        // Cosapisoft E75686 (Inicio)

        private string _TPROD_COMERCIALIZAR; //JAR -DTH

        private string _pdv;
        private string _usuario;

        public string pdv
        {
            get { return _pdv; }
            set { _pdv = value; }
        }
        public string usuario
        {
            get { return _usuario; }
            set { _usuario = value; }
        }
        // Cosapisoft E75686 (Fin)
        public BEVistaSolicitud()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        //********EXTRAS**************

        public DateTime solid_fec_reg
        {
            get { return _solid_fec_reg; }
            set { _solid_fec_reg = value; }
        }


        public string SOLIC_COD_APROB
        {
            get { return _SOLIC_COD_APROB; }
            set { _SOLIC_COD_APROB = value; }
        }

        public string SOLIV_MOTIVO_RECHAZO
        {
            get { return _SOLIV_MOTIVO_RECHAZO; }
            set { _SOLIV_MOTIVO_RECHAZO = value; }
        }

        public string TACTC_CODIGO
        {
            get { return _TACTC_CODIGO; }
            set { _TACTC_CODIGO = value; }
        }

        public string TCARV_DESCRIPCION
        {
            get { return _TCARV_DESCRIPCION; }
            set { _TCARV_DESCRIPCION = value; }
        }

        public int SOLIN_CAN_LIN
        {
            get { return _SOLIN_CAN_LIN; }
            set { _SOLIN_CAN_LIN = value; }
        }

        public string SOLIV_DES_OFI_VEN
        {
            get { return _SOLIV_DES_OFI_VEN; }
            set { _SOLIV_DES_OFI_VEN = value; }
        }

        public string CLIEV_APE_MAT
        {
            get { return _CLIEV_APE_MAT; }
            set { _CLIEV_APE_MAT = value; }
        }

        public string CLIEV_APE_PAT
        {
            get { return _CLIEV_APE_PAT; }
            set { _CLIEV_APE_PAT = value; }
        }

        public string CLIEV_NOMBRE
        {
            get { return _CLIEV_NOMBRE; }
            set { _CLIEV_NOMBRE = value; }
        }

        public DateTime SOLID_FEC_APR
        {
            get { return _SOLID_FEC_APR; }
            set { _SOLID_FEC_APR = value; }
        }

        public string TDOCV_DESCRIPCION
        {
            get { return _TDOCV_DESCRIPCION; }
            set { _TDOCV_DESCRIPCION = value; }
        }

        public string CLIEC_NUM_DOC
        {
            get { return _CLIEC_NUM_DOC; }
            set { _CLIEC_NUM_DOC = value; }
        }

        public string SOLIC_EVA_ESS
        {
            get { return _SOLIC_EVA_ESS; }
            set { _SOLIC_EVA_ESS = value; }
        }

        public string SOLIC_EVA_SUN
        {
            get { return _SOLIC_EVA_SUN; }
            set { _SOLIC_EVA_SUN = value; }
        }

        public string SOLIC_COD_RES_DIR
        {
            get { return _SOLIC_COD_RES_DIR; }
            set { _SOLIC_COD_RES_DIR = value; }
        }

        public string SOLIC_DES_RES_DIR
        {
            get { return _SOLIC_DES_RES_DIR; }
            set { _SOLIC_DES_RES_DIR = value; }
        }

        public string SOLIC_TIP_CAR_FIJ
        {
            get { return _SOLIC_TIP_CAR_FIJ; }
            set { _SOLIC_TIP_CAR_FIJ = value; }
        }

        public string SOLIN_IMP_DG
        {
            get { return _SOLIN_IMP_DG; }
            set { _SOLIN_IMP_DG = value; }
        }

        public string SOLIV_RES_EXP_CON
        {
            get { return _SOLIV_RES_EXP_CON; }
            set { _SOLIV_RES_EXP_CON = value; }
        }

        public string SOLIV_NUM_OPE_CON
        {
            get { return _SOLIV_NUM_OPE_CON; }
            set { _SOLIV_NUM_OPE_CON = value; }
        }

        public string SOLIN_LIM_CRE_CON
        {
            get { return _SOLIN_LIM_CRE_CON; }
            set { _SOLIN_LIM_CRE_CON = value; }
        }

        public string SOLIN_SUM_CAR_CON
        {
            get { return _SOLIN_SUM_CAR_CON; }
            set { _SOLIN_SUM_CAR_CON = value; }
        }

        public string SOLIN_NUM_CAR_FIJ
        {
            get { return _SOLIN_NUM_CAR_FIJ; }
            set { _SOLIN_NUM_CAR_FIJ = value; }
        }

        public string SOLIV_DES_RES_FIN
        {
            get { return _SOLIV_DES_RES_FIN; }
            set { _SOLIV_DES_RES_FIN = value; }
        }

        public string SOLIC_SCO_TXT_CON
        {
            get { return _SOLIC_SCO_TXT_CON; }
            set { _SOLIC_SCO_TXT_CON = value; }
        }

        public string SOLIN_SCO_NUM_CON
        {
            get { return _SOLIN_SCO_NUM_CON; }
            set { _SOLIN_SCO_NUM_CON = value; }
        }

        //****************************


        public Int64 solin_codigo
        {
            get { return _solin_codigo; }
            set { _solin_codigo = value; }
        }

        public string solic_fla_ter
        {
            get { return _solic_fla_ter; }
            set { _solic_fla_ter = value; }
        }

        public string cliev_ref_dir_tra
        {
            get { return _cliev_ref_dir_tra; }
            set { _cliev_ref_dir_tra = value; }
        }

        public string cliev_pre_tel_leg
        {
            get { return _cliev_pre_tel_leg; }
            set { _cliev_pre_tel_leg = value; }
        }

        public string cliev_tel_leg
        {
            get { return _cliev_tel_leg; }
            set { _cliev_tel_leg = value; }
        }

        public string cliev_pre_dir
        {
            get { return _cliev_pre_dir; }
            set { _cliev_pre_dir = value; }
        }

        public string cliev_direccion
        {
            get { return _cliev_direccion; }
            set { _cliev_direccion = value; }
        }

        public string cliev_ref_dir
        {
            get { return _cliev_ref_dir; }
            set { _cliev_ref_dir = value; }
        }

        public string cliec_cod_dep_dir
        {
            get { return _cliec_cod_dep_dir; }
            set { _cliec_cod_dep_dir = value; }
        }

        public string cliec_cod_pro_dir
        {
            get { return _cliec_cod_pro_dir; }
            set { _cliec_cod_pro_dir = value; }
        }

        public string cliec_cod_dis_dir
        {
            get { return _cliec_cod_dis_dir; }
            set { _cliec_cod_dis_dir = value; }
        }

        public string cliec_cod_pos_dir
        {
            get { return _cliec_cod_pos_dir; }
            set { _cliec_cod_pos_dir = value; }
        }

        public string cliev_pre_dir_fac
        {
            get { return _cliev_pre_dir_fac; }
            set { _cliev_pre_dir_fac = value; }
        }

        public string cliev_dir_fac
        {
            get { return _cliev_dir_fac; }
            set { _cliev_dir_fac = value; }
        }

        public string cliev_ref_dir_fac
        {
            get { return _cliev_ref_dir_fac; }
            set { _cliev_ref_dir_fac = value; }
        }

        public string cliec_cod_dep_fac
        {
            get { return _cliec_cod_dep_fac; }
            set { _cliec_cod_dep_fac = value; }
        }

        public string cliec_cod_pro_fac
        {
            get { return _cliec_cod_pro_fac; }
            set { _cliec_cod_pro_fac = value; }
        }

        public string cliec_cod_dis_fac
        {
            get { return _cliec_cod_dis_fac; }
            set { _cliec_cod_dis_fac = value; }
        }

        public string cliec_cod_pos_fac
        {
            get { return _cliec_cod_pos_fac; }
            set { _cliec_cod_pos_fac = value; }
        }

        public string cliev_pre_dir_tra
        {
            get { return _cliev_pre_dir_tra; }
            set { _cliev_pre_dir_tra = value; }
        }

        public string cliev_dir_tra
        {
            get { return _cliev_dir_tra; }
            set { _cliev_dir_tra = value; }
        }

        public string cliec_cod_dep_tra
        {
            get { return _cliec_cod_dep_tra; }
            set { _cliec_cod_dep_tra = value; }
        }

        public string cliec_cod_pro_tra
        {
            get { return _cliec_cod_pro_tra; }
            set { _cliec_cod_pro_tra = value; }
        }

        public string cliec_cod_dis_tra
        {
            get { return _cliec_cod_dis_tra; }
            set { _cliec_cod_dis_tra = value; }
        }

        public string cliec_cod_pos_tra
        {
            get { return _cliec_cod_pos_tra; }
            set { _cliec_cod_pos_tra = value; }
        }

        public string cliev_tel_ref_1
        {
            get { return _cliev_tel_ref_1; }
            set { _cliev_tel_ref_1 = value; }
        }

        public string cliev_tel_ref_2
        {
            get { return _cliev_tel_ref_2; }
            set { _cliev_tel_ref_2 = value; }
        }

        public string cliev_tel_fij_tra
        {
            get { return _cliev_tel_fij_tra; }
            set { _cliev_tel_fij_tra = value; }
        }

        public string solin_lim_cre_fin
        {
            get { return _solin_lim_cre_fin; }
            set { _solin_lim_cre_fin = value; }
        }

        public string solic_sco_txt_fin
        {
            get { return _solic_sco_txt_fin; }
            set { _solic_sco_txt_fin = value; }
        }

        public string solin_sco_num_fin
        {
            get { return _solin_sco_num_fin; }
            set { _solin_sco_num_fin = value; }
        }

        public string solin_lim_max_fin
        {
            get { return _solin_lim_max_fin; }
            set { _solin_lim_max_fin = value; }
        }

        public string solic_postcontrol
        {
            get { return _solic_postcontrol; }
            set { _solic_postcontrol = value; }
        }

        public string rdirc_codigo
        {
            get { return _rdirc_codigo; }
            set { _rdirc_codigo = value; }
        }

        public string rfinc_codigo
        {
            get { return _rfinc_codigo; }
            set { _rfinc_codigo = value; }
        }

        public string mrecc_codigo
        {
            get { return _mrecc_codigo; }
            set { _mrecc_codigo = value; }
        }

        public Double solin_imp_dg_man
        {
            get { return _solin_imp_dg_man; }
            set { _solin_imp_dg_man = value; }
        }

        public string solic_tip_car_man
        {
            get { return _solic_tip_car_man; }
            set { _solic_tip_car_man = value; }
        }

        public string soliv_com_dg
        {
            get { return _soliv_com_dg; }
            set { _soliv_com_dg = value; }
        }

        public string tevac_codigo
        {
            get { return _tevac_codigo; }
            set { _tevac_codigo = value; }
        }

        public string estac_codigo
        {
            get { return _estac_codigo; }
            set { _estac_codigo = value; }
        }

        public string soliv_des_est
        {
            get { return _soliv_des_est; }
            set { _soliv_des_est = value; }
        }

        public string solic_usu_cre
        {
            get { return _solic_usu_cre; }
            set { _solic_usu_cre = value; }
        }

        public string tafic_codigo
        {
            get { return _tafic_codigo; }
            set { _tafic_codigo = value; }
        }

        public string solic_cod_tar
        {
            get { return _solic_cod_tar; }
            set { _solic_cod_tar = value; }
        }

        public string solic_cod_tip_mon
        {
            get { return _solic_cod_tip_mon; }
            set { _solic_cod_tip_mon = value; }
        }

        public string soliv_num_tar
        {
            get { return _soliv_num_tar; }
            set { _soliv_num_tar = value; }
        }

        public string soliv_nom_tit_tar
        {
            get { return _soliv_nom_tit_tar; }
            set { _soliv_nom_tit_tar = value; }
        }

        public string soliv_doc_tit_tar
        {
            get { return _soliv_doc_tit_tar; }
            set { _soliv_doc_tit_tar = value; }
        }

        public string soliv_fec_ven_tar
        {
            get { return _soliv_fec_ven_tar; }
            set { _soliv_fec_ven_tar = value; }
        }

        public string solin_mntomax
        {
            get { return _solin_mntomax; }
            set { _solin_mntomax = value; }
        }

        public string soliv_num_ope_fin
        {
            get { return _soliv_num_ope_fin; }
            set { _soliv_num_ope_fin = value; }
        }

        public string topen_codigo
        {
            get { return _topen_codigo; }
            set { _topen_codigo = value; }
        }

        public string rucempleador
        {
            get { return _rucempleador; }
            set { _rucempleador = value; }
        }

        public string nombreempresa
        {
            get { return _nombreempresa; }
            set { _nombreempresa = value; }
        }

        //E75606 (inicio EXPRESS)
        public string PACUC_CODIGO //Codigo Plazo Acuerdo
        {
            get { return _PACUC_CODIGO; }
            set { _PACUC_CODIGO = value; }
        }
        public string PACUV_DESCRIPCION //Descripcion Plazo Acuerdo
        {
            get { return _PACUV_DESCRIPCION; }
            set { _PACUV_DESCRIPCION = value; }
        }
        public string FPAGV_DESCRIPCION //Forma de Pago
        {
            get { return _FPAGV_DESCRIPCION; }
            set { _FPAGV_DESCRIPCION = value; }
        }
        public string TACTV_DESCRIPCION //Tipo Activacion
        {
            get { return _TACTV_DESCRIPCION; }
            set { _TACTV_DESCRIPCION = value; }
        }
        public string TVENV_DESCRIPCION //Tipo Venta
        {
            get { return _TVENV_DESCRIPCION; }
            set { _TVENV_DESCRIPCION = value; }
        }
        public string TPROV_DESCRIPCION //Tipo Producto
        {
            get { return _TPROV_DESCRIPCION; }
            set { _TPROV_DESCRIPCION = value; }
        }
        public string OVENV_DESCRIPCION //Oficina de Venta
        {
            get { return _OVENV_DESCRIPCION; }
            set { _OVENV_DESCRIPCION = value; }
        }
        public string TOFIV_DESCRIPCION //Canal
        {
            get { return _TOFIV_DESCRIPCION; }
            set { _TOFIV_DESCRIPCION = value; }
        }
        public string TCESC_DESCRIPCION //Caso especial
        {
            get { return _TCESC_DESCRIPCION; }
            set { _TCESC_DESCRIPCION = value; }
        }
        public string SOLIC_EXI_BSC_FIN //Nuevo Recurrente
        {
            get { return _SOLIC_EXI_BSC_FIN; }
            set { _SOLIC_EXI_BSC_FIN = value; }
        }
        public string RDIRV_DESCRIPCION //Verificacion Datos
        {
            get { return _RDIRV_DESCRIPCION; }
            set { _RDIRV_DESCRIPCION = value; }
        }
        public string OPE_DESCRIPCION // Descripcion Operador
        {
            get { return _OPE_DESCRIPCION; }
            set { _OPE_DESCRIPCION = value; }
        }
        public string TDOCC_CODIGO // Codigo de Tipo de Documento
        {
            get { return _TDOCC_CODIGO; }
            set { _TDOCC_CODIGO = value; }
        }
        public string TPROC_CODIGO // Codigo de Tipo Producto
        {
            get { return _TPROC_CODIGO; }
            set { _TPROC_CODIGO = value; }
        }
        public string CLIEV_RAZ_SOC
        {
            get { return _CLIEV_RAZ_SOC; }
            set { _CLIEV_RAZ_SOC = value; }
        }
        public string FLAG_PORTABILIDAD
        {
            get { return _FLAG_PORTABILIDAD; }
            set { _FLAG_PORTABILIDAD = value; }
        }
        public Int64 PORT_OPER_CED
        {
            get { return _PORT_OPER_CED; }
            set { _PORT_OPER_CED = value; }
        }
        public string PORT_ESTADO
        {
            get { return _PORT_ESTADO; }
            set { _PORT_ESTADO = value; }
        }
        public string PORT_ESTADO_DESC
        {
            get { return _PARAV_DESCRIPCION; }
            set { _PARAV_DESCRIPCION = value; }
        }
        public string PORT_TELEF_CONT
        {
            get { return _PORT_TELEF_CONT; }
            set { _PORT_TELEF_CONT = value; }
        }
        public string PORT_FLAG_REC_OPE_CED
        {
            get { return _PORT_FLAG_REC_OPE_CED; }
            set { _PORT_FLAG_REC_OPE_CED = value; }
        }
        public string PORT_CARGO_FIJO_OPE_CED
        {
            get { return _PORT_CARGO_FIJO_OPE_CED; }
            set { _PORT_CARGO_FIJO_OPE_CED = value; }
        }
        public string PORT_COM_MP
        {
            get { return _PORT_COM_MP; }
            set { _PORT_COM_MP = value; }
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
        //E75606

        // E75606 - Cliente RUC
        public string TCLIC_CODIGO
        {
            get { return _TCLIC_CODIGO; }
            set { _TCLIC_CODIGO = value; }
        }
        // E75606 - Cliente RUC

        //T13087 - Reporte Portabilidad
        public string ESTAV_DESCRIPCION
        {
            get { return _ESTAV_DESCRIPCION; }
            set { _ESTAV_DESCRIPCION = value; }
        }
        public string PORT_FEC_REG
        {
            get { return _PORT_FEC_REG; }
            set { _PORT_FEC_REG = value; }
        }
        public string ACPOT_FECHA_PROGRAMA
        {
            get { return _ACPOT_FECHA_PROGRAMA; }
            set { _ACPOT_FECHA_PROGRAMA = value; }
        }
        public string PORT_NUMERO
        {
            get { return _PORT_NUMERO; }
            set { _PORT_NUMERO = value; }
        }
        public string SOPOC_ICCID
        {
            get { return _SOPOC_ICCID; }
            set { _SOPOC_ICCID = value; }
        }
        public string SOPOC_IMEI
        {
            get { return _SOPOC_IMEI; }
            set { _SOPOC_IMEI = value; }
        }
        public string SOPOC_NROPEDIDO
        {
            get { return _SOPOC_NROPEDIDO; }
            set { _SOPOC_NROPEDIDO = value; }
        }
        public string SOPOV_NUM_DOCUMENTO
        {
            get { return _SOPOV_NUM_DOCUMENTO; }
            set { _SOPOV_NUM_DOCUMENTO = value; }
        }
        public string ACPOC_TIPO_DOC_VENTA
        {
            get { return _ACPOC_TIPO_DOC_VENTA; }
            set { _ACPOC_TIPO_DOC_VENTA = value; }
        }
        public string ACPOV_NUM_DOC_VENTA
        {
            get { return _ACPOV_NUM_DOC_VENTA; }
            set { _ACPOV_NUM_DOC_VENTA = value; }
        }
        public string SOPOC_CONPAGO
        {
            get { return _SOPOC_CONPAGO; }
            set { _SOPOC_CONPAGO = value; }
        }
        public string CANAL_OFI
        {
            get { return _CANAL_OFI; }
            set { _CANAL_OFI = value; }
        }
        public string PLANV_DESCRIPCION
        {
            get { return _PLANV_DESCRIPCION; }
            set { _PLANV_DESCRIPCION = value; }
        }
        //T13087 - Reporte Portabilidad

        //T23398 - JULIO CHAVEZ - MANUEL LITO - Reporte Portabilidad

        public string SOPOV_MOTIVO_DESC
        {
            get { return _SOPOV_MOTIVO_DESC; }
            set { _SOPOV_MOTIVO_DESC = value; }
        }
        //FIN T23398 - JULIO CHAVEZ - MANUEL LITO - Reporte Portabilidad

        //INICIO - E75668
        public string CLIEV_FLAG_CORREO
        { get { return _CLIEV_FLAG_CORREO; } set { _CLIEV_FLAG_CORREO = value; } }
        public string CLIEV_CORREO
        { get { return _CLIEV_CORREO; } set { _CLIEV_CORREO = value; } }
        public DateTime CLIED_FEC_NAC
        { get { return _CLIED_FEC_NAC; } set { _CLIED_FEC_NAC = value; } }
        public string CLIEV_EST_CIV
        { get { return _CLIEV_EST_CIV; } set { _CLIEV_EST_CIV = value; } }
        public string TITULO_PERSONA_COD { set { _TITULO_PERSONA_COD = value; } get { return _TITULO_PERSONA_COD; } }
        //FIN

        public string TPROD_COMERCIALIZAR { set { _TPROD_COMERCIALIZAR = value; } get { return _TPROD_COMERCIALIZAR; } }

    }
}
