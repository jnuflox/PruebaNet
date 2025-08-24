using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEComentario
    {
        public int COMEC_CODIGO { get; set; }
        public Int64 SOLIN_CODIGO { get; set; }
        public string COMEV_COMENTARIO { get; set; }
        public string COMEC_USU_REG { get; set; }
        public DateTime COMED_FEC_REG { get; set; }
        public string COMEC_ESTADO { get; set; }
        public string COMEC_FLA_COM { get; set; }
        public string COMEC_FLA_COM_DES { get; set; }
    }
}
