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
    public class MessageResponseValidCantCampania
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersResponse header { get; set; }
        [DataMember(Name = "Body")]
        public BodyResponseValidCantCampania body { get; set; }

        public MessageResponseValidCantCampania()
        {
            header = new DataPowerRest.HeadersResponse();
            body = new BodyResponseValidCantCampania();
        }
    }
}
