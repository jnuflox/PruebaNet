//DIL:INI::20170910
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEValidarLineaAuditoria
    {
        public String strIdTransaccion { get; set; }
        public String strIpAplicacion { get; set; }
        public String strNombreAplicacion { get; set; }
        public String strUsuarioAplicacion { get; set; }
    }

    public class BEValidarLineaDetalle
    {
        public String strLinea { get; set; }
        public String strSegmento { get; set; }
    }

    public class BEValidarLinea
    {
        public int intCantidadLineasActivas { get; set; }
        public List<BEValidarLineaDetalle> lstConsolidadoLineas { get; set; }
        //[PROY-140600] INI 
        public string strLineasActivas { get; set; }
        public Int64 solin_codigo { get; set; }
        public string tdoc_cliente { get; set; }
        public string nroDoc_cliente { get; set; }
        public string usuario { get; set; }
        public string tipoVenta { get; set; }
        public int nLineasNuevas { get; set; }
        public string CuentaUsuario { get; set; }
        public string NombreUsuario { get; set; }
        //[PROY-140600] FIN
    }

    //INI: PROY-140335
    public class BEValidalineaPorta
    {
        public string StrLinea { get; set; }
        public string StrOperadorCedente { get; set; }
        public string StrTipoDoc { get; set; }
        public string StrTipoProducto { get; set; }
        public string StrModalidadVenta { get; set; }
        public string StrModalidadPorta { get; set; }
        public string StrNroDocumento { get; set; }
        public int SolinCodigo { get; set; }
        public string CodigoRespuesta { get; set; }
        public string MsjRespuesta { get; set; }
        public string EstadoAnulacion { get; set; }
        public string strCodigoSinergia { get; set; }
    }
    //FIN: PROY-140335

}
//DIL:FIN::20170910
