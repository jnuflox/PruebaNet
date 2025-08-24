using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEPaquete_AP
    {
        private string _PAQTV_CODIGO;
        private string _PAQTV_DESCRIPCION;
        private string _PAQTC_ESTADO;
        private string _TPAQTV_CODIGO;
        private string _TPROC_CODIGO;
        private DateTime _PAQTD_FECHA_CREA;
        private string _PAQTV_USUARIO_CREA;

        private ArrayList _PLANES;

        public string PAQTV_CODIGO { get { return _PAQTV_CODIGO; } set { _PAQTV_CODIGO = value; } }
        public string PAQTV_DESCRIPCION { get { return _PAQTV_DESCRIPCION; } set { _PAQTV_DESCRIPCION = value; } }
        public string PAQTC_ESTADO { get { return _PAQTC_ESTADO; } set { _PAQTC_ESTADO = value; } }
        public string TPAQTV_CODIGO { get { return _TPAQTV_CODIGO; } set { _TPAQTV_CODIGO = value; } }
        public string TPROC_CODIGO { get { return _TPROC_CODIGO; } set { _TPROC_CODIGO = value; } }
        public DateTime PAQTD_FECHA_CREA { get { return _PAQTD_FECHA_CREA; } set { _PAQTD_FECHA_CREA = value; } }
        public string PAQTV_USUARIO_CREA { get { return _PAQTV_USUARIO_CREA; } set { _PAQTV_USUARIO_CREA = value; } }

        public ArrayList PLANES { get { return _PLANES; } set { _PLANES = value; } }

        public BEPaquete_AP()
        {
            PAQTV_CODIGO = string.Empty;
            PAQTV_DESCRIPCION = string.Empty;
            PAQTC_ESTADO = string.Empty;
            PAQTD_FECHA_CREA = new DateTime();
            PAQTV_USUARIO_CREA = string.Empty;
            TPAQTV_CODIGO = string.Empty;
            TPROC_CODIGO = string.Empty;

            PLANES = new ArrayList();
        }
    }
}
