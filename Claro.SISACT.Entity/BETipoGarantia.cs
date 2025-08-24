using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BETipoGarantia
    {
        private string _TCARC_CODIGO;
        private string _TCARV_DESCRIPCION;
        private string _TCARC_ESTADO;

        public string TCARC_CODIGO
        {
            get { return this._TCARC_CODIGO; }
            set { this._TCARC_CODIGO = value; }
        }

        public string TCARV_DESCRIPCION
        {
            get { return this._TCARV_DESCRIPCION; }
            set { this._TCARV_DESCRIPCION = value; }
        }

        public string TCARC_ESTADO
        {
            get { return this._TCARC_ESTADO; }
            set { this._TCARC_ESTADO = value; }
        }
    }
}
