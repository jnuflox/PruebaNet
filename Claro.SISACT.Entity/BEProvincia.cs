using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEProvincia
    {
		private string _PROVC_CODIGO;
		private string _DEPAC_CODIGO;
		private string _PROVV_DESCRIPCION;
		private string _PROVC_ESTADO;
		//Pedro MArcos
		private string _PROVV_COBERTURA;
		//fin Pedro MArcos
		public BEProvincia(string vPROVC_CODIGO,string vPROVV_DESCRIPCION,string vDEPAC_CODIGO)
		{
			_PROVC_CODIGO=vPROVC_CODIGO;
			_DEPAC_CODIGO=vDEPAC_CODIGO;
			_PROVV_DESCRIPCION=vPROVV_DESCRIPCION;
		}
		//Alan Mateo
		public BEProvincia()
		{
		
		}
		public string PROVC_CODIGO{
			set{ _PROVC_CODIGO = value;}
			get{ return _PROVC_CODIGO;}
		}
		public string DEPAC_CODIGO
		{
			set{ _DEPAC_CODIGO= value;}
			get{ return _DEPAC_CODIGO;}
		}
		public string PROVV_DESCRIPCION
		{
			set{ _PROVV_DESCRIPCION= value;}
			get{ return _PROVV_DESCRIPCION;}
		}
		public string PROVC_ESTADO
		{
			set{ _PROVC_ESTADO= value;}
			get{ return _PROVC_ESTADO;}
		}
		//Pedro MArcos
		public string PROVV_COBERTURA
		{
			set{ _PROVV_COBERTURA= value;}
			get{ return _PROVV_COBERTURA;}
		}
    }
}
