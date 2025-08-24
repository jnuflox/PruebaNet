using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
//PROY-140618
namespace Claro.SISACT.Entity.IteracionCliente.Response
{
    [DataContract]
    [Serializable]
    public class BEDatosCabecera
    {
        public BEDatosCabecera()
        {
        }

        [DataMember(Name = "tipoDoc")]
        public string tipoDoc { get; set; }

        [DataMember(Name = "nroDoc")]
        public string nroDoc {get; set;}

        [DataMember(Name = "nombre")]
        public string nombre {get; set;}

        [DataMember(Name = "apePaterno")]
        public string apePaterno{get; set;}

        [DataMember(Name = "apeMaterno")]
        public string apeMaterno { get; set; }

        [DataMember(Name = "email")]
        public string email { get; set; }

        [DataMember(Name = "nacionalidad")]
        public string nacionalidad { get; set; }

    }
}
