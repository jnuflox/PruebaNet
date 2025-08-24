using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.AfiliaciónDEAUAsistidosWSRest.ConsultarAfiliacion
{
    [DataContract]
    [Serializable]
    public class BodyRequestConsultarAfiliacionDEAU
    {
       /* [DataMember(Name="Body")]
        public BEConsultaRegistroDEAURequest body { get; set; }

        public BodyRequestConsultaRegistroDEAU()
        {
            body = new BEConsultaRegistroDEAURequest();
        }*/

        [DataMember(Name = "idAfiliacion")]
        public string idAfiliacion { get; set; }
        [DataMember(Name = "tipoConsulta")]
        public string tipoConsulta { get; set; }
        [DataMember(Name = "tipoDocumento")]
        public string tipoDocumento { get; set; }
        [DataMember(Name = "nroDocumento")]
        public string nroDocumento { get; set; }
        [DataMember(Name = "canalMp")]
        public string canalMp { get; set; }
        [DataMember(Name = "estadoVenta")]
        public string estadoVenta { get; set; }
        [DataMember(Name = "estadoAfiliacion")]
        public string estadoAfiliacion { get; set; }
    }
}
