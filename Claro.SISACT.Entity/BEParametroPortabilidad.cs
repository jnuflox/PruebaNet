using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEParametroPortabilidad
    {
        public string PK_PARAT_PARAC_TP { get; set; }
        public string PK_PARAT_PARAC_COD { get; set; }
        public string DESCRIPCION { get; set; }
        public int STATUS { get; set; }
        public string REF1 { get; set; }
        public string REF2 { get; set; }
        public string REF3 { get; set; }
        public string REF4 { get; set; }
        public string REF5 { get; set; }
        public string REF6 { get; set; }
        public string REF7 { get; set; }
        public DateTime FECHA_CREACION { get; set; }
        public string USUARIO_CREA { get; set; }
    }
}
