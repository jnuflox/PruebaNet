using System;
using System.Configuration;
using Claro.SISACT.Configuracion;
using Claro.SISACT.IData;

namespace Claro.SISACT.Data
{
    public class BDCLAROCLUB : BDConexion
    {

        public BDCLAROCLUB(string aplicacion)
            : base(aplicacion) { }

        protected override IClaroBDConfiguracion Configuracion
        {
            get
            {
                return Conexion.GeneraConfiguracion<ConfigConexionPUNTOSCC>(this.Aplicacion);
            }
        }
    }
}