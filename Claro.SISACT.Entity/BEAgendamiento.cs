using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEAgendamiento
    {
        public string IdContratista { get; set; }
        public string Contratista { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }                    
    }
}
