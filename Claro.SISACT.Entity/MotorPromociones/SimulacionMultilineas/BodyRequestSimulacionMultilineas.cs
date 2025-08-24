using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Claro.SISACT.Entity.MotorPromociones.SimulacionMultilineas
{
    [Serializable]
    [DataContract]
    public class BodyRequestSimulacionMultilineas
    {
        [DataMember(Name = "numeroDocumento")]
        public string numeroDocumento { get; set; }

        [DataMember(Name = "tipoDocumento")]
        public string tipoDocumento { get; set; }

        [DataMember(Name = "lineasAdicionales")]
        public bool lineasAdicionales { get; set; }

        [DataMember(Name = "duplicaGb")]
        public bool duplicaGb { get; set; }

        [DataMember(Name = "fullClaro")]
        public bool fullClaro { get; set; }

        [DataMember(Name = "newContract")]
        public List<ListNewContractRequest> newContract { get; set; }

        public BodyRequestSimulacionMultilineas()
        {
            numeroDocumento = string.Empty;
            tipoDocumento = string.Empty;
            lineasAdicionales = false;
            duplicaGb = false;
            fullClaro = false;
        }
    }
}
