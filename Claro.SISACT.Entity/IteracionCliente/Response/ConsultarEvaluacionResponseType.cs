using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
//PROY-140618
namespace Claro.SISACT.Entity.IteracionCliente.Response
{
    [DataContract]
    [Serializable]
    public class ConsultarEvaluacionResponseType
    {
        [DataMember(Name = "mensajeError")]
        public string mensajeError { get; set; }

        [DataMember(Name = "codigoRespuesta")]
        public string codigoRespuesta { get; set; }

        [DataMember(Name = "mensajeRespuesta")]
        public string mensajeRespuesta { get; set; }

        [DataMember(Name = "listaCursor")]
        public List<BEDatosCabecera> listaCursor { get; set; }

        [DataMember(Name = "listaDetalle")]
        public List<BEDatosDetalle> listaDetalle { get; set; }

        public ConsultarEvaluacionResponseType()
        {
            this.listaCursor = new List<BEDatosCabecera>();
            this.listaDetalle = new List<BEDatosDetalle>();
        }
    }
}
