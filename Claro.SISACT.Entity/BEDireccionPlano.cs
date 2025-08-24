using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEDireccionPlano
    {
        public BEDireccionPlano()
        {
        }

        private string _IdPlano;
        public string IdPlano
        {
            get { return _IdPlano; }
            set { _IdPlano = value; }
        }

        private string _Descripcion;
        public string Descripcion
        {
            get { return _Descripcion; }
            set { _Descripcion = value; }
        }

        private string _Distrito_Desc;
        public string Distrito_Desc
        {
            get { return _Distrito_Desc; }
            set { _Distrito_Desc = value; }
        }

        private string _Centro_Poblado;
        public string Centro_Poblado
        {
            get { return _Centro_Poblado; }
            set { _Centro_Poblado = value; }
        }
        //gaa20130522
        private string _FlagVOD;
        public string FlagVOD
        {
            get { return _FlagVOD; }
            set { _FlagVOD = value; }
        }
        //fin gaa20130522
    }
}
