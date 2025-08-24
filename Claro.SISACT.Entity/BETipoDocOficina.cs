using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
   public class BETipoDocOficina
    {
        public string CodDoc { get; set; }
        public string DesDoc { get; set; }
        public string ClasDoc { get; set; }
    }
}
