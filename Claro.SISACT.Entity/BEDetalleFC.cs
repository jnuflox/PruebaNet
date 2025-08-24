using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity
{
    [DataContract]
    [Serializable] //PROY-140380 - FULLCLARO
    public class BEDetalleFC
    {
        
        [DataMember(Name = "consecutivo")]
        public string consecutivo { get; set; }
        
        [DataMember(Name = "planPvuDb")]
        public string planPvuDb { get; set; }
        
        [DataMember(Name = "planBSCS")]
        public string planBSCS { get; set; }
        
        [DataMember(Name = "descripcionPlan")]
        public string descripcionPlan { get; set; }
        
        [DataMember(Name = "linea")]
        public string linea { get; set; }
        
        [DataMember(Name = "tipoServicio")]
        public string tipoServicio { get; set; }
        
        [DataMember(Name = "numeroContrato")]
        public string numeroContrato { get; set; }
        
        [DataMember(Name = "coId")]
        public string coId { get; set; }
        
        [DataMember(Name = "customerId")]
        public string customerId { get; set; }
        
        [DataMember(Name = "soplnCodigo")]
        public string soplnCodigo { get; set; }
        
        [DataMember(Name = "soplnOrden")]
        public string soplnOrden { get; set; }
        
        [DataMember(Name = "estado")]
        public string estado { get; set; }
        
    }
}
