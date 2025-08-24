using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.RegistrarServExcluyentesRest
{
    [DataContract]
    [Serializable]
    public class RegistrarServiciosExcluyentesRequest
    {
        [DataMember(Name = "MessageRequest")]
        public MessageRequestRegistrarServExclu MessageRequest { get; set; }

        public RegistrarServiciosExcluyentesRequest()
        {
            MessageRequest = new MessageRequestRegistrarServExclu();
        }
    }

    [DataContract]
    [Serializable]
    public class MessageRequestRegistrarServExclu
    {
        [DataMember(Name = "Header")]
        public DataPowerRest.HeadersRequest header { get; set; }
        [DataMember(Name = "Body")]
        public RegistrarServExcluRequest body { get; set; }

        public MessageRequestRegistrarServExclu()
        {
            header = new DataPowerRest.HeadersRequest();
            body = new RegistrarServExcluRequest();
        }
    }

    [DataContract]
    [Serializable]
    public class RegistrarServExcluRequest
    {
        [DataMember(Name = "insertarRequest")]
        public InsertarRequest objInsertRequest { get; set; }

        public RegistrarServExcluRequest()
        {
            objInsertRequest = new InsertarRequest();
        }

    }

    [DataContract]
    [Serializable]
    public class InsertarRequest
    {
        [DataMember(Name = "descripcionProceso")]
        public string descripcionProceso { get; set; }

        [DataMember(Name = "sec")]
        public string sec { get; set; }

        [DataMember(Name = "servicios")]
        public string servicios { get; set; }

        [DataMember(Name = "codigoTipoVenta")]
        public string codigoTipoVenta { get; set; }

        [DataMember(Name = "codigoEstado")]
        public string codigoEstado { get; set; }

        [DataMember(Name = "codigoTipoOperacion")]
        public string codigoTipoOperacion { get; set; }

        [DataMember(Name = "codigoTipoProducto")]
        public string codigoTipoProducto { get; set; }

        [DataMember(Name = "tipoDocumento")]
        public string tipoDocumento { get; set; }

        [DataMember(Name = "numeroDocumento")]
        public string numeroDocumento { get; set; }

        [DataMember(Name = "puntoVenta")]
        public string puntoVenta { get; set; }

        [DataMember(Name = "usuario")]
        public string usuario { get; set; }
    }

}
