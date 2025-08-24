using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claroventacobertura.validarcobertura
{
    [DataContract]
    [Serializable]
    public class detalleError
    {
        [DataMember(Name = "errorCode")]
        public string errorCode { get; set; }

        [DataMember(Name = "errorDescription")]
        public string errorDescription { get; set; }

        public detalleError()
        {
            errorCode = string.Empty;
            errorDescription = string.Empty;

        }
    }

      
}
