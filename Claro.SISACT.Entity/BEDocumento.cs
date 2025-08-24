using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    //INI PROY-31948_Migracion
    [Serializable] //PROY-32439 Serializable
    public class BEDocumento
    {

        public string NOMBRE_CLIENTE { get; set; }
        public double IMPORTE { get; set; }
        public double SALDO { get; set; }
        public string TIPO_DOCUMENTO_DES { get; set; }
        public string FACTURA_SUNAT { get; set; }
        public string FECHA { get; set; }
        public string DOCUMENTO_SAP { get; set; }
        public string UTILIZACION_DES { get; set; }
        public string UTILIZACION { get; set; }
        public string CUOTAS { get; set; }
        public string MONEDA { get; set; }
        public double NETO { get; set; }
        public double IMPUESTO { get; set; }
        public double PAGOS { get; set; }
        public string NRO_DEP_GARANTIA { get; set; }
        public string NRO_REF_DEP_GAR { get; set; }
        public string CLASE_FACTURA { get; set; }
        public string NRO_CONTRATO { get; set; }
        public string NRO_SEC { get; set; }
        public int CODIGO { get; set; }
        public string NOMBRE { get; set; }
        public BETipoDocumentoE TIPO_DOCUMENTO { get; set; }
        public BERegistro REGISTRO { get; set; }

        public BEDocumento(int pCodigo, string pNombre, BETipoDocumentoE pTiposDocumento)
        {
            CODIGO = pCodigo;
            NOMBRE = pNombre;
            TIPO_DOCUMENTO = pTiposDocumento;
        }

        public BEDocumento()
        {

        }
        //FIN PROY-31948_Migracion
    }
}
