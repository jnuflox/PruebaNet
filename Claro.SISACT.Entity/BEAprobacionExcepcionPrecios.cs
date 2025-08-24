using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity
{
    [DataContract]
    [Serializable] 
    public class BEAprobacionExcepcionPrecios
    {

        public string SOLIN_CODIGO { get; set; }
        public string PEDIDO_TV { get; set; }
        public string PEDIDO_SINERGIA { get; set; }
        public string PRECIO_TV { get; set; }
        public string PRECIO_SISACT { get; set; }
        public string COD_OFICINA { get; set; }
        public string USUARIO_REGISTRO { get; set; }
        public string NODO_REGISTRO { get; set; }
        public string ESTADO { get; set; }
        public string ESTADO_POS { get; set; }
        public string ID_FLUJO { get; set; }
        public string cuotaInicialTienda { get; set; }
        public string cuotaInicialSisact { get; set; }

    }

}
