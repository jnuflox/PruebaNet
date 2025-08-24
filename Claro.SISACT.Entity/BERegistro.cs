using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    //INI PROY-31948_Migracion
    [Serializable] //PROY-32439 Serializable
public class BERegistro
    {
        public DateTime FECHA_CREACION { get; set; }
        public DateTime FECHA_MODIFICACION { get; set; }
        public string USUARIO_CREACION { get; set; }
        public string USUARIO_MODIFICACION { get; set; }
        public string ACTIVO { get; set; }
        public int ORDEN { get; set; }

            public BERegistro()
            {
                //
                // TODO: agregar aquí la lógica del constructor
                //
            }

            }

    //FIN PROY-31948_Migracion
}
