using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEOfertaBRMS: IComparable
    {
        public BEOfertaBRMS() { }

        public int idFila { get; set; }
        public string idProducto { get; set; }
        public string producto { get; set; }
        public string idPlazo { get; set; }
        public string plazo { get; set; }
        public string idPaquete { get; set; }
        public string paquete { get; set; }
        public string idPlan { get; set; }
        public string plan { get; set; }
        public string idCampana { get; set; }
        public string campana { get; set; }
        public double cargoFijo { get; set; }
        public string topeConsumo { get; set; }
        public int cantidadMesesOperadorCedente { get; set; }//PROY-26963 - IDEA-34399: Mejora Portabilidad Despacho PDV
        //gaa20170215
        public int cantidadLineasSEC { get; set; }
        public double montoCFSEC { get; set; }
        //fin gaa20170215
        public List<BEEquipoBRMS> oEquipo { get; set; }
        public List<BEItemGenerico> oServicio { get; set; }
        public List<BEBilletera> oBilletera { get; set; }

        public class sortByOrdenHelper : IComparer
        {
            int IComparer.Compare(object x, object y)
            {
                BEOfertaBRMS obj1 = (BEOfertaBRMS)x;
                BEOfertaBRMS obj2 = (BEOfertaBRMS)y;

                if (obj1.idFila > obj2.idFila)
                    return 1;
                if (obj1.idFila < obj2.idFila)
                    return -1;
                else
                    return 0;
            }
        }

        // Implement IComparable CompareTo to provide default sort order.
        int IComparable.CompareTo(object obj)
        {
            BEOfertaBRMS x = (BEOfertaBRMS)obj;

            if (this.idFila > x.idFila)
                return 1;
            if (this.idFila < x.idFila)
                return -1;
            else
                return 0;
        }

        // Method to return IComparer object for sort helper.
        public static IComparer sortByOrden()
        {
            return (IComparer)new sortByOrdenHelper();
        }
        //PROY-140335 INI RF1
        public String flagConsultaPrevia { get; set; } 
        public String numeroLinea { get; set; } 
        //PROY-140335 FIN RF1
    }
}
