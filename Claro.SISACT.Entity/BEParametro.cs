using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEParametro
    {

        private Int64 _codigo;
		private string _descripcion;		
		private string _valor;		
		private string _valor1;		
		private string _estado;
		private string _grupo;
		private string _grupodes;
		private string _estadodes;
		private string _flagSistema;

        public BEParametro()
		{
			//
			// TODO: Add constructor logic here
			//
		}		
		public Int64 Codigo
		{
			set{_codigo= value;}
			get{ return _codigo;}
		}
		public string Descripcion
		{
			set{_descripcion= value;}
			get{ return _descripcion;}
		}
        public string Valor
		{
            set { _valor = value; }
            get { return _valor; }
		}
		public string Valor1
		{
			set{_valor1= value;}
			get{ return _valor1;}
		}
		public string estado
		{
			set{_estado= value;}
			get{ return _estado;}
		}

		public string Grupo
		{
			set{_grupo= value;}
			get{ return _grupo;}
		}
		public string GrupoDes
		{
			set{_grupodes= value;}
			get{ return _grupodes;}
		}
		
		public string EstadoDes
		{
			set{_estadodes= value;}
			get{ return _estadodes;}
		}
		public string flagSistema
		{
			set{_flagSistema= value;}
			get{ return _flagSistema;}
		}

    }

}
