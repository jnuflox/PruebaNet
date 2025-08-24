using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.RegistraCandidatoFullClaroRest
{
    [DataContract]
    [Serializable] //PROY-140380 - FULLCLARO
    public class BECabeceraFC
    {
        [DataMember(Name = "nroSec")]
        public string nroSec {get; set;}
        
        [DataMember(Name = "tipoDoc")]
        public string tipoDoc {get; set;}
        
        [DataMember(Name = "numDoc")]
        public string numDoc {get; set;}
        
        [DataMember(Name = "codProducto")]
        public string codProducto {get; set;}
        
        [DataMember(Name = "beneficio")]
        public string beneficio {get; set;}
        
        [DataMember(Name = "tipoOperacion")]
        public string tipoOperacion {get; set;}
        
        [DataMember(Name = "flagPorta")]
        public string flagPorta {get; set;}
        
        [DataMember(Name = "usuario")]
        public string usuario {get; set;}
        
        [DataMember(Name = "detalle")]
        public List<BEDetalleFC> datosDetalleFC  {get; set;}

    }
}
