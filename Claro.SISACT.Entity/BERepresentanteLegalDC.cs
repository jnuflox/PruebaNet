using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BERepresentanteLegalDC
    {
        public string ws53_In_NumeroOperacionRepLeg { get; set; }
        public string ws53_Out_RepresentanteLegalTipoPersona { get; set; }
        public string ws53_Out_RepresentanteLegalTipoDocumento { get; set; }
        public string ws53_Out_RepresentanteLegalNumeroDocumento { get; set; }
        public string ws53_Out_RepresentanteLegalNombre { get; set; }
        public string ws53_Out_RepresentanteLegalCargo { get; set; }
        /*INI PROY-32438*/
        public string ws53_Out_RepresentanteLegalFechNomb { get; set; }
        public Int64 ws53_Out_RepresentanteLegalCantMesesNomb { get; set; }
        /*FIN PROY-32438*/
    }
}
