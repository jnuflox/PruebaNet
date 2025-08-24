//PROY-140380 - FULLCLARO
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity
{
    [DataContract]
    [Serializable]
    public class BEDatosClienteFC
    {
        [DataMember(Name = "elegido")]
        public bool elegido { get; set; }
        
        [DataMember(Name = "coId")]
        public string coId { get; set; }
        
        [DataMember(Name = "linea")]
        public string linea { get; set; }
        
        [DataMember(Name = "planPvudb")]
        public string planPvudb { get; set; }
        
        [DataMember(Name = "tipoServicio")]
        public string tipoServicio { get; set; }
        
        [DataMember(Name = "tmCode")]
        public string tmCode { get; set; }
        
        [DataMember(Name = "desTmcode")]
        public string desTmcode { get; set; }
        
        [DataMember(Name = "customerId")]
        public string customerId { get; set; }
        
        [DataMember(Name = "soplnOrden")]
        public string soplnOrden { get; set; }
        
        [DataMember(Name = "soplnCodigo")]
        public string soplnCodigo { get; set; }
        
    }
}
