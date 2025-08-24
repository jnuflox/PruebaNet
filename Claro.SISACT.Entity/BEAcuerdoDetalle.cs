using System;
using System.Collections.Generic;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEAcuerdoDetalle
    {
        public BEAcuerdoDetalle() { }
        public Int64 IdContrato { get; set; }
        public string Material { get; set; }
        public string Material_des { get; set; }
        public string Campana { get; set; }
        public string Campana_desc { get; set; }
        public string Plan_tarifar { get; set; }
        public string Plan_tarifar_desc { get; set; }
        public double Precio_lista { get; set; }
        public double Precio_venta { get; set; }
        public string Telefono { get; set; }
        public Int64 Co_id { get; set; }
        public int IdDetalle { get; set; }
        public string Imei19 { get; set; }
        public string Utilizacion { get; set; }
        public string Des_utilizacion { get; set; }
        public double Descuento { get; set; }
        public double Impuesto { get; set; }
        public string Cod_equipo { get; set; }
        public string Des_equipo { get; set; }
        public string Serie_equipo { get; set; }
        public string Principal { get; set; }
        public string Prdc_codigo { get; set; }
        public string Pacuc_codigo { get; set; }
        public string Pacuv_descripcion { get; set; }
        public int NroRecibo { get; set; }
        public Int64 Sub_contrato { get; set; }
        public Int64 Solin_codigo { get; set; }
        public string DocumentoSap { get; set; }
        public Int64 NroSot { get; set; }
    }
}
