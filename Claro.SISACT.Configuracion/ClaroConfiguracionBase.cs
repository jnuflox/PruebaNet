using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Configuracion
{
    public abstract class ClaroConfiguracionBase : IConfiguracionBase
    {

        private ClaroBDConfiguration _objParametros;

        public ClaroConfiguracionBase(string aplicacion)
        {
            try
            {
                DatoProduccion(aplicacion);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DatoProduccion(string aplicacion)
        {
            if (_objParametros == null)
            {
                
                _objParametros = new ClaroBDConfiguration();
            }

            Base.Configuration cfgConexion = new Base.Configuration(aplicacion);

            LeerValores(cfgConexion);
        }

        public abstract void LeerValores(Base.Configuration configuracion);

     
            public ClaroBDConfiguration Parametros
        {
            get
            {
              
                return _objParametros;
            }
        }
    }
}
