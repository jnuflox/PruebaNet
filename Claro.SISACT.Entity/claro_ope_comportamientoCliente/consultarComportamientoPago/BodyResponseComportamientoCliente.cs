using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claro_ope_comportamientoCliente.consultarComportamientoPago
{
    [DataContract]
    [Serializable]
    public class BodyResponseComportamientoCliente
    {
        [DataMember(Name = "consultarComportamientoPagoResponse")]
        public ConsultarResponseComportamientoCliente consultarComportamientoPagoResponse {get; set;}

        [DataMember(Name = "consultarPromedioFacturadoResponse")]
        public ConsultarResponseComportamientoCliente consultarPromedioFacturadoResponse { get; set; }

        public BodyResponseComportamientoCliente()
        {
            consultarComportamientoPagoResponse = new ConsultarResponseComportamientoCliente();
            consultarPromedioFacturadoResponse = new ConsultarResponseComportamientoCliente();
        }
    }
}
