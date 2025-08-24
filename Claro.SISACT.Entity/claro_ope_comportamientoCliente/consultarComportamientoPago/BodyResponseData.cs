using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claro_ope_comportamientoCliente.consultarComportamientoPago
{
    [DataContract]
    [Serializable]
    public class BodyResponseData
    {
        [DataMember(Name = "comportamientoPago")]
        public string comportamientoPago { get; set; }

        [DataMember(Name = "listaPromedioFacturado")]
        public List<PromedioFacturado> listPromedioFacturado { get; set; }

        [DataMember(Name = "listaOpcional")]
        public List<ListaOpcional> listOpcional { get; set; }
    }
}
