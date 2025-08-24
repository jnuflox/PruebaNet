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
    public class MessageRequestValidCantCampania
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersRequest header { get; set; }
        [DataMember(Name = "Body")]
        public BodyRequestValidCantCampania body { get; set; }

        public MessageRequestValidCantCampania()
        {
            header = new DataPowerRest.HeadersRequest();
            body = new BodyRequestValidCantCampania();
        }
    }
}
