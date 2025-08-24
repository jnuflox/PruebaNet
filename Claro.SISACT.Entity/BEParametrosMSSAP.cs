using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    #region [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] | [ Clase generica para obtener las campañas de ventas varias]
    [Serializable] 
    public class BEParametrosMSSAP
    {
        public string CodigoOficina { get; set; }
        public string TipoOficina { get; set; }
        public string Centro { get; set; }
        public string Almacen { get; set; }
        public string UsuarioRed { get; set; }
        public string CodigoUsuario { get; set; }
        public string EstadoPedido { get; set; }
        public string MotivoPedido { get; set; }
        public Int64 NumeroPedido { get; set; }
        public string TipoVenta { get; set; }
        public string CodigoGenerico { get; set; }
        public string DescripcionGenerico { get; set; }
        public string EstadoGenerico { get; set; }
        public string SerieMaterial { get; set; }
        public string SerieMaterial2 { get; set; } //Serie IMEI para lista de Precios
        public string CodigoMaterial { get; set; }
        public string CodigoMaterial2 { get; set; } //Material IMEI para Lista de Precios
        public string Telefono { get; set; }
        public string CodigoProducto { get; set; }
        public string CodigoDepartamento { get; set; }
        public string CodigoCampania { get; set; }
        public string TipoOperacion { get; set; }
        public string PlazoAcuerdo { get; set; }
        public string Plan { get; set; }
        public string TipoDocCliente { get; set; }
        public string NroDocCliente { get; set; }
        public Int64 TopMateriales { get; set; }
        public Int64 TopSeries { get; set; }
        public string FlagServicio { get; set; }
        public string idSolPlan { get; set; }
        public object strTipoDocCliente { get; set; }
        public object strNroDocCliente { get; set; }
        public string tipoMaterial { get; set; }
        public string DescripcionMaterial { get; set; }
        public string strFlagProcesaPLC { get; set; }
        public string strMaterialClasificacion { get; set; }
    }
    #endregion
}
