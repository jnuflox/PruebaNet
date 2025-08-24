using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Configuracion
{
    public class ConfigConexionDC: ClaroConfiguracionBase
    {
        public ConfigConexionDC(string aplicacion)
            : base(aplicacion) { }

        public override void LeerValores(Base.Configuration configuracion)
        {         
            Parametros.Usuario = configuracion.LeerUsuario();
            Parametros.Password = configuracion.LeerContrasena();       
        }

        public static ConfigConexionDC GetInstance(string aplicacion)
        {
            return new ConfigConexionDC(aplicacion);
        }
    }
}
