using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BECentroPoblado
    {
        public BECentroPoblado()
        {
            //
            // TODO: agregar aquí la lógica del constructor
            //
        }

        private Int64 _IDPOBLADO;
        private string _CODCLASIFICACION;
        private string _NOMBRE;
        private string _COBERTURA;
        private string _IDUBIGEO;
        private string _COBERTURA_LTE;

        public Int64 IDPOBLADO
        {
            set { _IDPOBLADO = value; }
            get { return _IDPOBLADO; }
        }

        public string CODCLASIFICACION
        {
            set { _CODCLASIFICACION = value; }
            get { return _CODCLASIFICACION; }
        }

        public string NOMBRE
        {
            set { _NOMBRE = value; }
            get { return _NOMBRE; }
        }
        public string COBERTURA
        {
            set { _COBERTURA = value; }
            get { return _COBERTURA; }
        }
        public string IDUBIGEO
        {
            set { _IDUBIGEO = value; }
            get { return _IDUBIGEO; }
        }
        public string COBERTURA_LTE
        {
            set { _COBERTURA_LTE = value; }
            get { return _COBERTURA_LTE; }
        }
    }
}
