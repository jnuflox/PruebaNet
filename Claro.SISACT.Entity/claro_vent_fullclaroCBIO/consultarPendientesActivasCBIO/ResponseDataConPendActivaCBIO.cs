using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Claro.SISACT.Entity.GenericRequestResponse;
using Claro.SISACT.Entity.claro_vent_fullclaroCBIO.consultarPendientesActivasCBIO;

namespace Claro.SISACT.Entity.claro_vent_fullclaroCBIO
{
    [DataContract]
    [Serializable]
    public class ResponseDataConPendActivaCBIO
    {
        [DataMember(Name = "listaOnHold")]
        public List<ListaOnHold> listaOnHold { get; set; }

        [DataMember(Name = "listaCandidatos")]
        public List<ListaCandidatos> listaCandidatos { get; set; }
    }
}
