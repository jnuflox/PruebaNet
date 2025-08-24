using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEItemMensaje
    {
        public string id { get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public string mensajeSistema { get; set; }
        public string mensajeCliente { get; set; }
        public string cadenaValoresOut { get; set; }//PROY-26963 - IDEA-34399: Mejora Portabilidad Despacho PDV
        public bool exito;

        public BEItemMensaje() 
        {
            this.exito = true;        
        }

        public BEItemMensaje(bool exito)
        {
            this.exito = exito;
        }

        public BEItemMensaje(string codigo, string mensajeSistema, bool exito)
        {
            this.codigo = codigo;
            this.mensajeSistema = mensajeSistema;
            this.exito = exito;
        }
    }
}
