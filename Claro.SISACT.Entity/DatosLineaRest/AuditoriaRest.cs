using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;
using System.Collections;

namespace Claro.SISACT.Entity.DatosLineaRest
{
    [DataContract]
    [Serializable]
    public class AuditoriaRest
    {
        [DataMember(Name = "idTransaccion")]
        public string idTransaccion { get; set; }

        [DataMember(Name = "codigoRespuesta")]
        public string codigoRespuesta { get; set; }

        [DataMember(Name = "mensajeRespuesta")]
        public string mensajeRespuesta { get; set; }

        [DataMember(Name = "msgid")]
        public string msgid { get; set; }

        [DataMember(Name = "timestamp")]
        public string timestamp { get; set; }

        [DataMember(Name = "userId")]
        public string userId { get; set; }

        public Hashtable table { get; set; }

        public string url { get; set; }
        public string timeout { get; set; }
    }
}
