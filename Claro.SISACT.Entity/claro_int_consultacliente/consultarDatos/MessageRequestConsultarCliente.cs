using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Claro.SISACT.Entity.DataPowerRest;

namespace Claro.SISACT.Entity.claro_int_consultacliente.consultarDatos
{
    [DataContract]
    [Serializable]
    public class MessageRequestConsultarCliente
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersRequest Header { get; set; }

        [DataMember(Name = "consultarDatosRequest")]
        public BodyRequestConsultarCliente Body { get; set; }

        public MessageRequestConsultarCliente()
        {
            Header = new DataPowerRest.HeadersRequest();
            Body = new BodyRequestConsultarCliente();
        }
    }
}
