using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEDistrito
    {
        private string _DISTC_CODIGO;
        private string _DEPAC_CODIGO;
        private string _PROVC_CODIGO;
        private string _DISTC_CODIGO_POSTAL;
        private string _DISTV_DESCRIPCION;
        private string _DISTC_ESTADO;
        private string _ALMACEN;
        //Pedro Marcos
        private string _DISTV_COBERTURA;
        private string _DISTC_ZONA_ALEJADA;
        private string _DISTC_ALMACEN;
        //Fin Pedro Marcos
        //E76009 Inicio
        private string _UBIGEO_INEI;
        //E76009 Fin

        public BEDistrito()
        {

        }
        //Alan Mateo
        public BEDistrito(string vDISTC_CODIGO, string vDEPAC_CODIGO, string vPROVC_CODIGO, string vDISTV_DESCRIPCION)
        {
            _DISTC_CODIGO = vDISTC_CODIGO;
            _DEPAC_CODIGO = vDEPAC_CODIGO;
            _PROVC_CODIGO = vPROVC_CODIGO;
            _DISTV_DESCRIPCION = vDISTV_DESCRIPCION;
        }
        //
        public string DISTC_CODIGO
        {
            set { _DISTC_CODIGO = value; }
            get { return _DISTC_CODIGO; }
        }
        public string DEPAC_CODIGO
        {
            set { _DEPAC_CODIGO = value; }
            get { return _DEPAC_CODIGO; }
        }
        public string PROVC_CODIGO
        {
            set { _PROVC_CODIGO = value; }
            get { return _PROVC_CODIGO; }
        }
        public string DISTC_CODIGO_POSTAL
        {
            set { _DISTC_CODIGO_POSTAL = value; }
            get { return _DISTC_CODIGO_POSTAL; }
        }
        public string DISTV_DESCRIPCION
        {
            set { _DISTV_DESCRIPCION = value; }
            get { return _DISTV_DESCRIPCION; }
        }
        public string DISTC_ESTADO
        {
            set { _DISTC_ESTADO = value; }
            get { return _DISTC_ESTADO; }
        }
        //Pedro Marcos
        public string DISTV_COBERTURA
        {
            set { _DISTV_COBERTURA = value; }
            get { return _DISTV_COBERTURA; }
        }
        public string DISTC_ZONA_ALEJADA
        {
            set { _DISTC_ZONA_ALEJADA = value; }
            get { return _DISTC_ZONA_ALEJADA; }
        }

        public string DISTC_ALMACEN
        {
            set { _DISTC_ALMACEN = value; }
            get { return _DISTC_ALMACEN; }
        }
        //Fin Pedro Marcos
        public string ALMACEN
        {
            set { _ALMACEN = value; }
            get { return _ALMACEN; }
        }
        //E76009 Inicio
        public string UBIGEO_INEI
        {
            set { _UBIGEO_INEI = value; }
            get { return _UBIGEO_INEI; }
        }
        //E76009 Fin
    }
}
