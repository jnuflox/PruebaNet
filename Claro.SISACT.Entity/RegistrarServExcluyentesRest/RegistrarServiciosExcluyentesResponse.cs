using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.RegistrarServExcluyentesRest
{
    [DataContract]
    [Serializable]
    public class RegistrarServiciosExcluyentesResponse
    {
        [DataMember(Name = "MessageResponse")]
        public MessageResponseRegistrarServExclu MessageResponse { get; set; }

        public RegistrarServiciosExcluyentesResponse()
        {
            MessageResponse = new MessageResponseRegistrarServExclu();
        }
    }
    [DataContract]
    [Serializable]
    public class MessageResponseRegistrarServExclu
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeaderResponse header { get; set; }
        [DataMember(Name = "Body")]
        public BodyResponseRegistrarExclu body { get; set; }

        public MessageResponseRegistrarServExclu()
        {
            header = new DataPowerRest.HeaderResponse();
            body = new BodyResponseRegistrarExclu();
        }
    }
    [DataContract]
    [Serializable]
    public class BodyResponseRegistrarExclu
    {
        [DataMember(Name = "codigoRespuesta")]
        public string strCodigoRespuesta { get; set; }
        [DataMember(Name = "mensajeRespuesta")]
        public string mensajeRespuesta { get; set; }
    }

}
