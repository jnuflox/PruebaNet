using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.claroventacobertura.validarcobertura
{
    [DataContract]
    [Serializable]
    public class ResponseData
    {
        [DataMember(Name = "tipoTecnologia")]
        public string tipoTecnologia { get; set; }

        [DataMember(Name = "exiteCobertura")]
        public string exiteCobertura { get; set; }

        [DataMember(Name = "codPlano")]
        public string codPlano { get; set; }

        [DataMember(Name = "listaTecnologiasDisponibles")]
        public List<ListaTecnologiasDisponibles> listaTecnologiasDisponibles { get; set; }


        public ResponseData()
        {
            tipoTecnologia = string.Empty;
            exiteCobertura = string.Empty;
            codPlano = string.Empty;
            listaTecnologiasDisponibles = new List<ListaTecnologiasDisponibles>();

        }
    }
}
