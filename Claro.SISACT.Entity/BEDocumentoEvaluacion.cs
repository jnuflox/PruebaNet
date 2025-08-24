using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
[Serializable] //PROY-32439 Serializable
    public class BEDocumentoEvaluacion
    {

        public BEDocumentoEvaluacion()
		{

		}

        public BEDocumento DOCUMENTO { get; set; }
        public BEEvaluacionItem EVALUACION { get; set; }
        public string CODIGO { get; set; }
        public string RUTA { get; set; }
        public string TIPO_CREACION { get; set; }
    }
}
