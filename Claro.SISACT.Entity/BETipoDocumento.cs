using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BETipoDocumento
    {
        public String ID_SISACT { get; set; }
        public string DESCRIPCION { get; set; }
        public string ESTADO { get; set; }
        public int ID_INFOCORP { get; set; }
        public string ID_ABDCP { get; set; }
        public string ID_DC { get; set; }
        public int ID_BSCS { get; set; }
        public string ID_SGA { get; set; }
        public string ID_OAC { get; set; }
        public string ID_BSCS_IX { get; set; } //INICIATIVA-219
    }
}
