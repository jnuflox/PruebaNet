using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public static class BEGlobal
    {
        public static bool flagLogDatosConsulta { get; set; }
        public static bool flagLogDatosInsert { get; set; }
        public static int nroRegistroDataSet { get; set; }
        public static string usuarioConsulta { get; set; }

        public enum TIPO_PAGINA
        {
            WEB = 1,
            DATOS = 2,
            SAP = 3
        }
    }
}
