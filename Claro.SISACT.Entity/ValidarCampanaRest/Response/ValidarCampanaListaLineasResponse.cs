using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ValidarCampanaRest.Response
{
    [DataContract]
    [Serializable]
    public class ValidarCampanaListaLineasResponse
    {
        [DataMember(Name = "cscompregno")]
        public string cscompregno { get; set; }

        [DataMember(Name = "coId")]
        public string coId { get; set; }

        [DataMember(Name = "dnNum")]
        public string dnNum { get; set; }

        [DataMember(Name = "bonoDes")]
        public string bonoDes { get; set; }
    }
}
