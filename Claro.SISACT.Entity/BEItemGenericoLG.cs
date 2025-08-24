//INI PROY-CAMPANA LG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEItemGenericoLG
    {
        public string Codigo { get; set; }
        public string GamaAlta { get; set; }
        public string GamaBaja { get; set; }
        public string Umbral { get; set; }

        public BEItemGenericoLG()
        { 
        }
        
        public BEItemGenericoLG(string vCodigo, string vGamaAlta, string vGamaBaja, string vUmbral)
        {
            Codigo = vCodigo;
            GamaAlta = vGamaAlta;
            GamaBaja = GamaBaja;
            Umbral = vUmbral;
        }
    
    }
}//FIN PROY-CAMPANA LG
