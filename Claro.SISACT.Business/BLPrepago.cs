using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Data;
using System.Collections;
using Claro.SISACT.Entity;

namespace Claro.SISACT.Business
{
    public class BLPrepago
    {
        public List<BELineaAbonado> ListarLineasAbonado(string tipoDocumento, string nroDocumento)
        {
            return new DAPrepago().ListarLineasAbonado(tipoDocumento, nroDocumento);
        }

        public string ValidarBloqueoLinea(string nroDocumento, string telefeno)
        {
            return new DAPrepago().ValidarBloqueoLinea(nroDocumento, telefeno);
        }
    }
}
