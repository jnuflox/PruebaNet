using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BeConsultaPrevia
    {
        public BEItemGenerico auditoria { get; set; }
        public Int64 numeroSEC { get; set; }
        public string numeroSecuencial { get; set; }
        public string codigoCedente { get; set; }
        public string modalidad { get; set; }
        public string msisdn { get; set; }
        public string tipoDocumento { get; set; }
        public string numeroDocumento { get; set; }
        public string nombreRSAbonado { get; set; }
        public string observaciones { get; set; }
        public string tipoPorta { get; set; }
        public string modoEnvio { get; set; }
        public string tipoServicio { get; set; }
        public string tipoProdcuto { get; set; }
        //INI: PROY-140223 IDEA-140462
        public string modalidadVenta { get; set; }
        public string canalVenta { get; set; }
        //FIN: PROY-140223 IDEA-140462

    }
}
