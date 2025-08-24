using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEPlanDetalleConsumer
    {
        public BEPlanDetalleConsumer()
		{
			
		}


		private int _SOPLN_CODIGO;
		private string _SOPLN_TOPE_CONSUMO;
		private string _TOPE_DESCRIPCION;
		
		public string SOPLN_TOPE_CONSUMO
		{
			set{_SOPLN_TOPE_CONSUMO = value;}
			get{ return _SOPLN_TOPE_CONSUMO;}
		}
		public string TOPE_DESCRIPCION
		{
			set{_TOPE_DESCRIPCION = value;}
			get{ return _TOPE_DESCRIPCION;}
		}
		public int SOPLN_CODIGO
		{
			set{_SOPLN_CODIGO = value;}
			get{ return _SOPLN_CODIGO;}
		}


		private int _SOLIN_CODIGO;
		public int SOLIN_CODIGO
		{
			set{_SOLIN_CODIGO = value;}
			get{ return _SOLIN_CODIGO;}
		}


		private Double _SOPLC_MONTO_TOTAL;
		public Double SOPLC_MONTO_TOTAL
		{
			set{_SOPLC_MONTO_TOTAL = value;}
			get{ return _SOPLC_MONTO_TOTAL;}
		}


		private Double _SOPLN_MONTO_UNIT;
		public Double SOPLN_MONTO_UNIT
		{
			set{_SOPLN_MONTO_UNIT = value;}
			get{ return _SOPLN_MONTO_UNIT;}
		}


		private string _PLANC_CODIGO;
		public string PLANC_CODIGO
		{
			set{_PLANC_CODIGO = value;}
			get{ return _PLANC_CODIGO;}
		}

		private string _PLANV_DESCRIPCION;
		public string PLANV_DESCRIPCION
		{
			set{_PLANV_DESCRIPCION = value;}
			get{ return _PLANV_DESCRIPCION;}
		}


		private string _TPROC_CODIGO;
		public string TPROC_CODIGO
		{
			set{_TPROC_CODIGO = value;}
			get{ return _TPROC_CODIGO;}
		}


		private int _SOPLN_CANTIDAD;
		public int SOPLN_CANTIDAD
		{
			set{_SOPLN_CANTIDAD = value;}
			get{ return _SOPLN_CANTIDAD;}
		}
		
		private string _SERVICIOS_DEFAULT ="";
		public string SERVICIOS_DEFAULT
		{
			set{_SERVICIOS_DEFAULT = value;}
			get{ return _SERVICIOS_DEFAULT;}
		}

		private string _PAQTV_CODIGO ="";
		public string PAQTV_CODIGO
		{
			set{_PAQTV_CODIGO = value;}
			get{ return _PAQTV_CODIGO;}
		}

		private string _PAQPN_SECUENCIA ="";
		public string PAQPN_SECUENCIA
		{
			set{_PAQPN_SECUENCIA = value;}
			get{ return _PAQPN_SECUENCIA;}
		}

                private int _SOPLN_SECUENCIA;
		public int SOPLN_SECUENCIA
		{
			set{_SOPLN_SECUENCIA = value;}
			get{ return _SOPLN_SECUENCIA;}
		}

		private int _SOPLN_ORDEN;
		public int SOPLN_ORDEN
		{
			set{_SOPLN_ORDEN = value;}
			get{ return _SOPLN_ORDEN;}
		}
		
		// Tope de Consumo
		private string _SERVC_TOPE = "";
		public string SERVC_TOPE
		{
			set{_SERVC_TOPE = value;}
			get{ return _SERVC_TOPE;}
		}

		private string _PLANC_SISACT;
		public string PLANC_SISACT
		{
			set{_PLANC_SISACT = value;}
			get{ return _PLANC_SISACT;}
		}


    }
}
