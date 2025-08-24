using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEServicio
    {
        public string SERVV_CODIGO { get; set; }
        public string SERVV_DESCRIPCION { get; set; }
        public string SERVC_ESTADO { get; set; }
        public string GSRVC_CODIGO { get; set; }
        public int SERVN_ORDEN { get; set; }
        public string SELECCIONABLE_BASE { get; set; }
        public string SELECCIONABLE_EN_PLAN { get; set; }
        public string SELECCIONABLE_EN_PAQUETE { get; set; }
        public double CARGO_FIJO_BASE { get; set; }
        public double DESCUENTO_EN_PLAN { get; set; }
        public double CARGO_FIJO_EN_PAQUETE { get; set; }
        public DateTime SERVD_FECHA_CREA { get; set; }
        public string SERVV_USUARIO_CREA { get; set; }
        public string TSERVC_CODIGO { get; set; }

        public BEPlan oPLAN { get; set; }

		public BEServicio()
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
			TSERVC_CODIGO = string.Empty;
            oPLAN = new BEPlan();
		}

        public enum TIPO_TOPE_CONSUMO
        {
            TOPE_CONSUMO_ABIERTO = 1, //PROY-29296
            TOPE_CONSUMO_EXACTO = 2, //PROY-29296
            TOPE_CONSUMO_ADICIONAL = 3 //PROY-29296
        }

		public int GetSeleccionable()
		{
            return Convert.ToInt32(SELECCIONABLE_BASE) | Convert.ToInt32(SELECCIONABLE_EN_PLAN) | Convert.ToInt32(SELECCIONABLE_EN_PAQUETE);
		}

	    public enum SERVICIOS_SELECCIONABLE
	    {
		    OBLIGATORIO = 1, SELECCIONADO = 2, OPCIONAL = 0
	    }

       //PROY-26963-F3  - GPRD
        public string PRDC_CODIGO { get; set; }
        public string PLANC_CODIGO { get; set; }
        public double SERVN_PRECIO_SERV { get; set; }
        public int SOPLN_ORDEN { get; set; }
        //PROY-26963-F3  - GPRD
	}
}

