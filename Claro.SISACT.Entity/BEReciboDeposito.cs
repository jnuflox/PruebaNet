using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEReciboDeposito
    {
        public int RECIBO_ID { get; set; }
        public string NRO_OPERACION { get; set; }
        public double MONTO_DEPOSITO { get; set; }
        public DateTime FECHA_DEPOSITO { get; set; }
        public int BANCO_ID { get; set; }
        public string BANCO_DES { get; set; }
        public Int64 SOLIN_CODIGO { get; set; }
        public string LOGIN { get; set; }
        public string TERMINAL { get; set; }
        public string ESTADO { get; set; }
    }
}
