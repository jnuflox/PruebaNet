using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    //INI PROY-31948_Migracion
    [Serializable] //PROY-32439 Serializable
public class BETipoDocumentoE
    {

        public int CODIGO { get; set; }
        public string NOMBRE { get; set; }
        public int ORDEN { get; set; }
        public int Total_Docs_Adjunta { get; set; }
        public BERegistro REGISTRO { get; set; }

        public BETipoDocumentoE()
		{
		}

        public BETipoDocumentoE(int pCodigo,string pNombre,int pOrden, int pTotalDocsAdjunta)
		{
            CODIGO = pCodigo;
            NOMBRE = pNombre;
            ORDEN = pOrden;
            Total_Docs_Adjunta = pTotalDocsAdjunta;
		}
    }
    //FIN PROY-31948_Migracion
}
