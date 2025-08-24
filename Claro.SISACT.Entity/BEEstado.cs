using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEEstado
    {
        private Int64 _Estado_ID;
        private Int64 _NroSEC;
        private Int64 _NroSOT;
        private string _ESTAC_CODIGO;
        private string _ESTAV_DESCRIPCION;
        private string _HISEV_USUA_REG;
        private DateTime _HISED_FEC_REG;
        private string _HISEV_COMENTARIO;
        private string _HISEV_FLAG_ARCHIVO;

        public BEEstado()
        {
        }

        public Int64 Estado_ID
        {
            get { return _Estado_ID; }
            set { _Estado_ID = value; }
        }
        public Int64 NroSEC
        {
            get { return _NroSEC; }
            set { _NroSEC = value; }
        }
        public Int64 NroSOT
        {
            get { return _NroSOT; }
            set { _NroSOT = value; }
        }
        public string ESTAC_CODIGO
        {
            get { return _ESTAC_CODIGO; }
            set { _ESTAC_CODIGO = value; }
        }
        public string ESTAV_DESCRIPCION
        {
            get { return _ESTAV_DESCRIPCION; }
            set { _ESTAV_DESCRIPCION = value; }
        }
        public string HISEV_USUA_REG
        {
            get { return _HISEV_USUA_REG; }
            set { _HISEV_USUA_REG = value; }
        }
        public DateTime HISED_FEC_REG
        {
            get { return _HISED_FEC_REG; }
            set { _HISED_FEC_REG = value; }
        }
        public string HISEV_COMENTARIO
        {
            get { return _HISEV_COMENTARIO; }
            set { _HISEV_COMENTARIO = value; }
        }
        public string HISEV_FLAG_ARCHIVO
        {
            get { return _HISEV_FLAG_ARCHIVO; }
            set { _HISEV_FLAG_ARCHIVO = value; }
        }
    }
}
