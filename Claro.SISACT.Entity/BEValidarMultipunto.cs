using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable]
    public class BEValidarMultipunto
    {

        public List<BEItemGenerico> Canales { get; set; }
        public List<BEMultipunto> Datos { get; set; }
        public List<BEPuntoVenta> Oficinas { get; set; }

    }
}  
