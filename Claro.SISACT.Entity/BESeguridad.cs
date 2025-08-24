using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BESeguridad
    {
        public BESeguridad() { }

        public string USUACCOD { get; set; }
        public string PERFCCOD { get; set; }
        public string USUACCODVENSAP { get; set; }
        public string APLICCOD { get; set; }
        public string OPCCODPAD { get; set; }
        public string OPCICCOD { get; set; }
        public string OPCICNIVPAD { get; set; }
        public string OPCICNIV { get; set; }
        public string OPCICDES { get; set; }
        public string OPCICABREV { get; set; }
        public string OPCICNOMPAG { get; set; }
        public string OPCICNUMORD { get; set; }
        public string OPCICD1 { get; set; }  
    }
}
