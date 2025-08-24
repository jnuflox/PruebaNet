using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEDataCreditoCorpIN
    {
        public string istrTipoDocumento { get; set; }
        public string istrNumeroDocumento { get; set; }
        public string istrApellidoPaterno { get; set; }
        public string istrApellidoMaterno { get; set; }
        public string istrNombres { get; set; }
        public string istrTipoPersona { get; set; }
        public string istrCodSolicitud { get; set; }
        public string istrPuntoVenta { get; set; }
        public string istrTipoSEC { get; set; }
    }
}
