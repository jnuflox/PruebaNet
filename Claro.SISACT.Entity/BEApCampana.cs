using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //IDEA-142010
    public class BEApCampana
    {
        public string CAMPV_CODIGO { get; set; }
        public string CAMPV_DESCRIPCION { get; set; }
        public string CAMPV_TIPO_PRODUCTO { get; set; }
        public string CAMPD_FECHA_INICIO { get; set; }
        public string CAMPD_FECHA_FIN { get; set; }
    }
}
