using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
namespace Claro.SISACT.Configuracion
{
    public class ConfigConexionSISACT : ClaroConfiguracionBase
    {

        public ConfigConexionSISACT(string aplicacion)
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