using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEEquipo
    {
        private Int64 _EQUIPO_ID;
		private string  _TIPO_MATERIAL_ID;
		private string  _TIPO_MATERIAL_DES;
		private string  _GRUPO_MATERIAL;
		private string  _PLAN_ID;
		private string  _PLAN_DES;
		private int _CANTIDAD;
		private int _EXTENSION;
		private string  _PLAZO_ID;
		private string  _PLAZO_DES;
		private int  _NRO_CUOTAS;
		private double  _PRECIO_LISTA;
		private double  _DESCUENTO;
		private double  _PRECIO_VENTA;
		private double  _PRECIO_VENTA_ORIGINAL;
		private double  _PRECIO_TOTAL;
		private string _ESTADO;
		private int _LISTA_ID;
		//modificado por WSotomayor
		private DateTime _FECHA_INICIO;
		private DateTime _FECHA_FIN;
		private string _PAQUETE_ID;
		private string _PAQUETE_DES;
		private string _TERMINAL;
		private string _TIPO_LISTA;
		private string _ESTADO_DES;

		//<CAA>
		private string _NRO_SERIE;
		private string _NRO_TELEFONO;
		private string _NRO_SIMCARD;
		private string _EQUIPO_INFO;
		private string _TELEFONO_INFO;
		private string _FLAG_MOSTRAR;

		private int  _CUOTA_ID;
		private string  _CUOTA_DES;
		//</CAA>
        public BEEquipo()
        {
        }

		public BEEquipo(string vTIPO_MATERIAL_ID,string vTIPO_MATERIAL_DES)
		{
			_TIPO_MATERIAL_ID=vTIPO_MATERIAL_ID;
			_TIPO_MATERIAL_DES=vTIPO_MATERIAL_DES;
		}
		public Int64  EQUIPO_ID
		{
			set{_EQUIPO_ID= value;}
			get{ return _EQUIPO_ID;}
		}
		public string  TIPO_MATERIAL_ID
		{
			set{_TIPO_MATERIAL_ID= value;}
			get{ return _TIPO_MATERIAL_ID;}
		}	
		public string  TIPO_MATERIAL_DES
		{
			set{_TIPO_MATERIAL_DES= value;}
			get{ return _TIPO_MATERIAL_DES;}
		}
		public string  GRUPO_MATERIAL
		{
			set{_GRUPO_MATERIAL = value;}
			get{ return _GRUPO_MATERIAL;}
		}
		public string PLAN_ID
		{
			set{_PLAN_ID= value;}
			get{ return _PLAN_ID;}
		}
		public string PLAN_DES
		{
			set{_PLAN_DES= value;}
			get{ return _PLAN_DES;}
		}
		public int CANTIDAD
		{
			set{_CANTIDAD= value;}
			get{ return _CANTIDAD;}
		}
		public int EXTENSION
		{
			set{_EXTENSION= value;}
			get{ return _EXTENSION;}
		}
		public string PLAZO_ID
		{
			set{_PLAZO_ID= value;}
			get{ return _PLAZO_ID;}
		}
		public string PLAZO_DES
		{
			set{_PLAZO_DES= value;}
			get{ return _PLAZO_DES;}
		}
		public int NRO_CUOTAS
		{
			set{_NRO_CUOTAS= value;}
			get{ return _NRO_CUOTAS;}
		}
		public double PRECIO_LISTA
		{
			set{_PRECIO_LISTA= value;}
			get{ return _PRECIO_LISTA;}
		}
		public double DESCUENTO
		{
			set{_DESCUENTO= value;}
			get{ return _DESCUENTO;}
		}
		public double PRECIO_VENTA
		{
			set{_PRECIO_VENTA= value;}
			get{ return _PRECIO_VENTA;}
		}
		public double PRECIO_VENTA_ORIGINAL
		{
			set{_PRECIO_VENTA_ORIGINAL= value;}
			get{ return _PRECIO_VENTA_ORIGINAL;}
		}
		public double PRECIO_TOTAL
		{
			set{_PRECIO_TOTAL= value;}
			get{ return _PRECIO_TOTAL;}
		}
		public string ESTADO
		{
			set{_ESTADO= value;}
			get{ return _ESTADO;}
		}

		public int LISTA_ID
		{
			set{_LISTA_ID= value;}
			get{ return _LISTA_ID;}
		}
		//modificado por WSotomayor
		public DateTime FECHA_INICIO
		{
			set{_FECHA_INICIO= value;}
			get{ return _FECHA_INICIO;}
		}
		public DateTime FECHA_FIN
		{
			set{_FECHA_FIN= value;}
			get{ return _FECHA_FIN;}
		}
		public string PAQUETE_ID
		{
			set{_PAQUETE_ID= value;}
			get{ return _PAQUETE_ID;}
		}
		public string PAQUETE_DES
		{
			set{_PAQUETE_DES= value;}
			get{ return _PAQUETE_DES;}
		}
		public string TERMINAL
		{
			set{_TERMINAL= value;}
			get{ return _TERMINAL;}
		}
		public string TIPO_LISTA
		{
			set{_TIPO_LISTA= value;}
			get{ return _TIPO_LISTA;}
		}
		public string ESTADO_DES
		{
			set{_ESTADO_DES= value;}
			get{ return _ESTADO_DES;}
		}
		//<CAA>
		public string NRO_SERIE
		{
			set{_NRO_SERIE= value;}
			get{ return _NRO_SERIE;}
		}
		
		public string NRO_TELEFONO
		{
			set{_NRO_TELEFONO = value;}
			get{ return _NRO_TELEFONO;}
		}		
		
		public string NRO_SIMCARD
		{
			set{_NRO_SIMCARD= value;}
			get{ return _NRO_SIMCARD;}
		}

		public string EQUIPO_INFO
		{
			set{_EQUIPO_INFO= value;}
			get{ return _EQUIPO_INFO;}
		}
		public string TELEFONO_INFO
		{
			set{_TELEFONO_INFO = value;}
			get{ return _TELEFONO_INFO;}
		}		

		public string FLAG_MOSTRAR
		{
			set{_FLAG_MOSTRAR= value;}
			get{ return _FLAG_MOSTRAR;}
		}
		//</CAA>
		
		public int CUOTA_ID
		{
			set{_CUOTA_ID = value;}
			get{ return _CUOTA_ID;}
		}		

		public string CUOTA_DES
		{
			set{_CUOTA_DES= value;}
			get{ return _CUOTA_DES;}
		}
    }
}
