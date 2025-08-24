using System;
using System.Collections.Generic;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEAcuerdoServicio
    {
        public BEAcuerdoServicio() { }
        public int IdDetalle { get; set; }
        public string Servicio { get; set; }
        public string Servicio_des { get; set; }
        public string Orden { get; set; }
        public Int64 IdContrato { get; set; }
        public double CargoFijo { get; set; }
    }
}
