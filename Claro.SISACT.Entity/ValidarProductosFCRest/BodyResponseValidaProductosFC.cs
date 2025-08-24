using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.ValidarProductosFCRest
{
    [DataContract]
    [Serializable]
    public class BodyResponseValidaProductosFC
    {
        [DataMember(Name = "codigoRespuesta")]
        public string codigoRespuesta { get; set; }
        [DataMember(Name = "mensajeRespuesta")]
        public string mensajeRespuesta { get; set; }
        [DataMember(Name = "listaOnHold")]
        public List<BEDatosClienteFC> datosOnHoldClienteFC { get; set; }
        [DataMember(Name = "listaCandidatos")]
        public List<BENuevosProductosFC> nuevosProductosFC { get; set; }
    }
}
