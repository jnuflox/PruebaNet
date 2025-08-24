using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    //Clase creada para el PROY-29121
    public class BEEmpresaCategoria
    {
        public BEEmpresaCategoria()
        { }
        public string FLAG_TIPO { get; set; }
        public int CATEGORIA_ID { get; set; }
        public string FLAG_TOLERANCIA { get; set; }
        public double TOLERANCIA { get; set; }
        public string CATEGORIA_DES { get; set; }
        public Int64 EMPRESA_ID { get; set; }
        public double TOLERANCIA_BLOQUEO { get; set; }
        public int CATEN_DIAS_VENCIDO { get; set; }
        public string TIPO_DOC { get; set; }
        public string NUM_DOC { get; set; }
        public string RAZON_SOCIAL { get; set; }
        public string EMPRESA_ESTADO { get; set; }
        public Int64 EMPRESA_CAT_ID { get; set; }
        public string LOGIN { get; set; }
        public DateTime FEG_REG { get; set; }
        public string TERMINAL { get; set; }
        public string ESTADO { get; set; }
        public string TIPO_DOC_DES { get; set; }
        public string TOLERANCIA_DES { get; set; }
        public string SEGMENTO_COD { get; set; }
        public double CATEN_MONTO_VENCIDO { get; set; }

        public int CODIGO { get; set; }
        public string DESCRIPCION { get; set; }
        public DateTime FECHA_REGISTRO { get; set; }
    }
}
