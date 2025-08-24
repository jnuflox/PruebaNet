using System;
using System.Configuration;
using Claro.SISACT.Configuracion;
using Claro.SISACT.IData;
using System.Diagnostics;

namespace Claro.SISACT.Data
{
    public class BDCLARIFY : BDConexion
    {

        public BDCLARIFY(string aplicacion)
            : base(aplicacion) { }

        protected override IClaroBDConfiguracion Configuracion
        {
            get
            {
                return Conexion.GeneraConfiguracion<ConfigConexionCLARIFY>(this.Aplicacion);
            }
        }
    }
}