using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Claro.SISACT.Entity.MotorPromociones
{
    [Serializable]
    [DataContract]
    public class ResponseAudit
    {
        [DataMember(Name = "idTransaccion")]
        public string idTransaccion { get; set; }

        [DataMember(Name = "itemId")]
        public string itemId { get; set; }

        [DataMember(Name = "codigoRespuesta")]
        public string codigoRespuesta { get; set; }

        [DataMember(Name = "mensajeRespuesta")]
        public string mensajeRespuesta { get; set; }

        public ResponseAudit()
        {
            idTransaccion = string.Empty;
            itemId = string.Empty;
            codigoRespuesta = string.Empty;
            mensajeRespuesta = string.Empty;
        }
    }
}
