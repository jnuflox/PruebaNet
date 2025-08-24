using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BECasoEspecial
    {
        public string TCESC_CODIGO { get; set; }
        public string TCESC_DESCRIPCION { get; set; }
        public string FLAG_WHITELIST { get; set; }
        public string TOFC_CODIGO { get; set; }
        public int TCEN_MAX_PLANES { get; set; }
        public int TCEN_MAX_PLAN_VOZ { get; set; }
        public int TCEN_MAX_PLAN_DATOS { get; set; }
        public string TCESC_DESCRIPCION2 { get; set; }

        public double CARGO_FIJO_MAX { get; set; }
        public string LISTA_PRODUCTOS { get; set; }
        public string PLANES_BSCS { get; set; }
        public string PLANES_SISACT { get; set; }
        public string PLANES_X_PRODUCTO { get; set; }
        public string NRO_PLANES_X_PRODUCTO { get; set; }

        //PROY-32129 :: INI
        public string COD_CASO_ESPECIAL_UNIV { get; set; }
        public string PREG_CASO_ESPECIAL_UNIV { get; set; }
        //PROY-32129 :: FIN
//PROY-140245
        public string COD_CASO_ESPECIAL_COLAB { get;set;}
        public string MSJ_CASO_ESPECIAL_COLAB_NO_ENCONTRADO { get; set; }
        public string MSJ_CASO_ESPECIAL_COLAB_AUTOGESTION { get; set; }
        public string MSJ_CASO_ESPECIAL_COLAB_VALID_CANT_PROD { get; set; }
        public string CANT_MAX_POR_PROD_CASO_ESP_COLAB { get; set; }
        //FIN PROY-140245

        //IDEA-142010 INI
        public string BENEFICIOADICIONAL { get; set; }
        //IDEA-142010 FIN
        
       
    }
}
