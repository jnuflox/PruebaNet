//INC-SMS_PORTA_INI
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable]
    public class BERegistroSmsPorta
    {
        public string nro_linea { get; set; }
        public string codigo { get; set; }
    }
}
//INC-SMS_PORTA_FIN