using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Claro.SISACT.Entity.MotorPromociones.GetMultilineaInfo
{
    [Serializable]
    [DataContract]
    public class BodyRequestGetMultilineaInfo
    {
        [DataMember(Name = "tipoDocumento")]
        public string tipoDocumento { get; set; }

        [DataMember(Name = "numeroDocumento")]
        public string numeroDocumento { get; set; }

        public BodyRequestGetMultilineaInfo()
        {
            tipoDocumento = string.Empty;
            numeroDocumento = string.Empty;
        }
    }
}
