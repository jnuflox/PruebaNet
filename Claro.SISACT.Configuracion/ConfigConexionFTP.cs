using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Configuracion
{
    public class ConfigConexionFTP : ClaroConfiguracionBase
    {
        public ConfigConexionFTP(string aplicacion) 
            : base(aplicacion) { }

        public override void LeerValores(Base.Configuration configuracion)
        {
            Parametros.BaseDatos = configuracion.LeerBaseDatos() ;
            Parametros.Usuario = configuracion.LeerUsuario();
            Parametros.Password = configuracion.LeerContrasena();         
            Parametros.Servidor = "";
            Parametros.Provider = "";
        }

        public static ConfigConexionFTP GetInstance(string aplicacion)
        {
            return new ConfigConexionFTP(aplicacion);
        }
    }
}
