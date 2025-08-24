using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
//PROY-140618
namespace Claro.SISACT.Entity.RegistrarEvaluacionMejPortaRest
{
    [DataContract]
    [Serializable]
    public class RegistrarEvaluacionRequest
    {
        [DataMember(Name = "idEval")]
        public string idEval { get; set; }

        [DataMember(Name = "tipoDoc")]
        public string tipoDoc { get; set; }

        [DataMember(Name = "nroDoc")]
        public string nroDoc { get; set; }

        [DataMember(Name = "nacionalidad")]
        public string nacionalidad { get; set; }

        [DataMember(Name = "nombre")]
        public string nombre { get; set; }

        [DataMember(Name = "apePaterno")]
        public string apePaterno { get; set; }

        [DataMember(Name = "apeMaterno")]
        public string apeMaterno { get; set; }

        [DataMember(Name = "email")]
        public string email { get; set; }

        [DataMember(Name = "tipoOperacion")]
        public string tipoOperacion { get; set; }

        [DataMember(Name = "tipoOferta")]
        public string tipoOferta { get; set; }

        [DataMember(Name = "modVenta")]
        public string modVenta { get; set; }

        [DataMember(Name = "casoEspecial")]
        public string casoEspecial { get; set; }

        [DataMember(Name = "tipoServicio")]
        public string tipoServicio { get; set; }

        [DataMember(Name = "modalidad")]
        public string modalidad { get; set; }

        [DataMember(Name = "operadorCedente")]
        public string operadorCedente { get; set; }

        [DataMember(Name = "codCampania")]
        public string codCampania { get; set; }

        [DataMember(Name = "descCampania")]
        public string descCampania { get; set; }

        [DataMember(Name = "plazo")]
        public string plazo { get; set; }

        [DataMember(Name = "familiaPlan")]
        public string familiaPlan { get; set; }

        [DataMember(Name = "codPlan")]
        public string codPlan { get; set; }

        [DataMember(Name = "descPlan")]
        public string descPlan { get; set; }

        [DataMember(Name = "servAdicional")]
        public string servAdicional { get; set; }

        [DataMember(Name = "cargoFijo")]
        public string cargoFijo { get; set; }

        [DataMember(Name = "codEquipo")]
        public string codEquipo { get; set; }

        [DataMember(Name = "descEquipo")]
        public string descEquipo { get; set; }

        [DataMember(Name = "nroCuotas")]
        public string nroCuotas { get; set; }

        [DataMember(Name = "linea")]
        public string linea { get; set; }

        [DataMember(Name = "resultado_eval")]
        public string resultado_eval { get; set; }

        [DataMember(Name = "lcDisponible")]
        public string lcDisponible { get; set; }

        [DataMember(Name = "comportamiento")]
        public string comportamiento { get; set; }

        [DataMember(Name = "rangoLc")]
        public string rangoLc { get; set; }

        [DataMember(Name = "tipoGarantia")]
        public string tipoGarantia { get; set; }

        [DataMember(Name = "importeRa")]
        public string importeRa { get; set; }

        [DataMember(Name = "codOficina")]
        public string codOficina { get; set; }

        [DataMember(Name = "descOficina")]
        public string descOficina { get; set; }

        [DataMember(Name = "consultaPrevia")]
        public string consultaPrevia { get; set; }

        [DataMember(Name = "fechaCp")]
        public string fechaCp { get; set; }

        [DataMember(Name = "deudaCp")]
        public string deudaCp { get; set; }

        [DataMember(Name = "usuarioReg")]
        public string usuarioReg { get; set; }
        
    }
}
