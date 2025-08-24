using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.RegistrarOmisionPINRest
{
    [DataContract]
    [Serializable]
    public class BEDetalleOmisionPIN
    {
        [DataMember(Name = "linea")]
        public string linea{ get; set; }

        [DataMember(Name = "consecutivo")]
         public string consecutivo{ get; set; }
    } 
}
