using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BENumeroPortabilidad
    {
        public Int64 SOLIN_CODIGO { get; set; }
        public string PORT_NUM_DOC { get; set; }
        public string PLANC_CODIGO { get; set; }
        public string PORT_NUMERO { get; set; }
        public string FLAG_ESTADO { get; set; }
        public string TPROC_CODIGO { get; set; }
        public string PORT_USU_CREA { get; set; }
        public string MENSAJE_ERROR { get; set; }
        public string ESTADO { get; set; }
        public string PREFIJO { get; set; }
        public string DESC_ESTADO { get; set; }
        public string PORT_MODALIDAD { get; set; }
        public Int64 SOPLN_CODIGO { get; set; }
        public Int64 SOLIN_GRUPO_SEC { get; set; }
        public string PORT_TIPO_SERVICIO { get; set; }
        public string PORT_TIPO_PLAN { get; set; }
        //INI: PROY-140223 IDEA-140462
        public Int32 PORT_CANTIDAD_NUM { get; set; }
        public string PORT_OPERADORCEDENTE { get; set; }
        public string PORT_TIPO_DOCUMENTO { get; set; }
        public string  USUARIO_CREA  { get; set; }
        public string USUARIO_MODIF { get; set; }
        public string INICIO_RANGO { get; set; }
        public string FINAL_RANGO { get; set; }
        public string NOM_RASO_ABONAD { get; set; }
        //FIN: PROY-140223 IDEA-140462
        public enum TIPO_SERVICIO
        {
            MOVIL = 1,
            FIJO = 2
        }

        public enum TIPO_PLAN_HFC
        {
            TFI = 1,
            HFC = 2,
            WIMAX = 3,
            ANALOGICO = 4,
            PRIMARIO = 5,
            ESPECIAL = 6,
			LTE = 7
        }
    }
}
