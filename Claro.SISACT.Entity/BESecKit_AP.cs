using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BESecKit_AP : BEKit_AP
    {
        private int _PLSVN_CODIGO;
        private int _SOLIN_CODIGO;
        private int _SOPLN_CODIGO;
        private double _KITN_PRECIO_KIT;
        private double _KITN_PRECIO_BASE;
        private double _KITN_COSTO_INST;

        public int PLSVN_CODIGO { get { return _PLSVN_CODIGO; } set { _PLSVN_CODIGO = value; } }
        public int SOLIN_CODIGO { get { return _SOLIN_CODIGO; } set { _SOLIN_CODIGO = value; } }
        public int SOPLN_CODIGO { get { return _SOPLN_CODIGO; } set { _SOPLN_CODIGO = value; } }
        public double CARGO_FIJO_EN_SEC { get { return _KITN_PRECIO_KIT; } set { _KITN_PRECIO_KIT = value; } }
        public double KITN_PRECIO_BASE { get { return _KITN_PRECIO_BASE; } set { _KITN_PRECIO_BASE = value; } }
        public double KITN_COSTO_INST { get { return _KITN_COSTO_INST; } set { _KITN_COSTO_INST = value; } }

        public BESecKit_AP()
        {
            new BEKit_AP();
            PLSVN_CODIGO = 0;
            SOLIN_CODIGO = 0;
            SOPLN_CODIGO = 0;
            CARGO_FIJO_EN_SEC = 0;
        }
    }
}
