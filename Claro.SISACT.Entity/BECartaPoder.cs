using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;


namespace Claro.SISACT.Entity
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BECartaPoder
    {
        public Int64 codSec_Solicitud { get; set; }
        public Int64 nro_Pedido { get; set; }
        public string idTransaccion { get; set; }
        public string tipotransaccion { get; set; }
        public string tipoOperacion { get; set; }
        public string descripcionoperacion { get; set; }
        public string tipodocumento { get; set; }
        public string numDocumento { get; set; }
        public string nomApoderado { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidomaterno { get; set; }
        public string comentario { get; set; }
        public string aplicacion { get; set; }
        public string usuariocrea { get; set; }
        public string usuariomodifica { get; set; }



        public BECartaPoder()
        {

        }



    }

}
