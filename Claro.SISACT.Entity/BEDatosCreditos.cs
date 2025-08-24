using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEDatosCreditos
    {
        public Int64 SOLIN_CODIGO { get; set; }
        public double LC_DISPONIBLE { get; set; }
        public string PRODUCTO_MONTO { get; set; }
        public string MSJ_AUTONOMIA { get; set; }
        public string MOTIVO { get; set; }
        public string USUARIO_CREA { get; set; }
        public string RANGO_LC_DISPONIBLE { get; set; }
        public int nroLineas { get; set; }
        public int nroLineaMenor7Dia { get; set; }
        public int nroLineaMenor30Dia { get; set; }
        public int nroLineaMenor90Dia { get; set; }
        public int nroLineaMenor180Dia { get; set; }
        public int nroLineaMayor180Dia { get; set; }
    }
}
