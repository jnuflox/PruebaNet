using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Claro.SISACT.Entity.MotorPromociones.GetBonos
{
    [Serializable]
    [DataContract]
    public class BodyRequestGetBonos
    {
        [DataMember(Name = "msisdn")]
        public string msisdn { get; set; }

        [DataMember(Name = "numeroDocumento")]
        public string numeroDocumento { get; set; }

        [DataMember(Name = "tipoDocumento")]
        public string tipoDocumento { get; set; }

        [DataMember(Name = "all")]
        public bool all { get; set; }

        [DataMember(Name = "actived")]
        public bool actived { get; set; }

        [DataMember(Name = "deactivated")]
        public bool deactivated { get; set; }

        [DataMember(Name = "category")]
        public string category { get; set; }

        [DataMember(Name = "group")]
        public string group { get; set; }

        [DataMember(Name = "event")]
        public string _event { get; set; }

        public BodyRequestGetBonos()
        {
            msisdn = string.Empty;
            numeroDocumento = string.Empty;
            tipoDocumento = string.Empty;
            all = false;
            actived = false;
            deactivated = false;
            category = string.Empty;
            group = string.Empty;
            _event = string.Empty;
        }
    }
}
