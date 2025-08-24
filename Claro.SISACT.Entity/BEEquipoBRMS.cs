using System.Configuration;
using System; //PROY-140126 - IDEA 140248 

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEEquipoBRMS
    {
        public string idFila { get; set; }
        public string idProducto { get; set; }
        public string idEquipo { get; set; }
        public double costo { get; set; }
        public double factorDePagoInicial { get; set; }
        public double factorDeSubsidio { get; set; }
        public string kit { get; set; }
        public string gama { get; set; }
        public string modelo { get; set; }
        public double montoDeCuota { get; set; }
        public double precioDeVenta { get; set; }
        public string grupo { get; set; }
        public string tipoDeOperacionKit { get; set; }
        public string formaDePago { get; set; }
        public int cantidadDeCuotas { get; set; }
        public double porcentajeCuotaInicial { get; set; }
        public string tipoDeDeco { get; set; }
        public string riesgo { get; set; }
        public double montoDeCuotaInicialComercial { get; set; } //PROY-30166-INICIO
        public double montoDeCuotaComercial { get; set; } //PROY-30166-FIN

        public BEEquipoBRMS()
        {
            this.tipoDeOperacionKit = "";
            this.formaDePago = "";
            this.tipoDeDeco = "";
            this.gama = ConfigurationManager.AppSettings["constGamaEquipoBajo"].ToString();
            this.formaDePago = ConfigurationManager.AppSettings["constFormaPagoContado"].ToString();
        }
    }
}
