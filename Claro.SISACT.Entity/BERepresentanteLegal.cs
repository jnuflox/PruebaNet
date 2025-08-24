using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BERepresentanteLegal
    {
        public Int64 APODN_CODIGO { get; set; }
        public Int64 SOLIN_CODIGO { get; set; }
        public Int64 P_SCPN_NRO_PEDIDO { get; set; } //PROY-25335
        public string CLIEC_NUM_DOC { get; set; }
        public string APODV_NOM_REP_LEG { get; set; }
        public string APODV_APA_REP_LEG { get; set; }
        public string APODV_AMA_REP_LEG { get; set; }
        public string APODC_TIP_DOC_REP { get; set; }
        public string APODV_NUM_DOC_REP { get; set; }
        public string APODV_CAR_REP { get; set; }
        public string APODC_ESTADO { get; set; }
        public string TDOCV_DESCRIPCION_REP { get; set; }
        public string TPODC_CODIGO { get; set; }
        public string SPODC_CODIGO { get; set; }
        public string E_MAIL { get; set; }
        public string SRLC_CODNACIONALIDAD { get; set; } //PROY-31636
        public string SRLV_DESCNACIONALIDAD { get; set; } //PROY-31636
        //PROY-25335 INI RIHU
        public string P_SRLV_ID_TX_P { get; set; }
        public string P_SCPV_OBSERVACION { get; set; }
        public string P_SCPV_APLICACION { get; set; }
        public string P_SCPV_USUARIO_CREA { get; set; }
        //PROY-25335 FIN RIHU
        /*INI PROY-32438*/
        public string APODD_FECHA_NOMBRAMIENTO { get; set; }
        public string APODI_CANT_MESES_NOMBRAMIENTO { get; set; }
        /*FIN PROY-32438*/

    }
}
