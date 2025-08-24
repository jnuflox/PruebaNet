using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEPlanBilletera
    {
        public string plan { get; set; }
        public string cuenta { get; set; }
        public int idBilletera { get; set; }
        public int nroPlanes { get; set; }
        public double CF { get; set; }
        public double montoFacturado { get; set; }
        public double montoNoFacturado { get; set; }
        public TIPO_PLAN tipoPlan { get; set; }
        public TIPO_FACTURADOR tipoFacturador { get; set; }
        public List<BEBilletera> oBilletera { get; set; }

        //PROY-24740
        public bool ContainsAny(List<BEBilletera> objLista)
        {
            int existe = objLista.Join(this.oBilletera, a => a.idBilletera, b => b.idBilletera, (a, b) => new { objLista = a, oBilletera = b }).Count();
            return existe > 0;
        }
        public enum TIPO_PLAN
        {
            MOVIL = 1,
            DATOS = 2
        }
        public enum TIPO_FACTURADOR
        {
            BSCS = 0,
            SGA = 1
        }
        public enum TIPO_SISTEMA
        {
            BSCS = 0,
            SGA = 1,
            SISACT = 2
        }
        //PROY-26963-F3 - GPRD - PROMFACT
        public string nroSEC { get; set; } //NroSEC
        //PROY-26963-F3 - GPRD - PROMFACT
    }
}
