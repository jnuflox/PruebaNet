using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ValidarServExcluyentesRest
{
    [DataContract]
    [Serializable]
    public class ValidarServiciosExcluyentesResponse
    {
        [DataMember(Name = "MessageResponse")]
        public MessageResponseValidarServExclu MessageResponse {get; set;}

        public ValidarServiciosExcluyentesResponse()
        {
            MessageResponse = new MessageResponseValidarServExclu();
        }
    }
    [DataContract]
    [Serializable]
    public class MessageResponseValidarServExclu
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeaderResponse header { get; set; }
        [DataMember(Name = "Body")]
        public BodyResponseValidarServExclu body { get; set; }

        public MessageResponseValidarServExclu()
        {
            header = new DataPowerRest.HeaderResponse();
            body = new BodyResponseValidarServExclu();
        }
    }
    [DataContract]
    [Serializable]
    public class BodyResponseValidarServExclu
    {
        [DataMember(Name = "codigoRespuesta")]
        public string strCodigoRespuesta { get; set; }
        [DataMember(Name = "mensajeRespuesta")]
        public string mensajeRespuesta { get; set; }
        [DataMember(Name = "validarServicioExcluyente")]
        public ValidarServicioExcluyente ValidarServicio {get; set;}

        public BodyResponseValidarServExclu()
        {
            ValidarServicio = new ValidarServicioExcluyente();
        }
    }

    public class ValidarServicioExcluyente
    {
        [DataMember(Name = "codigoServExcluyente")]
        public string codigoServExcluyente { get; set; }
    }

}
