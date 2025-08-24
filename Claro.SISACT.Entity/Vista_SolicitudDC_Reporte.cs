using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class Vista_SolicitudDC_Reporte
    {
        public string DCREV_NUM_OPERACION { get; set; }
        public Int64 DCREN_SOLIN_CODIGO { get; set; }
        public string DCREV_OVEN_CODIGO { get; set; }
        public string DCREV_OVEN_DESC { get; set; }
        public string DCREN_USUARIO_REG { get; set; }
        public string DCREC_TIPO_DOCUMENTO { get; set; }
        public string DCREV_TIPO_DOCUMENTO_DESC { get; set; }
        public string DCREV_NUM_DOCUMENTO { get; set; }
        public string DCREV_APELLIDO_PAT { get; set; }
        public string DCREV_APELLIDO_MAT { get; set; }
        public string DCREV_NOMBRE { get; set; }
        public int DCREN_CANT_INTENTOS { get; set; }
        public string DCREC_VALIDAR_CLIENTE { get; set; }
        public string FUENTECONSULTA { get; set; }
    }
}
