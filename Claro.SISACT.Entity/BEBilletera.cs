using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEBilletera
    {
        public BEBilletera() { }

        public int idBilletera { get; set; }
        public string billetera { get; set; }
        public int nroPlanes { get; set; }
        public double monto { get; set; }

        public BEBilletera(int idBilletera, string billetera) 
        {
            this.idBilletera = idBilletera;
            this.billetera = billetera;
        }
        public BEBilletera(int idBilletera, int nroPlanes)
        {
            this.idBilletera = idBilletera;
            this.nroPlanes = nroPlanes;
        }
        public enum TIPO_BILLETERA
        {
            MOVIL = 2,
            INTERNET = 4,
            CLAROTV = 8,
            TELEFONIA = 16,
            BAM = 32,
            TRIPLEPLAY = 28
        }
    }
}