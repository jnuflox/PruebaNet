//PROY-32439 MAS INI Clase nueva para NVOBRMS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Claro.SISACT.Entity
{
    #region ValidacionDeudaBRMS
    [Serializable] //PROY-140126
    public class ValidacionDeudaBRMS
    {
        public ValidacionDeudaBRMSrequest request { get; set; }
        public ValidacionDeudaBRMSresponse response { get; set; }
        public List<ValidacionDeudaBRMS> clientes { get; set; }
    }
    #endregion

    #region ValidacionDeudaBRMSrequest
    [Serializable] //PROY-140126
    public class ValidacionDeudaBRMSrequest
    {
        public String decisionID { get; set; }
        public Cliente cliente { get; set; }
        public LineaARenovar lineaARenovar { get; set; }
        public PuntoDeVenta puntoDeVenta { get; set; }
        public String sistemaEvaluacion { get; set; }
        public String tipoOperacion { get; set; }

        public enum tipoSiNo
        {
            SI = 1,
            NO = 0,
        }

        #region ValidacionDeudaBRMSrequest|Cliente
        [Serializable] //PROY-140126
        public class Cliente
        {
            public Int32 antiguedadDeuda { get; set; }
            public List<Bloqueo> bloqueos { get; set; }
            public Int32 cantidadDocumentosDeuda { get; set; }
            public String comportamientoPago { get; set; }
            public Disputa disputa { get; set; }
            public Documento documento { get; set; }
            public tipoSiNo flagBloqueos { get; set; }
            public tipoSiNo flagSuspensiones { get; set; }
            public Double montoDeuda { get; set; }
            public Double montoDeudaCastigada { get; set; }
            public Double montoDeudaVencida { get; set; }
            public Double montoTotalPago { get; set; }
            public Double promedioFacturadoSoles { get; set; }
            public String segmento { get; set; }
            public List<Suspension> suspensiones { get; set; }
            public Int32 tiempoPermanencia { get; set; }
            public List<String> tiposFraude { get; set; }
            [Serializable] //PROY-140126
            public class Bloqueo
            {
                public String tipo { get; set; }
                public String tipoLinea { get; set; }
            }
            [Serializable] //PROY-140126
            public class Disputa
            {
                public Int32 antiguedad { get; set; }
                public Int32 cantidad { get; set; }
                public Double monto { get; set; }
            }
            [Serializable] //PROY-140126
            public class Documento
            {
                public String numero { get; set; }
                public String tipo { get; set; }
            }
            [Serializable] //PROY-140126
            public class Suspension
            {
                public String tipo { get; set; }
                public String tipoLinea { get; set; }
            }

            #region [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil]
            public Int64 cantidadLineaCuotasPendientesAcc { get; set; }
            public Int64 cantidadLineaCuotasPendientesAccUltiVenta { get; set; }
            public Int64 cantidadMaximaCuotasPendientesAcc { get; set; }
            public Int64 cantidadMaximaCuotasPendientesAccUltiVenta { get; set; }
            public Double montoCuotasPendientesAcc { get; set; }
            public Double montoCuotasPendientesAccUltiVenta { get; set; }
            #endregion

        }
        #endregion

        #region ValidacionDeudaBRMSrequest|LineaARenovar
        [Serializable] //PROY-140126
        public class LineaARenovar
        {
            public Int32 antiguedad { get; set; }
            public DateTime fechaActivacion { get; set; }
        }
        #endregion

        #region ValidacionDeudaBRMSrequest|PuntoDeVenta
        [Serializable] //PROY-140126
        public class PuntoDeVenta
        {
            public String canal { get; set; }
            public String codigo { get; set; }
            public String departamento { get; set; }
            public String nombre { get; set; }
            public String region { get; set; }
            public String segmento { get; set; }
            public Vendedor vendedor { get; set; }

            [Serializable] //PROY-140126 
            public class Vendedor
            {
                public String codigo { get; set; }
                public String nombre { get; set; }
            }
        }
        #endregion
    }
    #endregion

    #region ValidacionDeudaBRMSresponse
   [Serializable] //PROY-140126
    public class ValidacionDeudaBRMSresponse
    {
        public String decisionID { get; set; }
        public tipoSiNo validacionCliente { get; set; }
        public String mensajeValidacionCliente { get; set; }
        public List<String> restriccionTipoOperacion { get; set; }
        public List<String> restriccionProductoComercial { get; set; }
        public List<String> restriccionProducto { get; set; }

        public enum tipoSiNo
        {
            SI = 1,
            NO = 0,
        }
    }
    #endregion  
}
//PROY-32439 MAS FIN Clase nueva para NVOBRMS