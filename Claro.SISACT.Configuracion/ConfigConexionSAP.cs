using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Claro.SISACT.Configuracion
{
    public class ConfigConexionSAP : ClaroConfiguracionBase
	{
        public ConfigConexionSAP(string aplicacion)
			: base(aplicacion) { }
		
		public override void LeerValores(Base.Configuration configuracion)
		{
			Parametros.BaseDatos = configuracion.LeerBaseDatos();
			Parametros.Usuario = configuracion.LeerUsuario();
			Parametros.Password = configuracion.LeerContrasena();
		}

        public static ConfigConexionSAP GetInstance(string aplicacion)
		{
            return new ConfigConexionSAP(aplicacion);
		}
    }
}
