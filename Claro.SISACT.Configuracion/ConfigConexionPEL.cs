using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Claro.SISACT.Common;

namespace Claro.SISACT.Configuracion
{
    public class ConfigConexionPEL : ClaroConfiguracionBase
    {

        public ConfigConexionPEL(string aplicacion)
            : base(aplicacion) { }

        public override void LeerValores(Base.Configuration configuracion)
        {
            Parametros.BaseDatos = configuracion.LeerBaseDatos();
            Parametros.Usuario = configuracion.LeerUsuario();
            Parametros.Password = configuracion.LeerContrasena();
            Parametros.Servidor = "";
            Parametros.Provider = "";

        }

    }
}