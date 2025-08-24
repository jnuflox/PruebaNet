using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable]
    public class BEDetalleLinea_CBIO
    {
        //CABECERA
        public string C_CUSTOMER_ID { get; set; }
        public int PLANES { get; set; }
        public double CF { get; set; }
        public int BLOQ { get; set; }
        public int SUSP { get; set; }
        public string DEUV { get; set; }
        public string DEUC { get; set; }
        public string RAZON_SOCIAL { get; set; }
        public string NOMBRES { get; set; }
        public string APELLIDOS { get; set; }
        public string DIAS_DEUDA { get; set; }
        public string C_PROM_FACT { get; set; }
        public string C_MONTO_NO_FACT { get; set; }
        public int NRO_7 { get; set; }
        public int NRO_30 { get; set; }
        public int NRO_90 { get; set; }
        public int NRO_90_MAS { get; set; }
        public int NRO_180 { get; set; }
        public int NRO_180_MAS { get; set; }
        public List<BEDetalleLinea_CBIO_D> objListaDetalle { get; set; }
        //
    }

    public class BEDetalleLinea_CBIO_D
    {
        //DETALLE
        public string D_CUSTOMER_ID { get; set; }
        public string CUENTA { get; set; }
        public string CO_ID { get; set; }
        public string TM_CODE { get; set; }
        public string PLAN { get; set; }
        public string PRODUCTO_SERVICIO { get; set; }
        public string NUMERO { get; set; }
        public double CF_CONTRATO { get; set; }
        public double D_PROM_FACT { get; set; }
        public string FECHA_ACTIVACION { get; set; }
        public string ESTADO { get; set; }
        public string COD_BLOQ { get; set; }
        public string COD_SUSP { get; set; }
        public string MOT_BLOQ { get; set; }
        public string MOT_SUSP { get; set; }
        public string FECHA_ESTADO { get; set; }
        public string MONTO_VENCIDO { get; set; }
        public string MONTO_CASTIGADO { get; set; }
        public string CAMPANA { get; set; }
        public double D_MONTO_NO_FACT { get; set; }
        public int NRO_MOVIL { get; set; }
        public int NRO_INTERNET_FIJO { get; set; }
        public int NRO_CLARO_TV { get; set; }
        public int NRO_TELEF_FIJA { get; set; }
        public int NRO_BAM { get; set; }
        public int NRO_BLOQ { get; set; }
        public int NRO_SUSP { get; set; }
        public string DEUDA_REINT_EQUIPO { get; set; }
        public int NRO_PLANES { get; set; }
        public string CICLO_FACTURACION { get; set; }
        //
    }
}
