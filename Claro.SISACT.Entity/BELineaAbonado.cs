using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BELineaAbonado
    {
        public string nro_telefono { get; set; }
        public string nro_documento { get; set; }
        public string tipo_documento { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string plan_tarifario { get; set; }
        public string segmento { get; set; }
        public string create_date { get; set; }
        public string estado { get; set; }
        public string fecha_activacion { get; set; }
    }
}
