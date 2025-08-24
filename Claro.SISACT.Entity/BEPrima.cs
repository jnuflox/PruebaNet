using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEPrima //PROY-24724-IDEA-28174 - NUEVA CLASE
    {
        public string CodEval { get; set; }
        public string CodMaterial { get; set; }
        public string DeducibleDanio { get; set; }
        public string DeducibleRobo { get; set; }
        public string DescProt { get; set; }
        public string DescProd { get; set; }
        public string FechaCreacion { get; set; }
        public string FechaEvaluacion { get; set; }
        public string FechaModif { get; set; }
        public string FlagEstado { get; set; }
        public string FlagPortabilidad { get; set; }
        public string IncidenciaTipoDanio { get; set; }
        public string IncidenciaTipoRobo { get; set; }
        public string Ip { get; set; }
        public string MontoPrima { get; set; }
        public string NombreProd { get; set; }
        public string NroCertif { get; set; }
        public string NroDoc { get; set; }
        public string NroSec { get; set; }
        public string Resultado { get; set; }
        public string SoplnCodigo { get; set; }
        public string TipoCliente { get; set; }
        public string TipoDoc { get; set; }
        public string TipoOperacion { get; set; }        
        public string UsrMod { get; set; }
        public string UsrAplicacion { get; set; }
    }
}
