using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Claro.SISACT.Entity.MotorPromociones.SimulacionMultilineas
{
    [Serializable]
    [DataContract]
    public class ListNewContractRequest
    {
        [DataMember(Name = "linea")]
        public string linea { get; set; }

        [DataMember(Name = "operacionOrigen")]
        public string operacionOrigen { get; set; }

        [DataMember(Name = "planFinal")]
        public string planFinal { get; set; }

        [DataMember(Name = "cargoFijoPlanDestino")]
        public string cargoFijoPlanDestino { get; set; }

        [DataMember(Name = "campania")]
        public string campania { get; set; }

        [DataMember(Name = "tipoProducto")]
        public string tipoProducto { get; set; }

        [DataMember(Name = "tipoSuscripcion")]
        public string tipoSuscripcion { get; set; }

        [DataMember(Name = "antiguedadLinea")]
        public string antiguedadLinea { get; set; }

        [DataMember(Name = "poNameDestino")]
        public string poNameDestino { get; set; }

        public ListNewContractRequest()
        {
            linea = string.Empty;
            operacionOrigen = string.Empty;
            planFinal = string.Empty;
            cargoFijoPlanDestino = string.Empty;
            campania = string.Empty;
            tipoProducto = string.Empty;
            tipoSuscripcion = string.Empty;
            antiguedadLinea = string.Empty;
            poNameDestino = string.Empty;
        }
    }
}
