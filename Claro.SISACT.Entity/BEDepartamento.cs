using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEDepartamento
    {
        private string _DEPAC_CODIGO;
        private string _DEPAV_DESCRIPCION;
        private string _DEPAV_COD_CIU;
        private string _DEPAC_ESTADO;
        //Pedro Marcos
        private string _DEPAV_COBERTURA;
        //Pedro Marcos
        public BEDepartamento() { }

        public BEDepartamento(string vDEPAC_CODIGO, string vDEPAV_DESCRIPCION, string vDEPAV_COD_CIU)
        {
            _DEPAC_CODIGO = vDEPAC_CODIGO;
            _DEPAV_DESCRIPCION = vDEPAV_DESCRIPCION;
            _DEPAV_COD_CIU = vDEPAV_COD_CIU;
        }



        public string DEPAC_CODIGO
        {
            set { _DEPAC_CODIGO = value; }
            get { return _DEPAC_CODIGO; }
        }
        public string DEPAV_DESCRIPCION
        {
            set { _DEPAV_DESCRIPCION = value; }
            get { return _DEPAV_DESCRIPCION; }
        }
        public string DEPAV_COD_CIU
        {
            set { _DEPAV_COD_CIU = value; }
            get { return _DEPAV_COD_CIU; }
        }
        public string DEPAC_ESTADO
        {
            set { _DEPAC_ESTADO = value; }
            get { return _DEPAC_ESTADO; }
        }
        public string DEPAV_COBERTURA
        {
            set { _DEPAV_COBERTURA = value; }
            get { return _DEPAV_COBERTURA; }
        }
    }
}
