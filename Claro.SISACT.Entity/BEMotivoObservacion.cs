using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    //INI PROY-31948_Migracion
    [Serializable] //PROY-32439 Serializable
public class BEMotivoObservacion
    {
		public int CODIGO { get; set; }
		public string NOMBRE { get; set; }
		public BETipoDocumentoE TIPO_DOCUMENTO { get; set; }
		public BERegistro REGISTRO { get; set; }
        
    }
    //FIN PROY-31948_Migracion
}
