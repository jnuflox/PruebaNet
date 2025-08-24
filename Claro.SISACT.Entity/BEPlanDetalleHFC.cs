using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEPlanDetalleHFC
    {
        public Int64 IDDET { get; set; }
        public Int64 IdProducto { get; set; }
        public Int64 IdLinea { get; set; }
        public string Producto { get; set; }
        public int Grupo { get; set; }
        public string IdServicio { get; set; }
        public string Servicio { get; set; }
        public string IdEquipo { get; set; }
        public string Equipo { get; set; }
        public double Precio { get; set; }
        public double CF_Linea { get; set; }
        public string FlagPrincipal { get; set; }
        public int Cantidad { get; set; }
        public int Orden { get; set; }
        public string Agrupa { get; set; }
        public string Tipo { get; set; }
        public string Promocion { get; set; }

        public Int64 IdSolucion { get; set; }
        public Int64 IdPaquete { get; set; }
        public Int64 IdPromocion { get; set; }
        public string Solucion { get; set; }
        public string Paquete { get; set; }

        public string Campana { get; set; }
        public string Plan { get; set; }
        public string GrupoDescripcion { get; set; }
//gaa20140801
        public string IdTope { get; set; }
        public int Id_MontoTope { get; set; } //PROY-29296
//fin gaa20140801
    }
}
