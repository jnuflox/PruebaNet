using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEPuntoVenta
    {
        public BEPuntoVenta() { }

		public BEPuntoVenta(string vCodigo,string vDescripcion)
		{			
			OvencCodigo = vCodigo;
			OvenvDescripcion = vDescripcion;			
		}

		public BEPuntoVenta(string vCodigo,string vDescripcion,string vTipo,string vCanal)
		{			
			OvencCodigo = vCodigo;
			OvenvDescripcion = vDescripcion;	
			ToficCodigo = vTipo;
			CanacCodigo = vCanal;
		}

        public string OvencCodigo { get; set; }
        public string TprocCodigo { get; set; }
        public string CanacCodigo { get; set; }
        public string OvenvDescripcion { get; set; }
        public string ToficCodigo { get; set; }
        public string OvencEstado { get; set; }
        public string TofivDescripcion { get; set; }
        public string TprovDescripcion { get; set; }
        public string CanavDescripcion { get; set; }
        public string TcescCodigo { get; set; }
        public string EstadoRestriccion { get; set; }
        public string OvencRegion { get; set; }
        public string OvenvCentro { get; set; }
        public string OvenvNroCliePadre { get; set; }
        public string OvenvNroClieHijo { get; set; }
        public string OvenvAlmacen { get; set; }
        public string DepacCodigo { get; set; }
        public string DepavDescripcion { get; set; }
        //PROY-32439 MAS INI Clase modificada con campos region
        public string RegionCodigo { get; set; }
        public string RegionDescripcion { get; set; }
        //PROY-32439 MAS FIN Clase modificada con campos region
        public string CanalSAP { get; set; }
        public string OrgVentaSAP { get; set; }
        public string CentroSAP { get; set; }
    }
}
