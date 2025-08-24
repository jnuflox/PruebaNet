using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEArchivo
    {
        public Int64 ARCH_ID { get; set; }
        public Int64 SOLIN_CODIGO { get; set; }
        public string ARCH_NOMBRE { get; set; }
        public string ARCH_RUTA { get; set; }
        public string FLAG_ESTADO { get; set; }
        public string ARCH_TIPO { get; set; }
        public string ARCH_CODIGO { get; set; }
        public string ARCH_DESCRIPCION { get; set; }

        public BEParametroPortabilidad TIPO_ARCHIVO { get; set; }
    }
}
