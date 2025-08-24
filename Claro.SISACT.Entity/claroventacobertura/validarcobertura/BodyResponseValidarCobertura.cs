using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claroventacobertura.validarcobertura
{
    [DataContract]
    [Serializable]
    public class BodyResponseValidarCobertura
    {
        [DataMember(Name = "validarCoberturaResponse")]
        public ValidarCoberturaResponse validarCoberturaResponse { get; set; }

        public BodyResponseValidarCobertura()
        {
            validarCoberturaResponse = new ValidarCoberturaResponse();
        }
    }
}
