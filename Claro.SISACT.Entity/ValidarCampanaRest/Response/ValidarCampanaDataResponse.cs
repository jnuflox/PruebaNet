using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ValidarCampanaRest.Response
{    
    [DataContract]
    [Serializable]
    public class ValidarCampanaDataResponse
    {
        [DataMember(Name = "campana")]
        public string campana { get; set; }

        [DataMember(Name = "listaLineas")]
        public List<ValidarCampanaListaLineasResponse> listaLineas { get; set; }

        [DataMember(Name = "responseOpcional")]
        public string responseOpcional { get; set; }

    }
}
