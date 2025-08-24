using System; //PROY-140126 - IDEA 140248 
namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    
    public class BEItemGenerico
    {
        public string Codigo { get; set; }
        public string Codigo2 { get; set; }
        public string Codigo3 { get; set; }
        public string Codigo4 { get; set; }
        public string Codigo5 { get; set; }
        public string Descripcion { get; set; }
        public string Descripcion2 { get; set; }
        public string Estado { get; set; }
        public string Tipo { get; set; }
        public double Monto { get; set; }
        public string Valor { get; set; }
        public int Cantidad { get; set; }
        public string Valor1 { get; set; }//PROY-29121 
        public double CargoFijo { get; set; }//INICIATIVA-219

        public BEItemGenerico()
        {

        }

        public BEItemGenerico(string vCodigo, string vDescripcion)
        {
            Codigo = vCodigo;
            Descripcion = vDescripcion;
            Codigo2 = "";
        }  

        public BEItemGenerico(string vCodigo, string vDescripcion, double vMonto)
        {
            Codigo = vCodigo;
            Descripcion = vDescripcion;
            Codigo2 = string.Empty;
            Monto = vMonto;
        }
    }
}
