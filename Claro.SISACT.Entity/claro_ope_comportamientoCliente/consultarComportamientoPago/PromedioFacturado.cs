using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claro_ope_comportamientoCliente.consultarComportamientoPago
{
    [DataContract]
    [Serializable]
    public class PromedioFacturado
    {
        [DataMember(Name = "customerId")]
        public string customerId { get; set; }

        [DataMember(Name = "promedio")]
        public double dblPromedio { get; set; }
    }
}
