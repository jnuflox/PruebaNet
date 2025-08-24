using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEDataCreditoOUT
    {
        public BEDataCreditoOUT() {
        }

        public string FECHANACIMIENTO { get; set; }
        public double LIMITECREDITOCLARO { get; set; }
        public double LIMITECREDITOBASEEXTERNA { get; set; }
        public double LIMITECREDITODATACREDITO { get; set; }
        public double PUNTAJE { get; set; }
        public string NUMERO_ENTIDADES_RANKIN_OPERATIVO { get; set; }
        public string SCORE_RANKIN_OPERATIVO { get; set; }
        public string ANALISIS { get; set; }
        public string RAZONES { get; set; }
        public string NOMBRES { get; set; }
        public string APELLIDO_MATERNO { get; set; }
        public string APELLIDO_PATERNO { get; set; }
        public string NUMERO_DOCUMENTO { get; set; }
        public int ANTIGUEDAD_LABORAL { get; set; }
        public string TOP_10000 { get; set; }
        public string TIPO_DE_TARJETA { get; set; }
        public string TIPO_DE_CLIENTE { get; set; }
        public double INGRESO_O_LC { get; set; }
        public int EDAD { get; set; }
        public double LINEA_DE_CREDITO_EN_EL_SISTEMA { get; set; }
        public string TIPO_DE_PRODUCTO { get; set; }
        public string CREDIT_SCORE { get; set; }
        public int SCORE { get; set; }
        public string EXPLICACION { get; set; }
        public double NV_TOTAL_CARGOS_FIJOS { get; set; }
        public double NV_LC_MAXIMO { get; set; }
        public double LC_DISPONIBLE { get; set; }
        public string CLASE_DE_CLIENTE { get; set; }
        public double LIMITE_DE_CREDITO { get; set; }
        public string DIRECCIONES { get; set; }
        public string ACCION { get; set; }
        public string REGSCALIFICACION { get; set; }
        public string CODIGOMODELO { get; set; }
        public string NUMEROOPERACION { get; set; }
        public string RESPUESTA { get; set; }
        public string FECHACONSULTA { get; set; }
        public string ESTADOCIVIL { get; set; }
        public int CODIGOBURO { get; set; }
        public string FUENTECONSULTA { get; set; }
        //gaa20170215
        public string BUROCONSULTADO { get; set; }
        //gaa20170215
        public enum TIPO_CLIENTE
        {
            SUNAT = 1,
            ESSALUD = 2
        }

        public enum FUENTE_CONSULTA
        {
            BURO = 1,
            REPOSITORIO = 2
        }

        //PROY-24740
        public String toString()
        {
            //gaa20170215
            //return string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15};{16};{17};{18};{19};{20};{21};{22};{23};{24};{25};{26};{27};{28};{29};{30};{31};{32};{33};{34};{35};{36};{37}",
            return string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15};{16};{17};{18};{19};{20};{21};{22};{23};{24};{25};{26};{27};{28};{29};{30};{31};{32};{33};{34};{35};{36};{37};{38}",
            //fin gaa20170215
                                    FECHANACIMIENTO,
                                    LIMITECREDITOCLARO,
                                    LIMITECREDITOBASEEXTERNA,
                                    LIMITECREDITODATACREDITO,
                                    PUNTAJE,
                                    NUMERO_ENTIDADES_RANKIN_OPERATIVO,
                                    SCORE_RANKIN_OPERATIVO,
                                    ANALISIS,
                                    RAZONES,
                                    NOMBRES,
                                    APELLIDO_MATERNO,
                                    APELLIDO_PATERNO,
                                    NUMERO_DOCUMENTO,
                                    ANTIGUEDAD_LABORAL,
                                    TOP_10000,
                                    TIPO_DE_TARJETA,
                                    TIPO_DE_CLIENTE,
                                    INGRESO_O_LC,
                                    EDAD,
                                    LINEA_DE_CREDITO_EN_EL_SISTEMA,
                                    TIPO_DE_PRODUCTO,
                                    CREDIT_SCORE,
                                    SCORE,
                                    EXPLICACION,
                                    NV_TOTAL_CARGOS_FIJOS,
                                    NV_LC_MAXIMO,
                                    LC_DISPONIBLE,
                                    CLASE_DE_CLIENTE,
                                    LIMITE_DE_CREDITO,
                                    DIRECCIONES,
                                    ACCION,
                                    REGSCALIFICACION,
                                    CODIGOMODELO,
                                    NUMEROOPERACION,
                                    RESPUESTA,
                                    FECHACONSULTA,
                                    ESTADOCIVIL,
                                    CODIGOBURO
                                    //gaa20170215
                                    ,BUROCONSULTADO
                                    //fin gaa20170215
                                    );
        }
    }
}
