using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEConsultarPrecioBase
    {
        public string CodigoMaterial { get; set; } //Codigo Material
        public string DescripcionMaterial { get; set; } //Descripcion Material
        public decimal PrecioBase { get; set; } //Precio Base
        public decimal PrecioCompra { get; set; } // Precio Compra
    }
}
