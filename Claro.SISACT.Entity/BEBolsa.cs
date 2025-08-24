using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEBolsa
    {
        public string Descripcion { get; set; }
        public string TipoCliente { get; set; }
        public string SaldoTT { get; set; }
        public string ValorSegmento { get; set; }        
    }
}
