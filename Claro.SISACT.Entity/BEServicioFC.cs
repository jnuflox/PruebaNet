using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140380 - FULLCLARO
    public class BEServicioFC
    {
        public string CodigoFC {get; set; }

        public string MensajeFC {get; set; }

        public List<BEDatosClienteFC> objDatosClienteFC {get; set; }

    }
}
