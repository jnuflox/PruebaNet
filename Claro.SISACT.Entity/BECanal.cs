using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BECanal
    {

       
	public BECanal()
	{			
	}

        public BECanal(string vCodigo, string vDescripcion)
	{			
            CANAC_CODIGO = vCodigo;
            CANAV_DESCRIPCION = vDescripcion;
	}

        public string CANAC_CODIGO { get; set; }
        public string TPROC_CODIGO { get; set; }
        public string CANAV_DESCRIPCION { get; set; }
        public string CANAC_FLA_EVA { get; set; }
        public Int64 CANAN_NUM_PLA { get; set; }
        public Int64 CANAN_CAR_FIJ_MAX { get; set; }
        public string CANAC_ESTADO { get; set; }
    }
}
