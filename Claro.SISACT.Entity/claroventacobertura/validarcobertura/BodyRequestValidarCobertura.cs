using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claroventacobertura.validarcobertura
{
    [DataContract]
    [Serializable]
    public class BodyRequestValidarCobertura
    {
        [DataMember(Name = "validarCoberturaRequest")]
        public ValidarCoberturaRequest validarCoberturaRequest { get; set; }

        public BodyRequestValidarCobertura()
        {
            validarCoberturaRequest = new ValidarCoberturaRequest();
        }
    }
}
