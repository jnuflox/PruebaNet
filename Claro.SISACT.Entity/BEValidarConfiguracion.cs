using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claro_vent_identidadcontingencia.validarconfiguracion
{
    [Serializable]
    [DataContract]
    public class BEValidarConfiguracion
    {
        [DataMember(Name = "validacion")]
        public string validacion { get; set; }

        public BEValidarConfiguracion()
        {
            validacion = string.Empty;
        }
    }
}
