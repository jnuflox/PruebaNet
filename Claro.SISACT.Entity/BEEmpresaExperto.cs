using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEEmpresaExperto
    {
        public string strRazonSocial { get; set; }
        public string strNombres { get; set; }
        public string strApellidoPaterno { get; set; }
        public string strApellidoMaterno { get; set; }
        public string strRiesgo { get; set; }
        public string strNroOperacion { get; set; }
        public string strCodRetorno { get; set; }
        public string strCodError { get; set; }
        public string strMensajeError { get; set; }
        public string strMensaje { get; set; }
        public string strFlagInterno { get; set; }
        public double deuda_financiera { get; set; }
        public string estado_ruc { get; set; }
        public string origen_experto { get; set; }
        public List<BERepresentanteLegal> oRepresentanteLegal { get; set; }

        //INICIO PROY-20054-IDEA-23849
        public int buro_consultado { get; set; }
        //FIN

        /*INI PROY-32438*/
        public string TipContribuyente { get; set; }
        public string NomComercial { get; set; }
        public string FecIniActividades { get; set; }
        public string EstContribuyente { get; set; }
        public string CondContribuyente { get; set; }
        public string CiiuContribuyente { get; set; }
        public int CantTrabajadores { get; set; }
        public string EmisionComp { get; set; }
        public string SistEmielectronica { get; set; }
        public int CantMesIniActividades { get; set; }
        /*FIN PROY-32438*/
    }
}
