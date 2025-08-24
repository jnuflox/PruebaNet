using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ConsultarCantidadCampaniaRest
{
    
    //proy-140245 
    [Serializable] //PROY-140126 - IDEA 140248 
    public class Cursor
    {
        [DataMember(Name = "codProducto")]
        public string codProducto { get; set; }
        [DataMember(Name = "descProducto")]
        public string descProducto { get; set; }
        [DataMember(Name = "cantMaxProducto")]
        public int cantMaxProducto { get; set; }
       
    }
}
