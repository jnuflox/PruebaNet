using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable]
    public class BEFormLead // PROY-140739 Formulario Leads
    {
        public Int64 SOALN_SOLIN_CODIGO { get; set; }
        public string SOALC_COD_LEAD { get; set; }
        public Int64 SOALN_PEDIN_CODIGO { get; set; }
        public string SOALV_COD_OFICINA { get; set; }
        public string SOALV_DES_OFICINA { get; set; }
        public DateTime SOALD_FECHA_CREACION { get; set; }
        public string SOALV_USU_CREACION { get; set; }
        public DateTime SOALD_FECHA_MODIFICACION { get; set; }
        public string SOALV_USU_MODIFICACION { get; set; }
    }
}