using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Configuracion
{
    public interface IConfiguracionBase
    {

        ClaroBDConfiguration Parametros
        {
            get;
        }
    }
}
