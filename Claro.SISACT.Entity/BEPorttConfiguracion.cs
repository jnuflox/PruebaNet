using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    //PROY-31393 INI
    public class BEPorttConfiguracion
    {
        public double PORTN_CODIGO { get; set; }
        public string PORTV_EST_PROCESO { get; set; }
        public string PORTV_MOTIVO { get; set; }
        public int PORTV_FLAG_ACREDITA { get; set; }
        public string PORTV_OPERADOR { get; set; }
        public string PORTC_TIPO_PRODUCTO { get; set; }
        public string PORTV_TIPO_VENTA { get; set; }
        public string PORTV_APLICACION { get; set; }
        public string PORTC_ESTADO { get; set; }
        public string PORTV_MOD_VENTA { get; set; }

    }
    //PROY-31393 FIN
}
