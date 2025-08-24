using System;

namespace Claro.SISACT.Entity
{
	/// <summary>
	/// Descripción breve de Servicio_AP.
	/// </summary>
        [Serializable] //PROY-140126 - IDEA 140248 
	public class BEServicio_AP
	{
		/*
		private string _PAQPN_SECUENCIA;
		*/
		private string _SERVV_CODIGO;
		private string _SERVV_DESCRIPCION;
		private string _SERVC_ESTADO;
		private string _GSRVC_CODIGO;
		private int _SERVN_ORDEN;
		private string _SERVV_SELECCIONABLE;
		private string _PSRVV_SELECCIONABLE;
		private string _PAQSV_SELECCIONABLE;
		private double _SERVN_PRECIO_BASE;
		private double _PSRVN_DESCUENTO;
		private double _PAQSN_CARGO_FIJO;
		private DateTime _SERVD_FECHA_CREA;
		private string _SERVV_USUARIO_CREA;
                private int _SERVN_ID_MONTO_TOPE; //PROY-29296

		private BEPlan_AP _PLAN;
		//JAR
		private string _TSERVC_CODIGO;

        private string _GSRVV_DESCRIPCION; //PROY-31812 - IDEA-43340
		/*
		public string PAQPN_SECUENCIA {get{ return _PAQPN_SECUENCIA;} set{_PAQPN_SECUENCIA = value;}}
		*/
		public string SERVV_CODIGO {get{ return _SERVV_CODIGO;} set{_SERVV_CODIGO = value;}}
		public string SERVV_DESCRIPCION {get{ return _SERVV_DESCRIPCION;} set{_SERVV_DESCRIPCION = value;}}
		public string SERVC_ESTADO {get{ return _SERVC_ESTADO;} set{_SERVC_ESTADO = value;}}
		public string GSRVC_CODIGO {get{ return _GSRVC_CODIGO;} set{_GSRVC_CODIGO = value;}}
		public int SERVN_ORDEN {get{ return _SERVN_ORDEN;} set{_SERVN_ORDEN = value;}}
		public string SELECCIONABLE_BASE {get{ return _SERVV_SELECCIONABLE;} set{_SERVV_SELECCIONABLE = value;}}
		public string SELECCIONABLE_EN_PLAN {get{ return _PSRVV_SELECCIONABLE;} set{_PSRVV_SELECCIONABLE = value;}}
		public string SELECCIONABLE_EN_PAQUETE {get{ return _PAQSV_SELECCIONABLE;} set{_PAQSV_SELECCIONABLE = value;}}
		public double CARGO_FIJO_BASE {get{ return _SERVN_PRECIO_BASE;} set{_SERVN_PRECIO_BASE = value;}}
		public double DESCUENTO_EN_PLAN {get{ return _PSRVN_DESCUENTO;} set{_PSRVN_DESCUENTO = value;}}
		public double CARGO_FIJO_EN_PAQUETE {get{ return _PAQSN_CARGO_FIJO;} set{_PAQSN_CARGO_FIJO = value;}}
		public DateTime SERVD_FECHA_CREA {get{ return _SERVD_FECHA_CREA;} set{_SERVD_FECHA_CREA = value;}}
		public string SERVV_USUARIO_CREA {get{ return _SERVV_USUARIO_CREA;} set{_SERVV_USUARIO_CREA = value;}}
                public int SERVN_ID_MONTO_TOPE { get { return _SERVN_ID_MONTO_TOPE; } set { _SERVN_ID_MONTO_TOPE = value; } } //PROY-29296

                public string GSRVV_DESCRIPCION { get { return _GSRVV_DESCRIPCION; } set {_GSRVV_DESCRIPCION = value;}} //PROY-31812 - IDEA-43340     
		public BEPlan_AP PLAN {get{ return _PLAN;} set{_PLAN = value;}}
		//JAR
		public string TSERVC_CODIGO {get{ return _TSERVC_CODIGO;} set{_TSERVC_CODIGO = value;}}

		public BEServicio_AP()
		{
			SERVV_CODIGO = string.Empty;
			SERVV_DESCRIPCION = string.Empty;
			SERVC_ESTADO = string.Empty;
			GSRVC_CODIGO = string.Empty;
			SERVN_ORDEN = 0;
			SELECCIONABLE_BASE = string.Empty;
			SELECCIONABLE_EN_PAQUETE = string.Empty;
			CARGO_FIJO_BASE = 0;
			DESCUENTO_EN_PLAN = 0;
			CARGO_FIJO_EN_PAQUETE = 0;

			SERVD_FECHA_CREA = new DateTime();
			SERVV_USUARIO_CREA = string.Empty;

            GSRVV_DESCRIPCION = string.Empty; //PROY-31812 - IDEA-43340
            
			PLAN = new BEPlan_AP();
			//JAR
			TSERVC_CODIGO=string.Empty;

		}

		public int GetSeleccionable()
		{
			return Convert.ToInt32(SELECCIONABLE_BASE) | Convert.ToInt32(SELECCIONABLE_EN_PLAN) | Convert.ToInt32(SELECCIONABLE_EN_PAQUETE);
		}
	}

	public enum SERVICIOS_SELECCIONABLE
	{
		OBLIGATORIO = 1, SELECCIONADO = 2, OPCIONAL = 0
	}
}
