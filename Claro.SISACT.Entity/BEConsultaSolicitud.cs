using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEConsultaSolicitud
    {
        private string _SOLIN_CODIGO;
        private string _TPROV_DESCRIPCION;
        private string _SEGMV_DESCRIPCION;
        private string _OVENV_DESCRIPCION;
        private string _ESTAV_DESCRIPCION;
        private string _TVENV_DESCRIPCION;
        private DateTime _SOLID_FEC_REG;
        private DateTime _SOLID_FEC_APR;
        private string _CLIEV_NOMBRE;
        private string _CLIEV_APE_PAT;
        private string _CLIEV_APE_MAT;
        private string _CLIEV_RAZ_SOC;
        private string _TDOCC_CODIGO;
        private string _CLIEC_NUM_DOC;
        private string _ESTAC_CODIGO;
        private string _MRECC_CODIGO;
        private string _MRECV_DESCRIPCION;
        private string _SOLIC_FLA_TER;
        private string _TEVAC_CODIGO;
        private string _TACTC_CODIGO;
        private string _TACTV_DESCRIPCION;
        private double _CANTIDAD_CARGOS_FIJOS;
        private string _TCARV_DESCRIPCION;
        private string _SOLIN_IMP_DG;
        private string _RDIRC_CODIGO;
        private string _RDIRV_DESCRIPCION;
        private string _MRDIV_CAD_CODIGO;
        private string _TIPO_GARANTIA_DES;
        private string _SOLIC_TIPO_EVALUACION;
        private string _TDOCV_DESCRIPCION;
        private string _TCESC_DESCRIPCION;
        private string _SOLIV_MOTIVO_RECHAZO;
        private string _FLAG_PORTABILIDAD;
        private string _SOLIC_COD_APROB;
        #region "TS_2009"
        private string _SOLID_FEC_APR_STR;
        #endregion
        //INICIO - E75688
        private string _CLIEV_FLAG_CORREO;
        private string _CLIEV_CORREO;
        private string _CLIEV_TEL_SMS;
        private DateTime _CLIED_FEC_NAC;
        private string _CLIEV_EST_CIV;
        private string _TITULO_PERSONA_COD;
        //FIN - E75688
        private string _DEPAC_CODIGO;
        private string _TPROD_COMERCIALIZAR = "";
        private string _PRDV_DESCRIPCION;
        private string _PRDC_CODIGO;
        private double _SOLIN_SUM_CAR_CON;

        public BEConsultaSolicitud()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public string SOLIN_CODIGO
        {
            get { return _SOLIN_CODIGO; }
            set { _SOLIN_CODIGO = value; }
        }
        public string TPROV_DESCRIPCION
        {
            get { return _TPROV_DESCRIPCION; }
            set { _TPROV_DESCRIPCION = value; }
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
        public string ESTAV_DESCRIPCION
        {
            get { return _ESTAV_DESCRIPCION; }
            set { _ESTAV_DESCRIPCION = value; }
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
        public DateTime SOLID_FEC_APR
        {
            get { return _SOLID_FEC_APR; }
            set { _SOLID_FEC_APR = value; }
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
        public string CLIEC_NUM_DOC
        {
            get { return _CLIEC_NUM_DOC; }
            set { _CLIEC_NUM_DOC = value; }
        }
        public string ESTAC_CODIGO
        {
            get { return _ESTAC_CODIGO; }
            set { _ESTAC_CODIGO = value; }
        }
        public string MRECC_CODIGO
        {
            get { return _MRECC_CODIGO; }
            set { _MRECC_CODIGO = value; }
        }
        public string MRECV_DESCRIPCION
        {
            get { return _MRECV_DESCRIPCION; }
            set { _MRECV_DESCRIPCION = value; }
        }
        public string SOLIC_FLA_TER
        {
            get { return _SOLIC_FLA_TER; }
            set { _SOLIC_FLA_TER = value; }
        }
        public string TEVAC_CODIGO
        {
            get { return _TEVAC_CODIGO; }
            set { _TEVAC_CODIGO = value; }
        }
        public string TACTC_CODIGO
        {
            get { return _TACTC_CODIGO; }
            set { _TACTC_CODIGO = value; }
        }
        public string TACTV_DESCRIPCION
        {
            get { return _TACTV_DESCRIPCION; }
            set { _TACTV_DESCRIPCION = value; }
        }
        public string TCARV_DESCRIPCION
        {
            get { return _TCARV_DESCRIPCION; }
            set { _TCARV_DESCRIPCION = value; }
        }
        public string SOLIN_IMP_DG
        {
            get { return _SOLIN_IMP_DG; }
            set { _SOLIN_IMP_DG = value; }
        }
        public double CANTIDAD_CARGOS_FIJOS
        {
            get { return _CANTIDAD_CARGOS_FIJOS; }
            set { _CANTIDAD_CARGOS_FIJOS = value; }
        }
        public string RDIRC_CODIGO
        {
            get { return _RDIRC_CODIGO; }
            set { _RDIRC_CODIGO = value; }
        }
        public string RDIRV_DESCRIPCION
        {
            get { return _RDIRV_DESCRIPCION; }
            set { _RDIRV_DESCRIPCION = value; }
        }
        public string MRDIV_CAD_CODIGO
        {
            get { return _MRDIV_CAD_CODIGO; }
            set { _MRDIV_CAD_CODIGO = value; }
        }
        public string TIPO_GARANTIA_DES
        {
            get { return _TIPO_GARANTIA_DES; }
            set { _TIPO_GARANTIA_DES = value; }
        }


        public string TDOCV_DESCRIPCION
        {
            get { return _TDOCV_DESCRIPCION; }
            set { _TDOCV_DESCRIPCION = value; }
        }

        public string SOLIC_TIPO_EVALUACION
        {
            get { return _SOLIC_TIPO_EVALUACION; }
            set { _SOLIC_TIPO_EVALUACION = value; }
        }
        public string TCESC_DESCRIPCION
        {
            get { return _TCESC_DESCRIPCION; }
            set { _TCESC_DESCRIPCION = value; }
        }
        public string SOLIV_MOTIVO_RECHAZO
        {
            get { return _SOLIV_MOTIVO_RECHAZO; }
            set { _SOLIV_MOTIVO_RECHAZO = value; }
        }
        public string SOLIC_COD_APROB
        {
            get { return _SOLIC_COD_APROB; }
            set { _SOLIC_COD_APROB = value; }
        }

        #region "TS-2009"
        public string SOLID_FEC_APR_STR
        {
            get { return _SOLID_FEC_APR_STR; }
            set { _SOLID_FEC_APR_STR = value; }
        }
        #endregion
        public string FLAG_PORTABILIDAD
        {
            get { return _FLAG_PORTABILIDAD; }
            set { _FLAG_PORTABILIDAD = value; }
        }

        //INICIO - E75688
        public string CLIEV_FLAG_CORREO
        {
            get { return _CLIEV_FLAG_CORREO; }
            set { _CLIEV_FLAG_CORREO = value; }
        }
        public string CLIEV_CORREO
        {
            get { return _CLIEV_CORREO; }
            set { _CLIEV_CORREO = value; }
        }
        public string CLIEV_TEL_SMS
        {
            get { return _CLIEV_TEL_SMS; }
            set { _CLIEV_TEL_SMS = value; }
        }

        public DateTime CLIED_FEC_NAC
        {
            get { return _CLIED_FEC_NAC; }
            set { _CLIED_FEC_NAC = value; }
        }
        public string CLIEV_EST_CIV
        {
            get { return _CLIEV_EST_CIV; }
            set { _CLIEV_EST_CIV = value; }
        }
        public string TITULO_PERSONA_COD { set { _TITULO_PERSONA_COD = value; } get { return _TITULO_PERSONA_COD; } }
        //FIN - E75688

        public string DEPAC_CODIGO
        {
            get { return _DEPAC_CODIGO; }
            set { _DEPAC_CODIGO = value; }
        }
        public string TPROD_COMERCIALIZAR
        {
            get { return _TPROD_COMERCIALIZAR; }
            set { _TPROD_COMERCIALIZAR = value; }
        }
        public string PRDV_DESCRIPCION
        {
            get { return _PRDV_DESCRIPCION; }
            set { _PRDV_DESCRIPCION = value; }
        }
        public string PRDC_CODIGO
        {
            get { return _PRDC_CODIGO; }
            set { _PRDC_CODIGO = value; }
        }
        public double SOLIN_SUM_CAR_CON
        {
            get { return _SOLIN_SUM_CAR_CON; }
            set { _SOLIN_SUM_CAR_CON = value; }
        }

        //PROY-26963-F3 - GPRD
        public string TCESC_CODIGO { get; set; }
        public string NOMBRE_COMPLETO { get; set; }
        public string OVENC_CODIGO { get; set; }
        public string ESTADO_VENTA { get; set; }
        public string PORT_NUMERO { get; set; }
        public string TPROC_CODIGO { get; set; }
        public string PORT_OPER_CED { get; set; }
        public int TOPEN_CODIGO { get; set; }
        public string RESPUESTA { get; set; }
        public string MODALIDAD_VENTA { get; set; }
        //PROY-26963-F3 - GPRD
    }
}
