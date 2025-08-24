using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEDataCreditoHistorico
    {
        public string HISTV_NUM_OPERACION { get; set; }
        public string HISTC_TIPO_DOCUMENTO { get; set; }
        public string HISTV_TIPO_DOCUMENTO_DESC { get; set; }
        public string HISTV_NUM_DOCUMENTO { get; set; }
        public string HISTV_APELLIDO_PAT { get; set; }
        public string HISTV_APELLIDO_MAT { get; set; }
        public string HISTV_NOMBRE { get; set; }
        public string HISTC_TIPO_RESPUESTA { get; set; }
        public string HISTC_TIPO_RIESGO { get; set; }
        public int HISTN_CANT_INTENTOS { get; set; }
        public string HISTV_OVEN_CODIGO { get; set; }
        public string HISTV_OVEN_DESC { get; set; }
        public string HISTV_TERMINAL_ID { get; set; }
        public string HISTN_USUARIO_REG { get; set; }
        public string HISTD_FECHA_REG { get; set; }
    }
}
