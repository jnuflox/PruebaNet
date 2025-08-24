using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
   public class BEConsultarMaterialXCampania
    {
        public string CodigoMaterial { get; set; } //Codigo Material
        public string DescripcionMaterial { get; set; } //Descripcion Material
        public string CodigoCentro { get; set; } //Codigo Centro
        public string CodigoAlmacen { get; set; } //Codigo Almacen
        public string CodigoOficina { get; set; } //Codigo Oficina
        public string DescripcionOficina { get; set; } //Descripcion Oficina
        public string TipoMaterial { get; set; } //Tipo de Material
    }
}
