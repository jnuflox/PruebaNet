using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    /// <summary>
    /// Descripción breve de PlanDetalleVenta.
    /// </summary>
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEPlanDetalleVenta
    {
        public Int64 PLVTN_CODIGO { get; set; }
        public Int64 SOLIN_CODIGO { get; set; }
        public Int64 SOPLN_CODIGO { get; set; }
        public Int64 SOPLN_SECUENCIA { get; set; }
        public int SOPLN_ORDEN { get; set; }

        public string PAQTV_CODIGO { get; set; }
        public string PAQTV_DESCRIPCION { get; set; }
        public string PLANC_CODIGO { get; set; }
        public string PLANV_DESCRIPCION { get; set; }
        public string TPROC_CODIGO { get; set; }
        public string TPROV_DESCRIPCION { get; set; }

        public int SOPLN_CANTIDAD { get; set; }

        public double CARGO_FIJO { get; set; }
        public double SOPLC_MONTO_TOTAL { get; set; }

        public double TOPE_MONTO { get; set; }

        public string TELEFONO { get; set; }
        public string MATERIAL { get; set; }
        public string MATERIAL_DESC { get; set; }
        public string PACUC_CODIGO { get; set; }
        public string PACUV_DESCRIPCION { get; set; }
        public string CAMPANA { get; set; }
        public string CAMPANA_DESC { get; set; }
        public string LISTA_PRECIO { get; set; }
        public string LISTA_PRECIO_DESC { get; set; }
        public double PRECIO_LISTA { get; set; }
        public double PRECIO_VENTA { get; set; }

        public string CUOTA_DESCRIPCION { get; set; }
        public string CUOTA_CODIGO { get; set; }
        public double CUOTA_INICIAL { get; set; }
        public string SOPLV_PAQU_AGRU { get; set; }
        public string SUBSIDIO { get; set; }
        public int PLNN_TIPO_PLAN { get; set; }
        public double CARGO_FIJO_LIN { get; set; }
        public double SOLIN_COSTO_INST_DTH { get; set; }

        public string CASO_ESPECIAL { get; set; }
        public string OFERTA { get; set; }
        public string TIPO_PRODUCTO { get; set; }
        public string FLAG_PORTABILIDAD { get; set; }
        public string RIESGO { get; set; }
        public string TIPO_OFICINA { get; set; }
        public string OFICINA { get; set; }
        public string TOPE_CODIGO { get; set; }
        public string TOPE_DESCRIPCION { get; set; }
        public string TOPEN_CODIGO { get; set; }	//TIPO OPERACION
        public string GRUPO_PLAN { get; set; }
        public string PRDC_CODIGO { get; set; }		//TABLA SISACT_AP_PRODUCTO
        public string PRDV_DESCRIPCION { get; set; }

        public List<BESecServicio_AP> SERVICIO { get; set; }
        public BEPlanSolicitudDTH PLAN_SOL_DTH { get; set; }
        public BEPlanSolicitudHFC PLAN_SOL_HFC { get; set; }

        public Int64 ID_SOLUCION { get; set; }
        public string SOLUCION { get; set; }
        public Int64 IDDET { get; set; }
        public Int64 IDPRODUCTO { get; set; }
        public Int64 IDLINEA { get; set; }

        public double SOLIN_CF_ALQUILER_KIT { get; set; }
        public double SOLIN_COSTO_INST_EVAL_DTH { get; set; }

        public string RIESGO_TOTAL_EQUIPO { get; set; }
        public string CAPACIDAD_PAGO { get; set; }
        public string EXONERACION_RENTAS { get; set; }

        public Single SUBTOTAL { get; set; }
        public string FLAG_TITULARIDAD { get; set; }
        public string TOPE_CONSUMO { get; set; }
        public string TOPE_CONSUMO_DESC { get; set; }

        public string IDCOMBO { get; set; }
        public string COMBO { get; set; }
        public string USUARIO { get; set; }

        public string MODALIDAD_VENTA { get; set; }
//gaa20161024
        public string FAMILIA_PLAN { get; set; }
//fin gaa20161024

        //PROY-29215 - INICIO
        public string FORMA_PAGO { get; set; }
        public string CUOTA_PAGO { get; set; }
        //PROY-29215 - FIN

        public BEPlanDetalleVenta()
        {
            this.CUOTA_CODIGO = "00";
            this.CUOTA_INICIAL = 100;
        }

        //PROY-26963-F3 - GPRD
        public string CODIGO_SAP { get; set; }
        public string PORT_NUMERO { get; set; }
        public string PORT_MODALIDAD { get; set; }
        //PROY-26963-F3 - GPRD

        //PROY-140736 ini
        public string strServidor { get; set; }
        //PROY-140736 fin

        public string S_CARGO_FIJO { get; set; }
        public string CODIGO_BSCS { get; set; }
    }
}
