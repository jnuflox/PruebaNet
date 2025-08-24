using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEKit_AP
    {
        private int _KITV_CODIGO;
        private string _KITV_DESCRIPCION;
        private string _TKITC_CODIGO;
        private string _KITC_ESTADO;
        private DateTime _KITD_FECHA_CREA;
        private string _KITV_USUARIO_CREA;
        private double _KITN_PRECIO_BASE;
        private string _KITV_SELECCIONABLE;

        private BEPlan_AP _PLAN;

        public int KITV_CODIGO { get { return _KITV_CODIGO; } set { _KITV_CODIGO = value; } }
        public string KITV_DESCRIPCION { get { return _KITV_DESCRIPCION; } set { _KITV_DESCRIPCION = value; } }
        public string TKITC_CODIGO { get { return _TKITC_CODIGO; } set { _TKITC_CODIGO = value; } }
        public string KITC_ESTADO { get { return _KITC_ESTADO; } set { _KITC_ESTADO = value; } }
        public DateTime KITD_FECHA_CREA { get { return _KITD_FECHA_CREA; } set { _KITD_FECHA_CREA = value; } }
        public string KITV_USUARIO_CREA { get { return _KITV_USUARIO_CREA; } set { _KITV_USUARIO_CREA = value; } }
        public double CARGO_FIJO_BASE { get { return _KITN_PRECIO_BASE; } set { _KITN_PRECIO_BASE = value; } }
        public string SELECCIONABLE_EN_PLAN { get { return _KITV_SELECCIONABLE; } set { _KITV_SELECCIONABLE = value; } }


        public BEPlan_AP PLAN { get { return _PLAN; } set { _PLAN = value; } }
        public BEKit_AP()
        {
            _KITV_CODIGO = 0;
            _KITV_DESCRIPCION = string.Empty;
            _TKITC_CODIGO = string.Empty;
            _KITC_ESTADO = string.Empty;
            _KITD_FECHA_CREA = new DateTime();
            _KITV_USUARIO_CREA = string.Empty;
            _KITN_PRECIO_BASE = 0;
            _KITV_SELECCIONABLE = "0";

            PLAN = new BEPlan_AP();
        }
    }
}
