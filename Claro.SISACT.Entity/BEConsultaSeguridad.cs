//INICIO PROY-140419 Autorizar Portabilidad sin PIN
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity
{
    [DataContract]
    [Serializable] 
    public class BEConsultaSeguridad
    {
        [DataMember]
        public string USUACCOD { get; set; }

        [DataMember]
        public string PERFCCOD { get; set; }

        [DataMember]
        public string USUACCODVENSAP { get; set; }
    }
}
//FIN PROY-140419 Autorizar Portabilidad sin PIN