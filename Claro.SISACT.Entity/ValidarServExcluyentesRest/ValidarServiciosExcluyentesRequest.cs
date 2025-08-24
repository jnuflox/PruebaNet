using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ValidarServExcluyentesRest
{   
    [DataContract]
    [Serializable]
    public class ValidarServiciosExcluyentesRequest
    {
        [DataMember(Name = "MessageRequest")]
        public MessageRequestValidarServExclu MessageRequest { get; set; }

        public ValidarServiciosExcluyentesRequest()
        {
            MessageRequest = new MessageRequestValidarServExclu();
        }
    }

    [DataContract]
    [Serializable]
    public class MessageRequestValidarServExclu
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersRequest header { get; set; }
        [DataMember(Name = "Body")]
        public ValidarServExcluRequest body { get; set; }

        public MessageRequestValidarServExclu()
        {
            header = new DataPowerRest.HeadersRequest();
            body = new ValidarServExcluRequest();
        }
    }

    [DataContract]
    [Serializable]
    public class ValidarServExcluRequest
    {
        [DataMember(Name = "validarRequest")]
        public  validarRequestClase objValidarRequest { get; set; }

        public ValidarServExcluRequest()
        {
            objValidarRequest = new validarRequestClase();
        }

    }
    [DataContract]
    [Serializable]
    public class validarRequestClase
    {
        [DataMember(Name = "idServicioAdicional")]
        public string strIdServicioAdi { get; set; }

        [DataMember(Name = "idsServiciosContratados")]
        public string strIdServicioContra { get; set; }

    }

}
