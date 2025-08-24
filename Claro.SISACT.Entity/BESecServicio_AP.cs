using System;

namespace Claro.SISACT.Entity
{
    /// <summary>
    /// Descripción breve de SecServicio_AP.
    /// </summary>
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BESecServicio_AP : BEServicio_AP
    {
        private int _PLSVN_CODIGO;
        private int _SOLIN_CODIGO;
        private int _SOPLN_CODIGO;
        private double _SERVN_PRECIO_SERV;
        private Int64 _IDDET;
        private Int64 _IDPRODUCTO;
        private Int64 _IDLINEA;        
        private string _SERVC_PLAZO;
        private DateTime _SERVD_FECHA_ACTIVACION;
        private DateTime _SERVD_FECHA_DESACTIVACION;

        public int PLSVN_CODIGO { get { return _PLSVN_CODIGO; } set { _PLSVN_CODIGO = value; } }
        public int SOLIN_CODIGO { get { return _SOLIN_CODIGO; } set { _SOLIN_CODIGO = value; } }
        public int SOPLN_CODIGO { get { return _SOPLN_CODIGO; } set { _SOPLN_CODIGO = value; } }
        public double CARGO_FIJO_EN_SEC { get { return _SERVN_PRECIO_SERV; } set { _SERVN_PRECIO_SERV = value; } }

        public Int64 IDDET
        {
            get { return _IDDET; }
            set { _IDDET = value; }
        }
        public Int64 IDPRODUCTO
        {
            get { return _IDPRODUCTO; }
            set { _IDPRODUCTO = value; }
        }
        public Int64 IDLINEA
        {
            get { return _IDLINEA; }
            set { _IDLINEA = value; }
        }

        private Int64 _SOPLN_ORDEN;
        public Int64 SOPLN_ORDEN
        {
            set { _SOPLN_ORDEN = value; }
            get { return _SOPLN_ORDEN; }
        }

        public string SERVC_PLAZO
        {
            set { _SERVC_PLAZO = value; }
            get { return _SERVC_PLAZO; }
        }
        public DateTime SERVD_FECHA_ACTIVACION
        {
            set { _SERVD_FECHA_ACTIVACION = value; }
            get { return _SERVD_FECHA_ACTIVACION; }
        }
        public DateTime SERVD_FECHA_DESACTIVACION
        {
            set { _SERVD_FECHA_DESACTIVACION = value; }
            get { return _SERVD_FECHA_DESACTIVACION; }
        }

        public BESecServicio_AP()
        {
            new BEServicio_AP();
            PLSVN_CODIGO = 0;
            SOLIN_CODIGO = 0;
            SOPLN_CODIGO = 0;
            CARGO_FIJO_EN_SEC = 0;
        }

        public enum TIPO_SELECCION
        {
            INHABILITADO = 0,
            HABILITADO = 1,
            SELECCIONADO = 2,
            SELECCIONADO_EDITABLE = 3
        }


        //PROY-24740
        public int PAQPN_SECUENCIA { get; set; }
        public int PLNV_CODIGO { get; set; }
    }
}
