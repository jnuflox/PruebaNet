using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEServicioHFC
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
        public double CF_Precio { get; set; }
        public double CF_Linea { get; set; }
        public string FlagPrincipal { get; set; }
        public string FlagOpcional { get; set; }
        public string FlagDefecto { get; set; }
        public int CantVenta { get; set; }
        public string FlagVOD { get; set; }
//gaa20140414
        public string GrupoDescripcion { get; set; }
//fin gaa20140414
    }
}
