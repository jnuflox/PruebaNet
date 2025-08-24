using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claro_ope_comportamientoCliente.consultarComportamientoPago
{
    [DataContract]
    [Serializable]
    public class BodyRequestComportamientoCliente
    {
        [DataMember(Name = "consultarComportamientoPagoRequest")]
        public ConsultarRequestComportamientoCliente consultarComportamientPago { get; set; }

        [DataMember(Name = "consultarPromedioFacturadoRequest")]
        public ConsultarRequestComportamientoCliente consultarPromedioFacturado { get; set; }

        public BodyRequestComportamientoCliente()
        {
            consultarComportamientPago = new ConsultarRequestComportamientoCliente();
            consultarPromedioFacturado = new ConsultarRequestComportamientoCliente();
        }
    }
}
