using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ConsultarCantidadCampaniaRest
{
   
    //PROY-140245
    [DataContract]
    [Serializable] //PROY-140126 - IDEA 140248 
    public class ValidarCantidadCampaniasRequest
    {
        [DataMember(Name = "MessageRequest")]
        public MessageRequestValidCantCampania MessageRequest { get; set; }

        public ValidarCantidadCampaniasRequest()
        {
            MessageRequest = new MessageRequestValidCantCampania();
        }
    }
}
